using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

using Microsoft.Bot.Connector;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using Saltyeyes.Cephalon.Parsers.DropTables;
using Saltyeyes.Cephalon.Parsers.Relics;
using Saltyeyes.Cephalon.Parsers.Rewards;
using Saltyeyes.Cephalon.Parsers.WorldState;
using Saltyeyes.Cephalon.Properties;
using System.Text;

using System.IO;

using AdaptiveCards;
using System.Web;
using System.Globalization;
using Microsoft.Bot.Builder.ConnectorEx;
using System.Threading;

namespace Saltyeyes.Cephalon {

    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [LuisModel("1f849047-fa83-48a9-bc60-503780e95a56", "01af16f6ce5d43c0831095d5fc9aeaf7")]
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object> {

        [NonSerialized]
        Timer t;
        
        public async Task RegisterAlarm(IDialogContext context, IMessageActivity message) {
            ConversationStarter.toId = message.From.Id;
            ConversationStarter.toName = message.From.Name;
            ConversationStarter.fromId = message.Recipient.Id;
            ConversationStarter.fromName = message.Recipient.Name;
            ConversationStarter.serviceUrl = message.ServiceUrl;
            ConversationStarter.channelId = message.ChannelId;
            ConversationStarter.conversationId = message.Conversation.Id;
            t = new Timer(new TimerCallback(timerEvent));
            t.Change(10000, Timeout.Infinite);

            var response = context.MakeMessage();

            var url = HttpContext.Current.Request.Url;
            //We now tell the user that we will talk to them in a few seconds
            response.Text = "Hello! In a few seconds I'll send you a message proactively to demonstrate how bots can initiate messages. You can also make me send a message by accessing: " +
                    url.Scheme + "://" + url.Host + ":" + url.Port + "/api/CustomWebApi";
            await context.PostAsync(response);
            //context.Wait(RegisterAlarm);
        }

        public void timerEvent(object target) {
            t.Dispose();
            ConversationStarter.Resume(ConversationStarter.conversationId, ConversationStarter.channelId);
        }

        static readonly Dictionary<String, Dictionary<String, decimal>> rarityChances = new Dictionary<String, Dictionary<String, decimal>>() {
            {"Intact", new Dictionary<String, decimal>(){
                {"Common", 25.33M},
                {"Uncommon", 11},
                {"Rare", 2}
            } },
            {"Exceptional", new Dictionary<String, decimal>(){
                {"Common", 23.33M},
                {"Uncommon", 13},
                {"Rare", 4}
            } },
            {"Flawless", new Dictionary<String, decimal>(){
                {"Common", 20},
                {"Uncommon", 17},
                {"Rare", 6}
            } },
            {"Radiant", new Dictionary<String, decimal>(){
                {"Common", 16.67M},
                {"Uncommon", 20},
                {"Rare", 10}
            } }
        };

        public static readonly Dictionary<String, List<String>> damageTypes = new Dictionary<String, List<String>>() {
            { "Magnetic", new List<String>(){"Cold", "Electricity" } },
            { "Blast", new List<String>(){"Cold", "Heat" } },
            { "Radiation", new List<String>(){ "Electricity", "Heat" } },
            { "Viral", new List<String>(){"Cold", "Toxin" } },
            { "Gas", new List<String>(){"Heat", "Toxin" } },
            { "Corrosive", new List<String>(){"Electricity", "Toxin" } }
        };

        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"]))) {
        }

        protected WorldState CurrentWorldState {
            get {
                using (WebClient wc = new WebClient()) {
                    var json = wc.DownloadString("https://ws.warframestat.us/pc");
                    return JsonConvert.DeserializeObject<WorldState>(json);
                }
            }
        }

        public static Dictionary<string, Dictionary<string, decimal>> RarityChances => rarityChances;

        private AdaptiveColumnSet GetRelicCard(Relic relic) {

            AdaptiveColumnSet columnSet = new AdaptiveColumnSet();
            AdaptiveColumn imageColumn = new AdaptiveColumn() { Width="auto" };
            AdaptiveColumn textColumn = new AdaptiveColumn() { Width = "stretch" };
            imageColumn.Items.Add(new AdaptiveImage() {
                Url = new Uri(Images.ResourceManager.GetString(String.Format("{0}{1}", relic.Era, relic.Rarity =="Common"?"Intact":"Radiant"))),
                Size = AdaptiveImageSize.Small
            });
            textColumn.Items.Add(new AdaptiveTextBlock() {
                Text = String.Format("{0} {1} Relic", relic.Era, relic.Name),
                Weight = AdaptiveTextWeight.Bolder,
                Size = AdaptiveTextSize.Medium
            });
            NumberFormatInfo floatPercentageFormat = new NumberFormatInfo { PercentPositivePattern = 1, PercentDecimalDigits=2 };
            NumberFormatInfo intPercentageFormat = new NumberFormatInfo { PercentPositivePattern = 1, PercentDecimalDigits=0 };
            decimal intactChance = RarityChances["Intact"][relic.Rarity];
            decimal radiantChance = RarityChances["Radiant"][relic.Rarity];
            //textColumn.Items.Add(new AdaptiveTextBlock() {
            //    Text = relic.Rarity,
            //    Weight = AdaptiveTextWeight.Lighter,
            //    Size = AdaptiveTextSize.Default,
            //    Spacing = AdaptiveSpacing.None
            //});
            textColumn.Items.Add(new AdaptiveTextBlock() {
                Text = String.Format("{2} | {0}→{1}", 
                    (intactChance/100).ToString("P0", intPercentageFormat),
                    (radiantChance/100).ToString("P0", intPercentageFormat),
                    relic.Rarity
                ),
                Weight = AdaptiveTextWeight.Lighter,
                Size = AdaptiveTextSize.Default,
                Spacing = AdaptiveSpacing.None
            });
            columnSet.Columns.Add(imageColumn);
            columnSet.Columns.Add(textColumn);
            return columnSet;
        }

        private AdaptiveColumn GetDamageTypeColumn (String damageType) {
            AdaptiveColumn column = new AdaptiveColumn();
            column.Items.Add(new AdaptiveImage() {
                Url = new Uri(Images.ResourceManager.GetString(damageType)),
                HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                Size = AdaptiveImageSize.Small
            });
            column.Items.Add(new AdaptiveTextBlock() {
                Text = damageType,
                Weight = AdaptiveTextWeight.Lighter,
                Size = AdaptiveTextSize.Default,
                HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
            });
            return column;
        }

        private AdaptiveColumn GetIconColumn(String icon) {
            AdaptiveColumn column = new AdaptiveColumn();
            column.Items.Add(new AdaptiveImage() {
                Url = new Uri(Images.ResourceManager.GetString(icon)),
                HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                Size = AdaptiveImageSize.Small
            });
            return column;
        }

        private AdaptiveContainer GetRelicCards (List<Relic> relics) {
            AdaptiveContainer container = new AdaptiveContainer();
            foreach (Relic relic in relics) {
                container.Items.Add(GetRelicCard(relic));
            }
            return container;
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result) {
            var response = context.MakeMessage();
            response.Text = Utils.RandomSSML("ErrorSSML");

            await context.PostAsync(response);
            //context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task WelcomeIntent(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result) {
            var response = context.MakeMessage();
            response.Text = Utils.RandomSSML("GreetingSSML");



            response.InputHint = InputHints.ExpectingInput;
            //await context.PostAsync(response);
            var message = await activity;
            await RegisterAlarm(context, message);
            //context.Wait(MessageReceived);
        }

        [LuisIntent("DamageTypeInfo")]
        public async Task DamageTypeIntent(IDialogContext context, LuisResult result) {
            var response = context.MakeMessage();

            String damageType = null;
            IList<EntityRecommendation> listOfEntitiesFound = result.Entities;
            foreach (EntityRecommendation item in listOfEntitiesFound) {
                if (item.Type == "DamageType") {
                    damageType = item.Entity;
                }
            }
            if (damageType == null) {
                response.Text = Utils.RandomSSML("DamageTypeErrorSSML");
            } else {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                damageType = textInfo.ToTitleCase(damageType);
                List<String> comboType = damageTypes[damageType];
                AdaptiveCard card = new AdaptiveCard();
                AdaptiveContainer container = new AdaptiveContainer();
                AdaptiveColumnSet columnSet = new AdaptiveColumnSet();
                columnSet.Columns.Add(GetDamageTypeColumn(comboType[0]));
                columnSet.Columns.Add(GetIconColumn("Add"));
                columnSet.Columns.Add(GetDamageTypeColumn(comboType[1]));
                columnSet.Columns.Add(GetIconColumn("Equal"));
                columnSet.Columns.Add(GetDamageTypeColumn(damageType));
                container.Items.Add(columnSet);
                card.Body.Add(container);
                response.Speak = String.Format(Utils.RandomSSML("DamageTypeSSML"), damageType, comboType[0], comboType[1]);

                Attachment attachment = new Attachment() {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                };

                response.Attachments.Add(attachment);
            }

            response.InputHint = InputHints.ExpectingInput;
            await context.PostAsync(response);
            //context.Wait(MessageReceived);
        }

        [LuisIntent("Sortie")]
        public async Task SortieIntent(IDialogContext context, LuisResult result) {
            var response = context.MakeMessage();
            List<String> parts = new List<string>();
            parts.Add(String.Format(Utils.RandomSSML("SortieFactionSSML"), CurrentWorldState.sortie.faction));
            parts.Add(String.Format(Utils.RandomSSML("SortieTimeSSML"), Utils.TimeRemaining(CurrentWorldState.sortie.expiry)));
            foreach (Variant variant in CurrentWorldState.sortie.variants) {
                parts.Add(String.Format(Utils.RandomSSML("SortieMissionSSML"), 
                    SSMLHelper.SayAs("ordinal", CurrentWorldState.sortie.variants.IndexOf(variant)),
                    Utils.Article(variant.missionType),
                    Utils.Article(variant.modifier),
                    variant.missionType,
                    variant.modifier
                ));
            }
            response.Text = Utils.RandomSSML("SortieSSML");

            response.InputHint = InputHints.ExpectingInput;
            await context.PostAsync(response);
            //context.Wait(MessageReceived);
        }

        [LuisIntent("RelicInfo")]
        public async Task RelicInfoIntent(IDialogContext context, LuisResult result) {
            var response = context.MakeMessage();

            try {
                var relicInfo = RelicsByItem.FromJson(File.ReadAllText(HttpContext.Current.Server.MapPath("~/data/relicDropData.json")));
                String itemPrefix = null, itemSuffix = "blueprint";

                IList<EntityRecommendation> listOfEntitiesFound = result.Entities;
                foreach (EntityRecommendation item in listOfEntitiesFound) {
                    if (item.Type == "PartType") {
                        itemSuffix = item.Entity;
                    } else if (item.Type == "RelicDrops") {
                        itemPrefix = item.Entity;
                    }
                }

                if (itemPrefix == null) {
                    response.Text = Utils.RandomSSML("RelicPrefixMissingSSML");
                } else {
                    if (!relicInfo.Prefixes.Contains(itemPrefix)) {
                        response.Text = Utils.RandomSSML("RelicPrefixUnknownSSML");
                    } else if (!relicInfo.Suffixes.Contains(itemSuffix)) {
                        response.Text = Utils.RandomSSML("RelicSuffixMissingSSML") + String.Format("|{0}|{1}", itemPrefix, itemSuffix);
                    } else {
                        foreach (Item item in relicInfo.Items) {
                            if (item.ItemPrefix == itemPrefix) {
                                List<Relic> relics = null;
                                ItemSuffix itemData = null;
                                foreach (ItemSuffix itemWithSuffix in item.ItemSuffixes) {
                                    if (itemWithSuffix.Suffix == itemSuffix) {
                                        relics = itemWithSuffix.Relics;
                                        itemData = itemWithSuffix;
                                    }
                                }
                                if (relics == null || itemData == null) {
                                    response.Text = Utils.RandomSSML("RelicPrefixHasNoSuffixSSML");
                                } else {
                                    AdaptiveCard card = new AdaptiveCard();
                                    AdaptiveContainer container = new AdaptiveContainer();

                                    //card.Body.Add(new AdaptiveTextBlock() {
                                    //    Text = "Here's the drop data for",
                                    //    Size = AdaptiveTextSize.Medium
                                    //});
                                    card.Body.Add(new AdaptiveImage() {
                                        Url = new Uri(itemData.Image),
                                        Size = AdaptiveImageSize.Stretch
                                    });

                                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                                    card.Body.Add(new AdaptiveTextBlock() {
                                        Text = textInfo.ToTitleCase(String.Format("{0} {1}", itemPrefix, itemSuffix)),
                                        Size = AdaptiveTextSize.ExtraLarge
                                    });

                                    card.Body.Add(GetRelicCards(relics));

                                    Attachment attachment = new Attachment() {
                                        ContentType = AdaptiveCard.ContentType,
                                        Content = card
                                    };

                                    response.Speak = String.Format(Utils.RandomSSML("RelicSuccessSSML"), textInfo.ToTitleCase(itemPrefix), textInfo.ToTitleCase(itemSuffix));

                                    response.Attachments.Add(attachment);
                                }
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                response.Text = ex.Message + "\n\n" + ex.StackTrace;
            }
            response.InputHint = InputHints.ExpectingInput;
            await context.PostAsync(response);
            //context.Wait(MessageReceived);
        }


        [LuisIntent("CetusTime")]
        public async Task CetusTimeIntent(IDialogContext context, LuisResult result) {
            var response = context.MakeMessage();

            var worldState = CurrentWorldState;

            var card = new HeroCard() {
                Title = string.Format("It's {0} on the Plains.", worldState.cetusCycle.isDay ? "daytime" : "night"),
                Images = new List<CardImage>() {
                    new CardImage(worldState.cetusCycle.isDay ? "https://i.imgur.com/kllwm5a.jpg" : "https://i.imgur.com/UNgWSQe.jpg")
                },
                Buttons = new List<CardAction>() {
                    new CardAction(ActionTypes.ImBack, "Set Alarm", value: "SET_ALARM"),
                }
            };

            var message = context.MakeMessage();

            message.Attachments = new List<Attachment>()
            {
                card.ToAttachment()
            };

            var spoken = string.Empty;
            spoken = string.Format(Resources.ResourceManager.GetString("PlainsTimeSSML"),
                worldState.cetusCycle.isDay ? "daytime" : "night",
                worldState.cetusCycle.isDay ? "set" : "rise",
                Utils.TimeRemaining(worldState.cetusCycle.expiry)
            );

            message.Speak = SSMLHelper.Speak(spoken);

            //response.Text = spoken;

            //response.InputHint = InputHints.ExpectingInput;

            message.InputHint = InputHints.AcceptingInput;


            await context.PostAsync(message);

            context.Done<object>(null);
            //context.Wait(MessageReceived);
        }
    }
}