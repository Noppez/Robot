﻿using System;
using System.IO;
using System.Net;
using MonoBrickFirmware.FirmwareUpdate;

namespace DownloadInstallerTest
{
	class MainClass
	{
		
		private static bool DownloadFile(string file, string url, string downloadPath)
		{
			bool ok = true;
			try
			{
				if(!Directory.Exists(downloadPath))
					Directory.CreateDirectory(downloadPath);
				new WebClient ().DownloadFile (Path.Combine(url,file), Path.Combine(downloadPath,file));
			}
			catch(Exception e)
			{

				Console.WriteLine(e.Message);
				ok = false;
			}
			return ok;
		}

		private static bool DownloadAndInstallPackage(string packageName, string packageUrl, string downloadPath)
		{
			bool ok = true;
			InstallPackage installPackage = new InstallPackage();
			ok = DownloadFile(packageName, packageUrl, downloadPath);
			if(ok)
			{
				try
				{
					installPackage = installPackage.LoadFromXML(Path.Combine(downloadPath,packageName));
				}
				catch(Exception e)
				{
					Console.WriteLine (e.Message);
					ok = false;
				}
				if(ok)
				{
					var downloadElements = installPackage.DownloadElementToArray();
					foreach(var element in downloadElements)
					{
						ok = DownloadFile(element.FileName, Path.Combine(packageUrl, element.Subdir), Path.Combine(downloadPath, element.Subdir));
						if(!ok)
						{
							break;
						}
					}
				}
			}
			return ok;
		}

		public static void Main (string[] args)
		{
			DownloadAndInstallPackage("package.xml", "ftp://soborg.net/test/StartupApp/", Path.Combine(Directory.GetCurrentDirectory(),"download"));		
		}
	}
}
