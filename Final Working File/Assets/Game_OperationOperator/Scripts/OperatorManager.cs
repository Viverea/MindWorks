using UnityEngine;
using System.Collections;

public class OperatorManager : MonoBehaviour 
{
	
	public static string sPlus 		= " + ";
	public static string sMinus 	= " - ";
	public static string sMultiply 	= " * ";
	public static string sDivide 	= " ÷ ";
	private Color cMyColor;

	// Use this for initialization
	void Start () 
	{
		cMyColor = gameObject.renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( gameObject.name != "Clear" )
		{
			if ( !GameManager.bNumber )
				gameObject.renderer.material.color = cMyColor;
			else
				gameObject.renderer.material.color = Color.gray;
		}
	}
	
	void OnMouseUp()
	{
		if (!GameManager.bNumber)
		{
			GameObject.Find("Answer_Player").GetComponent<TextMesh>().text += ReturnOperator();
			GameManager.bNumber = true;
		}
		if ( gameObject.name == "Clear" )
		{
			Clear();
		}
	}
	
	private string ReturnOperator()
	{
		if(gameObject.name == "Operator_Plus")
		{
			return sPlus;
		}
		else if(gameObject.name == "Operator_Minus")
		{
			return sMinus;
		}
		else if(gameObject.name == "Operator_Multiply")
		{
			return sMultiply;
		}
		else if(gameObject.name == "Operator_Divide")
		{
			return sDivide;
		}
		return null;
	}
	
	private void Clear()
	{
		GameObject.Find("Answer_Player").GetComponent<TextMesh>().text = "";
		NumManager.nClickedNumber1 = 0;
		NumManager.nClickedNumber2 = 0;
		NumManager.bPressed = false;
		NumManager.bCheck = false;
		GameManager.bNumber = true;
		
		GameObject goTemp = GameObject.Find("Group_Plane");
		NumManager[] oTemp = goTemp.transform.GetComponentsInChildren<NumManager>();
		
		for ( int n = 0; n < oTemp.Length; ++n )
		{
			oTemp[n].bPressMeBabyOneMoreTime = true;
		}
	}
}
