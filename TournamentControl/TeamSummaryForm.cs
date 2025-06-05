namespace TournamentControl
{
    public class TeamSummaryForm : Form
    {
        public TeamSummaryForm(string teamName, List<Player> players, double avgEffectiveness)
        {
            this.Text = $"{teamName} — Эффективность: {avgEffectiveness:F2}";
            this.Font = new System.Drawing.Font("Segoe UI", 14);
            this.Width = 600;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            var listBox = new ListBox { Dock = DockStyle.Fill, Font = this.Font };
            foreach (var p in players)
            {
                var prefix = p.IsCaptain ? "*" : " ";
                listBox.Items.Add($"{prefix}{p.Name} ({p.Rank})");
            }

            this.Controls.Add(listBox);
        }
    }
}