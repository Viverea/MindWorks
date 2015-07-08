using UnityEngine;
using System.Collections;

public class ReturnButtonScript : MonoBehaviour 
{
	private GameObject m_goButton;
	
	// Use this for initialization
	void Start () 
	{
		m_goButton = GameObject.Find ("ReturnButton");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (0))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit rchHit = new RaycastHit();
			
			if (Physics.Raycast (ray, out rchHit, 100)) 
			{
				if(rchHit.transform.gameObject == m_goButton)
				{
					if(Application.loadedLevelName == "Game_CardMatch_Select")
					{
						Application.LoadLevel("Menu_Selection");
					}	
					Application.LoadLevel ("Game_CardMatch_Select");
				}
	
			}
		
		}
	}
}
