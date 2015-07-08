using UnityEngine;
using System.Collections;

public class ClassSubsequentLevel : MonoBehaviour 
{
	public Texture m_tSecondFruit;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public int GenerateQuestion(int _nPreviousAnswer, bool _bHardLevel)
	{
		m_tSecondFruit = Resources.Load ("Apple") as Texture;
		
		int nSecondFruitIndex = Random.Range (1, 10);
			
		switch(nSecondFruitIndex)
		{
		case 1:
			m_tSecondFruit = Resources.Load ("Apple") as Texture;
			break;
			
		case 2:
			m_tSecondFruit = Resources.Load ("Banana") as Texture;
			break;
			
		case 3:
			m_tSecondFruit = Resources.Load ("Grape") as Texture;
			break;
			
		case 4:
			m_tSecondFruit = Resources.Load ("Mango") as Texture;
			break;
			
		case 5:
			m_tSecondFruit = Resources.Load ("Mangosteen") as Texture;
			break;
			
		case 6:
			m_tSecondFruit = Resources.Load ("Orange") as Texture;
			break;
			
		case 7:
			m_tSecondFruit = Resources.Load ("Pear") as Texture;
			break;
			
		case 8:
			m_tSecondFruit = Resources.Load ("Strawberry") as Texture;
			break;
			
		case 9:
			m_tSecondFruit = Resources.Load ("Watermelon") as Texture;
			break;

		default:
			m_tSecondFruit = Resources.Load ("Apple") as Texture;
			break;
		}
		
		
		int nFirstVariable = _nPreviousAnswer;
		int nSecondVariable = Random.Range(0, 10);
		
		string strOperator;
		int nOperatorIndex = Random.Range(0, 2);
		
		if(_bHardLevel == false)
		{
			nOperatorIndex = 1;
		}
		
		if(nOperatorIndex == 0)
		{
			strOperator = "-";
		}
		else
		{
			strOperator = "+";
		}
		
		GameObject.Find("PreviousAnswer").GetComponent<TextMesh>().text = "A = Previous Answer";
		GameObject.Find("FirstVariable").GetComponent<TextMesh>().text = "A";
		GameObject.Find("SecondVariable").GetComponent<TextMesh>().text = nSecondVariable.ToString ();
		GameObject.Find("Operator").GetComponent<TextMesh>().text = strOperator;
		
		Texture tNone = Resources.Load ("Blank") as Texture;
		GameObject.Find("FirstFruit").renderer.material.mainTexture = tNone;
		GameObject.Find("SecondFruit").renderer.material.mainTexture = m_tSecondFruit;
		
		int nFinalAnswer;
		
		if(nOperatorIndex == 0)
		{
			nFinalAnswer = nFirstVariable - nSecondVariable;
		}
		else
		{
			nFinalAnswer = nFirstVariable + nSecondVariable;
		}
		
		return nFinalAnswer;
	}
}
