using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	public static string m_sHeading;
	public static string m_sScore;
	public static string m_sRank;
	
	public bool m_bVerifier = false;
	
	private bool m_bDrawText;
	
	// Use this for initialization
	void Start () 
	{
		m_bDrawText = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_bDrawText == false)
		{
			Debug.Log ("hmm..." + m_sRank);

			GameObject.Find ("Header").GetComponent<TextMesh>().text = "" + m_sHeading;
			GameObject.Find ("Score").GetComponent<TextMesh>().text =  "" + m_sScore;
			GameObject.Find ("Rank").GetComponent<TextMesh>().text = "" + m_sRank;
			
			m_bDrawText = true;
		}
	}
}
