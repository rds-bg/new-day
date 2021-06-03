using NewDayChallenge.Data;
using NewDayChallenge.Domain;
using NewDayChallenge.Services;
using NUnit.Framework;
using System;

namespace NewDayChallenge.Tests
{
    public class CandidateServiceUnitTests
    {
        [Test]
        public void No_Match_Returns_Null()
        {
            var candidateRepository = new CandidateRepository();

            var candidateService = new CandidateService(candidateRepository);

            var result = candidateService.Search(new string[] { "c" });

            Assert.IsNull(result);
        }

        [Test]
        public void Match_Returns_Candidate()
        {
            var candidateRepository = new CandidateRepository();

            var candidateService = new CandidateService(candidateRepository);

            var id = Guid.NewGuid().ToString();

            var candidate = new Candidate { Id = id, Name = "Candidate1", Skills = new string[] { "c" } };

            candidateService.Add(candidate);

            var result = candidateService.Search(new string[] { "c" });

            Assert.AreEqual(id, result.Id);
        }

        [Test]
        public void Match_Returns_Best_Candidate()
        {
            var candidateRepository = new CandidateRepository();

            var candidateService = new CandidateService(candidateRepository);

            var id1 = Guid.NewGuid().ToString();

            var candidate = new Candidate { Id = id1, Name = "Candidate1", Skills = new string[] { "python", "sql" } };

            candidateService.Add(candidate);

            var id2 = Guid.NewGuid().ToString();
            candidate = new Candidate { Id = id2, Name = "Candidate2", Skills = new string[] { "python", "java", "sql" } };

            candidateService.Add(candidate);

            var result = candidateService.Search(new string[] { "python", "java" });

            Assert.AreEqual(id2, result.Id);
        }

        [Test]
        public void Equal_Match_Returns_Candidate()
        {
            var candidateRepository = new CandidateRepository();

            var candidateService = new CandidateService(candidateRepository);

            var id1 = Guid.NewGuid().ToString();

            var candidate = new Candidate { Id = id1, Name = "Candidate1", Skills = new string[] { "python", "java", "sql" } };

            candidateService.Add(candidate);

            var id2 = Guid.NewGuid().ToString();
            candidate = new Candidate { Id = id2, Name = "Candidate2", Skills = new string[] { "python", "java", "sql", "css" } };

            candidateService.Add(candidate);

            var result = candidateService.Search(new string[] { "python", "java" });

            Assert.NotNull(result);
        }
    }
}
