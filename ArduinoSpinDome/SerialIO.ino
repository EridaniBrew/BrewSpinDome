
// readCommand
// reads string up to #  NOTE - the # is not in the string
// returns number of bytes read
// fills in inCommand and (optional) parm 1 & parm2
unsigned int readCommand(char *inCommand, char *parm1, char *parm2, int maxLength)
{
  unsigned int readCount = 0;
  String sBuf = "";
  
  if (Serial.available() > 0)
      {
      //traceln(Serial.available());
      sBuf = Serial.readStringUntil('#');
      readCount = sBuf.length();
//      trace(F("avail "));
//      trace (readCount);
//      traceln(sBuf.c_str());
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
  strcpy (parm1, "");
  strcpy (parm2, "");
  //trace(F("ReadCommand sBuf is {"));
  //trace(sBuf);
  //traceln(F("}"));
  if (sBuf.length() == 0)
      return(0);
  
  char sz[100];
  char *str;
  char *p = sz;
  sBuf.getBytes((unsigned char *)sz, sBuf.length() +1);
  if ((str = strtok_r(p, " ", &p)) != NULL) 
      {
      strcpy (inCommand, str);
      if ((str = strtok_r(p, " ", &p)) != NULL)
          {
          strcpy (parm1, str);
          if ((str = strtok_r(p, " ", &p)) != NULL)
              {
              strcpy (parm2, str);
              }
          }
      }
      
  return (readCount);
}


void writeBuf(char *s)
{
    Serial.println(s);
    //Serial.flush();
}
