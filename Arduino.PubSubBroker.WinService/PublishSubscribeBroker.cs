using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Arduino.PubSubService;
using System.Configuration;

namespace Arduino.PubSubBroker.WinService
{
	partial class PublishSubscribeBroker : ServiceBase
	{
		PublishSubscribeService service = new PublishSubscribeService();
		public PublishSubscribeBroker()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			int port = 9999;
			int.TryParse(ConfigurationManager.AppSettings["BrokerPort"], out port);
			service.Port = port;
			service.Connect();
		}

		protected override void OnStop()
		{
			if (service!=null)
				service.Disconnect();
		}
	}
}
