using System;
using Newtonsoft.Json;

namespace Crow.Model
{
    public class Mod
    {
        public string name { get; set; }
        public string title { get; set; }
        public string owner { get; set; }
        public string summary { get; set; }
        public int downloads_count { get; set; }

        //latest_release
        [JsonProperty(propertyName:"latest_release.info_json.factorio_version")]
        public string factorio_version { get; set; }
        public DateTime released_at { get; set; }
        public string version { get; set; }


        public override string ToString()
        {
            return $"{nameof(name)}: {name}, {nameof(title)}: {title}, " +
                   $"{nameof(owner)}: {owner}, {nameof(summary)}: {summary}, " +
                   $"{nameof(downloads_count)}: {downloads_count}, " +
                   $"{nameof(factorio_version)}: {factorio_version}, {nameof(released_at)}: " +
                   $"{released_at}, {nameof(version)}: {version}";
        }
    }
}