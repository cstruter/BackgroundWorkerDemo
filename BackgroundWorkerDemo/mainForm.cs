using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace BackgroundWorkerDemo
{
    public partial class mainForm : Form
    {
        BackgroundWorker bw = new BackgroundWorker();

        public mainForm()
        {
            InitializeComponent();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            //bw.RunWorkerCompleted
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lbxItems.Items.Insert(0, e.ProgressPercentage);
        }

        // Access controls residing in the main thread
        void addItem(Int32 i)
        {
            if (lbxItems.InvokeRequired)
            {
                Action<Int32> a = new Action<int>(addItem);
                this.BeginInvoke(a, i);
            }
            else
            {
                lbxItems.Items.Insert(0, i);
            }
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(100);

                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                }

                if (!e.Cancel)
                {
                    //addItem(i);
                    bw.ReportProgress(i);
                }
                else
                {
                    break;
                }
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (cbxWorker.Checked)
            {
                if (!bw.IsBusy)
                {
                    bw.RunWorkerAsync();
                }
            }
            else
            {
                for (int i = 0; i < 50; i++)
                {
                    Thread.Sleep(100);
                    lbxItems.Items.Insert(0, i);
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            bw.CancelAsync();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}