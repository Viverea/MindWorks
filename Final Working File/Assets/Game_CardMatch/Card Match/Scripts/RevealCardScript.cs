using UnityEngine;
using System.Collections;

public class RevealCardScript : MonoBehaviour 
{

	// Use this for initialization
	public void Start () 
	{
		StartCoroutine(Reveal ());
	}
	
	IEnumerator Reveal()
	{
		yield return new WaitForSeconds (1.0f);
	
		animation.Play("reveal");
		
		yield return new WaitForSeconds (3.0f);
		
		animation.Play("hide");
	}
}
