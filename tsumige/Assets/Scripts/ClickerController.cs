using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerController : MonoBehaviour
{
	public Text scoreLabel;
	public GameObject clickPoint;

	public decimal count;

	public Player player;

	void Start()
	{
		player = Player.GetInstance();
		player.Init();
	}

	/// <summary>
	/// クリック時加算
	/// </summary>
	public void Click()
	{
		count += (decimal)player.clickValue;

		scoreLabel.text = count + "";
	}

	void FixedUpdate()
	{
		// 自動クリック
	}

	void Update()
	{
		if (Input.GetKeyDown("z"))
		{
			Click();
		}
		if (Input.GetKeyDown("1"))
		{
			player.items[0].level += 1;
			player.RefreshClickValue();
		}
		if (Input.GetKeyDown("2"))
		{
			player.items[1].level += 1;
			player.RefreshClickValue();
		}
		if (Input.GetKeyDown("d"))
		{
			player.DebugItem();
		}
	}
}
