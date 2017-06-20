using System;
using System.Windows.Forms;

namespace GameServer
{
    public partial class GameServerSettingsForm : Form
    {
        public GameServerSettingsForm()
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

        private void frmServerSettings_Load(object sender, EventArgs e)
        {
            nudHeight.Value = Game.Map.Height;
            nudWidth.Value = Game.Map.Width;
            nudSeed.Value = Game.Map.Seed;
        }
    }
}
