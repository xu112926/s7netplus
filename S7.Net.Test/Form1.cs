﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7.Net;

namespace S7.Net.Test
{
    public partial class Form1 : Form
    {
        private Plc _myPlc;
        private ControlResizer Resizer1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Resizer1 = new ControlResizer(this);
            cmbType.DataSource = Enum.GetNames(typeof(CpuType));
            cmbType.SelectedIndex = 4;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                grbInit.Enabled = false;
                _myPlc = new Plc((CpuType)Enum.Parse(typeof(CpuType), cmbType.Text), txtIP.Text, 0, 1);
                _myPlc.Open();
            }
            catch (Exception ex)
            {
                recordMessage(ex.Message);
            }
            finally
            {
                grbInit.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            for (int i = 100; i < 255; i++)
            {
                using (var plc = new Plc((CpuType)Enum.Parse(typeof(CpuType), cmbType.Text), $"192.168.2.{i}", 0, 1))
                {
                    if (plc.IsAvailable)
                    {
                        recordMessage($"192.168.2.{i} OK");
                        break;
                    }
                    else
                    {
                        recordMessage($"192.168.2.{i} NG");
                    }
                }
            }
            
        }

        private void btnRead_Click(object sender, EventArgs e)
        {

        }

        private void btnWrite_Click(object sender, EventArgs e)
        {

        }

        private void recordMessage(string message)
        {
            this.Invoke((MethodInvoker)delegate
            {
                rtbMessage.Text = rtbMessage.Text.Insert(0,
                DateTime.Now.ToString("yy/MM/dd-HH:mm:ss fff") + "  ：  " + message + Environment.NewLine);
            });
        }
    }
}
