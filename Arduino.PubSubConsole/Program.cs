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

      SubscriptionService subscriptionService = new SubscriptionService();
      subscriptionService.Connect();

      PublishService publishService = new PublishService();
      publishService.Connect();

      Console.WriteLine("Press enter to close program");
      Console.ReadLine();
    }
  }
}
