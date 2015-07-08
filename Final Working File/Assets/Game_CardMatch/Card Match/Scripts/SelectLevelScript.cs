using UnityEngine;
using System.Collections;

public class SelectLevelScript : MonoBehaviour 
{
	public GameObject m_goLevel1Text;
	public GameObject m_goLevel2Text;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (0))
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit rchHit = new RaycastHit();
			
			if (Physics.Raycast (ray, out rchHit, 10)) 
			{
				if(rchHit.transform.gameObject == m_goLevel1Text)
				{
					Application.LoadLevel("Game_CardMatch_Easy");
				}
				else if (rchHit.transform.gameObject == m_goLevel2Text)
				{
					Application.LoadLevel("Game_CardMatch_Hard");
				}
	
			}
		
		}
	}
}
