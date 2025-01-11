using UnityEngine;
using System.Collections;
using System;


//----------------------------------------
// Класс для работы с Google Play Services
//----------------------------------------
public class MyGooglePlay 
{
	public void ShowRecords()
	{		
		/*if (!Social.localUser.authenticated) 
		{ 
			GooglePlayGames.PlayGamesPlatform.Activate ();			
			Social.localUser.Authenticate (authenticated => 
			{
				if (!authenticated || !Social.localUser.authenticated) 
				{
					throw new Exception ();
				}
				ReportScore();
			});
		}
		else 
		{
			ReportScore();
		}*/
	}
	
	private void ReportScore()
	{
		/*Social.ReportScore(MainGameScript.openedLevel, "CgkI5MbJjN4YEAIQBg", (bool success) => 
		{
			if (success) 
			{ 
				((GooglePlayGames.PlayGamesPlatform) Social.Active).ShowLeaderboardUI("CgkI5MbJjN4YEAIQBg"); 
			}
			else 
			{ 
				throw new Exception(); 
			}
		});*/
	}
}
