using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheBeachChallenge
{
    class Program
    {
        static bool isInvalidSequence { get; set; }
        static List<char> DoneChars = new List<char>();
        static List<char> markedChars = new List<char>();

        public class Job
        {
            public char X { get; set; }
            public List<Job> Dependencies { get; set; }
            public Job(char x)
            {
                this.X = x;
                this.Dependencies = new List<Job>();
            }
        }

        public static void GetJobSequence(List<Job> jobs, List<char> markedChars)
        {
            for (var i = 0; i < jobs.Count(); i++)
            {
                if (isInvalidSequence)
                    return;

                if (!DoneChars.Contains(jobs[i].X))
                {
                    if (markedChars.Contains(jobs[i].X))
                    {
                        isInvalidSequence = true;
                        return;
                    }

                    markedChars.Add(jobs[i].X);
                    GetJobSequence(jobs[i].Dependencies, markedChars);
                    DoneChars.Add(jobs[i].X);
                    markedChars.Clear();
                }
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
                var job = jobs.FirstOrDefault(j => j.X == d[0][0]);
                var dependent = jobs.FirstOrDefault(j => j.X == d[1][0]);
                if (job == null || dependent == null)
                    throw new ArgumentException("Input contains missing jobs.");

                job.Dependencies.Add(dependent);
            }

            return jobs;
        }

        static void Main(string[] args)
        {
            Job j1 = new Job('a');
            Job j2 = new Job('b');
            Job j3 = new Job('c');
            Job j4 = new Job('d');
            Job j5 = new Job('e');
            Job j6 = new Job('f');

            j2.Dependencies.Add(j3);
            j3.Dependencies.Add(j6);
            j4.Dependencies.Add(j1);
            j5.Dependencies.Add(j2);

            var jobs = new List<Job>() { j1, j2, j3, j4, j5, j6 };
            var markedChars = new List<Char>();
            GetJobSequence(jobs, markedChars);
            Console.WriteLine(isInvalidSequence);
            DoneChars.ForEach(d =>
                Console.WriteLine(d)
            );
            Console.Read();
        }
    }
}
