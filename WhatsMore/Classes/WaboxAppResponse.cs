using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WhatsMore
{
    class WaboxAppResponse
    {
        private bool isReceivedByWaboxapp;
        private string errorMessage;
        private bool hasError;
        private string customUID;

        [JsonProperty(PropertyName = "success")]
        public bool IsReceivedByWaboxapp { get => isReceivedByWaboxapp; set => isReceivedByWaboxapp = value; }

        [JsonProperty(PropertyName = "error")]
        public string ErrorMessage { get => errorMessage; set => errorMessage = value.Trim(); }

        [JsonIgnore]
        public bool HasError { get => hasError; set => hasError = value; }

        [JsonProperty(PropertyName = "custom_uid")]
        public string CustomUID { get => customUID; set => customUID = value; }

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
