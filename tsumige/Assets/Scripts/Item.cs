using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	// master(csv)
	public int id;             // id
	public string name;        // 名称
	public ItemType type;      // アイテム種別
	public double val;         // つよさ
	public double valPerLevel; // 1レベルごとに上昇するつよさ
	public double cost;          // 費用
	public double costPerLevel;  // 1レベルごとに上昇する費用
	// playerprefs
	public int level = 0;

	// 現在のつよさ
	public double currentVal
	{
		get { return val + valPerLevel * level; }
	}
	// 現在の費用
	public double currentCost
	{
		get { return cost + costPerLevel * level; }
	}

	public static List<Item> LoadMaster()
	{
		var items = new List<Item>();
		var data = Resources.Load("Data/master.txt") as TextAsset;
		var dataList = data.text.Replace("\r\n", "\n").Split('\n');
		string[] raw;
		for (var i = 0; i < dataList.Length; ++i)
		{
			var item = new Item();
			// parse csv
			raw = dataList[i].Split(',');
			int index = 0;
			item.id = int.Parse(raw[index++]);
			item.name = raw[index++];
			item.type = (ItemType)int.Parse(raw[index++]);
			item.val = double.Parse(raw[index++]);
			item.valPerLevel = double.Parse(raw[index++]);
			item.cost = double.Parse(raw[index++]);
			item.costPerLevel = double.Parse(raw[index++]);

			items.Add(item);
		}
		return items;
	}
}
