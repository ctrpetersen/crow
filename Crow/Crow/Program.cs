using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Crow
{
    class Program
    {
        private static void Main(string[] args) => Crow.Instance.StartAsync().GetAwaiter().GetResult();
    }
}
