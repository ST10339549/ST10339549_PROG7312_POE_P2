using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MunicipalServiceApp.Presentation
{
    partial class MainMenuForm
    {
        private System.ComponentModel.IContainer? components = null;

        private Panel header = null!;
        private Label lblTitle = null!;
        private Label lblSub = null!;
        private Panel pnlMenu = null!;
        private Button btnOpenReport = null!;
        private Button btnLocalEvents = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ===== Header =====
            header = new Panel
            {
                Height = 110,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(92, 71, 173),
                Padding = new Padding(24, 16, 24, 18)
            };

            var headerStack = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent
            };

            lblTitle = new Label
            {
                Text = "Municipal Services",
                ForeColor = Color.White,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 22f),
                Margin = new Padding(0, 0, 0, 2)
            };

            lblSub = new Label
            {
                Text = "Report issues • Discover local events • Track service requests",
                ForeColor = Color.White,
                AutoSize = true,
                Font = new Font("Segoe UI", 11f),
                Margin = new Padding(0)
            };

            headerStack.Controls.Add(lblTitle);
            headerStack.Controls.Add(lblSub);
            header.Controls.Add(headerStack);
            Controls.Add(header);

            // ===== Centered content area =====
            pnlMenu = new Panel
            {
                Width = 820,
                Height = 300,
                Anchor = AnchorStyles.Top, // not Dock; allows manual centering
                BackColor = Color.Transparent
            };
            Controls.Add(pnlMenu);

            // helper to make a rounded "card" with an icon
            Panel MakeCard(string title, string body, Icon icon, EventHandler onClick, bool enabled = true)
            {
                var card = new Panel
                {
                    Width = 260,
                    Height = 220,
                    BackColor = Color.White,
                    Padding = new Padding(16),
                };

                var pic = new PictureBox
                {
                    Image = icon.ToBitmap(),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(28, 28),
                    Location = new Point(16, 14)
                };

                var lblH = new Label
                {
                    Text = title,
                    Font = new Font("Segoe UI Semibold", 13.5f),
                    AutoSize = false,
                    Location = new Point(16 + 34, 12),
                    Size = new Size(260 - 16 - 34 - 16, 30)
                };

                var lblB = new Label
                {
                    Text = body,
                    Font = new Font("Segoe UI", 9.25f),
                    AutoSize = false,
                    Location = new Point(16, 52),
                    Size = new Size(260 - 32, 100)
                };

                var btn = new Button
                {
                    Text = "Open",
                    Image = SystemIcons.Information.ToBitmap(),
                    ImageAlign = ContentAlignment.MiddleLeft,
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    Width = 112,
                    Height = 36,
                    Location = new Point(16, 150),
                    FlatStyle = FlatStyle.Flat,
                    Enabled = enabled,
                    BackColor = enabled ? Color.FromArgb(92, 71, 173) : SystemColors.Control,
                    ForeColor = enabled ? Color.White : SystemColors.ControlText
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += onClick;

                Button btnMyReports = null;
                if (title == "Report Issues")
                {
                    btnMyReports = new Button
                    {
                        Text = "My Reports",
                        Width = 110,
                        Height = 34,
                        Left = btn.Right + 8,
                        Top = btn.Top,
                        FlatStyle = FlatStyle.Flat,
                        BackColor = btn.BackColor,
                        ForeColor = btn.ForeColor,
                        Enabled = btn.Enabled
                    };
                    btnMyReports.FlatAppearance.BorderSize = 0;
                    btnMyReports.Click += btnMyReports_Click;
                }

                card.Controls.Add(pic);
                card.Controls.Add(lblH);
                card.Controls.Add(lblB);
                card.Controls.Add(btn);
                if (btnMyReports != null)
                    card.Controls.Add(btnMyReports);

                // rounded corners
                UiKit.Round(card, 16);
                UiKit.Round(btn, 18);
                if (btnMyReports != null)
                    UiKit.Round(btnMyReports, 18);

                return card;
            }

            // Three cards
            var card1 = MakeCard(
                "Report Issues",
                "Log potholes, water leaks, streetlights and more.\nVerified addresses using maps.",
                SystemIcons.Warning,
                btnReportIssues_Click,
                enabled: true);

            var card2 = MakeCard(
                "Local Events (Part 2)",
                "Browse community events and announcements.",
                SystemIcons.Asterisk,
                btnLocalEvents_Click,
                enabled: true);

            var card3 = MakeCard(
                "Service Request Status (Part 3)",
                "Track the progress of your submitted requests.\nComing soon.",
                SystemIcons.Question,
                (_, __) => { },
                enabled: false);

            // layout cards
            card1.Location = new Point(10, 20);
            card2.Location = new Point(card1.Right + 20, 20);
            card3.Location = new Point(card2.Right + 20, 20);
            pnlMenu.Controls.Add(card1);
            pnlMenu.Controls.Add(card2);
            pnlMenu.Controls.Add(card3);

            // ===== Form =====
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(980, 560);
            BackColor = Color.FromArgb(247, 248, 250);
            Text = "Main Menu";
            MinimumSize = new Size(900, 560);

            // ... centering now handled in MainMenuForm.cs CenterMenu()

            btnLocalEvents = new Button
            {
                Text = "Local Events",
                Enabled = true,
                Size = new Size(120, 40),
                Location = new Point(20, 100),
                BackColor = Color.LightBlue
            };
            btnLocalEvents.Click += btnLocalEvents_Click;
            Controls.Add(btnLocalEvents);
        }

        private static class UiKit
        {
            public static void Round(Control c, int radius)
            {
                void apply(object? _ = null, EventArgs? __ = null)
                {
                    using var path = new GraphicsPath();
                    var r = radius * 2;
                    var rect = new Rectangle(0, 0, c.Width, c.Height);
                    path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                    path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                    path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                    path.CloseAllFigures();
                    c.Region = new Region(path);
                }

                c.SizeChanged += apply;
                if (c.Width > 0 && c.Height > 0) apply();
            }
        }
    }
}
