/*
 * Sketch to control the Foster System shutters. Note that this sketch ONLY controls the
 * shutter; I continue to use the normal Foster driver AstroMC to control dome rotation.
 * 
 * the sketch expects to receive commands from a program on the PC, sent through the Wireless LAN
 * to the Arduino CC3000 shield. When each command is received, the sketch sends a response as indicated below.
 * Many of the commands respond that they have begun operation; the system is then is a state where the operation is 
 * in progress (operation_in_progress == True). For example, to open the upper shutter the PC sends the 'U' command.
 * The operation starts, and the sketch sends back the "OK" response indicating the shutter movement has begun.
 * When the operation completes, operation_in_progress becomes false. A subsequent status command from the PC will show 
 * that the operation has been completed.
 *
 * Command protocol:
 *   Command from PC					Response
 *	OPEN_UPPER_SHUTTER 'U'				Success: "OK"			Failure: "Failed: Shutter already busy"
 *	CLOSE_UPPER_SHUTTER 'u'				Success: "OK"			Failure: "Failed: Shutter already busy"
 *	VENT_UPPER_SHUTTER 'V'				Success: "OK"			Failure: "Failed: Shutter already busy" 
 *																			"Vent failed: Upper shutter still closed" 
 *																			"Dome already fully opened"
 *	OPEN_LOWER_SHUTTER 'L'				Success: "OK"			Failure: "Failed: Shutter already busy"
 *	CLOSE_LOWER_SHUTTER 'l'				Success: "OK" 			Failure: "Failed: Shutter already busy"
 *	OPEN_BOTH_SHUTTER 'B'				Success: "OK" 			Failure: "Failed: Shutter already busy"
 *	CLOSE_BOTH_SHUTTER 'b'				Success: "OK" 			Failure: "Failed: Shutter already busy"
 *	GET_SHUTTER_STATUS 'S'				Success: "Status,uo,uc,lo,lc,uppCurrent,lowCurrent,volts,op" 			
 *								where uo (UpperOpenSw) = 'O' (Open) or 'C' (Closed)
 *								      uc (UpperClosedSw) = 'O' (Open) or 'C' (Closed)
 *								      lo (LowerOpenSw) = 'O' (Open) or 'C' (Closed)
 *								      lc (LowerClosedSw) = 'O' (Open) or 'C' (Closed)
 *									  uppCurrent is current being drawn by upper shutter motor %5.2f
 *									  lowCurrent is current being drawn by lower shutter motor %5.2f
 *									  volts is voltage from power supply %5.2f
 *									  op is whether operation is in progress ('T') or not ('F')
 *	IS_MOTOR_MOVING 'o'				Success: "Stopped" or "Moving"
 *	RECONFIG_PARAM 'C'				Success: "OK"
 *	ABORT_MOTORS 'A'				Success: "OK" 				
 *	other command:												Failed: "Failed: Unknown Command"
 * some white space "commands are silently ignored (cr, lf, etc)
 *
 * Status LED Patterns
 * Rapid Flashing	While motors are moving (operation is in progress)									
 * Solid on			While blocked on waiting for a connection from PC		
 * Slow Flashing	While checking for commands, no operation is in progress
 * Long/Quick/Quick	Network failed to initialize correctly
 *
 * Shield pins being used:
 *
 *   Overall
 *      pin 20 Status LED
 *
 *   cc3000
 *      pin 3  IRQ  must be IRQ line
 *      pin 5  VBAT      
 *      pin 10 CS
 *      pin 11 MOSI
 *      pin 12 MISO
 *      pin 13 SCK    NOTE - cannot used LED on 13!
 *      pin
 *
 *   Motor Shield
 *      pin 22 Upper Shutter Open Switch
 *      pin 23 Upper Shutter Closed Switch
 *      pin 24 Lower Shutter Open Switch
 *      pin 25 Lower Shutter Closed Switch
int inApin[2] = {7, 4};  // INA: Clockwise input
int inBpin[2] = {8, 9}; // INB: Counter-clockwise input
int pwmpin[2] = {5, 6}; // PWM input
int cspin[2] = {2, 3}; // CS: Current sense ANALOG input
int enpin[2] = {0, 1}; // EN: Status of switches output (Analog pin)
 *      pin 7,4 Clockwise input (7 is Motor1, 4 is Motor2)     Digital Output
 *      pin 8,9 CCW input                                Digital Output
 *      pin 5,6 PWM input    Digital output PWM pins, use Analog output 
 *      pin 2,3 CS Analog Input Current draw of motor   Analog pins
 *      pin 0,1 EN Status of switches output (Analog)
 *      pin
 *      pin 4    Analog Input for Voltage measurement
 *      pin
 *      pin
 *      pin
 *      pin
 *      pin
 */

/* NOTE - either all global variables are declared here, or I need to use
   a separate Variables.h header and include it in every file.
   */
/* 
  Version 1.1  Stop closing unused clients
               Add LED flash when command is received
          1.2  Removed LED flash
		       Changed pins for LEDs; turns out pin 48? is also used by control logic
	      1.3  Removed ability to configure motor speed; now motors ramp up from
		       511 to 1023 in steps of 64. Hopefully this allows lower shutter to close better
          1.4  Return to starting motor at full speed; ramping up speed did not help
               the reboot when starting lower motor
*/
#define VERSION "ShutterControl Version 1.4"

int mLoopDelay;          // track how long to delay at end of loop to avoid "working too hard"
#define LONG_LOOP_DELAY 500
#define SHORT_LOOP_DELAY 50

#define True 1
#define False 0

// Commands being received from host
#define OPEN_UPPER_SHUTTER 'U'
#define CLOSE_UPPER_SHUTTER 'u'
#define VENT_UPPER_SHUTTER 'V'
#define OPEN_LOWER_SHUTTER 'L'
#define CLOSE_LOWER_SHUTTER 'l'
#define OPEN_BOTH_SHUTTER 'B'
#define CLOSE_BOTH_SHUTTER 'b'
#define GET_SHUTTER_STATUS 'S'
#define IS_MOTOR_MOVING 'o'
#define ABORT_MOTORS 'A'
#define CONFIGURE_PARAM 'C'

// error return messages for complStatus
#define ERR_BUSY "Failed: Shutter already busy"
#define ERR_UNKNOWN_COMMAND "Failed: Unknown Command"
#define OPERATION_COMPLETED "OK"    // returned to host indicating operation complete

#define NOT_MOVING "Stopped"        // response to 'o' command
#define IS_MOVING "Moving"

int StatLED = 20;           // default built in LED

int active_operation = ' ';
boolean operation_in_progress = False;

#define BREWTRACE
#ifdef BREWTRACE
#define trace(s) Serial.println ((s))
#else
#define trace(s)
#endif

/*
 * setup() - this function runs once when you turn your Arduino on
 * We initialize the serial connection with the computer
 */
void setup()
{
  // initialize digital pin as output
  pinMode (StatLED, OUTPUT);

#ifdef BREWTRACE
      Serial.begin(115200);
#endif
  trace(VERSION);
  
  digitalWrite (StatLED, HIGH);
  if (!openNet()){  //Establish communications
      while(1){   // show error lights on StatLED
      	  digitalWrite(StatLED, HIGH);     // turn led on
	  delay(1000);
	  digitalWrite(StatLED, LOW);     // turn led off
	  delay(100);
	  digitalWrite(StatLED, HIGH);     // turn led on
	  delay(100);
	  digitalWrite(StatLED, LOW);     // turn led off
	  delay(100);
	  digitalWrite(StatLED, HIGH);     // turn led on
	  delay(100);
	  digitalWrite(StatLED, LOW);     // turn led off
	  delay(1000);
	  }
  }
  digitalWrite (StatLED, LOW);
                        
  initMotorPins();
}


void loop()                     // run over and over again
{
  int cmdByteCount;             // number of bytes in input command
  char complStatus[100];        // holds the response string back to the client program on PC
  char inCommand[100];          // holds the command sent from the client program
  
  digitalWrite (StatLED, HIGH);     // turn led on
  
  // check if pending move has finished
  if (operation_in_progress)
    {
    operation_in_progress = checkProgress (active_operation);
  } 

  inCommand[0] = 10;     // empty command string
  cmdByteCount = readCommand(inCommand,100);
  strcpy(complStatus, "");
  if (cmdByteCount > 0) {
    //ShowCmdRecvdLED();
  }
  switch (inCommand[0])
      { // each cmd must set complStatus as return string ack to host
      case ABORT_MOTORS:      // Stop both motors
            trace("Abort Motors cmd");
            operation_in_progress = False;
            strcpy(complStatus, abort_motors());
            active_operation = 0;
            break;
      case OPEN_BOTH_SHUTTER:      // Open upper shutter
            trace("Open Both - start Upper cmd");
            if (! operation_in_progress)
              {
              operation_in_progress = start_upper_open(complStatus);
              if (operation_in_progress)
                {
                active_operation = OPEN_BOTH_SHUTTER;    // later we start the lower
                }
              else
                {  // upper is already open. Start lower
                operation_in_progress = start_lower_open(complStatus);
                if (operation_in_progress)
                  {
                  active_operation = OPEN_LOWER_SHUTTER;
                  }
                }
              }
            else
              {
              strcpy(complStatus, ERR_BUSY);
              }
            break;
      case CLOSE_BOTH_SHUTTER:      // lower upper shutter
            trace("Close Both cmd - start Lower shutter");
            if (! operation_in_progress)
              {
	      trace("no op in progress");
              operation_in_progress = start_lower_close(complStatus);
              if (operation_in_progress)
                {
		trace("Op now in progress");
                active_operation = CLOSE_BOTH_SHUTTER;
                }
              else
                { // lower is already closed. Start upper
		trace("lower close is done? Start upper");
                operation_in_progress = start_upper_close(complStatus);
                if (operation_in_progress)
                  {
		  trace("Close upper is in prog");
                  active_operation = CLOSE_UPPER_SHUTTER;
                  }
                }
              }
            else
              {
              strcpy(complStatus, ERR_BUSY);
              }
			trace(complStatus);
			trace(" ");
            break;
      case OPEN_UPPER_SHUTTER:      // Open upper shutter
            trace("Open Upper cmd");
            if (! operation_in_progress)
              {
              operation_in_progress = start_upper_open(complStatus);
              if (operation_in_progress)
                {
                active_operation = OPEN_UPPER_SHUTTER;
                }
              }
            else
              {
              strcpy(complStatus, ERR_BUSY);
              }
            break;
      case VENT_UPPER_SHUTTER:      // Vent upper shutter
            trace("Vent Upper cmd");
            if (! operation_in_progress)
              {
              operation_in_progress = upper_vent(complStatus);
              if (operation_in_progress)
                {
                active_operation = VENT_UPPER_SHUTTER;
                }
              }
            else
              {
              strcpy(complStatus, ERR_BUSY);
              }
            break;
      case CLOSE_UPPER_SHUTTER:      // lower upper shutter
            trace("Close Upper cmd");
            if (! operation_in_progress)
              {
              operation_in_progress = start_upper_close(complStatus);
              if (operation_in_progress)
                {
                active_operation = CLOSE_UPPER_SHUTTER;
                }
              }
            else
              {
              strcpy(complStatus, ERR_BUSY);
              }
            break;
      case OPEN_LOWER_SHUTTER:      // Open lower shutter
            trace("Open Lower cmd");
            if (! operation_in_progress)
              {
              operation_in_progress = start_lower_open(complStatus);
              if (operation_in_progress)
                {
                active_operation = OPEN_LOWER_SHUTTER;
                }
              }
            else
              {
              strcpy(complStatus, ERR_BUSY);
              }
            break;
      case CLOSE_LOWER_SHUTTER:      // lower lower shutter
            trace("Close Lower cmd");
            if (! operation_in_progress)
              {
              operation_in_progress = start_lower_close(complStatus);
              if (operation_in_progress)
                {
                active_operation = CLOSE_LOWER_SHUTTER;
                }
              }
            else
              {
              strcpy(complStatus, ERR_BUSY);
              }
            break;
      case GET_SHUTTER_STATUS:      // retrieve status
            //trace("GetStatus cmd");
            char stat[400];
            getStatus(stat);
            strcpy(complStatus, stat);
            break;
      case CONFIGURE_PARAM:      // Change a configuration parameter
            //trace("ConfigureParam cmd");
            //String theCmd = inCommand;
            ReconfigureParam(inCommand);
            strcpy(complStatus, OPERATION_COMPLETED);
            break;
      case IS_MOTOR_MOVING:      // Shutter movement status
            trace("IsMoving cmd");
            strcpy(complStatus, NOT_MOVING);
            trace(operation_in_progress? "True":"False");
            if (operation_in_progress) 
              {
              strcpy(complStatus, IS_MOVING);
              }
            break;
      case 46:
      case 13:
      case 10:
            // empty command
            break;
      default:
            char b[400];
            sprintf(b,"%s{%c} %d", ERR_UNKNOWN_COMMAND, inCommand[0], inCommand[0]);
            strcpy(complStatus, b);
      }
  if (strcmp(complStatus,"") != 0)      // check for response to host
    {
    writeBuf (complStatus);
    }

  SetSwitchPins();
  CheckManualSwitches();

  // if we have an operation in progress, need a short delay
  if (operation_in_progress)
    {
    mLoopDelay = SHORT_LOOP_DELAY;
    }
  else
    {
    mLoopDelay = LONG_LOOP_DELAY;
    }
  
  delay(mLoopDelay); 
  digitalWrite (StatLED, LOW);     // turn led off
  delay(mLoopDelay);                                     //waiting a second
}


