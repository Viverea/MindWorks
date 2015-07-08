using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClassAnswerBox : MonoBehaviour 
{
	public GameObject m_goNumber;
	public GameObject m_goFireworks;
	public GameObject m_goExplosion;
	public GameObject m_goSparks;
	
	public int m_nPreviousNumber;
	public int m_nNumber = 0;
	public int m_nNumberAnswer = 0;
	
	// Use this for initialization
	void Start () 
	{
		m_nNumber = Random.Range (5, 21);
		m_nPreviousNumber = m_nNumber;
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_goNumber.GetComponent<TextMesh>().text = m_nNumber.ToString ();
		
		if(m_nPreviousNumber != m_nNumber)
		{
			if(m_nNumber < 0)
			{
				//m_nNumber = Random.Range (5, 21);
				
				//GameObject.Find ("ProgressBarManager").GetComponent<ClassProgression>().nCorrectStreak = 0;
				
				GameObject.Find ("ProgressBarManager").GetComponent<ClassProgression>().m_nScore -= 50;
				
				int index = Random.Range (11, 13);
				
				StartCoroutine(WordPop(index));
				
				StartCoroutine(Explosion(1.0f));
				
				//GameObject.Destroy(this.gameObject);
			}
			else if(m_nNumber == 0)
			{
				//m_nNumber = Random.Range (5, 21);
				
				//GameObject.Find ("ProgressBarManager").GetComponent<ClassProgression>().nCorrectStreak = GameObject.Find ("ProgressBarManager").GetComponent<ClassProgression>().nCorrectStreak + 2;
				
				GameObject.Find ("ProgressBarManager").GetComponent<ClassProgression>().m_nScore += 50;
				
				int index = Random.Range (1, 5);
				
				StartCoroutine(WordPop(index));
				
				StartCoroutine(Fireworks(1.0f));
				
				//GameObject.Destroy(this.gameObject);
			}
			else
			{
				if(GameObject.Find("GameManager").GetComponent<ClassCloudSortManager>().m_fTimeRemaining > 0.0f)
				{
					if(m_nPreviousNumber > 0)
					{
						StartCoroutine(Sparks(0.8f));
				
						//GameObject.Find ("ProgressBarManager").GetComponent<ClassProgression>().nCorrectStreak++;
				
						GameObject.Find ("ProgressBarManager").GetComponent<ClassProgression>().m_nScore += (10 * m_nNumberAnswer);
			
						int index = Random.Range (1, 5);
				
						StartCoroutine(WordPop(index));
					}
					
				}
			}
			
			m_nPreviousNumber = m_nNumber;
		}
	}
			
	IEnumerator Fireworks(float _fFireworksTime)
	{
		GameObject.Find("Sound_Correct").audio.Play();
		
		m_goFireworks.GetComponent<ParticleEmitter>().emit = true;
		
		yield return new WaitForSeconds(_fFireworksTime);
		
		m_goFireworks.GetComponent<ParticleEmitter>().emit = false;
		
		yield return new WaitForSeconds(1.0f);
		
		//GameObject.Destroy(this.gameObject);
		
		this.GetComponent<MeshRenderer>().enabled = false;
		this.GetComponent<BoxCollider>().enabled = false;
		m_goNumber.GetComponent<MeshRenderer>().enabled = false;
		
		yield return new WaitForSeconds(10.0f);
		
		this.GetComponent<MeshRenderer>().enabled = true;
		this.GetComponent<BoxCollider>().enabled = true;
		m_goNumber.GetComponent<MeshRenderer>().enabled = true;
		
		m_nNumber = Random.Range (5, 21);
		
		m_goNumber.GetComponent<TextMesh>().text = m_nNumber.ToString ();
		
		yield return null;
	}
	
	IEnumerator Explosion(float _fExplosionTime)
	{
		GameObject.Find("Sound_Wrong").audio.Play();
		
		m_goExplosion.GetComponent<ParticleEmitter>().emit = true;
		
		yield return new WaitForSeconds(_fExplosionTime);
		
		m_goExplosion.GetComponent<ParticleEmitter>().emit = false;
		
		yield return new WaitForSeconds(1.0f);
		
		//GameObject.Destroy(this.gameObject);
		
		this.GetComponent<MeshRenderer>().enabled = false;
		this.GetComponent<BoxCollider>().enabled = false;
		m_goNumber.GetComponent<MeshRenderer>().enabled = false;
		
		yield return new WaitForSeconds(10.0f);
		
		this.GetComponent<MeshRenderer>().enabled = true;
		this.GetComponent<BoxCollider>().enabled = true;
		m_goNumber.GetComponent<MeshRenderer>().enabled = true;
		
		m_nNumber = Random.Range (5, 21);
		
		m_goNumber.GetComponent<TextMesh>().text = m_nNumber.ToString ();
		
		yield return null;
	}
		
	IEnumerator Sparks(float _fSparksTime)
	{
		GameObject.Find("Sound_Correct").audio.Play();
		
		m_goSparks.GetComponent<ParticleEmitter>().emit = true;
		
		yield return new WaitForSeconds(_fSparksTime);
		
		m_goSparks.GetComponent<ParticleEmitter>().emit = false;
		
		yield return null;
	}
	
	IEnumerator WordPop(int _nIndex)
	{
		//GameObject goWord = Instantiate(Resources.Load("WordPop")) as GameObject;
		
		GameObject goFreeWord = new GameObject();
		
		Vector3 vOriginalPosition = Vector3.zero;
		Vector3 vOriginalScale = Vector3.zero;
		
		List <GameObject> agoWords = GameObject.Find ("WordPopArray").GetComponent<ClassWordManager>().m_agoWordPop;
		//agoWords.transform.position = GameObject.Find ("WordSpawnPoint").transform.position;
		
		for(int i = 0; i < agoWords.Count; i++)
		{
			if(agoWords[i].GetComponent<ClassWord>().m_bIsActive == false)
			{
				goFreeWord = agoWords[i];
				
				goFreeWord.GetComponent<ClassWord>().m_bIsActive = true;
				
				vOriginalPosition = goFreeWord.transform.position;
				vOriginalScale = goFreeWord.transform.localScale;
				
				i = agoWords.Count;
			}
		}
		
		goFreeWord.transform.position = GameObject.Find ("WordSpawnPoint").transform.position;
		
		switch(_nIndex)
		{
		case 1:
			
			goFreeWord.GetComponent<TextMesh>().text = "Well Done!";
			
			break;
			
		case 2:
			
			goFreeWord.GetComponent<TextMesh>().text = "Very Good!";
			
			break;
			
		case 3:
			
			goFreeWord.GetComponent<TextMesh>().text = "Fantastic!";
			
			break;
			
		case 4:
			
			goFreeWord.GetComponent<TextMesh>().text = "Masterful!";
			
			break;
			
			
			
		case 11:
			
			goFreeWord.GetComponent<TextMesh>().text = "Oh Dear!";
			
			break;
			
		case 12:
			
			goFreeWord.GetComponent<TextMesh>().text = "Try Again!";
			
			break;
		}
		
		//goWord.GetComponent<MeshRenderer>().enabled = true;
		
		float fWordTime = 1.0f;
		
		while ( fWordTime > 0 )
		{
			//goWord.GetComponent<TextMesh>().text = _sText;
			
			//goFreeWord.GetComponent<TextMesh>().fontSize = goFreeWord.GetComponent<TextMesh>().fontSize + (int)(5 * (fWordTime - (int)fWordTime));
			
			goFreeWord.transform.localScale = goFreeWord.transform.localScale + new Vector3(0.005f, 0.005f, 0.005f); 
			
			yield return new WaitForFixedUpdate();
			
			fWordTime = fWordTime - Time.deltaTime;
		}
		
		goFreeWord.transform.position = vOriginalPosition;
		
		goFreeWord.GetComponent<TextMesh>().fontSize = 100;
		
		goFreeWord.transform.localScale = vOriginalScale;
		
		goFreeWord.GetComponent<ClassWord>().m_bIsActive = false;
	}
}
