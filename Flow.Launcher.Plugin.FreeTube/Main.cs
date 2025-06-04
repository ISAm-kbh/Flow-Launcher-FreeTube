using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using Flow.Launcher.Plugin;
using Flow.Launcher.Plugin.FreeTube.ViewModels;
using Flow.Launcher.Plugin.FreeTube.Views;
using Microsoft.Win32;

namespace Flow.Launcher.Plugin.FreeTube
{
    public class FreeTube : IPlugin, ISettingProvider
    {
        private const string Image = "Images/freetube.ico";
        private const string FreetubeRegistryKeyPath = "freetube\\shell\\open\\command";

        private PluginInitContext context;
        private string? FreetubePath;
        private Settings settings;
        private SettingsViewModel viewModel;

        public void Init(PluginInitContext context)
        {
            this.context = context;
            this.settings = context.API.LoadSettingJsonStorage<Settings>();
            this.viewModel = new SettingsViewModel(this.settings);
            this.FreetubePath = null;    
        }

        public List<Result> Query(Query query)
        {
            var results = new List<Result>();
            string url = query.Search;
                this.FreetubePath = LoadFreetubePathFromRegistry();
            if (this.FreetubePath == null)
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
            int newScore = this.settings.favorNewInstance ? 1 : 0;
            int existScore = this.settings.favorNewInstance ? 0 : 1;

            Result newWindowResult = new()
            {
                Title = "Open with new instance",
                SubTitle = "Open in FreeTube. If an instance exists, create a new instance",
                IcoPath = Image,
                Score = newScore,
                Action = c =>
                {
                    var freetubeCommand = SharedCommands.ShellCommand.SetProcessStartInfo(this.FreetubePath, "", $"--new-window {url}");
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
                Score = existScore,
                Action = c =>
                {
                    var freetubeCommand = SharedCommands.ShellCommand.SetProcessStartInfo(this.FreetubePath, "", $"{url}");
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

        public Control CreateSettingPanel()
        {
            return new SettingsControl(this.viewModel);
        }
    }
}