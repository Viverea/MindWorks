using UnityEngine;
using System.Collections;

public class MenuMusic : MonoBehaviour 
{
	
	// Use this for initialization
	void Start () 
	{

		if(Application.loadedLevelName == "Menu_Main" || Application.loadedLevelName == "Menu_Selection")
		{
			GameObject.Find("Jukebox").audio.enabled = true;
		}
		else
		{
			GameObject.Find("Jukebox").audio.enabled = false;
		}
		
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
