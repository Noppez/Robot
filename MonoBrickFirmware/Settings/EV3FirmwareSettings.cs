﻿using System;
using System.Xml.Serialization;
using System.IO;
using MonoBrickFirmware.Tools;

namespace MonoBrickFirmware.Settings
{

	[XmlRoot("ConfigRoot")]
	public class EV3FirmwareSettings : IFirmwareSettings
	{
		private readonly object readWriteLock = new object();
		private readonly string settingsFileName;
		public EV3FirmwareSettings() :this("/mnt/bootpar/firmwareSettings.xml")
		{
		
		}

		public EV3FirmwareSettings(string settingsFileName)
		{
			this.settingsFileName = settingsFileName;
			GeneralSettings = new GeneralSettings();
			WiFiSettings = new WiFiSettings();
			SoundSettings = new SoundSettings();
			WebServerSettings = new WebServerSettings();

		}

		[XmlElement("GeneralSettings")]
		public GeneralSettings GeneralSettings { get ; set;}

		[XmlElement("WiFiSettings")]
		public WiFiSettings WiFiSettings { get ; set;}

		[XmlElement("SoundSettings")]
		public SoundSettings SoundSettings { get ; set;}

		[XmlElement("WebServerSettings")]
		public WebServerSettings WebServerSettings { get ; set;}

		public virtual bool Save()
		{
			lock (readWriteLock) {
				TextWriter textWriter = null;
				try {
					XmlSerializer serializer = XmlHelper.CreateSerializer(typeof(EV3FirmwareSettings));
					textWriter = new StreamWriter (settingsFileName);
					serializer.Serialize (textWriter, this);
					textWriter.Close ();
					return true;
				} 
				catch (Exception exp) 
				{ 
					Console.WriteLine("Exception during settings save: " + exp.Message);
					Console.WriteLine(exp.StackTrace);
				}
				if(textWriter!= null)
					textWriter.Close();
			}
			return false;
		}

		public virtual void Load()
		{
			lock (readWriteLock) 
			{
				TextReader textReader = null;
				try{
					XmlSerializer deserializer = XmlHelper.CreateSerializer(typeof(EV3FirmwareSettings));
					textReader = new StreamReader (settingsFileName);
					Object obj = deserializer.Deserialize (textReader);
					var loadSettings = (EV3FirmwareSettings)obj;
					textReader.Close ();
					GeneralSettings = loadSettings.GeneralSettings;
					WiFiSettings = loadSettings.WiFiSettings;
					SoundSettings = loadSettings.SoundSettings;
					WebServerSettings = loadSettings.WebServerSettings;
				}
				catch
				{
					Save();
				}
				if(textReader!= null)
					textReader.Close();
			}
		}
	}



}

