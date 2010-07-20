#include <Ethernet.h>
#include <stdlib.h>

int x=0;
int count=0;
int analogValue;
int threshHold=800;
int analogPin=0;

byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
byte ip[] = { 192, 168, 254, 214 };
byte server[] = { 192, 168, 254, 10 }; // Message Broker Ip

Client client(server, 9999);

void setup()
{
  Ethernet.begin(mac, ip);
  Serial.begin(9600);
  
  delay(1000);
  
  Serial.println("connecting...");
  
  if (client.connect()) {
    Serial.println("connected");
    //client.println("GET /search?q=arduino HTTP/1.0");
    //client.println();
  } else {
    Serial.println("connection failed");
  }
  }

void loop()
{
  // Read the sensor value
  analogValue = analogRead(analogPin);
  
  // Test for thresh hold
  if (analogValue>threshHold)
  {
    //Serial.print("AT ");
    //Serial.print(analogPin);
    //Serial.print(" ");
    //Serial.print(threshHold);
    //Serial.print(" ");
    //Serial.print(analogValue);
    //Serial.println("");
  }
  
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
    char str1[14] = "HB:";

    // Build the full command which includes the count
    strcat(str1,buf);
    
    // Send the command to the message broker
    client.println(str1);
  }
  
  // Delay for 1 sec
  delay(1000);
}
