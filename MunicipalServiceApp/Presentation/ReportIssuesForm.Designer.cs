using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MunicipalServiceApp.Presentation
{
    partial class ReportIssuesForm
    {
        private System.ComponentModel.IContainer? components = null;

        // Header
        private Panel header = null!;
        private Label lblBannerTitle = null!;
        private Label lblBannerSub = null!;

        // Body / centering
        private Panel body = null!;
        private Panel pnlCard = null!;
        private TableLayoutPanel grid = null!;

        // Controls used by the code-behind
        private Label lblHeading = null!;
        private Label lblLocation = null!;
        private TextBox txtLocation = null!;
        private Label lblCategory = null!;
        private ComboBox cmbCategory = null!;
        private Label lblDescription = null!;
        private RichTextBox rtbDescription = null!;
        private TableLayoutPanel attachRow = null!;
        private Button btnAttach = null!;
        private Label lblAttachmentPath = null!;
        private TableLayoutPanel statusRow = null!;
        private Label lblStatus = null!;
        private ProgressBar prgEngagement = null!;
        private FlowLayoutPanel flButtons = null!;
        private Button btnSubmit = null!;
        private Button btnBack = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            SuspendLayout();

            // ===== Header =====
            header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
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

            lblBannerTitle = new Label
            {
                Text = "Municipal Services",
                ForeColor = Color.White,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 22f),
                Margin = new Padding(0, 0, 0, 2)
            };

            lblBannerSub = new Label
            {
                Text = "Report issues • Discover local events • Track service requests",
                ForeColor = Color.White,
                AutoSize = true,
                Font = new Font("Segoe UI", 11f),
                Margin = new Padding(0)
            };

            headerStack.Controls.Add(lblBannerTitle);
            headerStack.Controls.Add(lblBannerSub);
            header.Controls.Add(headerStack);

            // ===== Body (scroll host) =====
            body = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(247, 248, 250),
                AutoScroll = true
            };

            // ===== Card (rounded, fixed working width) =====
            pnlCard = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(24),
                BackColor = Color.White,
                MinimumSize = new Size(860, 0),
                Width = 860
            };
            body.Controls.Add(pnlCard);
            UiKit.Round(pnlCard, 18);

            // Center card horizontally
            void Recenter(object? _, EventArgs __)
            {
                var x = Math.Max(0, (body.ClientSize.Width - pnlCard.Width) / 2);
                pnlCard.Location = new Point(x, 16);
            }
            body.Resize += Recenter;
            pnlCard.SizeChanged += Recenter;
            Shown += Recenter;

            // ===== Grid =====
            grid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                Padding = new Padding(8),
                MinimumSize = new Size(860 - (24 * 2), 0)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220f));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // 0 heading row
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // 1 location
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // 2 category
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 150f));  // 3 description
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // 4 attach
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // 5 status
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // 6 buttons
            pnlCard.Controls.Add(grid);

            // Heading with icon (span 2)
            var headingRow = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 12)
            };
            var picHead = new PictureBox
            {
                Image = SystemIcons.Warning.ToBitmap(),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(26, 26),
                Margin = new Padding(0, 2, 8, 0)
            };
            lblHeading = new Label
            {
                Text = "Report an Issue",
                Font = new Font("Segoe UI Semibold", 14f),
                AutoSize = true
            };
            headingRow.Controls.Add(picHead);
            headingRow.Controls.Add(lblHeading);
            grid.Controls.Add(headingRow, 0, 0);
            grid.SetColumnSpan(headingRow, 2);

            // Location
            lblLocation = new Label
            {
                Text = "Location",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 4, 12, 4)
            };
            txtLocation = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(0, 0, 0, 8)
            };
            grid.Controls.Add(lblLocation, 0, 1);
            grid.Controls.Add(txtLocation, 1, 1);

            // Category
            lblCategory = new Label
            {
                Text = "Category",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 4, 12, 4)
            };
            cmbCategory = new ComboBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 0, 8)
            };
            grid.Controls.Add(lblCategory, 0, 2);
            grid.Controls.Add(cmbCategory, 1, 2);

            // Description
            lblDescription = new Label
            {
                Text = "Description",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 4, 12, 4)
            };
            rtbDescription = new RichTextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Height = 150,
                Margin = new Padding(0, 0, 0, 10)
            };
            grid.Controls.Add(lblDescription, 0, 3);
            grid.Controls.Add(rtbDescription, 1, 3);

            // Attach row
            attachRow = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 0, 0, 6)
            };
            attachRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            attachRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            btnAttach = new Button
            {
                Text = "Attach Photo/Doc…",
                Image = SystemIcons.Application.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                ImageAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Margin = new Padding(0, 0, 12, 0),
                FlatStyle = FlatStyle.Flat
            };
            btnAttach.FlatAppearance.BorderSize = 0;
            UiKit.Round(btnAttach, 18);
            btnAttach.Click += btnAttach_Click;

            lblAttachmentPath = new Label
            {
                Text = "No file selected",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            };

            attachRow.Controls.Add(btnAttach, 0, 0);
            attachRow.Controls.Add(lblAttachmentPath, 1, 0);
            grid.Controls.Add(attachRow, 0, 4);
            grid.SetColumnSpan(attachRow, 2);

            // Status row
            statusRow = new TableLayoutPanel
            {
                ColumnCount = 1,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 2, 0, 0)
            };
            statusRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            lblStatus = new Label
            {
                Text = "Awaiting submission…",
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 2)
            };

            prgEngagement = new ProgressBar
            {
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Height = 16,
                Dock = DockStyle.Top
            };

            statusRow.Controls.Add(lblStatus, 0, 0);
            statusRow.Controls.Add(prgEngagement, 0, 1);
            grid.Controls.Add(statusRow, 0, 5);
            grid.SetColumnSpan(statusRow, 2);

            // Buttons (rounded)
            flButtons = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0, 8, 0, 0),
                WrapContents = false
            };

            btnSubmit = new Button
            {
                Text = "Submit",
                Image = SystemIcons.Shield.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                ImageAlign = ContentAlignment.MiddleLeft,
                Width = 130,
                Height = 36,
                Margin = new Padding(6, 0, 0, 0),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(92, 71, 173),
                ForeColor = Color.White
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            UiKit.Round(btnSubmit, 18);
            btnSubmit.Click += btnSubmit_Click;

            btnBack = new Button
            {
                Text = "Back to Main Menu",
                Image = SystemIcons.Hand.ToBitmap(),
                TextImageRelation = TextImageRelation.ImageBeforeText,
                ImageAlign = ContentAlignment.MiddleLeft,
                Width = 170,
                Height = 36,
                Margin = new Padding(6, 0, 0, 0),
                FlatStyle = FlatStyle.Flat
            };
            btnBack.FlatAppearance.BorderSize = 0;
            UiKit.Round(btnBack, 18);
            btnBack.Click += btnBack_Click;

            flButtons.Controls.Add(btnSubmit);
            flButtons.Controls.Add(btnBack);
            grid.Controls.Add(flButtons, 0, 6);
            grid.SetColumnSpan(flButtons, 2);

            // ===== Form =====
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 248, 250);
            MinimumSize = new Size(900, 620);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Report Issues";

            Controls.Add(body);
            Controls.Add(header);

            Load += ReportIssuesForm_Load;

            ResumeLayout(false);
        }

        private static class UiKit
        {
            public static void Round(Control c, int radius)
            {
                void apply(object? _ = null, EventArgs? __ = null)
                {
                    if (c.Width <= 0 || c.Height <= 0) return;
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
