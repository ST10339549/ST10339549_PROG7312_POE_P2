using MunicipalServiceApp.Application.Abstractions;
using MunicipalServiceApp.Domain;
using MunicipalServiceApp.Domain.Validation;
using MunicipalServiceApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServiceApp.Application.Services
{
    public sealed class IssueService : IIssueService
    {
        private readonly IIssueRepository _repo;

        public IssueService(IIssueRepository repo) => _repo = repo;

        public OperationResult<string> CreateIssue(Issue issue)
        {
            if (issue is null) return OperationResult<string>.Fail("Issue is required.");

            // Basic validation
            if (string.IsNullOrWhiteSpace(issue.Location))
                return OperationResult<string>.Fail("Location is required.");

            if (!CategoryExists(issue.Category))
                return OperationResult<string>.Fail($"Invalid category. Allowed: {string.Join(", ", MunicipalServiceApp.Domain.Categories.All())}");

            if (string.IsNullOrWhiteSpace(issue.Description) || issue.Description.Trim().Length < 5)
                return OperationResult<string>.Fail("Description should be at least 5 characters.");

            // Tracking number and timestamps
            issue.TrackingNumber = TrackingToken.Generate();
            issue.CreatedAt = DateTimeOffset.UtcNow;

            _repo.Add(issue);
            return OperationResult<string>.Ok(issue.TrackingNumber);
        }

        private static bool CategoryExists(string? category)
        {
            if (string.IsNullOrWhiteSpace(category)) return false;
            var wanted = category.Trim();
            foreach (var c in MunicipalServiceApp.Domain.Categories.All())
                if (string.Equals(c, wanted, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        public IEnumerable<Issue> All() => _repo.All();
    }

    internal static class TrackingToken
    {
        public static string Generate()
        {
            var now = DateTime.Now;
            var rand = System.Security.Cryptography.RandomNumberGenerator.GetInt32(0x100000, 0xFFFFFF).ToString("X");
            return $"T{now:yyyyMMddHHmmss}-{rand}";
        }
    }
}
