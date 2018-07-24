using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WhatsMore
{
    class WaboxAppPhoneInfoResponse
    {
        private bool isReceivedByWaboxapp;
        private string whatsappAccountName;
        private string phonePlatform;
        private int batteryPercentage;
        private string isPluggedIn; // 0 or 1
        private string webSessionLocale;
        private string accountUID;
        private string errorMessage;
        private bool hasError;

        [JsonProperty(PropertyName = "success")]
        public bool IsReceivedByWaboxapp { get => isReceivedByWaboxapp; set => isReceivedByWaboxapp = value; }

        [JsonProperty(PropertyName = "alias")]
        public string WhatsappAccountName { get => whatsappAccountName; set => whatsappAccountName = value; }

        [JsonProperty(PropertyName = "platform")]
        public string PhonePlatform { get => phonePlatform; set => phonePlatform = value; }

        [JsonProperty(PropertyName = "battery")]
        public int BatteryPercentage { get => batteryPercentage; set => batteryPercentage = value; }

        [JsonProperty(PropertyName = "plugged")]
        public string IsPluggedIn { get => isPluggedIn; set => isPluggedIn = value; }

        [JsonProperty(PropertyName = "locale")]
        public string WebSessionLocale { get => webSessionLocale; set => webSessionLocale = value; }

        [JsonProperty(PropertyName = "uid")]
        public string AccountUID { get => accountUID; set => accountUID = value; }

        [JsonProperty(PropertyName = "error")]
        public string ErrorMessage { get => errorMessage; set => errorMessage = value.Trim(); }

        [JsonIgnore]
        public bool HasError { get => hasError; set => hasError = value; }

        [OnDeserialized]
        private void OnDeserializedMethod(StreamingContext context)
        {
            if (String.IsNullOrEmpty(ErrorMessage) == false)
            {
                HasError = true;
                ErrorMessage = ErrorMessage.Trim(); // Removes the annoying space(s) present sometimes.
            }
        }
    }
}
