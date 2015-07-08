using UnityEngine;
using System.Collections;
using System;

public class gameKeypad : MonoBehaviour 
{
	public bool IsNum0 = false;
	public bool IsNum1 = false;
	public bool IsNum2 = false;
	public bool IsNum3 = false;
	public bool IsNum4 = false;
	public bool IsNum5 = false;
	public bool IsNum6 = false;
	public bool IsNum7 = false;
	public bool IsNum8 = false;
	public bool IsNum9 = false;
	
	public bool IsNegative 	= false;
	public bool IsConfirmed = false;
	public bool IsCancel = false;
	
	private	string 	sFinalAnswer; 
	private int		nFinalAnswer;	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		sFinalAnswer 	= GameObject.Find("Answer").GetComponent<TextMesh>().text;
		int.TryParse(sFinalAnswer,out nFinalAnswer);
		//MUST CONVERT STRING TO INT FOR COMPARISON OF ANSWERS
		//finalAnswer = int.Parse(GameObject.Find("Answer").GetComponent<TextMesh>().text);	
	} 
	
	void OnMouseUp()
	{	
		if(GameObject.Find("Question").GetComponent<TextMesh>().text == "?")
		{
			GameObject.Find("Question").GetComponent<TextMesh>().renderer.enabled = false;
			string 	sAnswer = GameObject.Find("Answer").GetComponent<TextMesh>().text;
			int 	nAnswer = theKeypad();
			
			if(IsCancel == true)
			{
				GameObject.Find("Answer").GetComponent<TextMesh>().text = "";
				GameObject.Find("Question").GetComponent<TextMesh>().text = "?";
				GameObject.Find("Question").renderer.enabled = true;
			}
			
			if(IsNegative == true)
			{
				string strAnswer = GameObject.Find("Answer").GetComponent<TextMesh>().text;
				
				if (strAnswer.Length != 0)
				{
					if(strAnswer[0] != '-')
					{
						GameObject.Find("Answer").GetComponent<TextMesh>().text = "-" + strAnswer;
					}
					else
					{
						GameObject.Find("Answer").GetComponent<TextMesh>().text = strAnswer.Substring(1);
					}
				}
				else
				{
					GameObject.Find("Answer").GetComponent<TextMesh>().text = "-";
				}
				
			}
			
			if(IsConfirmed == true)
			{
				if(GameObject.Find("Question").GetComponent<TextMesh>().text == "?")
				{
					if(nFinalAnswer == gameQuestion.nQuestion)
					{
						GameObject.Find("Answer").GetComponent<TextMesh>().color = Color.green;
						GameObject.Find("Sound_Correct").audio.Play();
						GameObject.Find("Score").GetComponent<TextMesh>().text = (10 * int.Parse(GameObject.Find("Time Counter").GetComponent<TextMesh>().text)).ToString();
					}
					else
					{
						GameObject.Find("Sound_Wrong").audio.Play();
						GameObject.Find("Question").GetComponent<TextMesh>().renderer.enabled = true;
						GameObject.Find("Answer").GetComponent<TextMesh>().renderer.enabled = false;
						GameObject.Find("Question").GetComponent<TextMesh>().text = gameQuestion.nQuestion.ToString();
					}
				}
				
				//Stop timer
				GameObject.Find("TimerManager").GetComponent<gameTimer>().StopTimer();
			}
			
			if(theKeypad() != 42)
			{
				GameObject.Find("Answer").GetComponent<TextMesh>().text += nAnswer.ToString();
			}
		}
		
	}
	
	public int theKeypad()//Keypad() is a reserved name?
	{
		if(IsNum0 == true)
		{	return 0; }
		else if(IsNum1 == true)
		{	return 1;	}
		else if(IsNum2 == true)
		{	return 2;	}
		else if(IsNum3 == true)
		{	return 3;	}
		else if(IsNum4 == true)
		{	return 4;	}
		else if(IsNum5 == true)
		{	return 5;	}
		else if(IsNum6 == true)
		{	return 6;	}
		else if(IsNum7 == true)
		{	return 7;	}
		else if(IsNum8 == true)
		{	return 8;	}
		else if(IsNum9 == true)
		{	return 9;	}
		else
			return 42;
	}
}
