/*
 * Sketch to control the rotation of my Dome.
 * Dome is an Exploradome I, with Foster Systems hardware.
 *
 * ASCOM driver communicates commands via USB serial.
 *
 * Hardware has the following:
 *    Home sensor - digital input triggered by magnet switch
 *    Position sensor - IR LED/receiver used to count blocks on a black/
 *                  white pattern. Number of blocks indicates the position
 *                  of the dome's rotation. This position gets zeroed 
 *                  whenever the Home sensor is encountered.
 *    Slewing LED - Red when turning East, Green when turning West. BiColor LED
 *                  Flashes yellow when nothing is going on.
 *
 *    Home LED - turns on when at the Home position
 *
 *    SparkFun MonsterMoto shield used to output 12V DC power to the rotation motor
 *
 *    Manual Switch: 3 position switch to allow manual rotation of the dome.
 *
 *  Command protocol:
 *    Command from PC              Response
 *    GETPOS#                      POS nnnn#        
 *      Returns current block position. 
 *
 *    SETPOS nnnn#                  OK#
 *      Sets the blobkpos to the specified value.
 *      Used when connecting to Arduino to set where the position was last time.
 *
 *    SLEWTO nnnn dir#              OK#
 *      Rotates dome in the direction indicated to the specified block position.
 *      dir is E or W
 *      Returns OK when rotation starts; application checks position to determine when
 *      rotation is complete.
 *        SETPOS 546 E#    rotates east to block position 546
 *        SETPOS 0 W#    homes the dome in the western direction
 *    ABORT#                      OK#
 *      Halts dome rotation
 *
 *    ISHOME#                      TRUE# or #FALSE
 *      True if Home Sensor indicates at home. GetPos "should" be at 0, may not be.
 *
 *    ISSLEWING#                   TRUE# or FALSE#
 *      True if dome is currently slewing to a target
 *
 *    COUNTBLOCKS#                 OK# or errmsg
 *      Starts internal routine to count the number of blocks in one rotation of dome
 *
 *    GETTOTALBLOCKS#                   COUNT nnnn#
 *      Returns the result of the CountBlocks command.
 *      If routine is not yet done, nnnn is 0
 *
 *    SETTOTALBLOCKS n#              OK#
 *      Initializes the TotalBlocks per rotation parameter. This would
 *      normally be performed when the ASCOM driver connects.
 *
 *    TOTALHIST#                     HIST n1,n2,n3,n4,n5#
 *      Returns the last 5 total block counts from history. 
 *      when the Home sensor is detected the clock count gets reset. The current
 *      block count should be equal to the TotalBlockCount at that point - 
 *      this is to verify that this is true.
 *
 *  Pin assignments:
 *    Motor Shield:  0-9
 *    Position sensor:  10
 *    Home Sensor:      11
 *    Position LED:     12
 *    Home LED:         13
 *
 Version 1.0    - initial version
         1.1    - fixed case where SLOW_RATE was selected for longer slews
 */
 
#define VERSION "SpinDome Arduino V 1.1"

// Used for tracking status of special counting operation
char CountingPhase[15] = "";
boolean motorRunning = false;        // track whether motor is running 

// hold the results of the SelfTest
// either blank, or a SelfTest Failure error message
char selfTestErr[60] = "";      // use literal messages

//#define BREWTRACE
#ifdef BREWTRACE
#define trace(s) Serial.print ((s))
#define traceln(s) Serial.println ((s))
#else
#define trace(s)
#define traceln(s)
#endif

void setup()
{
  Serial.begin(9600);
  traceln(F(VERSION));
  
  initPins();
  //SelfTest();
  if (strcmp(selfTestErr, "") != 0)
      {
      traceln(F("Setup: SelfTest Failed"));
      traceln(selfTestErr);
      }
}

void loop()                     // run over and over again
{
  unsigned int cmdByteCount;             // number of bytes in input command
  char complStatus[70];        // holds the response string back to the client program on PC
  char inCommand[20];          // holds the command sent from the client program
  char parm1[20];
  char parm2[20];
  long cycleDelay;
      
  inCommand[0] = 0;     // empty command string
  parm1[0]= 0;
  parm2[0]  = 0;
  cmdByteCount = readCommand(inCommand, parm1, parm2, 70);
//  if (cmdByteCount > 0)
//      {
//      String s = "readCommand gives " ;
//      s += inCommand;
//      traceln(inCommand);
//      }
  strcpy(complStatus, "");
  cycleDelay = 1000;
  if ((cmdByteCount == 0)&& (!motorRunning)) {
    unsigned long startFlash = millis();
    FlashYellowHeartbeat(10);
    cycleDelay -= (millis() - startFlash);
  }
   
  // check if the Counting Phases are still running
  if (strcmp(CountingPhase, "") != 0)
      {  // still running, check if the current phase is done
      CheckCountPhase();
      }
    
  // if self test failed, ignore command and return the error message
  if (strcmp(selfTestErr, "") != 0) 
      {
      if (strlen(inCommand) > 2)
          strcpy (complStatus, selfTestErr);
      FlashYellowHeartbeat(1000);
      }
  // these commands are allowed even if counting is going on
  else if (strcmp(inCommand, "ABORT") == 0)      // Stop both motors
      {
      traceln("Abort Motors cmd");
      strcpy(complStatus, abort_motors());
      }
  else if (strcmp(inCommand, "ISSLEWING") == 0)      
      {
      traceln ("ISSLEWING cmd");
      strcpy(complStatus, IsSlewing());    
      }
      
  else if (strcmp(CountingPhase,"") == 0)
      {
      // these are only allowed if CountBlocks is not in progress
      if (strcmp(inCommand, "GETPOS") == 0)      // Retrieve current block pos
          {
          traceln ("GETPOS cmd");
          sprintf(complStatus,"POS %ld#", GetPos());
          }
      else if (strcmp(inCommand, "SETPOS") == 0)      // Start moving dome to target position
          {
          traceln ("SETPOS cmd");
          strcpy (complStatus, SetPos(parm1));   // parm1 is new BlockPos 
          }
      else if (strcmp(inCommand, "SLEWTO") == 0)      // Start moving dome to target position
          {
          traceln ("SLEWTO cmd");
          strcpy (complStatus, SlewTo(parm1, parm2));   // parm1 is targetPos, parm2 is E/W 
          }
      else if (strcmp(inCommand, "ISHOME") == 0)      // Retrieve Home Sensor
          {
          traceln ("ISHOME cmd");
          strcpy (complStatus, IsHome());    
          }
      if (strcmp(inCommand, "GETVAL") == 0)      // Retrieve current block pos
          {
          traceln ("GETVAL cmd");
          sprintf(complStatus,"VAL %d#", GetTestVal());
          }
      else if (strcmp(inCommand, "GETTOTALBLOCKS") == 0)      // returns num blocks in one rotation
          {
          traceln ("GETTOTALBLOCKS cmd");
          GetTotalBlocks(complStatus);    
          }
      else if (strcmp(inCommand, "SETTOTALBLOCKS") == 0)      // returns num blocks in one rotation
          {
          traceln ("SETTOTALBLOCKS cmd");
          SetTotalBlocks(complStatus, parm1);    // parm1 is the number of blocks
          }
      else if (strcmp(inCommand, "COUNTBLOCKS") == 0)      // Count blocks in one rotation
          {
          traceln ("COUNTBLOCKS cmd");
          strcpy (complStatus, CountBlocks());    
          }
      else if (strcmp(inCommand, "TOTALHIST") == 0)      // Total Blocks per rotation history
          {
          traceln ("TOTALHIST cmd");
          strcpy (complStatus, TotalHist());    
          }
      else
          {
          //sprintf(complStatus,"%s {%s} %d#", "Unk Cmd ", inCommand, inCommand[0]);
          }
      }  // counting inactive commands
  else
      { // command not allowed while counting is active
      if (strcmp(inCommand,"") != 0)
          {
          strcpy(complStatus, "Cmd invalid while counting on#");
          }
      }
      
  if (strcmp(complStatus,"") != 0)      // check for response to host
    {
    writeBuf (complStatus);
    }
  if (strcmp(CountingPhase, "") == 0)
    {
    CheckManualSwitches();
    }
  cycleDelay = 50;
  delay(cycleDelay);
}

