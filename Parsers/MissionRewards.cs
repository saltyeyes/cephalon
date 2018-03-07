namespace Saltyeyes.Cephalon.Parsers.Rewards {
    using System;
    using System.Collections.Generic;
    using System.Net;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MissionReward {
        [JsonProperty("item")]
        public string Item { get; set; }

        [JsonProperty("locations")]
        public Location[] Locations { get; set; }
    }

    public partial class Location {
        [JsonProperty("planet")]
        public Planet Planet { get; set; }

        [JsonProperty("node")]
        public string Node { get; set; }

        [JsonProperty("mission")]
        public Mission Mission { get; set; }

        [JsonProperty("chance")]
        public double Chance { get; set; }

        [JsonProperty("rotation")]
        public Rotation? Rotation { get; set; }
    }

    public enum Mission { Arena, Assassination, AssassinationExtra, Caches, Capture, Conclave, ConclaveExtra, Defection, Defense, Excavation, Exterminate, InfestedSalvage, Interception, MobileDefense, Pursuit, Rescue, Rush, Sabotage, Spy, Survival };

    public enum Planet { Ceres, Derelict, Earth, Eris, Europa, Jupiter, KuvaFortress, Lua, Mars, Mercury, Neptune, Phobos, Pluto, Saturn, Sedna, Uranus, Venus, Void };

    public enum Rotation { A, B, C };

    public partial class MissionReward {
        public static MissionReward[] FromJson(string json) => JsonConvert.DeserializeObject<MissionReward[]>(json, Rewards.Converter.Settings);
    }

    static class MissionExtensions {
        public static Mission? ValueForString(string str) {
            switch (str) {
                case "Arena": return Mission.Arena;
                case "Assassination": return Mission.Assassination;
                case "Assassination Extra": return Mission.AssassinationExtra;
                case "Caches": return Mission.Caches;
                case "Capture": return Mission.Capture;
                case "Conclave": return Mission.Conclave;
                case "Conclave Extra": return Mission.ConclaveExtra;
                case "Defection": return Mission.Defection;
                case "Defense": return Mission.Defense;
                case "Excavation": return Mission.Excavation;
                case "Exterminate": return Mission.Exterminate;
                case "Infested Salvage": return Mission.InfestedSalvage;
                case "Interception": return Mission.Interception;
                case "Mobile Defense": return Mission.MobileDefense;
                case "Pursuit": return Mission.Pursuit;
                case "Rescue": return Mission.Rescue;
                case "Rush": return Mission.Rush;
                case "Sabotage": return Mission.Sabotage;
                case "Spy": return Mission.Spy;
                case "Survival": return Mission.Survival;
                default: return null;
            }
        }

        public static Mission ReadJson(JsonReader reader, JsonSerializer serializer) {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this Mission value, JsonWriter writer, JsonSerializer serializer) {
            switch (value) {
                case Mission.Arena: serializer.Serialize(writer, "Arena"); break;
                case Mission.Assassination: serializer.Serialize(writer, "Assassination"); break;
                case Mission.AssassinationExtra: serializer.Serialize(writer, "Assassination Extra"); break;
                case Mission.Caches: serializer.Serialize(writer, "Caches"); break;
                case Mission.Capture: serializer.Serialize(writer, "Capture"); break;
                case Mission.Conclave: serializer.Serialize(writer, "Conclave"); break;
                case Mission.ConclaveExtra: serializer.Serialize(writer, "Conclave Extra"); break;
                case Mission.Defection: serializer.Serialize(writer, "Defection"); break;
                case Mission.Defense: serializer.Serialize(writer, "Defense"); break;
                case Mission.Excavation: serializer.Serialize(writer, "Excavation"); break;
                case Mission.Exterminate: serializer.Serialize(writer, "Exterminate"); break;
                case Mission.InfestedSalvage: serializer.Serialize(writer, "Infested Salvage"); break;
                case Mission.Interception: serializer.Serialize(writer, "Interception"); break;
                case Mission.MobileDefense: serializer.Serialize(writer, "Mobile Defense"); break;
                case Mission.Pursuit: serializer.Serialize(writer, "Pursuit"); break;
                case Mission.Rescue: serializer.Serialize(writer, "Rescue"); break;
                case Mission.Rush: serializer.Serialize(writer, "Rush"); break;
                case Mission.Sabotage: serializer.Serialize(writer, "Sabotage"); break;
                case Mission.Spy: serializer.Serialize(writer, "Spy"); break;
                case Mission.Survival: serializer.Serialize(writer, "Survival"); break;
            }
        }
    }

    static class PlanetExtensions {
        public static Planet? ValueForString(string str) {
            switch (str) {
                case "Ceres": return Planet.Ceres;
                case "Derelict": return Planet.Derelict;
                case "Earth": return Planet.Earth;
                case "Eris": return Planet.Eris;
                case "Europa": return Planet.Europa;
                case "Jupiter": return Planet.Jupiter;
                case "Kuva Fortress": return Planet.KuvaFortress;
                case "Lua": return Planet.Lua;
                case "Mars": return Planet.Mars;
                case "Mercury": return Planet.Mercury;
                case "Neptune": return Planet.Neptune;
                case "Phobos": return Planet.Phobos;
                case "Pluto": return Planet.Pluto;
                case "Saturn": return Planet.Saturn;
                case "Sedna": return Planet.Sedna;
                case "Uranus": return Planet.Uranus;
                case "Venus": return Planet.Venus;
                case "Void": return Planet.Void;
                default: return null;
            }
        }

        public static Planet ReadJson(JsonReader reader, JsonSerializer serializer) {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this Planet value, JsonWriter writer, JsonSerializer serializer) {
            switch (value) {
                case Planet.Ceres: serializer.Serialize(writer, "Ceres"); break;
                case Planet.Derelict: serializer.Serialize(writer, "Derelict"); break;
                case Planet.Earth: serializer.Serialize(writer, "Earth"); break;
                case Planet.Eris: serializer.Serialize(writer, "Eris"); break;
                case Planet.Europa: serializer.Serialize(writer, "Europa"); break;
                case Planet.Jupiter: serializer.Serialize(writer, "Jupiter"); break;
                case Planet.KuvaFortress: serializer.Serialize(writer, "Kuva Fortress"); break;
                case Planet.Lua: serializer.Serialize(writer, "Lua"); break;
                case Planet.Mars: serializer.Serialize(writer, "Mars"); break;
                case Planet.Mercury: serializer.Serialize(writer, "Mercury"); break;
                case Planet.Neptune: serializer.Serialize(writer, "Neptune"); break;
                case Planet.Phobos: serializer.Serialize(writer, "Phobos"); break;
                case Planet.Pluto: serializer.Serialize(writer, "Pluto"); break;
                case Planet.Saturn: serializer.Serialize(writer, "Saturn"); break;
                case Planet.Sedna: serializer.Serialize(writer, "Sedna"); break;
                case Planet.Uranus: serializer.Serialize(writer, "Uranus"); break;
                case Planet.Venus: serializer.Serialize(writer, "Venus"); break;
                case Planet.Void: serializer.Serialize(writer, "Void"); break;
            }
        }
    }

    static class RotationExtensions {
        public static Rotation? ValueForString(string str) {
            switch (str) {
                case "A": return Rotation.A;
                case "B": return Rotation.B;
                case "C": return Rotation.C;
                default: return null;
            }
        }

        public static Rotation ReadJson(JsonReader reader, JsonSerializer serializer) {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this Rotation value, JsonWriter writer, JsonSerializer serializer) {
            switch (value) {
                case Rotation.A: serializer.Serialize(writer, "A"); break;
                case Rotation.B: serializer.Serialize(writer, "B"); break;
                case Rotation.C: serializer.Serialize(writer, "C"); break;
            }
        }
    }

    public static class Serialize {
        public static string ToJson(this MissionReward[] self) => JsonConvert.SerializeObject(self, Rewards.Converter.Settings);
    }

    internal class Converter : JsonConverter {
        public override bool CanConvert(Type t) => t == typeof(Mission) || t == typeof(Planet) || t == typeof(Rotation) || t == typeof(Mission?) || t == typeof(Planet?) || t == typeof(Rotation?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer) {
            if (t == typeof(Mission))
                return MissionExtensions.ReadJson(reader, serializer);
            if (t == typeof(Planet))
                return PlanetExtensions.ReadJson(reader, serializer);
            if (t == typeof(Rotation))
                return RotationExtensions.ReadJson(reader, serializer);
            if (t == typeof(Mission?)) {
                if (reader.TokenType == JsonToken.Null) return null;
                return MissionExtensions.ReadJson(reader, serializer);
            }
            if (t == typeof(Planet?)) {
                if (reader.TokenType == JsonToken.Null) return null;
                return PlanetExtensions.ReadJson(reader, serializer);
            }
            if (t == typeof(Rotation?)) {
                if (reader.TokenType == JsonToken.Null) return null;
                return RotationExtensions.ReadJson(reader, serializer);
            }
            throw new Exception("Unknown type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var t = value.GetType();
            if (t == typeof(Mission)) {
                ((Mission)value).WriteJson(writer, serializer);
                return;
            }
            if (t == typeof(Planet)) {
                ((Planet)value).WriteJson(writer, serializer);
                return;
            }
            if (t == typeof(Rotation)) {
                ((Rotation)value).WriteJson(writer, serializer);
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
