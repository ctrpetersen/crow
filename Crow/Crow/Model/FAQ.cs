using System;

namespace Crow.Model
{
    public class FAQ
    {
        public int FAQID { get; set; }
        public string AuthorID { get; set; }
        public string Command { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
        public string GuildID { get; set; }
        
    }
}