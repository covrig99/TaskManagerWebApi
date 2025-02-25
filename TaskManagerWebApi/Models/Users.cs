using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerWebApi.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [ForeignKey("Roles")]
        public int RoleId { get;set; }
        public virtual Roles Roles { get; set; }
    }
}
