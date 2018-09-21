using System;

namespace Crow.Model
{
    public class FAQ
    {
        public int ID { get; set; }
        public ulong AuthorID { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }

    }
}