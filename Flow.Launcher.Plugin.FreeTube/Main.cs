using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using Flow.Launcher.Plugin.FreeTube.ViewModels;
using Flow.Launcher.Plugin.FreeTube.Views;
using Microsoft.Win32;

namespace Flow.Launcher.Plugin.FreeTube
{
    public class FreeTube : IPlugin, ISettingProvider, IPluginI18n
    {
        private const string Image = "Images/freetube.ico";
        private const string FreetubeRegistryKeyPath = "freetube\\shell\\open\\command";

        private PluginInitContext context;
        private string? FreetubePath;
        private Settings settings;
        private SettingsViewModel? viewModel;

        public void Init(PluginInitContext context)
        {
            this.context = context;
            this.settings = context.API.LoadSettingJsonStorage<Settings>();
            this.viewModel = null;
            this.FreetubePath = null;    
        }

        public List<Result> Query(Query query)
        {
            var results = new List<Result>();
            string url = query.Search;

            if (this.settings.manuallySpecifyPath)
            {
                this.FreetubePath = this.settings.userSpecifiedPath;
            }
            else
            {
                this.FreetubePath = LoadFreetubePathFromRegistry();
            }
            NullifyPathIfExecutableUnreachable(ref this.FreetubePath);

            if (this.FreetubePath == null)
            {
                Result noPathResult = new()
                {
                    Title = this.context.API.GetTranslation("flowlauncher_plugin_freetube_executable_not_found"),
                    SubTitle = this.context.API.GetTranslation("flowlauncher_plugin_freetube_verify_install_message"),
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
                Title = this.context.API.GetTranslation("flowlauncher_plugin_freetube_open_new_instance"),
                SubTitle = this.context.API.GetTranslation("flowlauncher_plugin_freetube_new_instance_desc"),
                IcoPath = Image,
                Score = newScore,
                Action = c =>
                {
                    var freetubeCommand = SharedCommands.ShellCommand.SetProcessStartInfo(this.FreetubePath, "", $"--new-window {url}");
                    freetubeCommand.UseShellExecute = true;
                    try
                    {
                        SharedCommands.ShellCommand.Execute(freetubeCommand);
                    }
                    catch (Win32Exception e)
                    {
                        string errorMessage = string.Format(
                            this.context.API.GetTranslation("flowlauncher_plugin_freetube_process_run_error"),
                            this.FreetubePath
                            );
                        this.context.API.ShowMsgError(errorMessage, e.Message);
                        return false;
                    }
                    return true;
                }
            };
            Result existingWindowResult = new()
            {
                Title = this.context.API.GetTranslation("flowlauncher_plugin_freetube_open_existing_instance"),
                SubTitle = this.context.API.GetTranslation("flowlauncher_plugin_freetube_existing_instance_desc"),
                IcoPath = Image,
                Score = existScore,
                Action = c =>
                {
                    var freetubeCommand = SharedCommands.ShellCommand.SetProcessStartInfo(this.FreetubePath, "", $"{url}");
                    freetubeCommand.UseShellExecute = true;
                    try
                    {
                        SharedCommands.ShellCommand.Execute(freetubeCommand);
                    }
                    catch (Win32Exception e)
                    {
                        string errorMessage = string.Format(
                            this.context.API.GetTranslation("flowlauncher_plugin_freetube_process_run_error"),
                            this.FreetubePath
                            );
                        this.context.API.ShowMsgError(errorMessage, e.Message);
                        return false;
                    } 
                    return true;
                }
            };
            results.Add(newWindowResult);
            results.Add(existingWindowResult);
            return results;
        }

        public Control CreateSettingPanel()
        {
            if (this.viewModel == null)
            {
                this.viewModel = new SettingsViewModel(this.settings, this.context);
            }
            return new SettingsControl(this.viewModel);
        }

        public string GetTranslatedPluginTitle()
        {
            return this.context.API.GetTranslation("flowlauncher_plugin_freetube_plugin_name");
        }

        public string GetTranslatedPluginDescription()
        {
            return this.context.API.GetTranslation("flowlauncher_plugin_freetube_plugin_desc");
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

        private void NullifyPathIfExecutableUnreachable(ref string? path)
        {
            bool pathFileExists = File.Exists(path);
            if (!pathFileExists)
            {
                path = null;
                return;
            }
        }
    }
}