using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Customer
    {
        public long Id { get; init; }

        [Required]
        public string FirstName { get; init; }

        [Required]
        public string LastName { get; init; }   
    }

}
