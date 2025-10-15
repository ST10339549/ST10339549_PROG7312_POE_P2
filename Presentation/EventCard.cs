using System;
using System.Drawing;
using System.Windows.Forms;
using MunicipalServiceApp.Domain;

namespace MunicipalServiceApp.Presentation
{
    public partial class EventCard : UserControl
    {
        private Label lblTitle;
        private Label lblDate;
        private Label lblDescription;
        private Label lblCategory;
        private Panel panelCategoryTag;
        private Event _currentEvent;

        public EventCard()
        {
            InitializeComponent();
        }

        public void Populate(Event eventData)
        {
            _currentEvent = eventData;
            lblTitle.Text = eventData.Name;
            lblDate.Text = eventData.Date.ToString("MMMM dd, yyyy");
            lblDescription.Text = eventData.Description;
            lblCategory.Text = eventData.Category;

            // Set category colors and text colors
            switch (eventData.Category.ToLower())
            {
                case "roads":
                    panelCategoryTag.BackColor = Color.FromArgb(255, 242, 230);
                    lblCategory.ForeColor = Color.FromArgb(194, 97, 0);
                    break;
                case "utilities":
                    panelCategoryTag.BackColor = Color.FromArgb(230, 242, 255);
                    lblCategory.ForeColor = Color.FromArgb(0, 97, 194);
                    break;
                case "sanitation":
                    panelCategoryTag.BackColor = Color.FromArgb(230, 255, 242);
                    lblCategory.ForeColor = Color.FromArgb(0, 194, 97);
                    break;
                default:
                    panelCategoryTag.BackColor = Color.FromArgb(242, 242, 242);
                    lblCategory.ForeColor = Color.FromArgb(97, 97, 97);
                    break;
            }

            // Add click handlers
            this.Click += EventCard_Click;
            foreach (Control control in Controls)
            {
                control.Click += EventCard_Click;
            }
        }

        private void EventCard_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Event Details:\n\nName: {_currentEvent.Name}\nDate: {_currentEvent.Date:MMMM dd, yyyy}\nCategory: {_currentEvent.Category}\n\nDescription:\n{_currentEvent.Description}",
                          "Event Details",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
        }
    }
}