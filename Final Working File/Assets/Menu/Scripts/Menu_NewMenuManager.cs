using UnityEngine;
using System.Collections;

public class Menu_NewMenuManager : MonoBehaviour 
{
	public bool bSelectLevel 	= false;
	public bool bSelectGame 	= false;
	public bool bPlayButton		= false;
	
	private Vector3 vOffsetBorder = new Vector3(0.0f,0.0f,5.0f);
	
	//Difficulty Selection Menu
	//Selected Level
	private string sSelectedLevel;
	
	// Use this for initialization
	void Start () 
	{	
		GameObject.Find("SelectedIcon").transform.position = GameObject.Find(Menu_NewPlayButton.sSelectedLevel).transform.position;
		GameObject.Find("SelectedIcon").transform.position -= vOffsetBorder;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnMouseUp()
	{
		if(bSelectGame == true)
		{
			SelectGame();
		}
		else if(bSelectLevel == true)
		{
			SelectLevel(gameObject.name);
		}
		else if(bPlayButton == true)
		{
			Application.LoadLevel("Game"+Menu_NewPlayButton.sSelectedLevel+"_"+gameObject.name);
		}
	}
	
	void SelectGame()
	{
		//sSelectedLevel will be in the form of "(underscore)(gameName)"
		Menu_NewPlayButton.sSelectedLevel = gameObject.name;
		GameObject.Find("SelectedIcon").transform.position = GameObject.Find(Menu_NewPlayButton.sSelectedLevel).transform.position;
		GameObject.Find("SelectedIcon").transform.position -= vOffsetBorder;
		Renderer[] rInfo = GameObject.Find("Info_").GetComponentsInChildren<Renderer>();
		for(int i = 0; i < rInfo.Length; i++)
		{
			rInfo[i].renderer.enabled = false;	
		}
		GameObject.Find("Info"+Menu_NewPlayButton.sSelectedLevel).renderer.enabled = true;
	}
	
	void SelectLevel(string _sDifficulty)
	{
		if(_sDifficulty == "Dim_Hard")
		{
			Difficulty_Selection_Settings.SwapImages(_sDifficulty);
		}
		else if(_sDifficulty == "Dim_Easy")
		{
			Difficulty_Selection_Settings.SwapImages(_sDifficulty);
		}
	}
}
