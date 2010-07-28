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
	public class TemperatureProcessorFixture
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
		public void When_ValidTemperatureMessageArrives_ThenMessagedetailsArePublished()
		{
			_count = 0;
			TemperatureProcessor processor = new TemperatureProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1:2:38");
			processor.Execute(message);
			Assert.AreEqual(1, _count, "The message did not fire");

		}

		[Test]
		public void When_InValidTemperatureMessageArrives_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			TemperatureProcessor processor = new TemperatureProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message("BLAH", "1:2:38");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidTemperatureMessageDetailExists_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			TemperatureProcessor processor = new TemperatureProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidTemperatureMessageDeviceNumberContainsCharacters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			TemperatureProcessor processor = new TemperatureProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "A:2:38");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}


		[Test]
		public void When_InValidTemperatureMessageSensorNumberContainsCharacters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			TemperatureProcessor processor = new TemperatureProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1:A:38");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidTemperatureMessageTempValueContainsCharacters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			TemperatureProcessor processor = new TemperatureProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1:2:A");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		[Test]
		public void When_InValidTemperatureMessageContainsTooManyCharacters_ThenMessagedetailsAreNotPublished()
		{
			_count = 0;
			TemperatureProcessor processor = new TemperatureProcessor();
			_container.ComposeParts(processor);
			IMessage message = new Message(processor.MessageType, "1:2:38:blah");
			processor.Execute(message);
			Assert.AreEqual(0, _count, "The message fired and it should not have");

		}

		private void Compose()
		{
			var catalog = new AggregateCatalog(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()), new DirectoryCatalog(@".\"));

			_container = new CompositionContainer(catalog);

			_container.ComposeParts(this);
		}

		[Export("TemperatureProcessor")]
		public void TemperatureMessageReceived(int deviceNumber, int sensorNumber,int temp)
		{
			System.Console.WriteLine("Temperature msg received for device {0} with a sensor number of {1} and temp of {2}", deviceNumber, sensorNumber,temp);
			_count++;
		}

	}
}
