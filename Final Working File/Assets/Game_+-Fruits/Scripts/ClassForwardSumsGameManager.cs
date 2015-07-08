using UnityEngine;
using System.Collections;

public class ClassForwardSumsGameManager : MonoBehaviour 
{
	private bool m_bIsStartingLevel = true;
	
	public bool m_bHasCurrentLevelEndedCorrect = false;
	public bool m_bHasCurrentLevelEndedWrong = true;
	public int m_nCurrentAnswer = 0;
	
	public float m_nTimeLimit;
	private float m_fTimeRemaining;
	
	public bool m_bHardLevel = false;
	private bool m_bStarted = false;
	
	// Use this for initialization
	IEnumerator Start () 
	{
		yield return StartCoroutine( Countdown() );
		
		m_fTimeRemaining = m_nTimeLimit;
	}
	
	IEnumerator Countdown()
	{
		float countdown = 3.9999f;
		GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(false);
		GameObject.Find("TextCountdown").renderer.enabled = true;
		
		while(countdown > -0.025f)
		{
			yield return new WaitForEndOfFrame();
			if ( countdown > 1 )
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = Mathf.Floor(countdown).ToString("##");
			else
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = "GO!";
			countdown -= Time.deltaTime;
		}
		
		m_bStarted = true;
		GameObject.Find("TextCountdown").renderer.enabled = false;
		GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_bStarted )
		{
			m_fTimeRemaining = m_fTimeRemaining - Time.fixedDeltaTime;
			
			if(m_fTimeRemaining < 0.0f)
			{
				m_fTimeRemaining = 0.0f;
			}
	
			//GameObject.Find ("TimeRemaining").GetComponent<TextMesh>().text = m_fTimeRemaining.ToString("F2");
			GameObject.Find ("TimeRemaining").GetComponent<TextMesh>().text = m_fTimeRemaining.ToString("F0");
	
			if(m_bHasCurrentLevelEndedWrong == true || m_fTimeRemaining <= 0.0f)
			{
				if(m_bIsStartingLevel == false)
				{
					StartCoroutine(LevelEnder(false));
				}
				
				ClassStartingLevel oStartingLevel = new ClassStartingLevel();
				
				m_nCurrentAnswer = oStartingLevel.GenerateQuestion(m_bHardLevel);
				
				GameObject.Find("TextBox").GetComponent<TextMesh>().text = "";
				GameObject.Find("Sign").GetComponent<TextMesh>().text = "";
				
				GameObject.Find ("Button Minus").GetComponent<ClassButtonMinus>().m_bIsCurrentlyNegative = false;
				
				m_fTimeRemaining = m_nTimeLimit;
				
				GameObject.Find ("ProgressBarManager").GetComponent<ClassProgressionFruits>().nCorrectStreak = 0;
				
				m_bHasCurrentLevelEndedWrong = false;
				
				m_bIsStartingLevel = false;
			}
			
			if(m_bHasCurrentLevelEndedCorrect == true)
			{
				StartCoroutine(LevelEnder(true));
				
				ClassSubsequentLevel oSubsequentLevel = new ClassSubsequentLevel();
				
				m_nCurrentAnswer = oSubsequentLevel.GenerateQuestion(m_nCurrentAnswer, m_bHardLevel);
				
				GameObject.Find("TextBox").GetComponent<TextMesh>().text = "";
				GameObject.Find("Sign").GetComponent<TextMesh>().text = "";
				
				GameObject.Find ("Button Minus").GetComponent<ClassButtonMinus>().m_bIsCurrentlyNegative = false;
				
				m_fTimeRemaining = m_nTimeLimit;
				
				m_bHasCurrentLevelEndedCorrect = false;
			}
		}
	}
	
	IEnumerator LevelEnder(bool _bIsCorrect)
	{
		if(_bIsCorrect == true)
		{
			GameObject.Find("LastAnswer").GetComponent<MeshRenderer>().renderer.enabled = false;
			GameObject.Find("LastAnswerNum").GetComponent<TextMesh>().text = "";
			
			GameObject.Find("Sound_Correct").audio.Play();
			
			GameObject.Find ("Correct").GetComponent<MeshRenderer>().enabled = true;
		
			yield return new WaitForSeconds(0.5f);
			
			GameObject.Find ("Correct").GetComponent<MeshRenderer>().enabled = false;
			
			yield return null;
		}
		else
		{
			GameObject.Find("LastAnswer").GetComponent<MeshRenderer>().renderer.enabled = true;
			GameObject.Find("LastAnswerNum").GetComponent<TextMesh>().text = m_nCurrentAnswer.ToString();
			
			GameObject.Find("Sound_Wrong").audio.Play();
			
			GameObject.Find ("Wrong").GetComponent<MeshRenderer>().enabled = true;
		
			yield return new WaitForSeconds(0.5f);
			
			GameObject.Find ("Wrong").GetComponent<MeshRenderer>().enabled = false;
			
			yield return null;
		}
	}
	
}
