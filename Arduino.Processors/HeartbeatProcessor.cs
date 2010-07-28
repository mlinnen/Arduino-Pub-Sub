using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Arduino.Contract;

namespace Arduino.Processors
{
	/// <summary>
	/// The HeartbeatProcessor parses Heartbeat messages and publishes and event when a successful message is received 
	/// Message Format: HB:deviceNumber:Count
	///   Where 
	///		deviceNumber - is an integer that identifies which sensor the reading came from.  Use this to have heartbeats that come from multiple arduinos on the network.
	///		Count - is a 16 bit integer that starts at 1 and is incremented every heartbeat until it reaches 32,767 in which case on the next heartbeat it will become 1
	/// </summary>
	[Export(typeof(IMessageProcessor))]
	public class HeartbeatProcessor : IMessageProcessor
	{
		public HeartbeatProcessor()
		{
			
		}

		#region IMessageProcessor Members

		public string MessageType
		{
			get { return "HB"; }
		}

		public void Execute(IMessage msg)
		{
			if (msg != null && !string.IsNullOrEmpty(msg.MessageDetail) && msg.MessageType.Equals(MessageType))
			{
				int deviceNumber = 0;
				int count = 0;
				string[] parms = msg.MessageDetail.Split(':');
				if (parms.Length == 2)
				{
					if (int.TryParse(parms[0], out deviceNumber) &&
						int.TryParse(parms[1], out count))
						Send(deviceNumber,count);
				}
			}
		}

		public bool ShouldProcess(IMessage msg)
		{
			if (msg==null || string.IsNullOrEmpty(msg.MessageDetail) || string.IsNullOrEmpty(msg.MessageType) || !msg.MessageType.Equals(MessageType))
			{
				return false;
			}
			return true;
		}

		#endregion

		[Import("HeartBeatProcessor")]
		public Action<int,int> MessageReceived { get; set; }

		public void Send(int deviceNumber, int count)
		{
			MessageReceived(deviceNumber,count);
		}


	}
}
