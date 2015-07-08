using UnityEngine;
using System.Collections;

public class ClassNumberButton : MonoBehaviour 
{
	public float m_fButtonNumber;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnMouseDown ()
	{
		string strCurrentText = GameObject.Find("TextBox").GetComponent<TextMesh>().text;
		
		string strNewText = strCurrentText + m_fButtonNumber.ToString();
		
		GameObject.Find("TextBox").GetComponent<TextMesh>().text = strNewText;
	}
}
