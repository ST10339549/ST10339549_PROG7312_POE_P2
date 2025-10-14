#nullable enable

using MunicipalServiceApp.Application.Abstractions;
using MunicipalServiceApp.Domain;
using MunicipalServiceApp.Domain.DataStructures;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MunicipalServiceApp.Presentation
{
    public partial class MyReportedIssuesForm : Form
    {
        private readonly IIssueService _issueService;

        public MyReportedIssuesForm(IIssueService issueService)
        {
            _issueService = issueService ?? throw new ArgumentNullException(nameof(issueService));
            InitializeComponent();

            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;

            ApplyLayoutFixes();
        }

        private void MyReportedIssuesForm_Load(object? sender, EventArgs e)
        {
            if (grid.Columns.Count == 0)
            {
                grid.Columns.Clear();
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Tracking",
                    HeaderText = "Tracking #",
                    DataPropertyName = "TrackingNumber",
                    FillWeight = 18
                });
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Location",
                    HeaderText = "Location",
                    DataPropertyName = "Location",
                    FillWeight = 26
                });
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Category",
                    HeaderText = "Category",
                    DataPropertyName = "Category",
                    FillWeight = 14
                });
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Description",
                    HeaderText = "Description",
                    DataPropertyName = "Description",
                    FillWeight = 30
                });
                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Attachment",
                    HeaderText = "Attachment",
                    DataPropertyName = "AttachmentPath",
                    FillWeight = 12
                });
            }

            ConfigureGridLook();
            ReloadGrid();

            Shown += (_, __) => ApplyLayoutFixes();
            SizeChanged += (_, __) => ApplyLayoutFixes();
        }

        private void ReloadGrid()
        {
            var issues = _issueService.All();
            var issueList = new SinglyLinkedList<Issue>();
            foreach (var issue in issues)
            {
                issueList.AddLast(issue);
            }
            grid.DataSource = issueList.ToList();

            ConfigureColumnWeights();
            if (grid.Rows.Count > 0) grid.ClearSelection();
        }

        private void btnRefresh_Click(object? sender, EventArgs e) => ReloadGrid();

        private void btnOpenAttachment_Click(object? sender, EventArgs e)
        {
            if (grid.CurrentRow?.DataBoundItem is not Issue issue) return;

            var path = issue.AttachmentPath ?? string.Empty;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                MessageBox.Show("This report has no attachment (or the file path is no longer valid).",
                    "No Attachment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open the file.\n\n{ex.Message}",
                    "Open Attachment", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object? sender, EventArgs e) => Close();

        private void ApplyLayoutFixes()
        {
            FixHeaderHeight();
            header?.PerformLayout();
            this?.PerformLayout();
        }

        private void FixHeaderHeight()
        {
            if (header == null || lblSub == null) return;

            var desiredHeight = lblSub.Bottom + 16;
            header.Height = desiredHeight;
        }

        private void ConfigureGridLook()
        {
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.ColumnHeadersHeight = 36;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 6, 6, 6);

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AllowUserToResizeColumns = true;

            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            ConfigureColumnWeights();
        }

        private void ConfigureColumnWeights()
        {
            void weight(string name, float w)
            {
                if (grid.Columns.Contains(name))
                    grid.Columns[name].FillWeight = w;
            }

            weight("Tracking", 110);
            weight("Location", 220);
            weight("Category", 140);
            weight("Description", 360);
            weight("Attachment", 170);
        }
    }
}