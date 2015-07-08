using UnityEngine;
using System.Collections;

public class FlySwatterScoringScript : MonoBehaviour 
{
	int m_nConsecutiveCorrects = 0;
	float m_nScoreMultiplier = 1;
	float m_fTimeSinceLastPress = 0.0f;
	
	int m_nSecondsRemaining = 0;
	
	int m_nCurrentScore = 0;
	int m_nBaseScore = 10;

	bool m_bShrinkMultiplier = false;
	
	public GameObject m_3dtMultiplierText;
	public GameObject m_3dtMultiplierX;
	public GameObject m_3dtMultiplierHeaderText;
	public GameObject m_3dtScoreText;
	public GameObject m_3dtScoreHeaderText;
	public GameObject m_3dtScoreSeconds;

	// Use this for initialization
	void Start () 
	{
		//m_3dtMultiplierText.GetComponent<TextMesh>().text = m_nScoreMultiplier.ToString() ;
		m_3dtScoreText.GetComponent<TextMesh>().text = m_nCurrentScore.ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_fTimeSinceLastPress += Time.deltaTime;
		
		if(m_bShrinkMultiplier)
		{
			ShrinkMultiplier();
		}
		
	}
	
	public void AddScore()
	{
		//Adding to score
		float fScore = (float) m_nBaseScore;
		//fScore *= (float) m_nScoreMultiplier;
		
		if(m_fTimeSinceLastPress <= 0.5f)
		{
			fScore += 10.0f;
		}
		else if (m_fTimeSinceLastPress > 0.5f && m_fTimeSinceLastPress <= 1.0f)
		{
			//fScore += 10.0f;
		}
		
		m_nCurrentScore += (int) fScore;
		
		//Set variables
		m_nConsecutiveCorrects++;
		m_fTimeSinceLastPress = 0.0f;
		
		UpdateScoreAndMultiplier();
	}
	
	public void DeductScore()
	{
		m_nCurrentScore -= GameObject.Find("SceneManager").GetComponent<FSv2MainScript>().m_nLevelNumber * 10;
		if(m_nCurrentScore < 0) m_nCurrentScore = 0;
		
	}
	
	public void ResetVars()
	{
		m_nConsecutiveCorrects = 0;
		m_fTimeSinceLastPress = 0.0f;
		//m_nScoreMultiplier = 1;
		
		UpdateScoreAndMultiplier();
	}
	
	void UpdateScoreAndMultiplier()
	{
		if(m_nConsecutiveCorrects == 2)
		{
			m_3dtMultiplierText.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
			m_3dtMultiplierX.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
			m_bShrinkMultiplier = true;
			//m_nScoreMultiplier = 2;
		}
		else if (m_nConsecutiveCorrects == 4)
		{
			m_3dtMultiplierText.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
			m_3dtMultiplierX.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
			m_bShrinkMultiplier = true;
			//m_nScoreMultiplier = 3;
		}
		else if (m_nConsecutiveCorrects == 8)
		{
			m_3dtMultiplierText.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
			m_3dtMultiplierX.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
			m_bShrinkMultiplier = true;
			//m_nScoreMultiplier = 4;
		}
		
		//m_3dtMultiplierText.GetComponent<TextMesh>().text = m_nScoreMultiplier.ToString();
		m_3dtScoreText.GetComponent<TextMesh>().text = m_nCurrentScore.ToString();
		
	}
	
	public void UpdateFinalScore()
	{
		m_nCurrentScore += (int)((m_nSecondsRemaining / this.gameObject.GetComponent<FlySwatterTimerScript>().GetMaxTime()) * Time.deltaTime);
		
		m_3dtScoreText.GetComponent<TextMesh>().text = m_nCurrentScore.ToString();
	}
	
	public int GetScore()
	{
		return m_nCurrentScore;
	}
	
	public void SetSecondsRemaining()
	{
		m_nSecondsRemaining = int.Parse (m_3dtScoreText.GetComponent<TextMesh>().text);
	}
	
	public void DisplayText()
	{	
		m_3dtMultiplierText.renderer.enabled = true;
		m_3dtMultiplierX.renderer.enabled = true;
		m_3dtMultiplierHeaderText.renderer.enabled = true;
		
		m_3dtScoreText.renderer.enabled = true;
		m_3dtScoreHeaderText.renderer.enabled = true;
		m_3dtScoreSeconds.renderer.enabled = true;
	}
	
	public void HideText()
	{
		m_3dtMultiplierText.renderer.enabled = false;
		m_3dtScoreText.renderer.enabled = false;
		m_3dtScoreSeconds.renderer.enabled = false;
	}
	
	void ShrinkMultiplier()
	{
		m_3dtMultiplierText.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
		m_3dtMultiplierX.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
		
		if(m_3dtMultiplierText.transform.localScale == new Vector3(1.0f , 1.0f ,1.0f))
		{
			m_bShrinkMultiplier = false;
		}
	}
}
