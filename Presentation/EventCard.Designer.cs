using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServiceApp.Presentation
{
    public static class GraphicsPathExtensions
    {
        public static void AddRoundedRectangle(this System.Drawing.Drawing2D.GraphicsPath path, Rectangle bounds, int cornerRadius)
        {
            path.AddArc(bounds.X, bounds.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            path.AddArc(bounds.Right - cornerRadius * 2, bounds.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            path.AddArc(bounds.Right - cornerRadius * 2, bounds.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            path.CloseFigure();
        }
    }

    partial class EventCard
    {
        private System.ComponentModel.IContainer components = null;

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


            // Main settings
            this.Size = new Size(380, 200);
            this.BackColor = Color.White;
            this.Margin = new Padding(10);
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Padding = new Padding(15);
            this.MinimumSize = new Size(380, 200);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Initialize category tag at the top
            panelCategoryTag = new Panel();
            panelCategoryTag.Size = new Size(80, 25);
            panelCategoryTag.Location = new Point(285, 15);
            panelCategoryTag.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panelCategoryTag.BackColor = Color.LightBlue;
            panelCategoryTag.Paint += (s, e) => 
            {
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddRoundedRectangle(panelCategoryTag.ClientRectangle, 8);
                    panelCategoryTag.Region = new Region(path);
                }
            };
            this.Controls.Add(panelCategoryTag);

            lblCategory = new Label();
            lblCategory.AutoSize = false;
            lblCategory.Size = new Size(70, 17);
            lblCategory.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblCategory.ForeColor = Color.DarkBlue;
            lblCategory.Location = new Point(5, 4);
            lblCategory.TextAlign = ContentAlignment.MiddleCenter;
            panelCategoryTag.Controls.Add(lblCategory);

            // Initialize other labels
            lblTitle = new Label();
            lblTitle.AutoSize = false;
            lblTitle.Size = new Size(270, 45);
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.Location = new Point(15, 15);
            lblTitle.AutoEllipsis = true;
            this.Controls.Add(lblTitle);

            lblDate = new Label();
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblDate.ForeColor = Color.Gray;
            lblDate.Location = new Point(15, 65);
            this.Controls.Add(lblDate);

            lblDescription = new Label();
            lblDescription.AutoSize = true;
            lblDescription.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblDescription.Location = new Point(15, 90);
            lblDescription.MaximumSize = new Size(350, 60);
            lblDescription.AutoEllipsis = true;
            this.Controls.Add(lblDescription);

        }
    }
}