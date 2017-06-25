using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Image itemImage;
	public Text itemNameText;
	public Text costText;
	public Text ownedText;
	private string _itemName = "";

	public GameObject tipObject;
	public Text tipClickText;
	public Text tipAutoText;

	private Action _onMouseEnterCallback;

	void Awake()
	{
		GetComponent<Button>().interactable = false;
		itemImage.color = Color.black;
		tipObject.SetActive(false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (_onMouseEnterCallback != null) _onMouseEnterCallback.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tipObject.SetActive(false);
	}

	public void ShowTip(Item item, decimal click, decimal auto)
	{
		tipClickText.text = item.currentClick.ToString("0.0") + " / click (" +
			(item.currentClick / click * 100).ToString("0.00") + "%)";
		tipAutoText.text = item.currentAuto.ToString("0.0") + " / sec (" +
			(item.currentAuto / auto * 100).ToString("0.00") + "%)";
		tipObject.SetActive(true);
	}

	public void SetOnMouseCallback(Action callback)
	{
		_onMouseEnterCallback = callback;
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
