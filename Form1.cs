using tf2butt.src;

namespace tf2butt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            ButtplugConnection connection = new ButtplugConnection();

            TFWatcher watcher = new TFWatcher();
            connection.ButtplugInit().Wait();
            watcher.CheckStatus();
        }
    }
}