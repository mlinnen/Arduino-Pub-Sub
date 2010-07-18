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
		public Subscription(string messageType, string ip)
		{
			MessageType = messageType;
			Ip = ip;
		}
		public static Subscription Parse(string data)
		{
			if (string.IsNullOrEmpty(data))
				return null;
			string[] parts = data.Split(':');
			if (parts.Length > 1)
			{
				string messageType = parts[0];
				string ip = parts[1];
				return new Subscription(messageType, ip);

			}
			return null;
		}
	}
}
