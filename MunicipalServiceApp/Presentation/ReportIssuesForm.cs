#nullable enable

using MunicipalServiceApp.Application.Abstractions;
using MunicipalServiceApp.Domain;
using MunicipalServiceApp.Domain.DataStructures;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MunicipalServiceApp.Presentation
{
    public partial class ReportIssuesForm : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        private readonly IIssueService _issueService;
        private readonly IGeocodingService _geo;
        private string _attachedPath = string.Empty;
        private readonly SinglyLinkedList<Attachment> _attachmentList = new SinglyLinkedList<Attachment>();

        private bool _isSubmitting = false;

        private bool _isAddressValid = false;

        private readonly System.Windows.Forms.Timer _addressTypingTimer;
        private const int TypingDelayMs = 1200;

        public ReportIssuesForm() : this(null!, null!) { }

        public ReportIssuesForm(IIssueService issueService, IGeocodingService geo)
        {
            _issueService = issueService ?? throw new ArgumentNullException(nameof(issueService));
            _geo = geo ?? throw new ArgumentNullException(nameof(geo));

            InitializeComponent();
            Text = "Report Issues";
            StartPosition = FormStartPosition.CenterScreen;

            _addressTypingTimer = new System.Windows.Forms.Timer
            {
                Interval = TypingDelayMs
            };

            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = MaximizeBox = true;
            MinimumSize = new System.Drawing.Size(1000, 700);

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            Shown += (_, __) =>
            {
                if (!DesignMode)
                    WindowState = FormWindowState.Maximized;

                CenterCard();
            };
            SizeChanged += (_, __) => CenterCard();

            WireProgressEvents();
            SetupTypingTimer();
        }

        private void SetupTypingTimer()
        {
            _addressTypingTimer.Tick += async (s, e) =>
            {
                _addressTypingTimer.Stop();
                await ValidateAddress(txtLocation.Text.Trim());
            };
        }

        private void ReportIssuesForm_Load(object? sender, EventArgs e)
        {
            prgEngagement.Minimum = 0;
            prgEngagement.Maximum = 100;
            prgEngagement.Value = 0;
            lblAttachmentPath.Text = "No file selected";
            lblStatus.Text = "Awaiting submission…";

            _isAddressValid = false;
            RefreshProgress();

            PopulateCategories();
        }

        private void PopulateCategories()
        {
            cmbCategory.DataSource = null;
            cmbCategory.Items.Clear();

            foreach (var c in Domain.Categories.All())
                cmbCategory.Items.Add(c);

            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void WireProgressEvents()
        {
            txtLocation.TextChanged += (s, e) =>
            {
                _isAddressValid = false;
                RefreshProgress();
                _addressTypingTimer.Stop();
                _addressTypingTimer.Start();
            };

            rtbDescription.TextChanged += OnAnyInputChanged;
            cmbCategory.SelectedIndexChanged += OnAnyInputChanged;
        }

        private void OnAnyInputChanged(object? sender, EventArgs e) => RefreshProgress();

        private void RefreshProgress()
        {
            if (_isSubmitting) return;

            int p = 0;
            string statusTip = "Awaiting submission…";
            Color statusColor = Color.Blue;

            if (string.IsNullOrEmpty(txtLocation.Text.Trim()))
            {
                statusTip = "Tip: Enter a location.";
                statusColor = Color.Blue;
            }
            else if (!_isAddressValid)
            {
                statusTip = "Tip: Enter a valid address.";
                statusColor = Color.Red;
            }
            else
            {
                p += 30;
                statusTip = "Address validated successfully.";
                statusColor = Color.Green;
            }

            if (p >= 30 && cmbCategory.SelectedIndex < 0)
            {
                statusTip = "Tip: Select a category.";
                statusColor = Color.Blue;
            }
            else if (cmbCategory.SelectedIndex >= 0)
            {
                p += 20;
            }

            if (p >= 50 && (rtbDescription.Text?.Trim().Length ?? 0) < 10)
            {
                statusTip = "Tip: Enter a description (min 10 characters).";
                statusColor = Color.Blue;
            }
            else if ((rtbDescription.Text?.Trim().Length ?? 0) >= 10)
            {
                p += 30;
            }

            if (p >= 80 && string.IsNullOrEmpty(_attachedPath))
            {
                statusTip = "Tip: Attach a photo or document.";
                statusColor = Color.Blue;
            }
            else if (!string.IsNullOrEmpty(_attachedPath))
            {
                p += 20;
            }

            lblStatus.Text = statusTip;
            lblStatus.ForeColor = statusColor;
            prgEngagement.Value = Math.Max(0, Math.Min(100, p));
        }

        private async Task ValidateAddress(string rawAddress)
        {
            lblStatus.Text = "Validating address...";
            lblStatus.ForeColor = Color.DarkOrange;

            var addr = await _geo.ValidateAsync(rawAddress);
            _isAddressValid = addr.Success;

            RefreshProgress();
        }

        private void btnAttach_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Attach a photo or document",
                Filter = "Images or PDF|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.pdf|All Files|*.*",
                Multiselect = false
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _attachedPath = dlg.FileName;
                lblAttachmentPath.Text = _attachedPath;
                RefreshProgress();
            }
        }

        private async void btnSubmit_Click(object? sender, EventArgs e)
        {
            _isSubmitting = true;

            var startPct = prgEngagement.Value;
            lblStatus.Text = "Validating address…";

            var rawAddress = txtLocation.Text?.Trim() ?? string.Empty;
            var addr = await _geo.ValidateAsync(rawAddress);
            if (!addr.Success)
            {
                MessageBox.Show(addr.ErrorMessage, "Invalid Address",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _isSubmitting = false;
                RefreshProgress();
                txtLocation.Focus();
                return;
            }

            var issue = new Issue
            {
                Location = addr.NormalizedAddress,
                Category = cmbCategory.SelectedItem?.ToString() ?? string.Empty,
                Description = rtbDescription.Text?.Trim() ?? string.Empty,
                AttachmentPath = _attachedPath
            };

            lblStatus.Text = "Validating…";
            var result = _issueService.CreateIssue(issue);
            if (!result.Success)
            {
                MessageBox.Show(result.ErrorMessage, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _isSubmitting = false;
                RefreshProgress();
                return;
            }

            lblStatus.Text = "Submitting…";
            await AnimateProgressBarAsync(startPct, 100, 600);

            var token = result.Value ?? "(unavailable)";
            lblStatus.Text = $"Issue submitted. Tracking #: {token}";

            try { Clipboard.SetText(token); } catch { }

            MessageBox.Show(
                $"Your report has been logged successfully.\n\nTracking #: {token}\n\n" +
                "The tracking number has been copied to your clipboard.",
                "Submitted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            txtLocation.Clear();
            rtbDescription.Clear();
            cmbCategory.SelectedIndex = -1;
            _attachedPath = string.Empty;
            lblAttachmentPath.Text = "No file selected";
            _isAddressValid = false;
            _isSubmitting = false;

            RefreshProgress();
            txtLocation.Focus();
        }

        private async Task AnimateProgressBarAsync(int from, int to, int durationMs)
        {
            if (to < from) (from, to) = (to, from);
            int steps = 20;
            int delay = Math.Max(1, durationMs / steps);
            for (int i = 0; i <= steps; i++)
            {
                int val = from + (int)((to - from) * (i / (double)steps));
                prgEngagement.Value = Math.Min(prgEngagement.Maximum, Math.Max(prgEngagement.Minimum, val));
                await Task.Delay(delay);
            }
        }

        private void btnBack_Click(object? sender, EventArgs e) => Close();

        private void CenterCard()
        {
            if (body == null || pnlCard == null) return;

            var x = Math.Max(0, (body.ClientSize.Width - pnlCard.Width) / 2);
            pnlCard.Left = x;

            pnlCard.Top = 16;
        }
    }
}