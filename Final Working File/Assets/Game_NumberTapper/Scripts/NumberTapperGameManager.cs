using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NumberTapperGameManager : MonoBehaviour 
{
	public int m_nCaseNumber = 0;
	public bool m_bHasStarted = false;
	
	public int m_nScore = 0;
	
	public float m_fTimeRemaining;
	public float m_nTimeLimit;

	public List<int> m_anNumberList = new List<int>();
	
	IEnumerator Start () 
	{
		GenerateLevel();
		
		yield return StartCoroutine(Countdown());
		
		m_fTimeRemaining = m_nTimeLimit;
	}
	
	IEnumerator Countdown()
	{
		float countdown = 3.9999f;
		
		//GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(false);
		
		for(int i = 0; i < GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons.Count; i++)
		{
			GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].renderer.enabled = false;
		
			GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers[i].renderer.enabled = false;
		}

		GameObject.Find("TextCountdown").renderer.enabled = true;
		
		while(countdown > -0.025f)
		{
			yield return new WaitForEndOfFrame();
			
			if ( countdown > 1 )
			{
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = Mathf.Floor(countdown).ToString("##");
			}
			else
			{
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = "GO!";
			}
			
			countdown -= Time.deltaTime;
		}
		
		m_bHasStarted = true;
		
		GameObject.Find("TextCountdown").renderer.enabled = false;
		
		//GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(true);
		
		for(int i = 0; i < GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons.Count; i++)
		{
			GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].renderer.enabled = true;
		
			GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers[i].renderer.enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bHasStarted == true)
		{
			m_fTimeRemaining = m_fTimeRemaining - Time.fixedDeltaTime;
			
			if(m_fTimeRemaining < 0.0f)
			{
				m_fTimeRemaining = 0.0f;
			}

			GameObject.Find ("TimeRemaining").GetComponent<TextMesh>().text = m_fTimeRemaining.ToString("F0");
			
			if(CheckAnswer() == true)
			{
				GameObject.Find("Sound_Correct").audio.Play();
				
				m_nScore = m_nScore + (10 * (int)m_fTimeRemaining);
				
				GameObject.Find("Score").GetComponent<TextMesh>().text = m_nScore.ToString();

				StartCoroutine(CorrectGameEnder());
				
				//GenerateLevel();

				m_bHasStarted = false;
			}		
			
			if(m_fTimeRemaining <= 0.0f)
			{
				GameObject.Find("Sound_Wrong").audio.Play();

				StartCoroutine(WrongGameEnder());
				
				//GenerateLevel();

				m_bHasStarted = false;
			}
		}
	}
	
	private void GenerateLevel()
	{
		m_fTimeRemaining = m_nTimeLimit;
		
		GameObject.Find ("TimeRemaining").GetComponent<TextMesh>().text = m_fTimeRemaining.ToString("F0");
		
		for(int i = 0; i < GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons.Count; i++)
		{
			GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].GetComponent<ClassButton>().m_bIsSelected = false;
			
			GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].GetComponent<ClassButton>().m_tCurrentTexture = GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].GetComponent<ClassButton>().m_tStartingTexture;
		}
		
		for(int i = 0; i < GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers.Count; i++)
		{
			int nNumber = Random.Range (0, 10);

			GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers[i].GetComponent<TextMesh>().text = nNumber.ToString();
		}
		
		if(GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers.Count != 0)
		{
			int nNumberIndex = Random.Range (0, GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers.Count);

			//m_nCaseNumber = GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers[nNumberIndex].GetComponent<TextMesh>().text;
			
			int.TryParse(GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers[nNumberIndex].GetComponent<TextMesh>().text, out m_nCaseNumber);
			
			//GameObject.Find ("CheckButton").GetComponent<CheckButton>().m_bCheckNumber = true;
			
			GameObject.Find ("NumberInstruction").GetComponent<TextMesh>().text = (m_nCaseNumber.ToString());
		}
	}
	
	private bool CheckAnswer()
	{
		//List <GameObject> m_agoCorrectButtons = new List<GameObject>();
		
		int nTotalCorrectButtonCount = 0;
		int nCurrentlySelectedButtons = 0;
		
		for(int i = 0; i < GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons.Count; i++)
		{
			if(m_nCaseNumber == GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].GetComponent<ClassButton>().nNumber)
			{
				//m_agoCorrectButtons.Add(GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i]);
			
				nTotalCorrectButtonCount++;
			}
		}
		
		for(int i = 0; i < GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons.Count; i++)
		{
			if(GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].GetComponent<ClassButton>().m_bIsSelected == true)
			{
				if(m_nCaseNumber == GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].GetComponent<ClassButton>().nNumber)
				{
					nCurrentlySelectedButtons++;
				}
				else
				{
					nCurrentlySelectedButtons--;
				}
			}
		}
		
		if(nCurrentlySelectedButtons == nTotalCorrectButtonCount)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	IEnumerator CorrectGameEnder()
	{
		//GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(false);
		
		for(int i = 0; i < GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons.Count; i++)
		{
			GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].renderer.enabled = true;
		
			GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers[i].renderer.enabled = true;
		}
		
		GameObject.Find ("Correct").GetComponent<MeshRenderer>().enabled = true;
		
		yield return new WaitForSeconds (1);
		
		GameObject.Find ("Correct").GetComponent<MeshRenderer>().enabled = false;
		
		//GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(true);
		
		for(int i = 0; i < GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons.Count; i++)
		{
			GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].renderer.enabled = true;
		
			GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers[i].renderer.enabled = true;
		}
		
		m_bHasStarted = true;
		
		GenerateLevel();
		
		yield return null;
	}
	
	IEnumerator WrongGameEnder()
	{
		//GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(false);
		
		for(int i = 0; i < GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons.Count; i++)
		{
			GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].renderer.enabled = true;
		
			GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers[i].renderer.enabled = true;
		}
		
		GameObject.Find ("Wrong").GetComponent<MeshRenderer>().enabled = true;
		
		yield return new WaitForSeconds (1);
		
		GameObject.Find ("Wrong").GetComponent<MeshRenderer>().enabled = false;
		
		//GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(true);
		
		for(int i = 0; i < GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons.Count; i++)
		{
			GameObject.Find("ButtonArray").GetComponent<ButtonArray>().m_agoButtons[i].renderer.enabled = true;
		
			GameObject.Find("NumberArray").GetComponent<NumberArray>().m_agoNumbers[i].renderer.enabled = true;
		}
		
		m_bHasStarted = true;
		
		GenerateLevel();
		
		yield return null;
	}
}
