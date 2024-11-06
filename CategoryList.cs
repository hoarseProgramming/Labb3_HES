using System.Text.Json.Serialization;

namespace Labb3_HES
{
    class CategoryList

    {
        [JsonPropertyName("trivia_categories")]
        public List<Category> ListOfCategories { get; }
        public CategoryList(List<Category> listOfCategories)
        {
            this.ListOfCategories = listOfCategories;
        }
    }
}
