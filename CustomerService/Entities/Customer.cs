using System.ComponentModel.DataAnnotations;

namespace CustomerService.Entities
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; init; }

        public void AssignId(int id)
        {
            Id = id;
        }
    }
}