using UnityEngine;
using System.Collections;

public class ClassStartingLevel : MonoBehaviour 
{
	public Texture m_tFirstFruit;
	public Texture m_tSecondFruit;
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public int GenerateQuestion(bool _bHardLevel)
	{
		m_tFirstFruit = Resources.Load ("Apple") as Texture;
		m_tSecondFruit = Resources.Load ("Apple") as Texture;
		
		int nFirstFruitIndex = Random.Range (1, 10);
			
		switch(nFirstFruitIndex)
		{
		case 1:
			m_tFirstFruit = Resources.Load ("Apple") as Texture;
			break;
			
		case 2:
			m_tFirstFruit = Resources.Load ("Banana") as Texture;
			break;
			
		case 3:
			m_tFirstFruit = Resources.Load ("Grape") as Texture;
			break;
			
		case 4:
			m_tFirstFruit = Resources.Load ("Mango") as Texture;
			break;
			
		case 5:
			m_tFirstFruit = Resources.Load ("Mangosteen") as Texture;
			break;
			
		case 6:
			m_tFirstFruit = Resources.Load ("Orange") as Texture;
			break;
			
		case 7:
			m_tFirstFruit = Resources.Load ("Pear") as Texture;
			break;
			
		case 8:
			m_tFirstFruit = Resources.Load ("Strawberry") as Texture;
			break;
			
		case 9:
			m_tFirstFruit = Resources.Load ("Watermelon") as Texture;
			break;

		default:
			m_tFirstFruit = Resources.Load ("Apple") as Texture;
			break;
		}
		
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
		
		Debug.Log(nFirstFruitIndex.ToString() + nSecondFruitIndex.ToString());
		
		int nFirstVariable = Random.Range(0, 10);
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
		
		GameObject.Find("PreviousAnswer").GetComponent<TextMesh>().text = "";
		GameObject.Find("FirstVariable").GetComponent<TextMesh>().text = nFirstVariable.ToString ();
		GameObject.Find("SecondVariable").GetComponent<TextMesh>().text = nSecondVariable.ToString ();
		GameObject.Find("Operator").GetComponent<TextMesh>().text = strOperator;
		
		GameObject.Find("FirstFruit").renderer.material.mainTexture = m_tFirstFruit;
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
