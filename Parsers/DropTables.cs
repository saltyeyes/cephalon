namespace Saltyeyes.Cephalon.Parsers.DropTables {
    using System;
    using System.Collections.Generic;
    using System.Net;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using J = Newtonsoft.Json.JsonPropertyAttribute;

    public partial class DropTables {
        [J("tier")] public Tier Tier { get; set; }
        [J("name")] public string Name { get; set; }
        [J("rewards")] public Rewards Rewards { get; set; }
    }

    public partial class Rewards {
        [J("Intact")] public List<Exceptional> Intact { get; set; }
        [J("Exceptional")] public List<Exceptional> Exceptional { get; set; }
        [J("Flawless")] public List<Flawless> Flawless { get; set; }
        [J("Radiant")] public List<Exceptional> Radiant { get; set; }
    }

    public partial class Exceptional {
        [J("_id")] public string Id { get; set; }
        [J("itemName")] public string ItemName { get; set; }
        [J("rarity")] public Rarity Rarity { get; set; }
        [J("chance")] public double Chance { get; set; }
    }

    public partial class Flawless {
        [J("_id")] public string Id { get; set; }
        [J("itemName")] public string ItemName { get; set; }
        [J("rarity")] public Rarity Rarity { get; set; }
        [J("chance")] public long Chance { get; set; }
    }

    public enum Rarity { Common, Rare, Uncommon };

    public enum Tier { Axi, Lith, Meso, Neo };

    public partial class DropTables {
        public static List<DropTables> FromJson(string json) => JsonConvert.DeserializeObject<List<DropTables>>(json, Saltyeyes.Cephalon.Parsers.DropTables.Converter.Settings);
    }

    static class RarityExtensions {
        public static Rarity? ValueForString(string str) {
            switch (str) {
                case "Common": return Rarity.Common;
                case "Rare": return Rarity.Rare;
                case "Uncommon": return Rarity.Uncommon;
                default: return null;
            }
        }

        public static Rarity ReadJson(JsonReader reader, JsonSerializer serializer) {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this Rarity value, JsonWriter writer, JsonSerializer serializer) {
            switch (value) {
                case Rarity.Common: serializer.Serialize(writer, "Common"); break;
                case Rarity.Rare: serializer.Serialize(writer, "Rare"); break;
                case Rarity.Uncommon: serializer.Serialize(writer, "Uncommon"); break;
            }
        }
    }

    static class TierExtensions {
        public static Tier? ValueForString(string str) {
            switch (str) {
                case "Axi": return Tier.Axi;
                case "Lith": return Tier.Lith;
                case "Meso": return Tier.Meso;
                case "Neo": return Tier.Neo;
                default: return null;
            }
        }

        public static Tier ReadJson(JsonReader reader, JsonSerializer serializer) {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this Tier value, JsonWriter writer, JsonSerializer serializer) {
            switch (value) {
                case Tier.Axi: serializer.Serialize(writer, "Axi"); break;
                case Tier.Lith: serializer.Serialize(writer, "Lith"); break;
                case Tier.Meso: serializer.Serialize(writer, "Meso"); break;
                case Tier.Neo: serializer.Serialize(writer, "Neo"); break;
            }
        }
    }

    public static class Serialize {
        public static string ToJson(this List<DropTables> self) => JsonConvert.SerializeObject(self, Saltyeyes.Cephalon.Parsers.DropTables.Converter.Settings);
    }

    internal class Converter : JsonConverter {
        public override bool CanConvert(Type t) => t == typeof(Rarity) || t == typeof(Tier) || t == typeof(Rarity?) || t == typeof(Tier?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer) {
            if (t == typeof(Rarity))
                return RarityExtensions.ReadJson(reader, serializer);
            if (t == typeof(Tier))
                return TierExtensions.ReadJson(reader, serializer);
            if (t == typeof(Rarity?)) {
                if (reader.TokenType == JsonToken.Null) return null;
                return RarityExtensions.ReadJson(reader, serializer);
            }
            if (t == typeof(Tier?)) {
                if (reader.TokenType == JsonToken.Null) return null;
                return TierExtensions.ReadJson(reader, serializer);
            }
            throw new Exception("Unknown type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var t = value.GetType();
            if (t == typeof(Rarity)) {
                ((Rarity)value).WriteJson(writer, serializer);
                return;
            }
            if (t == typeof(Tier)) {
                ((Tier)value).WriteJson(writer, serializer);
                return;
            }
            throw new Exception("Unknown type");
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new Converter(),
                new IsoDateTimeConverter()
                {
                    DateTimeStyles = DateTimeStyles.AssumeUniversal,
                },
            },
        };
    }
}
