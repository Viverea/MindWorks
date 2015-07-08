using UnityEngine;
using System.Collections;

public class ClassBackButton : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnMouseDown()
	{
		if(Application.loadedLevelName == "Game_CloudSort_Easy" ||
			Application.loadedLevelName == "Game_CloudSort_Hard")
		{
			Application.LoadLevel("Game_CloudSort_Select");
		}
		else if(Application.loadedLevelName == "Game_ForwardSums_Easy" ||
			Application.loadedLevelName == "Game_ForwardSums_Hard")
		{
			Application.LoadLevel("Game_ForwardSums_Select");
		}
	}
}
