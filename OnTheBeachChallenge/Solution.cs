using System;
using System.Collections.Generic;
using System.Linq;

namespace OnTheBeachChallenge
{
    /// <summary>
    /// Main class that takes jobs as strings, processes them and outputs their order.
    /// </summary>    
    /// 
    /// <remarks>
    /// Job sorting is done through the algorithm known as Topological Sorting.
    /// We pass a job, then we pass it's dependents recursively until all the dependencies and grand-dependencies of this job have been passed, saving them to the final result along the way. If a job occurs more than once during a pass, it means that input contains a circular dependency, so an argument exception is thrown.
    /// </remarks>
    public partial class Solution
    {
        //
        // Summary:
        //      Holds the sorted sequence.
        //
        private List<char> JobSequence = new List<char>();

        //
        // Summary:
        //      Represents a job with title and dependencies.
        //
        private class Job<T>
            where T : struct
        {
            public T Title { get; private set; }
            public List<Job<T>> Dependencies { get; private set; }

            public Job(T title)
            {
                this.Title = title;
                this.Dependencies = new List<Job<T>>();
            }
        }

        //
        // Summary:
        //      Method to parse list of strings representing jobs and converts them into jobs represented by a single character for internal processing.
        //      Throws ArgumentException. See GetJobSequence for list of acceptable job formats.
        //
        private List<Job<char>> ParseInput(List<string> inputJobs)
        {
            var jobs = new List<Job<char>>();

            var jobPairs = inputJobs.Select(i => i.Split(new string[] { "=>" }, StringSplitOptions.None))
                                    .Select(i => i.Select(j => j.Trim()).ToArray());

            if (jobPairs.Any(j => j.Length < 1 || j.Length > 2))
                throw new ArgumentException("Invalid Format of Jobs");

            if (jobPairs.Any(j => (j.Length == 1 && j[0].Length != 1)
                               || (j.Length == 2 && (j[0].Length != 1 || j[1].Length > 1))))
                throw new ArgumentException("Each job must be represented by a single character");

            jobs.AddRange(jobPairs.Select(j => j[0][0])
                                  .Distinct()
                                  .Select(j => new Job<char>(j)));

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

        //
        // Summary:
        //      Processes a list of jobs and saves result in JobSequence field.
        //      Throws ArgumentException if input contains circular dependency.
        //
        private void GenerateJobSequence(List<Job<char>> jobs)
        {
            this.JobSequence.Clear();
            var visited = new List<char>();

            foreach (var job in jobs)
                VisitJob(job, visited);
        }

        //
        // Summary:
        //      Helper method used by GenerateJobSequence.
        //
        private void VisitJob(Job<char> job, List<char> visited)
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

        /// <summary>
        /// Public interface of the solution class.
        /// </summary>
        /// 
        /// <param name="inputs">
        /// List of strings where each string represents a job. Each job must be represented by a single character. A job and it's dependency must be separated by '=>'. Each job may have multiple dependecies but each string can contain one job and one dependency.
        /// </param>
        /// 
        /// <returns>
        /// String representing job names in ordered manner.
        /// </returns>
        /// 
        /// <example>
        /// Some examples of valid inputs are: 'a', 'a => a', 'a=>', 'a=>'.
        /// Some examples of invalid inputs are: '=>', 'aa =>', 'a=>aaa', '', 'a=>a=>b'.
        /// </example>
        /// 
        // TODO: Put this into an abstract class to work with different types of input titles. Right now this only works with jobs represented by a single character.
        public string GetJobSequence(List<string> inputs)
        {
            var jobs = ParseInput(inputs);
            try
            {
                GenerateJobSequence(jobs);
                return new string(JobSequence.ToArray());
            }
            catch (Exception) { /* TODO: Log the exception */ throw; }
        }

        static void Main(string[] args)
        {
            try
            {
                var solution = new Solution();
                List<string>[] inputs = {
                       new List<string>() { "a=>" },
                       new List<string>() { "a => ", "b=>", "c=>" },
                       new List<string>() { "a =>", "b => c", "c=>"},
                       new List<string>() { "a=>", "b=>c", "c=>f", "d => a", "e =>b", "f=>" },
                       new List<string>() { "a =>", "b", "c=>c" },
                       new List<string>() { "a=>", "b=>c", "c=>f", "d=>a", "e=>", "f=>b"},
                        };
                Array.ForEach(inputs, input =>
                {
                    try { OutputToConsole(input, solution.GetJobSequence(input)); }
                    catch (Exception ex) { OutputToConsole(input, ex.Message); }
                });
            }
            finally { Console.Read(); }
        }

        private static void OutputToConsole(List<string> input, string output)
        {
            Console.WriteLine($"Input: {string.Join(", ", input)}. Output: {output}");
        }
    }
}
