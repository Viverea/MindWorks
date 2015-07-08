using UnityEngine;
using System.Collections;

public class GenerateCardScriptLevel1 : MonoBehaviour 
{
	private GameObject[] m_arrgoCard;
	public  GameObject 	 m_goParentObject;
	public  GameObject 	 m_goShadowPlane;
	
	private Vector3 	 m_vCardLocationTaken;
	private GameObject 	 m_goCardTaken;
	
	//W X H - 4 X 3
	private Vector3[] 	 m_vLocationOfCards = 
	{
		new Vector3 (-59,  -5.0f, 20), new Vector3(1,  -5.0f, 20), 	new Vector3(61,  -5.0f, 20), 	new Vector3(121, -5.0f, 20),
		new Vector3 (-59, -5.0f, -40), new Vector3(1, -5.0f, -40), 	new Vector3(61, -5.0f, -40), 	new Vector3(121, -5.0f, -40),
		new Vector3 (-59, -5.0f, -100), new Vector3(1, -5.0f, -100), 	new Vector3(61, -5.0f, -100),	new Vector3(121, -5.0f, -100)
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
