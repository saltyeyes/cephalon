namespace Saltyeyes.Cephalon.Parsers.Relics {
    using System;
    using System.Collections.Generic;
    using System.Net;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using J = Newtonsoft.Json.JsonPropertyAttribute;

    public partial class RelicsByItem {
        [J("images")] public List<string> Images { get; set; }
        [J("prefixes")] public List<string> Prefixes { get; set; }
        [J("suffixes")] public List<string> Suffixes { get; set; }
        [J("items")] public List<Item> Items { get; set; }
    }

    public partial class Item {
        [J("itemPrefix")] public string ItemPrefix { get; set; }
        [J("itemSuffixes")] public List<ItemSuffix> ItemSuffixes { get; set; }
    }

    public partial class ItemSuffix {
        [J("suffix")] public string Suffix { get; set; }
        [J("image")] public string Image { get; set; }
        [J("relics")] public List<Relic> Relics { get; set; }
    }

    public partial class Relic {
        [J("name")] public string Name { get; set; }
        [J("rarity")] public string Rarity { get; set; }
        [J("era")] public string Era { get; set; }
        [J("image")] public string Image { get; set; }
    }

    public partial class RelicsByItem {
        public static RelicsByItem FromJson(string json) => JsonConvert.DeserializeObject<RelicsByItem>(json, Relics.Converter.Settings);
    }

    public static class Serialize {
        public static string ToJson(this RelicsByItem self) => JsonConvert.SerializeObject(self, Relics.Converter.Settings);
    }

    internal class Converter {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter()
                {
                    DateTimeStyles = DateTimeStyles.AssumeUniversal,
                },
            },
        };
    }
}
