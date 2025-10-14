using MunicipalServiceApp.Domain;
using System.Collections.Generic;

namespace MunicipalServiceApp.Application.Abstractions
{
    public interface IIssueRepository
    {
        // Add a new issue to the repository
        void Add(Issue issue);

        // Enumerate all issues (exposed as a sequence; stored in custom structures)
        IEnumerable<Issue> All();

        // Find an issue by tracking number (null if not found)
        Issue? FindByTracking(string tracking);
    }
}
