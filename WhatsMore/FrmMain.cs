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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhatsMore
{
    public partial class FrmMain : Form
    {
        private Ref<bool> cancelRequested;
        private readonly Configuration config;

        public FrmMain()
        {
            InitializeComponent();
            // Using custom type for cancellation because the token approach was randomly
            // setting itself to true when evaluated and because we can't use the ref
            // keyword in a async method.
            cancelRequested = new Ref<bool>();
            // Gets singleton instance, and it will show a one-time reminder message for configuration.
            config = Configuration.Instance;
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.isSending == false)
            {
                // Ensures that the program has been configured.
                if (String.IsNullOrWhiteSpace(config.Sender) || String.IsNullOrWhiteSpace(config.ApiToken) ||
                    String.IsNullOrWhiteSpace(config.Message))
                {
                    MnuConfig_Click(this, EventArgs.Empty); // Prompts for configuration.
                    return;
                }

                // Ensures that there is an internet connection before starting the sending process.
                if (Connection.IsInternetAvailable() == false)
                {
                    MessageBox.Show(Properties.Strings.Alert_NoInternetConnection,
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Prepares state information for the sending process.
                Properties.Settings.Default.isSending = true;
                Properties.Settings.Default.ControlsEnabled = false;
                btnSend.Text = Properties.Strings.Cancel; // Changes the Send button to a Cancel button.

                WaboxAppAPI whatsapp = new WaboxAppAPI(config.ApiToken, config.Sender);

                try
                {
                    PhoneState phoneState = await whatsapp.GetPhoneConnectedStateAsync();

                    if (phoneState == PhoneState.NoWhatsAppSession)
                    {
                        MessageBox.Show(String.Format(Properties.Strings.Alert_WhatsAppNotConnected, config.Sender),
                                this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (phoneState == PhoneState.Unauthorized)
                    {
                        MessageBox.Show(String.Format(Properties.Strings.Error_UnauthorizedSender, config.Sender),
                                this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else // Everything is connected and ready.
                    {
                        List<string> notSentList = new List<string>();

                        // Send the batch messages.
                        await whatsapp.SendBatchMessagesAsync(txtNumbers, config.Message, notSentList,
                            pbSending, cancelRequested);

                        // Saves to file those numbers that had sending issues or got canceled.
                        if (notSentList.Count > 0) { SaveNotSent(notSentList); }

                        MessageBox.Show(String.Format(Properties.Strings.Info_SentSuccessful,
                            pbSending.Maximum - notSentList.Count, pbSending.Maximum),
                            this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {
                    // Generic message for multiple possible exceptions that are handled the same.
                    MessageBox.Show(Properties.Strings.Error_GetRequestFailed, this.Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Resets everything after sending process is done.
                pbSending.Value = 0;
                Properties.Settings.Default.isSending = false;
                Properties.Settings.Default.ControlsEnabled = true;
                cancelRequested = false;
                btnSend.Text = Properties.Strings.Send;
                btnSend.Enabled = true;
            }
            else // Cancel button mode.
            {
                btnSend.Enabled = false; // Disabled to prevent canceling twice.
                cancelRequested = true;
            }
        }

        /// <summary>
        /// Saves a log to the desktop for those numbers that had issues during the sending
        /// process. The date and time will be appended to the log so that it doesn't overwrite
        /// a previous sending session.
        /// </summary>
        /// <param name="numbersList">List of numbers to save to the log</param>
        private void SaveNotSent(List<string> numbersList)
        {
            string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            $"WhatsMore Log_{DateTime.Now.ToString("yyyy-MM-dd_HHmmss")}.txt");
            
            File.WriteAllLines(desktopPath, numbersList);
        }

        private void FrmMain_Closing(object sender, FormClosingEventArgs e)
        {
            // Prevents program from exiting if sending messages.
            e.Cancel = Properties.Settings.Default.isSending;
        }

        private void MnuSpanish_Click(object sender, EventArgs e)
        {
            ChangeLocale("es");
        }

        private void MnuEnglish_Click(object sender, EventArgs e)
        {
            ChangeLocale("en");
        }

        /// <summary>
        /// Changes the working locale and refreshes the interface to see the new language applied.
        /// </summary>
        /// <param name="locale">Locale needed</param>
        private void ChangeLocale(string locale)
        {
            // Prevents refreshing the interface when click on the current language.
            if (Thread.CurrentThread.CurrentUICulture.Name.StartsWith(locale) == false)
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(locale);
                Controls.Clear();
                InitializeComponent();
            }
        }

        private void MnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MnuAbout_Click(object sender, EventArgs e)
        {
            using (FrmAbout frm = new FrmAbout())
            {
                frm.ShowDialog();
            }
        }

        private void MnuConfig_Click(object sender, EventArgs e)
        {
            using (FrmConfig frm = new FrmConfig())
            {
                frm.ShowDialog();
            }
        }

        private void TxtNumbers_TextChanged(object sender, EventArgs e)
        {
            // Restricts valid input to numbers, spaces, and new lines.
            btnSend.Enabled = (String.IsNullOrWhiteSpace(txtNumbers.Text) == false && 
                Regex.IsMatch(txtNumbers.Text, "[^0-9\r\n\\s]") == false);
        }

        private void TxtNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allows only numbers to be entered in for the phone numbers.
            e.Handled = char.IsDigit(e.KeyChar) == false && char.IsControl(e.KeyChar) == false;
        }
    }
}
