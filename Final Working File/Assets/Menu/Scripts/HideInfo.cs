using UnityEngine;
using System.Collections;

public class HideInfo : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		//Turn off rendering for all info planes
		Renderer[] rInfo = GameObject.Find("Info_").GetComponentsInChildren<Renderer>();
		for(int i = 0; i < rInfo.Length; i++)
		{
			rInfo[i].renderer.enabled = false;	
		}
		GameObject.Find("Info"+Menu_NewPlayButton.sSelectedLevel).renderer.enabled = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
