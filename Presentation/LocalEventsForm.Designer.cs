using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServiceApp.Presentation
{
    partial class LocalEventsForm
    {
        private System.ComponentModel.IContainer components = null;
        private FlowLayoutPanel flpEvents;
        private FlowLayoutPanel flpRecommendations;
        private TextBox txtSearch;
        private Button btnSearch;
        private ComboBox categoryCombo;
        private DateTimePicker datePicker;
        private Label lblRecommendations;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Initialize components
            this.flpEvents = new FlowLayoutPanel
            {
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.WhiteSmoke
            };

            this.flpRecommendations = new FlowLayoutPanel
            {
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Height = 150,
                Dock = DockStyle.Bottom,
                Padding = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            this.txtSearch = new TextBox();
            this.btnSearch = new Button();
            this.categoryCombo = new ComboBox();
            this.datePicker = new DateTimePicker();
            
            this.lblRecommendations = new Label
            {
                Text = "Recommended Events",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Bottom,
                Padding = new Padding(10),
                Height = 40
            };

            // Form setup
            this.Text = "Local Events and Announcements";
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Padding = new Padding(20);

            // Search panel
            var searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Search label
            var lblSearch = new Label
            {
                Text = "Events and Announcements",
                Font = new Font("Segoe UI", 14),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Configure search controls
            this.txtSearch.Size = new Size(300, 30);
            this.txtSearch.Font = new Font("Segoe UI", 12);
            this.txtSearch.PlaceholderText = "Search events or announcements...";

            this.categoryCombo.Size = new Size(200, 30);
            this.categoryCombo.Font = new Font("Segoe UI", 12);
            this.categoryCombo.Text = "All Categories";
            this.categoryCombo.DropDownStyle = ComboBoxStyle.DropDownList;

            this.datePicker.Size = new Size(200, 30);
            this.datePicker.Font = new Font("Segoe UI", 12);
            this.datePicker.Format = DateTimePickerFormat.Short;

            this.btnSearch.Text = "Search";
            this.btnSearch.Size = new Size(100, 30);
            this.btnSearch.BackColor = Color.RoyalBlue;
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.FlatStyle = FlatStyle.Flat;

            // Search layout
            var searchLayout = new FlowLayoutPanel
            {
                Location = new Point(20, 50),
                Size = new Size(1100, 40),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            // Add controls to search layout
            searchLayout.Controls.AddRange(new Control[] { 
                this.txtSearch, 
                this.categoryCombo, 
                this.datePicker, 
                this.btnSearch 
            });

            // Add spacing for search controls
            searchLayout.SetFlowBreak(txtSearch, false);
            searchLayout.SetFlowBreak(categoryCombo, false);
            searchLayout.SetFlowBreak(datePicker, false);
            
            // Add padding between controls
            foreach (Control control in searchLayout.Controls)
            {
                control.Margin = new Padding(0, 0, 10, 0);
            }

            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(searchLayout);

            // Create table layout for main content
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Search panel
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Main content

            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70)); // Events
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); // Recommendations

            mainLayout.Controls.Add(searchPanel, 0, 0);
            mainLayout.SetColumnSpan(searchPanel, 2);

            mainLayout.Controls.Add(flpEvents, 0, 1);

            // Recommendations Panel
            var recommendationsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10)
            };

            var lblRecTitle = new Label
            {
                Text = "⭐ Recommended for You",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(217, 132, 0),
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(0, 0, 0, 10) // Add padding below the title
            };

            recommendationsPanel.Controls.Add(lblRecTitle);
            recommendationsPanel.Controls.Add(flpRecommendations);
            flpRecommendations.Dock = DockStyle.Fill;
            flpRecommendations.BackColor = Color.Transparent;
            flpRecommendations.Margin = new Padding(0, 20, 0, 0); // Increased top margin

            mainLayout.Controls.Add(recommendationsPanel, 1, 1);

            this.Controls.Add(mainLayout);

            // Event handlers
            this.Load += new EventHandler(LocalEventsForm_Load);
            this.ResumeLayout(false);
        }
    }
}