# WvsGlobal v22 (WvsBeta_REVAMP)
WvsGlobal fork targeted for v22 and .net framework 4.8. Includes idb and scripts.

## Todo
### Crypt
- [ ] Fix AES (key already included)

### Login
- [ ] Fix blocked character creation in non-Scania worlds (worldID > 0)

### Game
- [ ] Can open player shop anywhere in fm rooms, even over other balloons
- [ ] Omok
- [ ] Guild (opcodes, packets, db)
- [ ] Quests (handle completion, etc)
- [ ] Scriptify KPQ, remove pqportalopen field
- [ ] FM portal, no sound effect
- [ ] Translate/convert remaining scripts
- [ ] Zakum (Adobis Mission)
- [ ] Pianus (Aqua, Deep Sea Gorge)
- [ ] Papulatus
- [ ] Some portals broken (Henesys Market, Deep Ant Tunnel II, wrong offset)
- [ ] Storage
- [ ] Update drop data
- [ ] Items/mesos fall from top of map when manually dropped, only in maps with mobs
- [ ] Can't loot when in party
- [ ] Party exp/leeching issues
- [ ] Party quits when leader logs out?
- [x] Scrolling
- [x] Player Shop not visible for person opening it
- [x] Player Shop empty error msg when closing
- [x] Pt job/level not updating
- [x] Town door going away when leaving party
- [x] Door not showing up when pt member with door joins pt
- [x] Door not going away when disbanding pt
- [x] Door not going away when left party
- [x] Door not showing up in field when joining party
- [x] Door not updating when creating party
- [x] Wrong party map status (leader id in wrong position?)
- [x] Doors not showing up in town, wrong spawn position
- [x] Sitting
- [x] Buddy map/find crash
- [x] Remote AvatarLook not updating in messenger
- [x] Cash eqps unseen
- [x] Messenger accept crash
- [x] Party accept crash
- [x] Cant loot other ppl's drops
- [x] Crash on user_enter_field

### Cash Shop
- [x] Crash on move item from CS to inventory
- [x] Best item list empty

## Notes
- AES no worky, use maplecrypt (shanda).
- Running Tespia or any other world other than Scania disables char creation for some reason. Tespia is assumed to have ID 20 (wz edited).
- Shops/player interactions not tested.
- Config options for Redis, ElasticSearch/logging and some others are missing
- ReNX has been modified to support merging of NX files
- log4net.ElasticSearch was made a bit better in terms of memory usage and logging
- SQLs/wvsbeta_sql_compatible.sql should be compatible with the server
- packages/ folder contains the NuGet packages.

## Features
- Lots.
- log4net and ElasticSearch for logging/tracing
- Redis for locking character transfers when they CC and other features
- NX data files
- Threaded packet sending queue to offload slow sockets
- PatchCreator for maple .patch files, using jdiff
- Improved MasterThread for running the best as possible, without CPU load
- Anti-Hack measures
- Discord webhook support
- bcrypt password hashing

## KMS Progression
### 2003
- 1st CBT - 21 jan
- 2nd CBT - 29 jan
- 3rd cbt - 24 feb
- 4th cbt, four basic classes available - 25 mar
- World: Bera & Town: Sleepywood - 21 apr
- Open beta - 29 apr
- 2nd job - 23 may
- cash shop - 27 may
- world: plana - 20 jul
- world: stiud & mushmom - 25 jul
- world: bellocan - 24 oct
- world: demethos - 30 dec

### 2004
- world: yellonde, ossyria, omok - 17 feb
- world: kastia - 2 mar
- 1st anniversary - 29 apr
- 3rd job advancement - 20 jul
- zakum dungeon quest, world: el nido - 25 oct
- ranking system, ludibrium, world: windia - 29 dec

### 2005
- zakum defeated for the first time - 12 feb
- LPQ - 24 mar
- lie detector - 28 apr
- 2nd anniversary - 29 apr
- world: judis, guild system - 12 jul
- papulatus - 23 aug
- aqua road - 29 sep
- aquarium - 18 nov
- korean folk town, world: khardia - 21 dec

### 2006
- pianus, deep sea gorge - 24 jan
- sharenian guild quest - 25 apr
- 3rd anniversary - 29 apr

## Credits
- wackyracer
- Exile
- Diamondo25
- Sifl
- Anthony
- SpAgentMoo
- Joob
- Rath
- Ginseng
- csproj (for doing about nothing at all. hurr)

And all staff and the players that have made the server a great success. You'll be remembered.

## Things we've learned
- Don't exploit or hack. We knew all along.
- Don't bug GiveEXP function so that it saves on every exp gain, while there are 50 people training (thanks Exile :) )
- Don't livestream with your camera pointing to your keyboard and then log in.
- Let other people check your code to remove exploits.
- Don't rush releases.
- Delaying releases because migration logic is borked is not helping the problem, either. Like the teleport bug, hurr.
- And if you decide to release and do your last push to git, your git host could drop out (Gitlab issue @ 2017-08-24 at 2 AM)
- KitterzPE or RiPE will not work. Stop crashing your clients...
- Keep your Windows Server up-to-date, otherwise you might get rekt >_>. (We never had that issue)
- There are VPSs running on old hardware out there. Ours was on a Nehalem or Westmere based host.
- Make sure your server host(s) is/are paid. Otherwise you get downtime out of nowhere.
- Don't get top ranked on Google for "maplestory global"
- GTOP is shit. Don't spend money and time into that.
- If you do decide to use GTOP, expect downtimes. And log _everything_ related to it.
- Don't boost your friends as a server owner, outside of Tespia. People are not going to like that. #boostgate
- People like being midget.
- Keep love outside of private servers, or you might get f*cked
- Keep your password private. We've had 5 people that we had to recover from some brother or sister deleting their character(s)...
- Do not push to production directly. That single line edit could have a lot of issues
- Use debug logging using debug logging functionalities, especially when you do it in the attack handler
- Trace.WriteLine is erased. Do not pass a function call, such as packet.ReadInt() to it, because that will disappear.
- Do NOT select text inside a console. It freezes the application running inside it.
- Double check, no, TRIPLE check your queries. Don't want your shoes to end up on your head ;')
- Have a way to test someones character to figure out issues.
- Do not put too much money in Facebook. Your reach will be capped at a certain point
- Use /map 0 for free meso
- Stay away from El Nath, it might kill you until you are dead (and lost all your EXP). Who needs to check HP anyway?
- When you think you fixed an issue with falling down the map, and you accidentally made a teleport cheat on the map boundaries. Which then keeps on existing for weeks.
- TomBradyProtect, and you'll never get a wz editor ever again.
- Don't give away Ilbis as a CM.
- Muting people might make them use emoji responses to talk, instead.
- Make sure repeated actions are in sync, and don't decide to stack up in events due to locking and such hurr...
