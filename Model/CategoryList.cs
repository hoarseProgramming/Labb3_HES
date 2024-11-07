using System.Text.Json.Serialization;

namespace Labb3_HES.Model
{
    class CategoryList
    {
        [JsonPropertyName("trivia_categories")]
        public List<Category> ListOfCategories { get; }

        public CategoryList(List<Category> listOfCategories)
        {
            ListOfCategories = listOfCategories;
        }
    }
}
