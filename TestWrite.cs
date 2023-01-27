using System;
using LiteDB;
using System.Linq;

public class CPHInline
{
	public class GlobalVariable
	{
		public string Id { get; set; }
		public object Value { get; set; }
	}

	public bool Execute()
	{
		using(var db = new LiteDatabase(@"Filename=.\data\globals.db"))
		{
			var col = db.GetCollection<GlobalVariable>("globals");

		    var myVar = new GlobalVariable
			{ 
				Id = "My variable", 
				Value = $"My value {CPH.Between(0, 100)}"
			};

			CPH.LogDebug($"Saving our variable. Key: {myVar.Id}, Value: {myVar.Value}");

			col.Upsert(myVar);

			var results = col.Query().Where(x => x.Id.Equals(myVar.Id)).ToList();
			foreach (var item in results)
			{
				CPH.LogDebug($"Getting our variable. Key: {item.Id.ToString()}, Value: {item.Value}");
			}
		}
		return true;
	}
}
