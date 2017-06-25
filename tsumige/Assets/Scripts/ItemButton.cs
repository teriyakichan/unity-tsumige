using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
	public Image itemImage;
	public Text itemNameText;
	public Text costText;
	public Text ownedText;
	private string _itemName = "";

	void Start()
	{
		GetComponent<Button>().interactable = false;
		itemImage.color = Color.black;
	}

	public void SetItem(Item item)
	{
		costText.text = item.currentCost.ToString("#,0");
		ownedText.text = item.level == 0 ? "" : item.level.ToString();
		_itemName = item.name;
	}

	public void SetSprite(int num)
	{
		Texture2D texture = Resources.Load("Sprites/h" + num) as Texture2D;
		itemImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		itemNameText.text = "?";
	}

	public void Unlock()
	{
		GetComponent<Button>().interactable = true;
		itemImage.color = Color.white;
		itemNameText.text = _itemName;
	}
}
