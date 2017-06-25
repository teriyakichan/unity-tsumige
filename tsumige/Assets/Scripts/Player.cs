using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	public const decimal DEFAULT_CLICK = 1;

	private static Player _player;
	public static Player GetInstance()
	{
		if (_player == null)
		{
			_player = new Player();
		}
		return _player;
	}

	// スコア(ゴールド)
	public decimal score;
	public decimal scoreTotal;
	// 所持アイテム
	public List<Item> items = new List<Item>();
	// 現在のクリックごとのポイント
	public decimal clickValue = (decimal)DEFAULT_CLICK;
	// 現在の自動加算ポイント
	public decimal autoValue = 0;

	public int unlockedItem
	{
		get
		{
			int unlocked = 0;
			for (int i = 0; i < items.Count; ++i)
			{
				if (items[i].level > 0) unlocked = i;
			}
			return unlocked;
		}
	}

	public void Init()
	{
		items = Item.LoadMaster();

		Refresh();
	}

	public void Click()
	{
		score += clickValue;
		scoreTotal += clickValue;
	}

	public void AutoClick(decimal percentage)
	{
		score += autoValue * percentage;
		scoreTotal += autoValue * percentage;
	}

	/// <summary>
	/// アイテム購入
	/// </summary>
	/// <param name="id"></param>
	/// <param name="count"></param>
	public Item Buy(int id, int count = 1)
	{
		int index = -1;
		for (int i = 0; i < items.Count; ++i)
		{
			if (items[i].id == id)
			{
				index = i;
				break;
			}
		}
		// check item
		if (index < 0) return null;
		decimal cost = (decimal)items[index].currentCost;
		// check score
		if (score < cost) return null;
		// buy
		score -= cost;
		items[index].level++;
		Refresh();
		return items[index];
	}

	/// <summary>
	/// アイテムの効果を再計算
	/// </summary>
	/// <returns></returns>
	public void Refresh()
	{
		decimal clickVal = DEFAULT_CLICK;
		decimal autoVal = 0;
		for (int i = 0; i < items.Count; ++i)
		{
			if (items[i].level == 0) continue;
			clickVal += items[i].currentClick;
			autoVal += items[i].currentAuto;
		}

		clickValue = (decimal)clickVal;
		autoValue = (decimal)autoVal;
	}

	/// <summary>
	/// save
	/// </summary>
	public void Save()
	{
		PlayerPrefs.SetString("score", score.ToString());
		PlayerPrefs.SetString("scoreTotal", scoreTotal.ToString());
		for (int i = 0; i < items.Count; ++i)
		{
			PlayerPrefs.SetInt("Item" + i, items[i].level);
			PlayerPrefs.SetInt("ItemUnlocked" + i, items[i].unlocked ? 1 : 0);
		}
		PlayerPrefs.Save();
	}

	/// <summary>
	/// load
	/// </summary>
	public void Load()
	{
		score = decimal.Parse(PlayerPrefs.GetString("score", "0"));
		scoreTotal = decimal.Parse(PlayerPrefs.GetString("scoreTotal", "0"));
		for (int i = 0; i < items.Count; ++i)
		{
			items[i].level = PlayerPrefs.GetInt("Item" + i, 0);
			items[i].unlocked = PlayerPrefs.GetInt("ItemUnlocked" + i, 0) == 1;
		}
		Refresh();
	}


	public void DebugItem()
	{
		for (var i = 0; i < items.Count; ++i)
		{
			var item = items[i];
			UnityEngine.Debug.Log(
				item.id + ", " +
				item.name + ", " +
				item.click + ", " +
				item.clickPerLevel + ", " +
				item.auto + ", " +
				item.autoPerLevel + ", " +
				item.cost + ", " +
				item.costPerLevel + " (" +
				item.level + ")"
				);
		}
	}
}
