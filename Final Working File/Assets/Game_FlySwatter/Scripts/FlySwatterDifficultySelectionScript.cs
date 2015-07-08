using UnityEngine;
using System.Collections;

public class FlySwatterDifficultySelectionScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray rRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rchHit = new RaycastHit();
			
			if(Physics.Raycast(rRay, out rchHit, 100))
			{
				//Input numbers
				if(rchHit.transform.gameObject == GameObject.Find ("3DTextDifficultyEasy"))
				{					
					Application.LoadLevel("Game_Flyswatter_Easy");
					
				}
				else if (rchHit.transform.gameObject == GameObject.Find ("3DTextDifficultyHard"))
				{
					Application.LoadLevel("Game_Flyswatter_Hard");
				}
				
			}
		}
	}
}
