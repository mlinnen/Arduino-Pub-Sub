using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Text;
using Arduino.PubSubConnector;

namespace Arduino.TestSubscriber.Console
{
	class Program
	{
		CompositionContainer _container;
		static void Main(string[] args)
		{
			Program p = new Program();

			// Get MEF to wire up all the dependencies
			p.Compose();

			p.Run();

		}
		private bool Compose()
		{
			var catalog = new AggregateCatalog(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()), new DirectoryCatalog(@".\"));

			_container = new CompositionContainer(catalog);

			_container.ComposeParts(this);
			return true;
		}

		private void Run()
		{
			PublishSubscribeClient client = new PublishSubscribeClient();
			client.BrokerIp = ConfigurationManager.AppSettings["BrokerIp"];
			client.BrokerPort = int.Parse(ConfigurationManager.AppSettings["BrokerPort"]);
			client.MessagePort = int.Parse(ConfigurationManager.AppSettings["MessagePort"]);
			string returnIp = ConfigurationManager.AppSettings["MessageIp"];
			client.Connect();

			// TODO use MEF to determine all the processors to subscribe for 
			// Subscribe to the hb message
			client.Publish("sub", "hb:" + returnIp + ":" + client.MessagePort.ToString());

			System.Console.WriteLine("Press Enter to end program");
			System.Console.ReadLine();

		}

		/// <summary>
		/// The call back method for when the Arduino sends the heart beat message
		/// </summary>
		/// <param name="count">The current count from the arduino</param>
		[Export("HeartBeatProcessor")]
		public void HeartBeatMessageReceived(int count)
		{
			System.Console.WriteLine("Heartbeat msg received with a count of {0}", count);
		}
	}
}
