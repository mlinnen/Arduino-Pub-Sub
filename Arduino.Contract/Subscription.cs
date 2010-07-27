using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arduino.Contract
{
	public class Subscription
	{
		public string MessageType { get; set; }
		public string Ip { get; set; }
		public int Port { get; set; }

		#region ctor
		public Subscription(string messageType, string ip,string myPort)
		{
			MessageType = messageType;
			Ip = ip;
			int port = 9999;
			int.TryParse(myPort, out port);
			Port = port;
		}

		public Subscription(string messageType, string ip, int myPort)
		{
			MessageType = messageType;
			Ip = ip;
			Port = myPort;
		}
		#endregion

		/// <summary>
		/// Gets a subscription message that is ready to send to the broker
		/// </summary>
		public string SubscribeMessage
		{
			get { return string.Format("sub:{0}:{1}:{2}",MessageType,Ip,Port); }
		}

		/// <summary>
		/// Gets an unsubscribe message that is ready to send to the broker
		/// </summary>
		public string UnsubSubscribeMessage
		{
			get { return string.Format("unsub:{0}:{1}:{2}", MessageType, Ip, Port); }
		}

		/// <summary>
		/// Helper for parsing the subscription data
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
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
