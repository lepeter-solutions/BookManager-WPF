using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beadando
{
    public class Books
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public string Genre { get; set; }

        public Books(string title, string author, string genre)
        {
            Title = title;
            Author = author;
            Genre = genre;
        }
    }
}
