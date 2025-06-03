using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Flow.Launcher.Plugin;
using Microsoft.Win32;

namespace Flow.Launcher.Plugin.FreeTube
{
    public class FreeTube : IPlugin
    {
        private PluginInitContext _context;
        private const string Image = "Images/freetube.ico";
        private const string FreetubeRegistryKeyPath = "freetube\\shell\\open\\command";
        private string? FreetubePath;

        public void Init(PluginInitContext context)
        {
            _context = context;

            FreetubePath = LoadFreetubePathFromRegistry();
        }

        public List<Result> Query(Query query)
        {
            var results = new List<Result>();
            string url = query.Search;
            if (FreetubePath == null)
            {
                Result noPathResult = new()
                {
                    Title = "FreeTube cannot be found",
                    SubTitle = "Verify that FreeTube is installed, or set the path manually in the plugin settings.",
                    Score = 0,
                    IcoPath = Image,
                };
                results.Add(noPathResult);
                return results;
            }

            if (String.IsNullOrEmpty(url))
            {
                return results;
            }

            Result newWindowResult = new()
            {
                Title = "Open with new instance",
                SubTitle = "Open in FreeTube. If an instance exists, create a new instance",
                IcoPath = Image,
                Score = 1,
                Action = c =>
                {
                    var freetubeCommand = SharedCommands.ShellCommand.SetProcessStartInfo(FreetubePath, "", $"--new-window {url}");
                    freetubeCommand.UseShellExecute = true;
                    SharedCommands.ShellCommand.Execute(freetubeCommand);
                    return true;
                }
            };
            Result existingWindowResult = new()
            {
                Title = "Open with no new instances",
                SubTitle = "Open in Freetube. If an instance exists, open within that instance",
                IcoPath = Image,
                Score = 0,
                Action = c =>
                {
                    var freetubeCommand = SharedCommands.ShellCommand.SetProcessStartInfo(FreetubePath, "", $"{url}");
                    freetubeCommand.UseShellExecute = true;
                    SharedCommands.ShellCommand.Execute(freetubeCommand);
                    return true;
                }
            };
            results.Add(newWindowResult);
            results.Add(existingWindowResult);
            return results;
        }

        private string? LoadFreetubePathFromRegistry()
        {
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(FreetubeRegistryKeyPath);
            if (regKey == null)
            {
                return null;
            }

            object? keyValue = regKey.GetValue(null);
            if (keyValue == null)
            {
                return null;
            }

            string? valueString = keyValue.ToString();
            if (valueString == null)
            {
                return null;
            }

            string[] commandComponents = valueString.Split(" ");
            if (commandComponents.Length < 1)
            {
                return null;
            }

            return commandComponents[0].Replace("\"", "");
        }
    }
}