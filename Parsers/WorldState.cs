namespace Saltyeyes.Cephalon.Parsers.WorldState
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using J = Newtonsoft.Json.JsonPropertyAttribute;

    public class Translations
    {
        public string en { get; set; }
        public string fr { get; set; }
        public string it { get; set; }
        public string de { get; set; }
        public string es { get; set; }
        public string pt { get; set; }
        public string ru { get; set; }
        public string pl { get; set; }
        public string tr { get; set; }
        public string ja { get; set; }
        public string zh { get; set; }
        public string ko { get; set; }
        public string tc { get; set; }
    }

    public class News
    {
        public string id { get; set; }
        public string message { get; set; }
        public string link { get; set; }
        public string imageLink { get; set; }
        public bool priority { get; set; }
        public DateTimeOffset date { get; set; }
        public string eta { get; set; }
        public bool update { get; set; }
        public bool primeAccess { get; set; }
        public bool stream { get; set; }
        public Translations translations { get; set; }
        public string asString { get; set; }
        public DateTimeOffset? startDate { get; set; }
        public DateTimeOffset? endDate { get; set; }
    }

    public class Job
    {
        public string id { get; set; }
        public string type { get; set; }
        public List<int> enemyLevels { get; set; }
        public List<int> standingStages { get; set; }
        public List<string> rewardPool { get; set; }
    }

    public class Event
    {
        public string id { get; set; }
        public DateTimeOffset expiry { get; set; }
        public string description { get; set; }
        public string tooltip { get; set; }
        public List<object> concurrentNodes { get; set; }
        public string victimNode { get; set; }
        public List<object> rewards { get; set; }
        public bool expired { get; set; }
        public string health { get; set; }
        public string affiliatedWith { get; set; }
        public List<Job> jobs { get; set; }
        public string asString { get; set; }
    }

    public class Reward
    {
        public List<object> items { get; set; }
        public List<object> countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }

    public class Mission
    {
        public string node { get; set; }
        public string type { get; set; }
        public string faction { get; set; }
        public Reward reward { get; set; }
        public int minEnemyLevel { get; set; }
        public int maxEnemyLevel { get; set; }
        public int maxWaveNum { get; set; }
        public bool nightmare { get; set; }
        public bool archwingRequired { get; set; }
    }

    public class Alert
    {
        public string id { get; set; }
        public DateTimeOffset activation { get; set; }
        public DateTimeOffset expiry { get; set; }
        public Mission mission { get; set; }
        public bool expired { get; set; }
        public string eta { get; set; }
        public List<string> rewardTypes { get; set; }
    }

    public class Variant
    {
        public string boss { get; set; }
        public string planet { get; set; }
        public string missionType { get; set; }
        public string modifier { get; set; }
        public string modifierDescription { get; set; }
        public string node { get; set; }
    }

    public class Sortie
    {
        public string id { get; set; }
        public DateTimeOffset activation { get; set; }
        public DateTimeOffset expiry { get; set; }
        public string rewardPool { get; set; }
        public List<Variant> variants { get; set; }
        public string boss { get; set; }
        public string faction { get; set; }
        public bool expired { get; set; }
        public string eta { get; set; }
    }

    public class SyndicateMission
    {
        public string id { get; set; }
        public DateTimeOffset activation { get; set; }
        public DateTimeOffset expiry { get; set; }
        public string syndicate { get; set; }
        public List<object> nodes { get; set; }
        public List<object> jobs { get; set; }
        public string eta { get; set; }
    }

    public class Fissure
    {
        public string id { get; set; }
        public string node { get; set; }
        public string missionType { get; set; }
        public string enemy { get; set; }
        public string tier { get; set; }
        public int tierNum { get; set; }
        public DateTimeOffset activation { get; set; }
        public DateTimeOffset expiry { get; set; }
        public bool expired { get; set; }
        public string eta { get; set; }
    }

    public class FlashSale
    {
        public string item { get; set; }
        public DateTimeOffset expiry { get; set; }
        public int discount { get; set; }
        public int premiumOverride { get; set; }
        public bool isFeatured { get; set; }
        public bool isPopular { get; set; }
        public string id { get; set; }
        public bool expired { get; set; }
        public string eta { get; set; }
    }

    public class AttackerReward
    {
        public List<object> items { get; set; }
        public List<object> countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }

    public class CountedItem
    {
        public int count { get; set; }
        public string type { get; set; }
    }

    public class DefenderReward
    {
        public List<object> items { get; set; }
        public List<CountedItem> countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }

    public class Invasion
    {
        public string id { get; set; }
        public string node { get; set; }
        public string desc { get; set; }
        public AttackerReward attackerReward { get; set; }
        public string attackingFaction { get; set; }
        public DefenderReward defenderReward { get; set; }
        public string defendingFaction { get; set; }
        public bool vsInfestation { get; set; }
        public DateTimeOffset activation { get; set; }
        public int count { get; set; }
        public int requiredRuns { get; set; }
        public double completion { get; set; }
        public bool completed { get; set; }
        public string eta { get; set; }
        public List<string> rewardTypes { get; set; }
    }

    public class DarkSector
    {
        public string id { get; set; }
        public bool isAlliance { get; set; }
        public string defenderName { get; set; }
        public int defenderDeployemntActivation { get; set; }
        public string defenderMOTD { get; set; }
        public string deployerName { get; set; }
        public string deployerClan { get; set; }
        public List<object> history { get; set; }
    }

    public class VoidTrader
    {
        public string id { get; set; }
        public DateTimeOffset activation { get; set; }
        public DateTimeOffset expiry { get; set; }
        public string character { get; set; }
        public string location { get; set; }
        public List<object> inventory { get; set; }
        public string psId { get; set; }
        public bool active { get; set; }
        public string startString { get; set; }
        public string endString { get; set; }
    }

    public class DailyDeal
    {
        public string item { get; set; }
        public DateTimeOffset expiry { get; set; }
        public int originalPrice { get; set; }
        public int salePrice { get; set; }
        public int total { get; set; }
        public int sold { get; set; }
        public string id { get; set; }
        public string eta { get; set; }
        public int discount { get; set; }
    }

    public class Simaris
    {
        public string target { get; set; }
        public bool isTargetActive { get; set; }
        public string asString { get; set; }
    }

    public class ConclaveChallenge
    {
        public string id { get; set; }
        public string description { get; set; }
        public DateTimeOffset expiry { get; set; }
        public int amount { get; set; }
        public string mode { get; set; }
        public string category { get; set; }
        public string eta { get; set; }
        public bool expired { get; set; }
        public bool daily { get; set; }
        public bool rootChallenge { get; set; }
        public string endString { get; set; }
        public string asString { get; set; }
    }

    public class EarthCycle
    {
        public string id { get; set; }
        public DateTimeOffset expiry { get; set; }
        public bool isDay { get; set; }
        public string timeLeft { get; set; }
    }

    public class CetusCycle
    {
        public string id { get; set; }
        public DateTimeOffset expiry { get; set; }
        public bool isDay { get; set; }
        public string timeLeft { get; set; }
        public bool isCetus { get; set; }
    }

    public class ConstructionProgress
    {
        public string id { get; set; }
        public string fomorianProgress { get; set; }
        public string razorbackProgress { get; set; }
        public string unknownProgress { get; set; }
    }

    public partial class WorldState
    {
        public DateTimeOffset timestamp { get; set; }
        public List<News> news { get; set; }
        public List<Event> events { get; set; }
        public List<Alert> alerts { get; set; }
        public Sortie sortie { get; set; }
        public List<SyndicateMission> syndicateMissions { get; set; }
        public List<Fissure> fissures { get; set; }
        public List<object> globalUpgrades { get; set; }
        public List<FlashSale> flashSales { get; set; }
        public List<Invasion> invasions { get; set; }
        public List<DarkSector> darkSectors { get; set; }
        public VoidTrader voidTrader { get; set; }
        public List<DailyDeal> dailyDeals { get; set; }
        public Simaris simaris { get; set; }
        public List<ConclaveChallenge> conclaveChallenges { get; set; }
        public List<object> persistentEnemies { get; set; }
        public EarthCycle earthCycle { get; set; }
        public CetusCycle cetusCycle { get; set; }
        public ConstructionProgress constructionProgress { get; set; }
    }

    public partial class WorldState
    {
        public static WorldState FromJson(string json) => JsonConvert.DeserializeObject<WorldState>(json, Saltyeyes.Cephalon.Parsers.WorldState.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this WorldState self) => JsonConvert.SerializeObject(self, Saltyeyes.Cephalon.Parsers.WorldState.Converter.Settings);
    }

    internal class Converter
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            Converters = {
                new IsoDateTimeConverter()
                {
                    DateTimeStyles = DateTimeStyles.AssumeUniversal,
                },
            },
        };

        public static JsonSerializerSettings Settings => settings;
    }
}