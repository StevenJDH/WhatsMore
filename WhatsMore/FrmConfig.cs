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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhatsMore
{
    public partial class FrmConfig : Form
    {
        private readonly Configuration config;

        public FrmConfig()
        {
            InitializeComponent();
            config = Configuration.Instance;
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            txtNumber.Text = config.Sender;
            txtAPI.Text = config.ApiToken;
            txtMessage.Text = config.Message;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            config.Sender = txtNumber.Text;
            config.ApiToken = txtAPI.Text;
            config.Message = txtMessage.Text;

            config.SaveSettings();
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

        /// <summary>
        /// Checks to see if actual data was entered in and that length and data requirements
        /// are met in all the fields.
        /// </summary>
        /// <returns>Pass or fail result of the validation test</returns>
        private bool FieldsValidated()
        {
            return (String.IsNullOrWhiteSpace(txtNumber.Text) == false &&
                txtNumber.TextLength >= 11 && Regex.IsMatch(txtNumber.Text, "[^0-9]") == false &&
                String.IsNullOrWhiteSpace(txtAPI.Text) == false &&
                String.IsNullOrWhiteSpace(txtMessage.Text) == false);
        }

        private void TxtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allows only numbers to be entered in for the sender's phone number.
            e.Handled = char.IsDigit(e.KeyChar) == false && char.IsControl(e.KeyChar) == false;
        }
    }
}
