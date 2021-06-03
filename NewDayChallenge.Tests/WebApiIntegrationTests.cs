using NewDayChallenge.Domain;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewDayChallenge.Tests
{
    public class WebApiIntegrationTests
    {
        private WebApiApplicationFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new WebApiApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Get_Missing_Skils_Returns_400()
        {
            var response = await _client.GetAsync("/candidates/search");

            Assert.AreEqual(400, (int) response.StatusCode);
        }

        [Test]
        public async Task Post_Missing_Body_Returns_400()
        {
            var content = new StringContent("",
                                    Encoding.UTF8,
                                    "application/json");

            var response = await _client.PostAsync("/candidates", content);

            Assert.AreEqual(400, (int)response.StatusCode);
        }

        [Test]
        public async Task Get_Returns_Candidate()
        {
            var id = Guid.NewGuid().ToString();

            var skills = new string[] { "c"};

            var candidate = new Candidate { Id = id, Name = "Candidate1", Skills = skills };

            var content = new StringContent(JsonConvert.SerializeObject(candidate),
                                    Encoding.UTF8,
                                    "application/json");

            await _client.PostAsync("/candidates", content);

            var response = await _client.GetAsync("/candidates/search?skills=c");

            response.EnsureSuccessStatusCode();

            var returnedCandidate = JsonConvert.DeserializeObject<Candidate>(
                await response.Content.ReadAsStringAsync());

            Assert.AreEqual(id, returnedCandidate.Id);
        }

        [Test]
        public async Task Get_Returns_Best_Matched_Candidate()
        {
            var id1 = Guid.NewGuid().ToString();

            var skills = new string[] { "python", "sql", "javascript" };

            var candidate = new Candidate { Id = id1, Name = "Candidate1", Skills = skills };

            var content = new StringContent(JsonConvert.SerializeObject(candidate),
                                    Encoding.UTF8,
                                    "application/json");

            await _client.PostAsync("/candidates", content);

            var id2 = Guid.NewGuid().ToString();

            skills = new string[] { "python", "java", "css" };

            candidate = new Candidate { Id = id2, Name = "Candidate2", Skills = skills };
            
            content = new StringContent(JsonConvert.SerializeObject(candidate),
                                    Encoding.UTF8,
                                    "application/json");

            await _client.PostAsync("/candidates", content);

            var response = await _client.GetAsync("/candidates/search?skills=python,java");

            response.EnsureSuccessStatusCode();

            var returnedCandidate = JsonConvert.DeserializeObject<Candidate>(
                await response.Content.ReadAsStringAsync());

            Assert.AreEqual(id2, returnedCandidate.Id);
        }

        [Test]
        public async Task Get_No_Match_Returns_404()
        {
            var response = await _client.GetAsync("/candidates/search?skills=orleans");

            Assert.AreEqual(404, (int)response.StatusCode);
        }

        [Test]
        public async Task Get_Returns_A_Best_Matched_Candidate_When_Equally_Matched()
        {
            var id1 = Guid.NewGuid().ToString();

            var skills = new string[] { "mongodb", "cosmosdb", "mssql" };

            var candidate = new Candidate { Id = id1, Name = "Candidate1", Skills = skills };

            var content = new StringContent(JsonConvert.SerializeObject(candidate),
                                    Encoding.UTF8,
                                    "application/json");

            await _client.PostAsync("/candidates", content);

            var id2 = Guid.NewGuid().ToString();

            skills = new string[] { "mongodb", "cosmosdb", "mysql" };

            candidate = new Candidate { Id = id2, Name = "Candidate2", Skills = skills };

            content = new StringContent(JsonConvert.SerializeObject(candidate),
                                    Encoding.UTF8,
                                    "application/json");

            await _client.PostAsync("/candidates", content);

            var response = await _client.GetAsync("/candidates/search?skills=mongodb,cosmosdb");

            response.EnsureSuccessStatusCode();

            var returnedCandidate = JsonConvert.DeserializeObject<Candidate>(
                await response.Content.ReadAsStringAsync());

            Assert.NotNull(returnedCandidate);
        }
    }
}