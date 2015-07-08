using UnityEngine;
using System.Collections;

public class SelectCardScriptLevel2 : MonoBehaviour 
{
	private RaycastHit m_rchHit;
	private GameObject m_goFirstCard;
	private GameObject m_goSecondCard;
	
	public int m_nCardsLeft = 18;
	public float m_fTimeLeft = 100.0f;
	public float m_fTotalTime = 100.0f;
	public float m_fTimeElapsed = 0.0f;
	public int m_nScore = 0;
	public int m_nStreak = 0;
	
	public GameObject m_goTimeCounter;
	public GameObject m_goScoreCounter;
	public GameObject m_goEndRoundText;
	
	private bool m_bHasPressedTwo = false;
	private bool m_bRoundStarted = false;
	private bool m_bRoundEnded = false;
	
	private float m_fRevealTime = 4.0f;
	
	// Use this for initialization
	void Start () 
	{
		Debug.Log("game has started");
		
		GetComponent<CardMatchTimerScript>().SetTime(3);
		GetComponent<CardMatchTimerScript>().StartTimer();
		GetComponent<CardMatchTimerScript>().DisplayTimer();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( !m_bRoundStarted )
		{
			if ( GetComponent<CardMatchTimerScript>().HasEndedTime() )
			{
				GetComponent<CardMatchTimerScript>().HideTimer();
				GameObject.Find("cardGenerator").GetComponent<GenerateCardScriptLevel2>().Generate();
				m_bRoundStarted = true;
			}
			
			return;
		}
		
		if(m_fRevealTime <= 0.0f && !m_bRoundEnded)
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			
			if(m_bHasPressedTwo == false)
			{
				if (Input.GetMouseButtonDown (0))
				{
					if (Physics.Raycast (ray, out m_rchHit, 100)) 
					{
						if(m_rchHit.transform.gameObject.tag == "Cards")
						{
							if(m_bHasPressedTwo == false)
							{
								if ( !m_goFirstCard)
								{
									StartCoroutine(RevealFirstCard());
									Debug.Log("Card ONE");
								}
								
								else
								{
									m_goSecondCard = m_rchHit.transform.gameObject;
			
									if(m_goFirstCard != m_goSecondCard)
									{
										//yield RevealSecondCard();
										m_bHasPressedTwo = true;
										StartCoroutine(RevealSecondCard());	
									}
								}
							}
						}
			
					}
				}
			}
			
			if(!m_bRoundEnded) //When reach 0, stop counting
			{
				m_fTimeLeft = m_fTotalTime - m_fTimeElapsed;
			}
			
			m_goTimeCounter.GetComponent<TextMesh>().text = "" + (int) m_fTimeLeft;
			
			if (m_fTimeLeft <= 0) 
			{
				m_bRoundEnded = true;
				//yield Lose();	
				StartCoroutine(Lose());	
			}
				
			m_fTimeElapsed += Time.deltaTime;
			
		}
		else
		{
			m_fRevealTime -= Time.deltaTime;
			m_goTimeCounter.GetComponent<TextMesh>().text = "" + (int) m_fTimeLeft;
		
		}
		
		
	}
	
	IEnumerator RevealFirstCard()
	{	
		m_goFirstCard = m_rchHit.transform.gameObject;
		
		m_goFirstCard.animation.Play("reveal");
		Debug.Log(m_goFirstCard.name);
		
		yield return new WaitForSeconds  (0.2f);
		
		yield return new WaitForSeconds (m_goFirstCard.animation["reveal"].length);
	}

 	
	IEnumerator RevealSecondCard()
	{	
	
		m_goSecondCard.animation.Play("reveal");
		
		yield return new WaitForSeconds (0.2f);
		
		m_goSecondCard.animation.Play("reveal");
		
		yield return new WaitForSeconds (0.2f);
		
		yield return new WaitForSeconds (m_rchHit.transform.gameObject.animation.clip.length);
		
		if (m_goFirstCard.name == m_goSecondCard.name)
		{
			yield return new WaitForSeconds (0.5f);
			
			Destroy (m_goFirstCard);
			Destroy (m_goSecondCard);
	
			m_nCardsLeft -= 2;
			m_nStreak++;
			m_nScore += 10 * m_nStreak;
			//m_fTotalTime = 3;
			GameObject.Find("Streak").GetComponent<TextMesh>().text = m_nStreak.ToString();
			m_goScoreCounter.GetComponent<TextMesh>().text = "" + m_nScore;
			//m_goTimeCounter.GetComponent<TextMesh>().text = "" + m_fTotalTime;
			
			m_bHasPressedTwo = false;
			
			GameObject.Find("Sound_Correct").GetComponent<BasicSoundScript>().PlaySound();
			
		}
		
		else
		{
			yield return new WaitForSeconds (0.5f);
			
			m_goFirstCard.animation.Play("hide");
			m_goSecondCard.animation.Play("hide");
			m_nStreak = 0;
			GameObject.Find("Streak").GetComponent<TextMesh>().text = m_nStreak.ToString();
			
			yield return new WaitForSeconds (m_goSecondCard.animation["hide"].length/1.2f);
			
			m_bHasPressedTwo = false;
			
			GameObject.Find("Sound_Wrong").GetComponent<BasicSoundScript>().PlaySound();
		
		}
		
		m_goFirstCard = null;
		m_goSecondCard = null;
		
		if (m_nCardsLeft == 0) 
		{
			m_bRoundEnded = true;
			//yield Win();
			StartCoroutine(Win());
		}

	}

	IEnumerator Win ()
	{
		m_goEndRoundText.GetComponent<TextMesh>().text = "Congratulations!" ;
		m_goEndRoundText.renderer.enabled = true;
		
		yield return new WaitForSeconds (3);
		
		Destroy (this);
		
		Application.LoadLevel(Application.loadedLevel);
	}

	IEnumerator Lose ()
	{
		m_goEndRoundText.GetComponent<TextMesh>().text = "Time's Up!" ;
		m_goEndRoundText.renderer.enabled = true;
		
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Cards"))
			if (!go.animation.IsPlaying("reveal"))
				go.animation.Play("reveal");
		
		yield return new WaitForSeconds (3);
		
		Destroy (this);
		
		Application.LoadLevel(Application.loadedLevel);
	}
}
