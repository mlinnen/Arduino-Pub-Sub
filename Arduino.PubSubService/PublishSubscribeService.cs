using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arduino.PubSubService
{
	public class PublishSubscribeService
	{
		public List<Subscription> Subscribers { get; set; }

		public void Subscribe(string message)
		{
			// TODO parse the message

			// TODO check to be sure the message is valid

			// TODO subscribe to the message
		}

		public void Subscribe(Subscription subscribe)
		{
			// TODO Do some locking
			bool found = false;
			foreach (Subscription subscriber in Subscribers)
			{
				if (subscriber.MessageType.Equals(subscribe.MessageType) && subscriber.Ip.Equals(subscribe.Ip))
				{
					found = true;
					break;
				}
			}
			if (!found)
				Subscribers.Add(subscribe);
		}

		public void UnSubscribe(string message)
		{
			// TODO parse the message

			// TODO check to be sure the message is valid

			// TODO UnSubscribe from the message
		}

		public void UnSubscribe(Subscription subscribe)
		{
			// TODO Do some locking
			foreach (Subscription subscriber in Subscribers)
			{
				if (subscriber.MessageType.Equals(subscribe.MessageType) && subscriber.Ip.Equals(subscribe.Ip))
				{
					Subscribers.Remove(subscriber);
					break;
				}
			}
		}

		public void Publish(string message)
		{
			// TODO parse the message
			string[] parts = message.Split(':');

			// TODO check to be sure the message is valid
			if (parts.Length != 2)
				return;

			string messageType = parts[0];
			if (messageType.Equals("sub"))
			{
				
			}


			// TODO check to see if this is a subscription message

			// If it is a subscription message then subscribe to it otherwise forward the message to all subscribers
		}
	}
}
