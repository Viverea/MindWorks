using UnityEngine;
using System.Collections;

public class GenerateCardScriptLevel2 : MonoBehaviour 
{
	private GameObject[] m_arrgoCard;
	public  GameObject 	 m_goParentObject;
	public  GameObject 	 m_goShadowPlane;
	
	private Vector3 	 m_vCardLocationTaken;
	private GameObject 	 m_goCardTaken;
	
	/*
	private Vector3[] 	 m_vLocationOfCards = 
	{
		new Vector3	(-75, -5.0f , 10),	new Vector3 (-34,  -5.0f, 10), new Vector3(7,  -5.0f, 10), 	new Vector3(48,  -5.0f, 10), 	new Vector3(89, -5.0f, 10),	 new Vector3(130, -5.0f , 10),
		new Vector3	(-75, -5.0f , -45), new Vector3 (-34, -5.0f, -45), new Vector3(7, -5.0f, -45), 	new Vector3(48, -5.0f, -45), 	new Vector3(89, -5.0f, -45), new Vector3(130, -5.0f , -45),	
		new Vector3	(-75, -5.0f , -100), new Vector3 (-34, -5.0f, -100), new Vector3(7, -5.0f, -100), 	new Vector3(48, -5.0f, -100),	new Vector3(89, -5.0f, -100), new Vector3(130, -5.0f , -100)
	};
	*/
	
	//Formation - 5 + 5 + 5 + 3
	private Vector3[] 	 m_vLocationOfCards = 
	{
		new Vector3	(-65, -5.0f , 25),	new Vector3 (-16.25f,  -5.0f, 25),   new Vector3(32.5f,  -5.0f, 25), 	new Vector3(81.25f,  -5.0f, 25), 	new Vector3(130, -5.0f, 25),
		new Vector3	(-65, -5.0f , -18),	new Vector3 (-16.25f,  -5.0f, -18),  new Vector3(32.5f,  -5.0f, -18), 	new Vector3(81.25f,  -5.0f, -18), 	new Vector3(130, -5.0f, -18),
		new Vector3	(-65, -5.0f , -62),	new Vector3 (-16.25f,  -5.0f, -62),  new Vector3(32.5f,  -5.0f, -62), 	new Vector3(81.25f,  -5.0f, -62), 	new Vector3(130, -5.0f, -62),
										new Vector3 (-16.25f,  -5.0f, -105), new Vector3(32.5f,  -5.0f, -105), new Vector3(81.25f,  -5.0f, -105)
	};

	// Use this for initialization
	public void Generate () 
	{	
	 	m_arrgoCard = GameObject.FindGameObjectsWithTag("Cards");
		
		// Randomly reorder the array
		for ( int i = 0; i <= m_arrgoCard.Length - 1; i++)
		{
			int goTempCardIndex = Random.Range (i, m_arrgoCard.Length);
			
			// Swap position of randomed card in the array to i
			m_goCardTaken = m_arrgoCard [i];
			
			m_arrgoCard [i] = m_arrgoCard [goTempCardIndex];
			
			m_arrgoCard [goTempCardIndex] = m_goCardTaken;
		}
		
		//Random location
		for ( int i=0; i <= m_vLocationOfCards.Length-1; i++)
		{
			int thisCardLocation = Random.Range (i, m_vLocationOfCards.Length);
				
			m_vCardLocationTaken = m_vLocationOfCards [i];
			
			m_vLocationOfCards [i] = m_vLocationOfCards [thisCardLocation];
			
			m_vLocationOfCards [thisCardLocation] = m_vCardLocationTaken;
			
			GameObject theCard = (GameObject) Instantiate (m_arrgoCard [i], new Vector3(0,0,0), Quaternion.identity);
		
			GameObject theShadow = (GameObject) Instantiate (m_goShadowPlane, new Vector3(0,-0.5f,0), Quaternion.identity);
			
			GameObject theParent  = (GameObject) Instantiate (m_goParentObject, new Vector3(0,0,0) , Quaternion.identity);
			
			theCard.transform.parent = theParent.transform;
			
			theShadow.transform.parent = theParent.transform;
			
			theParent.transform.position = m_vLocationOfCards [i];
			
			theCard.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
		}
	
		Destroy (GameObject.Find("Cards"), 0.1f);
		
		
	}
}
