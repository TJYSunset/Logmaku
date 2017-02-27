using CommandLine;

namespace Logmaku.Models
{
    public class CommandLineArguments
    {
        [Value(0, Required = true, HelpText = "Path to the target file.", MetaName = "Path")]
        public string FilePath { get; set; }

        [Option('c', "config", HelpText = "Custom path to config file.", Required = false)]
        public string CustomConfigPath { get; set; }
    }
}