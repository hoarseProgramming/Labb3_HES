using System.Text.Json.Serialization;

namespace Labb3_HES.Model
{
    class Question
    {
        public Question(string[] incorrectAnswers, string query = "New Question", string correctAnswer = "")
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = incorrectAnswers;
        }
        //public Question(string query = "New Question", string correctAnswer = "",
        //    string incorrectAnswer1 = "", string incorrectAnswer2 = "", string incorrectAnswer3 = "")
        //{
        //    Query = query;
        //    CorrectAnswer = correctAnswer;
        //    IncorrectAnswers = [incorrectAnswer1, incorrectAnswer2, incorrectAnswer3];
        //}
        //public Question(string[] incorrectAnswers, string query, string correctAnswer)
        //{
        //    Query = query;
        //    CorrectAnswer = correctAnswer;
        //    IncorrectAnswers = incorrectAnswers;
        //}

        //[JsonConstructor]
        //public Question(string query, string correctAnswer, string[] incorrectAnswers)
        //{
        //    Query = query;
        //    CorrectAnswer = correctAnswer;
        //    IncorrectAnswers = incorrectAnswers;
        //}

        [JsonIgnore]
        public string type { get; set; }
        [JsonIgnore]
        public string difficulty { get; set; }
        [JsonIgnore]
        public string category { get; set; }
        [JsonPropertyName("question")]
        public string Query { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] IncorrectAnswers { get; set; }

    }
}
