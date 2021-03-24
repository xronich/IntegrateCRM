using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegrateCRM.Database.Entity
{
    [Table("Email-messages")]
    public class EmailMessage : IEntity
    {
        [Autoincrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Ip { get; set; }
        public string Message { get; set; }
    }
}