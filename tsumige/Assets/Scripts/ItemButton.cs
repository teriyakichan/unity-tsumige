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

	public void SetItem(Item item)
	{
		itemNameText.text = item.name;
		costText.text = item.currentCost.ToString();
		ownedText.text = item.level == 0 ? "" : item.level.ToString();
	}
}
