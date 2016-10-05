
#define BlockTic 2
#define BlockTicLED 9
boolean volatile blockLEDstate = false;
#define BLOCK_INTERRUPT 0

#define EnableBlockInterrupt  attachInterrupt(BLOCK_INTERRUPT, bumpPosInterrupt, CHANGE)
#define DisableBlockInterrupt  detachInterrupt(BLOCK_INTERRUPT)

// some timing values
unsigned long volatile lastIntTime = 0;
unsigned long lastLoopTime = 0;

int blkPos = 0;
// interrupt for the position sensor. Gets called on both Rising and Falling voltage
void bumpPosInterrupt()
{
    digitalWrite(BlockTicLED, HIGH );
    blkPos++;
    
    // toggle blockLED
    blockLEDstate = ! blockLEDstate;
    digitalWrite(BlockTicLED,  LOW);
}



void setup()
{
  Serial.begin(115200);
  
  pinMode (BlockTic, INPUT_PULLUP);
  
  pinMode(BlockTicLED,OUTPUT);
  digitalWrite (BlockTicLED, LOW);
  
  EnableBlockInterrupt;
  
}

void loop()                     // run over and over again
{
    //DisableBlockInterrupt;
    int count = blkPos;    // use copy of blkPos
    blkPos = 0;
    //EnableBlockInterrupt;
    Serial.print("Count ");
    Serial.println(count);
    
    delay(5000);
}
