using UnityEngine;
using System.Collections;

public class TimerAlertBlink : MonoBehaviour 
{
	public int 	nAlertThreshold;
	public bool bStartFlashing;
	
	// Use this for initialization
	void Start () 
	{
		bStartFlashing		= false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(gameObject.GetComponent<TextMesh>().text == "0" ||
			int.Parse(gameObject.GetComponent<TextMesh>().text) >  nAlertThreshold )
		{
			if ( bStartFlashing )
				StopAllCoroutines();
			
			bStartFlashing = false;
			gameObject.GetComponent<MeshRenderer>().renderer.enabled = true;
		}
		
		if(!bStartFlashing && gameObject.GetComponent<TextMesh>().text == nAlertThreshold.ToString())
		{
			bStartFlashing = true;
			StartCoroutine(FlashingTimer());
		}
	}
	
	IEnumerator FlashingTimer()
	{
		while(bStartFlashing)
		{
			gameObject.GetComponent<MeshRenderer>().renderer.enabled = true;
			yield return new WaitForSeconds(0.3f);
			gameObject.GetComponent<MeshRenderer>().renderer.enabled = false;
			yield return new WaitForSeconds(0.3f);
		}
	}
}
