using UnityEngine;
using System.Collections;


//--------------------------------------------
// Скрипт для кнопки Google Play Game Services
//--------------------------------------------
public class GooglePlayButtonScript : MonoBehaviour 
{

	// если пользователь нажал на кнопку
	void OnTriggerEnter(Collider otherTrigger) 
	{	
		if (otherTrigger.tag == "Selector")
		{
			MenuGUIScript.instance.GooglePlayPress ();
		}
	}
}
