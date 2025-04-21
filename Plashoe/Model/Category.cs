namespace Plashoe.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Product>? products { get; set; }
    }
}
