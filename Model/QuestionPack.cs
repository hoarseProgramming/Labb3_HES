﻿using System.Text.Json.Serialization;

namespace Labb3_HES.Model
{
    enum Difficulty { Easy, Medium, Hard }

    class QuestionPack
    {
        public QuestionPack(string name = "PackName", Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimitInSeconds = timeLimitInSeconds;
            Questions = [];
        }

        [JsonConstructor]
        public QuestionPack(string name, Difficulty difficulty, int timeLimitInSeconds, List<Question> questions)
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimitInSeconds = timeLimitInSeconds;
            Questions = questions;
        }

        public string Name { get; set; }

        public Difficulty Difficulty { get; set; }

        public int TimeLimitInSeconds { get; set; }

        public List<Question> Questions { get; set; }
    }
}
