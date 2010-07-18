using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.PubSubService;

namespace Arduino.Client.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			PublishSubscribeClient client = new PublishSubscribeClient();
			client.BrokerIp = "127.0.0.1";
			client.BrokerPort = 9999;
			client.MessagePort = 9998;
			client.Connect();

			// Subscribe to the say meessage
			client.Publish("sub","say:127.0.0.1:" + client.MessagePort.ToString());

			while(true)
			{
				System.Console.WriteLine("Enter in a message type for publish (example say):");
				string messageType = System.Console.ReadLine();
				System.Console.WriteLine("Enter in the message body:");
				string messageBody = System.Console.ReadLine();

				System.Console.WriteLine(string.Format("Publishing the following: {0}:{1}",messageType,messageBody));
				client.Publish(messageType,messageBody);

			}
			//publish.Publish("Test", "1 2");
		}
	}
}
