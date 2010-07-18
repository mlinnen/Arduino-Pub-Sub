using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arduino.PubSubService
{
	public class Subscription
	{
		public string MessageType { get; set; }
		public string Ip { get; set; }
		public int Port { get; set; }
		public Subscription(string messageType, string ip,string myPort)
		{
			MessageType = messageType;
			Ip = ip;
			int port = 9999;
			int.TryParse(myPort, out port);
			Port = port;
		}
		public static Subscription Parse(string data)
		{
			if (string.IsNullOrEmpty(data))
				return null;
			string[] parts = data.Split(':');
			if (parts.Length > 2)
			{
				string messageType = parts[0];
				string ip = parts[1];
				return new Subscription(messageType, ip, parts[2]);

			}
			return null;
		}
	}
}
