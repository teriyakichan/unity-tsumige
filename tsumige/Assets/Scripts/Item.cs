using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	public const int COLUMNS = 8;

	// master(csv)
	public int id;               // id
	public string name;          // 名称
	public double click;         // 1クリックのポイント
	public double clickPerLevel; // 1レベルごとに上昇するポイント
	public double auto;          // 1秒のポイント
	public double autoPerLevel;  // 1レベルごとに上昇するポイント
	public double cost;          // 費用
	public double costPerLevel;  // 1レベルごとに上昇する費用
	// playerprefs
	public int level = 0;
	public bool unlocked = false;

	// 現在のつよさ
	public double currentClick
	{
		get { return click + clickPerLevel * (level - 1); }
	}
	public double currentAuto
	{
		get { return auto + autoPerLevel * (level - 1); }
	}
	// 現在の費用
	public double currentCost
	{
		get { return cost + costPerLevel * level; }
	}


	public static List<Item> LoadMaster()
	{
		var items = new List<Item>();
		var data = Resources.Load("Data/master_items") as TextAsset;
		var dataList = data.text.Replace("\r\n", "\n").Split('\n');
		string[] raw;
		for (var i = 0; i < dataList.Length; ++i)
		{
			var item = new Item();
			// parse csv
			raw = dataList[i].Split(',');
			if (raw.Length != COLUMNS) continue;
			int index = 0;
			item.id = int.Parse(raw[index++]);
			item.name = raw[index++];
			item.click = double.Parse(raw[index++]);
			item.clickPerLevel = double.Parse(raw[index++]);
			item.auto = double.Parse(raw[index++]);
			item.autoPerLevel = double.Parse(raw[index++]);
			item.cost = double.Parse(raw[index++]);
			item.costPerLevel = double.Parse(raw[index++]);

			items.Add(item);
		}
		return items;
	}
}
