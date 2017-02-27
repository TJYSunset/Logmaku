using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using YamlDotNet.Serialization;

namespace Logmaku.Models
{
    public class Config
    {
        // Font

        [YamlMember(Alias = "font_family")]
        public FontFamily FontFamily { get; set; } = new FontFamily("Consolas");

        [YamlMember(Alias = "font_size")]
        public double FontSize { get; set; } = 36;

        [YamlMember(Alias = "default_color")]
        public Color DefaultColor { get; set; } = Color.FromArgb(255, 255, 255, 255);

        [YamlMember(Alias = "explicit_colors")]
        public Dictionary<string, Color> ExplicitColors { get; set; } = new Dictionary<string, Color>
        {
            {"DEBUG", Color.FromArgb(192, 255, 255, 255)},
            {"WARN", Color.FromRgb(255, 152, 0)},
            {"ERROR", Color.FromRgb(255, 67, 54)}
        };

        [YamlMember(Alias = "outline_color")]
        public Color OutlineColor { get; set; } = Colors.Black; // TODO implement this

        // Parsing

        [YamlMember(Alias = "encoding")]
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        [YamlMember(Alias = "pattern")]
        public string LogPattern { get; set; } = @"^[\d\-:\/,\s]*?(DEBUG|INFO|WARN|ERROR).*?-\s*(.+)\s*$";

        [YamlMember(Alias = "level_pattern")]
        public string LevelPattern { get; set; } = @"$1";

        [YamlMember(Alias = "message_pattern")]
        public string MessagePattern { get; set; } = @"$2";

        // Animation

        [YamlMember(Alias = "duration")]
        public Duration Duration { get; set; } = new Duration(TimeSpan.FromSeconds(10));

        [YamlMember(Alias = "min_y_position")]
        public double MinYPos { get; set; } = 24;

        [YamlMember(Alias = "max_y_position")]
        public double MaxYPos { get; set; } = SystemParameters.PrimaryScreenHeight - 24; // TODO multi-screen support

        public bool IsValid() => MaxYPos - MinYPos >= FontSize;
    }
}