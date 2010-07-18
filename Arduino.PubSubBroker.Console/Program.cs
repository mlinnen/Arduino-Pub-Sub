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

      //SubscriptionService subscriptionService = new SubscriptionService();
      //subscriptionService.Connect();

		PublishSubscribeService service = new PublishSubscribeService();
    	int port = 9999;
		int.TryParse(ConfigurationManager.AppSettings["BrokerPort"], out port);

    	service.Port = port;
		service.Connect();

      System.Console.WriteLine("Press enter to close program");
      System.Console.ReadLine();
    }
  }
}
