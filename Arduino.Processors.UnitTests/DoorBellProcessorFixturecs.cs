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
	public class DoorBellProcessorFixturecs
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
		public void When_ValidDoorBellMessageArrives_ThenMessagedetailsArePublished()
		{
			_count = 0;
			DoorBellProcessor processor = new DoorBellProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1");
			processor.Execute(message);
			Assert.AreEqual(1, _count, "The message did not fire");

		}

		[Test]
		public void When_DoorBellMessageContainsTooManyParameters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			DoorBellProcessor processor = new DoorBellProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1:BLAH");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message did fire and it should not have");

		}

		[Test]
		public void When_InValidDoorbellMessageTypeArrives_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			DoorBellProcessor processor = new DoorBellProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message("BLAH", "10");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidDoorbellMessageDetailExists_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			DoorBellProcessor processor = new DoorBellProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidDoorbellMessageDoorNumberContainsCharacters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			DoorBellProcessor processor = new DoorBellProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "A");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		private void Compose()
		{
			var catalog = new AggregateCatalog(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()), new DirectoryCatalog(@".\"));

			_container = new CompositionContainer(catalog);

			_container.ComposeParts(this);
		}

		[Export("DoorBellProcessor")]
		public void DoorbellMessageReceived(int doorNumber)
		{
			System.Console.WriteLine("Doorbell msg received with a door number of {0}", doorNumber);
			_count++;
		}
	}
}
