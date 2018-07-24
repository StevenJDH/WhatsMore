using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace WhatsMore
{
    sealed class Settings
    {
        private AES aes;
        private string AESKey { get; set; }
        public string Sender { get; set; }
        public string ApiToken { get; set; }
        public string Message { get; set; }
        private static string SettingsPath { get; set; }
        public static Settings Instance { get; private set; }
        
        private Settings()
        {
            aes = new AES();
            AESKey = "3;eR*h9X6$7dVQZS"; // Hard-coded key for AES encryption.
        }

        static Settings()
        {
            // Path to settings file that is stored in the %AppData%/WhatsMore directory.
            SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "WhatsMore", "WhatsMoreConfig.json");
            // Singleton design pattern.
            Instance = LoadSettings();
        }

        private static Settings LoadSettings()
        {
            if (File.Exists(SettingsPath))
            {
                string jsonData = File.ReadAllText(SettingsPath);

                if (String.IsNullOrWhiteSpace(jsonData))
                {
                    MessageBox.Show(Properties.Strings.Error_ConfigCorrupt,
                        "WhatsMore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new Settings();
                }

                // Any trouble reading the configuration will just use the defaults.
                try
                {
                    return JsonConvert.DeserializeObject<Settings>(jsonData);
                }
                catch (JsonReaderException)
                {
                    MessageBox.Show(Properties.Strings.Error_ConfigCorrupt,
                        "WhatsMore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new Settings();
                }
            }
            else
            {
                MessageBox.Show(Properties.Strings.Info_FirstTimeConfig, 
                    "WhatsMore", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return new Settings();
            }
        }

        public void SaveSettings()
        {
            string jsonData = JsonConvert.SerializeObject(Instance, Formatting.Indented);
            
            // Builds any missing folders in path where the settings will be stored.
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
            // Saves the settings to profile.
            File.WriteAllText(SettingsPath, jsonData);
        }

        [OnSerializing]
        private void OnSerializingMethod(StreamingContext context)
        {
            ApiToken = aes.Encrypt(ApiToken, AESKey);
        }

        [OnSerialized]
        private void OnSerializedMethod(StreamingContext context)
        {
            ApiToken = aes.Decrypt(ApiToken, AESKey);
        }

        [OnDeserialized]
        private void OnDeserializedMethod(StreamingContext context)
        {
            ApiToken = aes.Decrypt(ApiToken, AESKey);
        }
    }
}
