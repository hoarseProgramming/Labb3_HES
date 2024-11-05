namespace Labb3_HES
{
    using Labb3_HES.Model;
    using System.IO;
    using System.Net.Http;
    using System.Text.Json;

    static class JsonHandler
    {
        public static async Task<List<QuestionPack>> LoadJsonFile(string pathToJsonFile)
        {

            var serializedPacks = await File.ReadAllTextAsync(pathToJsonFile);

            var questionPacks = JsonSerializer.Deserialize<QuestionPack[]>(serializedPacks);

            List<QuestionPack> deserializedPacks = new();

            foreach (var pack in questionPacks)
            {
                deserializedPacks.Add(new QuestionPack(pack.Name, pack.Difficulty, pack.TimeLimitInSeconds, pack.Questions));
            }

            return deserializedPacks;
        }
        public static async Task<List<Question>> LoadJsonFileFromApi(string url)
        {
            HttpClient client = new HttpClient();

            //Asynchronous method starts
            var serializedQuestions = await client.GetStringAsync("https://www.google.com");

            var deserializedQuestions = JsonSerializer.Deserialize<List<Question>>(serializedQuestions);

            return deserializedQuestions;
        }

        public static async Task SaveJsonFile(List<QuestionPack> packs, string pathToFile)
        {
            string serializedPacks = JsonSerializer.Serialize(packs);

            await File.WriteAllTextAsync(pathToFile, serializedPacks);
        }
    }
}
