using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegrateCRM.Database.Entity
{
    [Table("Registrations")]
    public class Registration : IEntity
    {
        [Autoincrement]
        public int Id { get; set; }
        public string Country { get; set; }
        public string CurrencyCode { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string GmtTimezone { get; set; }
        public string Password { get; set; }
        public string Lang { get; set; }
        public string CampaignId { get; set; }
        public string FreeText { get; set; }
        public string PromoCode { get; set; }
        public string ClientId { get; set; }
        public string Ip { get; set; }
    }
}