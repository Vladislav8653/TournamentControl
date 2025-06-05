namespace TournamentControl
{
    public class PlayerControl : UserControl
    {
        private ComboBox cmbRole = new();
        private TextBox txtName = new();
        private ComboBox cmbRank = new();
        private CheckBox chkCaptain = new();

        public PlayerControl()
        {
            this.Height = 70;
            this.Dock = DockStyle.Top;
            this.Margin = new Padding(5);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 8,
                AutoSize = true
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));

            cmbRole.Items.AddRange(new string[] { "Top", "Jungle", "Mid", "ADC", "Support" });
            cmbRole.SelectedIndex = 0;
            cmbRank.Items.AddRange(new string[] { "Unranked", "Iron", "Bronze", "Silver", "Gold" });
            cmbRank.SelectedIndex = 0;

            chkCaptain.AutoSize = true;
            chkCaptain.Text = "  Captain"; // Padding workaround for font clipping
            chkCaptain.Padding = new Padding(5, 5, 5, 5);

            txtName.Width = 150;

            layout.Controls.Add(new Label { Text = "Role:", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 0);
            layout.Controls.Add(cmbRole, 1, 0);
            layout.Controls.Add(new Label { Text = "Name:", Anchor = AnchorStyles.Left, AutoSize = true }, 2, 0);
            layout.Controls.Add(txtName, 3, 0);
            layout.Controls.Add(new Label { Text = "Rank:", Anchor = AnchorStyles.Left, AutoSize = true }, 4, 0);
            layout.Controls.Add(cmbRank, 5, 0);
            layout.Controls.Add(chkCaptain, 6, 0);

            this.Controls.Add(layout);
        }

        public Player GetPlayer()
        {
            return new Player
            {
                Role = cmbRole.SelectedItem?.ToString() ?? "",
                Name = txtName.Text,
                Rank = cmbRank.SelectedItem?.ToString() ?? "Unranked",
                IsCaptain = chkCaptain.Checked
            };
        }
    }
}