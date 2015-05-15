//-----------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Ozzymud">
// Copyright 5/14/2015 Ozzymud
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// <author>Ozzymud</author>
//-----------------------------------------------------------------------

namespace IPchecker
{
#region Using directives
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
#endregion

    /// <summary>
    /// Main window.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            this.InitializeComponent();
        }
        
        private string ip = string.Empty;
        
        private static string LocalIPAddress()
            {
            IPHostEntry host;
            string localIP = string.Empty;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
                {
                localIP = ip.ToString();
                string[] temp = localIP.Split('.');
                if (ip.AddressFamily == AddressFamily.InterNetwork && temp[0] == "192")
                    {
                    break;
                    }
                else
                {
                    localIP = null;
                }
            }

            return localIP;
        }

        private void BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.ip = new WebClient().DownloadString("http://api.ipify.org");
            e.Result= this.ip;
        }
        
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.resultBox.Text = this.ip;
            Clipboard.SetText(this.ip);
        }

        private void Button1Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.RunWorkerAsync();
            this.resultBox.Text = "Reading IP from external site...";
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }
        
        private void Button2Click(object sender, EventArgs e)
            {
            ip = LocalIPAddress();
            this.resultBox.Text = this.ip;
            Clipboard.SetText(this.ip);
            }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
