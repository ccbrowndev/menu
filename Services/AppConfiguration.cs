using System;
using System.Collections.Generic;
using System.Text;
using menu.Domain;
using Microsoft.Extensions.Configuration;
using System.IO;
using menu.Services;

namespace UserSchedule.Services
{
    public class AppConfiguration : IAppConfiguration
    {
        private const string DatabaseNameKey = "DatabaseName";
        private const string DefaultDatabaseName = "MenuLocalDatabase.db";
        private readonly IConfiguration configuration;

        public AppConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GetConnectionString()
        {
            return configuration.GetConnectionString("MenuLocalDatabase");
        }

        public string DatabaseName
        {
            get
            {
                string databaseName = configuration[DatabaseNameKey];
                if (string.IsNullOrEmpty(databaseName))
                {
                    databaseName = DefaultDatabaseName;
                }
                return Path.Combine(FileSystem.AppDataDirectory, databaseName);
            }
        }
    }
}
