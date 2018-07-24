using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhatsMore
{
    public partial class FrmOptions : Form
    {
        private Settings settings;

        public FrmOptions()
        {
            InitializeComponent();
            settings = Settings.Instance;
        }

        private void FrmOptions_Load(object sender, EventArgs e)
        {
            txtNumber.Text = settings.Sender;
            txtAPI.Text = settings.ApiToken;
            txtMessage.Text = settings.Message;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            settings.Sender = txtNumber.Text;
            settings.ApiToken = txtAPI.Text;
            settings.Message = txtMessage.Text;

            settings.SaveSettings();
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TxtNumber_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = FieldsValidated();
        }

        private void TxtAPI_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = FieldsValidated();
        }

        private void TxtMessage_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = FieldsValidated();
        }

        private bool FieldsValidated()
        {
            return (String.IsNullOrWhiteSpace(txtNumber.Text) == false &&
                txtNumber.TextLength >= 11 && Regex.IsMatch(txtNumber.Text, "[^0-9]") == false &&
                String.IsNullOrWhiteSpace(txtAPI.Text) == false &&
                String.IsNullOrWhiteSpace(txtMessage.Text) == false);
        }

        private void TxtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsDigit(e.KeyChar) == false && char.IsControl(e.KeyChar) == false;
        }
    }
}
