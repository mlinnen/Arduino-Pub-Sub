using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arduino.Contract;
using System.ComponentModel.Composition;

namespace Arduino.AnalogThreshold.MessageProcessors
{

  /// <summary>
  /// This message from the arduino is sent when a specific analog input reading has 
  /// exceeded a threshold
  /// </summary>
  [Export(typeof(IMessageProcessor))]
  public class AnalogThreshholdProcessor:IMessageProcessor
  {
    #region IMessageProcessor Members

    /// <summary>
    /// This message type is AT
    /// </summary>
    public string MessageType
    {
      get { return "AT"; }
    }

    /// <summary>
    /// Tests to be sure that the msg is truely an Analog Threshhold msg 
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public bool ShouldProcess(string msg)
    {
      if (string.IsNullOrEmpty(msg))
      {
        return false;
      }
      else
      {
        string[] parts = msg.Split(' ');

        // Verify the message is of the right type
        if (!parts[0].Equals(MessageType))
        {
          return false;
        }

      }
      return true;
    }

    /// <summary>
    /// Parses the msg and raises an event notifying subscribers that the message was 
    /// received from the device
    /// </summary>
    /// <param name="msg">the data from the device to act uppon</param>
    public void Execute(string msg)
    {
      int pin;
      int threshhold;
      int actualValue;
      string[] parms = msg.Split(' ');
      if (parms != null && parms.Length > 3)
      {
        int.TryParse(parms[1], out pin);
        int.TryParse(parms[2], out threshhold);
        int.TryParse(parms[3], out actualValue);
        Send(pin, threshhold, actualValue);
      }
    }

    #endregion

    [Import("AnalogThreshholdProcessor")]
    public Action<int, int,int> MessageReceived { get; set; }

    private void Send(int pin, int threshhold, int actualValue)
    {
      MessageReceived(pin, threshhold, actualValue);
    }

  }
}
