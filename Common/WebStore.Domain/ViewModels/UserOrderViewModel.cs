using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class UserOrderViewModel
    {
        public UserOrderViewModel(int id, string name, string phone, string address, decimal total_sum)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Address = address;
            TotalSum = total_sum;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        public decimal TotalSum { get; set; }
    }
}
