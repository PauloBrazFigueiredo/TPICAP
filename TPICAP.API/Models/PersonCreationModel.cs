using System;

namespace TPICAP.API.Models
{
    public class PersonCreationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Salutation { get; set; }
    }
}
