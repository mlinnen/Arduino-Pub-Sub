using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using Arduino.Contract;
using NUnit.Framework;

namespace Arduino.Processors.UnitTests
{
	[TestFixture]
	public class HeartbeatProcessorFixture
	{
		CompositionContainer _container;
		private int _count = 0;

		[TestFixtureSetUp]
		public void Setup()
		{
			Compose();
		}
		[TestFixtureTearDown]
		public void TearDown()
		{
			
		}

		[Test]
		public void When_ValidHeartbeatMessageArrives_ThenMessagedetailsArePublished()
		{
			_count = 0;
			HeartbeatProcessor processor = new HeartbeatProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType,"1:10");
			processor.Execute(message);
			Assert.AreEqual(1,_count,"The message did not fire");

		}

		[Test]
		public void When_InValidHeartbeatMessageArrives_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			HeartbeatProcessor processor = new HeartbeatProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message("BLAH", "1:10");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidHeartbeatMessageCountArrives_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			HeartbeatProcessor processor = new HeartbeatProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidHeartbeatMessageDeviceNumberContainsCharacters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			HeartbeatProcessor processor = new HeartbeatProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "A:10");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidHeartbeatMessageCountContainsCharacters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			HeartbeatProcessor processor = new HeartbeatProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1:A");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidHeartbeatMessageHasTooManyParms_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			HeartbeatProcessor processor = new HeartbeatProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1:10:blah");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		private void Compose()
		{
			var catalog = new AggregateCatalog(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()), new DirectoryCatalog(@".\"));

			_container = new CompositionContainer(catalog);

			_container.ComposeParts(this);
		}

		[Export("HeartBeatProcessor")]
		public void HeartBeatMessageReceived(int deviceNumber,int count)
		{
			System.Console.WriteLine("Heartbeat msg received for device {0} with a count of {1}",deviceNumber, count);
			_count++;
		}

	}
}
