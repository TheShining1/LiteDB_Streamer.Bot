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
		public bool Value { get; set; }
	}

	public bool Execute()
	{
		string[] specialUsers = new string[]{"ShiningOne", "765rs", "adetsea"};
		string[] usualUsers = new string[]{"Cyrtor", "CapnGordon", "BeRicedBack"};
		
		string targetName = "specialVariable";
		string varName = "usualVariable";

		CPH.LogDebug($"=====Getting {targetName}=====");
		foreach(var user in specialUsers)
		{
			CPH.LogDebug($"Getting {targetName} for {user}");
			CPH.LogDebug($"Got {targetName} for {user}: {CPH.GetUserVar<int>(user, targetName, true)}");
		}

		CPH.LogDebug($"=====Getting {varName}=====");
		foreach(var user in usualUsers)
		{
			CPH.LogDebug($"Getting {varName} for {user}");
			CPH.LogDebug($"Got {varName} for {user}: {CPH.GetUserVar<int>(user, varName, true)}");
		}

		CPH.LogDebug($"=====Setting {targetName}=====");
		foreach(var user in specialUsers)
		{
			CPH.LogDebug($"Setting {targetName} for {user}");
			CPH.SetUserVar(user, targetName, true, true);
			CPH.LogDebug($"Set {targetName} for {user}");
		}

		CPH.LogDebug($"=====Setting {varName}=====");
		foreach(var user in usualUsers)
		{
			CPH.LogDebug($"Setting {varName} for {user}");
			CPH.SetUserVar(user, varName, true, true);
			CPH.LogDebug($"Set {varName} for {user}");
		}

		CPH.LogDebug("=====Checking vars via CPH=====");
		foreach(var user in specialUsers)
		{
			CPH.LogDebug($"Getting {targetName} for {user}");
			CPH.LogDebug($"Got {targetName} for {user}: {CPH.GetUserVar<int>(user, targetName, true)}");
		}

		foreach(var user in usualUsers)
		{
			CPH.LogDebug($"Getting {targetName} for {user}");
			CPH.LogDebug($"Got {targetName} for {user}: {CPH.GetUserVar<int>(user, targetName, true)}");
		}

		CPH.LogDebug("=====Checking vars via DB=====");
		using(var db = new LiteDatabase(@"Filename=.\data\globals.db;ReadOnly=true"))
		{
			try {
				CPH.LogDebug(db.ToString());

				CPH.LogDebug($"Getting users for {targetName}");
				var persUsersCol = db.GetCollection<UserVariable>("user_globals");
				var persUsersResult = persUsersCol.Query().Where(x => x.Name.Equals(targetName)).ToList();

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
