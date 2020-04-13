using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsMain { get; set; }

        public string PublicId { get; set; }

        //  Setting this relationship forces EF to configure its migration
        //  in a way that Photos are cascade deleted whenever a user is deleted
        public User User { get; set; }

        public int UserId { get; set; }
    }
}