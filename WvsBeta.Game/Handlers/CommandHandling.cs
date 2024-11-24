using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using MySql.Data.MySqlClient;
using WvsBeta.Common;
using WvsBeta.Common.Characters;
using WvsBeta.Common.DataProviders;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Events;
using WvsBeta.Game.Events.GMEvents;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.Packets;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Game.Handlers
{
    public class CommandHandling
    {
        public static Dictionary<string, int> MapNameList { get; } = new Dictionary<string, int>
        {
            // Job maps
            { "gm", 180000000 },
            { "3rd", 211000001 },
            { "mage", 101000003 },
            { "bowman", 100000201 },
            { "thief", 103000003 },
            { "warrior", 102000003 },
            // Miscellaneous
            { "happyville", 209000000 },
            { "cafe", 193000000 },
            // Maple Island
            { "southperry", 60000 },
            { "amherst", 1010000 },
            // Victoria
            { "henesys", 100000000 },
            { "perion", 102000000 },
            { "ellinia", 101000000 },
            { "sleepy", 105040300 },
            { "sleepywood", 105040300 },
            { "lith", 104000000 },
            { "florina", 110000000 },
            { "kerning", 103000000 },
            // Ossyria
            { "orbis", 200000000 },
            { "elnath", 211000000 },
            { "nath", 211000000 },
            // Ludus Lake area
            { "ludi", 220000000 },
            { "omega", 221000000 },
            // Aquarium
            { "aqua", 230000000 },
            // Training Areas
            { "hhg1", 104040000 },
            { "kerningconstruct", 103010000 },
            { "westrockymountain1", 102020000 },
            { "pigbeach", 104010001 },
            { "fog", 106010102 },
            { "subwayb1", 103000902 },
            { "subwayb2", 103000905 },
            { "subwayb3", 103000909 },
            // Free Markets
            { "fm", 910000000 },
            // Dungeon areas
            { "dungeon", 105090200 },
            { "mine", 211041400 },
            // Area boss maps
            { "jrbalrog", 105090900 },
            { "mushmom", 100000005 },
            // PQ maps
            { "kpqexit", 103000890 },
            { "kpqbonus", 103000805 },
            { "kingslime", 103000804 },
            { "kpq5", 103000804 },
            { "kpq4", 103000803 },
            { "kpq3", 103000802 },
            { "kpq2", 103000801 },
            { "kpq1", 103000800 },
            { "gpq", 101030104 },
            { "sharenia", 101030104 },
            { "lpq", 221024500 },
            // Boss maps
            { "zakum", 280030000 },
            // Contimove
            { "elliniastation", 101000300 },
            { "orbisstation", 200000100 },
            { "orbiselliniastation", 200000111 },
            { "orbisludistation", 200000121 },
            { "orbiselliniatakeoff", 200000112 },
            { "orbiselliniaboat", 200090000 },
            { "elliniaorbistakeoff", 101000301 },
            { "elliniaorbisboat", 200090010 },
            // Events
            { "findthejewel", 109010000 },
            { "jewel", 109010000 },
            { "ftj", 109010000 },
            { "snowball", 109060001 },
            { "fitness", 109040000 },
            { "ox", 109020001 },
            { "quiz", 109020001 },
            { "oxquiz", 109020001 }
        };

        public static IDictionary<string, (int itemid, short amount)[]> ItemPacks = new Dictionary<string, (int itemid, short amount)[]>
        {
            { "gpq", new (int itemid, short amount)[] {
                (1032033, 1),
                (4001025, 4),
                (4001024, 1),
                (4001031, 1), (4001032, 1), (4001033, 1), (4001034, 1)
            }},
            { "lpq", new (int itemid, short amount)[] {
                (4001022, 500),
                (4001023, 10)
            }}
        };

        public static int GetMapidFromName(string name)
        {
            if (MapNameList.ContainsKey(name)) return MapNameList[name];
            else return -1;
        }

        private static GameCharacter.BanReasons GetBanReasonFromText(CommandArg arg)
        {
            switch (arg)
            {
                // Your account has been blocked for hacking or illegal use of third-party programs.
                case "1":
                case "ct":
                case "h":
                case "hack":
                case "hax": return GameCharacter.BanReasons.Hack;

                // Your account has been blocked for using macro / auto-keyboard.
                case "2":
                case "bot":
                case "macro": return GameCharacter.BanReasons.Macro;

                // Your account has been blocked for illicit promotion and advertising.
                case "3":
                case "promo":
                case "ad":
                case "ads":
                case "advertisement": return GameCharacter.BanReasons.Advertisement;

                // Your account has been blocked for for harassment.
                case "4":
                case "harass":
                case "harassment": return GameCharacter.BanReasons.Harassment;

                // Your account has been blocked for using profane language.
                case "5":
                case "trol":
                case "trolling":
                case "curse":
                case "badlanguage": return GameCharacter.BanReasons.BadLanguage;

                // Your account has been blocked for scamming.
                case "6":
                case "scamming":
                case "scam": return GameCharacter.BanReasons.Scam;

                // Your account has been blocked for misconduct.
                case "7":
                case "ks":
                case "misconduct": return GameCharacter.BanReasons.Misconduct;

                // Your account has been blocked for illegal cash transaction
                case "8":
                case "sell":
                case "irlmoney": return GameCharacter.BanReasons.Sell;

                // Your account has been blocked for illegal charging/funding. Please contact customer support for further details.
                case "9":
                case "moneyloundry":
                case "icash": return GameCharacter.BanReasons.ICash;

                default: return GameCharacter.BanReasons.Hack;
            }
        }

        enum UserIdFetchResult
        {
            Found,
            UserNotFound,
            PlayerNotFound,
            IDNotFound,
            UnknownType
        }

        private static UserIdFetchResult GetUserIDFromArgs(CommandArg argType, CommandArg argValue, out int userId)
        {
            userId = 0;
            switch (argType)
            {
                case "uid":
                case "userid":
                    if (!int.TryParse(argValue, out int u) || !Server.Instance.CharacterDatabase.ExistsUser(u))
                    {
                        return UserIdFetchResult.UserNotFound;
                    }
                    else
                    {
                        userId = u;
                    }

                    break;
                case "user":
                case "username":
                    userId = Server.Instance.CharacterDatabase.UserIDByUsername(argValue);
                    if (userId == -1)
                    {
                        return UserIdFetchResult.UserNotFound;
                    }
                    break;
                case "name":
                case "player":
                case "character":
                case "charname":
                    int id = Server.Instance.CharacterDatabase.UserIDByCharacterName(argValue);
                    if (id == -1)
                    {
                        return UserIdFetchResult.PlayerNotFound;
                    }
                    else
                    {
                        userId = id;
                    }
                    break;
                case "cid":
                case "charid":
                    int uid = Server.Instance.CharacterDatabase.UserIDByCharID(int.Parse(argValue));
                    if (uid == -1)
                    {
                        return UserIdFetchResult.IDNotFound;
                    }
                    else
                    {
                        userId = uid;
                    }
                    break;
                default:
                    return UserIdFetchResult.UnknownType;

            }

            return UserIdFetchResult.Found;
        }

        static bool shuttingDown = false;
        static IDictionary<GameCharacter, Packet> pendingPackets = new Dictionary<GameCharacter, Packet>();
        static HashSet<string> lookupTypes = new HashSet<string> { "item", "equip", "map", "mob", /*"quest", "npc", "skill"*/ };

        readonly struct CommandData
        {
            /// <summary>
            /// Command usage description.
            /// </summary>
            public readonly string usage;
            /// <summary>
            /// Command description.
            /// </summary>
            public readonly string description;

            public CommandData(string usage, string description)
            {
                this.usage = usage;
                this.description = description;
            }

            public override string ToString()
            {
                return $"{usage} : {description}";
            }
        }

        static Dictionary<string, CommandData> regularCommands = new Dictionary<string, CommandData>
        {
            { "help", new CommandData("/help [page]", "Display this text.") },
            { "roll", new CommandData("/roll", "Roll a random number between 1-100.") }
        };
        static Dictionary<string, CommandData> testerCommands = new Dictionary<string, CommandData>
        {
            { "lookup", new CommandData($"/lookup <{string.Join("|", lookupTypes)}> <name>", "Lookup the IDs for items, equips, or maps.") },
        };
        static Dictionary<string, CommandData> internCommands = new Dictionary<string, CommandData>
        {
            { "whereami", new CommandData($"/whereami", "Gives you your current map ID.") },
            { "map", new CommandData($"/map <id/here/town>", "Teleport to the given map id, or return to the nearest town.") },
            { "mapfs", new CommandData($"/mapfs <fieldset> <map index>", "Teleport to a field set map index.") },
            { "chase", new CommandData($"/chase <player>", "Teleport to a player.") },
            { "warphere", new CommandData($"/warphere <player>", "Teleport a player to you.") },
            { "online", new CommandData($"/online", "See which players are online.") },
            { "kick", new CommandData($"/kick", "Kick a player off the server.") },
            { "permaban", new CommandData("/permaban <userid/charname/charid> <value> [reason]", "Ban a player permanently.") },
            { "suspend", new CommandData("/suspend <userid/charname/charid> <value> <days to suspend> [reason]", "Ban a player temporarily.") },
            { "unban", new CommandData("/unban <userid/charname/charid> <value>", "Unban a player.") },
            { "mute", new CommandData("/mute <userid/charname/charid> <value> <hours> [reason]", "Mute a player.") },
            { "unmute", new CommandData("/unmute <userid/charname/charid> <value>", "Unmute a player.") },
            { "hackmute", new CommandData("/hackmute <charname> <hours>", "Mute the hack log for a given player.") },
            { "hackunmute", new CommandData("/hackunmute <charname>", "Unmute the hack log for a given player.") },
            { "movetracepet", new CommandData("/movetrace(pet|player|mob|summon) <userid/charname/charid> <value> <amount>", "Tracing a given trace type for a given amount.") },
            { "warn", new CommandData("/warn <charname> <text>", "Send a warning to a player.") },
            { "warnmap", new CommandData("/warnmap <text>", "Send a warning to all players in the current map.") },
            { "maxskills", new CommandData("/maxskills", "Max all skills.") },
            { "job", new CommandData("/job <job id>", "Set your job.") },
            { "mp", new CommandData("/mp <value>", "Set your MP.") },
            { "maxmp", new CommandData("/maxmp <value>", "Set your Max MP.") },
            { "hp", new CommandData("/hp <value>", "Set your HP.") },
            { "str", new CommandData("/str <value>", "Set your STR.") },
            { "dex", new CommandData("/dex <value>", "Set your DEX.") },
            { "luk", new CommandData("/luk <value>", "Set your LUK.") },
            { "int", new CommandData("/int <value>", "Set your INT.") },
            { "ap", new CommandData("/ap <value>", "Set your AP.") },
            { "sp", new CommandData("/sp <value>", "Set your SP.") },
            { "addsp", new CommandData("/addsp <value>", "Add SP.") },
            { "level", new CommandData("/level <value>", "Set your level.") },
            { "fame", new CommandData("/fame <value>", "Set your fame.") },
            { "maxslots", new CommandData("/maxslots", "Max all your inventory slots.") },
            { "setslots", new CommandData("/setslots <inventory> <slots>", "Set the number of slots for an inventory.") },
            { "maxstats", new CommandData("/maxstats", "Max all your stats.") },
            { "pos", new CommandData("/pos", "Get current position info.") },
            { "undercover", new CommandData("/undercover <true/false>", "Set undercover status.") },
            { "reports", new CommandData("/reports", "Get the latest abuse reports.") },
            { "whowashere", new CommandData("/whowashere", "Get the last 10 players who were previously in the current map.") },
            { "givecash", new CommandData("/givecash <player> <amount>", "Give nexon cash to a player.") }
        };
        static Dictionary<string, CommandData> gmCommands = new Dictionary<string, CommandData>
        {
            { "item", new CommandData($"/item <itemid> [amount]", "Give yourself an item.") },
            { "itempack", new CommandData($"/itempack <itemid>", "Give yourself an item pack.") },
            { "spawn", new CommandData($"/spawn <mobid> [amount] [summonType] [summonOption]", "Spawn a mob.") },
            { "spawnpos", new CommandData("/spawnpos <mobid> <x> <y> [fh] [summonType] [summonOption]", "Spawn a mob at a given position.") },
            { "fieldset", new CommandData("/fieldset <fieldsetname> <minmembers/maxmembers> [amount]", "Configure field set properties.") },
            { "fieldsetvar", new CommandData("/fieldsetvar <fieldsetname> <key> <value>", "Set a value to a field set variable.") },
            { "removefieldsetvar", new CommandData("/removefieldsetvar <fieldsetname> <key>", "Unset a field set variable.") },
            { "getid", new CommandData("/getid <charname>", "Get the character id from a player.") },
            { "delete", new CommandData("/delete <inventory, 1=equip, 2=use, etc>", "Delete the first item from your inventory.") },
            { "cleardrops", new CommandData("/cleardrops", "Clear all drops in the current map.") },
            { "killall", new CommandData("/killall [how]", "Kill all mobs in the current map.") },
            { "killalldmg", new CommandData("/killalldmg [damage]", "Kill all mobs in the current map with a given damage.") },
            { "dmgall", new CommandData("/dmgall [damage]", "Damage all mobs in the current map with a given damage.") },
            { "damage", new CommandData("/dmgmob <mobid> <dmg>", "Damage a mob.") },
            { "hpbar", new CommandData("/hpbar <mobid> <hp> <maxhp> <colorBottom> <colorTop>", "Add a Boss HP bar to the current map.") },
            { "mapnotice", new CommandData("/mapnotice <text>", "Send a notice to the current map.") },
            { "ditto", new CommandData("/ditto <charname or charid>", "Imitate a character.") },
            { "datto", new CommandData("/datto", "Stop imitating a character.") },
            { "notice", new CommandData("/notice <text>", "Send a notice to the entire channel.") },
            { "setsp", new CommandData("/setsp <skillid> [level/max]", "Set a skill to a given SP level.") },
            { "heal", new CommandData("/heal", "Recover all your HP/MP.") },
            { "resurrect", new CommandData("/resurrect <charname>", "Resurrect a player.") },
            { "exp", new CommandData("/exp <value>", "Give yourself EXP.") },
            { "mesos", new CommandData("/mesos <value>", "Give yourself mesos.") },
            { "pton", new CommandData("/pton <portal id>", "Enable a portal in your map.") },
            { "ptoff", new CommandData("/ptoff <portal id>", "Disable a portal in your map.") },
            { "portals", new CommandData("/portals", "List nearby portals.") },
            { "eventhelp", new CommandData("/eventhelp", "Display the GM event help menu.") },
            { "eventdesc", new CommandData("/eventdesc", "Show the GM event instructions to everyone in the map.") },
            { "startevent", new CommandData("/startevent", "Start the GM event.") },
            { "stopevent", new CommandData("/stopevent", "Stop the GM event.") },
            { "resetcoconuts", new CommandData("/resetcoconuts", "Reset the coconuts in your map. (Note: Only works in the coconuts map!)") },
            { "ftjhelp", new CommandData("/ftjhelp", "Display the Find the Jewel event help menu.") },
            { "snowballhelp", new CommandData("/snowballhelp", "Display the Snowball event help menu.") },
            { "fitnesshelp", new CommandData("/fitnesshelp", "Display the Fitness event help menu.") },
            { "quizhelp", new CommandData("/quizhelp", "Display the Quiz event help menu.") },
        };
        static Dictionary<string, CommandData> adminCommands = new Dictionary<string, CommandData>
        {
            { "shutdown", new CommandData("/shutdown <seconds>", "Shut down the server in <seconds>.") },
            { "clock", new CommandData("/clock <seconds>", "Show a map timer.") },
            { "header", new CommandData("/header <msg>", "Show a scrolling header.") },
            { "headernotice", new CommandData("/headernotice <msg>", "Show a scrolling header and a notice.") },
            { "packet", new CommandData("/packet <hex string>", "Send a packet using a hex string.") },
            { "typedpacket", new CommandData("/typedpacket <type> <value> <type> <value> ... where type is int, short, long, string, byte", "Send a packet using specific types for easy conversion.") },
            { "beginpacket", new CommandData("/beginpacket", "Initiate a pending packet. Used when you need to send a lot of data.") },
            { "endpacket", new CommandData("/endpacket", "Send your pending packet.") },
            { "drop", new CommandData("/drop [itemid] {amount}", "Drop item(s).") },
            { "droptext", new CommandData("/droptext [0=red, 1=green] <your text>", "Drop red/green letters and numbers.") },
            { "toggleportals", new CommandData("/toggleportals", "Toggle portals on/off.") },
            { "makedonator", new CommandData("/makedonator <charname>", "Set a player as a donator.") },
            { "getid2", new CommandData("/getid2 <charname>", "Get character ID.") },
            { "save", new CommandData("/save", "Save your character to the database.") },
            { "saveall", new CommandData("/saveall", "Save all characters in the current channel to the database.") },
            { "petname", new CommandData("/petname", "Change the pet name.") },
            { "vac", new CommandData("/vac [pet/mob]", "Loot all items in the map, provide pet/mob and they will loot it.") },
            { "mobinfo", new CommandData("/mobinfo", "Get current map mob info.") },
            { "mobchase", new CommandData("/mobchase <charname>", "Make all mobs in the map chase provided character.") },
            { "reloadscript", new CommandData("/reloadscript <script name or id> [1 here for all channels]", "Reload a given NPC script.") },
            { "reloadcashshop", new CommandData("/reloadcashshop", "Reload the Cash Shop data.") },
            { "reloadevents", new CommandData("/reloadevents", "Reload world events.") },
            { "setreactorstate", new CommandData("/setreactorstate <field reactor id> <byte state>", "Set the state of a field reactor.") },
            { "triggerreactor", new CommandData("/triggerreractor <field reactor id>", "Trigger a field reactor.") },
            { "triggerreactorbyname", new CommandData("/triggerreactorbyname <reactor name>", "Trigger a field reactor by name.") },
            { "reactor", new CommandData("/reactor <int reactorId> <byte state> <bool facesLeft>", "Add a new reactor at your position.") }
        };
        public static string GetUsage(CommandArgs args)
        {
            var key = args.Command.ToLowerInvariant();
            if (
                !regularCommands.TryGetValue(key, out CommandData value) &&
                !testerCommands.TryGetValue(key, out value) &&
                !internCommands.TryGetValue(key, out value) &&
                !gmCommands.TryGetValue(key, out value) &&
                !adminCommands.TryGetValue(key, out value)
            )
            {
                return "Unknown command!";
            }
            return "Usage: " + value.usage;
        }
        public static bool HandleChat(GameCharacter character, string text)
        {
            string logtext = string.Format("[{0,-9}] {1,-13}: {2}", character.MapID, character.Name, text);
            if (!Directory.Exists("Chatlogs"))
            {
                Directory.CreateDirectory("Chatlogs");
            }
            File.AppendAllText(Path.Combine("Chatlogs", "Map-" + character.MapID + ".txt"), logtext + Environment.NewLine);
            File.AppendAllText(Path.Combine("Chatlogs", character.Name + ".txt"), logtext + Environment.NewLine);

            if (text[0] != '/') return false;

            try
            {
                var Args = new CommandArgs(text);

                // Pleb commands
                switch (Args.Command.ToLowerInvariant())
                {
                    case "help":
                        {
                            int page = 1;
                            if (Args.Count > 0)
                            {
                                page = Math.Max(1, Args[0].GetInt32());
                            }
                            var commands = regularCommands.ToList();
                            if (character.IsTester)
                            {
                                commands.AddRange(testerCommands);
                            }
                            if (character.IsGM)
                            {
                                commands.AddRange(gmCommands);
                            }
                            if (character.IsAdmin)
                            {
                                commands.AddRange(adminCommands);
                            }
                            int pageSize = 10;
                            int pages = (int)Math.Ceiling((double)commands.Count / pageSize);
                            var messages = new List<string>
                            {
                                $"----------------- HELP{(pages > 1 ? $" ({page}/{pages})" : "")} -----------------"
                            };
                            messages.AddRange(commands
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .Select(i => i.Value.ToString()));
                            foreach (var message in messages)
                                character.Message(message);
                            return true;
                        }
                    case "roll":
                        {
                            int roll = Rand32.NextBetween(1, 100);
                            ChatPacket.SendBroadcastMessageToMap(character.Field, $"{character.Name} rolls {roll} (1-100)", BroadcastMessageType.Blue);
                            return true;
                        }
                }

                if (character.IsTester || character.IsGM)
                {
                    switch (Args.Command.ToLowerInvariant())
                    {
                        case "lu":
                        case "lookup":
                            {
                                Func<string, string, bool> findItem = (name, query) => name?.ToLowerInvariant().Contains(query.ToLowerInvariant()) ?? false;
                                if (Args.Count < 2)
                                {
                                    character.Notice($"Usage: /lookup <{string.Join("|", lookupTypes)}> <name>");
                                }
                                else if (!lookupTypes.Contains(Args[0]))
                                {
                                    character.Notice($"Invalid lookup type \"{Args[0]}\". Please select one of: {string.Join(",", lookupTypes)}.");
                                }
                                else
                                {
                                    string lookupType = Args[0];
                                    string searchQuery = string.Join(" ", Args.Args.Skip(1));
                                    var query = Enumerable.Empty<(int id, string name)>();
                                    if (lookupType == "item")
                                    {
                                        query = DataProvider.Items.Select(i => (i.Value.ID, i.Value.Name));
                                    }
                                    else if (lookupType == "equip")
                                    {
                                        query = DataProvider.Equips.Select(i => (i.Value.ID, i.Value.Name));
                                    }
                                    else if (lookupType == "map")
                                    {
                                        query = GameDataProvider.Maps.Select(i => (i.Value.ID, i.Value.Name));
                                    }
                                    else if (lookupType == "mob")
                                    {
                                        query = GameDataProvider.Mobs.Select(i => (i.Value.ID, i.Value.Name));
                                    }

                                    var results = query.Where(i => findItem(i.name, searchQuery)).Take(10).ToList();
                                    if (results.Count == 0)
                                    {
                                        character.Notice("Lookup query returned no results.");
                                    }
                                    else
                                    {
                                        character.Notice($"Lookup query returned {results.Count} results:");
                                        results.Select(i => $"({i.id}) {i.name}").ForEach(msg => character.Notice(msg));
                                    }
                                }
                                return true;
                            }
                    }
                }
                else
                {
                    return true;
                }

                if (character.IsGM) //Intern commands
                {
                    switch (Args.Command.ToLowerInvariant())
                    {
#region Whereami
                        case "whereami":
                        case "currentmap":
                            {
                                character.Notice(string.Format("You are currently at map {0}.", character.Field.ID));
                                return true;
                            }
#endregion
#region Map / Goto

                        case "m":
                        case "map":
                        case "goto":
                            {
                                if (Args.Count > 0)
                                {
                                    var FieldID = -1;

                                    if (!Args[0].IsNumber())
                                    {
                                        var MapStr = Args[0];
                                        var TempID = GetMapidFromName(MapStr);

                                        if (TempID == -1)
                                        {
                                            switch (MapStr)
                                            {
                                                case "here":
                                                    FieldID = character.MapID;
                                                    break;
                                                case "town":
                                                    FieldID = character.Field.ReturnMap;
                                                    break;
                                            }
                                        }
                                        else
                                            FieldID = TempID;
                                    }
                                    else
                                        FieldID = Args[0].GetInt32();

                                    if (GameDataProvider.Maps.ContainsKey(FieldID))
                                        character.ChangeMap(FieldID);
                                    else
                                        character.Message("Map not found.");
                                }
                                return true;
                            }
                        case "fieldsetmap":
                        case "mapfieldset":
                        case "mapfs":
                        case "fsmap":
                        case "mfs":
                        case "fsm":
                        {
                            if (Args.Count < 2) character.Message("Usage: /mapfs <fieldset> <map index>");
                            else if (!FieldSet.Instances.TryGetValue(Args[0], out FieldSet fs)) character.Message("Unknown fieldset.");
                            else if (!int.TryParse(Args[1], out int mapIdx) || fs.Maps.Length - 1 < mapIdx || mapIdx < 0) character.Message("Invalid map index.");
                            else character.ChangeMap(fs.Maps[mapIdx]);
                            return true;
                        }
#endregion

#region Chase / Warp

                        case "chase":
                        case "warp":
                            {
                                if (Args.Count > 0)
                                {
                                    string other = Args[0].Value.ToLower();
                                    var otherChar = Server.Instance.GetCharacter(other);
                                    if (otherChar == null) character.Message("Victim not found.");
                                    else character.ChangeMap(otherChar.MapID);
                                }
                                return true;
                            }

#endregion

#region ChaseHere / WarpHere

                        case "chasehere":
                        case "warphere":
                            {
                                if (Args.Count > 0)
                                {
                                    string other = Args[0].Value.ToLower();
                                    var otherChar = Server.Instance.GetCharacter(other);
                                    if (otherChar == null) character.Message("Victim not found.");
                                    else otherChar.ChangeMap(character.MapID);
                                }
                                return true;
                            }

#endregion

#region Online

                        case "online":
                            {
                                string playersonline =
                                    "Players online (" + Server.Instance.CharacterList.Count + "): \r\n";
                                playersonline += string.Join(
                                    ", ",
                                    Server.Instance.CharacterList.Select(x =>
                                        x.Value.Name + (x.Value.IsAFK ? " (AFK)" : ""))
                                );
                                character.Notice(playersonline);
                                return true;
                            }

#endregion

#region DC / Kick

                        case "dc":
                        case "kick":
                            {
                                if (Args.Count > 0)
                                {
                                    string victim = Args[0].Value.ToLower();
                                    GameCharacter who = Server.Instance.GetCharacter(victim);

                                    if (who != null)
                                        who.Player.Socket.Disconnect();
                                    else
                                        character.Message("You have entered an incorrect name.");
                                }
                                return true;
                            }

#endregion

#region Ban

                        case "ban":
                        case "banhelp":
                            {
                                character.Message(
                                    "Help: Use !permaban <userid/charname/charid> <value> (reason) to ban permanently. Use !suspend <userid/charname/charid> <value> <days to suspend> (reason)");
                                return true;
                            }
                        case "permban":
                        case "permaban":
                            {
                                if (Args.Count >= 2)
                                {
                                    GameCharacter.BanReasons banReason = Args.Count >= 3
                                        ? GetBanReasonFromText(Args[2])
                                        : GameCharacter.BanReasons.Hack;

                                    switch (GetUserIDFromArgs(Args[0], Args[1], out int userId))
                                    {
                                        case UserIdFetchResult.UnknownType: break; // Fallthrough
                                        case UserIdFetchResult.IDNotFound:
                                            character.Message("User with char id " + Args[1] + " does not exist");
                                            return true;
                                        case UserIdFetchResult.PlayerNotFound:
                                            character.Message("Player " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.UserNotFound:
                                            character.Message("User " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.Found:
                                            Server.Instance.CharacterDatabase.PermaBan(userId, (byte)banReason, character.Name, "");
                                            Server.Instance.CenterConnection.KickUser(userId);

                                            var msg =
                                                $"[{character.Name}] Permabanned {Args[0]} {Args[1]} (userid {userId}), reason {banReason}";
                                            Server.Instance.BanDiscordReporter.Enqueue(msg);
                                            ChatPacket.SendBroadcastMessageToGMs(msg, BroadcastMessageType.RedText);
                                            return true;
                                    }
                                }
                                character.Message("Usage: !permaban <userid/charname/charid> <value> (reason)");

                                return true;
                            }
                        case "suspend":
                        case "tempban":
                            {
                                if (Args.Count >= 3 && Args[2].IsNumber())
                                {
                                    GameCharacter.BanReasons banReason = Args.Count > 3
                                        ? GetBanReasonFromText(Args[3])
                                        : GameCharacter.BanReasons.Hack;

                                    switch (GetUserIDFromArgs(Args[0], Args[1], out int userId))
                                    {
                                        case UserIdFetchResult.UnknownType: break; // Fallthrough
                                        case UserIdFetchResult.IDNotFound:
                                            character.Message("User with char id " + Args[1] + " does not exist");
                                            return true;
                                        case UserIdFetchResult.PlayerNotFound:
                                            character.Message("Player " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.UserNotFound:
                                            character.Message("User " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.Found:
                                            var hours = Args[2].GetInt32();

                                            Server.Instance.CharacterDatabase.TempBan(userId, (byte)banReason, hours, character.Name);
                                            Server.Instance.CenterConnection.KickUser(userId);

                                            var msg =
                                                $"[{character.Name}] Tempbanned {Args[0]} {Args[1]} (userid {userId}), reason {banReason}, hours {hours}";
                                            Server.Instance.BanDiscordReporter.Enqueue(msg);
                                            ChatPacket.SendBroadcastMessageToGMs(msg, BroadcastMessageType.RedText);
                                            return true;
                                    }
                                }
                                character.Message(
                                    "Usage: !suspend/tempban <userid/charname/charid> <value> <hours> (reason)");
                                return true;
                            }

#endregion

#region Unban

                        case "unban":
                            {
                                if (Args.Count == 2)
                                {
                                    switch (GetUserIDFromArgs(Args[0], Args[1], out int userId))
                                    {
                                        case UserIdFetchResult.UnknownType: break; // Fallthrough
                                        case UserIdFetchResult.IDNotFound:
                                            character.Message("User with char id " + Args[1] + " does not exist");
                                            return true;
                                        case UserIdFetchResult.PlayerNotFound:
                                            character.Message("Player " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.UserNotFound:
                                            character.Message("User " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.Found:
                                            Server.Instance.CharacterDatabase.RunQuery(
                                                "UPDATE users SET ban_expire = @expire_date WHERE ID = @id",
                                                "@id", userId,
                                                "@expire_date", MasterThread.CurrentDate.AddDays(-1).ToUniversalTime()
                                            );

                                            var msg =
                                                $"[{character.Name}] Unbanned {Args[0]} {Args[1]} (userid {userId})";
                                            Server.Instance.BanDiscordReporter.Enqueue(msg);
                                            ChatPacket.SendBroadcastMessageToGMs(msg, BroadcastMessageType.RedText);
                                            return true;
                                    }
                                }
                                character.Message("Usage: !unban <userid/charname/charid> <value>");

                                return true;
                            }

#endregion

#region Muting

                        case "muteban":
                        case "mute":
                            {
                                if (Args.Count >= 3 && Args[2].IsNumber())
                                {
                                    MuteReasons banReason = Args.Count > 3
                                        ? ChatPacket.ParseMuteReason(Args[3])
                                        : MuteReasons.FoulLanguage;

                                    if (banReason == 0)
                                    {
                                        character.Message("Unknown mute reason.");
                                        return true;
                                    }

                                    switch (GetUserIDFromArgs(Args[0], Args[1], out int userId))
                                    {
                                        case UserIdFetchResult.UnknownType: break; // Fallthrough
                                        case UserIdFetchResult.IDNotFound:
                                            character.Message(
                                                "User with char id " + Args[1] + " does not exist");
                                            return true;
                                        case UserIdFetchResult.PlayerNotFound:
                                            character.Message("Player " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.UserNotFound:
                                            character.Message("User " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.Found:
                                            var hours = Args[2].GetInt32();

                                            Server.Instance.CharacterDatabase.MuteBan(userId, (byte)banReason, hours);

                                            var localPlayers = Server.Instance.CharacterList
                                                .Where(x => x.Value.UserID == userId).ToArray();
                                            if (localPlayers.Length == 0)
                                            {
                                                Server.Instance.CenterConnection.KickUser(userId);
                                            }
                                            else
                                            {
                                                localPlayers.ForEach(x =>
                                                {
                                                    x.Value.MutedUntil = MasterThread.CurrentDate.AddDays(hours);
                                                    x.Value.MuteReason = (byte)banReason;
                                                });
                                            }

                                            var msg =
                                                $"[{character.Name}] Muted {Args[0]} {Args[1]} (userid {userId}), reason {banReason}, hours {hours}";
                                            Server.Instance.MutebanDiscordReporter.Enqueue(msg);
                                            ChatPacket.SendBroadcastMessageToGMs(msg, BroadcastMessageType.RedText);
                                            return true;
                                    }
                                }
                                character.Message(
                                    "Usage: !muteban/mute <userid/charname/charid> <value> <hours> (reason)");
                                return true;
                            }
                        case "unmute":
                            {
                                if (Args.Count == 2)
                                {
                                    switch (GetUserIDFromArgs(Args[0], Args[1], out int userId))
                                    {
                                        case UserIdFetchResult.UnknownType: break; // Fallthrough
                                        case UserIdFetchResult.IDNotFound:
                                            character.Message("User with char id " + Args[1] + " does not exist");
                                            return true;
                                        case UserIdFetchResult.PlayerNotFound:
                                            character.Message("Player " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.UserNotFound:
                                            character.Message("User " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.Found:
                                            Server.Instance.CharacterDatabase.RunQuery(
                                                "UPDATE users SET quiet_ban_expire = @date WHERE ID = @id", "@id",
                                                userId, "@date", MasterThread.CurrentDate);

                                            var localPlayers = Server.Instance.CharacterList
                                                .Where(x => x.Value.UserID == userId).ToArray();
                                            if (localPlayers.Length == 0)
                                            {
                                                Server.Instance.CenterConnection.KickUser(userId);
                                            }
                                            else
                                            {
                                                localPlayers.ForEach(x =>
                                                {
                                                    x.Value.MutedUntil = MasterThread.CurrentDate;
                                                });
                                            }

                                            var msg =
                                                $"[{character.Name}] Unmuted {Args[0]} {Args[1]} (userid {userId})";
                                            Server.Instance.MutebanDiscordReporter.Enqueue(msg);
                                            ChatPacket.SendBroadcastMessageToGMs(msg, BroadcastMessageType.RedText);
                                            return true;
                                    }
                                }
                                character.Message("Usage: !unmute <userid/charname/charid> <value>");

                                return true;
                            }

#endregion

#region Hackmute / Hackunmute

                        case "hackmute":
                            {
                                if (Args.Count == 2 && Args[1].IsNumber())
                                {
                                    var chr = Server.Instance.GetCharacter(Args[0]);
                                    if (chr == null)
                                    {
                                        character.Message("Character " + Args[0] + " not found on this channel.");
                                    }
                                    else
                                    {
                                        var hours = Args[1].GetInt32();
                                        chr.HacklogMuted = MasterThread.CurrentDate.AddHours(hours);
                                        RedisBackend.Instance.MuteCharacter(character.ID, chr.ID, hours);
                                        ChatPacket.SendBroadcastMessageToGMs(
                                            $"[{character.Name}] Muted character {Args[0]} for {hours} hours.",
                                            BroadcastMessageType.RedText);
                                    }
                                    return true;
                                }
                                character.Message("Usage: !hackmute <charactername> <hours>");
                                return true;
                            }
                        case "hackunmute":
                            {
                                if (Args.Count == 1)
                                {
                                    var chr = Server.Instance.GetCharacter(Args[0]);
                                    if (chr == null)
                                    {
                                        character.Message("Character " + Args[0] + " not found on this channel.");
                                    }
                                    else
                                    {
                                        chr.HacklogMuted = DateTime.MinValue;
                                        RedisBackend.Instance.UnmuteCharacter(chr.ID);
                                        ChatPacket.SendBroadcastMessageToGMs($"[{character.Name}] Unmuted character {Args[0]}",
                                            BroadcastMessageType.RedText);
                                    }
                                    return true;
                                }
                                character.Message("Usage: !hackunmute <charactername>");
                                return true;
                            }

#endregion

#region MoveTrace

                        case "movetracepet":
                        case "movetraceplayer":
                        case "movetracemob":
                        case "movetracesummon":
                            {
                                MovePath.MovementSource source = 0;
                                switch (Args.Command.Replace("movetrace", ""))
                                {
                                    case "pet":
                                        source = MovePath.MovementSource.Pet;
                                        break;
                                    case "player":
                                        source = MovePath.MovementSource.Player;
                                        break;
                                    case "mob":
                                        source = MovePath.MovementSource.Mob;
                                        break;
                                    case "summon":
                                        source = MovePath.MovementSource.Summon;
                                        break;
                                }

                                if (Args.Count == 3 && Args[2].IsNumber())
                                {
                                    switch (GetUserIDFromArgs(Args[0], Args[1], out int userId))
                                    {
                                        case UserIdFetchResult.UnknownType: break; // Fallthrough
                                        case UserIdFetchResult.IDNotFound:
                                            character.Message("User with char id " + Args[1] + " does not exist");
                                            return true;
                                        case UserIdFetchResult.PlayerNotFound:
                                            character.Message("Player " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.UserNotFound:
                                            character.Message("User " + Args[1] + " does not exist.");
                                            return true;
                                        case UserIdFetchResult.Found:
                                            var amount = Math.Min(Args[2].GetInt32(), 10);

                                            var localPlayers = Server.Instance.CharacterList
                                                .Where(x => x.Value.UserID == userId).ToArray();
                                            if (localPlayers.Length > 0)
                                            {
                                                localPlayers.ForEach(x =>
                                                {
                                                    x.Value.MoveTraceCount = amount;
                                                    x.Value.MoveTraceSource = source;
                                                });
                                                character.Message(
                                                    $"Tracing player type {source} amount {amount}!");
                                            }
                                            return true;
                                    }
                                }
                                character.Message(
                                    "Usage: !movetrace(pet|player|mob|summon) <userid/charname/charid> <value> <amount>");
                                return true;
                            }

#endregion

#region Warn

                        case "w":
                        case "warn":
                            {
                                if (Args.Count >= 2)
                                {
                                    var charname = Args[0];
                                    var victim = Server.Instance.GetCharacter(charname);
                                    if (victim != null)
                                    {
                                        AdminPacket.SentWarning(character, true);
                                        ChatPacket.SendAdminWarning(victim, string.Join(" ", Args.Args.Skip(1)));
                                    }
                                    else
                                    {
                                        AdminPacket.SentWarning(character, false);
                                    }
                                    return true;
                                }
                                character.Message("Usage: !warn charname <text...>");
                                return true;
                            }

                        case "wm":
                        case "warnmap":
                            {
                                if (Args.Count >= 1)
                                {
                                    AdminPacket.SentWarning(character, true);
                                    ChatPacket.SendAdminWarning(character.Field, string.Join(" ", Args.Args));

                                    return true;
                                }
                                character.Message("Usage: !warnmap <text...>");
                                return true;
                            }

#endregion

#region MaxSkills

                        case "maxskills":
                            {
                                var mMaxedSkills = new Dictionary<int, byte>();
                                foreach (var kvp in GameDataProvider.Skills)
                                {
                                    var level = kvp.Value.MaxLevel;
                                    character.Skills.SetSkillPoint(kvp.Key, level, false);
                                    mMaxedSkills.Add(kvp.Key, level);
                                }
                                SkillPacket.SendSetSkillPoints(character, mMaxedSkills); // 1 packet for all skills
                                mMaxedSkills.Clear();
                                return true;
                            }

#endregion

#region Job

                        case "job":
                            {
                                if (Args.Count < 1 || !short.TryParse(Args[0], out short jobID))
                                {
                                    character.Message(GetUsage(Args));
                                }
                                else if (jobID != 0 && !GameDataProvider.HasJob(jobID))
                                {
                                    character.Message("Invalid job!");
                                }
                                else
                                {
                                    character.SetJob(jobID);
                                }
                                return true;
                            }

#endregion

#region MP/MP
                    case "mp":
                    {
                        if (Args.Count > 0 && short.TryParse(Args[0], out short mp))
                            character.SetMP(mp);
                        return true;
                    }
                    case "maxmp":
                    {
                        if (Args.Count > 0 && short.TryParse(Args[0], out short mp))
                            character.SetMPAndMaxMP(mp);
                        return true;
                    }
                    case "hp":
                    {
                        if (Args.Count > 0 && short.TryParse(Args[0], out short hp))
                        {
                            character.SetHP(hp);
                        }
                        return true;
                    }
                    case "maxhp":
                    {
                        if (Args.Count > 0 && short.TryParse(Args[0], out short hp))
                        {
                            character.SetHPAndMaxHP(hp);
                        }
                        return true;
                    }
#endregion

#region Str

                        case "str":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.SetStr(Args[0].GetInt16());
                                return true;
                            }

#endregion

#region Dex

                        case "dex":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.SetDex(Args[0].GetInt16());
                                return true;
                            }

#endregion

#region Int

                        case "int":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.SetInt(Args[0].GetInt16());
                                return true;
                            }

#endregion

#region Luk

                        case "luk":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.SetLuk(Args[0].GetInt16());
                                return true;
                            }

#endregion

#region AP

                        case "ap":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.SetAP(Args[0].GetInt16());
                                return true;
                            }

#endregion

#region SP

                        case "sp":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.SetSP(Args[0].GetInt16());
                                return true;
                            }

#endregion

#region AddSP

                        case "addsp":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.AddSP(Args[0].GetInt16());
                                return true;
                            }

#endregion

#region level/lvl

                        case "level":
                        case "lvl":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.SetLevel(Args[0].GetByte());
                                return true;
                            }

#endregion

                    #region Fame
                    case "fame":
                        {
                            if (short.TryParse(Args[0], out short fame))
                            {
                                character.SetFame(fame);
                            }
                            return true;
                        }
                    #endregion

#region MaxSlots

                        case "maxslots":
                            {
                                character.Inventory.SetInventorySlots(InventoryType.Equip, 100);
                                character.Inventory.SetInventorySlots(InventoryType.Use, 100);
                                character.Inventory.SetInventorySlots(InventoryType.Setup, 100);
                                character.Inventory.SetInventorySlots(InventoryType.Etc, 100);
                                character.Inventory.SetInventorySlots(InventoryType.Cash, 100);
                                return true;
                            }
                        case "setslots":
                            {
                                if (Args.Count < 2 || !byte.TryParse(Args[0], out byte inventory) || !byte.TryParse(Args[1], out byte slots))
                                {
                                    character.Message(GetUsage(Args));
                                }
                                else if (!System.Enum.IsDefined(typeof(InventoryType), inventory))
                                {
                                    character.Message($"Invalid inventory type {inventory}!");
                                }
                                else
                                {
                                    var inventoryType = (InventoryType)inventory;
                                    character.Inventory.SetInventorySlots(inventoryType, slots);
                                }
                                return true;
                            }

#endregion

#region MaxStats

                        case "maxstats":
                            {
                                character.SetHPAndMaxHP(30000);
                                character.SetMPAndMaxMP(30000);
                                character.SetLuk(30000);
                                character.SetStr(30000);
                                character.SetInt(30000);
                                character.SetDex(30000);
                                character.SetAP(0);
                                character.SetSP(2000);
                                return true;
                            }

#endregion

#region Pos

                        case "pos":
                        case "pos1": //prevent client limitation when spamming this command during testing.
                        case "pos2":
                        case "pos3":
                            {
                                string ret = "Position of " + character.Name + ". X: " + character.Position.X +
                                             ". Y: " + character.Position.Y + ". Fh: " + character.Foothold + ".";
                                character.Message(ret);
                                return true;
                            }

#endregion

#region Undercover

                        case "undercover":
                            if (Args.Count == 1)
                            {
                                var undercover = Args[0].GetBool();
                                RedisBackend.Instance.SetUndercover(character.ID, undercover);
                                character.Undercover = undercover;
                                character.Message(
                                    "You are now " + (undercover ? "" : "not") + " undercover.");
                                return true;
                            }

                            character.Message("Usage: !undercover <true/false>");
                            return true;

#endregion

#region reportlog/reports

                        case "reportlog":
                        case "reports":
                            {
                                character.Message("These are the last (at most) 15 reports: ");
                                ReportManager.GetAbuseReports()
                                    .ForEach(r => character.Message(r.ToString()));
                                return true;
                            }

#endregion

#region whowashere

                        case "whowashere":
                            {
                                const int MaxAmount = 10;
                                character.Message(
                                    "These are the last (at most) " + MaxAmount + " players that entered the map:");
                                var lastPlayers = character.Field.PlayersThatHaveBeenHere.ToList();
                                lastPlayers.Sort((x, y) => (int)(y.Value - x.Value));

                                var str = string.Join(", ", lastPlayers.Take(MaxAmount).Select(x =>
                                {
                                    long secondsAgo = (MasterThread.CurrentTime - x.Value) / 1000;
                                    return x.Key + " (" + secondsAgo + "s ago)";
                                }));

                                character.Message(str);

                                return true;
                            }

#endregion

#region Cash Shop
                        case "cash":
                        case "nxcash":
                        case "givecash":
                            {
                                try
                                {
                                    if (Args.Count < 2) throw new ArgumentException("Usage: /givecash <player> <amount>");
                                    string victimName = Args[0].Value.ToLower();
                                    GameCharacter victim = Server.Instance.GetCharacter(victimName);
                                    if (victim == null) throw new ArgumentException($"Player \"{victimName}\" not found.");
                                    if (!int.TryParse(Args[1].Value, out int amount)) throw new ArgumentException($"Invalid amount \"{Args[1].Value}\".");
                                    Common.Packets.CashPacket.AddTransactions(Server.Instance.CharacterDatabase, new List<ShopTransaction> {
                                        new ShopTransaction {
                                            userid = victim.UserID,
                                            type = TransactionType.NX,
                                            note = "",
                                            amount = -amount
                                        }
                                    });
                                    victim.Message($"You have been gifted {amount.Culture()} NX cash.");
                                }
                                catch(Exception e)
                                {
                                    character.Message(e.Message);
                                }
                                return true;
                            }
#endregion
                    }
                }

                if (character.GMLevel >= GMLevel.GM) //Full GMs
                {
                    switch (Args.Command.ToLowerInvariant())
                    {
#region Create / Item

                        case "create":
                        case "item":
                            {
                                try
                                {
                                    if (Args.Count > 0 && Args[0].IsNumber())
                                    {
                                        short Amount = 1;
                                        int ItemID = Args[0].GetInt32();
                                        byte Inv = (byte)(ItemID / 1000000);

                                        if (Inv <= 0 || Inv > 5 ||
                                            (!GameDataProvider.Equips.ContainsKey(ItemID) &&
                                             !GameDataProvider.Items.ContainsKey(ItemID) &&
                                             !GameDataProvider.Pets.ContainsKey(ItemID)))
                                        {
                                            character.Message("Item not found :(");
                                            return true;
                                        }

                                        var FreeSlots = character.Inventory.ItemAmountAvailable(ItemID);
                                        if (Args.Count >= 2)
                                        {
                                            if (Args[1] == "max" || Args[1] == "fill" || Args[1] == "full")
                                                Amount = (short)(FreeSlots > short.MaxValue
                                                    ? short.MaxValue
                                                    : FreeSlots);
                                            else if (Args[1].IsNumber())
                                            {
                                                Amount = Args[1].GetInt16();
                                                if (Amount > FreeSlots)
                                                    Amount = (short)(FreeSlots > short.MaxValue
                                                        ? short.MaxValue
                                                        : FreeSlots);
                                            }
                                        }

                                        if (Amount == 0)
                                        {
                                            DropPacket.CannotLoot(character, CannotLootDropReason.YouCantGetAnymoreItems);
                                            InventoryOperationPacket.NoChange(character);
                                        }
                                        else
                                        {
                                            character.Inventory.AddNewItem(ItemID, Amount);
                                            character.SendPacket(MessagePacket.DropPickup(false, ItemID, Amount));
                                        }
                                    }
                                    else
                                        character.Message($"Command syntax: !{Args.Command} [itemid] {{amount}}");
                                    return true;
                                }
                                catch (Exception ex)
                                {
                                    character.Message($"Command syntax: !{Args.Command} [itemid] {{amount}}");
                                    return true;
                                }
                            }
                    case "itempack":
                        {
                            if (ItemPacks.TryGetValue(Args[0], out var items))
                            {
                                character.Inventory.MassExchange(0, items);
                            } else character.Message("Unknown item pack \"" + Args[0] + "\"");
                            return true;
                        }
#endregion

#region Summon / Spawn

                        case "summon":
                        case "spawn":
                        case "mob":
                            {
                                if (Args.Count > 0)
                                {
                                    var Amount = 1;
                                    var MobID = -1;
                                    SummonType summonType = SummonType.Instant;
                                    int summonOption = 0;

                                    if (Args[0].IsNumber())
                                        MobID = Args[0].GetInt32();

                                    if (Args.Count > 1 && Args[1].IsNumber())
                                        Amount = Args[1].GetInt32();

                                    if (Args.Count > 2 && sbyte.TryParse(Args[2], out sbyte soption))
                                        summonType = (SummonType)soption;
                                    if (Args.Count > 3 && int.TryParse(Args[3], out summonOption)){ }

                                    Amount = character.IsAdmin ? Amount : Math.Min(Amount, 100);

                                    if (GameDataProvider.Mobs.ContainsKey(MobID))
                                    {
                                        for (int i = 0; i < Amount; i++)
                                        {
                                            character.Field.SpawnMobWithoutRespawning(MobID, character.Position,
                                                character.Foothold, null, summonType, summonOption);
                                        }
                                    }
                                    else
                                    {
                                        character.Message("Mob not found.");
                                    }
                                }
                                return true;
                            }
                    case "summonpos":
                    case "summonxy":
                    case "spawnpos":
                    case "spawnxy":
                        {
                            if (Args.Count < 3) character.Notice("Usage: /spawnxy <mobid> <x> <y> [fh] [summonType] [summonOption]");
                            else if (!int.TryParse(Args[0], out int mobID) || !GameDataProvider.Mobs.ContainsKey(mobID))
                                character.Notice("Invalid mob id " + Args[0]);
                            else if (!short.TryParse(Args[1], out short x) || !short.TryParse(Args[2], out short y))
                                character.Notice("Invalid pos " + Args[1] + "," + Args[2]);
                            else
                            {
                                var pos = new Pos(x, y);
                                short fh;
                                int summonOption;
                                SummonType st = SummonType.Instant;
                                if (Args.Count < 4 || !short.TryParse(Args[3], out fh)) fh = (short)(character.Field.GetFootholdUnderneath(pos.X, pos.Y, out int maxY)?.ID ?? 0);
                                if (Args.Count >= 5 && sbyte.TryParse(Args[4], out sbyte _st)) st = (SummonType)_st;
                                if (Args.Count < 6 || !int.TryParse(Args[5], out summonOption)) summonOption = 0;
                                character.Field.SpawnMobWithoutRespawning(mobID, pos, fh, summonType: st, summonOption: summonOption);
                            }
                            return true;
                        }
                    #endregion

                    #region FieldSet
                    case "fieldset":
                        {
                            if (Args.Count < 3 || (Args[1] != "minmembers" && Args[1] != "maxmembers"))
                            {
                                character.Message(GetUsage(Args));
                            }
                            else if (!FieldSet.Instances.TryGetValue(Args[0], out var fs))
                            {
                                character.Message($"Unknown field set '{Args[0]}'!");
                            }
                            else if (!int.TryParse(Args[2], out int value))
                            {
                                character.Message($"Invalid value!");
                            }
                            else
                            {
                                if (Args[1] == "minmembers") fs.Data.MinMembers = value;
                                else if (Args[1] == "maxmembers") fs.Data.MaxMembers = value;
                                character.Message($"Set '{Args[1]}' to {value} in fieldset '{fs.Data.Name}'.");
                            }

                            return true;
                        }
                    case "fieldsetvar":
                        {
                            if (Args.Count < 3)
                            {
                                character.Message("Usage: /fieldsetvar <fieldsetname> <key> <value>");
                                return true;
                            }
                            var set = FieldSet.Instances[Args[0]];
                            string key = Args[1];
                            var value = string.Join(" ", Args.Args.Skip(2));
                            set.SetVar(key, value);
                            return true;
                    }
                    case "removefieldsetvar":
                        {
                            if (Args.Count < 2)
                            {
                                character.Message("Usage: /removefieldsetvar <fieldsetname> <key>");
                                return true;
                            }
                            var set = FieldSet.Instances[Args[0]];
                            string key = Args[1];
                            set.SetVar(key, "");
                            return true;
                        }
                    #endregion

                    #region GetID

                    case "getid":
                            {
                                if (Args.Count > 0)
                                {
                                    string name = Args[0].Value.ToLower();
                                    Server.Instance.CharacterDatabase.RunQuery(
                                        "SELECT * FROM characters WHERE name = @name", "@name", name);
                                    MySqlDataReader data = Server.Instance.CharacterDatabase.Reader;
                                    data.Read();
                                    int id = data.GetInt32("ID");
                                    character.Message("ID is " + id + ".");
                                }
                                return true;
                            }

#endregion

#region d / delete

                        case "d":
                        case "delete":
                            {
                                if (Args.Count == 1 && Args[0].IsNumber() && Enum.TryParse(Args[0].GetByte().ToString(), out InventoryType inv))
                                {
                                    // Find first item to delete
                                    var slot = character.Inventory.DeleteFirstItemInInventory(inv);
                                    if (slot != 0)
                                    {
                                        InventoryOperationPacket.SwitchSlots(character, inv, slot, 0);
                                    }
                                    else
                                    {
                                        character.Message("No item to delete found.");
                                    }
                                    return true;
                                }
                                character.Message("Usage: !delete <inventory, 1=equip, 2=use, etc>");
                                return true;
                            }

#endregion

#region ClearDrops

                        case "cleardrops":
                            {
                                character.Field.DropPool.Clear();
                                return true;
                            }

#endregion

#region KillMobs / KillAll

                        case "killmobs":
                        case "killall":
                            {
                                byte how = 1;
                                if (Args.Count > 0 && !byte.TryParse(Args[0], out how))
                                {
                                    character.Message("Invalid how " + how);
                                    return true;
                                }
                                int amount = character.Field.KillAllMobs(0, how);
                                character.Message("Amount of mobs killed: " + amount.ToString());
                                return true;
                            }

#endregion

#region KillMobsDMG

                        case "killalldmg":
                        case "killmobsdmg":
                            {
                                int dmg = Args.Count == 0 ? 0 : Args[0].GetInt32();
                                int amount = character.Field.KillAllMobs(dmg);
                                character.Message("Amount of mobs killed: " + amount.ToString());
                                return true;
                            }
                        case "dmgmobs":
                        case "dmgall":
                        {
                                int dmg = Args.Count == 0 ? 0 : Args[0].GetInt32();
                                int amount = character.Field.DamageAllMobs(character, dmg);
                                character.Message("Amount of mobs damaged: " + amount.ToString());
                                return true;
                        }
                    case "dmgmob":
                    case "dmg":
                    case "damage":
                        {
                            if (Args.Count < 2)
                            {
                                character.Message("Usage: /dmgmob <mobid> <dmg>");
                                return true;
                            }
                            if (!int.TryParse(Args[0], out int mobid) || !character.Field.Mobs.Select(m => m.Value).TryFind(m => m.MobID == mobid, out Mob mob))
                            {
                                character.Message("Mob " + Args[0] + " not found in current field");
                                return true;
                            }
                            if (!int.TryParse(Args[1], out int dmg))
                            {
                                character.Message("Invalid dmg " + Args[1]);
                                return true;
                            }
                            MobPacket.SendMobDamageOrHeal(character.Field, mob, dmg, false, false);
                            if (!mob.GiveDamage(character, dmg))
                            {
                                mob.ForceDead();
                            }
                            return true;
                        }
                    #endregion

                    #region Boss HP Bar
                    case "bosshpbar":
                    case "hpbar":
                    {
                            ///packet 3F 05 00 47 86 00 E1 8D ED 03 80 14 EF 03 01 03 }
                            if (Args.Count < 5)
                            {
                                character.Message("Usage: /hpbar <mobid> <hp> <maxhp> <colorBottom> <colorTop>");
                                return true;
                            }
                            if (!int.TryParse(Args[0], out int mobid))
                            {
                                character.Message("Invalid mobid " + mobid);
                                return true;
                            }
                            if (!int.TryParse(Args[1], out int hp))
                            {
                                character.Message("Invalid hp " + hp);
                                return true;
                            }
                            if (!int.TryParse(Args[2], out int mhp))
                            {
                                character.Message("Invalid mhp " + mhp);
                                return true;
                            }
                            if (!byte.TryParse(Args[3], out byte colorBottom))
                            {
                                character.Message("Invalid colorBottom " + colorBottom);
                                return true;
                            }
                            if (!byte.TryParse(Args[4], out byte colorTop))
                            {
                                character.Message("Invalid colorTop " + colorTop);
                                return true;
                            }
                            character.Field.SendPacket(FieldEffectPacket.BossHPBar(mobid, hp, mhp, colorBottom, colorTop));
                            return true;
                    }
                    #endregion

                    #region MapNotice

                    case "mapnotice":
                            {
                                if (Args.Count > 0)
                                {
                                    ChatPacket.SendBroadcastMessageToMap(character.Field, $"[{character.Name}] : {Args.CommandText}", BroadcastMessageType.PopupBox);
                                }
                                return true;
                            }

#endregion

#region ditto/datto

                        case "ditto":
                            {
                                if (Args.Count == 1)
                                {
                                    int charid = Server.Instance.CharacterDatabase.CharacterIdByName(Args[0]);
                                    if (charid == -1)
                                    {
                                        if (int.TryParse(Args[0], out charid) == false)
                                        {
                                            character.Message("Character " + Args[0] + " not found??");
                                            return true;
                                        }
                                    }

                                    RedisBackend.Instance.SetImitateID(character.ID, charid);
                                    ChatPacket.SendBroadcastMessageToGMs($"[{character.Name}] Imitating character {Args[0]}.",
                                        BroadcastMessageType.RedText);
                                    // CC
                                    character.Player.Socket.DoChangeChannelReq(Server.Instance.ID);
                                    return true;
                                }
                                character.Message("Usage: !ditto <charactername or id>");
                                return true;
                            }
                        case "datto":
                            {
                                RedisBackend.Instance.SetImitateID(character.ID, 0);
                                ChatPacket.SendBroadcastMessageToGMs(
                                    $"[{character.ImitatorName}] Stopped imitating {character.Name}. Glad to have you back",
                                    BroadcastMessageType.RedText);
                                return true;
                            }

#endregion

#region Notice

                        case "notice":
                            {
                                if (Args.Count > 0)
                                {
                                    ChatPacket.SendBroadcastMessageToChannel(Args.CommandText, BroadcastMessageType.Notice);
                                }
                                return true;
                            }

#endregion

#region SetSP

                        case "setsp":
                            {
                                if (Args.Count > 1 && Args[0].IsNumber())
                                {
                                    int SkillID = Args[0].GetInt32();
                                    byte Level = 1;
                                    byte MaxLevel = (byte)(GameDataProvider.Skills.TryGetValue(SkillID, out var sd)
                                        ? sd.MaxLevel
                                        : 0);

                                    if (MaxLevel > 0)
                                    {
                                        if (Args[1] == "max")
                                            Level = MaxLevel;
                                        else if (Args[1].IsNumber())
                                            Level = Args[1].GetByte();
                                        else
                                            Level = 1;

                                        character.Skills.SetSkillPoint(SkillID, Level);
                                    }
                                    else
                                        character.Message("Skill not found.");
                                }
                                return true;
                            }

#endregion

#region Heal

                        case "heal":
                            {
                                int hpHealed = character.PrimaryStats.GetMaxHP(false) - character.HP;
                                character.ModifyHP(character.PrimaryStats.GetMaxHP(false));
                                character.ModifyMP(character.PrimaryStats.GetMaxMP(false));
                                // CharacterStatsPacket.SendCharacterDamage(character, 0, -hpHealed, 0, 0, 0, 0, null);
                                return true;
                            }
                    case "resurrect":
                    case "revive":
                        {
                            if (character.Field.Characters.TryFind(i => i.Name.ToLower() == Args[0].ToString().ToLower(), out var victim) && victim.HP == 0)
                            {
                                PlayerEffectPacket.SendSkill(victim, Constants.Gm.Skills.Resurrection, 1, skillOnOther: true);
                                victim.ModifyHP(victim.PrimaryStats.GetMaxHP(false), true);
                            }
                            else character.Message($"Dead player \"{Args[0]}\" not found.");
                            return true;
                        }

#endregion

#region AP

                        case "ap":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.SetAP(Args[0].GetInt16());
                                return true;
                            }

#endregion

#region SP

                        case "sp":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.SetSP(Args[0].GetInt16());
                                return true;
                            }

#endregion

#region AddSP

                        case "addsp":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.AddSP(Args[0].GetInt16());
                                return true;
                            }

#endregion

#region GiveEXP

                        case "exp":
                        case "giveexp":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.AddEXP(Args[0].GetInt32(), MessageAppearType.SideWhite);
                                return true;
                            }

#endregion

#region Mesos

                        case "mesos":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    character.SetMesos(Args[0].GetInt32());
                                return true;
                            }

#endregion

#region pton/ptoff

                        case "pton":
                        case "ptoff":
                            {
                                if (Args.Count > 0)
                                {
                                    if (!character.Field.Portals.TryGetValue(Args[0], out var pt))
                                    {
                                        character.Message("Portal not found.");
                                    }
                                    else
                                    {
                                        var enabled = pt.Enabled = Args.Command.ToLowerInvariant() == "pton";
                                        character.Message("Portal " + Args[0] + " is now " + (enabled ? "enabled" : "disabled"));
                                    }
                                }

                                return true;
                            }

#endregion

#region portals

                        case "portals":
                            {
                                var portalsInRange = character.Field.Portals.Values
                                    .OrderBy(x => new Pos(x.X, x.Y) - character.Position).Take(3).ToArray();
                                if (portalsInRange.Length == 0)
                                {
                                    character.Message("No portals found.");
                                }
                                else
                                {
                                    foreach (var portal in portalsInRange)
                                    {
                                        character.Message($"Portal '{portal.Name}' id {portal.ID} script '{portal.Script}' enabled {portal.Enabled} Distance {new Pos(portal.X, portal.Y) - character.Position} ToMap {portal.ToMapID} ToName {portal.ToName} Type {portal.Type}");
                                    }
                                }
                                return true;
                            }

#endregion

#region Event

                        case "event":
                        case "events":
                        case "eventhelp":
                            {
                                List<string> HelpMessages = new List<string>()
                            {
                                "============= GM Event Help Guide =============",
                                "Each event has its own help guide that can be brought up via command, and a lobby map.",
                                "In any event lobby map, use !eventdesc to display an event description message to all players.",
                                " ",
                                "AVAILABLE EVENTS:",
                                "Find the Jewel. Help: /ftjhelp Map: /map jewel",
                                "Snowball. Help: /snowballhelp Map: /map snowball for event map, /map 109060001 for lobby",
                                "Fitness. Help: /fitnesshelp Map: /map fitness",
                                "Quiz. Help: /quizhelp Map: /map quiz"
                            };

                                HelpMessages.ForEach(m => character.Message(m));
                                return true;
                            }
                    case "eventdesc":
                        MapPacket.SendGMEventInstructions(character.Field);
                        character.Message("Sent event description to everybody");
                        return true;
                    case "eventstart":
                    case "startevent":
                        {
                            if (character.Field is EventMap eventMap)
                            {
                                if (eventMap.Started)
                                {
                                    character.Notice("Event already started!", BroadcastMessageType.Notice);
                                    return true;
                                }
                                else eventMap.Start();
                            }
                            else if (character.Field.FieldSet != null)
                            {
                                var fs = character.Field.FieldSet;
                                if (fs.Opened)
                                {
                                    character.Notice("Event already started!", BroadcastMessageType.Notice);
                                    return true;
                                }
                                else
                                {
                                    character.Field.FieldSet?.Start(character);
                                }
                            }
                            else
                            {
                                character.Notice("No event found in this map!", BroadcastMessageType.Notice);
                                return true;
                            }
                            return true;
                        }
                    case "eventstop":
                    case "stopevent":
                    case "endevent":
                    case "eventend":
                        {
                            ChatPacket.SendBroadcastMessageToGMs($"Event stopped early. Kicking everyone if event was in progress...");
                            if (character.Field is EventMap eventMap) eventMap.End();
                            else character.Field.FieldSet?.End();
                            return true;
                        }
                    case "resetcoconuts":
                        {
                            if (character.Field is Map_Coconut map) map.ResetCoconuts();
                            else character.Message("Invalid map!");
                            return true;
                        }
                    #endregion

                    #region Find The Jewel

                    case "ftjhelp":
                            {
                                List<string> HelpMessages = new List<string>()
                            {
                                "============= Find The Jewel GM Help Guide =============",
                                "Treasure scroll item ID: 4031018. Devil Scroll: 4031019. Entry map is 109010000. Other maps are 109010100-3, 109010200-3, 109010300-3, 109010400-3.",
                                "Use '/spawn <mobid>' to create mobs at your location. Mob IDs: Super slime - 9100000, Super Jr. Necki - 9100001, Super Stirge - 9100002.",
                                "Spawn reactors by moving and using '/ftjreactorhere <id> <jewel>', where <id> is the reactor id, and <jewel> indicates if it contains the treasure: 1 for treasure, 0 for nothing",
                                "Make sure id does not exceed map limit, or client will crash. Example: /ftjreactorhere 1 1 places a reactor with id 1 and contains the treasure",
                                "Big maps will have 20 reactors (rid's 0 - 19) and small maps have 2 (rid's 0 - 1). Going past these limits will crash everything and require a server reboot",
                                "Use /ftjenable to allow players to enter the entry map (via NPC Billy) before the event starts.",
                                "Use /ftjstart to enable the portals, disably entry, and start the event. It will stop automatically when the timer runs out. Stop early with /ftjstop.",
                                "Tip: From here, the viking NPCs will take care of the rest. It may be worth going into hide and going into maps in case some try to cheat/hack.",
                                "Tip: Use AdminFly to get to platforms for mob and reactor placement, BUT, make sure to turn it off and land on the platform before placing things.",
                                "Tip: For the most authentic experience, stirges only go in the kerning map, while slimes and necki go in the others. Stirge in hene maps looks weird.",
                                "Tip: It is recommended (and GMS-like) to run the event with more than 1 hidden jewel. Put a handful, like 5-15."
                            };

                                HelpMessages.ForEach(m => character.Message(m));
                                return true;
                            }
                        //case "ftjreactorhere":
                        //    {
                        //        if (Args.Count < 2)
                        //        {
                        //            ChatPacket.SendNotice(
                        //                "Usage: !ftjreactorhere <reactor id> <jewel>, <jewel> = 0 for no treasure or 1 for treasure",
                        //                character);
                        //        }
                        //        else
                        //        {
                        //            int maxFTJReactors()
                        //            {
                        //                switch (character.MapID)
                        //                {
                        //                    case 109010100:
                        //                    case 109010200:
                        //                    case 109010300:
                        //                    case 109010400:
                        //                        return 20;
                        //                    case 109010101:
                        //                    case 109010102:
                        //                    case 109010103:
                        //                    case 109010201:
                        //                    case 109010202:
                        //                    case 109010203:
                        //                    case 109010301:
                        //                    case 109010302:
                        //                    case 109010303:
                        //                    case 109010401:
                        //                    case 109010402:
                        //                    case 109010403:
                        //                        return 1;
                        //                    default:
                        //                        return -1;
                        //                }
                        //            }

                        //            int rid = short.Parse(Args[0]);
                        //            if (rid > maxFTJReactors() || rid < 0)
                        //            {
                        //                ChatPacket.SendNotice(
                        //                    "Exceeded max reactor limit for this map!!! Did not place.", character);
                        //                return true;
                        //            }

                        //            Reactor r = new Reactor(character.Field, (short)rid, 0, character.Position.X,
                        //                character.Position.Y, 0, 0);

                        //            if (int.Parse(Args[1]) == 1)
                        //            {
                        //                r.ItemDrops.Add((4031018, 1));
                        //            }

                        //            character.Field.AddReactor(r);
                        //        }
                        //        return true;
                        //    }

#endregion

#region Snowball

                        case "snowballhelp":
                            {
                                List<string> HelpMessages = new List<string>()
                            {
                                "============= Snowball Event GM Help Guide =============",
                                "1. Use /snowballenable to allow entry to the hub map via event NPCs.",
                                "2. Use /snowballstart to begin the event. Teams are automatically assigned and warped to the right spots",
                                "3. To stop early, use !snowballstop. Otherwise, the event will stop and determine a winner",
                                "if the timer runs out or if a team reaches the finish line.",
                                "4. After the event ends, there will be a 10 second delay for players to be warped to win/lose maps before",
                                "another event can be started."
                            };

                                HelpMessages.ForEach(m => character.Message(m));
                                return true;
                            }
#endregion

#region Fitness

                        case "fitnesshelp":
                        case "fithelp":
                            {
                                List<string> HelpMessages = new List<string>()
                            {
                                "============= Fitness Event GM Help Guide =============",
                                "1. Use /fitenable or /fitnessenable to allow entry to the hub map via event NPCs.",
                                "2. Use /fitstart or /fitnessstart to begin the event. Characters are warped to the starting spot automatically",
                                "3. To stop early, use /fitstop or /fitnessstop. Otherwise, the event will run until",
                                "the timer runs out. All who make it past stage 4 are automatically taken to the victory map by the portal."
                            };

                                HelpMessages.ForEach(m => character.Message(m));
                                return true;
                            }
#endregion

#region Quiz

                        case "quizhelp":
                            {
                                List<string> HelpMessages = new List<string>()
                            {
                                "============= Quiz Event GM Help Guide =============",
                                "1. Use /quizenable to allow entry to the hub map via event NPCs.",
                                "2. Use /quizstart to begin the event. Characters are warped to the starting spot automatically, and 10 questions are asked automatically.",
                                "3. To stop early, use /quizstop. Otherwise, the event will run until all questions have been asked."
                            };

                                HelpMessages.ForEach(m => character.Message(m));
                                return true;
                            }
#endregion
                    }
                }

                if (character.IsAdmin) //Admin
                {
                    switch (Args.Command.ToLowerInvariant())
                    {
#region Shutdown

                        case "shutdown":
                            {
                                if (!shuttingDown)
                                {
                                    int len = 10;
                                    if (Args.Count > 0 && Args[0].IsNumber())
                                    {
                                        len = Args[0].GetInt32();
                                        if (len == 0)
                                            len = 10;
                                    }

                                    character.Message(string.Format("Shutting down in {0} seconds", len));

                                    MasterThread.RepeatingAction.Start("Shutdown Thread",
                                        (a) => { Environment.Exit(9001); }, (long)len * 1000, 0);
                                    shuttingDown = true;
                                    return true;
                                }
                                else
                                {
                                    character.Message("Unable to shutdown now!");
                                }
                                return true;
                            }

#endregion

#region Clock

                        case "clock":
                            {
                                if (Args.Count > 0 && Args[0].IsNumber())
                                    MapPacket.ShowMapTimerForMap(character.Field, Args[0].GetInt32());
                                return true;
                            }

#endregion

#region Header

                        case "header":
                            {
                                var txt = Args.Count == 0 ? "" : Args.CommandText;
                                Server.Instance.SetScrollingHeader(txt);
                                return true;
                            }
                        case "headernotice":
                            {
                                if (Args.Count == 0)
                                {
                                    var txt = Args.CommandText;
                                    Server.Instance.SetScrollingHeader(txt);
                                    ChatPacket.SendBroadcastMessageToChannel(txt, BroadcastMessageType.Notice);
                                }
                                return true;
                            }

#endregion

#region Packet

                        case "beginpacket":
                            {
                                pendingPackets[character] = new Packet();
                                character.Message("Packet initiated. Use /packet to write your packet.");
                                return true;
                            }
                        case "endpacket":
                            {
                                if (!pendingPackets.TryGetValue(character, out Packet packet))
                                {
                                    character.Message("You haven't started a packet! Use /beginpacket to initiate a packet.");
                                    return true;
                                }
                                character.SendPacket(packet);
                                pendingPackets.Remove(character);
                                return true;
                            }
                        case "packet":
                            {
                                if (Args.Count > 0)
                                {
                                    bool pending = pendingPackets.TryGetValue(character, out Packet pw);
                                    if (!pending)
                                    {
                                        pw = new Packet();
                                    }
                                    pw.WriteHexString(Args.CommandText);
                                    if (!pending) character.SendPacket(pw);
                                    else character.Message("Packet: " + pw + ". Use /endpacket to send.");
                                }
                                return true;
                            }
                        case "typedpacket":
                            {
                                if (Args.Count % 2 != 0 || Args.Count == 0)
                                {
                                    character.Message("Usage: /packet <type> <value> <type> <value> ... where type is int, short, long, string, byte");
                                    return true;
                                }

                                Packet pw = new Packet();

                                for (int i = 0; i < Args.Count; i += 2)
                                {
                                    switch (Args[i].Value.ToLowerInvariant())
                                    {
                                        case "opcode":
                                        case "op":
                                        case "byte":
                                            pw.WriteByte(Args[i + 1].GetByte());
                                            break;
                                        case "short":
                                            pw.WriteShort(Args[i + 1].GetInt16());
                                            break;
                                        case "int":
                                            pw.WriteInt(Args[i + 1].GetInt32());
                                            break;
                                        case "long":
                                            pw.WriteLong(Args[i + 1].GetInt64());
                                            break;
                                        case "string":
                                            pw.WriteString(Args[i + 1].Value);
                                            break;
                                        default:
                                            character.Message("Unknown type: " + Args[i].Value);
                                            return true;
                                    }
                                }

                                character.SendPacket(pw);
                                return true;
                            }

#endregion

#region Drop

                        case "drop":
                            {
                                try
                                {
                                    if (Args.Count > 0)
                                    {
                                        if (!Args[0].IsNumber())
                                        {
                                            character.Message("Command syntax: /drop [itemid] {amount}");
                                            return true;
                                        }

                                        short Amount = 1;
                                        var ItemID = Args[0].GetInt32();
                                        var Inv = (byte)(ItemID / 1000000);

                                        if (Inv <= 0 || Inv > 5 ||
                                            (!GameDataProvider.Equips.ContainsKey(ItemID) &&
                                             !GameDataProvider.Items.ContainsKey(ItemID) &&
                                             !GameDataProvider.Pets.ContainsKey(ItemID)))
                                        {
                                            character.Message("Item not found :(");
                                            return true;
                                        }

                                        if (Args.Count > 1 && Args[1].IsNumber())
                                            Amount = Args[1].GetInt16();

                                        var dropItem = Item.CreateFromItemID(ItemID);
                                        dropItem.Amount = Amount;

                                        character.Field.DropPool.Create(Reward.Create(dropItem), character.ID, 0, DropType.Normal, 0, new Pos(character.Position), character.Position.X, 0, false, true);
                                    }
                                    return true;
                                }
                                catch
                                {
                                    character.Message("Item not found :(");
                                    return true;
                                }
                            }
                        case "droptext":
                            {
                                if (Args.Count < 2 || !Args[0].IsNumber())
                                {
                                    character.Message("Command syntax: /droptext [0=red, 1=green] your text");
                                    return true;
                                }

                                var itemidNumber = 3990000;
                                var itemidAlphabet = 3991000;
                                var posTextNumbers = "";
                                var posTextAlphabet = "";

                                switch (Args[0].GetInt32())
                                {
                                    case 0: // Red
                                        posTextNumbers = "1234567890" + // red numbers
                                                         "~~~~~~~~~~" + // green numbers
                                                         "+-" +
                                                         "~~" + // green +-
                                                         "";
                                        posTextAlphabet = "abcdefghijklmnopqrstuvwxyz" +
                                                          "~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                                                          "";
                                        break;
                                    case 1: // Green
                                        posTextNumbers = "~~~~~~~~~~" + // red numbers
                                                         "1234567890" + // green numbers
                                                         "~~" + // red numbers
                                                         "+-" + // green +-
                                                         "";
                                        posTextAlphabet = "~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                                                          "abcdefghijklmnopqrstuvwxyz" +
                                                          "";
                                        break;
                                    default:
                                        character.Message("Command syntax: /droptext [0=red, 1=green] your text");
                                        return true;
                                }

                                var Rewards = string.Join(" ", Args.Args.Skip(1).Select(x => x.Value)).Select(x =>
                                {
                                    if ((x >= '0' && x <= '9') || (x == '+' || x == '-'))
                                    {
                                        return itemidNumber + posTextNumbers.IndexOf(x);
                                    }

                                    char lowerx = char.ToLower(x);
                                    if (lowerx >= 'a' && lowerx <= 'z')
                                    {
                                        return itemidAlphabet + posTextAlphabet.IndexOf(lowerx);
                                    }
                                    return 1;
                                }).Select(x => Reward.Create(Item.CreateFromItemID(x, 1))).ToList();

                                var Pos = character.Position;

                                short Delay = 0;
                                int x2 = Pos.X + Rewards.Count * -10;
                                foreach (Reward Drop in Rewards)
                                {
                                    if (Drop.ItemID != 1 && character.Field.DropPool.Create(Drop, character.ID,
                                            int.MaxValue, DropType.Party, 0, Pos, x2, Delay, true, false))
                                        continue;
                                    Delay += 200;
                                    x2 += 35;
                                }


                                return true;
                            }

#endregion

#region TogglePortal

                        case "toggleportals":
                            {
                                character.Field.PortalsOpen = !character.Field.PortalsOpen;
                                character.Notice($"You have toggled the portals {(character.Field.PortalsOpen ? "on" : "off")}.");
                                return true;
                            }

#endregion

#region MakeDonator

                        case "makedonator":
                            {
                                if (Args.Count > 0)
                                {
                                    string name = Args[0].Value.ToLower();
                                    int derp = Server.Instance.CharacterDatabase.UserIDByCharacterName(name);
                                    if (derp > 1)
                                    {
                                        Server.Instance.CharacterDatabase.RunQuery(
                                            $"UPDATE users SET donator = 1 WHERE ID = {derp}");
                                        character.Message($"'{name} ' is now set as a donator on the AccountID : {derp}");
                                    }
                                    else if (derp <= 1)
                                        character.Message("You have entered an incorrect name.");
                                }
                                return true;
                            }

#endregion

#region Participate

                        case "participate":
                            {
                                if (Args.Count > 0)
                                {
                                    string name = Args[0].Value.ToLower();
                                    /**
                                            if (EventManager.hasParticipated(name) == true)
                                            {
                                                MessagePacket.SendText(MessagePacket.MessageTypes.RedText, "HAS PARTICIPATED DUN DUN DUN.", character, MessagePacket.MessageMode.ToPlayer);
                                            }
                                            else if (EventManager.hasParticipated(name) == false)
                                            {
                                                MessagePacket.SendText(MessagePacket.MessageTypes.RedText, "NOT PARTICIPATED DUN DUN DUNNNN.", character, MessagePacket.MessageMode.ToPlayer);
                                            }
                                             * */
                                }
                                return true;
                            }

#endregion

#region GetID2

                        case "getid2":
                            {
                                if (Args.Count > 0)
                                {
                                    string name = Args[0].Value.ToLower();
                                    int id = Server.Instance.CharacterDatabase.CharacterIdByName(name);
                                    string name2 = character.Name;
                                    character.Message($"ID is '{id}'.");
                                }
                                return true;
                            }

#endregion

#region Save

                        case "save":
                            {
                                character.Save();
                                character.Message("Saved!");
                                return true;
                            }

#endregion

#region SaveAll

                        case "saveall":
                            {
                                foreach (var kvp in Server.Instance.CharacterList)
                                {
                                    MasterThread.Instance.AddCallback(x =>
                                    {
                                        kvp.Value.Save();
                                        character.Message(kvp.Value.Name + " saved at : " + DateTime.Now + ".");
                                    }, "Saving message for " + kvp.Key);
                                }
                                return true;
                            }

#endregion

#region PetName

                        case "petname":
                            {
                                if (Args.Count > 0)
                                {
                                    string newname = Args[0].Value;
                                    if (newname.Length > 13)
                                    {
                                        character.Message("Cannot change the name! It's too long :(");
                                        return true;
                                    }
                                    var pet = character.GetSpawnedPet();
                                    if (pet == null)
                                    {
                                        character.Message("Pet not found!");
                                    }
                                    else
                                    {
                                        pet.Name = newname;
                                        character.Message("Changed name");
                                    }
                                }
                                return true;
                            }

#endregion

#region VAC

                        case "vac":
                            {
                                bool petLoot = false;
                                bool mobLoot = false;
                                if (Args.Count > 0)
                                {
                                    switch (Args[0].Value)
                                    {
                                        case "pet":
                                            petLoot = true;
                                            break;
                                        case "mob":
                                            mobLoot = true;
                                            break;
                                    }
                                }

                                var mobs = character.Field.Mobs.Values.ToList();
                                if (mobLoot && mobs.Count == 0) mobLoot = false;

                                var dropBackup = new Dictionary<int, Drop>(character.Field.DropPool.Drops);
                                foreach (var kvp in dropBackup)
                                {
                                    if (kvp.Value == null)
                                        continue;

                                    Drop drop = kvp.Value;
                                    short pickupAmount = drop.Reward.Amount;
                                    if (drop.Reward.Mesos)
                                    {
                                        character.Inventory.AddMesos(drop.Reward.Drop);
                                    }
                                    else
                                    {
                                        character.Inventory.AddItemOperations(drop.Reward.Data, out short amountLeft);
                                        if (amountLeft == drop.Reward.Amount) continue;
                                    }
                                    if (drop.Reward.Mesos) character.SendPacket(MessagePacket.DropPickup(true, drop.Reward.Drop, 0));
                                    else character.SendPacket(MessagePacket.DropPickup(false, drop.Reward.Drop, pickupAmount));
                                    if (mobLoot)
                                    {
                                        var mob = mobs[(int)(Rand32.Next() % mobs.Count)];
                                        character.Field.DropPool.RemoveDrop(drop, RewardLeaveType.Remove, mob.SpawnID);
                                    }
                                    else
                                    {
                                        character.Field.DropPool.RemoveDrop(drop,
                                            petLoot ? RewardLeaveType.PetPickup : RewardLeaveType.FreeForAll,
                                            character.ID);
                                    }
                                }
                                return true;
                            }

#endregion

#region MobInfo

                        case "mobinfo":
                            {
                                var Field = character.Field;
                                var Capacity = Field.GetCapacity();
                                var boosted = Field.IsBoostedMobGen;
                                var RemainCapacity = Capacity - Field.Mobs.Count;
                                character.Message($"Min Limit {Field.MobCapacityMin}, Max Limit {Field.MobCapacityMax}, Count {Field.Mobs.Count}");
                                character.Message($"Capacity {Capacity}, RemainCapacity {RemainCapacity}");
                                character.Message($"Boosted {boosted}, Boost trigger @ {Field.MobCapacityMin / 2} players (cur {Field.Characters.Count})");
                                return true;
                            }

#endregion

#region MobChase

                        case "mobchase":
                            {
                                if (Args.Count > 0)
                                {
                                    string victim = Args[0].Value.ToLower();
                                    GameCharacter who = Server.Instance.GetCharacter(victim);

                                    if (who != null)
                                    {
                                        who.Field.Mobs.ForEach(x => x.Value.SetController(who, true));
                                    }
                                    else
                                        character.Message("You have entered an incorrect name.");
                                }
                                return true;
                            }

#endregion

#region npcreload

                        case "scriptreload":
                        case "reloadscript":
                            {
                                if (Args.Count > 0)
                                {
                                    var scriptName = Args[0];

                                    var fileName = ScriptAccessor.GetScriptPath(scriptName);
                                    if (fileName == null)
                                    {
                                        character.Notice("Could not find a script with the name " + scriptName + "!");
                                        return true;
                                    }

                                    var toAllChannels = Args.Count > 1 && Args[1].GetBool();
                                    if (toAllChannels)
                                    {
                                        var p = new Packet(ISClientMessages.BroadcastPacketToGameServers);
                                        p.WriteByte((byte)ISServerMessages.ReloadNPCScript);
                                        p.WriteString(scriptName);
                                        Server.Instance.CenterConnection.SendPacket(p);

                                        character.Notice("Sent request to reload the script to all channels.");
                                    }
                                    else if (ScriptAccessor.GetScript(Server.Instance, scriptName, (script) => {
                                        character.Notice("Error while recompiling " + scriptName + ". See logs. Script: " + script);
                                    }, true) != null)
                                    {
                                        if (int.TryParse(scriptName, out int npcid) && GameDataProvider.NPCs.TryGetValue(npcid, out NPCData data))
                                        {
                                            data.Shop.Clear();
                                        }
                                        character.Notice("Recompiled the script.");
                                    }
                                }
                                else
                                {
                                    character.Notice($"Usage: /{Args.Command} <script name or id> (1 here for all channels)");
                                }
                                return true;
                            }
#endregion

#region reload cashshop data

                        case "csreload":
                        case "cashshopreload":
                        case "reloadcs":
                        case "reloadcashshop":
                            {
                                var p = new Packet(ISClientMessages.BroadcastPacketToShopservers);
                                p.WriteByte((byte)ISServerMessages.ReloadCashshopData);
                                Server.Instance.CenterConnection.SendPacket(p);

                                character.Message("Sent request to reload the cashshop data.");
                            }
                            return true;

#endregion

#region reload world events

                        case "reloadevents":
                        case "eventsreload":
                            {
                                var p = new Packet(ISClientMessages.ReloadEvents);
                                Server.Instance.CenterConnection.SendPacket(p);

                                character.Message("Sent request to reload events.");
                            }
                            return true;

                    #endregion

                    #region Reactors
                    case "reactorstate":
                    case "setreactorstate":
                    case "setreactor":
                        {
                            if (Args.Count<2)
                            {
                                character.Message("Usage: /reactorstate <rid> <state>");
                            }
                            else if (!int.TryParse(Args[0], out int rid))
                            {
                                character.Message("Invalid rid " + Args[0]);
                            }
                            else if (!character.Field.ReactorPool.Reactors.TryGetValue(rid, out FieldReactor reactor))
                            {
                                character.Message($"Reactor {rid} not found!");
                            }
                            else if (!byte.TryParse(Args[1], out byte state))
                            {
                                character.Message($"Invalid state {Args[1]}!");
                            }
                            else
                            {
                                reactor.ChangeState(character, state);
                            }
                            return true;
                        }
                    case "triggerreactor":
                        {
                            if (Args.Count == 0) character.Message("Usage: /triggerreractor <reactor id>");
                            else if (!int.TryParse(Args[0], out int rid) || !character.Field.ReactorPool.Reactors.TryGetValue(rid, out FieldReactor reactor))
                            {
                                character.Message($"Reactor {rid} not found.");
                                return true;
                            }
                            else if (!reactor.Shown)
                            {
                                character.Message("This reactor is not shown!");
                            }
                            else
                            {
                                reactor.Trigger(character);
                            }
                            return true;
                        }
                    case "triggerreactorbyname":
                        {
                            if (!character.Field.ReactorPool.Reactors.Values.TryFind(i => i.Name == Args[0], out FieldReactor reactor))
                            {
                                character.Message($"Reactor {Args[0]} not found.");
                                return true;
                            }
                            else if (!reactor.Shown)
                            {
                                character.Message("This reactor is not shown!");
                            }
                            else
                            {
                                reactor.Trigger();
                            }
                            return true;
                        }
                    case "reactor":
                        {
                            if (Args.Count < 3)
                            {
                                character.Notice("Usage: <int reactorId> <byte state> <bool facesLeft>");
                                return true;
                            }
                            if (!int.TryParse(Args[0], out int rid) || !GameDataProvider.Reactors.TryGetValue(rid, out Reactor reactor))
                            {
                                character.Notice("Unknown reactor id " + Args[0]);
                                return true;
                            }
                            var pos = character.Position;
                            byte id = (byte)character.Field.ReactorPool.Reactors.Count;
                            var mr = new FieldReactor(id, character.Field, reactor, byte.Parse(Args[1]), pos.X, (short)(pos.Y - 80), bool.Parse(Args[2]));
                            character.Field.ReactorPool.AddReactor(mr, true);
                            Program.MainForm.LogAppend("Added reactor with ID " + reactor.ID + " (" + mr.ID + ") on map " + character.Field.ID);
                            return true;
                        }
#endregion
                }
                }

                character.Message($"Unknown command: {text}");
                return true;
            }
            catch (Exception ex)
            {
                ////Console.WriteLine(ex.ToString());
                character.Message("Something went wrong while processing this command.");
                if (character.IsGM)
                {
                    character.Message(ex.ToString());
                }
                return true;
            }
        }




        public static void HandleAdminCommand(GameCharacter chr, Packet packet)
        {
            if (chr.AssertForHack(!chr.IsGM, "Tried to use slash GM command while not GM")) return;
            //  41 12 1E 00 00 00 
            byte opcode = packet.ReadByte();
            switch (opcode)
            {
                case 0x00: // /create (no idea what it does)
                    break;
                case 0x02:
                    {
                        // /exp (int amount) 
                        int exp = packet.ReadInt();
                        chr.AddEXP(exp, MessageAppearType.SideWhite);
                        break;
                    }
                case 0x03:
                    {
                        // /ban (user) (permanantly)
                        string name = packet.ReadString();
                        int charid = Server.Instance.CharacterDatabase.CharacterIdByName(name);
                        int ID = Server.Instance.CharacterDatabase.UserIDByCharacterName(name);
                        using (MySqlDataReader data = Server.Instance.CharacterDatabase.RunQuery("SELECT * FROM characters WHERE name = '" + MySqlHelper.EscapeString(name) + "'") as MySqlDataReader)
                        {
                            if (data.HasRows)
                            {
                                if (!Server.Instance.CharacterList.ContainsKey(charid))
                                {
                                    if (Server.Instance.CharacterList.ContainsKey(charid))
                                    {
                                        GameCharacter victim = Server.Instance.GetCharacter(name);
                                        victim.Player.Socket.Disconnect();
                                        Server.Instance.CharacterDatabase.RunQuery("UPDATE users SET ban_reason = 8 WHERE ID = " + ID); //8 : permanent ban
                                        AdminPacket.BanCharacterMessage(chr);
                                    }
                                    else
                                    {
                                        Server.Instance.CharacterDatabase.RunQuery("UPDATE users SET ban_reason = 0 WHERE ID = " + ID);
                                        AdminPacket.BanCharacterMessage(chr);
                                    }
                                }
                            }
                            else
                            {
                                AdminPacket.InvalidNameMessage(chr);
                            }
                        }
                        break;
                    }
                case 0x04:
                    {
                        string name = packet.ReadString();
                        byte type = packet.ReadByte();
                        int duration = packet.ReadInt();
                        string comment = packet.ReadString();

                        int charid = Server.Instance.CharacterDatabase.CharacterIdByName(name);
                        int ID = Server.Instance.CharacterDatabase.UserIDByCharacterName(name);
                        using (MySqlDataReader data = Server.Instance.CharacterDatabase.RunQuery("SELECT * FROM characters WHERE name = '" + MySqlHelper.EscapeString(name) + "'") as MySqlDataReader)
                        {
                            if (data.HasRows)
                            {
                                if (!Server.Instance.CharacterList.ContainsKey(charid))
                                {
                                    if (Server.Instance.CharacterList.ContainsKey(charid))
                                    {
                                        GameCharacter victim = Server.Instance.GetCharacter(name);
                                        victim.Player.Socket.Disconnect();
                                        Server.Instance.CharacterDatabase.RunQuery("UPDATE users SET ban_reason = " + type + " WHERE ID = " + ID); //8 : permanent ban
                                        AdminPacket.BanCharacterMessage(chr);
                                    }
                                    else
                                    {
                                        Server.Instance.CharacterDatabase.RunQuery("UPDATE users SET ban_reason = " + type + " WHERE ID = " + ID);
                                        AdminPacket.BanCharacterMessage(chr);
                                    }
                                }
                            }
                            else
                            {
                                AdminPacket.InvalidNameMessage(chr);
                            }
                        }
                        break;
                    }

                case 0x11: //not sure what this is supposed to do. The only thing that comes after the received string is an INT(0). the format is /send (something) (something)
                    {
                        // /send (user) (mapid)
                        string To = packet.ReadString();
                        break;
                    }
                case 0x12:
                    {
                        // /snow
                        TimeSpan time = new TimeSpan(0, packet.ReadInt(), 0);
                        chr.Field.MakeWeatherEffect(2090000, "", time, true);
                        //FileWriter.WriteLine(@"Logs\Admin Command Log.txt", string.Format("[{0}] Character {1} ({2}, UID: {3}) used admin command: /snow {4}", DateTime.Now.ToString(), chr.ID, chr.Name, chr.UserID, time.TotalMinutes));
                        break;
                    }
                case 0x0F:
                    {
                        // /hide 0/1
                        bool doHide = packet.ReadBool();
                        //if (doHide == chr.GMHideEnabled) return;
                        chr.SetGMHide(doHide, false);

                        //FileWriter.WriteLine(@"Logs\Admin Command Log.txt", string.Format("[{0}] Character {1} ({2}, UID: {3}) used admin command: /hide {4}", DateTime.Now.ToString(), chr.ID, chr.Name, chr.UserID, doHide));
                        break;
                    }
                case 0x0A:
                    {
                        // /block NAME TIME REASON
                        string name = packet.ReadString();
                        byte reason = packet.ReadByte();
                        int len = packet.ReadInt();
                        string reasonmsg = packet.ReadString();
                        break;
                    }
                default:
                    {
                        ////Console.WriteLine("Unhandled Command! Opcode: " + opcode);
                        //FileWriter.WriteLine(@"Logs\Admin Command Log.txt", string.Format("[{0}] Character {1} ({2}, UID: {3}) tried using an admin command. Packet: {4}", DateTime.Now.ToString(), chr.ID, chr.Name, chr.UserID, packet.ToString()));
                        break;
                    }
            }
        }

        public static void HandleAdminCommandLog(GameCharacter chr, Packet packet)
        {
            // 42 04 00 2F 70 6F 73 
            packet.ReadString();
        }

        public class CommandArgs
        {
            public string PlainText;
            public char Sign;
            public string Command;
            public string CommandText;
            public List<CommandArg> Args;

            public int Count => Args?.Count ?? 0;
            public CommandArg this[int Index] => GetArg(Index);

            public CommandArgs(string text)
            {
                var SplitText = text.Split(' ');
                PlainText = text;
                Sign = text[0];
                Command = SplitText[0].Remove(0, 1);
                CommandText = PlainText.Remove(0, 1).Replace($"{Command} ", "");
                SetArgs(SplitText);
            }

            public void Regenerate(string text)
            {
                SetArgs(text.Split(' '));
            }

            public CommandArg GetArg(int Index)
            {
                if (Index >= 0 && Index < Args.Count)
                    return Args[Index];
                else
                    throw new IndexOutOfRangeException($"Index must be greater then 0 and less then {Args.Count}.");
            }

            public string GetString(int index)
            {
                CommandArg arg = GetArg(index);
                string val = "";
                if (arg.ToString().StartsWith("\""))
                {
                    while (!arg.ToString().EndsWith("\""))
                    {
                        val += " " + arg.ToString().Trim('"');
                        arg = GetArg(index + 1);
                    }
                }
                return val.Trim();
            }

            public void SetArgs(string[] Strings)
            {
                if (Args == null)
                    Args = new List<CommandArg>();
                else
                    Args.Clear();

                for (int i = 1; i < Strings.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(Strings[i]) && Strings[i] != Command)
                        Args.Add(new CommandArg(Strings[i]));
                }
            }
        }

        public class CommandArg
        {
            public string Value;

            public CommandArg(string Value)
            {
                this.Value = Value;
            }

            public bool IsNumber()
            {
                foreach (var Char in Value)
                {
                    if (Char < '0' || Char > '9')
                        return false;
                }
                return true;
            }

            public byte GetByte()
            {
                byte Result = 0;
                byte.TryParse(Value, out Result);

                return Result;
            }

            public short GetInt16()
            {
                short Result = 0;
                short.TryParse(Value, out Result);

                return Result;
            }

            public int GetInt32()
            {
                int Result = 0;
                int.TryParse(Value, out Result);

                return Result;
            }

            public long GetInt64()
            {
                long Result = 0;
                long.TryParse(Value, out Result);

                return Result;
            }

            public bool GetBool()
            {
                switch (Value.ToLowerInvariant())
                {
                    case "true":
                    case "t":
                    case "yes":
                    case "y":
                    case "1":
                        return true;
                    default:
                        return false;
                }
            }
            public bool IsString()
            {
                return Value.StartsWith("\"");
            }

            public static implicit operator string(CommandArg Arg) => Arg.Value;

            public override string ToString()
            {
                return Value;
            }
        }
    }
}