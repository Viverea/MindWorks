using UnityEngine;
using System.Collections;

public class FSv2MainScript : MonoBehaviour 
{
	enum eGameStates
	{
		eNone = 0,
		eRandomiseToAvoid,
		eRevealToAvoid,
		ePrelevelCountdown,
		eLevelStart,
		eDisplayCrossTick,
		eSummary,
		eLevelPass,
		eLevelFail,
		eGameEnd,
		eTotal
	};	
	
	eGameStates m_eMainGameStates;
	
	float m_fCountdownTimer = 0.0f;
	float m_fTimer = 0.0f;
	
	float m_fFliesLeft = 0;
	public int m_nLevelNumber = 0;
	
	bool m_bHasSpawnedFlies = false;
	
	public bool m_bIsEasyLevel = false;
	bool m_bHasWonAll = false;
	bool m_bHasWonLevel = false;
	
	public GameObject m_goTickPrefab;
	public GameObject m_goCrossPrefab;
	
	// Use this for initialization
	void Start () 
	{
		UpdateDifficultyOnScripts();
		
		//Setup first level
		
		m_nLevelNumber = 1;
		
		this.gameObject.GetComponent<FlySwatterGlobalVarScript>().m_fRoundTime = 5.0f;
		
		GameObject.Find("3DTextLevelNum").GetComponent<TextMesh>().text = m_nLevelNumber.ToString ();
			
		m_eMainGameStates = eGameStates.eRandomiseToAvoid;
		
		//Disable renderer for post game answers
		GameObject.Find("3DTextCharIntValues").GetComponent<MeshRenderer>().renderer.enabled = false;
		GameObject.Find("3DTextChartIntHeader").GetComponent<MeshRenderer>().renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(m_eMainGameStates)
		{
			case eGameStates.eRandomiseToAvoid:
			{
				this.gameObject.GetComponent<FlySwatterCharIntToRememberScript>().GenerateRandomCharInt();
				this.gameObject.GetComponent<FlySwatterCharIntToRememberScript>().DisplayCharInt();
				
				GameObject.Find("3DTextHeader1").renderer.enabled = true;
				GameObject.Find("3DTextHeader2").renderer.enabled = true;
				GameObject.Find("3DTextHeader3").renderer.enabled = true;
				GameObject.Find ("3DTextTimeLimitHeader").renderer.enabled = true;
				GameObject.Find ("3DTextTimeLimitHeader").transform.GetChild(0).renderer.enabled = true;
			
				//Show Sidebar every round
				GameObject.Find("3DTextTimer").GetComponent<TextMesh>().text = GameObject.Find("SceneManager").GetComponent<FlySwatterGlobalVarScript>().m_fRoundTime.ToString();
				
				for(int i = 0; i < 10; i++)
				{
					GameObject.Find ("Elements(Sidebar)").transform.GetChild(i).renderer.enabled = true;
				}
				
				this.gameObject.GetComponent<FlySwatterTimerScript>().SetTime(5.0f);
			
				m_eMainGameStates = eGameStates.eRevealToAvoid;
			
				break;
			}
			
			case eGameStates.eRevealToAvoid:
			{
				if(this.gameObject.GetComponent<FlySwatterTimerScript>().HasReachedTime())
				{
					this.gameObject.GetComponent<FlySwatterCharIntToRememberScript>().RemoveRevealCharInt();
					
					GameObject.Find("3DTextHeader1").renderer.enabled = false;
					GameObject.Find("3DTextHeader2").renderer.enabled = false;
					GameObject.Find("3DTextHeader3").renderer.enabled = false;
					GameObject.Find ("3DTextTimeLimitHeader").renderer.enabled = false;
					GameObject.Find ("3DTextTimeLimitHeader").transform.GetChild(0).renderer.enabled = false;
				
					GameObject.Find("3DTextCountdownNum").renderer.enabled = true;
				
					this.gameObject.GetComponent<FlySwatterTimerScript>().SetTime (4.0f);
					m_fCountdownTimer = 4.0f;
			
					m_eMainGameStates = eGameStates.ePrelevelCountdown;
				}
			
				//Else continue to show
			
				break;
			}
			
			case eGameStates.ePrelevelCountdown:
			{
				if(this.gameObject.GetComponent<FlySwatterTimerScript>().HasReachedTime())
				{
					GameObject.Find("3DTextCountdownNum").renderer.enabled = false;
				
					float fRoundTime = this.gameObject.GetComponent<FlySwatterGlobalVarScript>().m_fRoundTime;
					this.gameObject.GetComponent<FlySwatterTimerScript>().SetTime (fRoundTime);
				
					this.gameObject.GetComponent<FlySwatterTimerScript>().DisplayTimer();
					this.gameObject.GetComponent<FlySwatterScoringScript>().DisplayText();
				
					m_eMainGameStates = eGameStates.eLevelStart;
				
				}
				else
				{
					if(m_fCountdownTimer <= 1.0f)	
					{
						//GameObject.Find("3DTextCountdownNum").transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
					
						GameObject.Find("3DTextCountdownNum").GetComponent<TextMesh>().text = "GO!";
					}
					else
					{
						int nSecondsBeforeChange = int.Parse (GameObject.Find("3DTextCountdownNum").GetComponent<TextMesh>().text);
					
						GameObject.Find("3DTextCountdownNum").GetComponent<TextMesh>().text = ((int) m_fCountdownTimer).ToString ();
					
					
						if(nSecondsBeforeChange > (int) m_fCountdownTimer)
						{
							//GameObject.Find("3DTextCountdownNum").transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
						}
						
						
						//GameObject.Find("3DTextCountdownNum").transform.localScale -= new Vector3(0.03f,0.03f, 0.03f);
					}
				
				}
			
				m_fCountdownTimer -= Time.deltaTime;
			
				break;
			}
			
			case eGameStates.eLevelStart:
			{
				//Spawn flies first
				if(!m_bHasSpawnedFlies)
				{
					this.gameObject.GetComponent<FlySwatterFliesManagerScript>().SpawnFlies(m_nLevelNumber);
					
					m_fFliesLeft = GameObject.FindGameObjectsWithTag("Fly").Length;
					m_bHasSpawnedFlies = true;
				}
				
				//Check for mouse down
				if(Input.GetMouseButtonDown(0))
				{
					Ray rRay = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit rchHit = new RaycastHit();
					
					if(Physics.Raycast(rRay, out rchHit, 100))
					{
						//Input numbers
						if(rchHit.transform.gameObject.tag == "Fly")
						{
							if(!rchHit.transform.gameObject.GetComponent<FlySwatterFlyScript>().GetIsSquished())
							{
								bool bIsCharacter = rchHit.transform.gameObject.GetComponent<FlySwatterFlyScript>().GetIsCharacter();
								
								if(bIsCharacter)
								{
									char cFlyChar = rchHit.transform.gameObject.GetComponent<TextMesh>().text[0];
									
									//Hit something that is to be remembered 
									if(this.gameObject.GetComponent<FlySwatterCharIntToRememberScript>().CompareChar(cFlyChar))
									{
										Debug.Log ("NopeNopeNope");
										
										this.gameObject.GetComponent<FlySwatterScoringScript>().DeductScore();
										this.gameObject.GetComponent<FlySwatterScoringScript>().ResetVars();
									}
									else
									{
										rchHit.transform.gameObject.GetComponent<FlySwatterFlyScript>().TriggerDeath();
										m_fFliesLeft--;
										
										this.gameObject.GetComponent<FlySwatterScoringScript>().AddScore ();
										GameObject.Find("Sound_Squish").GetComponent<BasicSoundScript>().PlaySound();
									}
								}
								else
								{
									int cFlyInt = int.Parse(rchHit.transform.gameObject.GetComponent<TextMesh>().text);
									
									//Hit something that is to be remembered 
									if(this.gameObject.GetComponent<FlySwatterCharIntToRememberScript>().CompareInt(cFlyInt))
									{
										Debug.Log ("NopeNopeNope");
										
										this.gameObject.GetComponent<FlySwatterScoringScript>().DeductScore();
										this.gameObject.GetComponent<FlySwatterScoringScript>().ResetVars();
									}
									else
									{
										rchHit.transform.gameObject.GetComponent<FlySwatterFlyScript>().TriggerDeath();
										m_fFliesLeft--;
										
										this.gameObject.GetComponent<FlySwatterScoringScript>().AddScore ();
										GameObject.Find("Sound_Squish").GetComponent<BasicSoundScript>().PlaySound();
									}
								}
							}
						}
					}
				}
				
				//Based on level, check for number of flies left
				if(m_bIsEasyLevel)
				{
					if(m_fFliesLeft <= 2)
					{
						Debug.Log("Yay Winner");	
						
						this.gameObject.GetComponent<FlySwatterTimerScript>().StopTimer ();
	
						this.gameObject.GetComponent<FlySwatterScoringScript>().SetSecondsRemaining(); //Set seconds remaining in level
						
						m_fTimer = 1.0f;
					
						if(m_nLevelNumber >= 10)
						{
							//SetGameEndMessage();
							m_bHasWonAll = true;
						}
					
						m_bHasWonLevel = true;
					
						DisplayCrossTick();
						GameObject.Find("Sound_Correct").GetComponent<BasicSoundScript>().PlaySound();
		
						m_eMainGameStates = eGameStates.eDisplayCrossTick;
						
					}
				}
				else
				{
					if(m_fFliesLeft <= 4)
					{
						Debug.Log("Yay Winner");	
						
						this.gameObject.GetComponent<FlySwatterTimerScript>().StopTimer ();
						
						this.gameObject.GetComponent<FlySwatterScoringScript>().SetSecondsRemaining(); //Set seconds remaining in level
						
						m_fTimer = 1.0f;
					
						if(m_nLevelNumber >= 10)
						{
							//SetGameEndMsessage();
							m_bHasWonAll = true;
						}
		
						m_bHasWonLevel = true;
					
						DisplayCrossTick();
						GameObject.Find("Sound_Correct").GetComponent<BasicSoundScript>().PlaySound();
					
						m_eMainGameStates = eGameStates.eDisplayCrossTick;

					}
				}
				
				//Check time
				if(this.gameObject.GetComponent<FlySwatterTimerScript>().HasReachedTime())
				{
					Debug.Log("Uhoh! Times Up!");
					
					this.gameObject.GetComponent<FlySwatterTimerScript>().StopTimer ();
				
					this.gameObject.GetComponent<FlySwatterScoringScript>().SetSecondsRemaining(); //Set seconds remaining in level
				
					m_fTimer = 1.0f;
				
					m_bHasWonLevel = false;
				
					DisplayCrossTick();
					GameObject.Find("Sound_Wrong").GetComponent<BasicSoundScript>().PlaySound();
											
					m_eMainGameStates = eGameStates.eDisplayCrossTick;
				}
				
				break;
			}
			
			case eGameStates.eDisplayCrossTick:
			{
				if(m_fTimer <= 0.0f)
				{
					if(m_bHasWonLevel)
					{
						DestroyCrossTick();
						DestroyRemainingFlies();
						CleanUpLevel();
						ResetVariables();
						
					
//						if(m_bHasWonAll)
//						{
//							this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().SetSummaryScreen(true);
//							m_eMainGameStates = eGameStates.eSummary;
//						}	
//						else //Still has next level
						{
							SetNextLevel();	
							m_eMainGameStates = eGameStates.eRandomiseToAvoid;
						}
					}
					else
					{
						DestroyCrossTick();
//						DestroyRemainingFlies();
//						this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().SetSummaryScreen(false);
					
//						Application.LoadLevel(Application.loadedLevelName);
						StartCoroutine(DisplayAnswer());
						m_eMainGameStates = eGameStates.eTotal;
					
//						m_eMainGameStates = eGameStates.eSummary;
//						GameObject.Find("SceneManager").GetComponent<FlySwatterCharIntToRememberScript>().ShowAnswersPostGame();
		
					}
					
	
				}	
			
				m_fTimer -= Time.deltaTime;
			
				break;
			}
			
			case eGameStates.eSummary:
			{
				
				this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().FadeInSummaryScreenElements();
			
				//Check for mouse down
				if(Input.GetMouseButtonDown(0))
				{
					Ray rRay = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit rchHit = new RaycastHit();
					
					if(Physics.Raycast(rRay, out rchHit, 100))
					{
						if(this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().m_bHasFadedIn)
						{
							if(rchHit.collider.gameObject == GameObject.Find("RetryButton(Clone)"))
							{
								Application.LoadLevel(Application.loadedLevel);
							}
							else if (rchHit.collider.gameObject == GameObject.Find("SelectDifficultyButton(Clone)"))
							{
								Application.LoadLevel("Game_Flyswatter_Select");
							}
						}
						else
						{
							this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().SetElementsMaxAlpha();
						}
					}
				}
			
				break;
			}
			
			
			/*
			case eGameStates.eLevelPass:
			{
			
				this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().FadeInEndLevelElements();
			
				//Check for mouse down
				if(Input.GetMouseButtonDown(0))
				{
					Ray rRay = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit rchHit = new RaycastHit();
					
					if(Physics.Raycast(rRay, out rchHit, 100))
					{
						//Input numbers
						if(rchHit.transform.gameObject != GameObject.Find ("BackButton"))
						{
							if(this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().m_bHasFadedIn)
							{
								CleanUpLevel();
								ResetVariables();
								SetNextLevel();		
								m_eMainGameStates = eGameStates.eRandomiseToAvoid;
							}
							else
							{
								this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().SetElementsMaxAlpha();
							}
						}
					}
				}
				break;
				
				//Correct ones
				DestroyRemainingFlies();
				CleanUpLevel();
				ResetVariables();
				SetNextLevel();		
				m_eMainGameStates = eGameStates.eRandomiseToAvoid;
			
				break;
			
			}
	
			
			case eGameStates.eLevelFail:
			{
				this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().FadeInEndLevelElements();
				
				//Check for mouse down
				if(Input.GetMouseButtonDown(0))
				{
					Ray rRay = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit rchHit = new RaycastHit();
					
					if(Physics.Raycast(rRay, out rchHit, 100))
					{
						if(this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().m_bHasFadedIn)
						{
							if(rchHit.collider.gameObject == GameObject.Find("RetryButton(Clone)"))
							{
								Application.LoadLevel(Application.loadedLevel);
							}
							else if (rchHit.collider.gameObject == GameObject.Find("SelectDifficultyButton(Clone)"))
							{
								Application.LoadLevel("Game_Flyswatter_Select");
							}
						}
						else
						{
							this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().SetElementsMaxAlpha();
						}
					}
				}
				
				break;
			}
				
			case eGameStates.eGameEnd:
			{
				this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().FadeInEndLevelElements();
				
				//Check for mouse down
				if(Input.GetMouseButtonDown(0))
				{
					Ray rRay = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit rchHit = new RaycastHit();
					
					if(Physics.Raycast(rRay, out rchHit, 100))
					{
						if(this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().m_bHasFadedIn)
						{
							if(rchHit.collider.gameObject == GameObject.Find("RetryButton(Clone)"))
							{
								Application.LoadLevel(Application.loadedLevel);
							}
							else if (rchHit.collider.gameObject == GameObject.Find("SelectDifficultyButton(Clone)"))
							{
								Application.LoadLevel("Game_Flyswatter_Select");
							}
						}
						else
						{
							this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().SetElementsMaxAlpha();
						}
					}
				}
				
				break;
			}
		
			*/		
			
		}
	}
	
	void UpdateDifficultyOnScripts()
	{
		this.gameObject.GetComponent<FlySwatterCharIntToRememberScript>().SetDifficulty(m_bIsEasyLevel);
		this.gameObject.GetComponent<FlySwatterFliesManagerScript>().SetDifficulty(m_bIsEasyLevel);
	}
	
	void DisplayCrossTick()
	{
		if(m_bHasWonLevel)
		{
			GameObject.Instantiate(m_goTickPrefab);
		}
		else
		{
			GameObject.Instantiate(m_goCrossPrefab);
		}
	}
	
	void DestroyCrossTick()
	{
		if(m_bHasWonLevel)
		{
			GameObject.Destroy (GameObject.Find("TickPrefab(Clone)"));
		}
		else
		{
			GameObject.Destroy (GameObject.Find("CrossPrefab(Clone)"));
		}
	}
	
	void DestroyRemainingFlies()
	{
		GameObject[] arrgoFlies = GameObject.FindGameObjectsWithTag("Fly");
		
		for(int i = 0; i < arrgoFlies.Length; i++)
		{
			Destroy(arrgoFlies[i]);
		}
	}
	
	void CleanUpLevel()
	{
		GameObject[] arrgoListOfEndLevelTexts = GameObject.FindGameObjectsWithTag("FSEndLevelText");
		GameObject[] arrgoListOfEndLevelButtons = GameObject.FindGameObjectsWithTag("FSEndLevelButton");
		GameObject goPreRoundElements = GameObject.Find("Elements(Pre-round)");
		GameObject goSidebarElements = GameObject.Find ("Elements(Sidebar)");
		
		for(int i = 0; i < arrgoListOfEndLevelTexts.Length; i++)
		{
			GameObject.Destroy(arrgoListOfEndLevelTexts[i]);
		}
		
		for(int i = 0; i < arrgoListOfEndLevelButtons.Length; i++)
		{
			GameObject.Destroy(arrgoListOfEndLevelButtons[i]);
		}
		
		for(int i = 0; i < goPreRoundElements.transform.childCount; i++)
		{
			goPreRoundElements.transform.GetChild(i).renderer.enabled = false;
		}
		
		for(int i = 0; i < goSidebarElements.transform.childCount; i++)
		{
			if( goSidebarElements.transform.GetChild(i).name == "3DTextLevelNum" ||
				goSidebarElements.transform.GetChild(i).name == "3DTextLevelNumBorder"  ||
				goSidebarElements.transform.GetChild(i).name == "3DTextLevelNumHeader")
			{
				goSidebarElements.transform.GetChild(i).renderer.enabled = true;
			}
			else
			{
				goSidebarElements.transform.GetChild(i).renderer.enabled = false;
			}
		}
		
	}
	
	void SetNextLevel()
	{
		m_nLevelNumber++;
		
		GameObject.Find("3DTextLevelNum").GetComponent<TextMesh>().text = m_nLevelNumber.ToString ();
			
		this.gameObject.GetComponent<FlySwatterGlobalVarScript>().m_fRoundTime += 1.0f;

		this.gameObject.GetComponent<FlySwatterGlobalVarScript>().m_fFlyFlightVelocity += 0.1f;
		
		GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().text = "REMEMBER!";
		GameObject.Find("3DTextHeader1").GetComponent<TextMesh>().fontSize = 150;
		
		GameObject.Find("3DTextHeader2").GetComponent<TextMesh>().text = "DO NOT SQUASH:";
		GameObject.Find("3DTextHeader2").GetComponent<TextMesh>().fontSize = 150;
		GameObject.Find("3DTextHeader2").transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		GameObject.Find("3DTextHeader2").transform.position = new Vector3(28.7f, 48.2f, 15.3f);
			
		GameObject.Find("3DTextHeader3").transform.position = new Vector3(5.6f, 46.4f, 15.3f);
		GameObject.Find("3DTextHeader3").transform.localScale = new Vector3(4.0f, 1.0f, 1.0f);
		GameObject.Find("3DTextHeader3").GetComponent<TextMesh>().text = "_";
		
		GameObject.Find ("3DTextCountdownNum").GetComponent<TextMesh>().text = "3";	
		
		GameObject.Find ("3DTextTimeLimitHeader").GetComponent<TextMesh>().text = "Time Limit: " + 
		this.gameObject.GetComponent<FlySwatterGlobalVarScript>().m_fRoundTime + "s";
	}
	
	void ResetVariables()
	{
		m_bHasSpawnedFlies = false;
		m_fCountdownTimer = 0.0f;
		m_fTimer = 0.0f;
		m_bHasWonLevel = false;
		GameObject.Find("3DTextTimer").GetComponent<TextMesh>().text = "";
		
		//Script variables
		this.gameObject.GetComponent<FlySwatterSummaryScreenScript>().ResetVariables();
		this.gameObject.GetComponent<FlySwatterTimerScript>().ResetTimer();
	}
	
	IEnumerator DisplayAnswer()
	{
		yield return new WaitForSeconds(0.5f);
		GameObject[] arrgoFlies = GameObject.FindGameObjectsWithTag("Fly");
		for ( int n = 0; n < arrgoFlies.Length; ++n )
		{
			if ( arrgoFlies[n].GetComponent<FlySwatterFlyScript>().GetIsCharacter() )
			{
				if( this.gameObject.GetComponent<FlySwatterCharIntToRememberScript>().CompareChar(arrgoFlies[n].GetComponent<TextMesh>().text[0]) )
					continue;
			}
			else
			{
				if ( this.gameObject.GetComponent<FlySwatterCharIntToRememberScript>().CompareInt(int.Parse(arrgoFlies[n].GetComponent<TextMesh>().text)) )
					continue;
			}
			
			arrgoFlies[n].GetComponent<FlySwatterFlyScript>().TriggerDeath();
		}
		GameObject.Find("Sound_Squish").GetComponent<BasicSoundScript>().PlaySound();
		yield return new WaitForSeconds(2.5f);
		
		Application.LoadLevel(Application.loadedLevelName); // Samuel's solution, don't ask me.
	}

}
