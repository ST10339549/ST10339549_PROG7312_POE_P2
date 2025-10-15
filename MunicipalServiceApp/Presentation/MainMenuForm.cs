using MunicipalServiceApp.Application.Abstractions;
using MunicipalServiceApp.Presentation;
using System;
using System.Windows.Forms;

namespace MunicipalServiceApp.Presentation
{
    public partial class MainMenuForm : Form
    {
        private readonly IIssueService _issueService;
        private readonly IGeocodingService _geo;

        public MainMenuForm() : this(null!, null!) { }

        public MainMenuForm(IIssueService issueService, IGeocodingService geo)
        {
            _issueService = issueService ?? throw new ArgumentNullException(nameof(issueService));
            _geo = geo ?? throw new ArgumentNullException(nameof(geo));

            InitializeComponent();

            Text = "Municipal Services - Main Menu";
            StartPosition = FormStartPosition.CenterScreen;

            // Smooth repaint when resizing
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            // Let the window be resizable/movable after restore
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = MaximizeBox = true;
            MinimumSize = new Size(1000, 650);

            // Open maximized (done on Shown so Designer isn’t affected)
            Shown += (_, __) =>
            {
                if (!DesignMode)
                {
                    WindowState = FormWindowState.Maximized;
                }
            };

            // Keep the menu centered when users resize/restore the window
            SizeChanged += (_, __) => CenterMenu();
        }

        private void MainMenuForm_Load(object? sender, EventArgs e)
        {
            // initial center for first layout pass
            CenterMenu();

            btnLocalEvents.Enabled = true;
        }

        private void CenterMenu()
        {
            if (pnlMenu == null || header == null) return;

            pnlMenu.Left = Math.Max(0, (ClientSize.Width - pnlMenu.Width) / 2);

            var desiredTop = header.Bottom + 16;
            pnlMenu.Top = Math.Max(desiredTop, (ClientSize.Height - pnlMenu.Height) / 2);
        }

        private void btnReportIssues_Click(object? sender, EventArgs e)
        {
            using var frm = new ReportIssuesForm(_issueService, _geo);
            Hide();
            frm.ShowDialog(this);
            Show();
        }

        private void btnMyReports_Click(object? sender, EventArgs e)
        {
            using var frm = new MyReportedIssuesForm(_issueService);
            Hide();
            frm.ShowDialog(this);
            Show();
        }

        private void btnLocalEvents_Click(object? sender, EventArgs e)
        {
            using var frm = new LocalEventsForm();
            Hide();
            frm.ShowDialog(this);
            Show();
        }

        private void btnServiceStatus_Click(object? sender, EventArgs e)
        {
            // Disabled in Part 1
        }
    }
}
