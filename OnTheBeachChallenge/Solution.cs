using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheBeachChallenge
{
    partial class Solution : JobSequencer<char>
    {
        //
        // Summary:
        //      Method to parse list of strings representing jobs and converts them into jobs represented by a single character for internal processing.
        //      Throws ArgumentException. See GetJobSequence for list of acceptable job formats.
        //
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

        /// <summary>
        /// Public interface of the solution class.
        /// </summary>
        /// 
        /// <param name="inputs">
        /// List of strings where each string represents a job. Each job must be represented by a single character. A job and it's dependency must be separated by '=>'. Each job may have multiple dependencies but each string must contain only one job and one dependency.
        /// </param>
        /// 
        /// <returns>
        /// List containing job titles in ordered manner.
        /// </returns>
        /// 
        /// <example>
        /// Some examples of valid inputs are: 'a', 'a => a', 'a=>', 'a=>'.
        /// Some examples of invalid inputs are: '=>', 'aa =>', 'a=>aaa', '', 'a=>a=>b'.
        /// </example>
        public override List<char> GetJobSequence(List<string> inputs)
        {
            var jobs = ParseInput(inputs);
            try
            {
                return GenerateJobSequence(jobs);
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
                    try { OutputToConsole(input, new string(solution.GetJobSequence(input).ToArray())); }
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
