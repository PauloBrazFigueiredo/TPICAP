using System;

#nullable disable

namespace TPICAP.Data.Models
{
    public partial class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string Salutation { get; set; }
    }
}
