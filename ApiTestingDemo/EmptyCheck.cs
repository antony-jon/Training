using NUnit.Framework;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace ApiTestingDemo
{
    public class PostsApiTests
    {
        [Test]
        public void CreatePost_ValidateResponseTypeAndBody()
        {
            // Arrange
            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/posts", Method.Post);
            var requestBody = new
            {
                title = "foo",
                body = "bar",
                userId = 1
            };
            request.AddJsonBody(requestBody);
            var response = client.Execute(request);
            TestContext.WriteLine($"Content-Type: {response.ContentType}");
            Assert.That(response.ContentType, Does.Contain("application/json"), "Response type is not application/json");
            var json = JObject.Parse(response.Content!);

            Assert.That(json["title"]!.ToString(), Is.EqualTo("foo"), "Title does not match");
            Assert.That(json["body"]!.ToString(), Is.EqualTo("bar"), "Body does not match");
            Assert.That(json["userId"]!.Value<int>(), Is.EqualTo(1), "UserId does not match");
            Assert.That(json["id"]!.Value<int>(), Is.EqualTo(101), "Id does not match");

            TestContext.WriteLine("Response JSON:");
            TestContext.WriteLine(json.ToString());
        }
    }
}