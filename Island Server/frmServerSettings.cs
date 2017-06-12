using System;
using System.Windows.Forms;

namespace IslandServer
{
    public partial class frmServerSettings : Form
    {
        public frmServerSettings()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Game.Map.Height = (int)nudHeight.Value;
            Game.Map.Width = (int)nudWidth.Value;
            Game.Map.Seed = (int)nudSeed.Value;

            Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
