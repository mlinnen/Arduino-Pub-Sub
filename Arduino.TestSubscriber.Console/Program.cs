using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Text;
using Arduino.Contract;

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

			p = null;
			
			Environment.Exit(0);

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

			// Subscribe to the HB message
			Subscription hbMessage = new Subscription("HB", returnIp, client.MessagePort);
			client.Subscribe(hbMessage);

			// Subscribe to the AT message
			Subscription atMessage = new Subscription("AT", returnIp, client.MessagePort);
			client.Subscribe(atMessage);

			System.Console.WriteLine("Press Enter to end program");
			System.Console.ReadLine();

			client.UnSubscribe(hbMessage);

			client.UnSubscribe(atMessage);

		}

		/// <summary>
		/// The call back method for when the Arduino sends the heart beat message
		/// </summary>
		/// <param name="count">The current count from the Arduino</param>
		[Export("HeartBeatProcessor")]
		public void HeartBeatMessageReceived(int deviceNumber, int count)
		{
			System.Console.WriteLine("Heartbeat msg received from device {0} with a count of {1}",deviceNumber, count);
		}
	}
}
