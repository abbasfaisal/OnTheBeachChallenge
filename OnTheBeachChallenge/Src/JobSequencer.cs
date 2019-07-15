using System;
using System.Collections.Generic;
using System.Linq;

namespace OnTheBeachChallenge
{
    /// <summary>
    /// Main class that processes jobs. Contains an abstract interface that takes jobs as strings and outputs their order.
    /// </summary>    
    /// 
    /// <remarks>
    /// Job sorting is done through the algorithm known as Topological Sorting.
    /// We pass a job, then we pass it's dependents recursively until all the dependencies and grand-dependencies of this job have been passed, saving them to the final result along the way. If a job occurs more than once during a pass, it means that input contains a circular dependency, so an argument exception is thrown.
    /// 
    /// To Do:
    /// This should only work correctly with struct or string as generic parameter. For other types of generic parameter, we would need a way to check for job title by *value* in VisitJob().
    /// Note: The whole solution could be simplified if we choose to represent job title using a string.
    /// </remarks>
    public abstract partial class JobSequencer<T> // T is the type of title of a job
    {
        //
        // Summary:
        //      Holds the sorted sequence.
        //
        private List<T> JobSequence = new List<T>();

        //
        // Summary:
        //      Represents a job with title and dependencies.
        //
        protected class Job
        {
            public T Title { get; private set; }
            public List<Job> Dependencies { get; private set; }

            public Job(T title)
            {
                this.Title = title;
                this.Dependencies = new List<Job>();
            }
        }

        //
        // Summary:
        //      Processes a list of jobs and saves result in JobSequence field.
        //      Throws ArgumentException if input contains circular dependency.
        //
        protected List<T> GenerateJobSequence(List<Job> jobs)
        {
            this.JobSequence.Clear();
            var visited = new List<T>();

            foreach (var job in jobs)
                VisitJob(job, visited);

            return JobSequence;
        }

        //
        // Summary:
        //      Helper method used by GenerateJobSequence.
        //
        private void VisitJob(Job job, List<T> visited)
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

        //
        // See Solution class
        //
        public abstract List<T> GetJobSequence(List<string> jobs);
    }
}
