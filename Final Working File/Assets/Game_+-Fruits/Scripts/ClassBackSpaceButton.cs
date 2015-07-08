using UnityEngine;
using System.Collections;

public class ClassBackSpaceButton : MonoBehaviour 
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
		string strCurrentText = GameObject.Find ("TextBox").GetComponent<TextMesh>().text;
		
		int nTextLength = strCurrentText.Length;
		
		if(nTextLength > 0)
		{
			strCurrentText = strCurrentText.Substring(0, nTextLength - 1);
			
			GameObject.Find ("TextBox").GetComponent<TextMesh>().text = strCurrentText;
		}
		
	}
}
