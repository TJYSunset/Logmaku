# Logmaku

[![Build status]( 	https://img.shields.io/appveyor/ci/TJYSunset/Logmaku.svg?style=flat)](https://ci.appveyor.com/project/TJYSunset/Logmaku)
[![GitHub license](https://img.shields.io/badge/license-WTFPL-brightgreen.svg)](https://raw.githubusercontent.com/TJYSunset/Logmaku/master/LICENSE)

Somewhat log monitor in danmaku style.
弹幕样式的日志监视器。

## What's "Danmaku" anyway?

Something like [Niconico](http://www.nicovideo.jp)'s featured commenting system, in which comments fly through the screen in real-time.

## Usage

```
$ logmaku [-c /path/to/config/file] filename
```

## Configuration

Either put a `.logmaku` file at the same folder of logmaku.exe, or specify the path explicitly in the arguments.

If fails to load/parse configuration file, Logmaku will use the default configuration(the same as the example below). Any mistake in the configuration file will lead to parse failure, so be careful!

The config file is in YAML format. Here's one example:

```yaml
# All items are optional

# Font related
font_family: Consolas
font_size: 36
default_color: White # You can use string for colors
explicit_colors: # ARGB & RGB format are also acceptable
    DEBUG: '#C0FFFFFF'
    WARN:  '#FF9800'
    ERROR: '#FF4336'
outline_color: Black # Not implemented yet

# Parsing related
encoding: 'UTF-8'
pattern: '^[\d\-:\/,\s]*?(DEBUG|INFO|WARN|ERROR).*?-\s*(.+)\s*$' # .NET-flavored regular expression
level_pattern: '$1'
message_pattern: '$2'

# Animation related
duration: 00:00:10 # Strings are okay
min_y_position: 24
max_y_position: 744
```

## What's not implemented

- ~~Everything~~
- Multi-monitor support
- Text outline
- Tray icon (maybe later)

## Why you should *not* use this

This project is for personal purpose, and is not fully tested, and the code is a mess, and its author is a ❤❤❤❤ing ❤❤❤❤❤❤... Oh for heaven's sakes, why are you even still reading this gibberish? Why can't you just press <kbd>Ctrl</kbd>+<kbd>W</kbd> and forget about this cr