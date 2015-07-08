using UnityEngine;
using System.Collections;

public class ClassButtonMinus : MonoBehaviour 
{
	public bool m_bIsCurrentlyNegative = false;
	
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
		string strCurrentText = GameObject.Find("Sign").GetComponent<TextMesh>().text;
		
		if(m_bIsCurrentlyNegative == false)
		{
			strCurrentText = "-";
			
			m_bIsCurrentlyNegative = true;
		}
		else
		{
			strCurrentText = "";
			
			m_bIsCurrentlyNegative = false;
		}

		GameObject.Find("Sign").GetComponent<TextMesh>().text = strCurrentText;
	}
}
