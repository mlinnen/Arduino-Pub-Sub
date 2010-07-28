using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Arduino.Contract;

namespace Arduino.Processors
{
	/// <summary>
	/// The TemperatureProcessor parses Temperature messages and publishes an event when a valid message is executed.
	/// Message Format: TEMP:deviceNumber:sensorNumber:temperature
	///   Where 
	///		deviceNumber - is an integer that identifies which sensor the reading came from.  Use this to have temperatures that come from multiple arduinos on the network.
	///     sensorNumber - is an integer that identifies which sensor the reading came from on a given device.  Use this to have more than 1 temperature that comes from an arduino.
	///     temperature - is an integer that expresses the temperature in Fahrenheit.  Note the fact that the temp is in Fahrenheit is completely dependent on the arduino code so if 
	///					want Celsius then you can change the arduino code.
	/// </summary>
	[Export(typeof(IMessageProcessor))]
	public class TemperatureProcessor : IMessageProcessor
	{
		#region IMessageProcessor Members

		public string MessageType
		{
			get { return "TEMP"; }
		}

		public void Execute(IMessage msg)
		{
			if (msg != null && !string.IsNullOrEmpty(msg.MessageDetail) && msg.MessageType.Equals(MessageType))
			{
				int deviceNumber = 0;
				int sensorNumber = 0;
				int temp = 0;
				bool success = true;
				string[] parms = msg.MessageDetail.Split(':');
				if (parms.Length == 3)
				{
					if (int.TryParse(parms[0], out deviceNumber) && 
						int.TryParse(parms[1], out sensorNumber) &&
						int.TryParse(parms[2], out temp))
						Send(deviceNumber,sensorNumber,temp);
					
				}
			}
		}

		public bool ShouldProcess(IMessage msg)
		{
			if (msg==null || msg.MessageType==null || msg.MessageType!=MessageType || string.IsNullOrEmpty(msg.MessageDetail))
			{
				return false;
			}
			return true;
		}

		#endregion

		[Import("TemperatureProcessor")]
		public Action<int, int,int> MessageReceived { get; set; }

		public void Send(int deviceNumber,int sensorNumber,int temp)
		{
			MessageReceived(deviceNumber,sensorNumber,temp);
		}

	}
}
