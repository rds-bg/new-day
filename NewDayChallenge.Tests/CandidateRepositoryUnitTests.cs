using NewDayChallenge.Data;
using NewDayChallenge.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NewDayChallenge.Tests
{
    public class CandidateRepositoryUnitTests
    {
        [Test]
        public void No_Match_Returns_Match_Count_Zero()
        {
            var candidateRepository = new CandidateRepository();

            var result = candidateRepository.Search(new string[] { "c#" });

            Assert.AreEqual(0, result.MatchCount);
        }

        [Test]
        public void Match_Returns_Candidate()
        {
            var candidateRepository = new CandidateRepository();

            var id = Guid.NewGuid().ToString();

            var candidate = new Candidate { Id = id, Name = "Candidate1", Skills = new string[] { "c" } };

            candidateRepository.Add(candidate);

            var result = candidateRepository.Search(new string[] { "c" });

            Assert.AreEqual(id, result.Candidate.Id);
        }

        [Test]
        public void Match_Returns_Correct_Match_Count()
        {
            var candidateRepository = new CandidateRepository();

            var id = Guid.NewGuid().ToString();

            var candidate = new Candidate { Id = id, Name = "Candidate1", Skills = new string[] { "c", "python", "sql" } };

            candidateRepository.Add(candidate);

            var result = candidateRepository.Search(new string[] { "c", "python", "css" });

            Assert.AreEqual(2, result.MatchCount);
        }

        [Test]
        public void Match_Returns_Best_Candidate()
        {
            var candidateRepository = new CandidateRepository();

            var id1 = Guid.NewGuid().ToString();

            var candidate = new Candidate { Id = id1, Name = "Candidate1", Skills = new string[] { "python", "sql" } };

            candidateRepository.Add(candidate);

            var id2 = Guid.NewGuid().ToString();
            candidate = new Candidate { Id = id2, Name = "Candidate2", Skills = new string[] { "python", "java", "sql" } };

            candidateRepository.Add(candidate);

            var result = candidateRepository.Search(new string[] { "python", "java" });

            Assert.AreEqual(id2, result.Candidate.Id);
        }

        [Test]
        public void Match_Returns_Best_Candidate_When_Equally_Matched()
        {
            var candidateRepository = new CandidateRepository();

            var id1 = Guid.NewGuid().ToString();

            var candidate = new Candidate { Id = id1, Name = "Candidate1", Skills = new string[] { "python", "java", "sql" } };

            candidateRepository.Add(candidate);

            var id2 = Guid.NewGuid().ToString();
            candidate = new Candidate { Id = id2, Name = "Candidate2", Skills = new string[] { "python", "java", "css" } };

            candidateRepository.Add(candidate);

            var result = candidateRepository.Search(new string[] { "python", "java" });

            Assert.NotNull(result.Candidate);
        }

        [Test]
        public void Equal_Match_Returns_Candidate()
        {
            var candidateRepository = new CandidateRepository();

            var id1 = Guid.NewGuid().ToString();

            var candidate = new Candidate { Id = id1, Name = "Candidate1", Skills = new string[] { "python", "java", "sql" } };

            candidateRepository.Add(candidate);

            var id2 = Guid.NewGuid().ToString();
            candidate = new Candidate { Id = id2, Name = "Candidate2", Skills = new string[] { "python", "java", "sql", "css" } };

            candidateRepository.Add(candidate);

            var result = candidateRepository.Search(new string[] { "python", "java" });

            Assert.NotNull(result);
        }

        [Test]
        [Ignore("Added to sanity check performance")]
        public void SearchPerformanceCheck()
        {
            var candidateRepository = new CandidateRepository();

            for (var i = 0; i < 1000; i++)
            {
                var skills = GetSkills();

                var id1 = Guid.NewGuid().ToString();

                var candidate = new Candidate { Id = id1, Name = "Candidate1", Skills = skills.ToArray() };

                candidateRepository.Add(candidate);
            }

            var id2 = Guid.NewGuid().ToString();

            var matchSkills = GetSkills();

            matchSkills.AddRange(new string[] { "python", "java", "sql" });

            var candidate2 = new Candidate { Id = id2, Name = "Candidate1", Skills = matchSkills.ToArray() };

            candidateRepository.Add(candidate2);

            matchSkills.AddRange(new string[] { "python", "java", "sql" });

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var result = candidateRepository.Search(matchSkills.ToArray());
            watch.Stop();

            Console.WriteLine($"Search execution time with 5003 skills: {watch.ElapsedMilliseconds} ms");

            watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            result = candidateRepository.Search(new string[] { "python", "java", "sql" });
            watch.Stop();

            Console.WriteLine($"Search execution time with 3 skills: {watch.ElapsedMilliseconds} ms");

            Assert.AreEqual(id2, result.Candidate.Id);
        }

        private static List<string> GetSkills()
        {
            var skills = new List<string>();
            for (var j = 0; j < 5000; j++)
            {
                skills.Add(Guid.NewGuid().ToString());
            }
            return skills;
        }
    }
}
