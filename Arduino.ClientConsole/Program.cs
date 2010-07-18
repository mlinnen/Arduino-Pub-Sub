using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.PubSubService;

namespace Arduino.ClientConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			PublishProxyService publish = new PublishProxyService();

			// Subscribe to the say meessage
			publish.Publish("sub","say:127.0.0.1");

			while(true)
			{
				Console.WriteLine("Enter in a message type for publish (example say):");
				string messageType = Console.ReadLine();
				Console.WriteLine("Enter in the message body:");
				string messageBody = Console.ReadLine();

				Console.WriteLine(string.Format("Publishing the following: {0}:{1}",messageType,messageBody));
				publish.Publish(messageType,messageBody);

			}
			//publish.Publish("Test", "1 2");
		}
	}
}
