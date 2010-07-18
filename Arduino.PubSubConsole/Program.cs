using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.PubSubService;
using System.ComponentModel.Composition.Hosting;

namespace Arduino.PubSubConsole
{
  class Program
  {
    static void Main(string[] args)
    {

      //SubscriptionService subscriptionService = new SubscriptionService();
      //subscriptionService.Connect();

		PublishSubscribeService service = new PublishSubscribeService();
		service.Connect();

      Console.WriteLine("Press enter to close program");
      Console.ReadLine();
    }
  }
}
