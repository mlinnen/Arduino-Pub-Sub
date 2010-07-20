using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Arduino.PubSubConnector;

namespace Arduino.Logger.Console
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
			// TODO add in some way to load subscriptions from a file instead of changing the code or subscribing to all messages
			client.Publish("sub", "say:" + returnIp + ":" + client.MessagePort.ToString());

			System.Console.WriteLine("This is a simple data logger that logs messages that it is subscribed to");
			System.Console.ReadLine();

		}
	}
}
