using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Arduino.Contract;

namespace Arduino.Heartbeat.MessageProcessor
{
	[Export(typeof(IMessageProcessor))]
	public class HeartbeatProcessor : IMessageProcessor
	{
		#region IMessageProcessor Members

		public string MessageType
		{
			get { return "HB"; }
		}

		public void Execute(string msg)
		{
			int count;
			string[] parms = msg.Split(' ');
			if (parms != null && parms.Length > 1)
			{
				int.TryParse(parms[1], out count);
				Send(count);
			}
		}

		public bool ShouldProcess(string msg)
		{
			if (string.IsNullOrEmpty(msg))
			{
				return false;
			}
			else
			{
				string[] parts = msg.Split(':');

				if (parts == null)
					return false;

				// Verify the message is of the right type
				if (!parts[0].Equals(MessageType))
				{
					return false;
				}

			}
			return true;
		}

		#endregion

		[Import("HeartBeatProcessor")]
		public Action<int> MessageReceived { get; set; }

		public void Send(int count)
		{
			MessageReceived(count);
		}
	}
}
