using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arduino.Contract
{
	/// <summary>
	/// The inteface all message processors have to impliment
	/// </summary>
	public interface IMessageProcessor
	{
		/// <summary>
		/// A unique string that distinguishes the message processor from all other message processors.
		/// This should match what is comming from the device
		/// </summary>
		string MessageType { get; }

		/// <summary>
		/// This is called once it is determined that the message processor is a match for the data that 
		/// came from the device
		/// </summary>
		/// <param name="msg"></param>
		void Execute(string msg);

		/// <summary>
		/// A simple test to see if the msg is for this message processor
		/// </summary>
		/// <param name="msg"></param>
		/// <returns>true if this data should be processed by this message processor</returns>
		bool ShouldProcess(string msg);
	}
}
