using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;

namespace PhamHuynhSumWPF.Helpers
{
    public static class Config
    {
        private static IConfigurationRoot? _root;
        public static IConfigurationRoot Root
        {
            get
            {
                if (_root != null) return _root;
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                _root = builder.Build();
                return _root;
            }
        }


        public static string AdminEmail => Root["AdminAccount:Email"] ?? string.Empty;
        public static string AdminPassword => Root["AdminAccount:Password"] ?? string.Empty;
    }
}
