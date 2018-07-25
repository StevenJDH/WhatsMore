/**
 * This file is part of WhatsMore <https://github.com/StevenJDH/WhatsMore>.
 * Copyright (C) 2018 Steven Jenkins De Haro.
 *
 * WhatsMore is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * WhatsMore is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with WhatsMore.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Diagnostics;

namespace WhatsMore
{
    sealed class Configuration
    {
        private AES aes;
        private readonly string aesKey;
        private readonly string aesSaltKey;
        public string Sender { get; set; }
        public string ApiToken { get; set; }
        public string Message { get; set; }
        private static string configPath { get; set; }
        public static Configuration Instance { get; private set; }
        
        private Configuration()
        {
            aes = new AES();
            // Hard-coded keys for AES encryption.
            aesKey = "3;eR*h9X6$7dVQZS";
            aesSaltKey = "Ì¥Ø¡eÈ";
        }

        static Configuration()
        {
            // Path to configuration file that is stored in the %AppData%/WhatsMore directory.
            configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "WhatsMore", "WhatsMoreConfig.json");
            try
            {
                // Singleton design pattern.
                Instance = LoadSettings();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(Properties.Strings.Error_MissingJsonDllExit,
                        "WhatsMore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
        }

        private static Configuration LoadSettings()
        {
            if (File.Exists(configPath))
            {
                string jsonData = File.ReadAllText(configPath);

                if (String.IsNullOrWhiteSpace(jsonData))
                {
                    MessageBox.Show(Properties.Strings.Error_ConfigCorrupt,
                        "WhatsMore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new Configuration();
                }

                // Any trouble reading the configuration will just use the defaults.
                try
                {
                    return JsonConvert.DeserializeObject<Configuration>(jsonData);
                }
                catch (JsonReaderException)
                {
                    MessageBox.Show(Properties.Strings.Error_ConfigCorrupt,
                        "WhatsMore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new Configuration();
                }
            }
            else
            {
                MessageBox.Show(Properties.Strings.Info_FirstTimeConfig,
                    "WhatsMore", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return new Configuration();
            }
        }

        public void SaveSettings()
        {
            string jsonData = JsonConvert.SerializeObject(Instance, Formatting.Indented);

            // Builds any missing folders in path where the configuration will be stored.
            Directory.CreateDirectory(Path.GetDirectoryName(configPath));
            // Saves the configuration to profile.
            File.WriteAllText(configPath, jsonData);
        }

        [OnSerializing]
        private void OnSerializingMethod(StreamingContext context)
        {
            ApiToken = aes.Encrypt(ApiToken, aesKey, aesSaltKey);
        }

        [OnSerialized]
        private void OnSerializedMethod(StreamingContext context)
        {
            ApiToken = aes.Decrypt(ApiToken, aesKey, aesSaltKey);
        }

        [OnDeserialized]
        private void OnDeserializedMethod(StreamingContext context)
        {
            ApiToken = aes.Decrypt(ApiToken, aesKey, aesSaltKey);
        }
    }
}
