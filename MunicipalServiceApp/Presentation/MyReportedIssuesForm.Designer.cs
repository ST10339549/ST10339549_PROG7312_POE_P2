using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServiceApp.Presentation
{
    partial class MyReportedIssuesForm
    {
        private System.ComponentModel.IContainer? components = null;

        // Header
        private Panel header = null!;
        private Label lblTitle = null!;
        private Label lblSub = null!;

        // Body
        private TableLayoutPanel root = null!;
        private GroupBox grp = null!;
        private DataGridView grid = null!;
        private FlowLayoutPanel actions = null!;
        private Button btnRefresh = null!;
        private Button btnOpenAttachment = null!;
        private Button btnBack = null!;

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
                Dock = DockStyle.Top,
                Height = 96,
                BackColor = Color.FromArgb(92, 71, 173),
                Padding = new Padding(24, 16, 24, 16)
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

            var headerStack = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent
            };
            headerStack.Controls.Add(lblTitle);
            headerStack.Controls.Add(lblSub);
            header.Controls.Add(headerStack);

            // ===== Root layout (fills screen) =====
            root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(247, 248, 250),
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(24),
            };
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));   // grid
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));        // buttons

            // ===== Group “card” =====
            grp = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = "My Submitted Reports",
                Font = new Font("Segoe UI", 10f),
                Padding = new Padding(12)
            };

            // ===== DataGrid =====
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            grp.Controls.Add(grid);

            // ===== Actions (bottom-right) =====
            actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true
            };

            btnBack = new Button
            {
                Text = "Back to Main Menu",
                Width = 160,
                Height = 34,
                Margin = new Padding(8, 8, 0, 0)
            };
            btnBack.Click += btnBack_Click;

            btnOpenAttachment = new Button
            {
                Text = "Open Attachment",
                Width = 140,
                Height = 34,
                Margin = new Padding(8, 8, 0, 0)
            };
            btnOpenAttachment.Click += btnOpenAttachment_Click;

            btnRefresh = new Button
            {
                Text = "Refresh",
                Width = 100,
                Height = 34,
                Margin = new Padding(8, 8, 0, 0)
            };
            btnRefresh.Click += btnRefresh_Click;

            actions.Controls.Add(btnBack);
            actions.Controls.Add(btnOpenAttachment);
            actions.Controls.Add(btnRefresh);

            // Compose
            root.Controls.Add(grp, 0, 0);
            root.Controls.Add(actions, 0, 1);

            // ===== Form =====
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Text = "My Submitted Reports";

            Controls.Add(root);
            Controls.Add(header);

            Load += MyReportedIssuesForm_Load;
        }
    }
}