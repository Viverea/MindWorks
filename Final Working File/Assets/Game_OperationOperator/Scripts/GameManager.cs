using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour 
{
	public	int					AmntOfOperators		= 2;
	public	int					AmntOfNumbers		= 3;
	public 	int 				MinNumber 			= 1;
	public 	int 				MaxNumber 			= 9;
	public 	int					AmntOfOperatorsUsed	= 1;
	public	static 	bool		bNumber				= true;
	private static 	string[] 	asOperators;
	public 	static 	int[] 		anNumbers 			= new int[5];
	public	 		int			nScore				= 0;
	
	private float				fTimer				= 0;
	private string 				sAnswerToGet;
	private int 				nAnswerToGet;
	private int					nTempAns;
	private Vector3				vRescale 		= new Vector3(0.5f,0.5f,0.5f);
	
	public GameObject[]			magoStuffToHide;
	
	// Use this for initialization
	void Start () 
	{
		bNumber		 = true;
		nAnswerToGet = 99999;
		StartCoroutine(StartGame());
//		InitialiseGame();
//		SetAnswer();
//		Scaling();
//		NumManager.bPressed = false;
//		GameObject.Find("Answer_Com").GetComponent<TextMesh>().text = nAnswerToGet.ToString();
//		GameObject.Find("Score").GetComponent<TextMesh>().text = nScore.ToString();
		fTimer = GameObject.Find("Time Counter").GetComponent<Timer>().Seconds + 1;
		
		foreach (GameObject go in magoStuffToHide)
			go.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(NumManager.bCheck) //if(GameObject.Find("Answer_Player").GetComponent<TextMesh>().text.Length == 3)
		{
			nTempAns = EvaluatePlayer(NumManager.nClickedNumber1, NumManager.nClickedNumber2, FindOperator());
			
			Debug.Log(NumManager.nClickedNumber1.ToString() + " + " + NumManager.nClickedNumber2.ToString());
			
			GameObject.Find("Answer_Player").GetComponent<TextMesh>().text = nTempAns.ToString();
			NumManager.nClickedNumber1 = nTempAns;
			NumManager.bCheck = false;
			bNumber = false;
		}
		
		if(nAnswerToGet == nTempAns)
		{
			//Application.LoadLevel(Application.loadedLevelName);
			ResetGame();
			ResetButtons();
			GameObject.Find("Sound_Correct").audio.Play();
			bNumber = true;
			nScore += 10;
			GameObject.Find("Score").GetComponent<TextMesh>().text = nScore.ToString();
		}
		
		if(GameObject.Find("Time Counter").GetComponent<TextMesh>().text == "0")
		{
			//Application.LoadLevel(Application.loadedLevelName);
			ResetGame();
			ResetButtons();
			
			GameObject.Find("Time Counter").GetComponent<Timer>().Seconds = fTimer;
		}
	}
	
	void ResetGame()
	{
//		StartCoroutine(Round());
//		GameObject.Find("Time Counter").GetComponent<Timer>().StartTimer = false;
		
		InitialiseGame();
		SetAnswer();
		Scaling();
		NumManager.bPressed = false;
		GameObject.Find("Answer_Com").GetComponent<TextMesh>().text = nAnswerToGet.ToString();
		GameObject.Find("Answer_Player").GetComponent<TextMesh>().text = "";
		GameObject.Find("Score").GetComponent<TextMesh>().text = nScore.ToString();
		GameObject.Find("Time Counter").GetComponent<Timer>().Seconds = fTimer;
		GameObject.Find("Time Counter").GetComponent<TextMesh>().color = Color.white;
	}
	
	IEnumerator StartGame()
	{
		float countdown = 3.9999f;
		GameObject.Find("TextCountdown").renderer.enabled = true;
		
		InitialiseGame();
		GameObject.Find("Answer_Player").GetComponent<TextMesh>().text = "";
		GameObject.Find("Answer_Com").GetComponent<TextMesh>().text = "0";
		GameObject.Find("Score").GetComponent<TextMesh>().text = nScore.ToString();
		GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = Mathf.Floor(countdown).ToString("##");
		
		while(countdown > -0.025f)
		{
			GameObject.Find("Time Counter").GetComponent<Timer>().StartTimer = false;
			yield return new WaitForEndOfFrame();
			if ( countdown > 1 )
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = Mathf.Floor(countdown).ToString("##");
			else
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = "GO!";
			countdown -= Time.deltaTime;
		}
		SetAnswer();
		Scaling();
		NumManager.bPressed = false;
		
		foreach (GameObject go in magoStuffToHide)
			go.SetActive(true);
		
		GameObject.Find("Answer_Com").GetComponent<TextMesh>().text = nAnswerToGet.ToString();
		GameObject.Find("TextCountdown").renderer.enabled = false;
		GameObject.Find("Time Counter").GetComponent<Timer>().StartTimer = true;
		
	}
	
	void ResetButtons()
	{
		GameObject goTemp = GameObject.Find("Group_Plane");
		NumManager[] oTemp = goTemp.transform.GetComponentsInChildren<NumManager>();
		
		for ( int n = 0; n < oTemp.Length; ++n )
		{
			oTemp[n].bPressMeBabyOneMoreTime = true;
		}
		nTempAns = 0x7fffffff;
	}
	
	private void Scaling()
	{
		if(MinNumber > 99 || MaxNumber > 99)
		{
			for(int i = 1; i <= AmntOfNumbers; i++)
			{
				GameObject.Find("Num"+(i)).transform.localScale = vRescale;
				GameObject.Find("Answer_Player").transform.localScale = vRescale;
				GameObject.Find("Answer_Com").transform.localScale = vRescale;
			}
		}
	}
	
	private void InitialiseGame()
	{
		for(int i = 0; i < AmntOfNumbers; i++)
		{
			anNumbers[i] = Random.Range(MinNumber,MaxNumber);
			GameObject.Find("Num"+(i+1)).GetComponent<TextMesh>().text = anNumbers[i].ToString();
		}	
		
		asOperators		= new string[AmntOfOperators];
		switch ( AmntOfOperators )
		{
		case 4:
			asOperators[3]	= OperatorManager.sDivide;
			goto case 3;
		case 3:
			asOperators[2]	= OperatorManager.sMultiply;
			goto case 2;
		case 2:
			asOperators[1]	= OperatorManager.sMinus;
			goto case 1;
		case 1:
			asOperators[0] = OperatorManager.sPlus;
			break;
		}
		
		for(int i = AmntOfNumbers+1; i <= 5; i++)
		{
			Destroy(GameObject.Find(("Plane"+i).ToString()));
			Destroy(GameObject.Find(("Num"+i).ToString()));
		}
	}
	
	private void SetAnswer()
	{
		int[] anNumberPool;
		// Clone our numbers array to our number pool
		anNumberPool = new int[anNumbers.Length];
		for ( int n = 0; n  < anNumbers.Length; ++n )
			anNumberPool[n] = anNumbers[n];
		
		int nRandomNo = Random.Range(0, AmntOfNumbers-1), nTemp;
		nTemp = anNumberPool[nRandomNo];
		anNumberPool[nRandomNo] = anNumberPool[0];
		anNumberPool[0] = nTemp;
		nRandomNo = Random.Range(1, AmntOfNumbers-1);
		nTemp = anNumberPool[nRandomNo];
		anNumberPool[nRandomNo] = anNumberPool[1];
		anNumberPool[1] = nTemp;
		
		nAnswerToGet = EvaluateCom(anNumberPool[0], anNumberPool[1]);
		
		for(int i = 2; i < AmntOfOperatorsUsed + 1; i++)
		{
			nRandomNo = Random.Range(i, AmntOfNumbers-1);
			nTemp = anNumberPool[nRandomNo];
			anNumberPool[nRandomNo] = anNumberPool[i];
			anNumberPool[i] = nTemp;
			
			nAnswerToGet = EvaluateCom(nAnswerToGet, anNumberPool[i]);
		}
		
		/*
		for(int i = 0; i < AmntOfOperatorsUsed; i++)
		{
			//So as to not get a negative for arrays
			nRandomNumber = Random.Range(0,AmntOfNumbers-1);
			int nRandomNumber2 = Random.Range (0, AmntOfNumbers-1);
			while(nRandomNumber == -1)
			{
				nRandomNumber = Random.Range(0,AmntOfNumbers-1);
			}
			while(nRandomNumber == nRandomNumber2)
				nRandomNumber2 = Random.Range (0, AmntOfNumbers-1);
			
			nAnswerToGet = EvaluateCom(anNumbers[nRandomNumber], anNumbers[nRandomNumber2]);
		}
		*/
	}
	
	private int EvaluateCom(int _nNumber1, int _nNumber2)
	{
		int randNum = Random.Range(1, AmntOfOperators+1);
		switch(randNum)
		{
		case 1:	
			Debug.Log(_nNumber1.ToString()+"+"+_nNumber2.ToString()+" = "+(_nNumber1+_nNumber2).ToString());
			return (_nNumber1 + _nNumber2);
			break;
		case 2:	
			Debug.Log(_nNumber1.ToString()+"-"+_nNumber2.ToString()+" = "+(_nNumber1-_nNumber2).ToString());
			return (_nNumber1 - _nNumber2);
			break;
		case 3:	
			Debug.Log(_nNumber1.ToString()+"*"+_nNumber2.ToString()+" = "+(_nNumber1*_nNumber2).ToString());
			return (_nNumber1 * _nNumber2);
			break;
		case 4:	
			Debug.Log(_nNumber1.ToString()+"÷"+_nNumber2.ToString()+" = "+(_nNumber1/_nNumber2).ToString());
			return (_nNumber1 / _nNumber2);
			break;
		default:
			print("Incorrect variable");
			break;
		}
		
		return 0;
	}
			
	private int FindOperator()
	{
		string[] strTemp = GameObject.Find("Answer_Player").GetComponent<TextMesh>().text.Split(' ');
		
		if(strTemp[1] == "+")
		{	return 1; }
		else if(strTemp[1] == "-")
		{	return 2; }
		else if(strTemp[1] == "*")
		{	return 3; }
		else if(strTemp[1] == "÷")
		{	return 4; }
		return 0;
	}
	
	private int EvaluatePlayer(int _nNumber1, int _nNumber2, int _nOperator)
	{
		switch(_nOperator)
		{
		case 1:	
			return (_nNumber1 + _nNumber2);
			break;
		case 2:	
			return (_nNumber1 - _nNumber2);
			break;
		case 3:	
			return (_nNumber1 * _nNumber2);
			break;
		case 4:	
			return (_nNumber1 / _nNumber2);
			break;
		default:
			print("Incorrect variable");
			break;
		}
		
		return 0;
	}
	
	//public static double Evaluate(string expression)  
	//{//Don't ask me how, i googled this up  
	//	return (double)new System.Xml.XPath.XPathDocument  
	//	(new System.IO.StringReader("<r/>")).CreateNavigator().Evaluate  
	//	(string.Format("number({0})", new  
	//	System.Text.RegularExpressions.Regex(@"([\+\-\*])").Replace(expression, " ${1} ").Replace("/", " div ").Replace("%", " mod ")));  
	//
	//}
	
	public static int RandomNumber()
	{
		int RandNum = Random.Range(0,asOperators.Length-1);
		return RandNum;
	}
}
