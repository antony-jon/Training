using NUnit.Framework;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ApiTestingDemo
{
    public class TodosApiTests
    {
        private RestClient _client;
        private JArray _todos;

        [SetUp]
        public void Setup()
        {
            _client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/todos", Method.Get);
            var response = _client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "API did not return 200 OK");

            _todos = JArray.Parse(response.Content!);
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

        [Test]
        public void CountTodosForEachUser()
        {
            var userCounts = _todos
                .GroupBy(t => t["userId"]!.Value<int>())
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var kvp in userCounts)
            {
                TestContext.WriteLine($"UserId {kvp.Key} has {kvp.Value} todos.");
            }

            Assert.That(userCounts.Count, Is.GreaterThan(0), "No user counts found.");
        }

        [Test]
        public void GetAllTitlesForUserId1()
        {
            var titles = _todos
                .Where(t => t["userId"]!.Value<int>() == 1)
                .Select(t => t["title"]!.Value<string>())
                .ToList();

            foreach (var title in titles)
            {
                TestContext.WriteLine(title);
            }

            Assert.That(titles.Count, Is.GreaterThan(0), "No titles found for userId 1.");
        }
    }
}