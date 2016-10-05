/* 
 * Write a simple simulator to act like the
 * Exploradome rotation hardware
 * This hopefully allows testing of the BrewSpin
 * Arduino code
 *
 * Inputs:
 *   Rotate East - rotate the dome East direction
 *   Rotate West - rotate the dome West
 *
 * Outputs:
 *   BlockTic - When the dome is rotating, a square wave
 *   is created by toggling this pin from High to Low.
 *   This is simulating the block sensor reacting to the
 *   white/black blocks.
 *
 *   Home sensor - Every totalBlocks tics, the home sensor is turned on.
 *
 */

 #define DOME_TICS	50

 #define CYCLE_PERIOD   100   // msec between checking pins. This is the BlockTic pulse width

 #define GoingEastLED 3     // lights Red when rotating East
 #define GoingWestLED 4     // lights Green when rotating West
 #define RotateEast 5			// Input signal to rotate
 #define RotateWest 6			
 #define BlockTic   7			// Block Sensor Output pulse
 #define HomeSensor 8			// Output home position
 #define CycleLED   10			// Just to see that the simulator is running

 boolean blockState = false;
 int blockCount = 0;

 char inCommand[20];
 char response[100];
 String sResp = "";

 void setup()
 {
   Serial.begin(115200);
   pinMode (RotateEast, INPUT_PULLUP);
   pinMode (RotateWest, INPUT_PULLUP);

   pinMode(GoingEastLED,OUTPUT);
   pinMode(GoingWestLED,OUTPUT);
   pinMode(BlockTic,OUTPUT);
   pinMode(HomeSensor,OUTPUT);
   pinMode(CycleLED,OUTPUT);

 }

void Rotate(int count)
 {
	blockCount += count;
	if (blockCount < 0)
	    {
		blockCount = DOME_TICS;
	    }
	else if (blockCount > DOME_TICS)
		{
		blockCount = 0;
		}

	// set the HomeSensor
	if (blockCount == 0)
		{
                //Serial.println("Home!!!!!!!!!!!!!!!!!!");
		digitalWrite (HomeSensor, LOW);
		}
	else
		{
		digitalWrite (HomeSensor, HIGH);
		}

	blockState = ! blockState;
	digitalWrite(BlockTic, blockState);
 }

 void loop()
 {
	digitalWrite (GoingEastLED, LOW);
	digitalWrite (GoingWestLED, LOW);
	if (digitalRead(RotateEast) == LOW)
		{  // rotate!
                //Serial.println("Rotate East");
		Rotate (-1);
                digitalWrite (GoingEastLED, HIGH);
		}
	if (digitalRead(RotateWest) == LOW)
		{  // rotate!
		//Serial.println("Rotate West");
		Rotate (1);
		digitalWrite (GoingWestLED, HIGH);
		}
	digitalWrite(CycleLED, LOW);
	delay(CYCLE_PERIOD/2);
	digitalWrite(CycleLED, HIGH);
	delay(CYCLE_PERIOD/2);
        if(readCommand(inCommand) > 0)
		{
		if (strcmp(inCommand,"POS") == 0)
			{
			sprintf (response, "%d", blockCount);
			Serial.println(response);
			}
		}
 }

 


// readCommand
// reads string up to #  NOTE - the # is not in the string
// returns number of bytes read
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
  strcpy (inCommand, "");
  sBuf.getBytes((unsigned char *)inCommand, sBuf.length() +1);
      
  return (readCount);
}



