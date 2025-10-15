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
            AddEvent(1, "Potholes in Durban", "Roads", new DateTime(2025, 9, 12), "Pothole repairs in Durban from 10:00 till 12:00");
            AddEvent(2, "Water Maintenance in Durban", "Utilities", new DateTime(2025, 10, 14), "Electricity maintenance in Durban from 08:00 till 12:00");
            AddEvent(3, "Garbage Collection Delayed", "Sanitation", new DateTime(2025, 9, 12), "Garbage collection in Roodepoort delayed to 12:00");
            AddEvent(4, "Potholes in Braamfontein", "Roads", new DateTime(2025, 10, 11), "Road maintenance in Braamfontein from 09:00 till 15:00");
            AddEvent(5, "No water in Sandton", "Utilities", new DateTime(2025, 10, 14), "No water supply in Sandton from 13:00 till 17:00");
            AddEvent(6, "Community Park Cleanup", "Community", new DateTime(2025, 10, 18), "Volunteer cleanup event at Central Park from 09:00.");
            AddEvent(7, "Planned Power Outage: Hillcrest", "Utilities", new DateTime(2025, 10, 22), "Essential maintenance will cause a power outage from 10:00 to 16:00.");
            AddEvent(8, "New Traffic Light Installation", "Roads", new DateTime(2025, 10, 25), "Installation at the corner of Main and Broad street. Expect delays.");
            AddEvent(9, "Recycling Drive", "Sanitation", new DateTime(2025, 10, 28), "Special collection for recyclable materials. Please leave bins out by 07:00.");
            AddEvent(10, "Local Library Book Fair", "Events", new DateTime(2025, 11, 4), "Annual book fair to raise funds for the children's section.");
            AddEvent(11, "Fire Safety Workshop", "Safety", new DateTime(2025, 11, 9), "Free workshop at the Durban North Fire Station at 11:00.");
            AddEvent(12, "Emergency Water Pipe Repair", "Utilities", new DateTime(2025, 11, 12), "Urgent repairs on Smith Street may cause low water pressure.");
            AddEvent(13, "Road Resurfacing on M4 Highway", "Roads", new DateTime(2025, 11, 15), "Lane closures on M4 South between 09:00 and 15:00 for resurfacing.");
            AddEvent(14, "Town Hall Meeting", "Community", new DateTime(2025, 11, 20), "Open meeting to discuss the new municipal budget at 18:00.");
            AddEvent(15, "Annual Holiday Market", "Events", new DateTime(2025, 12, 5), "Join us for the festive holiday market at the City Hall gardens.");

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

            List<Event> recommendationsToShow;

            if (searchFrequency.Count == 0)
            {
                // Default: show the 3 most recent events if no search history
                recommendationsToShow = events.Values.OrderByDescending(ev => ev.Date).Take(3).ToList();
            }
            else
            {
                // Find the most frequently searched category
                string favoriteCategory = searchFrequency.OrderByDescending(kvp => kvp.Value).First().Key;

                // Recommend events from the favorite category, excluding already displayed events
                recommendationsToShow = events.Values
                    .Where(ev => ev.Category == favoriteCategory)
                    .Take(3)
                    .ToList();
            }

            foreach (var ev in recommendationsToShow)
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

                    // Track search frequency for recommendations
                    if (searchFrequency.ContainsKey(selectedCategory))
                    {
                        searchFrequency[selectedCategory]++;
                    }
                    else
                    {
                        searchFrequency[selectedCategory] = 1;
                    }
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

            // Update recommendations based on current filters
            UpdateRecommendations(null); // Pass null as the method now determines recommendations internally
        }

        private void btnBackToMainMenu_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the LocalEventsForm
            Owner.Show(); // Show the MainMenuForm
        }
    }
}