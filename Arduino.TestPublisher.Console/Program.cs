using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Arduino.PubSubConnector;

namespace Arduino.TestPublisher.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			PublishSubscribeClient client = new PublishSubscribeClient();
			client.BrokerIp = ConfigurationManager.AppSettings["BrokerIp"];
			client.BrokerPort = int.Parse(ConfigurationManager.AppSettings["BrokerPort"]);
			client.Connect();

			System.Console.WriteLine("All the program does is publish messages to the message broker");

			while (true)
			{
				System.Console.WriteLine("Enter in a message type to publish (example say):");
				string messageType = System.Console.ReadLine();
				if (string.IsNullOrEmpty(messageType))
					break;
				System.Console.WriteLine("Enter in the message body:");
				string messageBody = System.Console.ReadLine();

				System.Console.WriteLine(string.Format("Publishing the following: {0}:{1}", messageType, messageBody));
				client.Publish(messageType, messageBody);

			}
	

		}
	}
}
