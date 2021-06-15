using System;

namespace TPICAP.API.Models
{
    public class PersonModificationModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Salutation { get; set; }
    }
}
