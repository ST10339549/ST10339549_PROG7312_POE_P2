using MunicipalServiceApp.Domain;
using MunicipalServiceApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServiceApp.Application.Abstractions
{
    public interface IIssueService
    {
        /// <summary>
        /// Validates and stores an Issue; returns a tracking number on success.
        /// </summary>
        OperationResult<string> CreateIssue(Issue issue);

        IEnumerable<Issue> All();
    }
}
