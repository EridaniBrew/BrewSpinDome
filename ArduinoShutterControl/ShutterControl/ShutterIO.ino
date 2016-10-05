// Routines to run the hardware - motors, switches

// pin definitions
int upperOpenSwitch = 22;    // Digital Input pin for upper shutter Open switch 
int upperClosedSwitch = 23;    //  upper shutter Closed switch
int lowerOpenSwitch = 24;    
int lowerClosedSwitch = 25;    

#define LEDupperOpenSw 26
#define LEDupperClosedSw 27
#define LEDlowerOpenSw 28 
#define LEDlowerClosedSw 29

#define UPPER_MOTOR 0
#define LOWER_MOTOR 1
#define NUM_MOTORS 2

// pins to run the motors. 
#define BRAKEVCC 0
#define CW   1
#define CCW  2
#define BRAKEGND 3
#define CS_THRESHOLD 100        // appears to be a limit check on the motor current?
#define DEFAULT_UPPER_OPEN_DIR CW
#define DEFAULT_LOWER_OPEN_DIR CCW

// PWM constants
#define PWM_FULL_SPEED 1023        // SampleMonster indicates 256 is maximum, although they use 1023
#define PWM_STOP       0
#define PWM_HALF_SPEED 511
#define DEFAULT_MOTOR_SPEED 750
#define PWM_DELTA      64			// motor speed ramps up by this increment
int motorRunningSpeed = 0;			// Current speed of operating motor at the moment

// directions for the motors
#define OPEN_SHUTTER    0   // use openShutter directions for motor
#define CLOSE_SHUTTER   1   // Use closeShutter directions

// when lower shutter closes, run a little longer to ensure switch is closed
#define LOWER_SHUTTER_CLOSE_DELAY 500     

/*  VNH2SP30 pin definitions
 xxx[0] controls motor '1' outputs
 xxx[1] controls motor '2' outputs */
int inApin[2] = {7, 4};  // INA: Clockwise input   Digital Output
int inBpin[2] = {8, 9}; // INB: Counter-clockwise input  Digital Output
// I have connected pin45, which supports PWM, to pin 5 on the shield.
// Pin 5 of the shield has been cut in the header so it DOES NOT connect
// to the main board pin 5.
int pwmpin[2] = {45, 6}; // PWM input   Digital Output pins, use AnalogWrite
int cspin[2] = {2, 3}; // CS: Current sense ANALOG input
int enpin[2] = {0, 1}; // EN: Status of switches output (Analog pin) ? not used?
int openingLED[2] = {32,34};    // When the motor is run to Open, light this LED
int closingLED[2] = {33,35};    // when motor runs to close

int openDirection[2] = {CW, CCW};  // opening/closing motor directions
int closeDirection[2] = {CCW, CW}; // May be configured from SimpleShutter to reverse
                                   // directions of motor
                                   // Note - open and close should be opposites!
int motorSpeed[2] = {DEFAULT_MOTOR_SPEED,DEFAULT_MOTOR_SPEED};   // PWM value for motor

// Pins for digital input to manually run motor open/close
int runOpenPin[2] = {38,40};
int runClosePin[2] = {39,41};

int pinVoltage = A8;      // Analog In for voltage
int pinTemperature = A10;  // Analog in for Temperature (TMP36)

#define VENT_TIME 3000              // Time to let motor run during venting
long VentTime = VENT_TIME;

long ventStartTime = 0;          // track timing for vent operation

// initPins  - initialize the various pins
//      The analogRead pins cspin, enpin? do not need initializing
void initMotorPins()
{
  pinMode (upperOpenSwitch, INPUT_PULLUP);
  pinMode (upperClosedSwitch, INPUT_PULLUP);
  pinMode (lowerOpenSwitch, INPUT_PULLUP);
  pinMode (lowerClosedSwitch, INPUT_PULLUP);

  pinMode(LEDupperOpenSw, OUTPUT);
  pinMode(LEDupperClosedSw, OUTPUT);
  pinMode(LEDlowerOpenSw, OUTPUT);
  pinMode(LEDlowerClosedSw, OUTPUT);
  digitalWrite(LEDupperOpenSw,LOW);
  digitalWrite(LEDupperClosedSw,LOW);
  digitalWrite(LEDlowerOpenSw,LOW);
  digitalWrite(LEDlowerClosedSw,LOW);

  // Initialize digital pins as outputs
  for (int i=0; i<NUM_MOTORS; i++)
  {
    pinMode(inApin[i], OUTPUT);
    pinMode(inBpin[i], OUTPUT);
    pinMode(pwmpin[i], OUTPUT);
    pinMode(openingLED[i], OUTPUT);
    pinMode(closingLED[i], OUTPUT);
    pinMode (runOpenPin[i], INPUT_PULLUP);
    pinMode (runClosePin[i], INPUT_PULLUP);
  }
  // Initialize braked
  for (int i=0; i<NUM_MOTORS; i++)
  {
    digitalWrite(inApin[i], LOW);
    digitalWrite(inBpin[i], LOW);
  }
  
  SetUpperMotorDirection(True);
  SetLowerMotorDirection(True);

}


/////////////////////////////////////////////////////////////
// start_upper_open() - if the upper shutter is not open, start it opening
//     returns True if open operation started
boolean start_upper_open(char *complStatus)
{
  boolean result = False;
  
  // If already open, return
  if (switchClosed (upperOpenSwitch))
    { // Already open - return
    strcpy(complStatus, OPERATION_COMPLETED);
    return (result);
    }
    
  trace ("Upper Motor Opening");
  // Start open - turn on upper motor
  turnOnMotor (UPPER_MOTOR, OPEN_SHUTTER, motorSpeed[UPPER_MOTOR]);
  
  strcpy(complStatus, OPERATION_COMPLETED);
  result = True;
  return (result);
}

// start_upper_close() - if the upper shutter is not closed, start closing it 
//     returns True if close operation started
boolean start_upper_close(char *complStatus)
{
  boolean result = False;
  
  // If already closed, return
  if (switchClosed (upperClosedSwitch))
    { // Already closed - return
    strcpy (complStatus, OPERATION_COMPLETED);
    return (result);
    }
  
  // Lower shutter must be closed
  if (switchOpen (lowerClosedSwitch))
    {
    strcpy(complStatus, "Failed: Lower shutter not closed");
    return (result);
    }

  trace ("Upper Motor Closing");
  // Start close - turn on upper motor, direction reverse
  turnOnMotor (UPPER_MOTOR, CLOSE_SHUTTER, motorSpeed[UPPER_MOTOR]);
  
  strcpy(complStatus, OPERATION_COMPLETED);
  result = True;
  return (result);
}

// start_lower_open() - if the lower shutter is not open, start it opening
//     returns True if open operation started
boolean start_lower_open(char *complStatus)
{
  boolean result = False;
  
  // If already open, return
  if (switchClosed (lowerOpenSwitch))
    { // Already open - return
    strcpy(complStatus, OPERATION_COMPLETED);
    return (result);
    }
  
  trace ("Lower Motor Opening");  
  // Start open - lower the lowerShutter
  turnOnMotor (LOWER_MOTOR, OPEN_SHUTTER, motorSpeed[LOWER_MOTOR]);
  
  strcpy(complStatus, OPERATION_COMPLETED);
  result = True;
  return (result);
}

// start_lower_close() - if the lower shutter is not closed, start closing it 
//     returns True if close operation started
boolean start_lower_close(char *complStatus)
{
  boolean result = False;
  
  // If already closed, return
  if (switchClosed (lowerClosedSwitch))
    { // Already closed - return
    strcpy (complStatus, OPERATION_COMPLETED);
    return (result);
    }
  
  // Upper shutter can not be in Closed position already.
  if (switchClosed (upperClosedSwitch))
    {
    strcpy(complStatus, "Failed: Upper shutter already closed");
    return (result);
    }

  trace ("Lower Motor Closing");
  // Start close - turn on upper motor, direction reverse
  turnOnMotor (LOWER_MOTOR, CLOSE_SHUTTER, motorSpeed[LOWER_MOTOR]);
  
  strcpy(complStatus, OPERATION_COMPLETED);
  result = True;
  return (result);
}


// Venting must wait several seconds to allow the vent to complete.
// Completes immediately if UpperOpenSwitch is closed (dome already 
// completely open).
// register venting time; vent will continue until
//    a) VENT_TIMEOUT occurs, or
//    b) upperOpenSwitch is closed
// return False if vent is done, or True if operation has been started
boolean upper_vent(char *complStatus)
{
  boolean result = False;
  if(switchClosed(upperOpenSwitch))
    {
      strcpy (complStatus, "Dome already fully opened");
      result = False;
    }
  else
    {  // do the move
    ventStartTime = millis();
    turnOnMotor (UPPER_MOTOR,OPEN_SHUTTER,motorSpeed[UPPER_MOTOR]);
    result = True;
    strcpy(complStatus, OPERATION_COMPLETED);
    }
    
  return result;
}


////////////////////////////////////////////////////////////////////////
// checkProgress - Check whether appropriate switch closings should
// trigger turning off motors
// we don't return a status indicating operation has completed.
// user checks with the IS_MOTOR_MOVING cmd
// If we shut off the motor, 
//    return False for operation_in_progress (op has finished) 
//           True means keep running
boolean checkProgress (int cmd)
{
  boolean result = True;
  char comp[60];
  
  switch (cmd)
    {
    case OPEN_BOTH_SHUTTER:       // upper shutter was started
			if (motorRunningSpeed < PWM_FULL_SPEED)
			  {
			  motorRunningSpeed = motorRunningSpeed + PWM_DELTA;
			  analogWrite(pwmpin[UPPER_MOTOR], motorRunningSpeed);
			  }
           if (switchClosed (upperOpenSwitch))
             { // turn off upper motor
             turnOffMotor (UPPER_MOTOR);
             trace("Turned off Upper motor");
             // start the lower shutter
             result = start_lower_open(comp);
             if (result) 
               {
               active_operation = OPEN_LOWER_SHUTTER;
               }
             else
               {
               trace("Lower Shutter already open");
               }  
             }
           break;
    case CLOSE_BOTH_SHUTTER:    // close lower shutter was started
			if (motorRunningSpeed < PWM_FULL_SPEED)
			  {
			  motorRunningSpeed = motorRunningSpeed + PWM_DELTA;
			  analogWrite(pwmpin[LOWER_MOTOR], motorRunningSpeed);
			  }
           if (switchClosed (lowerClosedSwitch))
             { // turn off lower motor
             delay(LOWER_SHUTTER_CLOSE_DELAY);
             turnOffMotor (LOWER_MOTOR);
             trace("Turned off Lower motor");
             // start the upper shutter
             result = start_upper_close(comp);
             if (result) 
               {
               active_operation = CLOSE_UPPER_SHUTTER;
               }
             else
               {
               trace("Upper Shutter already closed");
               }  
             }
           break;
    case OPEN_UPPER_SHUTTER:
			if (motorRunningSpeed < PWM_FULL_SPEED)
			  {
			  motorRunningSpeed = motorRunningSpeed + PWM_DELTA;
			  analogWrite(pwmpin[UPPER_MOTOR], motorRunningSpeed);
			  }
           if (switchClosed (upperOpenSwitch))
             { // turn off upper motor
             turnOffMotor (UPPER_MOTOR);
             trace("Turned off Upper motor");
             result = False;
             }
           break; 
    case CLOSE_UPPER_SHUTTER:
			if (motorRunningSpeed < PWM_FULL_SPEED)
			  {
			  motorRunningSpeed = motorRunningSpeed + PWM_DELTA;
			  analogWrite(pwmpin[UPPER_MOTOR], motorRunningSpeed);
			  }
           if (switchClosed (upperClosedSwitch))
             { // turn off upper motor
             turnOffMotor (UPPER_MOTOR);
             trace("Turned off Upper motor");
             result = False;
             }
           break; 
    case OPEN_LOWER_SHUTTER:
			if (motorRunningSpeed < PWM_FULL_SPEED)
			  {
			  motorRunningSpeed = motorRunningSpeed + PWM_DELTA;
			  analogWrite(pwmpin[LOWER_MOTOR], motorRunningSpeed);
			  }
           if (switchClosed (lowerOpenSwitch))
             { // turn off lower motor
             turnOffMotor (LOWER_MOTOR);
             trace("Turned off Lower motor");
             result = False;
             }
           break; 
    case CLOSE_LOWER_SHUTTER:
			if (motorRunningSpeed < PWM_FULL_SPEED)
			  {
			  motorRunningSpeed = motorRunningSpeed + PWM_DELTA;
			  analogWrite(pwmpin[LOWER_MOTOR], motorRunningSpeed);
			  }
           if (switchClosed (lowerClosedSwitch))
             { // turn off lower motor
             delay(LOWER_SHUTTER_CLOSE_DELAY);
             turnOffMotor (LOWER_MOTOR);
             trace("Turned off Lower motor");
             result = False;
             }
           break; 
    case VENT_UPPER_SHUTTER:
			if (motorRunningSpeed < PWM_FULL_SPEED)
			  {
			  motorRunningSpeed = motorRunningSpeed + PWM_FULL_SPEED;
			  analogWrite(pwmpin[UPPER_MOTOR], motorRunningSpeed);
			  }
           if (switchClosed(upperOpenSwitch))
             {
             turnOffMotor (UPPER_MOTOR);
             trace("Turned off Upper motor - switch");
             result = False;
             }
           else if ((millis() - ventStartTime) > VentTime)
             {
             turnOffMotor (UPPER_MOTOR);
             trace("Turned off Upper motor after time");
             result = False;
             }
         break;
    default:
           ;
    }
    return (result);
}

// manualSwitchOpen (int switch) - return TRUE if switch is open
// these are normally open switches
boolean manualSwitchOpen (int sw)
{
  boolean op = (digitalRead(sw) == HIGH);  // Open is Low - these switches are normally closed
  return (op);
}

boolean manualSwitchClosed (int sw)
{
  boolean op = (digitalRead(sw) == LOW);  
  return (op);
}

// switchOpen (int switch) - return TRUE if switch is open
// these are normally closed switches
boolean switchOpen (int sw)
{
  boolean op = (digitalRead(sw) == LOW);  // Open is Low - these switches are normally closed
  return (op);
}

boolean switchClosed (int sw)
{
  boolean op = (digitalRead(sw) == HIGH);  
  return (op);
}

// getStatus() returns status of 4 switches, voltage?
void getStatus (char * result)
{
  char upperOpen = 'O';
  char upperClosed = 'O';
  char lowerOpen = 'O';
  char lowerClosed = 'O';
  
  if (switchClosed(upperOpenSwitch)) upperOpen = 'C';
  if (switchClosed(upperClosedSwitch)) upperClosed = 'C';
  if (switchClosed(lowerOpenSwitch)) lowerOpen = 'C';
  if (switchClosed(lowerClosedSwitch)) lowerClosed = 'C';
  
  // potentially, sensors can interfere, so take second reading
  // assuming 5 volt reference voltage
  int rawUpCurrent = analogRead (cspin[UPPER_MOTOR]);    
  rawUpCurrent = analogRead (cspin[UPPER_MOTOR]);
  
  int rawLowCurrent = analogRead (cspin[LOWER_MOTOR]);
  rawLowCurrent = analogRead (cspin[LOWER_MOTOR]);
  
  int rawVoltage = analogRead (pinVoltage);
  rawVoltage = analogRead (pinVoltage);
  
  int rawTemp = analogRead (pinTemperature);
  rawTemp = analogRead (pinTemperature);

  // combine pieces into result string
  // Note - arduino does not support floating point in sprintf
  sprintf(result,"Status,%c,%c,%c,%c,%5d,%5d,%5d,%5d,%c", upperOpen, upperClosed, lowerOpen, lowerClosed,
      rawUpCurrent , rawLowCurrent , rawVoltage , rawTemp, operation_in_progress? 'T':'F');
  //trace(result);
}


/* turnOnMotor() will set a motor going in a specific direction
 the motor will continue going in that direction, at that speed
 until told to do otherwise.
 
 motor: this should be either UPPER_MOTOR or LOWER_MOTOR, selectd which of the two
 motors to be controlled
 
 direct: Should be between 0 and 3, with the following result
 0: Brake to VCC
 1: Clockwise
 2: CounterClockwise
 3: Brake to GND
 
 pwm: should be a value between ? and 1023, higher the number, the faster
 it'll go
 */
void turnOnMotor(uint8_t motor, int whichWay, uint8_t pwm)
{
  uint8_t direct;
  
  if (motor < NUM_MOTORS)
  {
    if (whichWay == OPEN_SHUTTER)
    {
      direct = openDirection[motor];
      digitalWrite(openingLED[motor], HIGH);
    } else {
      direct = closeDirection[motor];
      digitalWrite(closingLED[motor], HIGH);
    }
    if (direct <= 2)
    {
      // Set inA[motor]
      if (direct <=1){
        digitalWrite(inApin[motor], HIGH);
      } else {
        digitalWrite(inApin[motor], LOW);
      }

      // Set inB[motor]
      if ((direct==0)||(direct==2)){
        digitalWrite(inBpin[motor], HIGH);
      } else {
        digitalWrite(inBpin[motor], LOW);
      }

      motorRunningSpeed = PWM_FULL_SPEED;       // use PWM_HALF_SPEED to ramp up
      analogWrite(pwmpin[motor], motorRunningSpeed);     // was pwm. Instead, set to 511, ramp up to 1023
	}
  }
}

// turnOffMotor - turn off up the target motor
void turnOffMotor (int motor)
{
  // Initialize braked
  for (int i=0; i<2; i++)
  {
    digitalWrite(inApin[i], LOW);
    digitalWrite(inBpin[i], LOW);
  }
  analogWrite(pwmpin[motor], 0);
  digitalWrite(openingLED[motor], LOW);
  digitalWrite(closingLED[motor], LOW);
}


// abort_motors()
char *abort_motors()
{
  turnOffMotor(UPPER_MOTOR);
  turnOffMotor(LOWER_MOTOR);
  return (OPERATION_COMPLETED);
}

//
// SetUpperMotorDirection
//   useDefault True means use DEFAULT_UPPER_OPEN_DIR and opposite
//              False means use the reverse
void SetUpperMotorDirection(boolean useDefault)
{
  if (useDefault){
    openDirection[UPPER_MOTOR] = DEFAULT_UPPER_OPEN_DIR;
    closeDirection[UPPER_MOTOR] = DEFAULT_UPPER_OPEN_DIR == CW? CCW: CW;
  } else {
    openDirection[UPPER_MOTOR] = DEFAULT_UPPER_OPEN_DIR == CW? CCW: CW;
    closeDirection[UPPER_MOTOR] = DEFAULT_UPPER_OPEN_DIR == CW? CW: CCW;
  }
}

void SetLowerMotorDirection(boolean useDefault)
{
  if (useDefault){
    openDirection[LOWER_MOTOR] = DEFAULT_LOWER_OPEN_DIR;
    closeDirection[LOWER_MOTOR] = DEFAULT_LOWER_OPEN_DIR == CW? CCW: CW;
  } else {
    openDirection[LOWER_MOTOR] = DEFAULT_LOWER_OPEN_DIR == CW? CCW: CW;
    closeDirection[LOWER_MOTOR] = DEFAULT_LOWER_OPEN_DIR == CW? CW: CCW;
  }
}

// inCommand looks like
//    C,name,val
void ReconfigureParam(char *inCommand)
{
  char *p = inCommand;
  char *name;
  char *val;
  String tr = "name = ";
  
  name = strtok_r(p, ",", &p);    //throw away the C cmd
  name = strtok_r(p, ",", &p);    //throw away the C cmd
  val = strtok_r(p, ",", &p);    //throw away the C cmd
  if (strcmp(name,"VentTime") == 0)
  {
    VentTime = atoi(val);
  }
  
  else if (strcmp(name,"ReverseUpper") == 0)
  {
    if (strcmp(strupr(val),"TRUE") == 0)
    {
      SetUpperMotorDirection(False);
    }
    else if (strcmp(strupr(val),"FALSE") == 0)
    {
      SetUpperMotorDirection(True);
    }
  }
  
  else if (strcmp(name,"ReverseLower") == 0)
  {
    if (strcmp(strupr(val),"TRUE") == 0)
    {
      SetLowerMotorDirection(False);
    }
    else if (strcmp(strupr(val),"FALSE") == 0)
    {
      SetLowerMotorDirection(True);
    }
  }
  
  else if (strcmp(name,"UpperSpeed") == 0)
  {
    //motorSpeed[UPPER_MOTOR] = atoi(val);
  }
  else if (strcmp(name,"LowerSpeed") == 0)
  {
    //motorSpeed[LOWER_MOTOR] = atoi(val);
  }
}

// setting the LEDs to reflect the current status of the input switches
void SetSwitchPins()
{
  digitalWrite(LEDupperOpenSw, switchClosed(upperOpenSwitch));
  digitalWrite(LEDupperClosedSw, switchClosed(upperClosedSwitch));
  digitalWrite(LEDlowerOpenSw, switchClosed(lowerOpenSwitch));
  digitalWrite(LEDlowerClosedSw, switchClosed(lowerClosedSwitch));
}

// Check whether user has manually clicked the switch (requestPin)
// in order to open or close the desired motor
// checkDonePin is the limit shutter switch to check for being done
void ManualRunMotor(int motor, int requestPin, int motorDir, int checkDonePin)
{
  if (manualSwitchClosed(requestPin)) {
    delay (100);  // debounce
    if (manualSwitchClosed(requestPin) && switchOpen(checkDonePin)){
      turnOnMotor(motor, motorDir, motorSpeed[motor]);
      while (manualSwitchClosed(requestPin) && switchOpen(checkDonePin)){
        delay(100);
      }
      if ((motor == LOWER_MOTOR) && (checkDonePin == lowerClosedSwitch)){
        delay(LOWER_SHUTTER_CLOSE_DELAY);
      }
      turnOffMotor(motor);
    }
  }
}

// Check to see if user has pressed one of the motor open/close switches
// if so, run the selected motor
void CheckManualSwitches()
{
  ManualRunMotor(UPPER_MOTOR, runOpenPin[UPPER_MOTOR], OPEN_SHUTTER, upperOpenSwitch);
  ManualRunMotor(UPPER_MOTOR, runClosePin[UPPER_MOTOR], CLOSE_SHUTTER, upperClosedSwitch);
  ManualRunMotor(LOWER_MOTOR, runOpenPin[LOWER_MOTOR], OPEN_SHUTTER, lowerOpenSwitch);
  ManualRunMotor(LOWER_MOTOR, runClosePin[LOWER_MOTOR], CLOSE_SHUTTER, lowerClosedSwitch);
}

// To help debugging, flash some LEDs when a command is received
void ShowCmdRecvdLED()
{
  digitalWrite(StatLED, HIGH);     // turn led on
  digitalWrite(LEDupperOpenSw, HIGH);     
  digitalWrite(LEDupperClosedSw, HIGH);     
  digitalWrite(LEDlowerOpenSw, HIGH);     
  digitalWrite(LEDlowerClosedSw, HIGH);     
  delay(300);
  digitalWrite(StatLED, LOW);     // turn led off
  digitalWrite(LEDupperOpenSw, LOW);     
  digitalWrite(LEDupperClosedSw, LOW);     
  digitalWrite(LEDlowerOpenSw, LOW);     
  digitalWrite(LEDlowerClosedSw, LOW);     
	  
}

