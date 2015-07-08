using UnityEngine;
using System.Collections;

public class NumManager : MonoBehaviour 
{
	public static bool 	bPressed 		= false;
	public static bool	bCheck			= false;
	public static int 	nClickedNumber1 = 0;
	public static int 	nClickedNumber2 = 0;
	public bool bPressMeBabyOneMoreTime = true;
	private Color cMyColor;

	// Use this for initialization
	void Start () 
	{
		cMyColor = gameObject.renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( GameManager.bNumber && bPressMeBabyOneMoreTime )
			gameObject.renderer.material.color = cMyColor;
		else
			gameObject.renderer.material.color = Color.gray;
	}
	
	void OnMouseUp()
	{
		if ( GameManager.bNumber && bPressMeBabyOneMoreTime )
		{
			if(bPressed == false)
			{
				nClickedNumber1 = ReturnNumber();
				bPressed = true;
			}
			else if(bPressed == true)
			{
				nClickedNumber2 = ReturnNumber();
				bCheck = true;
			}
			GameManager.bNumber = false;
			bPressMeBabyOneMoreTime = false;
			
			GameObject.Find("Answer_Player").GetComponent<TextMesh>().text += ReturnNumber();
		}
	}
	
	private int ReturnNumber()
	{
		if(gameObject.name == "Plane1")
		{
			return GameManager.anNumbers[0];
		}
		else if(gameObject.name == "Plane2")
		{
			return GameManager.anNumbers[1];
		}
		else if(gameObject.name == "Plane3")
		{
			return GameManager.anNumbers[2];
		}
		else if(gameObject.name == "Plane4")
		{
			return GameManager.anNumbers[3];
		}
		else if(gameObject.name == "Plane5")
		{
			return GameManager.anNumbers[4];
		}
		return 0;
	}
}
