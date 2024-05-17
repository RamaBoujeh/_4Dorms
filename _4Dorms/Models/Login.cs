using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace _4Dorms.Models
{
    public class LogIn
    {
        public int LoginId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
