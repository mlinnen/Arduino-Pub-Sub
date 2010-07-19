using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace Arduino.PubSubBroker.WinService
{
	[RunInstaller(true)]
	public partial class PublishSubscribeBrokerInstaller : System.Configuration.Install.Installer
	{
		private ServiceProcessInstaller processInstaller;
		private ServiceInstaller serviceInstaller;
		public PublishSubscribeBrokerInstaller()
		{
			InitializeComponent();

			processInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();
			processInstaller.Account = ServiceAccount.LocalSystem;
			serviceInstaller.StartType = ServiceStartMode.Automatic;
			serviceInstaller.ServiceName = "Arduino.PubSubBroker.WinService";
			serviceInstaller.DisplayName = "Message Broker";

		}
	}
}
