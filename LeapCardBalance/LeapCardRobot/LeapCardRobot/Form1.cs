using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeapCardRobot
{
    public partial class Form1 : Form
    {
        LeapCardFlow lcf;
        WebBrowser wb;
        DataLeapCard request;

        public Form1()
        {
            InitializeComponent();
            request = new DataLeapCard("igorafff", "M0reiraa");
            request.EvDataLeapCardCompleted += request_EvDataLeapCardCompleted;

            lcf = new LeapCardFlow(request);
            lcf.wb = this.webBrowser1;
            lcf.GetBalance();
            //lcf.GetBalanceQuick();
        }

        void request_EvDataLeapCardCompleted(object sender, EvDataLeapCardCompleted e)
        {
            if (e.Error)
            {
                MessageBox.Show("Error", "Error");
            }
            else
            {
                MessageBox.Show("Your Balance is:" + e.Balance, "Balance");
            }
        }        
    }
}
