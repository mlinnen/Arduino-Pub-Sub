using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace Arduino.PubSubService
{
	public class SubscriptionService
	{
		private TcpListener _tcpListener;
		private Thread _listenThread;
		private Thread _clientThread;

		public SubscriptionService()
		{
			Subscribers = new List<Subscription>();
		}

		[Export("Subscriptions")]
		public List<Subscription> Subscribers { get; set; }

		public void Subscribe(Subscription subscribe)
		{
			// TODO Do some locking
			bool found = false;
			foreach (Subscription subscriber in Subscribers)
			{
				if (subscriber.MessageType.Equals(subscribe.MessageType))
				{
					found = true;
					break;
				}
			}
			if (!found)
				Subscribers.Add(subscribe);
		}

		public void Connect()
		{
			// We simply listen on port 9998
			_tcpListener = new TcpListener(IPAddress.Any, 9998);
			_listenThread = new Thread(new ThreadStart(ListenForClients));
			_listenThread.Start();

		}

		private void ListenForClients()
		{
			_tcpListener.Start();
			while (true)
			{
				TcpClient client = _tcpListener.AcceptTcpClient();

				// create a thread to handle incomming messages
				_clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));

				_clientThread.Start(client);
			}
		}
		private void HandleClientComm(object client)
		{
			TcpClient tcpClient = (TcpClient)client;
			NetworkStream clientStream = tcpClient.GetStream();
			StreamReader reader = new StreamReader(clientStream);
			while (true)
			{
				string data = "";
				try
				{
					// Wait for an entire line to be read
					data = reader.ReadLine();
				}
				catch (Exception ex)
				{
					break;
				}

				Subscription subscription = Subscription.Parse(data);
				if (subscription != null)
				{
					Subscribe(subscription);
				}
			}
			tcpClient.Close();
		}

	}
}
