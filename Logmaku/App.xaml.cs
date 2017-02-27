using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CommandLine;
using Logmaku.Models;
using Logmaku.Utils;
using YamlDotNet.Serialization;
using static Logmaku.Utils.RandomHelper;

namespace Logmaku
{
    /// <summary>
    ///     App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        public const string ConfigFilename = @".logmaku";
        public static CommandLineArguments Args = new CommandLineArguments();
        public static Config Config;
        public static FileSystemWatcher Watcher;
        public static long LastOffset;
        public static Dictionary<string, BrushPair> PredefinedColors = new Dictionary<string, BrushPair>();
        public static BrushPair DefaultColors;
        public static Regex Pattern;

        public static readonly object Lock = new object();

        private void Main(object sender, StartupEventArgs e)
        {
            var result = Parser.Default.ParseArguments<CommandLineArguments>(Environment.GetCommandLineArgs().Skip(1));
            if (result.Tag == ParserResultType.NotParsed)
                Environment.Exit(1);
            Args = ((Parsed<CommandLineArguments>) result).Value;
            Args.FilePath = Path.GetFullPath(Args.FilePath);
            Config = ParseConfig(Args.CustomConfigPath ?? ConfigFilename);
            DefaultColors = new BrushPair(new SolidColorBrush(Config.DefaultColor),
                new SolidColorBrush(Config.OutlineColor.CorrectAlphaChannel(Config.DefaultColor)));
            foreach (var data in Config.ExplicitColors)
                PredefinedColors.Add(data.Key,
                    new BrushPair(new SolidColorBrush(data.Value),
                        new SolidColorBrush(Config.OutlineColor.CorrectAlphaChannel(data.Value))));
            Pattern = new Regex(Config.LogPattern, RegexOptions.Compiled);
            try
            {
                LastOffset = new FileInfo(Args.FilePath).Length;
            }
            catch
            {
                // Ignored
            }
            Watcher = new FileSystemWatcher(Path.GetDirectoryName(Args.FilePath), Path.GetFileName(Args.FilePath))
            {
                NotifyFilter =
                    NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName |
                    NotifyFilters.DirectoryName,
                EnableRaisingEvents = true
            };
            Watcher.Changed += ReadFile;
            Watcher.Deleted += ReadFile;
            Watcher.Created += ReadFile;
            Watcher.Renamed += ReadFile;
        }

        private static void ReadFile(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            if (!File.Exists(Args.FilePath))
                LastOffset = 0;
            else
                lock (Lock)
                {
                    try
                    {
                        using (var reader = new StreamReader(new FileStream(Args.FilePath,
                            FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Config.Encoding))
                        {
                            if (reader.BaseStream.Length == LastOffset)
                                return;

                            if (reader.BaseStream.Length < LastOffset)
                                LastOffset = 0;

                            reader.BaseStream.Seek(LastOffset, SeekOrigin.Begin);

                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (!Pattern.IsMatch(line)) continue;
                                var level = Pattern.Replace(line, Config.LevelPattern);
                                var log = Pattern.Replace(line, Config.MessagePattern);
                                var pair = PredefinedColors.ContainsKey(level) ? PredefinedColors[level] : DefaultColors;
                                Current.Dispatcher.Invoke(() =>
                                {
                                    new TextWindow
                                    {
                                        Title = log,
                                        Duration = Config.Duration,
                                        FontBrush = pair.Fill,
                                        OutlineBrush = pair.Stroke,
                                        FontFamily = Config.FontFamily,
                                        FontSize = Config.FontSize,
                                        Top = Random(Config.MinYPos, Config.MaxYPos - Config.FontSize),
                                        Left = SystemParameters.PrimaryScreenWidth
                                    }.Show();
                                });
                            }

                            LastOffset = reader.BaseStream.Position;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
        }

        public Config ParseConfig(string path)
        {
            try
            {
                var conf =
                    new DeserializerBuilder().WithTypeConverter(new CustomYamlConverter())
                        .Build()
                        .Deserialize<Config>(File.ReadAllText(path));
                if (!conf.IsValid()) throw new ArgumentException();
                return conf;
            }
            catch (Exception ex)
            {
                WarnInvalidConfig();
                return new Config();
            }
        }

        public void WarnInvalidConfig()
        {
            Console.WriteLine(Logmaku.Properties.Resources.InvalidConfigWarning);
        }

        private void CommandBinding_AlwaysCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Current.Shutdown();
        }

        public struct BrushPair
        {
            public BrushPair(Brush fill, Brush stroke)
            {
                Fill = fill;
                Stroke = stroke;
            }

            public Brush Fill;
            public Brush Stroke;
        }
    }
}