using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerController : MonoBehaviour
{
	internal readonly float SAVE_INTERVAL = 30f;
	public DropSoftController dropController;

	public Text scoreLabel;
	public Text clickSpeedLabel;
	public Text autoSpeedLabel;
	public Button clickerButton;

	public Transform buttonContainer;
	public ItemButton[] buttons;
	public GameObject[] hardwares;
	private int[] _itemIds;

	public Player player;

	private DateTime _lastUpdated;

	void Start()
	{
		_init();
	}

	private void _init()
	{
		_lastUpdated = DateTime.Now;
		// events
		clickerButton.onClick.AddListener(() => Click());
		// init player
		player = Player.GetInstance();
		player.Init();
		// init UI
		dropController.Init();
		UnityEngine.Object buttonPrefab = Resources.Load("ItemButton");
		buttons = new ItemButton[player.items.Count];
		_itemIds = new int[player.items.Count];
		for (int i = 0; i < player.items.Count; ++i)
		{
			hardwares[i].SetActive(false);
			GameObject obj = Instantiate(buttonPrefab) as GameObject;
			RectTransform rect = obj.GetComponent<RectTransform>();
			rect.SetParent(buttonContainer);
			rect.anchoredPosition = new Vector2(0, i * -58);
			int index = i;
			_itemIds[i] = player.items[i].id;
			ItemButton button = obj.GetComponent<ItemButton>();
			button.SetOnMouseCallback(() => ShowTip(index));
			button.SetItem(player.items[i]);
			button.SetSprite(player.items[i].id);
			buttons[i] = button;
			obj.GetComponent<Button>().onClick.AddListener(() => BuyItem(index));
		}
		// load data & refresh
		player.Load();
		for (int i = 0; i < player.items.Count; ++i)
		{
			buttons[i].SetItem(player.items[i]);
			if (player.items[i].unlocked)
				buttons[i].Unlock();
			if (player.items[i].level > 0)
				hardwares[i].SetActive(true);
			dropController.SetMaxUnlockedNum(player.unlockedItem);
			dropController.SetSpeed(player.autoValue);
		}
		RefreshScore();
	}

	public void ShowTip(int index)
	{
		if (player.items[index].level > 0)
			buttons[index].ShowTip(player.items[index], player.clickValue, player.autoValue);
	}

	/// <summary>
	/// クリック時加算
	/// </summary>
	public void Click()
	{
		player.Click();
		dropController.Drop();
	}

	public void BuyItem(int index)
	{
		Item item = player.Buy(_itemIds[index]);
		if (item != null) buttons[index].SetItem(item);
		dropController.SetMaxUnlockedNum(player.unlockedItem);
		dropController.SetSpeed(player.autoValue);
		ShowTip(index);
		RefreshScore();
		for (int i = 0; i < player.items.Count; ++i)
			if (player.items[i].level > 0)
				hardwares[i].SetActive(true);
		player.Save();
	}

	public void RefreshScore(bool onlyScore = false)
	{
		scoreLabel.text = player.score.ToString("#,0").Split('.')[0];
		for (int i = 0; i < player.items.Count; ++i)
		{
			if (player.items[i].unlocked) continue;
			if (player.score >= (decimal)player.items[i].cost)
			{
				player.items[i].unlocked = true;
				buttons[i].Unlock();
			}
			else
			{
				break;
			}
		}
		if (onlyScore) return;
		autoSpeedLabel.text = player.autoValue.ToString("0.0");
		clickSpeedLabel.text = player.clickValue.ToString("0.0");
	}

	private TimeSpan _ts;
	private DateTime _now;
	void FixedUpdate()
	{
		_now = DateTime.Now;
		_ts = _now - _lastUpdated;
		if (_ts.TotalSeconds > 5)
		{
			player.AutoClick((decimal)_ts.TotalSeconds);
		}
		_lastUpdated = _now;
		// 自動クリック
		player.AutoClick((decimal)(Time.deltaTime / 1f));
	}

	private float _time = 0;
	void Update()
	{
		RefreshScore(true);
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
		{
			Click();
		}
		// auto save
		_time += Time.deltaTime;
		if (_time >= SAVE_INTERVAL)
		{
			player.Save();
		}
#if false
		// DEBUG
		if (Input.GetKey(KeyCode.Z))
		{
			Click();
		}
		if (Input.GetKey(KeyCode.A)) player.score += 10000;
		if (Input.GetKey(KeyCode.S)) player.score += 100000;
		if (Input.GetKey(KeyCode.D)) player.score += 1000000;
		if (Input.GetKey(KeyCode.F)) player.score += 10000000;
		if (Input.GetKeyDown("1"))
		{
			player.Buy(1);
		}
		if (Input.GetKeyDown("2"))
		{
			player.Buy(2);
		}
		if (Input.GetKeyDown("d"))
		{
			player.DebugItem();
		}
#endif
	}
}
