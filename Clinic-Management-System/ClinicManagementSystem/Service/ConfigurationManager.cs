using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.Service
{
	public static class ConfigurationManager
	{
		public static IConfiguration AppSetting { get; }
		
		static ConfigurationManager()
		{
			try
			{
				AppSetting = new ConfigurationBuilder()
					.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
					.AddJsonFile("appsettings.json", optional: false)
					.Build();
			}
			catch (Exception ex)
			{
				throw new Exception($"Error initializing configuration: {ex.Message}");
			}
		}
	}
}
