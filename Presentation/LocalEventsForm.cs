using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MunicipalServiceApp.Domain;

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
            AddEvent(1, "Concert in the Park", "Music", new DateTime(2025, 11, 20), "Live music event.");
            AddEvent(2, "Local Marathon", "Sports", new DateTime(2025, 11, 22), "Annual city marathon.");
            AddEvent(3, "Farmers Market", "Community", new DateTime(2025, 11, 23), "Fresh local produce.");
            AddEvent(4, "Art Exhibition", "Art", new DateTime(2025, 11, 25), "Showcasing local artists.");
            AddEvent(5, "Tech Conference", "Technology", new DateTime(2025, 11, 27), "Latest in tech innovations.");
            AddEvent(6, "Book Fair", "Literature", new DateTime(2025, 11, 29), "Explore new books and authors.");
            AddEvent(7, "Food Festival", "Culinary", new DateTime(2025, 12, 1), "Taste dishes from around the world.");
            AddEvent(8, "Charity Run", "Community", new DateTime(2025, 12, 3), "Run for a cause.");
            AddEvent(9, "Jazz Night", "Music", new DateTime(2025, 12, 5), "Evening of smooth jazz.");
            AddEvent(10, "Coding Bootcamp", "Education", new DateTime(2025, 12, 7), "Learn to code in a day.");
            AddEvent(11, "Yoga Retreat", "Health", new DateTime(2025, 12, 9), "Relax and rejuvenate.");
            AddEvent(12, "Photography Workshop", "Art", new DateTime(2025, 12, 11), "Improve your photography skills.");
            AddEvent(13, "Startup Pitch Night", "Business", new DateTime(2025, 12, 13), "Showcase your startup ideas.");
            AddEvent(14, "Holiday Parade", "Community", new DateTime(2025, 12, 15), "Celebrate the holiday season.");
            AddEvent(15, "New Year's Gala", "Celebration", new DateTime(2025, 12, 31), "Ring in the new year in style.");

            // Populate listBoxEvents with event data
            foreach (var eventItem in events.Values)
            {
                listBoxEvents.Items.Add($"{eventItem.Name} - {eventItem.Category} ({eventItem.Date.ToShortDateString()})");
            }

            // Populate cmbSort with sorting options
            cmbSort.Items.AddRange(new string[] { "Name", "Category", "Date" });
            cmbSort.SelectedIndex = 0;

            // Display recommendations
            lblRecommendations.Text = "Recommended: Concert in the Park, Local Marathon";
        }

        private void AddEvent(int id, string name, string category, DateTime date, string description)
        {
            var newEvent = new Event { Id = id, Name = name, Category = category, Date = date, Description = description };
            events.Add(id, newEvent);
            eventCategories.Add(category);
        }

        private void DisplayEvents(List<Event> eventsToDisplay)
        {
            listBoxEvents.Items.Clear();
            foreach (var ev in eventsToDisplay)
            {
                listBoxEvents.Items.Add(ev);
            }
        }

        private void UpdateRecommendations()
        {
            if (searchFrequency.Count == 0) return;

            // Find the most frequently searched category
            string favoriteCategory = searchFrequency.OrderByDescending(kvp => kvp.Value).First().Key;

            var recommendedEvents = events.Values
                .Where(ev => ev.Category == favoriteCategory)
                .Take(3)
                .ToList();

            lblRecommendations.Text = "Recommended for you:\n";
            foreach (var ev in recommendedEvents)
            {
                lblRecommendations.Text += $"- {ev.Name}\n";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                DisplayEvents(events.Values.ToList()); // Show all if search is empty
                return;
            }

            // Use LINQ to filter the events from the dictionary's values
            var filteredEvents = events.Values
                .Where(ev => ev.Category.ToLower().Contains(searchTerm) ||
                             ev.Date.ToString("yyyy-MM-dd").Contains(searchTerm))
                .ToList();

            DisplayEvents(filteredEvents);

            // Check if the search term matches a known category, increment its frequency.
            foreach (string category in eventCategories)
            {
                if (searchTerm.Contains(category.ToLower()))
                {
                    if (!searchFrequency.ContainsKey(category))
                    {
                        searchFrequency[category] = 0;
                    }
                    searchFrequency[category]++;
                    break; // Only count the first matched category per search
                }
            }

            UpdateRecommendations();
        }

        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selection = cmbSort.SelectedItem.ToString();
            List<Event> currentEvents = events.Values.ToList();

            if (selection == "Sort by Date")
            {
                currentEvents = currentEvents.OrderBy(ev => ev.Date).ToList();
            }
            else if (selection == "Sort by Name")
            {
                currentEvents = currentEvents.OrderBy(ev => ev.Name).ToList();
            }

            DisplayEvents(currentEvents); // Re-display the sorted list
        }

        private void btnBackToMainMenu_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the LocalEventsForm
            Owner.Show(); // Show the MainMenuForm
        }
    }
}