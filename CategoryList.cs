namespace Labb3_HES
{
    class CategoryList

    {
        public List<Category> trivia_categories { get; }
        public CategoryList(List<Category> trivia_categories)
        {
            this.trivia_categories = trivia_categories;
        }
    }
}
