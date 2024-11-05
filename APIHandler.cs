using Labb3_HES.Model;
using System.Net.Http;
using System.Text.Json;

namespace Labb3_HES
{
    class APIHandler
    {
        public static async Task<CategoryList> GetQuestionCategories()
        {
            HttpClient client = new HttpClient();

            //Asynchronous method starts
            var serializedCategories = await client.GetStringAsync("https://opentdb.com/api_category.php");

            var categoryList = JsonSerializer.Deserialize<CategoryList>(serializedCategories);

            return categoryList;
        }
        public static async Task<List<Question>> GetQuestions()
        {
            HttpClient client = new HttpClient();

            //Asynchronous method starts
            var serializedCategories = await client.GetStringAsync("https://opentdb.com/api.php?amount=10");

            var categoryList = JsonSerializer.Deserialize<CategoryList>(serializedCategories);

            var questions = new List<Question>();

            return questions;
        }
    }
}
