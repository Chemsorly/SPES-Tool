using NGitLab;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NGitLab.Models;

namespace SPES_App
{
    public class GitlabApiManager
    {
        private GitLabClient _client;


        public GitlabApiManager()
        {

        }

        public void Initialize(String pHosturl, String pToken)
        {
            _client = new GitLabClient(pHosturl, pToken);
            Console.WriteLine("GitlabAPI initialized");
        }

        public Task CreateIssue(int pProjectId, String pTitle, String pBody, String pAuthor = "Anonymous")
        {
            //check for null author
            if (String.IsNullOrWhiteSpace(pAuthor))
                pAuthor = "Anonymous";

            IssueCreate issue = new IssueCreate()
            {
                Description = $"**_The issue was submitted by {pAuthor} via API**.\n\n {pBody}",
                Title = pTitle,
                ProjectId = pProjectId,
                Labels = "Bug-Report"
            };
            var task = _client.Issues.CreateAsync(issue);
            task.ContinueWith(t =>
            {
                Console.WriteLine($"Issue with ID {t.Result.IssueId} has been created.");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith(t =>
            {
                if (t.Exception != null)
                    Console.WriteLine($"Issue could not be created. Reason: {t.Exception.Message}");
            }, TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }
    }
}
