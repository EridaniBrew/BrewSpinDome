/*
 * Simplified form of SpinDome to test interrupt issue with
 * position pulses.
 * Accepts 2 commands through Serial:
 *   A#: stop motor
 *   S#: Start motor
 */

// pin definitions
int BlockTic = 2;       // Block Sensor. Pin 2 is associated with interrupt 0
#define BLOCK_INTERRUPT 0

/*  VNH2SP30 pin definitions */
int inApin = 7;  // INA: Clockwise input   Digital Output
int inBpin = 8; // INB: Counter-clockwise input  Digital Output
// I have connected pin45, which supports PWM, to pin 5 on the shield.
// Pin 5 of the shield has been cut in the header so it DOES NOT connect
// to the main board pin 5.
int pwmpin = 5; // PWM input   Digital Output pins, use AnalogWrite

int BUMP_INTERRUPT_PIN = 9;     // shows oscilloscope duration of interrupt

volatile unsigned long last_button_time = 0; 
volatile unsigned long button_time = 0;


// interrupt for the position sensor. Gets called on both Rising and Falling voltage
void bumpPosInterrupt()
{
  button_time = millis();
  //check to see if increment() was called in the last few milliseconds
  if ((button_time - last_button_time) > 10)
      {
      last_button_time = button_time;
      digitalWrite(BUMP_INTERRUPT_PIN, HIGH);
      for (long i=0; i<1000; i++)
          digitalRead(BlockTic);
      digitalWrite(BUMP_INTERRUPT_PIN, LOW);
  }
    
}

// Brute force
void CheckBlockState()
{
    int state = digitalRead(BlockTic);
    digitalWrite(BUMP_INTERRUPT_PIN, state);
}


void abort_motors()
{
  digitalWrite(inApin, LOW);
  digitalWrite(inBpin, LOW);
  analogWrite(pwmpin, 0);      // speed 0
}

void start_motor()
{
    digitalWrite(inApin, HIGH);
    digitalWrite(inBpin, LOW);
    analogWrite(pwmpin, 1023);   // full speed  
}

// readCommand
// reads string up to #  NOTE - the # is not in the returned string
// returns number of bytes read
// fills in inCommand
unsigned int readCommand(char *inCommand)
{
  unsigned int readCount = 0;
  String sBuf;
  
  if (Serial.available() > 0)
      {
      sBuf = Serial.readStringUntil('#');
      readCount = sBuf.length();
      }
  else
      {
      return (0);
      }
  
  sBuf.toUpperCase();
  
  // remove cr or lf chars if in beginning
  while ((sBuf[0] == 10) || (sBuf[0] == 13))
      {
      sBuf.remove(0,1);
      }
  readCount = sBuf.length();
  
  strcpy (inCommand, "");
  if (sBuf.length() == 0)
      return(0);
  
  sBuf.getBytes((unsigned char *)inCommand, sBuf.length() +1);
  return (readCount);
}


void setup()
{
  Serial.begin(115200);
  
  pinMode (BlockTic, INPUT_PULLUP);
  pinMode (BUMP_INTERRUPT_PIN, OUTPUT);
  
  // Motor Shield pins
  pinMode(inApin, OUTPUT);
  pinMode(inBpin, OUTPUT);
  pinMode(pwmpin, OUTPUT);

  digitalWrite(inApin, LOW);
  digitalWrite(inBpin, LOW);
  digitalWrite(BUMP_INTERRUPT_PIN, LOW);
  attachInterrupt(BLOCK_INTERRUPT, bumpPosInterrupt, CHANGE);
}

void loop()                    
{
  unsigned int cmdByteCount;             // number of bytes in input command
  char inCommand[100];          // holds the command sent from the client program
      
  inCommand[0] = 0;     // empty command string
  cmdByteCount = readCommand(inCommand);
   
  if (strcmp(inCommand, "A") == 0)      // Stop motor
      {
      Serial.println("Abort motor");
      abort_motors();
      }
  else if (strcmp(inCommand, "S") == 0)      // Start motor
      {
      Serial.println("Start motor");
      start_motor();    
      }
  
  //CheckBlockState();    
  delay(100);
}

