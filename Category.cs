using System.Text.Json.Serialization;

namespace Labb3_HES
{
    class Category
    {
        [JsonPropertyName("id")]
        public int Id { get; }
        [JsonPropertyName("name")]
        public string Name { get; }

        public Category(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
