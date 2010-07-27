using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Arduino.Contract;

namespace Arduino.Processors
{
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
				int count = 0;
				string[] parms = msg.MessageDetail.Split(':');
				int.TryParse(parms[0], out count);
				Send(count);
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
		public Action<int> MessageReceived { get; set; }

		public void Send(int count)
		{
			MessageReceived(count);
		}


	}
}
