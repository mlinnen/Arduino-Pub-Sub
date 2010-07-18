using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.PubSubService;
using System.ComponentModel.Composition.Hosting;

namespace Arduino.PubSubBroker.Console
{
  class Program
  {
    static void Main(string[] args)
    {

      //SubscriptionService subscriptionService = new SubscriptionService();
      //subscriptionService.Connect();

		PublishSubscribeService service = new PublishSubscribeService();
		service.Connect();

      System.Console.WriteLine("Press enter to close program");
      System.Console.ReadLine();
    }
  }
}
