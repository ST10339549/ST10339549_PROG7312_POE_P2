using System.Windows.Forms;

namespace MunicipalServiceApp.Presentation
{
    partial class LocalEventsForm
    {
        private System.ComponentModel.IContainer components = null;
        private ListBox listBoxEvents;
        private Label lblRecommendations;
        private TextBox txtSearch;
        private ComboBox cmbSort;

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
            this.listBoxEvents = new ListBox();
            this.lblRecommendations = new Label();
            this.txtSearch = new TextBox();
            this.cmbSort = new ComboBox();

            // listBoxEvents
            this.listBoxEvents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.listBoxEvents.Location = new System.Drawing.Point(20, 20);
            this.listBoxEvents.Size = new System.Drawing.Size(this.ClientSize.Width - 40, this.ClientSize.Height / 2);

            // lblRecommendations
            this.lblRecommendations.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblRecommendations.Location = new System.Drawing.Point(20, this.ClientSize.Height / 2 + 30);
            this.lblRecommendations.Size = new System.Drawing.Size(this.ClientSize.Width - 40, 40);
            this.lblRecommendations.Text = "Recommendations";

            // txtSearch
            this.txtSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtSearch.Location = new System.Drawing.Point(20, this.ClientSize.Height / 2 + 80);
            this.txtSearch.Size = new System.Drawing.Size(this.ClientSize.Width - 40, 20);

            // cmbSort
            this.cmbSort.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.cmbSort.Location = new System.Drawing.Point(20, this.ClientSize.Height / 2 + 110);
            this.cmbSort.Size = new System.Drawing.Size(this.ClientSize.Width - 40, 20);

            // LocalEventsForm
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.listBoxEvents);
            this.Controls.Add(this.lblRecommendations);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cmbSort);
            this.Text = "Local Events";
            this.WindowState = FormWindowState.Maximized;
        }
    }
}