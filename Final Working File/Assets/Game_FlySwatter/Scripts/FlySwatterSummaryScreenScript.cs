using UnityEngine;
using System.Collections;

public class FlySwatterSummaryScreenScript : MonoBehaviour 
{
	public GameObject m_goScoreHeaderPrefab;
	public GameObject m_goScorePrefab;
	public GameObject m_goTimerHeaderPrefab;
	public GameObject m_goTimerPrefab;
	
	public GameObject m_goRetryLevelButtonPrefab;
	public GameObject m_goRetryLevelTextPrefab;
	public GameObject m_goSelectDifficultyButtonPrefab;
	public GameObject m_goSelectDifficultyTextPrefab;
	
	GameObject m_goScoreHeader;
	GameObject m_goScore;
	GameObject m_goTimerHeader;
	GameObject m_goTimer;
	
	GameObject m_goRetryLevelButton;
	GameObject m_goRetryLevelText;
	GameObject m_goSelectDifficultyButton;
	GameObject m_goSelectDifficultyText;
	
	bool m_bHasWon = false;
	public bool m_bHasFadedIn = false;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public void InstantiateTextsAndButtons(bool _bHasWon)
	{
		m_goScoreHeader = (GameObject) Instantiate(m_goScoreHeaderPrefab);
		m_goScore 		= (GameObject) Instantiate(m_goScorePrefab);
		
		if(!_bHasWon)
		{	
			m_goRetryLevelButton 	   = (GameObject) Instantiate(m_goRetryLevelButtonPrefab);
			m_goRetryLevelText		   = (GameObject) Instantiate(m_goRetryLevelTextPrefab);
			m_goSelectDifficultyButton = (GameObject) Instantiate(m_goSelectDifficultyButtonPrefab);
			m_goSelectDifficultyText   = (GameObject) Instantiate(m_goSelectDifficultyTextPrefab);
		}
		else
		{
			m_goTimerHeader = (GameObject) Instantiate(m_goTimerHeaderPrefab);
			m_goTimer 		= (GameObject) Instantiate(m_goTimerPrefab);
		}

		m_bHasWon = _bHasWon;
		
		
	}
	
	public void FadeInSummaryScreenElements()
	{
		if(!m_bHasFadedIn)
		{
			Color cAlphaIncrement = new Color(0.0f, 0.0f, 0.0f, 0.015f);
			float fTargetAlpha = 1.0f;
			
			//Instantiated texts/buttons
			GameObject[] arrgoListOfEndLevelTexts = GameObject.FindGameObjectsWithTag("FSEndLevelText");
			GameObject[] arrgoListOfEndLevelButtons = GameObject.FindGameObjectsWithTag("FSEndLevelButton");
			
			for(int i = 0; i < arrgoListOfEndLevelTexts.Length; i++)
			{
				arrgoListOfEndLevelTexts[i].GetComponent<TextMesh>().color += cAlphaIncrement;
			}
			
			for(int i = 0; i < arrgoListOfEndLevelButtons.Length; i++)
			{
				arrgoListOfEndLevelButtons[i].renderer.material.color += cAlphaIncrement;
			}
			
			//Reuse of headers
			GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().color += cAlphaIncrement;
			GameObject.Find("3DTextHeader3").GetComponent<TextMesh>().color += cAlphaIncrement;
			
			if(m_bHasWon)
			{
				GameObject.Find("3DTextHeader2").GetComponent<TextMesh>().color += cAlphaIncrement;
			}
			
			if(GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().color.a >= fTargetAlpha)
			{
				m_bHasFadedIn = true;
			}
		}
		
	}
	
	public void SetElementsMaxAlpha()
	{
		Color fTargetColor = GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().color;
		fTargetColor.a = 1.0f;
		
		//Instantiated texts/buttons
		GameObject[] arrgoListOfEndLevelTexts = GameObject.FindGameObjectsWithTag("FSEndLevelText");
		GameObject[] arrgoListOfEndLevelButtons = GameObject.FindGameObjectsWithTag("FSEndLevelButton");
		
		for(int i = 0; i < arrgoListOfEndLevelTexts.Length; i++)
		{
			if(arrgoListOfEndLevelTexts[i] == m_goRetryLevelText)
			{
				m_goRetryLevelText.GetComponent<TextMesh>().color += new Color(0.0f, 0.0f, 0.0f ,1.0f);
			}
			else if (arrgoListOfEndLevelTexts[i] == m_goSelectDifficultyText)
			{
				m_goSelectDifficultyText.GetComponent<TextMesh>().color += new Color(0.0f, 0.0f, 0.0f ,1.0f);
			}
			else
			{
				arrgoListOfEndLevelTexts[i].GetComponent<TextMesh>().color = fTargetColor;
			}
		}
		
		for(int i = 0; i < arrgoListOfEndLevelButtons.Length; i++)
		{
			arrgoListOfEndLevelButtons[i].renderer.material.color = fTargetColor;
		}
		
		//Reuse of headers
		GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().color = fTargetColor;
		GameObject.Find("3DTextHeader3").GetComponent<TextMesh>().color = fTargetColor;
		
		if(m_bHasWon)
		{
			GameObject.Find("3DTextHeader2").GetComponent<TextMesh>().color = fTargetColor;
		}
		
		m_bHasFadedIn = true;
		
	}
	
	public void ResetVariables()
	{
		m_bHasFadedIn = false;
		m_bHasWon = false;
	}
	
	public void SetSummaryScreen(bool _bHasWon)
	{
		if(_bHasWon)
		{
			GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().text = "Game Completed!";
		}
		else
		{
			GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().text = "Level Failed!";
		}
		
		GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().fontSize = 175;
		GameObject.Find("3DTextHeader1").renderer.enabled = true;
		
		GameObject.Find("3DTextHeader3").transform.localScale = new Vector3(18.0f, 1.0f, 1.0f);
		GameObject.Find("3DTextHeader3").transform.position = GameObject.Find("3DTextHeader1").transform.position;
		GameObject.Find("3DTextHeader3").transform.position -= new Vector3(0.0f, 5.5f, 0.0f);
		GameObject.Find("3DTextHeader3").renderer.enabled = true;
		
		//Change header alpha value to 0 to allow for fade in
		Color cHeader1Color = GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().color;
		Color cHeader3Color = GameObject.Find("3DTextHeader3").GetComponent<TextMesh>().color;
		
		cHeader1Color.a = 0.0f;
		cHeader3Color.a = 0.0f;
		
		GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().color = cHeader1Color;
		GameObject.Find("3DTextHeader3").GetComponent<TextMesh>().color = cHeader3Color;
		
		this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().InstantiateTextsAndButtons(false);
		
		GameObject.Find("3DTextSummaryScore(Clone)").GetComponent<TextMesh>().text = this.gameObject.GetComponent<FlySwatterScoringScript>().GetScore().ToString ();
		GameObject.Find("3DTextSummaryScore(Clone)").transform.position += new Vector3(0.0f, 45.0f, 0.0f);
		GameObject.Find("3DTextSummaryScoreHeader(Clone)").transform.position += new Vector3(0.0f, 45.0f, 0.0f);
	}
		
}
