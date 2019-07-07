using System;
using System.Collections.Generic;
using System.Linq;

namespace OnTheBeachChallenge
{
    public partial class Solution
    {
        private List<char> JobSequence = new List<char>();

        private class Job
        {
            public char Title { get; private set; }
            public List<Job> Dependencies { get; private set; }

            public Job(char title)
            {
                this.Title = title;
                this.Dependencies = new List<Job>();
            }
        }

        private List<Job> ParseInput(List<string> inputJobs)
        {
            var jobs = new List<Job>();

            var jobPairs = inputJobs.Select(i => i.Split(new string[] { "=>" }, StringSplitOptions.None))
                                    .Select(i => i.Select(j => j.Trim()).ToArray());

            if (jobPairs.Any(j => j.Length < 1 || j.Length > 2))
                throw new ArgumentException("Invalid Format of Jobs");

            if (jobPairs.Any(j => (j.Length == 1 && j[0].Length != 1)
                               || (j.Length == 2 && (j[0].Length != 1 || j[1].Length > 1))))
                throw new ArgumentException("Each job must be represented by a single character");

            jobs.AddRange(jobPairs.Select(j => j[0][0])
                                  .Distinct()
                                  .Select(j => new Job(j)));

            var jobDependencies = jobPairs.Where(j => j.Length == 2 && j[1].Length > 0).ToArray();
            foreach (var d in jobDependencies)
            {
                var job = jobs.FirstOrDefault(j => j.Title == d[0][0]);
                var dependent = jobs.FirstOrDefault(j => j.Title == d[1][0]);
                if (job == null || dependent == null)
                    throw new ArgumentException("Input contains missing jobs.");

                job.Dependencies.Add(dependent);
            }

            return jobs;
        }

        private void GenerateJobSequence(List<Job> jobs)
        {
            this.JobSequence.Clear();
            var visited = new List<char>();

            foreach (var job in jobs)
                VisitJob(job, visited);
        }

        private void VisitJob(Job job, List<char> visited)
        {
            if (visited.Contains(job.Title))
                throw new ArgumentException("Input contains circular dependency");

            else if (!JobSequence.Contains(job.Title))
            {
                visited.Add(job.Title);
                foreach (var j in job.Dependencies)
                    VisitJob(j, visited);

                visited.Remove(job.Title);
                JobSequence.Add(job.Title);
            }
        }

        public string GetJobSequence(List<string> inputs)
        {
            var jobs = ParseInput(inputs);
            try
            {
                GenerateJobSequence(jobs);
                return new string(JobSequence.ToArray());
            }
            catch (Exception) { throw; }
        }

        static void Main(string[] args)
        {
        }
    }
}
