## Installation
To get started, you'll first need to install Visual Studio and MySQL Server (latest version should work). For the database client, I like to use HeidiSQL. Python will also be a requirement to run some scripts. Once installed, open up `wz2nx\WZ2NX.sln` and build it in both Debug and Release mode. This is important for being able to run the conversion scripts later on.

Now, to be able to load the game data for the servers, you'll need a fresh MapleStory v22 installation inside `ClientBin\`. Old MapleStory installations can be found on forums such as RaGEZONE. Once installed, you can go into `ClientBin\Patches\`, extract `0001_Update.patch.zip`, and then run `DataWZ_ApplyPatches.bat` in `ClientBin\`. This will bring `Data.wz` up to date with the repository.

## Configuration
In `DataSvr\` you'll see a bunch of .example files which are the configuration files used by the servers, loaded as arguments when you start them. `Game0.img` is the configuration for `WvsBeta.Game` (Channel server), `Center.img` for `WvsBeta.Center` (World server), and so on. `Config.img` contains general configuration used by all the servers. Database.img contains your database configuration.

Before launching the servers, you'll also need the `ClientData.nx` file which is converted by running ClientDataConvert.bat. The batch file expects a Data.wz file inside `ClientBin\`, so make sure you have a fresh MapleStory v22 installation in there first. `ServerData.nx` is another file you'll need, which contains the server data such as drop tables, field set data, reactor actions, npc shops, etc. The file is generated from ServerData.wz, which is small enough to be hosted on GitHub, so to generate the file you just have to run `ServerDataConvert.bat`.

## Database
The SQL for the database is located in `SQLs\wvsbeta_sql_compatible.sql`. Once the database has been generated, create an admin account in the `users` table, and set yourself as an admin and a tester (`gm` column to `144`)

## Running
To run the servers, you'll first need to build them to `BinSvr\`, so open up `WvsBeta_REVAMP.sln` with Visual Studio and then hit `Ctrl+Shift+B` to build your solution. To run the servers in Debug mode, go into the project properties of WvsBeta.Center, go to Debug, and in command line arguments, enter the name of the `Center.img` configuration file from above (without the `.img` extension). For example, if your file is named `Center.img`, you would put in `Center`. Repeat these steps for `WvsBeta.Game`, `WvsBeta.Login`, and `WvsBeta.Shop`.
