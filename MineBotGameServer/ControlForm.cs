using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineBotGame
{
    public partial class ControlForm : Form
    {
        public ControlForm(Game game)
        {
            InitializeComponent();
            this.game = game;
            game.onStatsUpdate += Game_onStatsUpdate;
            Program.cancellationToken.Register(() => { /*Thread.Sleep(1000);*/ if (!isSelfCancelled) Invoke(new Action(() => Close())); });
        }

        private void Game_onStatsUpdate(Game.GameStats obj)
        {
            Invoke(new Action(() => { statsLabel.Text = obj.ToString(); }));
        }

        public void Append(string msg)
        {
            logTextBox.AppendText(msg);
        }

        public void AppendLine(string msg)
        {
            Append(msg + "\r\n");
        }

        private void ConsoleWrite(string msg)
        {
            AppendLine("[CONSOLE]: " + msg);
        }

        public AutoResetEvent loadEvent = new AutoResetEvent(false);

        private void ControlForm_Load(object sender, EventArgs e)
        {
            isLoaded = true;
            loadEvent.Set();
        }

        private void ControlForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void ControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.cancellationToken.IsCancellationRequested)
            {
                isSelfCancelled = true;
                Program.cancelSource.Cancel();
            }
            if (!canBeClosed)
            {
                e.Cancel = true;
                canBeClosed = true;
                Task.Delay(20).ContinueWith((_) => Invoke(new Action(() => Close())));
                return;
            }


            isFormClosing = true;
            if (!formCloseMutex.WaitOne(10))
            {
                e.Cancel = true;
                new Thread(CloseWorker).Start();
            }
        }

        private void CloseWorker()
        {
            Thread.Sleep(10);
            Invoke(new Action(() => Close()));
        }

        private Game game;
        private bool canBeClosed = false;
        private bool isSelfCancelled = false;

        public Mutex formCloseMutex = new Mutex();
        public bool isFormClosing = false;
        public bool isLoaded;

        private List<string> commandHistory = new List<string>();
        int historyPosition = 0; //count from the end
        string cmdBuffer = "";

        private string GetVersion()
        {
            return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        }

        private async void ProcessCommand(string command)
        {
            string help = "MineBotGame v. " + GetVersion() + @"
Aviliable commands:
about
help
stop
stats
restart
";

            string[] substrs = command.Split(' ');
            switch (substrs[0].ToLower())
            {
                case "exit":
                case "stop":
                    await game.StopAsync();
                    break;
                case "help":
                    ConsoleWrite(help);
                    break;
                case "info":
                case "about":
                case "version":
                    ConsoleWrite("MineBotGame v. " + GetVersion() + "\r\n\t\tby DCNick3");
                    break;
                case "stats":
                    {
                        var s = await game.GetStatsAsync();
                        ConsoleWrite("Stats: \r\n" + s.ToString());
                    }
                    break;
                default:
                    ConsoleWrite("Error - command not found");
                    break;
            }
        }

        private void HistoryUpdate(int delta)
        {
            if (historyPosition == -1)
                cmdBuffer = commandTextBox.Text;

            historyPosition += delta;

            if (historyPosition >= commandHistory.Count)
                historyPosition = commandHistory.Count - 1;
            if (historyPosition < -1)
                historyPosition = -1;

            if (historyPosition == -1 || commandHistory.Count == 0)
            {
                commandTextBox.Text = cmdBuffer;
                historyPosition = -1;
            }
            else
            {
                commandTextBox.Text = commandHistory[commandHistory.Count - historyPosition - 1];
            }
                commandTextBox.Select(Math.Max(0, commandTextBox.Text.Length), 0);
        }

        private void commandTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                historyPosition = -1;
                string cmd = commandTextBox.Text;
                commandHistory.Add(cmd);
                ProcessCommand(cmd);
                commandTextBox.Text = "";
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                HistoryUpdate(1);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                HistoryUpdate(-1);
                e.Handled = true;
            }
        }
    }
}
