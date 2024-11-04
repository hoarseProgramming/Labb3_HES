namespace Labb3_HES
{
    using Labb3_HES.Model;
    using Labb3_HES.ViewModel;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text.Json;

    static class JsonHandler
    {
        public static ObservableCollection<QuestionPackViewModel> LoadJsonFile()
        {
            string serializedPacks = File.ReadAllText("packs.json");

            var questionPacks = JsonSerializer.Deserialize<QuestionPack[]>(serializedPacks);

            ObservableCollection<QuestionPackViewModel> deserializedPacks = new();

            foreach (var pack in questionPacks)
            {
                deserializedPacks.Add(new QuestionPackViewModel(pack));
            }

            return deserializedPacks;
        }

        public static void SaveJsonFile(ObservableCollection<QuestionPackViewModel> packs)
        {

            string serializedPacks = JsonSerializer.Serialize(packs);

            File.WriteAllText("packs.json", serializedPacks);
        }

        public static async Task<List<QuestionPack>> LoadJsonFileTest(string pathToJsonFile)
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

        public static async Task SaveJsonFileTest(List<QuestionPack> packs, string pathToFile)
        {
            string serializedPacks = JsonSerializer.Serialize(packs);

            await File.WriteAllTextAsync(pathToFile, serializedPacks);
        }
    }
}
