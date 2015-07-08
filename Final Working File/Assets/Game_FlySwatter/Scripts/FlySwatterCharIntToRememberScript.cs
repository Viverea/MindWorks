using UnityEngine;
using System.Collections;

public class FlySwatterCharIntToRememberScript : MonoBehaviour 
{
	char[] m_arrcListOfAlphabets = 
	{'A', 'B', 'C', 'D', 'E',
	 'F', 'G', 'H', 'J', 'K',
	 'L', 'M', 'N', 'P', 'Q',
	 'R', 'S', 'T', 'U', 'V',
	 'W', 'X', 'Y', 'Z' };
	
	
	bool m_bIsEasyLevel;
	
	char m_cCharToRemember1;
	char m_cCharToRemember2;
	int	 m_nIntToRemember1;
	int  m_nIntToRemember2;
	
	public GameObject m_3dtCharToRemember1;
	public GameObject m_3dtCharToRemember2;
	public GameObject m_3dtIntToRemember1;
	public GameObject m_3dtIntToRemember2;
	
	Vector3 m_vOutPosition;
	
	// Use this for initialization
	void Start () 
	{
		m_vOutPosition = new Vector3(300.0f, 150.0f, 200.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void GenerateRandomCharInt()
	{
		//Alphabets
		int nRandomCharIndex1 = Random.Range(0, m_arrcListOfAlphabets.Length);
		int nRandomCharIndex2 = Random.Range(0, m_arrcListOfAlphabets.Length);
		
		while(nRandomCharIndex2 == nRandomCharIndex1)
		{
			nRandomCharIndex2 = Random.Range(0, m_arrcListOfAlphabets.Length);
		}
		
		m_cCharToRemember1 = m_arrcListOfAlphabets[nRandomCharIndex1];
		m_cCharToRemember2 = m_arrcListOfAlphabets[nRandomCharIndex2];
		
		//Intergers
		m_nIntToRemember1 = Random.Range(2, 10);
		m_nIntToRemember2 = Random.Range(2, 10);
		
		while(m_nIntToRemember2 == m_nIntToRemember1)
		{
			m_nIntToRemember2 = Random.Range(2, 10);
		}
		
		SetValuesInFliesManagerScript();
	}
	
	public void DisplayCharInt()
	{
		if(m_bIsEasyLevel)
		{
			m_3dtCharToRemember1.GetComponent<TextMesh>().text = m_cCharToRemember1.ToString();
			m_3dtCharToRemember1.renderer.enabled = true;
			m_3dtCharToRemember1.transform.GetChild(0).renderer.enabled = true;
			
			m_3dtIntToRemember1.GetComponent<TextMesh>().text = m_nIntToRemember1.ToString ();
			m_3dtIntToRemember1.renderer.enabled = true;
			m_3dtIntToRemember1.transform.GetChild(0).renderer.enabled = true;
		}
		else
		{
			m_3dtCharToRemember1.GetComponent<TextMesh>().text = m_cCharToRemember1.ToString();
			m_3dtCharToRemember1.renderer.enabled = true;
			m_3dtCharToRemember1.transform.GetChild(0).renderer.enabled = true;
			m_3dtCharToRemember2.GetComponent<TextMesh>().text = m_cCharToRemember2.ToString();
			m_3dtCharToRemember2.renderer.enabled = true;
			m_3dtCharToRemember2.transform.GetChild(0).renderer.enabled = true;
			
			m_3dtIntToRemember1.GetComponent<TextMesh>().text = m_nIntToRemember1.ToString ();
			m_3dtIntToRemember1.renderer.enabled = true;
			m_3dtIntToRemember1.transform.GetChild(0).renderer.enabled = true;
			m_3dtIntToRemember2.GetComponent<TextMesh>().text = m_nIntToRemember2.ToString ();
			m_3dtIntToRemember2.renderer.enabled = true;
			m_3dtIntToRemember2.transform.GetChild(0).renderer.enabled = true;
		}
	}
	
	public void RemoveRevealCharInt()
	{
		m_3dtCharToRemember1.renderer.enabled = false;
		m_3dtCharToRemember1.transform.GetChild(0).renderer.enabled = false;
		m_3dtCharToRemember2.renderer.enabled = false;
		m_3dtCharToRemember2.transform.GetChild(0).renderer.enabled = false;
		m_3dtIntToRemember1.renderer.enabled = false;
		m_3dtIntToRemember1.transform.GetChild(0).renderer.enabled = false;
		m_3dtIntToRemember2.renderer.enabled = false;
		m_3dtIntToRemember2.transform.GetChild(0).renderer.enabled = false;
	}
	
	public bool CompareInt(int _nCollidedInt)
	{
		if(m_bIsEasyLevel)
		{
			//Compare with only one
			if(_nCollidedInt == m_nIntToRemember1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			//Compare with both
			if(_nCollidedInt == m_nIntToRemember1 || _nCollidedInt == m_nIntToRemember2)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	
	public bool CompareChar(char _cCollidedChar)
	{
		if(m_bIsEasyLevel)
		{
			//Compare with only one
			if(_cCollidedChar == m_cCharToRemember1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			//Compare with both
			if(_cCollidedChar == m_cCharToRemember1 || _cCollidedChar == m_cCharToRemember2)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	
	public void SetDifficulty(bool _bIsEasyLevel)
	{
		m_bIsEasyLevel = _bIsEasyLevel;
	}
	
	public void SetValuesInFliesManagerScript()
	{
		this.gameObject.GetComponent<FlySwatterFliesManagerScript>().m_cCharToRemember1 = m_cCharToRemember1 ;
		this.gameObject.GetComponent<FlySwatterFliesManagerScript>().m_cCharToRemember2 = m_cCharToRemember2 ;
		
		this.gameObject.GetComponent<FlySwatterFliesManagerScript>().m_nIntToRemember1 = m_nIntToRemember1;
		this.gameObject.GetComponent<FlySwatterFliesManagerScript>().m_nIntToRemember2 = m_nIntToRemember2;
	}
	
	public void ShowAnswersPostGame()
	{
		GameObject.Find("3DTextCharIntValues").GetComponent<MeshRenderer>().renderer.enabled = true;
		GameObject.Find("3DTextChartIntHeader").GetComponent<MeshRenderer>().renderer.enabled = true;
		
		if(m_bIsEasyLevel == true)
		{
			GameObject.Find("3DTextCharIntValues").GetComponent<TextMesh>().text = 
				m_cCharToRemember1.ToString()+"  "+m_nIntToRemember1.ToString();
		}
		else
		{
			GameObject.Find("3DTextCharIntValues").GetComponent<TextMesh>().text = 
				m_cCharToRemember1.ToString()+"  "+m_nIntToRemember1.ToString()+"  "+m_cCharToRemember2.ToString()+"  "+m_nIntToRemember2.ToString();
		}
	}
	
}
