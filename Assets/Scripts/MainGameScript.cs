using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using YG;

//---------------------------------------------------------------------
// Главный скрипт игры. Загружает настройки, хранит данные всех деталей,
// проверяет корректность решения уровня игроком
//---------------------------------------------------------------------
public class MainGameScript : MonoBehaviour
{

	public static float maxLevel = 120; // количество уровней в игре	
	public static float speed = 1.95f; // настройка для движения деталей, плиток, смены экранов(чем больше тем быстрее) 
	public static bool soundOn; // настройка звука
	public static int openedLevel; // настройка последнего открытого уровня
	public static int currentLevel; // номер уровня запущеного во время игры
	public List<GameObject> planesList; // список всех перемещаемых деталей уровня
	public List<PlanesDataScript> planesDataList; // список данных перемещаемых деталей уровня
	public GameObject finalCheker; // объект для проверки что не осталось пустых ячеек при завершении уровня

	public static MainGameScript instance { get; private set; }
	public int commonCountOfPlanes { get;  set; }  // не считая недвигающихся деталей


	void Start ()
	{
		instance = this;

		commonCountOfPlanes = 10; // изначально до загрузки уровня

		CheckFirstRun ();
		LoadMusicSettings ();
		LoadLevelsNumberSettings ();
		//MenuGUIScript.SetMusicButtonProperties ();
		GameGUIScript.instance.ChangeGameGUIPropertiesByNumberOfLevel ();

		Input.multiTouchEnabled = false;
	}
	
	// проверяет все ли детали корректно установлены и если да то запускает проверку на пустые ячейки
	public void CheckIsAllStand ()
	{
		int countOfStandingPlanes = 0;
		
		foreach (PlanesDataScript planeData in planesDataList) 
		{
			if (planeData.checkScript.isStand) 
			{ 
				countOfStandingPlanes++; 
			}
		}
		
		if (countOfStandingPlanes >= commonCountOfPlanes) 
		{
			finalCheker.SetActive (true);
		}
	}

	public void GoToNextLevel()
	{
		increaseLevelNumber ();
		MenuGUIScript.instance.ChangeScreenFromGameToNext ();		
		SelectorScript.instance.enabled = false;
		saveLevelSettings ();
	}
	
	public void sortPlanesPosition(PlanesDataScript upperPlaneData, float posY, float delta)
	{
		// перемещаем на верх массива
		planesDataList.Remove(upperPlaneData);
		planesDataList.Add(upperPlaneData);
		
		// смещаем все детали вниз относительно вверхней
		float diffPositionY = 0;	
		int sortIndex = 0;	
		for (int i = planesDataList.Count - 1; i >= 0; i--) 
		{
			planesDataList[i].thisTransform.position = new Vector3 (planesDataList[i].thisTransform.position.x,
			                                                        posY + planesDataList[i].movePlaneScript.typeDiffPos - diffPositionY,
			                                                        planesDataList[i].thisTransform.position.z);
			planesDataList[i].sortIndex = sortIndex;
			diffPositionY += delta;
			sortIndex++;
		}
	}

	private void CheckFirstRun()
	{    
#if UNITY_WEBGL
		if (LevelLoaderScript.alreadyRuned) 
		{
			if (currentLevel == 1 && YG2.saves.Tutor == 0)
            {
				MenuGUIScript.instance.ShowTutor(true);
            }

			return;
		}
#else
		if (LevelLoaderScript.alreadyRuned) 
		{
			if (currentLevel == 1 && PlayerPrefs.GetInt("Tutor", 0) == 0)
            {
				MenuGUIScript.instance.ShowTutor(true);
            }

			return;
		}
#endif

		// при самом первом запуске открывается меню и нельзя перейти к игровому экрану
		SelectorScript.instance.enabled = false;
		MenuGUIScript.instance.SwitchButtonsToNoRuned();
		CameraScript.instance.SetCameraPositionToNormal();
	}

	// настройка звука загружается из Preferences
	private void LoadMusicSettings ()
	{
		if (PlayerPrefs.HasKey ("music")) 
		{
			if (PlayerPrefs.GetInt ("music") == 0) 
				soundOn = false; 
			else 
				soundOn = true; 
		}
		else 
		{ 
			soundOn = true; 
		}

		MenuGUIScript.instance.SetMusicButtonProperties();
	}

	// открытый уровень загружается из Preferences, текущий уровень тоже если загружается не из меню выбора уровней
	private void LoadLevelsNumberSettings ()
	{
#if UNITY_WEBGL
		openedLevel = YG2.saves.level;
#else
		openedLevel = PlayerPrefs.GetInt ("level");
#endif
		if (openedLevel == 0) 
		{ 
			openedLevel = 1; 
		}
		//openedLevel = 100;
		if (MainGameScript.currentLevel == 0) 
		{ 
			MainGameScript.currentLevel = openedLevel; 
		}
	}

	private void increaseLevelNumber ()
	{
		LevelLoaderScript.UndoSkipLevel(MainGameScript.currentLevel);
		if (MainGameScript.currentLevel < maxLevel)
		{
			MainGameScript.currentLevel++;
		}
	}

	private void saveLevelSettings ()
	{
		if (openedLevel < MainGameScript.currentLevel) 
		{ 
#if UNITY_WEBGL
			YG2.saves.level = MainGameScript.currentLevel;
			YG2.SaveProgress();
#else
			PlayerPrefs.SetInt ("level", MainGameScript.currentLevel); 
#endif
			openedLevel = MainGameScript.currentLevel;
		}
	}
}
