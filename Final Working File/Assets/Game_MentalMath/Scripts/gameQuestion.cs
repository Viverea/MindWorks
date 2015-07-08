using UnityEngine;
using System.Collections;

public class gameQuestion : MonoBehaviour 
{
	public 			int 	maxNumOfQuestions;
	public 			float	timeInterval;
	public			int		lowestNumber;
	public			int		highestNumber;
	public static 	int 	nQuestion;
	
	private 		int 	nRandNum;
	private 		int 	finalNum;
	private 		int[]	arrayNum;
	private			float	waitTime;
	private			bool	checkNow;
	private			Vector3	disableKeypad;
	
	// Use this for initialization
	void Start () 
	{
		nQuestion = 0;
		waitTime = 2.5f;
		checkNow = false;
		
		//if(external.roundNum == 2)
		//{
		//	highestNumber = 20;
		//}
		//if(external.roundNum == 3)
		//{
		//	highestNumber = 20;
		//	lowestNumber = -10;
		//}
		//if(external.roundNum == 4)
		//{
		//	highestNumber = 20;
		//	lowestNumber = -10;
		//	maxNumOfQuestions = 20;
		//}
		//if(external.roundNum >= 5)
		//{
		//	highestNumber = 20;
		//	lowestNumber = -10;
		//	maxNumOfQuestions = 20;
		//	timeInterval = 2.0f;
		//}
		
		StartCoroutine(Countdown ());
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Check timer
		if(GameObject.Find("TimerManager").GetComponent<gameTimer>().HasReachedTimeLimit())
		{
			TimesUp();
		}
		
		//GameObject.Find("RoundNumber").GetComponent<TextMesh>().text = "LEVEL: "+external.roundNum.ToString();
		StartCoroutine(NewRound(waitTime));

	}
	
	IEnumerator NewRound(float _waitTime)
	{
		if(GameObject.Find("Question").GetComponent<TextMesh>().text == "?")
		{
			checkNow = true;
		}
		if(checkNow == true)
		{
			if(GameObject.Find("Answer").GetComponent<TextMesh>().color == Color.green ||
				GameObject.Find("Question").GetComponent<TextMesh>().text == nQuestion.ToString()) //It's always the same, that's why it's always restarting
			{
				yield return new WaitForSeconds(_waitTime);
				external.roundNum++;
				Application.LoadLevel(Application.loadedLevelName);
			}
		}
	}
	
	IEnumerator Question(float _timeInterval)
	{
		arrayNum			= new int[maxNumOfQuestions];
		for(int i = 0; i < maxNumOfQuestions; i++)
		{	
			int nRandNum = Random.Range(lowestNumber,highestNumber+1);
			arrayNum[i] = nRandNum;
			
			nQuestion = nQuestion + arrayNum[i];
			
			//if it's the first number or it's a negative
			if(i==0 || arrayNum[i] < 0)
			{
				//Without the + sign at the front
				GameObject.Find("Question").GetComponent<TextMesh>().text = arrayNum[i].ToString();
			}
			else
			{
				GameObject.Find("Question").GetComponent<TextMesh>().text = "+"+arrayNum[i].ToString();	
			}
			yield return new WaitForSeconds(_timeInterval);
			GameObject.Find("Question").GetComponent<TextMesh>().text = " ";
			yield return new WaitForSeconds(0.3f);	// AHHHHH, now i know the importance of the 'f' at the back.
													// No 'f', system assume double;
			
			
			//set time interval
				//within time interval, display arrayNum[i]
				//if arrayNum[i + 1] > arrayNum[i]
				//display operator "+"
		}
		GameObject.Find("Question").GetComponent<TextMesh>().text = "?";
		//Debug.Log("Question: "+nQuestion);	
		
		//Start game timer
		GameObject.Find("TimerManager").GetComponent<gameTimer>().StartTimer();
	
	}
	
	IEnumerator Countdown()
	{
		GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(false);
		GameObject.Find("Countdown").renderer.enabled = true;
		
		GameObject.Find("Countdown").GetComponent<TextMesh>().text = "3";
		yield return new WaitForSeconds(1.0f);
		GameObject.Find("Countdown").GetComponent<TextMesh>().text = "2";
		yield return new WaitForSeconds(1.0f);
		GameObject.Find("Countdown").GetComponent<TextMesh>().text = "1";
		yield return new WaitForSeconds(1.0f);
		GameObject.Find("Countdown").GetComponent<TextMesh>().text = "GO!";
		yield return new WaitForSeconds(1.0f);
		
		GameObject.Find("Countdown").renderer.enabled = false;
		GameObject.Find("DisableStuff").GetComponent<DisableStuff>().SetObjectsActive(true);
		
		StartCoroutine(Question(timeInterval));

	}
	
	void TimesUp()
	{
		if(GameObject.Find("Question").GetComponent<TextMesh>().text == "?")
		{
			int nParsedInt = 0;
			bool bTryParse = int.TryParse(GameObject.Find("Answer").GetComponent<TextMesh>().text, out nParsedInt);
			
			if(bTryParse)
			{
				if(nParsedInt == nQuestion)
				{
					GameObject.Find("Answer").GetComponent<TextMesh>().color = Color.green;
					GameObject.Find("Sound_Correct").audio.Play();
				}
				else
				{
					GameObject.Find("Sound_Wrong").audio.Play();
					GameObject.Find("Question").GetComponent<TextMesh>().renderer.enabled = true;
					GameObject.Find("Answer").GetComponent<TextMesh>().renderer.enabled = false;
					GameObject.Find("Question").GetComponent<TextMesh>().text = gameQuestion.nQuestion.ToString();
				}
			}
			else
			{
				GameObject.Find("Sound_Wrong").audio.Play();
				GameObject.Find("Question").GetComponent<TextMesh>().renderer.enabled = true;
				GameObject.Find("Answer").GetComponent<TextMesh>().renderer.enabled = false;
				GameObject.Find("Question").GetComponent<TextMesh>().text = gameQuestion.nQuestion.ToString();
			}
		}
	}
}
