using UnityEngine;
using System.Collections;

public class Section_Check : MonoBehaviour 
{
	public 	bool 	bCorrectAnswer;
	
	private	int 	index = 0;
	private	bool	bIconLoaded = false;
	
	//public int m_nScoreFactor = 10;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//What is to be done each time a new icon loads
		if(bIconLoaded == false)
		{
			//Reset correct answers. Game manager will set correct answer again
//			bCorrectAnswer = false;
			//Stop this from looping
			bIconLoaded = true;
		}
		
		if(GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().m_fTimeRemaining <= 0)
		{
			//Play Sound
			GameObject.Find("Sound_Wrong").audio.Play();
			//Delete loaded icon for next one to come in
			Destroy(GameObject.FindGameObjectWithTag("Loaded"));
			//Prepare next icon
			index++;
			//Give feedback
			StartCoroutine(DisplayResult("Wrong"));
			//Tell Game manager that there is no icon on screen for it to setup question
			Game_MixAndMatchManager.bIconLoaded = false;
			//Tell ourselves (lol) that the icon isn't loaded, run "loop" in update
			bIconLoaded = false;
			
			GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().m_fTimeRemaining = GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().m_nTimeLimit;
		}
	}
	
	void OnMouseUp()
	{
		//If it's the correct answer that's selected
		if(bCorrectAnswer == true)
		{
			//Play Sound
			GameObject.Find("Sound_Correct").audio.Play();
			//Delete loaded icon for next one to come in
			Destroy(GameObject.FindGameObjectWithTag("Loaded"));
			//Prepare next icon
			index++;
			//Give score
			
			GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().nScore += GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().m_nScoreFactor
				* int.Parse(GameObject.Find("TimePerTrial").GetComponent<TextMesh>().text);
			
			GameObject.Find("Score").GetComponent<TextMesh>().text = 
				GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().nScore.ToString();
			//Give feedback
			StartCoroutine(DisplayResult("Correct"));
			//Tell Game manager that there is no icon on screen for it to setup question
			Game_MixAndMatchManager.bIconLoaded = false;
			//Tell ourselves (lol) that the icon isn't loaded, run "loop" in update
			bIconLoaded = false;
			
			GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().m_nScoreFactor = 10;
			
			GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().m_fTimeRemaining = GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().m_nTimeLimit;
		}
		//If user selects a wrong answer (idiot)
		else
		{
			//Play Sound
			GameObject.Find("Sound_Wrong").audio.Play();
			//Give feedback
			StartCoroutine(DisplayResult("Wrong"));
			//Deduct total time to solve
			GameObject.Find("TimeCounter").GetComponent<Timer>().Seconds -= 10;
			
			if(GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().m_nScoreFactor != 0)
			{
				GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().m_nScoreFactor = GameObject.Find("Plane_Menu").GetComponent<Game_MixAndMatchManager>().m_nScoreFactor - 5;
			}
		}
	}
	
	IEnumerator DisplayResult(string _Result)
	{
		if(_Result == "Correct")
		{
			GameObject.Find("Plane_Correct").renderer.enabled = true;
			yield return new WaitForSeconds(0.8f);
			GameObject.Find("Plane_Correct").renderer.enabled = false;
		}
		else if(_Result == "Wrong")
		{
			GameObject.Find("Plane_Wrong").renderer.enabled = true;
			yield return new WaitForSeconds(0.8f);
			GameObject.Find("Plane_Wrong").renderer.enabled = false;
		}
	}
}


