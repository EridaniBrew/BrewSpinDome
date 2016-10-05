/*
 * Routines to run the Hardware
 */

// pin definitions
#define BlockTic 2       // Block Sensor. Pin 2 is associated with interrupt 0
#define BLOCK_INTERRUPT 0
#define HomeSwitch  3    // Home sensor, associated with interrupt 1
#define HOME_INTERRUPT 1
#define HOME_SW_ON    HIGH
#define HOME_SW_OFF   LOW

#define RotateDir1LED A4    // LED indicates rotation direction
#define RotateDir2LED A5    // LED indicates rotation direction
#define HomeLED  12       // Home Sensor LED indicator for at home
#define ManualEastSw 10     // Manual Switch to turn dome to East
#define ManualWestSw 11
#define MANUAL_SW_ON    LOW
#define MANUAL_SW_OFF   HIGH
 
/* Holds the total number of blocks in one dome rotation */
int totalBlocks = 490;

#define HIST_DEPTH 5
int HistCount[HIST_DEPTH];      // save the last 5 total block counts when
                                // Home Sensor resets the count
                                // these "should" equal totalBlocks
int NextHistIdx = 0;            // index into HistCount. HistCount is a circular buffer
                                // NextHistIdx is the oldest, next to be filled in


#define EnableBlockInterrupt  attachInterrupt(BLOCK_INTERRUPT, bumpPosInterrupt, CHANGE)
#define DisableBlockInterrupt  detachInterrupt(BLOCK_INTERRUPT)

/* Motor stuff */
/* NOTE - only using one motor */
#define NUM_MOTORS 2

#define DOME_MOTOR 0
#define BRAKEVCC 0
#define CW   1
#define CCW  2
#define BRAKEGND 3
#define CS_THRESHOLD 100        // appears to be a limit check on the motor current?


// If we wish to rotate the dome continuously, do a SlewTo 9999 E# command
#define CONTINUOUS_ROTATE_POS 9999


#define LEDTurningStop   BicolorLEDset (0,0)
#define LEDTurningEast   BicolorLEDset (255,0)    // should be red
#define LEDTurningWest   BicolorLEDset (0,255)    // should be Green

// PWM constants
#define PWM_FULL_SPEED 1023        // SampleMonster indicates 256 is maximum, although they use 1023
#define PWM_STOP       0
#define DEFAULT_MOTOR_SPEED 1023    // PWM_FULL_SPEED

// When the motor gets close to the target position,
// slow down to hopefully get better precision
#define SLOW_MOTOR_SPEED   650
int EastSlowTargetPos = -1;    // when moving East, this position starts slowing
int WestSlowTargetPos = -1;
#define SLOW_ZONE  10     // slow down when within this many bloocks of target
int StartMotorSpeed = DEFAULT_MOTOR_SPEED;

/*  VNH2SP30 pin definitions
 xxx[0] controls motor '1' outputs
 xxx[1] controls motor '2' outputs */
int inApin[2] = {7, 4};  // INA: Clockwise input   Digital Output
int inBpin[2] = {8, 9}; // INB: Counter-clockwise input  Digital Output
// I have connected pin45, which supports PWM, to pin 5 on the shield.
// Pin 5 of the shield has been cut in the header so it DOES NOT connect
// to the main board pin 5.
int pwmpin[2] = {5, 6}; // PWM input   Digital Output pins, use AnalogWrite
int cspin[2] = {2, 3}; // CS: Current sense ANALOG input
int enpin[2] = {0, 1}; // EN: Status of switches output (Analog pin) ? not used?

#define OPERATION_COMPLETED "OK#"

// These pins are supposed to be used by the second motor.
// Since we don't use this motor, I am using them for debugging pins
// so the oscilloscope can show what is going on
#define BUMP_INTERRUPT_PIN 9       // shows high during the bump pos interrupt
volatile unsigned long last_button_time = 0; 
volatile unsigned long button_time = 0;
volatile int prevHomeState = 0;      // prev Home State
volatile int homeIntCount = 0;
volatile int bumpIntCount = 0;   // used to only flash interrupt LED every 5 pulses
#define BUMP_FLASH_MOD 5
volatile int prevBlockState = 0;      // prev Block State

// used in the CountingBlocks process
volatile boolean phaseDone = true;
#define PHASE1 "Ph1HomeW"    // these go into CountingPhase
#define PHASE2 "Ph2CntHomeW"



// global parameters controlling the movements
// mark volatile since used in interrupts, do not use register
volatile boolean rotEast = true;      // False if going West
volatile  long targetPos = -1;       // desired block position
volatile  long blkPos = 0;           // current block position.


// interrupt for the position sensor. Gets called on both Rising and Falling voltage
void bumpPosInterrupt()
{
  int blockState = digitalRead(BlockTic);
  //check to see if increment() was called in the last 25 milliseconds
  if (prevBlockState != blockState)
      {
      prevBlockState = blockState;
      digitalWrite(BUMP_INTERRUPT_PIN, HIGH);
      
      for (int i=0; i<200; i++)      // busy work
          digitalRead(ManualEastSw);
      if (rotEast)
          {
          blkPos--;
          if (blkPos < 0) blkPos = totalBlocks - 1;
          }
      else
          {
          blkPos++;
          //if (blkPos >= totalBlocks) blkPos = 0;
          }
          
      // check to see if it is time to slow the motor down
      if ((targetPos > -1) && (targetPos <= totalBlocks))
          {
          if (rotEast && (blkPos == EastSlowTargetPos))
              { 
              analogWrite(pwmpin[DOME_MOTOR], SLOW_MOTOR_SPEED);
              }
          else if (!rotEast && (blkPos == WestSlowTargetPos))
              {
              analogWrite(pwmpin[DOME_MOTOR], SLOW_MOTOR_SPEED);
              }
          }
          
      if ((blkPos == targetPos) && (targetPos > 0))
          { // We have reached our targetPos, so stop the motor
          // Note that Homing operation completes in the Home Interrupt
          stopMotor(DOME_MOTOR);
          targetPos = -1;
          }
          
      int homeVal = digitalRead(HomeSwitch);
      digitalWrite(HomeLED, homeVal);
      if ((homeVal == HOME_SW_ON) && (prevHomeState == HOME_SW_OFF))
          {
          homeIntCount++;
          HomeInterrupt();    // faking the home interrupt
          }
      prevHomeState = homeVal;
      digitalWrite(BUMP_INTERRUPT_PIN, LOW);
      }
    
}

// interrupt for the home sensor. Gets called on either Rising signal
void HomeInterrupt()
{
  if (targetPos == 0)
      {  // Homing operation
      stopMotor(DOME_MOTOR);
      targetPos = -1;
      }

  // save the current blockPos in history
  // "should" equal totalBlocks
  if (rotEast)
      {  // going East - counting down
      HistCount[NextHistIdx] = totalBlocks - blkPos;
      }
  else
      { // counting up
      HistCount[NextHistIdx] = blkPos;
      }
  NextHistIdx++;
  if (NextHistIdx >= HIST_DEPTH) NextHistIdx = 0;
  
  // only zero block count if going west.
  // The East home is off a bit
  if (!rotEast)
      blkPos = 0;
}


// initPins - initialize the various pins
//
void initPins()
{
  pinMode (HomeSwitch, INPUT_PULLUP);
  pinMode (BlockTic, INPUT_PULLUP);
  pinMode (ManualEastSw, INPUT_PULLUP);
  pinMode (ManualWestSw, INPUT_PULLUP);

  pinMode(RotateDir1LED,OUTPUT);
  pinMode(RotateDir2LED,OUTPUT);
  pinMode(HomeLED,OUTPUT);
  pinMode(BUMP_INTERRUPT_PIN,OUTPUT);
  LEDTurningStop;
  digitalWrite (HomeLED, LOW);
  prevBlockState = digitalRead(BlockTic);
  prevHomeState = digitalRead(HomeSwitch);
  
    // Initialize digital pins as outputs
  for (int i=0; i<NUM_MOTORS; i++)
  {
    pinMode(inApin[i], OUTPUT);
    pinMode(inBpin[i], OUTPUT);
    pinMode(pwmpin[i], OUTPUT);
  }
  // Initialize braked
  for (int i=0; i<NUM_MOTORS; i++)
  {
    digitalWrite(inApin[i], LOW);
    digitalWrite(inBpin[i], LOW);
  }
  EnableBlockInterrupt;
}

// abort_motors()
// called in response to ABORT command
char *abort_motors()
{
  stopMotor(DOME_MOTOR);
  
  // clear these in case we were running them
  if (strcmp(CountingPhase,"") != 0)
      {
      strcpy(CountingPhase,"");
      phaseDone = true;
      DisableBlockInterrupt;    // remove Counting interrupt
      EnableBlockInterrupt;     // restore normal interrupt
      }

  return (OPERATION_COMPLETED);
}

// GetPos()
//  returns the current block position of the dome
unsigned long GetPos()
{
    unsigned long pos = blkPos;    // use a copy so interrupts don't get weird
    return (pos);
}

int GetTestVal()
{
    return (homeIntCount);
}

// SetPos()
//  returns the current block position of the dome
char *SetPos(char *cPos)
{
    String s = cPos;
    int tempVal = 0;
    tempVal = s.toInt();
    blkPos = tempVal;    
    return (OPERATION_COMPLETED);
}


// SlewTo (target, dir)
//   starts the motors moving in the specified direction
//   dir = "E" or "W" for East or West direction
//   zero target means go to home sensor.
char * SlewTo(char *target, char *dir)
{
    char errmsg[30];
    strcpy (errmsg, OPERATION_COMPLETED);
    trace(F("SlewTo "));
    trace(target);
    traceln(dir);
    
    if ((strcmp(dir, "E") != 0) && (strcmp(dir, "W") != 0))
        {
        sprintf(errmsg, "Invalid direction %s#", dir);
        }
    
    String s = String(target);
    unsigned long tempPos = s.toInt();
    if (((tempPos > totalBlocks) && (tempPos != CONTINUOUS_ROTATE_POS)) || (tempPos < 0))
        {
        sprintf(errmsg, "Invalid position %ld#", tempPos);
        }

    if (strcmp(errmsg, OPERATION_COMPLETED) == 0)
        {   // Edits are OK
        StartMotorSpeed = DEFAULT_MOTOR_SPEED;
        homeIntCount = 0;
        if (strcmp(dir, "E") == 0)
            {  // run East
            rotEast = true;
            // set SlowTargetPos
            EastSlowTargetPos = tempPos + SLOW_ZONE;
            WestSlowTargetPos = -1;
            if (EastSlowTargetPos > totalBlocks) EastSlowTargetPos -= totalBlocks;
            trace(F("EastSlow limit is "));
            traceln(EastSlowTargetPos);
            }
        else
            {
            rotEast = false;
            // set SlowTargetPos
            WestSlowTargetPos = tempPos - SLOW_ZONE;
            EastSlowTargetPos = -1;
            if (WestSlowTargetPos < 0) WestSlowTargetPos += totalBlocks;
            trace(F("WestSlow limit is "));
            traceln(WestSlowTargetPos);
            }
        targetPos = tempPos;
        // check to see if close enough to slow the motor down
        if ((targetPos > -1) && (targetPos <= totalBlocks))
          {
          if (rotEast && (blkPos <= EastSlowTargetPos) && (blkPos > targetPos))
              { 
              StartMotorSpeed = SLOW_MOTOR_SPEED;
              }
          else if (!rotEast && (blkPos >= WestSlowTargetPos) && (blkPos < targetPos))
              {
              StartMotorSpeed = SLOW_MOTOR_SPEED;
              }
          }
      
        startMotor(DOME_MOTOR);
        }
    return (errmsg);
}

char *IsHome()
{
  //int homeSensor = digitalRead(HomeSwitch);
  if (digitalRead(HomeSwitch) == HOME_SW_ON)
      {
      return ("TRUE#");
      }
  else
      {
      return ("FALSE#");
      }
}

char *IsSlewing()
{
  if (motorRunning)
      {
      return ("TRUE#");
      }
  else
      {
      return ("FALSE#");
      }
}

// if no command is received and the motors are not running, flash the bicolor
// LED yellow to show we are operational
void FlashYellowHeartbeat(int count) {
  int i=0;
  for (i = 0; i < count; i++)
      {
      analogWrite (RotateDir1LED, 255);
      analogWrite (RotateDir2LED, 0);
      delay(1);
      analogWrite (RotateDir1LED, 0);
      analogWrite (RotateDir2LED, 255);
      delay(1);
      }
  analogWrite (RotateDir1LED, 0);
  analogWrite (RotateDir2LED, 0);
}


// returns HIST n1,n2,n3,n4,n5#
// where n are the last 5 totalblock counts 
//   per rotation measured as the dome has been rotating
char *TotalHist()
{
  char buf[60];
  int i = 0;
  int srcIdx = 0;
  int hist[HIST_DEPTH];
  
  for (i = 0; i < HIST_DEPTH; i++)
      {
        srcIdx = i + NextHistIdx;
        if (srcIdx >= HIST_DEPTH) srcIdx -= HIST_DEPTH;
        hist[i] = HistCount[srcIdx];
      }
  sprintf (buf,"HIST %d,%d,%d,%d,%d#", hist[0], hist[1], hist[2], hist[3], hist[4]); 
  return (buf);
}

/*
 * startMotor (whichMotor)
 * arranges interrupts, then starts motor
 */
void startMotor(int motor)
{
  motorRunning = true;
  turnOnMotor(motor);
}

/*
 * stopMotor (whichMotor)
 * arranges interrupts, then stops motor
 */
void stopMotor(int motor)
{
  motorRunning = false;
  turnOffMotor(motor);
  targetPos = -1;
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
 */
void turnOnMotor(uint8_t motor)
{
      // Set inA[motor]
      if (rotEast){
        digitalWrite(inApin[motor], LOW);
        digitalWrite(inBpin[motor], HIGH);
        LEDTurningWest;
      } else {
        digitalWrite(inApin[motor], HIGH);
        digitalWrite(inBpin[motor], LOW);
        LEDTurningEast;
      }

      analogWrite(pwmpin[motor], StartMotorSpeed);     
}


// turnOffMotor - turn off up the target motor
void turnOffMotor (int motor)
{
  digitalWrite(inApin[motor], LOW);
  digitalWrite(inBpin[motor], LOW);
  analogWrite(pwmpin[motor], PWM_STOP);      // speed 0
  LEDTurningStop;
}

// routine to light Bicolor LED Red, Green, or Off
void BicolorLEDset(int pin1val, int pin2val)
{
    analogWrite (RotateDir1LED, pin1val);
    analogWrite (RotateDir2LED, pin2val);
}


// CheckManualSwitches
// If the user pushes Manual switch to one side or the other, rotate dome East or West
void CheckManualSwitches()
{
    // Turn off HomeLED in case it got turned on
    digitalWrite(HomeLED, digitalRead(HomeSwitch));

    int turnEastSw = digitalRead(ManualEastSw);
    int turnWestSw = digitalRead(ManualWestSw);

    if (turnEastSw == MANUAL_SW_ON)   
        {
        rotEast = true;
        targetPos = -1;
        startMotor(DOME_MOTOR);
        while (turnEastSw == MANUAL_SW_ON)
            {
            digitalWrite(HomeLED, digitalRead(HomeSwitch));
            delay (500);
            turnEastSw = digitalRead(ManualEastSw);
            }
        stopMotor(DOME_MOTOR);
        digitalWrite(HomeLED, digitalRead(HomeSwitch));
        }

    if (turnWestSw == MANUAL_SW_ON)
        {
        rotEast = false;
        targetPos = -1;
        startMotor(DOME_MOTOR);
        while (turnWestSw == MANUAL_SW_ON)
            {
            digitalWrite(HomeLED, digitalRead(HomeSwitch));
            delay (500);
            turnWestSw = digitalRead(ManualWestSw);
            }
        stopMotor(DOME_MOTOR);
        digitalWrite(HomeLED, digitalRead(HomeSwitch));
        }
}



/*
 * These routines allow measuring of the total number of blocks in one
 * dome rotation. This parameter is needed for the 
 * BlockCountInterrupt routine to properly count.
 *
 * Note that these routines use different interrupt routines (block and home) 
 * than the usual ones.
 *
 * CountBlocks begins the counting process. While this running, some other
 * commands are allowed, such as ABORT
 *
 * TotalBlocks checks whether we are done counting. If so, returns the total
 * number of blocks in a rotation as COUNT n#
 * If not done, returns COUNT 0#
 * if counting is not in process, returns COUNT -1#
 */
// interrupt for the home sensor. Goes off on Rising signal
// for positioning at home sensor
// Reworked - interrupts are not working right for Home Switch
// maybe this is due to various delay commands, etc?
// So, calll interrupt whenever block changes, check for Home 
// state change (similar to regular block processing)
volatile int cbPrevHomeState = LOW;
void CountingHomeInterrupt()
{
  int homeState = digitalRead(HomeSwitch);
  
  if ((cbPrevHomeState != homeState) && (homeState == HIGH))
      {
      stopMotor(DOME_MOTOR);
      motorRunning = true;
      phaseDone = true;
      digitalWrite (HomeLED, HIGH);
      }
  cbPrevHomeState =  homeState;
}


// interrupt for the position sensor. Gets called on both Rising and Falling voltage
// This is counting the blocks until the Home sensor gets hit
void CountingBlockInterrupt()
{
  int blockState = digitalRead(BlockTic);
  if (blockState != prevBlockState)
      {
      prevBlockState = blockState;
      blkPos++;
      digitalWrite(BUMP_INTERRUPT_PIN, HIGH);
      for (int i=0; i<200; i++)      // busy work
          digitalRead(ManualEastSw);
      digitalWrite(BUMP_INTERRUPT_PIN, LOW);

      CountingHomeInterrupt();
      }
}

char *CountBlocks()
{
  if (motorRunning)
      {
      return ((char *)(F("Arduino busy#")));
      }
  // Need to do 2 phases:
  // Home dome moving west. Now we are at starting point
  // Home again going west. Get total blocks encountered
  DisableBlockInterrupt;      // disable the standard interrupts

  StartMotorSpeed = DEFAULT_MOTOR_SPEED;

  // Phase 1 - Home west
  // only need Home interrupt
  if (digitalRead(HomeSwitch) == LOW)
      {
      traceln(F("Starting phase 1 - Home West"));
      strcpy (CountingPhase,PHASE1);
      phaseDone = false;
      attachInterrupt(BLOCK_INTERRUPT, CountingHomeInterrupt, CHANGE);
      rotEast = false;
      digitalWrite (HomeLED, LOW);
      turnOnMotor(DOME_MOTOR);
      motorRunning = true;
      }
  else
      { // already at home
      cbPrevHomeState = HIGH;
      startPhase2();
      }
  return (OPERATION_COMPLETED);
}


// CheckCountPhase
// when each phase completes, phaseDone becomes true
void CheckCountPhase()
{
  //traceln(F("Checking Count Phase"));
  if (phaseDone)
      {
        if (strcmp(CountingPhase, PHASE1) ==0)
            { // start Phase 2 - Counting blocks until home
            delay(500);     // for testing
            startPhase2();
            }
        else if (strcmp(CountingPhase, PHASE2) ==0)
            { // All done
            traceln(F("Finished phase 2 - All Done"));
            detachInterrupt(BLOCK_INTERRUPT);
            strcpy(CountingPhase, "");
            traceln(blkPos);
            totalBlocks = blkPos;   // blkPos contains the total counted
            motorRunning = false;
            EnableBlockInterrupt;    // re-enable the usual interrupt
            }
       }
}

void startPhase2()
{
    detachInterrupt(BLOCK_INTERRUPT);
    traceln(F("Starting phase 2 - Home West counting blocks"));
    phaseDone = false;
    digitalWrite(HomeLED, LOW);
    strcpy (CountingPhase,PHASE2);
    rotEast = false;
    blkPos = 0;
    attachInterrupt(BLOCK_INTERRUPT, CountingBlockInterrupt, CHANGE);
    turnOnMotor(DOME_MOTOR);
    motorRunning = true;
}


// After CountingBlocks has finished all the phases, read back the totalBlocks
// in the form TOTALBLOCKS n#
void GetTotalBlocks(char *msg)
{
  if (strcmp(CountingPhase,"") == 0)
      {
      sprintf(msg,"TOTALBLOCKS %d#", totalBlocks);
      }
  else
      {
      sprintf(msg,(char *)(F("Still Counting#")));
      }
}

// SetTotalBlocks
// Set the totalBlocks parameter (number of blocks per dome rotation)
void SetTotalBlocks (char *msg, char *count)
{
    strcpy (msg, OPERATION_COMPLETED);   // hopefully OK
    String s = count;
    int tempVal = 0;
    tempVal = s.toInt();
    if (tempVal > 0)
        {
        totalBlocks = tempVal;
        }
    else
        {
        String err = "Invalid total blocks parameter {" + s + "}#";
        strcpy (msg, err.c_str());
        }
}

// SelfTest
// Run some tests to make sure sensors seem to be working
// At end of successful test dome should be homed
// selfTestErr is set to "" if all OK, else
// an error message which is returned in response to any commands received.
// Terminate with # so it registers as a response
// This error message is only cleared by resetting the Arduino (presumably
// after fixing whatever caused the test to fail)
//void HomeWestSelfTest(int startPos)
//{
//    traceln (F("SelfTest: Starting West Homing"));
//    rotEast = false;
//    WestSlowTargetPos = totalBlocks - SLOW_ZONE;
//    targetPos = 0;
//    startMotor(DOME_MOTOR);
//    int i = 0;
//    while ((i < 60) && motorRunning)
//        {
//        delay(1000);
//        i++;
//        }
//    traceln (F("SelfTest: Homing done"));
//    
//    
//    if (motorRunning)
//        {
//        stopMotor (DOME_MOTOR);
//        if (digitalRead(HomeSwitch) == HOME_SW_OFF)
//            {
//            strcpy_P(selfTestErr, (char *)F("SelfTest failed: Home switch is not responding#"));
//            return;
//            }
//        if (blkPos == startPos)
//            {
//            strcpy_P(selfTestErr, (char *)F("SelfTest failed: Missing block interrupt, or motor did not move#"));
//            return;
//            }
//        if (blkPos != 0)
//            {
//            strcpy_P(selfTestErr, (char *)F("SelfTest failed: Block interrupts detected, did not reach home#"));
//            return;
//            }
//        strcpy_P(selfTestErr, (char *)F("SelfTest failed: Home reached, but motor did not stop#"));
//        return;
//        }
//    else
//        {  // motor was stopped
//        if (digitalRead(HomeSwitch) == HOME_SW_OFF)
//            {
//            strcpy_P(selfTestErr, (char *)F("SelfTest failed: Home switch is not responding#"));
//            return;
//            }
//        if (blkPos != 0)
//            {
//            strcpy_P(selfTestErr, (char *)F("SelfTest failed: home failed, not at 0#"));
//            return;
//            }
//        if (digitalRead(HomeSwitch) == HOME_SW_OFF)
//            {
//            strcpy_P(selfTestErr, (char *)F("SelfTest failed: Home detected, home switch stayed off#"));
//            return;
//            }
//        }
//}
//
//void SelfTest()
//{
//  strcpy(selfTestErr, "");     // hopefully everything is OK
//  // make sure Manual switches aren't on
//  traceln(F("SelfTest: Checking Manual switches"));
//  if (digitalRead(ManualEastSw) == MANUAL_SW_ON)
//      {
//      strcpy_P(selfTestErr, (char *)F("SelfTest failed: Manual Switch East is on#"));
//      return;
//      }
//  if (digitalRead(ManualWestSw) == MANUAL_SW_ON)
//      {
//      strcpy_P(selfTestErr, (char *)F("SelfTest failed: Manual Switch West is on#"));
//      return;
//      }
//  
//  if (digitalRead(HomeSwitch) == HOME_SW_OFF)
//      { // we should be somewhere random (not home)
//      traceln (F("SelfTest: Starting HomeWestSelfTest"));
//      blkPos = 100;
//      HomeWestSelfTest(blkPos);
//      }
//  else
//      { // At home. First slew East to somewhere before home switch, check tests
//      // then do HomeWestSelfTest
//      traceln (F("SelfTest: Starting East rotation"));
//      rotEast = true;
//      EastSlowTargetPos = totalBlocks - 20 + SLOW_ZONE;
//      targetPos = totalBlocks - 20;
//      int startBlk = blkPos;
//      traceln(F("  Starting motor"));
//      startMotor(DOME_MOTOR);
//      int i = 0;
//      while ((i < 35) && motorRunning)
//          {
//          delay(2000);
//          i++;
//          if (blkPos == startBlk)
//              {
//              strcpy_P(selfTestErr, (char *)F("SelfTest failed: East movement either did not see position interrupts, or motor did not move#"));
//              return;
//              }
//          }
//      traceln (F("SelfTest: rotation done"));
//      if (motorRunning)
//          {  // never found position 470
//          stopMotor(DOME_MOTOR);
//          if (blkPos != 0)
//              {
//              strcpy_P(selfTestErr, (char *)F("SelfTest failed: motor moved, but East movement did not find East target position#"));
//              return;
//              }
//          }
//      else
//          { // seems to have reached 470
//          if (digitalRead(HomeSwitch) == HOME_SW_ON)
//              {
//              strcpy_P(selfTestErr, (char *)F("SelfTest failed: home switch is staying on#"));
//              return;
//              }
//          if (blkPos == 0)
//              {
//              strcpy_P(selfTestErr, (char *)F("SelfTest failed: East move failed to change blkPos. Missing position interrupts, or dome did not move#"));
//              return;
//              }
//          else if (abs(blkPos - totalBlocks + 20) > 5)
//              {
//              strcpy_P(selfTestErr, (char *)F("SelfTest failed: East move failed to reach correct position#"));
//              return;
//              }
//          HomeWestSelfTest(totalBlocks - 20);
//          }
//      }
//}

