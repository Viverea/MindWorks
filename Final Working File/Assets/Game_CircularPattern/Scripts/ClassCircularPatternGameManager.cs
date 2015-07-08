using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClassCircularPatternGameManager : MonoBehaviour 
{
	public bool m_bHasCurrentLevelEndedCorrect = false;
	public bool m_bHasCurrentLevelEndedWrong = true;
	
	public int m_nNumberOfSegmentsMarked = 0;
	
	public float m_nTimeLimit;
	private float m_fTimeRemaining;
	
	private bool m_bIsRotating = false;
	
	public int m_nScore = 0;
	
	public bool m_bHasStarted = false;
	
	// Use this for initialization
	IEnumerator Start () 
	{
		m_nNumberOfSegmentsMarked = 0;
		
		GeneratePattern();
		
		yield return StartCoroutine(Countdown());
		
		m_fTimeRemaining = m_nTimeLimit;
	}
	
	IEnumerator Countdown()
	{
		float countdown = 3.9999f;
		
		//GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(false);
		
		for(int i = 0; i < GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments.Count; i++)
		{
			GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments[i].renderer.enabled = false;
		}
		
		GameObject.Find ("LolipopStickActual").renderer.enabled = false;
		
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
		
		for(int i = 0; i < GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments.Count; i++)
		{
			GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments[i].renderer.enabled = true;
		}
		
		GameObject.Find ("LolipopStickActual").renderer.enabled = true;
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
			
			GameObject.Find ("TimeRemaining").GetComponent<TextMesh>().text = m_fTimeRemaining.ToString("##");
			
			bool bIsCorrect = CheckPattern();
			
			if(bIsCorrect == true)
			{
				m_nScore = m_nScore + (10 * (int)m_fTimeRemaining);
					
				GameObject.Find("Score").GetComponent<TextMesh>().text = m_nScore.ToString();
				
				m_fTimeRemaining = m_nTimeLimit;
				
				GameObject.Find("Sound_Correct").audio.Play();
				
				StartCoroutine(CorrectGameEnder());
				
				m_bHasStarted = false;
				
				//ResetAllSegments();
			}
			
			if(m_fTimeRemaining == 0.0f)
			{
				m_fTimeRemaining = m_nTimeLimit;
				
				GameObject.Find("Sound_Wrong").audio.Play();
				
				StartCoroutine(WrongGameEnder());
				
				m_bHasStarted = false;
				
				//ResetAllSegments();
			}
			
			/*if(m_bHasCurrentLevelEndedWrong == true || m_fTimeRemaining <= 0.0f)
			{
				ResetAllSegments();
				
				GeneratePattern();
	
				m_fTimeRemaining = m_nTimeLimit;
	
				m_bHasCurrentLevelEndedWrong = false;
			}
			
			if(m_bHasCurrentLevelEndedCorrect == true)
			{
				ResetAllSegments();
	
				GeneratePattern();
	
				m_fTimeRemaining = m_nTimeLimit;
	
				m_bHasCurrentLevelEndedCorrect = false;	
			}*/
		}
		
		ShowAnswer();
	}
	
	public void ResetAllSegments()
	{
		List<GameObject> agoGuideSegments = GameObject.Find ("LolipopGuide").GetComponent<ClassSegmentManager>().m_agoSegments;
		List<GameObject> agoActualSegments = GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments;
		
		for(int i = 0; i < agoGuideSegments.Count; i++)
		{
			agoGuideSegments[i].GetComponent<ClassSegment>().m_bMarked = false;
			agoActualSegments[i].GetComponent<ClassSegment>().m_bMarked = false;
			
			agoGuideSegments[i].GetComponent<ClassSegment>().m_bSelected = false;
			agoActualSegments[i].GetComponent<ClassSegment>().m_bSelected = false;
		}
	}

	public void GeneratePattern()
	{
		List<GameObject> agoGuideSegments = GameObject.Find ("LolipopGuide").GetComponent<ClassSegmentManager>().m_agoSegments;
		List<GameObject> agoActualSegments = GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments;
		
		int nFirstIndex = Random.Range(0, agoGuideSegments.Count);
		
		agoGuideSegments[nFirstIndex].GetComponent<ClassSegment>().m_bMarked = true;
		agoActualSegments[nFirstIndex].GetComponent<ClassSegment>().m_bMarked = true;
		
		GameObject.Find ("GameManager").GetComponent<ClassCircularPatternGameManager>().m_nNumberOfSegmentsMarked ++;
		
		for(int i = 0; i < agoGuideSegments.Count; i++)
		{
			if(agoGuideSegments[i].GetComponent<ClassSegment>().m_bMarked == false)
			{
				int nSelectionBasis = Random.Range(0, 3);
				
				switch(nSelectionBasis)
				{
				case 0:
					
					// do nothing
					
					break;
					
				case 1:
					
					agoGuideSegments[i].GetComponent<ClassSegment>().m_bMarked = true;
					agoActualSegments[i].GetComponent<ClassSegment>().m_bMarked = true;
					
					GameObject.Find ("GameManager").GetComponent<ClassCircularPatternGameManager>().m_nNumberOfSegmentsMarked ++;
					
					break;
					
				default:
					
					//do nothing
					
					break;
				}
			}
		}
		
		float fRotationAngle = 0.0f;
		int nRotationIndex = Random.Range(0, 6);
		
		switch(nRotationIndex)
		{
		case 0:
			
			fRotationAngle = 0.0f;
			
			break;
			
		case 1:
			
			fRotationAngle = 60.0f;
			
			break;
			
		case 2:
			
			fRotationAngle = 120.0f;
			
			break;
			
		case 3:
			
			fRotationAngle = 180.0f;
			
			break;
			
		case 4:
			
			fRotationAngle = 240.0f;
			
			break;
			
		case 5:
			
			fRotationAngle = 300.0f;
			
			break;
			
		default:
			
			fRotationAngle = 0.0f;
			
			break;
		}
		
		GameObject.Find ("LolipopActual").transform.rotation = Quaternion.Euler(0, 0, fRotationAngle);
	}
	
	public bool CheckPattern()
	{
		int nCurrentCorrectCount = 0;
		int nActualCorrectCount = 0;
		
		for(int i = 0; i < GameObject.Find ("LolipopGuide").GetComponent<ClassSegmentManager>().m_agoSegments.Count; i++)
		{
			if(GameObject.Find ("LolipopGuide").GetComponent<ClassSegmentManager>().m_agoSegments[i].GetComponent<ClassSegment>().m_bMarked == true)
			{
				nActualCorrectCount++;
			}
		}
		
		for(int i = 0; i < GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments.Count; i++)
		{
			if(GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments[i].GetComponent<ClassSegment>().m_bSelected == true)
			{
				if(GameObject.Find ("LolipopGuide").GetComponent<ClassSegmentManager>().m_agoSegments[i].GetComponent<ClassSegment>().m_bMarked == true)
				{
					nCurrentCorrectCount++;
				}
			}
		}
		
		if(nCurrentCorrectCount == nActualCorrectCount)
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
		
		for(int i = 0; i < GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments.Count; i++)
		{
			GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments[i].renderer.enabled = true;
		}
		
		GameObject.Find ("Correct").GetComponent<MeshRenderer>().enabled = true;
		
		yield return new WaitForSeconds (1);
		
		GameObject.Find ("Correct").GetComponent<MeshRenderer>().enabled = false;
		
		//GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(true);
		
		for(int i = 0; i < GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments.Count; i++)
		{
			GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments[i].renderer.enabled = true;
		}
		
		m_bHasStarted = true;
		
		ResetAllSegments();
		
		GeneratePattern();

		yield return null;
	}
	
	IEnumerator WrongGameEnder()
	{
		//GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(false);
		
		for(int i = 0; i < GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments.Count; i++)
		{
			GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments[i].renderer.enabled = true;
		}
		
		GameObject.Find ("Wrong").GetComponent<MeshRenderer>().enabled = true;
		
		yield return new WaitForSeconds (1);
		
		GameObject.Find ("Wrong").GetComponent<MeshRenderer>().enabled = false;
		
		//GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(true);
		
		for(int i = 0; i < GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments.Count; i++)
		{
			GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments[i].renderer.enabled = true;
		}
		
		m_bHasStarted = true;
		
		ResetAllSegments();
		
		GeneratePattern();

		yield return null;
	}
	
	public void ShowAnswer()
	{
		if(GameObject.Find ("Wrong").GetComponent<MeshRenderer>().enabled == true)
		{
			for(int i = 0; i < GameObject.Find ("LolipopGuide").GetComponent<ClassSegmentManager>().m_agoSegments.Count; i++)
			{
				if(GameObject.Find ("LolipopGuide").GetComponent<ClassSegmentManager>().m_agoSegments[i].GetComponent<ClassSegment>().m_bMarked == true)
				{
					GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments[i].GetComponent<ClassSegment>().m_bSelected = true;
				}
				else
				{
					GameObject.Find ("LolipopActual").GetComponent<ClassSegmentManager>().m_agoSegments[i].GetComponent<ClassSegment>().m_bSelected = false;
				}
			}
		}
	}
}
