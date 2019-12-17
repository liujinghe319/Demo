using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LjhTools.AutoCarCatch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAutoHome_Click(object sender, EventArgs e)
        {
            AutoHomeDataFrm frm = new AutoHomeDataFrm();
            frm.ShowDialog(this);
        }

        private void btnYiChe_Click(object sender, EventArgs e)
        {
            CatchYiche frm = new CatchYiche();
            frm.ShowDialog(this);
        }

        private void btnQuanNa_Click(object sender, EventArgs e)
        {
            CatchQuanna frm = new CatchQuanna();
            frm.ShowDialog(this);
        }

        private void btnKaChe_Click(object sender, EventArgs e)
        {
            Catch360Che frm = new Catch360Che();
            frm.ShowDialog(this);
        }
    }
}
