using UnityEngine;
using System.Collections;

public class ClassLinkButton : MonoBehaviour 
{
	public string m_strGameName = "";
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_strGameName = Menu_NewPlayButton.sSelectedLevel;
	}
	
	void OnMouseDown()
	{
		switch(m_strGameName)
		{
		case "_AvianCounter":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#aviancounter");
			break;
			
		case "_CardMatch":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#cardmatch");
			break;
			
		case "_CloudSort":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#cloudsort");
			break;
			
		case "_ColourSequencing":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#coloursequence");
			break;
			
		case "_Conveyor":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#conveyorbelt");
			break;
			
		case "_Dice":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#dicegame");
			break;
			
		case "_FlySwatter":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#flyswatter");
			break;
			
		case "_ForwardSums":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#fruits");
			break;
			
		case "_MentalMath":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#mentalmath");
			break;
			
		case "_NumberTapper":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#numbertapper");
			break;
			
		case "_OperationOperator":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#operationoperator");
			break;
			
		case "_ShapeSorting":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#shapesorting");
			break;
			
		case "_WhatsThatShadow":
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions/#whatisthatshadow");
			break;
			
		default:
			Application.OpenURL("http://mindworks2014instructions.wordpress.com/2014/05/09/mindworks-2014-instructions");
			break;
		}
	}
}
