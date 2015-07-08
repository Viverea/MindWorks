using UnityEngine;
using System.Collections;

public class Game_MixAndMatchManager : MonoBehaviour 
{
	public 	static 	GameObject 	goObject;
	public 	static	bool		bIconLoaded;
	public	static	int[]		aIcons 	= new int[15];
	
	public  		int			nScore		= 0;
	private			int 		index 		= 0;
	
	public int m_nScoreFactor = 10;
	
	public float m_nTimeLimit;
	public float m_fTimeRemaining;
	
	private bool m_bCountdown				= true;
	
	public		GameObject[]	agoStuffToHide;
	
	// Use this for initialization
	void Start () 
	{
		GenerateIcons();
		GameObject.Find("Plane_Correct").renderer.enabled = false;
		GameObject.Find("Plane_Wrong").renderer.enabled = false;
		
		GameObject.Find("Count_Circle").renderer.enabled = false;
		GameObject.Find("Count_Square").renderer.enabled = false;
		GameObject.Find("Count_Triangle").renderer.enabled = false;
		
		bIconLoaded = false;
		
		m_fTimeRemaining = m_nTimeLimit;
		StartCoroutine(Countdown());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_bCountdown )
			return;
		
		GameObject.Find("Score").GetComponent<TextMesh>().text = nScore.ToString();
		
		if(bIconLoaded == false)
		{				
			//Setup Icon
			goObject = GameObject.Find("Prefab_Icon"+aIcons[index]);
			goObject.transform.localScale = new Vector3 (1.7f,1.7f,1.7f);
			goObject.transform.position = new Vector3(-4.0f,2.0f,95.0f);
			goObject.transform.localRotation = new Quaternion(1.0f,0.0f,0.0f,1.0f);
			goObject.tag = "Loaded";
			
			//Setup Question
			SetAnswers(GenerateQuestion());
			//Stop all icons from loading at once
			bIconLoaded = true;
			//Prepare next icon
			index++;
		}
		
		if(GameObject.Find("TimeCounter").GetComponent<TextMesh>().text == "0")
		{
			Application.LoadLevel(Application.loadedLevelName);	
		}
		
		m_fTimeRemaining = m_fTimeRemaining - Time.fixedDeltaTime;
		
		if(m_fTimeRemaining < 0.0f)
		{
			m_fTimeRemaining = 0.0f;
		}

		//GameObject.Find ("TimeRemaining").GetComponent<TextMesh>().text = m_fTimeRemaining.ToString("F2");
		GameObject.Find ("TimePerTrial").GetComponent<TextMesh>().text = m_fTimeRemaining.ToString("F0");

		
	}
	
	IEnumerator Countdown()
	{
		float countdown = 3.9999f;
		GameObject.Find("TextCountdown").renderer.enabled = true;
		
		foreach (GameObject go in agoStuffToHide)
			go.SetActive(false);
		
		while(countdown > -0.025f)
		{
			GameObject.Find("TimeCounter").GetComponent<Timer>().StartTimer = false;
			yield return new WaitForEndOfFrame();
			if ( countdown > 1 )
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = Mathf.Floor(countdown).ToString("##");
			else
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = "GO!";
			countdown -= Time.deltaTime;
		}
		
		GameObject.Find("TextCountdown").renderer.enabled = false;
		
		GameObject.Find("TimeCounter").GetComponent<Timer>().StartTimer = true;
		foreach (GameObject go in agoStuffToHide)
			go.SetActive(true);
		
		yield return new WaitForEndOfFrame();
		m_bCountdown = false;
	}
	
	//List of Icons
	void GenerateIcons()
	{
		//Initialize list of numbers
		for(int i = 0; i < aIcons.Length; i++)
		{
			aIcons[i] = i+1;	
		}
		
		//Go thru array
		for(int i = 0; i < aIcons.Length; i++)
		{
			//Only find randNum after the current number
			int nRandNum = Random.Range(i,aIcons.Length);//Random.Range(i+1,aIcons.Length);
			//Swap
			int temp = aIcons[i];
			aIcons[i] = aIcons[nRandNum];
			aIcons[nRandNum] = temp;
		}
	}
	
	string GenerateQuestion()
	{
		//Set shape to test
		int nRandNum = Random.Range(1,4);
		//Make sure shapes are all resetted before rendering right answer
		GameObject.Find("Count_Circle").renderer.enabled = false;
		GameObject.Find("Count_Triangle").renderer.enabled = false;
		GameObject.Find("Count_Square").renderer.enabled = false;
		
		//Depending on nRandNum above, the following will be tested
		switch(nRandNum)
		{
		case 1:
			//Turn on circle shape to test
			GameObject.Find("Count_Circle").renderer.enabled = true;
			//For 'SetAnswers' function
			return "Circle";
			break;
		case 2:
			GameObject.Find("Count_Square").renderer.enabled = true;
			return "Square";
			break;
		case 3:
			GameObject.Find("Count_Triangle").renderer.enabled = true;
			return "Triangle";
			break;
		}
		
		return "";
	}
	
	void SetAnswers(string _QuestionIcon)
	{
		//Pre-define correct answer
		int nCorrectAnswer = Random.Range(1,4);
		
		//If player has to count no. of [shape]
		if(_QuestionIcon == "Circle")
		{
			//Set that pre-defined correct section to true
			GameObject.Find("Section_"+nCorrectAnswer).GetComponent<Section_Check>().bCorrectAnswer = true;
			//Set the 3D text with the correct section to the right number
			GameObject.Find("Condition_"+nCorrectAnswer).GetComponent<TextMesh>().text = 
			goObject.GetComponent<Game_IconManager>().n_numberOfCircles.ToString();
			
			//Traverse thru sections 1 - 3
			for(int i = 1; i < 4; i++)
			{
				//Don't touch the correct answer
				if(i != nCorrectAnswer)
				{
					int nNewNumberVariable = 0;
					
					//Set the other numbers to be incorrect
					int nRandomOperationFactor = Random.Range (1, 3);
					GameObject.Find("Section_"+i).GetComponent<Section_Check>().bCorrectAnswer = false;
					
					switch(nRandomOperationFactor)
					{
					case 1:
						
						nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfCircles + i;
						
						if(nNewNumberVariable == goObject.GetComponent<Game_IconManager>().n_numberOfCircles)
						{
							nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfCircles + 1;
						}
						
						GameObject.Find("Condition_" + i).GetComponent<TextMesh>().text = nNewNumberVariable.ToString();
						
						break;
						
					case 2:
						
						nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfCircles - i;
					
						if(nNewNumberVariable < 0)
						{
							nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfCircles + i + i;
						}

						GameObject.Find("Condition_" + i).GetComponent<TextMesh>().text = nNewNumberVariable.ToString();
						
						break;
					}
					
				}
			}
		}
		//Same goes for the rest below
		else if(_QuestionIcon == "Square")
		{
			GameObject.Find("Section_"+nCorrectAnswer).GetComponent<Section_Check>().bCorrectAnswer = true;
			GameObject.Find("Condition_"+nCorrectAnswer).GetComponent<TextMesh>().text = 
			goObject.GetComponent<Game_IconManager>().n_numberOfSquares.ToString();
			
			for(int i = 1; i < 4; i++)
			{
				if(i != nCorrectAnswer)
				{
					int nNewNumberVariable = 0;
					
					//Set the other numbers to be incorrect
					int nRandomOperationFactor = Random.Range (1, 3);
					GameObject.Find("Section_"+i).GetComponent<Section_Check>().bCorrectAnswer = false;
					
					switch(nRandomOperationFactor)
					{
					case 1:
						
						nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfSquares + i;
					
						if(nNewNumberVariable == goObject.GetComponent<Game_IconManager>().n_numberOfSquares)
						{
							nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfSquares + i;
						}
						
						GameObject.Find("Condition_" + i).GetComponent<TextMesh>().text = nNewNumberVariable.ToString();
						
						break;
						
					case 2:
						
						nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfSquares - i;

						if(nNewNumberVariable < 0)
						{
							nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfSquares + i + i;
						}

						GameObject.Find("Condition_" + i).GetComponent<TextMesh>().text = nNewNumberVariable.ToString();
						
						break;
					}
					
				}
			}
		}
		else if(_QuestionIcon == "Triangle")
		{
			GameObject.Find("Section_"+nCorrectAnswer).GetComponent<Section_Check>().bCorrectAnswer = true;
			GameObject.Find("Condition_"+nCorrectAnswer).GetComponent<TextMesh>().text = 
			goObject.GetComponent<Game_IconManager>().n_numberOfTriangles.ToString();
			
			for(int i = 1; i < 4; i++)
			{
				if(i != nCorrectAnswer)
				{
					int nNewNumberVariable = 0;
					
					//Set the other numbers to be incorrect
					int nRandomOperationFactor = Random.Range (1, 3);
					GameObject.Find("Section_"+i).GetComponent<Section_Check>().bCorrectAnswer = false;
					
					switch(nRandomOperationFactor)
					{
					case 1:
						
						nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfTriangles + i;
					
						if(nNewNumberVariable == goObject.GetComponent<Game_IconManager>().n_numberOfTriangles)
						{
							nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfTriangles + i;
						}
						
						GameObject.Find("Condition_" + i).GetComponent<TextMesh>().text = nNewNumberVariable.ToString();
						
						break;
						
					case 2:
						
						nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfTriangles - i;
					
						if(nNewNumberVariable < 0)
						{
							nNewNumberVariable = goObject.GetComponent<Game_IconManager>().n_numberOfTriangles + i + i;
						}

						GameObject.Find("Condition_" + i).GetComponent<TextMesh>().text = nNewNumberVariable.ToString();
						
						break;
					}
					
				}
			}
		}
	}
}
