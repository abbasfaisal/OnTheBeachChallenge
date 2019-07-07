using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NUnit.Framework;

namespace OnTheBeachChallenge
{
    /// <summary>
    /// Contains unit tests for methods of class JobSequence. Uses NUnit.
    /// </summary>
    public class JobSequencerTest : JobSequencer<char>
    {
        public override List<char> GetJobSequence(List<string> jobs)
        {
            throw new NotImplementedException();
        }

        [TestFixture]
        public class TestClass
        {
            private JobSequencerTest testObject = new JobSequencerTest();

            /// <summary>
            /// Basic tests for testing circular dependencies from GenerateJobSequence().
            /// </summary>
            [Test]
            public void UT_GetJobSequence_Basic()
            {
                var j1 = new Job('a');
                var j2 = new Job('b');
                var j3 = new Job('c');
                var j4 = new Job('d');
                var j5 = new Job('e');
                var j6 = new Job('f');

                TestJobSequence(new List<Job>() { j1 }, "a");
                TestJobSequence(new List<Job>() { j1, j2, j3 }, "abc");

                j2.Dependencies.Add(j3);
                TestJobSequence(new List<Job>() { j1, j2, j3 }, "acb");

                j3.Dependencies.Add(j6);
                j4.Dependencies.Add(j1);
                j5.Dependencies.Add(j2);
                var result = new string(this.testObject.GenerateJobSequence(new List<Job>() { j1, j2, j3, j4, j5, j6 }).ToArray());
                Assert.IsTrue(result.Length == 6 &&
                              IsBefore(result, 'c', 'b') &&
                              IsBefore(result, 'f', 'c') &&
                              IsBefore(result, 'a', 'd') &&
                              IsBefore(result, 'b', 'e') &&
                              result.Contains('a'));
            }

            /// <summary>
            /// Basic tests for output from GenerateJobSequence().
            /// </summary>
            public void UT_GetJobSequence_circularInput()
            {
                var j1 = new Job('a');
                var j2 = new Job('b');
                var j3 = new Job('c');
                var j4 = new Job('d');
                var j5 = new Job('e');
                var j6 = new Job('f');

                j1.Dependencies.Add(j1);
                TestcircularDependency(new List<Job>() { j1 });

                j1.Dependencies.Clear();
                j3.Dependencies.Add(j3);
                TestcircularDependency(new List<Job>() { j1, j2, j3 });

                j3.Dependencies.Clear();
                j2.Dependencies.Add(j3);
                j3.Dependencies.Add(j6);
                j4.Dependencies.Add(j1);
                j6.Dependencies.Add(j2);
                TestcircularDependency(new List<Job>() { j1, j2, j3, j4, j5, j6 });
            }

            #region HelperFunctions

            private void TestJobSequence(List<Job> inputs, string output)
            {
                var result = this.testObject.GenerateJobSequence(inputs);
                Assert.IsTrue(new string(result.ToArray()) == output);
            }

            private bool IsBefore(string str, char first, char second)
            {
                return str.Contains(first) && str.Contains(second) && str.IndexOf(first) < str.IndexOf(second);
            }

            private void TestcircularDependency(List<Job> inputs)
            {
                var result = Assert.Throws<ArgumentException>(() => this.testObject.GenerateJobSequence(inputs));
                Assert.IsTrue(result.Message == "Input contains circular dependency");
            }

            #endregion
        }
    }
}