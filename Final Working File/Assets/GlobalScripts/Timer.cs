using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour 
{
	public float 	Seconds;
	public float	AlertThreshold;
	public float 	FlashInterval;
	public bool		StartTimer;
	
	// Use this for initialization
	void Start () 
	{
		AlertThreshold	= 5.0f;
		AlertThreshold 	+= 1;
		StartTimer = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Seconds < 0)
		{
			Seconds = 0;	
		}
		
		if(Seconds > 0 && StartTimer == true)
		{
			Seconds -= Time.deltaTime;
		}
		gameObject.GetComponent<TextMesh>().text = ((int)Seconds).ToString();
		
		Threshold();
	}
	
	void Threshold()
	{
	
	}
	
	IEnumerator FlashingTimer(float _fFlashInterval)
	{
		gameObject.GetComponent<TextMesh>().text = "";
		StartTimer = false;
		yield return new WaitForSeconds(_fFlashInterval);
		StartTimer = true;
		gameObject.GetComponent<TextMesh>().text = ((int)Seconds).ToString();
				
	}
}
