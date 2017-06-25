using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using naichilab;

public class TweetButton : MonoBehaviour
{
	public void Tweet()
	{
		decimal score = 0;
		score = GameObject.Find("Script").GetComponent<ClickerController>().player.score;
		UnityRoomTweet.Tweet(
			"game-tsumutsumu",
			"ゲーム積む積むで" + score.ToString("#,0").Split('.')[0] + "本ゲームを積みました",
			"unity1week");
	}
}
