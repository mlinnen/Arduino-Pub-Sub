#include <Ethernet.h>
#include <stdlib.h>

int x=0;
int count=0;

// Make sure you change the MAC address for each Arduino on the network
byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED }; // Arduino Mac
byte ip[] = { 192, 168, 254, 214 }; // Arduino IP
byte server[] = { 192, 168, 254, 10 }; // Message Broker Ip

// Connect to the broker on a specific port
Client client(server, 9999);

void setup()
{
  Ethernet.begin(mac, ip);
  Serial.begin(9600);
  
  delay(1000);
  
  Serial.println("connecting...");
  
  if (client.connect()) {
    Serial.println("connected");
  } else {
    Serial.println("connection failed");
  }
}

void loop()
{
  count=count+1;
  // Every 5 seconds send a heart beat message
  if (count>4)
  {
    x=x+1;
    count=0;

    // Define the buffer for the count
    char buf[12];
    char myNumberString[6];
    
    // Convert the count x to a string
    itoa(x, buf, 10);
    
    // Define the command buffer
    char str1[14] = "HB:1:"; // Change the 1 to another number to differentiate the heartbeat message from other arduino devices that also send a heartbeat

    // Build the full command which includes the count
    strcat(str1,buf);
    
    // Send the command to the message broker
    client.println(str1);

    if (x==32767)
    {
      x=0;
    }
  }
  
  // Delay for 1 sec
  delay(1000);
}
