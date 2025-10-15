using System;

namespace MunicipalServiceApp.Domain
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd} - {Name} ({Category})";
        }
    }
}