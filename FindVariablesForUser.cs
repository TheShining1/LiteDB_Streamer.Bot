using System;
using LiteDB;
using System.Linq;
using System.Collections.Generic;

public class CPHInline
{
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

		Dictionary<string,int> testVars = new Dictionary<string,int>(){
			{"var0", 0},
			{"var1", 0},
			{"var2", 0},
			{"var3", 0}
		};
		CPH.LogDebug("=====Getting vars=====");
		foreach(var name in testVars.Keys.ToList())
		{
			CPH.LogDebug($"Getting {name}");
			testVars[name] = CPH.GetUserVar<int>(testUser.Name, name, true);
			CPH.LogDebug($"Got {name}: {testVars[name]}");
		}

		CPH.LogDebug($"=====Setting vars=====");
		foreach(var kvp in testVars)
		{
			CPH.LogDebug($"Setting {kvp.Key}");
			int val = kvp.Value;
			CPH.SetUserVar(testUser.Name, kvp.Key, ++val, true);
			CPH.LogDebug($"Set {kvp.Key}: {val}");
		}

		CPH.LogDebug("=====Checking vars via CPH=====");
		foreach(var kvp in testVars)
		{
			CPH.LogDebug($"Got user {testUser.Name}, key: {kvp.Key}: {CPH.GetUserVar<int>(testUser.Name, kvp.Key, true)}");
		}

		CPH.LogDebug("=====Checking vars via DB=====");
		using(var db = new LiteDatabase(@"Filename=.\data\globals.db;ReadOnly=true"))
		{
			try {
				CPH.LogDebug(db.ToString());

				CPH.LogDebug($"Getting variables for {testUser.Name}");
				var persUsersCol = db.GetCollection<UserVariable>("user_globals");
				var persUsersResult = persUsersCol.Query().Where(x => x.userId.Equals(testUser.Id)).ToList();

				foreach(var item in persUsersResult)
				{
					CPH.LogDebug($"Got user: {item.userId.ToString()}, key: {item.Name.ToString()}, value: {item.Value}");
				
				}			
			} catch(Exception e)
			{
				CPH.LogDebug(e.ToString());
			}
		}

		return true;
	}
}
