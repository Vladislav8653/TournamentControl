namespace TournamentControl
{
    public partial class ControlForm : Form
    {
        private readonly List<PlayerControl> _playerControls = new();
        private TextBox _txtTeam;
        private ComboBox _cmbLang;

        public ControlForm()
        {
            InitializeComponent();
            InitializeLayout();
            ApplyLoLDarkTheme();
        }

        private void InitializeLayout() // UI дизайн
        {
            this.Text = "Tournament Control";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(1280, 800);
            this.MinimumSize = new Size(1024, 768);
            this.Font = new Font("Segoe UI", 14);

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                AutoScroll = true,
                Padding = new Padding(30),
                AutoSize = true,
            };

            var playersPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 1,
                RowCount = 5,
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 20)
            };

            for (int i = 0; i < 5; i++)
            {
                var pc = new PlayerControl { Dock = DockStyle.Top, Margin = new Padding(0, 15, 0, 15) };
                _playerControls.Add(pc);
                playersPanel.Controls.Add(pc);
            }

            var teamPanel = new TableLayoutPanel { ColumnCount = 4, AutoSize = true, Dock = DockStyle.Top, Padding = new Padding(0, 0, 0, 20) };
            teamPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            teamPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            teamPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            teamPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            teamPanel.Controls.Add(new Label { Text = "Team:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            _txtTeam = new TextBox { Width = 200, Anchor = AnchorStyles.Left };
            teamPanel.Controls.Add(_txtTeam, 1, 0);

            teamPanel.Controls.Add(new Label { Text = "Language:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 0);
            _cmbLang = new ComboBox { Width = 150, Anchor = AnchorStyles.Left, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbLang.Items.AddRange(new string[] { "English", "Polish", "Spanish" });
            _cmbLang.SelectedIndex = 0;
            teamPanel.Controls.Add(_cmbLang, 3, 0);

            var btnSave = new Button { Text = "Save", AutoSize = true };
            var btnExit = new Button { Text = "Exit", AutoSize = true };
            btnExit.Click += (s, e) => this.Close();
            btnSave.Click += (s, e) => OnSaveClicked(_txtTeam.Text);
            this.AcceptButton = btnSave;
            this.CancelButton = btnExit;

            var buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10),
                Margin = new Padding(0, 20, 0, 0)
            };
            buttonPanel.Controls.AddRange(new Control[] { btnSave, btnExit });

            mainLayout.Controls.Add(playersPanel);
            mainLayout.Controls.Add(teamPanel);
            mainLayout.Controls.Add(buttonPanel);

            this.Controls.Add(mainLayout);
        }

        private void OnSaveClicked(string teamName) // обработчик кнопки Save
        {
            var players = _playerControls.Select(p => p.GetPlayer()).ToList();
            var captains = players.Where(p => p.IsCaptain).ToList();

            if (captains.Count != 1)
            {
                MessageBox.Show("The team must have exactly one captain!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var rankValues = new Dictionary<string, int> {
                ["----"] = 0, ["Iron"] = 1, ["Bronze"] = 2, ["Silver"] = 3, ["Gold"] = 4
            };

            var sorted = players.OrderByDescending(p => rankValues[p.Rank]).ToList();
            double avg = sorted.Average(p => rankValues[p.Rank]);

            var summary = new TeamSummaryForm(teamName, sorted, avg);
            summary.StartPosition = FormStartPosition.CenterScreen;
            summary.Show();
        }

        private void ApplyLoLDarkTheme()
        {
            Color background = Color.FromArgb(18, 18, 18);
            Color foreground = Color.Goldenrod;
            Color inputBackground = Color.FromArgb(35, 35, 35);

            this.BackColor = background;
            void ApplyTheme(Control control)
            {
                control.BackColor = control is TextBox or ComboBox ? inputBackground : background;
                control.ForeColor = foreground;

                foreach (Control child in control.Controls)
                    ApplyTheme(child);
            }
            ApplyTheme(this);
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