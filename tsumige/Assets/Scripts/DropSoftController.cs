using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DropSoftController : MonoBehaviour
{
	internal readonly float MIN_X = -5f;
	internal readonly float MAX_X = 5f;
	internal readonly float INIT_Y = 15f;
	internal readonly int MAX_ITEMS = 100;
	internal readonly decimal MAX_SPEED = 100;

	public Transform softwareContainer;
	public List<GameObject> softwareList;
	private List<Sprite> _sprites;
	public List<bool> stateList;
	public decimal _speed = 0;
	public int _maxUnlockedNum;

	public void Init()
	{
		// init
		stateList = new List<bool>();
		_sprites = new List<Sprite>();
		for (int i = 0; i < softwareList.Count; ++i)
		{
			_sprites.Add(softwareList[i].GetComponent<SpriteRenderer>().sprite);
			softwareList[i].SetActive(false);
			stateList.Add(true);
		}
	}

	public void SetSpeed(decimal speed)
	{
		_speed = speed;
		if (_speed > MAX_SPEED) _speed = MAX_SPEED;
	}

	public void SetMaxUnlockedNum(int num)
	{
		_maxUnlockedNum = num;
		if (_maxUnlockedNum >= _sprites.Count - 1) _maxUnlockedNum = _sprites.Count - 1;
	}

	public void Drop()
	{
		// find idle object
		int index = 0;
		bool found = false;
		for (; index < softwareList.Count; ++index)
		{
			if (stateList[index])
			{
				found = true;
				break;
			}
		}
		// instantiate if needed
		if (!found)
		{
			if (softwareList.Count >= MAX_ITEMS) return;
			index = softwareList.Count;
			GameObject obj = Instantiate(softwareList[0]);
			obj.transform.parent = softwareContainer;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = new Vector3(0, 15f);
			softwareList.Add(obj);
			stateList.Add(true);
		}
		stateList[index] = false;
		softwareList[index].SetActive(true);
		// set sprite
		int num = Random.Range(0, _maxUnlockedNum + 1);
		softwareList[index].GetComponent<SpriteRenderer>().sprite = _sprites[num];
		// drop
		softwareList[index].transform.localPosition = new Vector3(Random.Range(MIN_X, MAX_X), INIT_Y);
		softwareList[index].transform.DOLocalMoveY(0f, Random.Range(2f, 4f)).SetEase(Ease.Linear).OnComplete(() => {
			stateList[index] = true;
			softwareList[index].SetActive(false);
		});
	}

	private float _time = 0f;
	void Update()
	{
		_time += Time.deltaTime;
		if (_time >= 1f / (float)_speed)
		{
			_time = 0f;
			Drop();
		}
	}
}
