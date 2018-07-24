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
        private Settings settings = Settings.Instance; // Ensures first time message displays.

        public FrmMain()
        {
            InitializeComponent();
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            // Ensures that the program has been configured.
            if (String.IsNullOrWhiteSpace(settings.Sender) || String.IsNullOrWhiteSpace(settings.ApiToken) ||
                String.IsNullOrWhiteSpace(settings.Message))
            {
                MnuOptions_Click(this, EventArgs.Empty); // Prompts for configuration.
                return;
            }
            if (Connection.IsInternetAvailable() == false)
            {
                MessageBox.Show(Properties.Strings.Alert_NoInternetConnection,
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Properties.Settings.Default.isSending = false;
                return;
            }

            // Removes blank lines from list of numbers.
            txtNumbers.Text = Regex.Replace(txtNumbers.Text, "\\s+\r\n", "\r\n").Trim();

            int totalNumbers = txtNumbers.Lines.Length;
            List<string> notSentList = new List<string>();
            WaboxAppAPI whatsapp = new WaboxAppAPI(settings.ApiToken, settings.Sender);

            try
            {
                Properties.Settings.Default.isSending = true;

                PhoneState phoneState = await whatsapp.GetPhoneConnectedStateAsync();
                switch (phoneState)
                {
                    case PhoneState.NoWhatsAppSession:
                        MessageBox.Show(String.Format(Properties.Strings.Alert_WhatsAppNotConnected, settings.Sender),
                            this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Properties.Settings.Default.isSending = false;
                        return;
                    case PhoneState.Unauthorized:
                        MessageBox.Show(String.Format(Properties.Strings.Error_UnauthorizedSender, settings.Sender),
                            this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Properties.Settings.Default.isSending = false;
                        return;
                }

                pbSending.Maximum = totalNumbers;

                for (int i = 0; i < totalNumbers; i++)
                {
                    string msgID = Guid.NewGuid().ToString("N"); // The 'N' removes dashes in GUID.
                    WaboxAppResponse response = await whatsapp.SendMessageAsync(/* "32" + */ txtNumbers.Lines[i],
                        msgID, settings.Message);
                    if (response == null || response.HasError)
                    {
                        notSentList.Add(txtNumbers.Lines[i]);
                    }

                    pbSending.Value += 1;
                    SetProgressNoAnimation(pbSending);
                }

                // Save numbers that could not be sent to file.
                if (notSentList.Count > 0) { SaveNotSent(notSentList); }

                MessageBox.Show(String.Format(Properties.Strings.Info_SentSuccessful,
                    totalNumbers - notSentList.Count, totalNumbers),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                // Generic message for multiple possible exceptions that are handled the same.
                MessageBox.Show(Properties.Strings.Error_GetRequestFailed, this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                pbSending.Value = 0;
                Properties.Settings.Default.isSending = false;
            }
        }

        // Sets the progress bar value, without using Windows Aero animation
        private void SetProgressNoAnimation(ProgressBar pb)
        {
            // To get around this animation, we need to move the progress bar backwards.
            // Special case (can't set value > Maximum).
            if (pb.Value == pb.Maximum)
            {
                pb.Maximum += 1;
                pb.Value += 1; // Moves past
                pb.Value -= 1; // and back to set correct value
                pb.Maximum -= 1;
            }
            else
            {
                pb.Value += 1; // Moves past
                pb.Value -= 1; // and back to set correct value
            }
        }

        private void SaveNotSent(List<string> numbersList)
        {
            string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            $"WhatsMore Log_{DateTime.Now.ToString("yyyy-MM-dd_HHmmss")}.txt");

            File.WriteAllLines(desktopPath, numbersList);
        }

        private void FrmMain_Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = Properties.Settings.Default.isSending; // Prevents program from exiting if sending messages.
        }

        private void MnuSpanish_Click(object sender, EventArgs e)
        {
            ChangeLocale("es");
        }

        private void MnuEnglish_Click(object sender, EventArgs e)
        {
            ChangeLocale("en");
        }

        private void ChangeLocale(string locale)
        {
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
            MessageBox.Show("WhatsMore 1.0.0 Beta\n\nSteven Jenkins De Haro\nMicrosoft .NET Framework 4.6.1",
                this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MnuOptions_Click(object sender, EventArgs e)
        {
            using (FrmOptions frm = new FrmOptions())
            {
                frm.ShowDialog();
            }
        }

        private void TxtNumbers_TextChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = (String.IsNullOrWhiteSpace(txtNumbers.Text) == false && 
                Regex.IsMatch(txtNumbers.Text, "[^0-9\r\n\\s]") == false);
        }

        private void TxtNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsDigit(e.KeyChar) == false && char.IsControl(e.KeyChar) == false;
        }
    }
}
