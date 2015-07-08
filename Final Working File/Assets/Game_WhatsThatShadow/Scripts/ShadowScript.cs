using UnityEngine;
using System.Collections;

public class ShadowScript : MonoBehaviour 
{
	public static bool m_bEnabled = false;
	
	// Use this for initialization
	void Start () 
	{	
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnMouseUp()
	{	
		if ( m_bEnabled )
			if(gameObject.name == GameManager_Shadow.asShadowNames[GameManager_Shadow.nCorrectAnswer])
			{
				GameObject.Find("Sound_Correct").audio.Play();
				gameObject.renderer.material.color = Color.green;
				GameObject.Find("Time_Counter").GetComponent<Timer>().StartTimer = false;
				GameObject.Find("Plane_Menu").GetComponent<GameManager_Shadow>().nScore += (int)GameObject.Find("Time_Counter").GetComponent<Timer>().Seconds * 10;
				//Application.LoadLevel(Application.loadedLevelName);
				GameObject.Find("Plane_Menu").GetComponent<GameManager_Shadow>().ResetGame();
				
			}
			else if (GameObject.Find("Time_Counter").GetComponent<Timer>().StartTimer == true)
			{
				GameObject.Find("Sound_Wrong").audio.Play();
				gameObject.renderer.material.color = Color.red;
				GameObject.Find("Time_Counter").GetComponent<Timer>().Seconds -= 
					GameObject.Find("Plane_Menu").GetComponent<GameManager_Shadow>().fPenalty;
			}
	}
}
