using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClassCloudSortManager : MonoBehaviour 
{
	public int m_nMaxNumbers = 10;
	
	public float m_nTimeLimit;
	public float m_fTimeRemaining;
	
	private bool m_bTimeWarning1 = false;
	private bool m_bTimeWarning2 = false;
	private bool m_bTimeWarning3 = false;
	
	public Camera Camera1;
	//public Camera Camera2;
	
	private bool m_bStarted = false;
	
	// Use this for initialization
	IEnumerator Start () 
	{
		yield return StartCoroutine(Countdown());
		
		m_fTimeRemaining = m_nTimeLimit;
		
		for(int i = 0; i < (m_nMaxNumbers - 1); i++)
		{
			GameObject goNumber = Instantiate(Resources.Load("PrefabNumber")) as GameObject;
			goNumber.transform.parent = GameObject.Find ("NumberArray").transform;
		}
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
			{
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = Mathf.Floor(countdown).ToString("##");
			}
			else
			{
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = "GO!";
			}
			
			countdown -= Time.deltaTime;
		}
		
		m_bStarted = true;
		GameObject.Find("TextCountdown").renderer.enabled = false;
		GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(true);
		
		GameObject.Find("ScreenBlocker").renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bStarted == true)
		{
			m_fTimeRemaining = m_fTimeRemaining - Time.fixedDeltaTime;
			
			/*if(m_fTimeRemaining < 0.0f)
			{
				m_fTimeRemaining = 0.0f;
				//StartCoroutine(WordPop(4));
				
				Camera2.GetComponent<Camera>().enabled = true;
				Camera2.GetComponent<Camera>().enabled = true;
				
				GameObject.Find ("FinalScore").GetComponent<TextMesh>().text = GameObject.Find ("ProgressBarManager").GetComponent<ClassProgression>().m_nScore.ToString();
		
			}*/
			
			if(m_fTimeRemaining <= 0.0f)
			{
				m_fTimeRemaining = 0.0f;
				
				StartCoroutine(GameEnder());
				
				m_bStarted = false;
			}	
			
			if(m_fTimeRemaining <= 5.0f)
			{
				if(m_bTimeWarning3 == false)
				{
					StartCoroutine(WordPop(3));
					
					m_bTimeWarning3 = true;
				}
			}
			else if(m_fTimeRemaining <= 10.0f)
			{
				if(m_bTimeWarning2 == false)
				{
					StartCoroutine(WordPop(2));
					
					m_bTimeWarning2 = true;
				}
			}
			else if(m_fTimeRemaining <= 30.0f)
			{
				if(m_bTimeWarning1 == false)
				{
					StartCoroutine(WordPop(1));
					
					m_bTimeWarning1 = true;
				}
			}
	
			GameObject.Find ("TimeRemaining").GetComponent<TextMesh>().text = m_fTimeRemaining.ToString("F0");
		}
	}
	
	IEnumerator WordPop(int _nIndex)
	{
		//GameObject goWord = Instantiate(Resources.Load("WordPop")) as GameObject;
		
		GameObject goFreeWord = new GameObject();
		
		Vector3 vOriginalPosition = Vector3.zero;
		Vector3 vOriginalScale = Vector3.zero;
		
		List <GameObject> agoWords = GameObject.Find ("WordPopArray").GetComponent<ClassWordManager>().m_agoWordPop;
		//agoWords.transform.position = GameObject.Find ("WordSpawnPoint").transform.position;
		
		for(int i = 0; i < agoWords.Count; i++)
		{
			if(agoWords[i].GetComponent<ClassWord>().m_bIsActive == false)
			{
				goFreeWord = agoWords[i];
				
				goFreeWord.GetComponent<ClassWord>().m_bIsActive = true;
				
				vOriginalPosition = goFreeWord.transform.position;
				vOriginalScale = goFreeWord.transform.localScale;
				
				i = agoWords.Count;
			}
		}
		
		goFreeWord.transform.position = GameObject.Find ("TimeWarningSpawnPoint").transform.position;
		
		switch(_nIndex)
		{
		case 1:
			
			goFreeWord.GetComponent<TextMesh>().text = "Only 30 seconds remaining";
			
			break;
			
		case 2:
			
			goFreeWord.GetComponent<TextMesh>().text = "Only 10 seconds remaining";
			
			break;
			
		case 3:
			
			goFreeWord.GetComponent<TextMesh>().text = "Only 5 seconds remaining";
			
			break;
			
		case 4:
			
			goFreeWord.GetComponent<TextMesh>().text = "Game Over!";
			
			break;
		}
		
		//goWord.GetComponent<MeshRenderer>().enabled = true;
		
		float fWordTime = 0.8f;
		
		while ( fWordTime > 0 )
		{
			//goWord.GetComponent<TextMesh>().text = _sText;
			
			//goFreeWord.GetComponent<TextMesh>().fontSize = goFreeWord.GetComponent<TextMesh>().fontSize + (int)(5 * (fWordTime - (int)fWordTime));
			
			goFreeWord.transform.localScale = goFreeWord.transform.localScale + new Vector3(0.005f, 0.005f, 0.005f); 
			
			yield return new WaitForFixedUpdate();
			
			fWordTime = fWordTime - Time.deltaTime;
		}
		
		goFreeWord.transform.position = vOriginalPosition;
		
		goFreeWord.GetComponent<TextMesh>().fontSize = 100;
		
		goFreeWord.transform.localScale = vOriginalScale;
		
		goFreeWord.GetComponent<ClassWord>().m_bIsActive = false;
	}
	
	IEnumerator GameEnder()
	{
		GameObject.Find("ScreenBlocker").renderer.enabled = true;
		
		GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(false);
		
		GameObject.Find("GameEndText").GetComponent<TextMesh>().text = "Time's Up!" ;
		GameObject.Find("GameEndText").renderer.enabled = true;
		
		yield return new WaitForSeconds (3);
		
		Application.LoadLevel(Application.loadedLevel);
	}
}
