using UnityEngine;
using System.Collections;
using TMPro;

//---------------------
// Скрипт для меню игры
//---------------------
public class MenuGUIScript : MonoBehaviour {

	private const int PAGE_COUNT = 5;
	private const int TILE_ON_PAGE_COUNT = 24;

	//private MyGooglePlay googlePlay;	
	public GameObject playButton;
	public GameObject restartButton;
	public GameObject soundButton;
	public GameObject nextPanel;
	public GameObject emptyHolesPanel;
	public GameObject rulesPanel;
	public GameObject menuBackPanel;
	public GameObject logoPanel;
	//public GameObject socialPanel;
	public GameObject levelsPanel;
	public RectTransform levelsPanelContent;
	public TextMeshProUGUI soundText;
	public Color[] soundColor;
	public GameObject nextButton;
	public GameObject tutorPanel;
	public GameObject tutorHighligtPanel;

	public static MenuGUIScript instance { get; private set; }


	void Awake()
	{
		instance = this;
		//googlePlay = new MyGooglePlay ();
	}

	public void Update()
	{
		// переход на экран меню 
		if (Input.GetKey(KeyCode.Escape))
		{
			OnBackClick();
		}
	}

	public void OnBackClick()
	{
		if (levelsPanel.activeInHierarchy)
			MenuGUIScript.instance.ChangeScreenFromChooseLevelToMenu();
	}

	public void ShowTutor(bool value)
	{
		tutorPanel.SetActive(value);
		tutorHighligtPanel.SetActive(value);

		if (!value)
			PlayerPrefs.SetInt("Tutor", 1);
	}
	
	public void ChangeScreenFromGameToMenu()
	{
		SelectorScript.instance.enabled = false;
		MenuGUIScript.instance.enabled = true;
		TurnOnMenuButtons ();
	}
	
	public void ChangeScreenFromGameToNext()
	{
		SelectorScript.instance.enabled = false;
		MenuGUIScript.instance.enabled = true;
		Audio.instance.PlayWin();
		nextPanel.SetActive(true);
		TurnOnNextButtons ();
		menuBackPanel.SetActive(true);
	}
	
	public void ChangeScreenFromMenuToGame()
	{ 		
		if (MainGameScript.instance.planesDataList.Count == 0)
        {
			ChangeScreenFromMenuToChooseLevel();
			return;
        }

		//CameraScript.instance.RandomizeCameraRotation ();
		SelectorScript.instance.enabled = true;
		MenuGUIScript.instance.TurnOffMenuButtons();
	}
	
	public void ChangeScreenFromChooseLevelToMenu()
	{
		levelsPanel.SetActive(false);
		GameGUIScript.instance.ChangeGameGUIPropertiesByNumberOfLevel ();
		MenuGUIScript.instance.enabled = true;
		menuBackPanel.SetActive(true);
		logoPanel.SetActive(true);
	}
	
	public void ChangeScreenFromMenuToChooseLevel()
	{
		menuBackPanel.SetActive(false);
		logoPanel.SetActive(false);
		MenuGUIScript.instance.ChooseLevelPress();
	}
	
	public void ChangeScreenFromNextToChooseLevel()
	{
		SelectorScript.instance.enabled = false;
		ChooseLevelPress();
		TurnOffNextButtons();
		SwitchButtonsToNoRuned();
		GameGUIScript.instance.ChangeGameGUIPropertiesByNumberOfLevel();
		nextPanel.SetActive(false);
		menuBackPanel.SetActive(false);
		logoPanel.SetActive(false);
		//FindObjectOfType<RewardedAds>().Init();
	}
	
	public void TurnOffMenuButtons()
	{
		playButton.SetActive(false);
		restartButton.SetActive(false);
		soundButton.SetActive(false);
		rulesPanel.SetActive(false);
		menuBackPanel.SetActive(false);
		logoPanel.SetActive(false);
		//socialPanel.SetActive(false);
	}
	
	public void TurnOnMenuButtons()
	{
		playButton.SetActive(true);
		restartButton.SetActive(true);
		soundButton.SetActive(true);
		rulesPanel.SetActive(true);
		menuBackPanel.SetActive(true);
		logoPanel.SetActive(true);
		//socialPanel.SetActive(true);
	}
	
	public void TurnOffNextButtons()
	{
		nextButton.SetActive(false);
	}
	
	public void TurnOnNextButtons()
	{
		nextButton.SetActive(true);
	}
	
	public void SwitchButtonsToNoRuned()
	{
		playButton.SetActive(true);
		soundButton.SetActive(true);		
		restartButton.SetActive(false);
		rulesPanel.SetActive(true);
		menuBackPanel.SetActive(true);
		logoPanel.SetActive(true);
		//socialPanel.SetActive(true);

		MainGameScript.instance.planesDataList.Clear();
	}
	
	public void SetMusicButtonProperties()
	{
		if (MainGameScript.soundOn) 
		{ 
			soundText.text = Application.systemLanguage == SystemLanguage.Russian ? "ЗВУК: ВКЛ" : "SOUND: ON";
			soundText.color = soundColor[0];
			Audio.instance.VolumeOn();
		}
		else 
		{ 
			soundText.text = Application.systemLanguage == SystemLanguage.Russian ? "ЗВУК: ВЫКЛ" : "SOUND: OFF";
			soundText.color = soundColor[1];
			Audio.instance.VolumeOff();
		}
	}
	
	public void ChooseLevelPress()
	{
		RewindToOpenedLevelPage();
		levelsPanel.SetActive(true);
	}

	// перемотка на страницу текущего навыка
	public void RewindToOpenedLevelPage()
	{
		var pos = -((int)((MainGameScript.currentLevel - 1) / TILE_ON_PAGE_COUNT)) * 3840;
		levelsPanelContent.localPosition = new Vector3(pos, levelsPanelContent.localPosition.y, levelsPanelContent.localPosition.z);
	}

	public void MusicPress()
	{
		if (MainGameScript.soundOn) 
		{ 
			soundText.text = Application.systemLanguage == SystemLanguage.Russian ? "ЗВУК: ВЫКЛ" : "SOUND: OFF";
			soundText.color = soundColor[1];
			Audio.instance.VolumeOff();
			MainGameScript.soundOn = false;
			PlayerPrefs.SetInt("music", 0);
		}
		else 
		{ 
			soundText.text = Application.systemLanguage == SystemLanguage.Russian ? "ЗВУК: ВКЛ" : "SOUND: ON";
			soundText.color = soundColor[0];
			Audio.instance.VolumeOn();
			MainGameScript.soundOn = true; 
			PlayerPrefs.SetInt("music", 1);
		}
	}
	  
	public void GooglePlayPress()
	{
		//googlePlay.ShowRecords();
	}
}
