using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlySwatterFliesManagerScript : MonoBehaviour 
{
	public GameObject[] m_arrgoAlphabetPrefabs;
	public GameObject[] m_arrgoIntegerPrefabs;
	
	private bool m_bIsEasyLevel = false;
	
	public char m_cCharToRemember1;
	public char m_cCharToRemember2;
	public int m_nIntToRemember1;
	public int m_nIntToRemember2;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void SpawnFlies(int _nLevelNumber)
	{		
		//Spawn alphabets first 
		List<GameObject> listAlphabetPrefabsCreated = new List<GameObject>();
		
		//Make first one/two char the ones to avoid
		//Base 3 alphabets + (levelnumber - 1)	
		for(int i = 0; i < (3 + _nLevelNumber - 1); i++)
		{
			//Max 10 alphabets on screen at anytime, 0 - 9
			if( i < 10)
			{
				if(m_bIsEasyLevel && i == 0)
				{
					for(int j = 0; j < m_arrgoAlphabetPrefabs.Length; j++)
					{
						if(m_arrgoAlphabetPrefabs[j].name[m_arrgoAlphabetPrefabs[j].name.Length - 1] == m_cCharToRemember1)
						{
							listAlphabetPrefabsCreated.Add(m_arrgoAlphabetPrefabs[j]);
							Instantiate(m_arrgoAlphabetPrefabs[j]);
							
							break;	
						}
					}
				}
				else if (!m_bIsEasyLevel && (i == 0 ||  i == 1))
				{
					for(int j = 0; j < m_arrgoAlphabetPrefabs.Length; j++)
					{
						if(i == 0)
						{
							if(m_arrgoAlphabetPrefabs[j].name[m_arrgoAlphabetPrefabs[j].name.Length - 1] == m_cCharToRemember1)
							{
								listAlphabetPrefabsCreated.Add(m_arrgoAlphabetPrefabs[j]);
								Instantiate(m_arrgoAlphabetPrefabs[j]);
								
								break;	
							}
						}
						else
						{
							if(m_arrgoAlphabetPrefabs[j].name[m_arrgoAlphabetPrefabs[j].name.Length - 1] == m_cCharToRemember2)
							{
								listAlphabetPrefabsCreated.Add(m_arrgoAlphabetPrefabs[j]);
								Instantiate(m_arrgoAlphabetPrefabs[j]);
								
								break;	
							}
						}
					}
				}
				else
				{
					bool bIsDifferent = false;
					int nRandomIndex = 0;
					
					while(!bIsDifferent)
					{
						nRandomIndex = Random.Range (0, m_arrgoAlphabetPrefabs.Length);
						
						for( int j = 0; j < listAlphabetPrefabsCreated.Count; j++)
						{
							if(m_arrgoAlphabetPrefabs[nRandomIndex] == listAlphabetPrefabsCreated[j])
							{
								bIsDifferent = false;
								break;
							}
							
							if( j == listAlphabetPrefabsCreated.Count - 1)
							{
								bIsDifferent = true;
							}
						}
				
					}
					
					if(bIsDifferent)
					{
						listAlphabetPrefabsCreated.Add(m_arrgoAlphabetPrefabs[nRandomIndex]);
						
						Instantiate(m_arrgoAlphabetPrefabs[nRandomIndex]);
					}
				}
			}
		}
		
		//Spawn numbers next
		
		List<GameObject> listIntPrefabsCreated = new List<GameObject>();
		
		for(int i = 0; i < (3 + _nLevelNumber - 1); i++)
		{
			//Max 8 numbers available, 0 - 7
			if( i < 8)
			{
				if(m_bIsEasyLevel && i == 0)
				{
					for(int j = 0; j < m_arrgoIntegerPrefabs.Length; j++)
					{
						if(int.Parse(m_arrgoIntegerPrefabs[j].GetComponent<TextMesh>().text) == m_nIntToRemember1)
						{
							listIntPrefabsCreated.Add(m_arrgoIntegerPrefabs[j]);
							Instantiate(m_arrgoIntegerPrefabs[j]);
							
							break;
						}
					}
				}
				else if (!m_bIsEasyLevel && (i == 0 ||  i == 1))
				{
					for(int j = 0; j < m_arrgoIntegerPrefabs.Length; j++)
					{
						if(i == 0)
						{
							if(int.Parse(m_arrgoIntegerPrefabs[j].GetComponent<TextMesh>().text) == m_nIntToRemember1)
							{
								listIntPrefabsCreated.Add(m_arrgoIntegerPrefabs[j]);
								Instantiate(m_arrgoIntegerPrefabs[j]);
								
								break;
							}
						}
						else
						{
							if(int.Parse(m_arrgoIntegerPrefabs[j].GetComponent<TextMesh>().text) == m_nIntToRemember2)
							{
								listIntPrefabsCreated.Add(m_arrgoIntegerPrefabs[j]);
								Instantiate(m_arrgoIntegerPrefabs[j]);
								
								break;
							}
						}
					}
				}
				else
				{
					bool bIsDifferent = false;
					int nRandomIndex = 0;
					
					while(!bIsDifferent)
					{
						nRandomIndex = Random.Range (0, m_arrgoIntegerPrefabs.Length);
						
						for( int j = 0; j < listIntPrefabsCreated.Count; j++)
						{
							if(m_arrgoIntegerPrefabs[nRandomIndex] == listIntPrefabsCreated[j])
							{
								bIsDifferent = false;
								break;
							}
							
							if( j == listIntPrefabsCreated.Count - 1)
							{
								bIsDifferent = true;
							}
						}
				
					}
					
					if(bIsDifferent)
					{
						listIntPrefabsCreated.Add(m_arrgoIntegerPrefabs[nRandomIndex]);
						
						Instantiate(m_arrgoIntegerPrefabs[nRandomIndex]);
					}
				}
			}
		}
		
		ScatterAndStartMovement();
		ScaleFlies(_nLevelNumber);
		ChangeVelocity(this.gameObject.GetComponent<FlySwatterGlobalVarScript>().m_fFlyFlightVelocity);
	}
	
	public void DestroyFlies()
	{
		GameObject[] arrgoFliesPrefabs = GameObject.FindGameObjectsWithTag("Fly");
		
		for(int i = 0; i < arrgoFliesPrefabs.Length; i++)
		{
			Destroy(arrgoFliesPrefabs[i]);
		}
	}
	
	void ScatterAndStartMovement()
	{
		float fXMin = this.gameObject.GetComponent<FlySwatterGlobalVarScript>().m_fXMin;
		float fXMax = this.gameObject.GetComponent<FlySwatterGlobalVarScript>().m_fXMax;
		float fYMin = this.gameObject.GetComponent<FlySwatterGlobalVarScript>().m_fYMin;
		float fYMax = this.gameObject.GetComponent<FlySwatterGlobalVarScript>().m_fYMax;
		
		float fOffset = 35.0f;
		float fMaxZCoord = 60.0f; //To set appropriate overlapping of flies
		
		GameObject[] arrgoFliesPrefabs = GameObject.FindGameObjectsWithTag("Fly");
		
		for(int i = 0; i < arrgoFliesPrefabs.Length; i++)
		{
			Vector3 vRandPos = new Vector3(Random.Range(fXMin + fOffset, fXMax - fOffset), Random.Range (fYMin + fOffset, fYMax - fOffset), fMaxZCoord - 2.0f*i);
			arrgoFliesPrefabs[i].transform.position = vRandPos;
			arrgoFliesPrefabs[i].GetComponent<FlySwatterFlyScript>().StartFlying();
		}
		
	}
	
	void ScaleFlies(int _nLevelNumber)
	{
		GameObject[] arrgoFliesPrefabs = GameObject.FindGameObjectsWithTag("Fly");
		float fScaleValue = 2.0f - ( (_nLevelNumber-1) * 0.025f);
		Vector3 vScaleVector = new Vector3(fScaleValue,fScaleValue,1.0f);
		
		for(int i = 0; i < arrgoFliesPrefabs.Length; i++)
		{
			arrgoFliesPrefabs[i].transform.localScale = vScaleVector;
		}
	}
	
	public void SetDifficulty(bool _bIsEasyLevel)
	{
		m_bIsEasyLevel = _bIsEasyLevel;
	}
	
	public void ChangeVelocity(float _fVelocity)
	{
		GameObject[] arrgoFliesPrefabs = GameObject.FindGameObjectsWithTag("Fly");
		
		for(int i = 0; i < arrgoFliesPrefabs.Length; i++)
		{
			arrgoFliesPrefabs[i].GetComponent<FlySwatterFlyScript>().SetVelocity(_fVelocity);
		}
			
	}

}
