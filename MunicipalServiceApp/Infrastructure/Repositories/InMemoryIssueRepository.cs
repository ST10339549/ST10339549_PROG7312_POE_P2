using MunicipalServiceApp.Application.Abstractions;
using MunicipalServiceApp.Domain;
using MunicipalServiceApp.Domain.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServiceApp.Infrastructure.Repositories
{
    // Stores issues in own linked list (no List<T> / arrays).
    public sealed class InMemoryIssueRepository : IIssueRepository
    {
        private readonly SinglyLinkedList<Issue> _store = new();

        public void Add(Issue issue) => _store.AddLast(issue);

        public IEnumerable<Issue> All()
        {
            foreach (var i in _store) yield return i;
        }

        public Issue? FindByTracking(string tracking)
            => _store.FirstOrDefault(i => i.TrackingNumber == tracking);
    }
}
