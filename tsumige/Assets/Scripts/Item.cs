using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	public const int COLUMNS = 8;

	// master(csv)
	public int id;               // id
	public string name;          // 名称
	public decimal click;         // 1クリックのポイント
	public decimal clickPerLevel; // 1レベルごとに上昇するポイント
	public decimal auto;          // 1秒のポイント
	public decimal autoPerLevel;  // 1レベルごとに上昇するポイント
	public decimal cost;          // 費用
	public decimal costPerLevel;  // 1レベルごとに上昇する費用
	// playerprefs
	public int level = 0;
	public bool unlocked = false;

	// 現在のつよさ
	public decimal currentClick
	{
		get { return click + clickPerLevel * (level - 1); }
	}
	public decimal currentAuto
	{
		get { return auto + autoPerLevel * (level - 1); }
	}
	// 現在の費用
	public decimal currentCost
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
			item.click = decimal.Parse(raw[index++]);
			item.clickPerLevel = decimal.Parse(raw[index++]);
			item.auto = decimal.Parse(raw[index++]);
			item.autoPerLevel = decimal.Parse(raw[index++]);
			item.cost = decimal.Parse(raw[index++]);
			item.costPerLevel = decimal.Parse(raw[index++]);

			items.Add(item);
		}
		return items;
	}
}
