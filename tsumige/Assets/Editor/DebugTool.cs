using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugTool {
	[MenuItem("Debug/Reset score")]
	public static void ResetScore()
	{
		PlayerPrefs.DeleteAll();
	}
}
