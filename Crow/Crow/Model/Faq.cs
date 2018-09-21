using System;
using System.Collections.Generic;

namespace Crow.Model
{
    public partial class Faq
    {
        public int Faqid { get; set; }
        public string AuthorId { get; set; }
        public string Command { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
        public string GuildId { get; set; }

        public Guild Guild { get; set; }
    }
}
