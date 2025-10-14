using MunicipalServiceApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServiceApp.Domain.Validation
{
    public static class IssueValidator
    {
        public static OperationResult Validate(Issue issue, string[] validCategories)
        {
            if (string.IsNullOrWhiteSpace(issue.Location) || issue.Location.Trim().Length < 3)
                return OperationResult.Fail("Location must be at least 3 characters.");

            if (string.IsNullOrWhiteSpace(issue.Category))
                return OperationResult.Fail("Category is required.");

            var found = false;
            foreach (var c in validCategories)
            {
                if (string.Equals(c, issue.Category, System.StringComparison.OrdinalIgnoreCase))
                {
                    found = true; break;
                }
            }
            if (!found)
                return OperationResult.Fail("Category is invalid.");

            if (string.IsNullOrWhiteSpace(issue.Description) || issue.Description.Trim().Length < 10)
                return OperationResult.Fail("Description must be at least 10 characters.");

            if (issue.AttachmentPath is not null && issue.AttachmentPath.Length > 260)
                return OperationResult.Fail("Attachment path is too long.");

            return OperationResult.Ok();
        }
    }
}
