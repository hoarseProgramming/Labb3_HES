using System.Net.Http;
using System.Text.Json;
using System.Web;

namespace Labb3_HES
{
    class APIHandler
    {
        public static async Task CheckConnectionToOpenTDB()
        {
            HttpClient client = new HttpClient();

            var myVar = await client.GetStreamAsync("https://opentdb.com");
        }

        public static async Task<CategoryList> GetQuestionCategories()
        {
            HttpClient client = new HttpClient();

            var serializedCategories = await client.GetStringAsync("https://opentdb.com/api_category.php");

            var categoryList = JsonSerializer.Deserialize<CategoryList>(serializedCategories);

            return categoryList;
        }

        public static async Task<APIQuestionRequest> GetQuestions(int amount, int categoryId, string difficulty)
        {
            HttpClient client = new HttpClient();

            string urlForAPIRequest = $"https://opentdb.com/api.php?amount={amount}&category={categoryId}&difficulty={difficulty}&type=multiple";

            var serializedQuestions = await client.GetStringAsync(urlForAPIRequest);

            var questionRequest = JsonSerializer.Deserialize<APIQuestionRequest>(serializedQuestions);

            DecodeFromHTTP(questionRequest);

            return questionRequest;
        }

        private static void DecodeFromHTTP(APIQuestionRequest questionRequest)
        {
            foreach (var question in questionRequest.ImportedQuestions)
            {
                question.Query = HttpUtility.HtmlDecode(question.Query);
                question.CorrectAnswer = HttpUtility.HtmlDecode(question.CorrectAnswer);

                for (int i = 0; i < 3; i++)
                {
                    question.IncorrectAnswers[i] = HttpUtility.HtmlDecode(question.IncorrectAnswers[i]);
                }
            }
        }
    }
}
