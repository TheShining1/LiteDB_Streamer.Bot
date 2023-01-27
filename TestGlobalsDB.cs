using System;
using LiteDB;
using System.Linq;
using System.Collections.Generic;

public class CPHInline
{
	public class GlobalVariable
	{
		public string Id { get; set; }
		public int Value { get; set; }
	}

	public class UserVariable
	{
		public string userId { get; set; }
		public string Name { get; set; }
		public int Value { get; set; }
	}

	public class User {
		public string Id { get; set; }
		public string Name { get; set; }
	}

	public bool Execute()
	{
		User testUser = new User(){
			Id = "638259079",
			Name = "ShiningOne"
		};

		string[] varsNames = new string[]{"testGlobalPers", "testGlobalNonPers", "testUserPers", "testUserNonPers"};

		CPH.LogDebug("=====Getting vars=====");
		CPH.LogDebug($"Getting {varsNames[0]}");
		int testGlobalPers = CPH.GetGlobalVar<int>(varsNames[0], true);
		CPH.LogDebug($"Got {varsNames[0]}: {testGlobalPers}");

		CPH.LogDebug($"Getting {varsNames[1]}");
		int testGlobalNonPers = CPH.GetGlobalVar<int>(varsNames[1], false);
		CPH.LogDebug($"Got {varsNames[1]}: {testGlobalNonPers}");

		CPH.LogDebug($"Getting {varsNames[2]}");
		int testUserPers = CPH.GetUserVar<int>(testUser.Name, varsNames[2], true);
		CPH.LogDebug($"Got {varsNames[2]}: {testUserPers}");

		CPH.LogDebug($"Getting {varsNames[3]}");
		int testUserNonPers = CPH.GetUserVar<int>(testUser.Name, varsNames[3], false);
		CPH.LogDebug($"Got {varsNames[3]}: {testUserNonPers}");

		CPH.LogDebug($"=====Setting vars=====");
		CPH.LogDebug($"Setting {varsNames[0]}");
		CPH.SetGlobalVar(varsNames[0], ++testGlobalPers, true);
		CPH.LogDebug($"Set {varsNames[0]}: {testGlobalPers}");

		CPH.LogDebug($"Setting {varsNames[1]}");
		CPH.SetGlobalVar(varsNames[1], ++testGlobalNonPers, false);
		CPH.LogDebug($"Set {varsNames[1]}: {testGlobalNonPers}");

		CPH.LogDebug($"Setting {varsNames[2]}");
		CPH.SetUserVar(testUser.Name, varsNames[2], ++testUserPers, true);
		CPH.LogDebug($"Set {varsNames[2]}: {testUserPers}");

		CPH.LogDebug($"Setting {varsNames[3]}");
		CPH.SetUserVar(testUser.Name, varsNames[3], ++testUserNonPers, false);
		CPH.LogDebug($"Set {varsNames[3]}: {testUserNonPers}");

		CPH.LogDebug("=====Checking vars via CPH=====");
		CPH.LogDebug($"Got {varsNames[0]}: {CPH.GetGlobalVar<int>(varsNames[0], true)}");
		CPH.LogDebug($"Got {varsNames[1]}: {CPH.GetGlobalVar<int>(varsNames[1], false)}");
		CPH.LogDebug($"Got {varsNames[2]}: {CPH.GetUserVar<int>(testUser.Name, varsNames[2], true)}");
		CPH.LogDebug($"Got {varsNames[3]}: {CPH.GetUserVar<int>(testUser.Name, varsNames[3], false)}");

		CPH.LogDebug("=====Checking vars via DB=====");

		using(var db = new LiteDatabase(@"Filename=.\data\globals.db;ReadOnly=true"))
		{
			try {
				CPH.LogDebug(db.ToString());

				CPH.LogDebug($"Getting {varsNames[0]}");
				var persGlobalsCol = db.GetCollection<GlobalVariable>("globals");
				var persGlobalsResult = persGlobalsCol.FindOne(x => x.Id.Equals(varsNames[0].ToString()));
				CPH.LogDebug($"Got {varsNames[0]}. Key: {persGlobalsResult.Id.ToString()}, value: {persGlobalsResult.Value}");

				CPH.LogDebug($"Getting {varsNames[1]}");
				var tempGlobalsCol = db.GetCollection<GlobalVariable>("temp_globals");
				var tempGlobalsResult = tempGlobalsCol.FindOne(x => x.Id.Equals(varsNames[1].ToString()));
				CPH.LogDebug($"Got {varsNames[1]}. Key: {tempGlobalsResult.Id.ToString()}, value: {tempGlobalsResult.Value}");

				CPH.LogDebug($"Getting {varsNames[2]}");
				var persUsersCol = db.GetCollection<UserVariable>("user_globals");
				var persUsersResult = persUsersCol.FindOne(x => x.userId.Equals(testUser.Id) && x.Name.Equals(varsNames[2].ToString()));
				CPH.LogDebug($"Got {varsNames[2]}. User: {persUsersResult.userId.ToString()}, key: {persUsersResult.Name.ToString()}, value: {persUsersResult.Value}");

				CPH.LogDebug($"Getting {varsNames[3]}");
				var tempUsersCol = db.GetCollection<UserVariable>("temp_user_globals");
				var tempUsersResult = tempUsersCol.FindOne(x => x.userId.Equals(testUser.Id) && x.Name.Equals(varsNames[3].ToString()));
				CPH.LogDebug($"Got {varsNames[3]}. User: {tempUsersResult.userId.ToString()}, key: {tempUsersResult.Name.ToString()}, value: {tempUsersResult.Value}");
			} catch(Exception e)
			{
				CPH.LogDebug(e.ToString());
			}
		}

		return true;
	}
}
