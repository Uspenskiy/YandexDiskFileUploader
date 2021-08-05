using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace YandexDiskFileUploader.Extensions
{
    public static class CommandLineExtensions
    {
        public static IConfigurationBuilder AddCommandLineParam(this IConfigurationBuilder configurationBuilder, string[] args)
        {
            configurationBuilder.Build();
            var arguments = new List<string>();
            if (args.Length == 2)
            {
                arguments.Add("--s=" + args[0]);
                arguments.Add("--d=" + args[1]);
            };
            return configurationBuilder.Add(new CommandLineConfigurationSource { Args = arguments });
        }
    }
}
