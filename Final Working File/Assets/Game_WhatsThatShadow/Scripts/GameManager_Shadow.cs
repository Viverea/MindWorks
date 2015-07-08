using UnityEngine;
using System.Collections;

public class GameManager_Shadow : MonoBehaviour 
{
	public 	static 	int 			nCorrectAnswer 		= 0;
	public 	static 	string[]		asShadowNames 		= new string[20];					//Stores strings in alphabetical order
	public 	static 	GameObject[]	agoShadowNames		= new GameObject[20];				//Stores Gameobject 3D text
	public 	static 	GameObject[]	agoShadows			= new GameObject[20];				//Stores Gameobject Shadows
	public 	 		int				nScore				= 0;
	public			float			fPenalty			= 0.0f;
	
	private static 	int 			nRandNum 			= 0;
	private static 	int[]			numLockedList		= new int[asShadowNames.Length];
	private static 	Vector3			nameOffset			= new Vector3(0.0f,-3.5f,1.0f);
	
	public			GameObject[]	agoStuffToHide;
	
	// Use this for initialization
	IEnumerator Start () 
	{
		yield return StartCoroutine ( Countdown () );
		
		PositionSetup();
		nCorrectAnswer 	= Random.Range(0,asShadowNames.Length);
		Debug.Log(asShadowNames[nCorrectAnswer]);
		for(int i = 0; i < numLockedList.Length; i++)
		{	
			numLockedList[i] = -1;
		}
		Randomizer();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Timer
		if(GameObject.Find("Time_Counter").GetComponent<Timer>().Seconds < 0)
		{
			GameObject.Find("Time_Counter").GetComponent<Timer>().Seconds = 0;	
		}
		
		//Score
		GameObject.Find("Score").GetComponent<TextMesh>().text = nScore.ToString();
		
		if(GameObject.Find("Time_Counter").GetComponent<Timer>().Seconds == 0)
		{	
			GameObject.Find(asShadowNames[nCorrectAnswer]).renderer.material.color = Color.green;
		}
	}
	
	IEnumerator Countdown()
	{
		foreach (GameObject go in agoStuffToHide)
			go.renderer.enabled = false;
		
		float countdown = 3.9999f;
		GameObject.Find("TextCountdown").renderer.enabled = true;
		GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = "3";
		ShadowScript.m_bEnabled = false;
		
		while(countdown > -0.025f)
		{
			GameObject.Find("Time_Counter").GetComponent<Timer>().StartTimer = false;
			yield return new WaitForEndOfFrame();
			if ( countdown > 1 )
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = Mathf.Floor(countdown).ToString("##");
			else
				GameObject.Find("TextCountdown").GetComponent<TextMesh>().text = "GO!";
			countdown -= Time.deltaTime;
		}
		
		ShadowScript.m_bEnabled = true;
		GameObject.Find("TextCountdown").renderer.enabled = false;
		
		GameObject.Find("Time_Counter").GetComponent<Timer>().StartTimer = true;
		foreach (GameObject go in agoStuffToHide)
			go.renderer.enabled = true;
	}
	
	public void ResetGame()
	{
		StartCoroutine(Reset ());
	}
	
	IEnumerator Reset()
	{
		
		GameObject.Find("Time_Counter").GetComponent<Timer>().Seconds = 16;
		if(Application.loadedLevelName == "Game_WhatsThatShadow_Hard")
		{
			GameObject.Find("Time_Counter").GetComponent<Timer>().Seconds = 11;	
		}
		GameObject.Find("Time_Counter").GetComponent<TextMesh>().color = Color.white;
		for(int i = 0; i < numLockedList.Length; i++)
		{	
			numLockedList[i] = -1;
		}
		
		for(int i = 0; i < asShadowNames.Length; i++)
		{	
			GameObject.Find(GameManager_Shadow.asShadowNames[i])
				.renderer.material.color = Color.white;
		}
		
		Transform parent = GameObject.Find("aShadow_").transform;
		foreach(Transform child in parent)
		{
			child.renderer.material.color = Color.white;	
		}
		
		yield return StartCoroutine ( Countdown () );
		
		nCorrectAnswer 	= Random.Range(0,asShadowNames.Length);
		Debug.Log(asShadowNames[nCorrectAnswer]);
		SetPositions();
		Randomizer();
		GameObject.Find("Time_Counter").GetComponent<Timer>().StartTimer = true;
	}
	
	void PositionSetup()
	{
		asShadowNames[0] 	= "Bone";
		asShadowNames[1] 	= "Butterfly";
		asShadowNames[2] 	= "Car";
		asShadowNames[3] 	= "Cat";
		asShadowNames[4] 	= "Cloud";
		asShadowNames[5] 	= "Dog";
		asShadowNames[6] 	= "Feet";
		asShadowNames[7] 	= "Fire";
		asShadowNames[8] 	= "Fish";
		asShadowNames[9] 	= "Hand";
		asShadowNames[10] 	= "Key";
		asShadowNames[11] 	= "Leaf";
		asShadowNames[12] 	= "Lightbulb";
		asShadowNames[13] 	= "Moon";
		asShadowNames[14] 	= "Puzzle";
		asShadowNames[15] 	= "Scissors";
		asShadowNames[16] 	= "Snail";
		asShadowNames[17] 	= "Snowflake";
		asShadowNames[18] 	= "Sun";
		asShadowNames[19] 	= "Umbrella";

		//Loading arrays
		for(int i = 0; i < agoShadows.Length; i++)
		{
			//Placing the gameobjects in the same order as listed above
			agoShadows[i] = GameObject.Find(asShadowNames[i]);
			//Store all the 3D text inside
			agoShadowNames[i] = GameObject.FindGameObjectsWithTag("ShadowText")[i];
			//Set the 3D text to be that name and in same order as above
			agoShadowNames[i].name = "Name_"+asShadowNames[i];
		}
		
		SetPositions();
	}
	
	public static void SwapPositions(int _nRandNum1, int _nRandNum2)
	{
		Vector3 temp = new Vector3(0.0f,0.0f,0.0f);
		temp = agoShadowNames[_nRandNum1].transform.position;
		agoShadowNames[_nRandNum1].transform.position = agoShadowNames[_nRandNum2].transform.position;
		agoShadowNames[_nRandNum2].transform.position = temp;
	}
	
	public static void SwapPositionsIcons(int _nRandNum1, int _nRandNum2)
	{
		Vector3 temp = new Vector3(0.0f,0.0f,0.0f);
		temp = agoShadows[_nRandNum1].transform.position;
		agoShadows[_nRandNum1].transform.position = agoShadows[_nRandNum2].transform.position;
		agoShadows[_nRandNum2].transform.position = temp;
	}
	
	public static void Randomizer()
	{
		//Randomize positions of icons
		for(int i = 0; i < agoShadowNames.Length-1; i++)
		{
			int RandNum1 = Random.Range(1+i,agoShadowNames.Length);
			SwapPositionsIcons(i,RandNum1);
			//3D texts has to follow, for now
			SwapPositions(i,RandNum1);
		}
		
		//Now just randomize positions of 3D texts
		for(int i = 0; i < agoShadowNames.Length-1; i++)
		{
			//If it's the correct answer, ignore swapping
			if ( i != nCorrectAnswer )
			{
				int RandNum2 = Random.Range(1+i,agoShadowNames.Length);
				
				//If i is not the second last number (cuz of +1 below) 
				//and the correct answer is not the last number, run while loop
				//Why? To make sure Randnum2 is not stuck at the last number
				
				if ( !(i == agoShadowNames.Length-2 && nCorrectAnswer == agoShadowNames.Length-1 ) )
				{
					//If just so happen the random number is the correct answer, re roll
					while (RandNum2 == nCorrectAnswer)
					{
						RandNum2 = Random.Range(1+i,agoShadowNames.Length);
					}
					SwapPositions(i,RandNum2);
				}
			}
		}
	}
	
	public static void SetPositions()
	{	
		Vector3 vTemp = new Vector3(0.0f,0.0f,0.0f);
		for(int i = 0; i < 20; i++)
		{
			agoShadowNames[i].transform.position = agoShadows[i].transform.position;
			vTemp = agoShadowNames[i].transform.position;
			vTemp.y -= 0.18f;
			agoShadowNames[i].transform.position = vTemp;
		}
		
		for(int i = 0; i < asShadowNames.Length; i++)
		{
			agoShadowNames[i].GetComponent<TextMesh>().text = asShadowNames[i];
			agoShadowNames[i].transform.position += nameOffset;
		}
	}

}
