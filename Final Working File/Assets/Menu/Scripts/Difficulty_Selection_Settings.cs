using UnityEngine;
using System.Collections;

public class Difficulty_Selection_Settings : MonoBehaviour 
{
	public	static	string	sLastLoadedLevel;
	private static 	Vector3	vEasyPlayPosition;
	private static 	Vector3	vHardPlayPosition;
	
	// Use this for initialization
	void Start () 
	{
		vEasyPlayPosition 	= GameObject.Find("Easy").transform.position;
		vHardPlayPosition 	= GameObject.Find("Hard").transform.position;
		
		//Hide play button for hard
		vHardPlayPosition.z = 100.0f;
		GameObject.Find("Hard").transform.position = vHardPlayPosition;
		
		if(sLastLoadedLevel.Contains("Hard"))
		{
			SwapImages("Dim_Hard");
			sLastLoadedLevel = "";
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public static void SwapImages(string _sDifficulty)
	{
		//Swap BG
		Vector3 tempBG = GameObject.Find("BGPlane").transform.position;
		GameObject.Find("BGPlane").transform.position = GameObject.Find("BGPlaneDim").transform.position;
		GameObject.Find("BGPlaneDim").transform.position = tempBG;
		//Swap Hard
		Vector3 tempHard = GameObject.Find("Dim_Hard").transform.position;
		GameObject.Find("Dim_Hard").transform.position = GameObject.Find("Selected_Hard").transform.position;
		GameObject.Find("Selected_Hard").transform.position = tempHard;	
		//Swap Easy
		Vector3 tempEasy = GameObject.Find("Dim_Easy").transform.position;
		GameObject.Find("Dim_Easy").transform.position = GameObject.Find("Selected_Easy").transform.position;
		GameObject.Find("Selected_Easy").transform.position = tempEasy;
		
		if(_sDifficulty == "Dim_Hard")
		{
			//Hard bring to front
			vHardPlayPosition.z = 105.0f;
			GameObject.Find("Hard").transform.position = vHardPlayPosition;
			//Easy send to back
			vEasyPlayPosition.z = 100.0f;
			GameObject.Find("Easy").transform.position = vEasyPlayPosition;
		}
		else if(_sDifficulty == "Dim_Easy")
		{
			//Easy bring to front
			vEasyPlayPosition.z = 105.0f;
			GameObject.Find("Easy").transform.position = vEasyPlayPosition;
			//Hard send to back
			vHardPlayPosition.z = 100.0f;
			GameObject.Find("Hard").transform.position = vHardPlayPosition;
		}
	}
}
