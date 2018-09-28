using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Crow.Model;
using Discord.Commands;
using Newtonsoft.Json;

namespace Crow.Commands
{
    public class LinkMod : ModuleBase<SocketCommandContext>
    {
        //First sends a request to https://mods.factorio.com/api/mods/<name>. If nothing is found, sends request to https://mods.factorio.com/query/<name> and parses manually.
        //Returns top 5 results.
        //Also will support a:<author>, t and c:<category>.
        [Command("linkmod")]
        [Summary("Finds a mod from parameter. Can also search for author, trending or categories.")]
        [Alias("findmod, mod")]
        public async Task LinkModCommand(string modName)
        {

        }

        //returns null if response == mod not found from API
        private dynamic FindSingleMod(string modname)
        {
            dynamic response = JsonConvert.DeserializeObject<dynamic>(Crow.Instance.HttpClient.GetStringAsync(@"https://mods.factorio.com/api/mods/" + modname + "/full").Result);

            if (response.message == "Mod not found")
                return null;

            return response;
        }
        
    }
}