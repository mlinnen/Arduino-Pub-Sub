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
      publish.Publish("Test", "1 2");
    }
  }
}
