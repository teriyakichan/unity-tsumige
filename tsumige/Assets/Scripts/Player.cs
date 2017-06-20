using System;
using System.Collections;
using System.Collections.Generic;

public class Player
{
	public const double DEFAULT_CLICK = 1;

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
	// 所持アイテム
	public List<Item> items = new List<Item>();
	// 現在のクリックごとのポイント
	public decimal clickValue = (decimal)DEFAULT_CLICK;
	// 現在の自動加算ポイント
	public decimal autoValue = 0;

	public void Init()
	{
		items = Item.LoadMaster();

		Refresh();
	}

	public void Click()
	{
		score += clickValue;
	}

	public void AutoClick()
	{
		score += autoValue;
	}

	/// <summary>
	/// アイテム購入
	/// </summary>
	/// <param name="id"></param>
	/// <param name="count"></param>
	public void Buy(int id, int count = 1)
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
		if (index < 0) return;
		decimal cost = (decimal)items[index].currentCost;
		// check score
		if (score < cost) return;
		// buy
		score -= cost;
		items[index].level++;
		Refresh();
	}

	/// <summary>
	/// アイテムの効果を再計算
	/// </summary>
	/// <returns></returns>
	public void Refresh()
	{
		double clickVal = DEFAULT_CLICK;
		double autoVal = 0;
		for (int i = 0; i < items.Count; ++i)
		{
			if (items[i].level == 0) continue;
			switch (items[i].type)
			{
				case ItemType.PowerUp:
					clickVal += items[i].currentVal;
					break;
				case ItemType.BoostPowerUp:
					clickVal *= items[i].currentVal;
					break;
				case ItemType.AutoClick:
					autoVal += items[i].currentVal;
					break;
				case ItemType.BoostAutoClick:
					autoVal *= items[i].currentVal;
					break;
			}
		}

		clickValue = (decimal)clickVal;
		autoValue = (decimal)autoVal;
	}

	public void DebugItem()
	{
		for (var i = 0; i < items.Count; ++i)
		{
			var item = items[i];
			UnityEngine.Debug.Log(
				item.id + ", " +
				item.name + ", " +
				item.type + ", " +
				item.val + ", " +
				item.valPerLevel + ", " +
				item.cost + ", " +
				item.costPerLevel + " (" +
				item.level + ")"
				);
		}
	}
}
