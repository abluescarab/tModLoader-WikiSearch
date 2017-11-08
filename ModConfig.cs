using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace ModConfiguration {
    public static class ModConfig {
        private static Dictionary<string, ModOption> options = new Dictionary<string, ModOption>();
        private static string modName = "";
        private static Preferences preferences;

        /// <summary>
        /// The name of the configuration file (without extension).
        /// </summary>
        public static string ModName {
            get { return modName; }
            set {
                if(!modName.Equals(value) && !string.IsNullOrWhiteSpace(value)) {
                    modName = value;
                    preferences = new Preferences(FilePath);
                }
            }
        }

        /// <summary>
        /// Where the configuration file will be saved.
        /// </summary>
        public static string FilePath {
            get { return Path.Combine(Main.SavePath, "Mod Configs", ModName + ".json"); }
        }

        /// <summary>
        /// Add a new configuration option.
        /// </summary>
        public static void AddOption(string name, object defaultValue) {
            if(!options.ContainsKey(name)) {
                options.Add(name, new ModOption(name, defaultValue));
            }
        }

        /// <summary>
        /// Add a new configuration option.
        /// </summary>
        public static void AddOption(ModOption option) {
            if(!options.ContainsKey(option.Name)) {
                options.Add(option.Name, option);
            }
        }

        /// <summary>
        /// Remove a configuration option.
        /// </summary>
        public static void RemoveOption(string name) {
            if(options.ContainsKey(name)) {
                options.Remove(name);
            }
        }

        /// <summary>
        /// Remove a configuration option.
        /// </summary>
        public static void RemoveOption(ModOption option) {
            if(options.ContainsKey(option.Name)) {
                options.Remove(option.Name);
            }
        }

        /// <summary>
        /// Get the value of a configuration option.
        /// </summary>
        public static object GetOption(string name) {
            return options[name].Value;
        }

        /// <summary>
        /// Set the value of a configuration option.
        /// </summary>
        public static void SetOption(string name, object value) {
            options[name].Value = value;
        }

        /// <summary>
        /// Load the configuration from <see cref="FilePath"/>, or write to a new file if it doesn't exist.
        /// </summary>
        public static void Load() {
            if(!ReadConfig()) {
                ErrorLogger.Log("Failed to read " + FilePath + ". Recreating config...");
                WriteConfig();
            }
        }

        /// <summary>
        /// Read the configuration from <see cref="FilePath"/>.
        /// </summary>
        /// <returns>whether a configuration file exists</returns>
        private static bool ReadConfig() {
            if(preferences.Load()) {
                foreach(ModOption obj in options.Values) {
                    obj.Value = preferences.Get(obj.Name, obj.Value);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Write the configuration file if it doesn't exist.
        /// </summary>
        private static void WriteConfig() {
            preferences.Clear();

            foreach(ModOption obj in options.Values) {
                preferences.Put(obj.Name, obj.Value);
            }

            preferences.Save();
        }
    }
    
    public class ModOption {
        public string Name { get; set; }
        public object Value { get; set; }
        
        public ModOption(string name, object value) {
            Name = name;
            Value = value;
        }
    }
}
