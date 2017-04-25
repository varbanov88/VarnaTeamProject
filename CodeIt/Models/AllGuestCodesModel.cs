using System;

namespace CodeIt.Models
{
    public class AllGuestCodesModel
    {
        public AllGuestCodesModel()
        {
            this.Author = "Guest";
        }

        public int Id { get; set; }

        public string Author { get; set; }

        public string CodeTitle { get; set; }

        public DateTime TimeCreated { get; set; }

    }
}