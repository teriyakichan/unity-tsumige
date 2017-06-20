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

	// 所持ゲーム本数(ゴールド)
	public double games;
	// 所持アイテム
	public List<Item> items = new List<Item>();

	// 現在のクリックごとのポイント
	public decimal clickValue = (decimal)DEFAULT_CLICK;
	// 現在の自動加算ポイント
	public decimal autoValue = 0;

	public void Init()
	{
		items = Item.LoadMaster();

		RefreshClickValue();
		RefreshAutoValue();
	}

	/// <summary>
	/// 1クリックで得られるポイントを取得
	/// </summary>
	/// <returns></returns>
	public void RefreshClickValue()
	{
		double val = DEFAULT_CLICK;
		for (int i = 0; i < items.Count; ++i)
		{
			if (items[i].level == 0) continue;
			// add
			if (items[i].type == ItemType.PowerUp)
			{
				val += items[i].currentVal;
			}
			// mul
			if (items[i].type == ItemType.BoostPowerUp)
			{
				val *= items[i].currentVal;
			}
		}

		clickValue = (decimal)val;
	}

	public void RefreshAutoValue()
	{
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
