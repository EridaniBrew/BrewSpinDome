// Various routines to handle WiFi reading and writing on the cc3000
// Would like to use the Arduino library for this, but it seems to be crazy buggy
#include <Adafruit_CC3000.h>
#include <SPI.h>
#include "utility/debug.h"
#include "utility/socket.h"

// These are the interrupt and control pins
#define ADAFRUIT_CC3000_IRQ   3  // MUST be an interrupt pin!
// These can be any two pins
#define ADAFRUIT_CC3000_VBAT  5
#define ADAFRUIT_CC3000_CS    10
// Use hardware SPI for the remaining pins
// On an UNO, SCK = 13, MISO = 12, and MOSI = 11
Adafruit_CC3000 cc3000 = Adafruit_CC3000(ADAFRUIT_CC3000_CS, ADAFRUIT_CC3000_IRQ, ADAFRUIT_CC3000_VBAT,
	SPI_CLOCK_DIVIDER); // you can change this clock speed

// Constants for networking
#define WLAN_SSID       "DoubleSlit"           // cannot be longer than 32 characters!
#define WLAN_PASS       "Brew&Grace1552"
// Security can be WLAN_SEC_UNSEC, WLAN_SEC_WEP, WLAN_SEC_WPA or WLAN_SEC_WPA2
#define WLAN_SECURITY   WLAN_SEC_WPA2

#define LISTEN_PORT           1552    // What TCP port to listen on for connections.  The echo protocol uses port 7.

Adafruit_CC3000_Server echoServer(LISTEN_PORT);
//Adafruit_CC3000_ClientRef *gClient;
int gClientIndex = -1;    // keeps track of clientIndex 0..2


// openNet - establish the network connection
int openNet()
{
	trace("Connecting to cc3000");
	if (!cc3000.begin())
	{
		// error initializing
		trace("Error initializing cc3000");
		return 0;
	}

	if (!cc3000.connectToAP(WLAN_SSID, WLAN_PASS, WLAN_SECURITY)) {
		trace("Could not connect to DoubleSlit");
		return 0;
	}

	//while (!cc3000.checkDHCP())
	//{
	//	delay(100); // ToDo: Insert a DHCP timeout!
	//}

	// start listening for connections
	echoServer.begin();
	
	return 1;   // open is OK
}

// readCommand - retrieve the next command for the shutters
// buf gets filled in with the string
// returns the number of characters read
int readCommand(char *buf, int buflen)
{
	int count = 0;					// if no client is connected, or no data available from client, return 0 bytes

	// does not block
	//trace("Check echoServer.available");
	Adafruit_CC3000_ClientRef client = echoServer.available();
	//trace("  returned");
	//gClient = &client;
	if (client) {
		// Check if there is data available to read.
		//trace("Client found");
                int clientIdx = -1;
                clientIdx = echoServer.availableIndex(NULL);
                //trace(clientIdx);
		if (client.available() > 0) {
			//trace("  has data");
			count = client.read(buf, buflen);
                        ClearOtherClients(clientIdx);
                        gClientIndex = clientIdx;
		}
	}

	return count;
}

// writeBuf - write the string back to the network
void writeBuf(String s)
{
	char b[100];
	s.toCharArray(b, s.length()+1);
	//trace("writeBuf: Writing buf");
        if (gClientIndex > -1){
            Adafruit_CC3000_ClientRef client = echoServer.getClientRef (gClientIndex);
	    client.write(b, s.length()+1);
	    client.flush();
	    trace(s);
        }
	
}

// This routine attempts to fix a "problem"
// The Server class provides 3 clients to be running. So, each time
// the client connects it uses one of the client slots.
// However, when a client disconnects the slot is not cleared. Eventually all 3 
// slots are filled; subsequent connection requests will not be completed.
// My solution:
// I expect only one client at a time to be connected.
// So, when read data is available on a client, at that time I look for other clients
// that are "active" and close them. Don't close the currentClient!
void ClearOtherClients(int currentClient)
{
  int i;
  if (currentClient > -1){
      for (i=0; i<3; i++){
        Adafruit_CC3000_ClientRef client = echoServer.getClientRef (i);
        if (i != currentClient){
            if (client){
                if (client.connected()){
                    trace("Would close client ");
                    trace(i);
                    trace("   Current client is ");
                    trace (currentClient);
                    //client.close();
                }
            }
        }
      }
  }
}
