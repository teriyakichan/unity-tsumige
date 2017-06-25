using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerController : MonoBehaviour
{
	public Text scoreLabel;
	public Text clickSpeedLabel;
	public Text autoSpeedLabel;
	public Button clickerButton;

	public Transform buttonContainer;
	public ItemButton[] buttons;
	public GameObject[] hardwares;
	private int[] _itemIds;

	public Player player;

	void Start()
	{
		_init();
	}

	private void _init()
	{
		// events
		clickerButton.onClick.AddListener(() => Click());
		// init player
		player = Player.GetInstance();
		player.Init();
		// init UI
		Object buttonPrefab = Resources.Load("ItemButton");
		buttons = new ItemButton[player.items.Count];
		_itemIds = new int[player.items.Count];
		for (int i = 0; i < player.items.Count; ++i)
		{
			hardwares[i].SetActive(false);
			GameObject obj = Instantiate(buttonPrefab) as GameObject;
			obj.transform.parent = buttonContainer;
			RectTransform rect = obj.GetComponent<RectTransform>();
			rect.anchoredPosition = new Vector2(0, i * -58);
			int index = i;
			_itemIds[i] = player.items[i].id;
			ItemButton button = obj.GetComponent<ItemButton>();
			button.SetItem(player.items[i]);
			button.SetSprite(player.items[i].id);
			buttons[i] = button;
			obj.GetComponent<Button>().onClick.AddListener(() => BuyItem(index));
		}

		RefreshScore();
	}

	/// <summary>
	/// クリック時加算
	/// </summary>
	public void Click()
	{
		player.Click();
	}

	public void BuyItem(int index)
	{
		Item item = player.Buy(_itemIds[index]);
		if (item != null) buttons[index].SetItem(item);
		RefreshScore();
		for (int i = 0; i < player.items.Count; ++i)
		{
			if (player.items[i].level == 0) return;
			hardwares[i].SetActive(true);
		}
	}

	public void RefreshScore(bool onlyScore = false)
	{
		scoreLabel.text = player.score.ToString().Split('.')[0];
		for (int i = 0; i < player.items.Count; ++i)
		{
			if (player.items[i].unlocked) continue;
			if (player.score >= (decimal)player.items[i].cost)
			{
				player.items[i].unlocked = true;
				buttons[i].Unlock();
				Debug.Log("unlock" + i);
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

	void FixedUpdate()
	{
		// 自動クリック
		player.AutoClick((decimal)(Time.deltaTime / 1f));
	}

	void Update()
	{
		RefreshScore(true);
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
		{
			Click();
		}
		// DEBUG
		if (Input.GetKey(KeyCode.Z))
		{
			Click();
		}
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
	}
}
