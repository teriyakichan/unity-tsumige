using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerController : MonoBehaviour
{
	public Text scoreLabel;
	public GameObject clickPoint;

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
		player.Click();
		RefreshScore();
	}

	public void RefreshScore()
	{
		scoreLabel.text = player.score.ToString().Split('.')[0];
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
			player.Buy(1);
			RefreshScore();
		}
		if (Input.GetKeyDown("2"))
		{
			player.Buy(2);
			RefreshScore();
		}
		if (Input.GetKeyDown("d"))
		{
			player.DebugItem();
		}
	}
}
