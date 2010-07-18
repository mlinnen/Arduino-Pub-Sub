using System;
using System.Collections.Generic;
using System.Configuration;
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
			client.BrokerIp = ConfigurationManager.AppSettings["BrokerIp"];
			client.BrokerPort = int.Parse(ConfigurationManager.AppSettings["BrokerPort"]);
			client.MessagePort = int.Parse(ConfigurationManager.AppSettings["MessagePort"]);
			string returnIp = ConfigurationManager.AppSettings["MessageIp"];
			client.Connect();

			// Subscribe to the say message
			client.Publish("sub","say:" + returnIp + ":" + client.MessagePort.ToString());

			while(true)
			{
				System.Console.WriteLine("Enter in a message type for publish (example say):");
				string messageType = System.Console.ReadLine();
				System.Console.WriteLine("Enter in the message body:");
				string messageBody = System.Console.ReadLine();

				System.Console.WriteLine(string.Format("Publishing the following: {0}:{1}",messageType,messageBody));
				client.Publish(messageType,messageBody);

			}
		}
	}
}
