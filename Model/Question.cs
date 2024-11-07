using System.Text.Json.Serialization;

namespace Labb3_HES.Model
{
    class Question
    {
        [JsonIgnore]
        public string type { get; set; }
        [JsonIgnore]
        public string difficulty { get; set; }
        [JsonIgnore]
        public string category { get; set; }
        [JsonPropertyName("question")]
        public string Query { get; set; }
        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; }
        [JsonPropertyName("incorrect_answers")]
        public string[] IncorrectAnswers { get; set; }
        public Question(string[] incorrectAnswers, string correctAnswer, string query = "New Question")
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = incorrectAnswers;
        }

    }
}
