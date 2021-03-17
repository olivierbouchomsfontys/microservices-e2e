using System.ComponentModel.DataAnnotations;

namespace CustomerService.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}