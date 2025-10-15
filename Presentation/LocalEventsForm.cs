using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MunicipalServiceApp.Domain;
using MunicipalServiceApp.Presentation;

namespace MunicipalServiceApp.Presentation
{
    public partial class LocalEventsForm : Form
    {
        private SortedDictionary<int, Event> events = new SortedDictionary<int, Event>();

        private HashSet<string> eventCategories = new HashSet<string>();

        private Stack<Event> lastViewedEvents = new Stack<Event>();

        private Dictionary<string, int> searchFrequency = new Dictionary<string, int>();

        public LocalEventsForm()
        {
            InitializeComponent();
        }

        private void LocalEventsForm_Load(object sender, EventArgs e)
        {
            AddEvent(1, "Potholes in Benoni", "Roads", new DateTime(2025, 9, 12), "Pothole repairs in Benoni from 10:00 till 12:00");
            AddEvent(2, "Water Maintenance in Benoni", "Utilities", new DateTime(2025, 10, 14), "Electricity maintenance in Benoni from 08:00 till 12:00");
            AddEvent(3, "Garbage Collection Delayed", "Sanitation", new DateTime(2025, 9, 12), "Garbage collection in Roodepoort delayed to 12:00");
            AddEvent(4, "Potholes in Braamfontein", "Roads", new DateTime(2025, 10, 11), "Road maintenance in Braamfontein from 09:00 till 15:00");
            AddEvent(5, "No water in Sandton", "Utilities", new DateTime(2025, 10, 14), "Water outage in Sandton area");

            // Populate events in ListBox
            UpdateEventList(events.Values.ToList());

            // Populate category options
            categoryCombo.Items.AddRange(new string[] { "All Categories", "Roads", "Utilities", "Sanitation" });
            categoryCombo.SelectedIndex = 0;

            // Set default date
            datePicker.Value = DateTime.Today;

            // Hook up event handlers
            txtSearch.TextChanged += TxtSearch_TextChanged;
            categoryCombo.SelectedIndexChanged += CategoryCombo_SelectedIndexChanged;
            btnSearch.Click += BtnSearch_Click;

            // Display recommendations
            var recommendedEvents = events.Values.OrderByDescending(ev => ev.Date).Take(3).ToList();
            UpdateRecommendations(recommendedEvents);
        }

        private void AddEvent(int id, string name, string category, DateTime date, string description)
        {
            var newEvent = new Event { Id = id, Name = name, Category = category, Date = date, Description = description };
            events.Add(id, newEvent);
            eventCategories.Add(category);
        }

        private void DisplayEvents(List<Event> eventsToDisplay)
        {
            flpEvents.Controls.Clear();

            foreach (var ev in eventsToDisplay)
            {
                var card = new EventCard();
                card.Populate(ev);
                flpEvents.Controls.Add(card);
            }
        }

        private void UpdateEventList(List<Event> eventList)
        {
            flpEvents.Controls.Clear();
            foreach (var ev in eventList)
            {
                var card = new EventCard();
                card.Populate(ev);
                flpEvents.Controls.Add(card);
            }
        }

        private void UpdateRecommendations(List<Event> recommendedEvents)
        {
            flpRecommendations.Controls.Clear();

            foreach (var ev in recommendedEvents)
            {
                var card = new EventCard();
                card.Populate(ev);
                flpRecommendations.Controls.Add(card);
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            var filteredEvents = events.Values.Where(ev => ev.Name.ToLower().Contains(searchTerm) || ev.Category.ToLower().Contains(searchTerm)).ToList();
            DisplayEvents(filteredEvents);
        }

        private void CategoryCombo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            FilterAndDisplayEvents();
        }

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            FilterAndDisplayEvents();
        }

        private void FilterAndDisplayEvents()
        {
            var filteredEvents = events.Values.AsEnumerable();

            // Apply category filter
            if (categoryCombo.SelectedIndex > 0 && categoryCombo.SelectedItem != null) // Skip "All Categories"
            {
                string selectedCategory = categoryCombo.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(selectedCategory))
                {
                    filteredEvents = filteredEvents.Where(ev => ev.Category == selectedCategory);
                }
            }

            // Apply date filter
            var selectedDate = datePicker.Value.Date;
            filteredEvents = filteredEvents.Where(ev => ev.Date.Date == selectedDate);

            // Apply search text filter
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string searchTerm = txtSearch.Text.ToLower();
                filteredEvents = filteredEvents.Where(ev => 
                    ev.Name.ToLower().Contains(searchTerm) || 
                    ev.Description.ToLower().Contains(searchTerm));
            }

            // Display filtered events
            DisplayEvents(filteredEvents.OrderByDescending(ev => ev.Date).ToList());
        }

        private void btnBackToMainMenu_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the LocalEventsForm
            Owner.Show(); // Show the MainMenuForm
        }
    }
}