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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Windows.Forms;

namespace WhatsMore
{
    public enum PhoneState { Unauthorized = 0, NoWhatsAppSession = 1, Connected = 2 }

    class WaboxAppAPI
    {
        private string apiToken;
        private string sender;

        public WaboxAppAPI(string apiToken, string sender)
        {
            ApiToken = apiToken;
            Sender = sender;
        }

        public string ApiToken
        {
            get => this.apiToken;
            set
            {
                if (String.IsNullOrWhiteSpace(value) == true)
                {
                    throw new ArgumentException(Properties.Strings.ArgEx_InvalidAPIKey);
                }
                else
                {
                    this.apiToken = value.Trim();
                }
            }
        }

        public string Sender {
            get => sender;
            set
            {
                if (String.IsNullOrWhiteSpace(value) == true)
                {
                    throw new ArgumentException(Properties.Strings.ArgEx_InvalidSenderPhone);
                }
                else
                {
                    this.sender = value.Trim();
                }
            }
        }

        /// <summary>
        /// Sends a WhatsApp text messaged to a recipient via the specified account.
        /// </summary>
        /// <param name="recipient">Number for person receiving the message</param>
        /// <param name="msgID">Unique ID to identify the message</param>
        /// <param name="message">Text message that the recipient will receive</param>
        /// <returns>Response object for the message sent</returns>
        public async Task<WaboxAppResponse> SendMessageAsync(string recipient, string msgID, string message)
        {
            string url = $"https://www.waboxapp.com/api/send/chat";
            string postData = $"token={ApiToken}&uid={Sender}&to={recipient.Trim()}&custom_uid={msgID}&text={HttpUtility.UrlEncode(message.Trim())}";

            try
            {
                return await APIServiceCallAsync<WaboxAppResponse>(url, postData);
            }
            catch (HttpRequestException)
            {
                // For now, we don't care about individual errors as this is a batch job.
                // The caller will be tracking which numbers had issues when sending.
                return null;
            }
        }

        /// <summary>
        /// Gets information about the linked phone such as battery percentage and locale.
        /// </summary>
        /// <returns>Response object loaded with the requested data</returns>
        public async Task<WaboxAppPhoneInfoResponse> GetPhoneInfoAsync()
        {
            string url = $"https://www.waboxapp.com/api/status/{Sender}";
            string postData = $"token={ApiToken}";

            return await APIServiceCallAsync<WaboxAppPhoneInfoResponse>(url, postData);
        }

        /// <summary>
        /// Calls the WaboxApp API service to communicate with WhatsApp accounts and linked phones. The
        /// post method uses the 'application/x-www-form-urlencoded' media type.
        /// </summary>
        /// <typeparam name="T">Object representing the API response</typeparam>
        /// <param name="apiLink">API link to access service data</param>
        /// <param name="postData">URL encoded parameters need by the API call</param>
        /// <returns>Response object loaded with the requested data</returns>
        private async Task<T> APIServiceCallAsync<T>(string apiLink, string postData)
        {
            StringContent content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage result = await client.PostAsync(apiLink, content))
                {
                    string jsonData = await result.Content.ReadAsStringAsync();
                    // Will provide error information if there is a problem.
                    return JsonConvert.DeserializeObject<T>(jsonData);
                }
            }
        }

        /// <summary>
        /// Provides information regarding the connected state of the phone linked to the service.
        /// </summary>
        /// <returns>Phone's connected state</returns>
        public async Task<PhoneState> GetPhoneConnectedStateAsync()
        {
            WaboxAppPhoneInfoResponse response = await GetPhoneInfoAsync();

            if (response.HasError == false)
            {
                return PhoneState.Connected;
            }
            else if (response.ErrorMessage.Contains("Unauthorized"))
            {
                return PhoneState.Unauthorized;
            }
            else
            {
                return PhoneState.NoWhatsAppSession;
            }
        }
    }
}
