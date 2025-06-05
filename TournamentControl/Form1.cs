using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TournamentControl
{
        public partial class Form1 : Form
    {
        private List<PlayerControl> playerControls = new();
        private TextBox txtTeam;
        private ComboBox cmbLang;

        public Form1()
        {
            InitializeComponent();
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            this.Text = "Tournament Control";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new System.Drawing.Font("Segoe UI", 14);
            this.MinimumSize = new System.Drawing.Size(1024, 768);

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                AutoScroll = true,
                Padding = new Padding(20),
                AutoSize = true
            };

            var playersPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 1,
                RowCount = 5,
                AutoSize = true
            };

            for (int i = 0; i < 5; i++)
            {
                var pc = new PlayerControl { Dock = DockStyle.Top, Margin = new Padding(0, 10, 0, 10) };
                playerControls.Add(pc);
                playersPanel.Controls.Add(pc);
            }

            // Team Info
            var teamPanel = new TableLayoutPanel { ColumnCount = 4, AutoSize = true, Dock = DockStyle.Top };
            teamPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            teamPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            teamPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            teamPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            teamPanel.Controls.Add(new Label { Text = "Team:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            txtTeam = new TextBox { Width = 200, Anchor = AnchorStyles.Left };
            teamPanel.Controls.Add(txtTeam, 1, 0);

            teamPanel.Controls.Add(new Label { Text = "Language:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 0);
            cmbLang = new ComboBox { Width = 150, Anchor = AnchorStyles.Left };
            cmbLang.Items.AddRange(new string[] { "English", "Russian", "Spanish" });
            cmbLang.SelectedIndex = 0;
            teamPanel.Controls.Add(cmbLang, 3, 0);

            // Buttons
            var btnSave = new Button { Text = "Save", AutoSize = true };
            var btnExit = new Button { Text = "Exit", AutoSize = true };
            btnExit.Click += (s, e) => this.Close();
            btnSave.Click += (s, e) => OnSaveClicked(txtTeam.Text);
            this.AcceptButton = btnSave;
            this.CancelButton = btnExit;

            var buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10)
            };
            buttonPanel.Controls.AddRange(new Control[] { btnSave, btnExit });

            mainLayout.Controls.Add(playersPanel);
            mainLayout.Controls.Add(teamPanel);
            mainLayout.Controls.Add(buttonPanel);

            this.Controls.Add(mainLayout);
        }

        private void OnSaveClicked(string teamName)
        {
            var players = playerControls.Select(p => p.GetPlayer()).ToList();
            var captains = players.Where(p => p.IsCaptain).ToList();

            if (captains.Count != 1)
            {
                MessageBox.Show("Команда должна иметь ровно одного капитана!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var rankValues = new Dictionary<string, int> {
                ["Unranked"] = 0, ["Iron"] = 1, ["Bronze"] = 2, ["Silver"] = 3, ["Gold"] = 4
            };

            var sorted = players.OrderByDescending(p => rankValues[p.Rank]).ToList();
            double avg = sorted.Average(p => rankValues[p.Rank]);

            var summary = new TeamSummaryForm(teamName, sorted, avg);
            summary.StartPosition = FormStartPosition.CenterScreen;
            summary.Show();
        }
    }

    public class Player
    {
        public string Role { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public bool IsCaptain { get; set; }
    }
}