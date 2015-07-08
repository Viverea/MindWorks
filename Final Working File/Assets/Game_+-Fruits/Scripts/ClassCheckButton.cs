using UnityEngine;
using System.Collections;

public class ClassCheckButton : MonoBehaviour 
{
	
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnMouseDown()
	{
		int nCorrectAnswer = GameObject.Find ("GameManager").GetComponent<ClassForwardSumsGameManager>().m_nCurrentAnswer;
		string strInputAnswer = GameObject.Find ("TextBox").GetComponent<TextMesh>().text;
		
		int nInputAnswer = 0;
		
		bool bHasAnswer = System.Int32.TryParse(strInputAnswer, out nInputAnswer);
		
		
		
		if(bHasAnswer == true)
		{
			bool bIsNegative = GameObject.Find ("Button Minus").GetComponent<ClassButtonMinus>().m_bIsCurrentlyNegative;
			
			if(bIsNegative == true)
			{
				nInputAnswer = -nInputAnswer;
			}
			
			if(nInputAnswer == nCorrectAnswer)
			{
				Debug.Log ("Correct");
				
				GameObject.Find ("ProgressBarManager").GetComponent<ClassProgressionFruits>().nCorrectStreak++;
				
				GameObject.Find ("ProgressBarManager").GetComponent<ClassProgressionFruits>().m_nScore = GameObject.Find ("ProgressBarManager").GetComponent<ClassProgressionFruits>().m_nScore + 10;
				
				GameObject.Find ("Score").GetComponent<TextMesh>().text = GameObject.Find ("ProgressBarManager").GetComponent<ClassProgressionFruits>().m_nScore.ToString();

				GameObject.Find ("GameManager").GetComponent<ClassForwardSumsGameManager>().m_bHasCurrentLevelEndedCorrect = true;
				
				StartCoroutine(FlyIn());
			}
			else
			{
				Debug.Log ("Wrong");
				
				GameObject.Find ("ProgressBarManager").GetComponent<ClassProgressionFruits>().nCorrectStreak = 0;

				GameObject.Find ("GameManager").GetComponent<ClassForwardSumsGameManager>().m_bHasCurrentLevelEndedWrong = true;
			}
		}
		else
		{
			Debug.Log ("Wrong");
			
			GameObject.Find ("ProgressBarManager").GetComponent<ClassProgressionFruits>().nCorrectStreak = 0;

			GameObject.Find ("GameManager").GetComponent<ClassForwardSumsGameManager>().m_bHasCurrentLevelEndedWrong = true;
		}

		

	}
	
	
	IEnumerator FlyIn()
	{
		//GameObject goStar = Instantiate(Resources.Load("Star")) as GameObject;
		
		//GameObject.Find ("Apple").GetComponent<ClassFruit>().m_bIsMoving = true;
		//goStar.transform.position = GameObject.Find ("Waypoint1").transform.position;
		
		bool bFoundInactiveFruit = false;
		
		while(bFoundInactiveFruit == false)
		{
			int nFruitIndex = Random.Range (0, GameObject.Find ("FruitManager").GetComponent<ClassFruitManager>().m_agoFruits.Count);
			
			if(GameObject.Find ("FruitManager").GetComponent<ClassFruitManager>().m_agoFruits[nFruitIndex].GetComponent<ClassFruit>().m_bIsActive == false)
			{
				GameObject.Find ("FruitManager").GetComponent<ClassFruitManager>().m_agoFruits[nFruitIndex].GetComponent<ClassFruit>().m_bIsMoving = true;
				GameObject.Find ("FruitManager").GetComponent<ClassFruitManager>().m_agoFruits[nFruitIndex].GetComponent<ClassFruit>().m_bIsActive = true;
				
				bFoundInactiveFruit = true;
			}
		}
		
		
		yield return new WaitForSeconds(4.0f);
		
		StartCoroutine(ScoreFireworks(1.0f));
		
		yield return null;
	}
	
	IEnumerator ScoreFireworks(float _fFireworksTime)
	{
		GameObject.Find ("ScoreFireworks").GetComponent<ParticleEmitter>().emit = true;
		
		yield return new WaitForSeconds(_fFireworksTime);
		
		GameObject.Find ("ScoreFireworks").GetComponent<ParticleEmitter>().emit = false;
		
		yield return null;
	}
}
