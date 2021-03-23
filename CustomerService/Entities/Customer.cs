using System.ComponentModel.DataAnnotations;

namespace CustomerService.Entities
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class Customer
    {
        [Key]
        public int Id { get; init; }
        public string Name { get; init; }
    }
}