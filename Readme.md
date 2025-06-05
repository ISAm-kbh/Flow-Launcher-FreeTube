FreeTube for Flow Launcher
==================

A plugin for the [Flow launcher](https://github.com/Flow-Launcher/Flow.Launcher).

The plugin allows users to open urls in [FreeTube](https://github.com/FreeTubeApp/FreeTube).

Note that the plugin needs FreeTube installed in order to work.

<!--toc:start-->
- [FreeTube for Flow Launcher
==================](#freetube-for-flow-launcher)
- [Usage](#usage)
  - [Window Opening Strategy](#window-opening-strategy)
- [Settings](#settings)
  - [Primary Window Opening Strategy](#primary-window-opening-strategy)
  - [FreeTube Executable Path](#freetube-executable-path)
- [Contributions](#contributions)
- [Attributions](#attributions)
<!--toc:end-->

# Usage

Usage is simple, simply invoke the keyword associated with the plugin (the default is `ft`),
along with a youtube URL you wish FreeTube to open.

    ft <url>

As long as FreeTube is installed, and can be located by the plugin, the plugin will call FreeTube and give it the URL.

Once FreeTube opens, it will immediately load the video pointed to by the URL, if it is accessible.

If the URL is inaccessible, FreeTube will relay those errors.

If you pass a string to FreeTube that isn't a URL it can parse, it will simply open to whatever statup page you have configured.

## Window Opening Strategy

Upon entering the url, there will be two options available for selection.

 1. "Open in new window"

 2. "Open in no new window"

This is because the FreeTube application can be opened in 2 different ways. "Open in new window" means that the new video will always create a new FreeTube window. "Open in no new window" also will open a FreeTube window if FreeTube isn't open yet. *But,* if there is one or many FreeTube windows already open, FreeTube will choose one of those windows to open the video in. 

# Settings

## Primary Window Opening Strategy

By default, "Open in new window" will be the primary opening strategy, or the one at the top of the result list.

If you want to make it so that "Open in no new window" is the primary opening strategy, then you can select "Existing instance" in the settings for the plugin under "Primary Open Strategy".

## FreeTube Executable Path

The plugin will try it's best to find the FreeTube executable based on certain assumptions on how FreeTube is usually installed. If the plugin cannot find FreeTube, it will tell you.

If you know that FreeTube is installed on your machine, and the plugin cannot find it, you can manually give the path to the executable in the settings.

Simply check "Manually specify FreeTube executable path", and enter the path into the textbox.

If the plugin *still* can't find or open FreeTube, double-check that your path is an accurate, correctly-formatted, absoulte path.

# Contributions

This is a small project, and all of it's functionality is finished. Since I actively use this plugin, I will maintain it in the face of any bugs that may pop up.

However, I will look at pull-requests if you want to add any functionalities you think are missing.

I will add more comments to the codebase soon.

Contributitions in the form of translations are the most welcome!

# Attributions

This project uses an icon from FreeTube, an open-source project with it's own licensing. However, this plugin is no affiliated with FreeTube in any way.

Big thanks to both the creators of FreeTube and Flow Launcher for their amazing open-source desktop products.
