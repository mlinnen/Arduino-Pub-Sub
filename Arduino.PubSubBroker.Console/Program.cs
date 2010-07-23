using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.PubSubService;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;

namespace Arduino.PubSubBroker.Console
{
	class Program
	{
		static void Main(string[] args)
		{

			PublishSubscribeService service = new PublishSubscribeService();
			int port = 9999;
			int.TryParse(ConfigurationManager.AppSettings["BrokerPort"], out port);

			service.Port = port;
			service.Connect();

			System.Console.WriteLine("Message Broker Program");
			System.Console.WriteLine("Press enter to close");
			System.Console.ReadLine();

			Environment.Exit(0);
		}
	}
}
