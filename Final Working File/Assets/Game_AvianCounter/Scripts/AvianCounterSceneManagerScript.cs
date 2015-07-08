using UnityEngine;
using System.Collections;

public class AvianCounterSceneManagerScript : MonoBehaviour
{
	//Difficulty varaible
	private bool m_bIsLevelOne = true;
	//Difficulty-based max num to be generated (non-inclusive)
	private int m_nMaxNumGenerated = 8;
	
	//Equation related variables
	private bool m_bIsAddition = true;
	
	private int m_nFirstInteger = 0;
	private int m_nSecondInteger = 0;
	private int m_nSum = 0;
	
	//Equation Timer
	public GameObject m_3dtTimer;
	private float m_fGenericTimer = 5.0f;
	
	//Gameobject 3DText
	public GameObject m_3dtFirstInteger;
	public GameObject m_3dtSecondInteger;
	public GameObject m_3dtSum;
	public GameObject m_3dtOperator;
	
	//Player input game objects
	public GameObject m_goKeypadTick;
	public GameObject m_goKeypadCross;
	public GameObject m_3dtPlayerInput;
	
	//Player input number
	private int m_nPlayerInputNumber = 0;
	
	//BlackScreen Layer
	public GameObject m_goBlackScreen;
	
	//Internal variables
	private bool m_bChangeEquation = true;
	private bool m_bIsPlayerAnswering = false;
	
	private bool m_bCountdownEnded = false;
	
	private bool m_bHasDisplayedFirstSetBirds = false;
	private bool m_bHasDisplayedSecondSetBirds = false;
	
	private bool m_bShowTickCross = false;
	private bool m_bHasPlayerWon = false;
	
	//Cross/Tick
	public Object m_oCrossPrefab;
	public Object m_oTickPrefab;
		
	//Bird prefab
	public Object m_oBirdPrefab;
	
	// Use this for initialization
	void Start () 
	{
		if(Application.loadedLevelName == "Game_AvianCounter_Hard")
		{
			m_bIsLevelOne = false;
		}
		
		if(m_bIsLevelOne)
		{
			m_nMaxNumGenerated = 8;
		}
		else
		{
			m_nMaxNumGenerated = 13;
		}
		
		this.gameObject.GetComponent<AvianCounterScoreScript>().SetDifficulty(m_bIsLevelOne);
		
		this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().SetTime(3.0f);
		this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().StartTimer();
		this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().DisplayTimer();
		GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(!m_bCountdownEnded)
		{
			if(this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().HasEndedTime())
			{
				m_bCountdownEnded = true;
				this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().StopTimer();
				this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().HideTimer();
				GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(true);
				
				m_goBlackScreen.renderer.enabled = false;
			}
		}
		
		if(m_bChangeEquation && !m_bIsPlayerAnswering && m_bCountdownEnded)
		{
			GenerateEquation();
			m_bChangeEquation = false;
			m_bIsPlayerAnswering = true;
		}
		
		if(!m_bHasDisplayedFirstSetBirds && !m_bHasDisplayedSecondSetBirds && m_bCountdownEnded)
		{
			//Instantiate all birds once, then countdown
			
			if(m_fGenericTimer > 4.0f)
			{
				DisplayFirstSetOfBirds();
				
				m_fGenericTimer = 3.0f;
			}

			if(m_fGenericTimer > 0.0f)
			{
				m_fGenericTimer -= Time.deltaTime;
			}
			else
			{
				m_fGenericTimer = 5.0f;
				
				m_bHasDisplayedFirstSetBirds = true;
			}
		}
		else if (m_bHasDisplayedFirstSetBirds && !m_bHasDisplayedSecondSetBirds)
		{
			if(m_fGenericTimer > 4.0f)
			{
				DisplaySecondSetOfBirds();
				
				m_fGenericTimer = 3.0f;
			}
			
			if(m_fGenericTimer < 2.8f)
			{
				m_goBlackScreen.renderer.enabled = false;
			}

			if(m_fGenericTimer > 0.0f)
			{
				m_fGenericTimer -= Time.deltaTime;
			}
			else
			{
				m_fGenericTimer = 5.0f;
				
				m_bHasDisplayedSecondSetBirds = true;
				
				m_goBlackScreen.renderer.enabled = true;
				
				this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().DisplayQuestionMark();
//				this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().m_3dtTimerText.transform.position = m_goBlackScreen.transform.position;
				
				this.gameObject.GetComponent<AvianCounterScoreScript>().StartTimer ();
			}
		}
						
		if(!m_bChangeEquation && m_bIsPlayerAnswering)
		{
			if(m_bHasDisplayedFirstSetBirds && m_bHasDisplayedSecondSetBirds)
			{
				//Countdown timer first
				m_fGenericTimer -= Time.deltaTime;
				m_3dtTimer.GetComponent<TextMesh>().text = ((int) m_fGenericTimer).ToString();
				
				if(m_fGenericTimer <= 0.0f)
				{
						
					Debug.Log ("Booo sucker");
					m_bIsPlayerAnswering = false;
					m_bHasPlayerWon = false;
					m_bShowTickCross = true;
					
					Instantiate(m_oCrossPrefab);
					
					GameObject.Find("Sound_Wrong").GetComponent<BasicSoundScript>().PlaySound();
					
					this.gameObject.GetComponent<AvianCounterScoreScript>().StopTimer();
					this.gameObject.GetComponent<AvianCounterScoreScript>().ResetTimer();
					this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().DisplayNumber(m_nSecondInteger);
					
					m_fGenericTimer = 0.0f;
					
				}		
				
				if(Input.GetMouseButtonDown(0))
				{
					Ray rRay = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit rchHit = new RaycastHit();
					
					if(Physics.Raycast(rRay, out rchHit, 100))
					{
						//Input numbers
						if(rchHit.transform.gameObject.tag == "KeypadNumbers")
						{
							//Allow input up to 2 digits only
							if(m_3dtPlayerInput.GetComponent<TextMesh>().text.Length < 2)
							{
								GameObject goHitTarget = rchHit.transform.gameObject;
								int nNumberValue = goHitTarget.GetComponent<AvianCounterNumpadNumberScript>().GetValue();
								
								//If no input initially, enable blue quad below
								if(m_3dtPlayerInput.GetComponent<TextMesh>().text.Length == 0)
								{
									GameObject.Find("QuadPlayerInput").renderer.enabled = true;
								}
								
								//Change text on player's input
								m_3dtPlayerInput.GetComponent<TextMesh>().text += nNumberValue;
								
								//Change number in player's input
								m_nPlayerInputNumber = int.Parse(m_3dtPlayerInput.GetComponent<TextMesh>().text);
								
								//Set quad as selected
								goHitTarget.GetComponent<AvianCounterKeypadColorScript>().SetSelected(true);
							}
						}
						
						//Backspace number
						else if (rchHit.transform.gameObject == m_goKeypadCross)
						{
							if(m_3dtPlayerInput.GetComponent<TextMesh>().text.Length != 0)
							{
								string strPlayerInputText = m_3dtPlayerInput.GetComponent<TextMesh>().text;
		  						strPlayerInputText = strPlayerInputText.Substring(0, strPlayerInputText.Length - 1);
								m_3dtPlayerInput.GetComponent<TextMesh>().text = strPlayerInputText;
								
								if(m_nPlayerInputNumber > 10) //Not a single digit number
								{
									m_nPlayerInputNumber = int.Parse (m_3dtPlayerInput.GetComponent<TextMesh>().text);
								}
								else
								{
									m_nPlayerInputNumber = 0;
								}
							}
							
							if(m_3dtPlayerInput.GetComponent<TextMesh>().text.Length == 0)
							{
								GameObject.Find("QuadPlayerInput").renderer.enabled = false;
							}
							
							//Set quad as selected
							rchHit.transform.gameObject.GetComponent<AvianCounterKeypadColorScript>().SetSelected(true);
						}
						
						//Compare player input
						else if (rchHit.transform.gameObject == m_goKeypadTick)
						{
							//Set quad as selected
							rchHit.transform.gameObject.GetComponent<AvianCounterKeypadColorScript>().SetSelected(true);
							
							this.gameObject.GetComponent<AvianCounterScoreScript>().StopTimer();
							
							//Hide question mark
							this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().HideTimer();
							
							if(m_nPlayerInputNumber == m_nSecondInteger)
							{
								//Won
								Debug.Log ("Yay you are wenier!");
								m_bHasPlayerWon = true;
								Instantiate(m_oTickPrefab);
								
								this.gameObject.GetComponent<AvianCounterScoreScript>().CalculateAndDisplayScore();
								
								GameObject.Find("Sound_Correct").GetComponent<BasicSoundScript>().PlaySound();
							}
							else
							{
								//Lost
								m_bHasPlayerWon = false;
								Instantiate(m_oCrossPrefab);
								Debug.Log ("Booo sucker");
								
								this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().
									DisplayNumber(m_nSecondInteger);
								GameObject.Find("TextCountdown").GetComponent<TextMesh>().fontSize = 700;
								
								GameObject.Find("Sound_Wrong").GetComponent<BasicSoundScript>().PlaySound();
							}
							
							m_bIsPlayerAnswering = false;
							m_bShowTickCross = true;
							
							this.gameObject.GetComponent<AvianCounterScoreScript>().ResetTimer();
							
							m_fGenericTimer = 0.0f;
						}
					}
				}
					
				
			}
		}
		
		if(m_bShowTickCross)
		{
			m_fGenericTimer += Time.deltaTime;
			
			if(m_fGenericTimer >= 1.0f)
			{
				ResetLevel();
				GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(false);
			}
		}
	}
	
	void GenerateEquation()
	{
		//Addition or subtraction, + change operator sign
		
		int nRandNum = Random.Range (1,3); // 1 or 2
		
		if(nRandNum == 1) //Next equation - Addition
		{
			m_bIsAddition = true;
			m_3dtOperator.GetComponent<TextMesh>().text = "+";
		}
		else if (nRandNum == 2) //Next equation - Subtraction
		{
			m_bIsAddition = false;
			m_3dtOperator.GetComponent<TextMesh>().text = "-";
		}
		
		//Randomise remaining numbers, second number always blank
		
		if(m_bIsAddition)
		{
			m_nFirstInteger = Random.Range (1,m_nMaxNumGenerated);
			m_nSecondInteger = Random.Range (1, m_nMaxNumGenerated - m_nFirstInteger);
			
			m_nSum = m_nFirstInteger + m_nSecondInteger;
			
			m_3dtFirstInteger.GetComponent<TextMesh>().text = m_nFirstInteger.ToString();
			m_3dtSecondInteger.GetComponent<TextMesh>().text = "__";
			m_3dtSum.GetComponent<TextMesh>().text = m_nSum.ToString ();
		}
		else
		{			
			//Second integer must be smaller than first, and cannot be the same number so as to avoid a final sum of 0 birds
			m_nSecondInteger = Random.Range(1,m_nMaxNumGenerated);
			m_nFirstInteger = Random.Range (m_nSecondInteger + 1, m_nMaxNumGenerated);
			
			m_nSum = m_nFirstInteger - m_nSecondInteger;
			
			m_3dtFirstInteger.GetComponent<TextMesh>().text = m_nFirstInteger.ToString();
			m_3dtSecondInteger.GetComponent<TextMesh>().text = "__";
			m_3dtSum.GetComponent<TextMesh>().text = m_nSum.ToString ();
		}
	}
	
	void ResetLevel()
	{
		//Reset font size for countdown
		GameObject.Find("TextCountdown").GetComponent<TextMesh>().fontSize = 450;
		
		//Reset all player inputs
		m_3dtPlayerInput.GetComponent<TextMesh>().text = "";
	
		//Player input number
		m_nPlayerInputNumber = 0;
		
		//Equation Timer
		m_fGenericTimer = 5.0f;
		
		m_bHasDisplayedFirstSetBirds = false;
		m_bHasDisplayedSecondSetBirds = false;
		
		m_bCountdownEnded = false;
		
		m_bShowTickCross = false;
										
		m_bChangeEquation = true;
		m_bIsPlayerAnswering = false;
		
		m_nFirstInteger = 0;
		m_nSecondInteger = 0;
		m_bIsAddition = false;
		m_nSum = 0;
		
		m_goBlackScreen.renderer.enabled = true;
		
		GameObject[] arrgoBirdPrefabs = GameObject.FindGameObjectsWithTag("BirdPrefab");
		
		for(int i = 0; i < arrgoBirdPrefabs.Length; i++)
		{
			Destroy(arrgoBirdPrefabs[i]);
		}
		
		m_3dtTimer.GetComponent<TextMesh>().text = "5";
		
		GameObject.Find("QuadPlayerInput").renderer.enabled = false;
		
		GameObject[] arrgoPositions = GameObject.FindGameObjectsWithTag("BirdPosition");
		
		for(int i = 0; i < arrgoPositions.Length; i++)
		{
			arrgoPositions[i].GetComponent<AvianCounterBirdPositionScript>().SetOccupied(false);
		}
		
		if(GameObject.Find("PrefabTick(Clone)")!= null)
		{
			Destroy(GameObject.Find ("PrefabTick(Clone)"));
		}
		else if (GameObject.Find ("PrefabCross(Clone)") != null)
		{
			Destroy(GameObject.Find ("PrefabCross(Clone)"));
		}
		
		this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().SetTime(3.0f);
		this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().StartTimer();
		this.gameObject.GetComponent<AvianCounterCountdownTimerScript>().DisplayTimer();
		
		GameObject.Find("SceneManager").GetComponent<AvianCounterCountdownTimerScript>().ReturnSizeToNormal();
	}
	
	void DisplayFirstSetOfBirds()
	{	
		//Addition, first number < sum
		//Subtraction, First number > sum
		for (int i = 0; i < m_nFirstInteger; i++)
		{
			Instantiate(m_oBirdPrefab);
		}	
		
			AssignBirdPositions();
			FadeInBirds();
	}
	
	void DisplaySecondSetOfBirds()
	{
		m_goBlackScreen.renderer.enabled = true;
		
		//If addition, instantiate more bird prefabs
		if(m_bIsAddition)
		{
			for(int i = 0; i < m_nSecondInteger; i++)
			{
				Instantiate(m_oBirdPrefab);
			}
		}
		else //Subtract, destroy gameobjects
		{
			GameObject[] arrgoBirdPrefabs = GameObject.FindGameObjectsWithTag("BirdPrefab");
			
			for(int i = 0; i < m_nSecondInteger; i++)
			{
				Destroy(arrgoBirdPrefabs[i].gameObject);
			}
		}
		
		AssignBirdPositions();
		SnapToPositionBirds();
	} 
	
	void AssignBirdPositions()
	{
		GameObject[] arrgoBirdPrefabs = GameObject.FindGameObjectsWithTag("BirdPrefab");
		GameObject[] arrgoPositions = GameObject.FindGameObjectsWithTag("BirdPosition");
		
		for(int i = 0; i < arrgoBirdPrefabs.Length; i++)
		{
			if(!arrgoBirdPrefabs[i].GetComponent<AvianCounterBirdScript>().HasBeenPositioned())		
			{
				int nRandomPositionIndex = Random.Range(0, arrgoPositions.Length);
				
				if(!arrgoPositions[nRandomPositionIndex].GetComponent<AvianCounterBirdPositionScript>().GetOccupied())
				{
					arrgoBirdPrefabs[i].gameObject.GetComponent<AvianCounterBirdScript>().SetAssignedPosition(arrgoPositions[nRandomPositionIndex].transform.position);
					
					arrgoPositions[nRandomPositionIndex].GetComponent<AvianCounterBirdPositionScript>().SetOccupied(true);
				}
				else
				{
					bool bHasFoundUnoccupiedSpace = false;
					
					while(!bHasFoundUnoccupiedSpace)
					{
						if(nRandomPositionIndex == arrgoPositions.Length - 1)
						{
							nRandomPositionIndex = 0;
						}
						else
						{
							nRandomPositionIndex++;
						}
						
						if(!arrgoPositions[nRandomPositionIndex].GetComponent<AvianCounterBirdPositionScript>().GetOccupied())
						{
							bHasFoundUnoccupiedSpace = true;
							arrgoBirdPrefabs[i].gameObject.GetComponent<AvianCounterBirdScript>().SetAssignedPosition(arrgoPositions[nRandomPositionIndex].transform.position);
							arrgoPositions[nRandomPositionIndex].GetComponent<AvianCounterBirdPositionScript>().SetOccupied(true);
						}
						
						
					}
				}
			}
				
		}
	}
	
	void FadeInBirds()
	{
		GameObject[] arrgoBirdPrefabs = GameObject.FindGameObjectsWithTag("BirdPrefab");
		
		for(int i = 0; i < arrgoBirdPrefabs.Length; i++)
		{
			arrgoBirdPrefabs[i].gameObject.GetComponent<AvianCounterBirdScript>().StartMoving();
		}
	}
	
	void SnapToPositionBirds()
	{
		GameObject[] arrgoBirdPrefabs = GameObject.FindGameObjectsWithTag("BirdPrefab");
		
		for(int i = 0; i < arrgoBirdPrefabs.Length; i++)
		{
			arrgoBirdPrefabs[i].gameObject.GetComponent<AvianCounterBirdScript>().GoToPosition();
		}
	}
	
}

			