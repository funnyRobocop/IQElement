using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

//-----------------------------------------
// Скрипт для загрузки уровней из xml файла
//-----------------------------------------
public class LevelLoaderScript : MonoBehaviour
{
	private const float START_SORT_Y = 40; // высота от которой происходит расположение сортированных деталей
	private const float DELTA_SORT = 4; // интервал между деталями при их сортировке
	private const float WAITING_TIME = 0.05f; // время перед апуском недвигающихся деталей

	public TextAsset LevelAsset; // xml файл уровня, содержит координаты фигур и их углы вращения
	
	private List<Dictionary<string,string>> listPlanesValues = 
		new List<Dictionary<string,string>>(); // данные фигур из файла
	private List<MoveUntachablePlanesScript> allPlaneScripts = 
		new List<MoveUntachablePlanesScript>(); // данные фигур из инпектора объектов
	public static bool alreadyRuned; // запускалась ли уже игра

	public MenuGUIScript menuGUIScript;
	//public RewardedAds rewardedAds;

	void Start()
	{
		GameGUIScript.instance.HideAdBtn();
		// при первом запуске не нужно загружать детали, так как загружается только меню
		if (alreadyRuned)
		{
			GetLevel (MainGameScript.currentLevel);
			LoadLevel ();

			/*rewardedAds.Init();
			rewardedAds.OnUnityAdsShowCompleted += SkipLevel;
			rewardedAds.OnUnityAdsShowCompleted += menuGUIScript.ChangeScreenFromNextToChooseLevel;*/
#if UNITY_WEBGL
		//Todo
#else
			AdsInterstitial.Instance.ShowInterstitial();
			AdsRewarded.Instance.RequestRewardedAd();
			AdsRewarded.Instance.OnSuccess += SkipLevel;
			AdsRewarded.Instance.OnSuccess += menuGUIScript.ChangeScreenFromNextToChooseLevel;
			AdsRewarded.Instance.OnLoaded += GameGUIScript.instance.ShowAdBtn;
			AdsRewarded.Instance.OnFail += GameGUIScript.instance.HideAdBtn;
#endif
		}
		else
		{
			alreadyRuned = true;
		}
	}

	void OnDestroy()
	{
#if UNITY_WEBGL
		//Todo
#else
		if (AdsRewarded.Instance == null)
			return;

		AdsRewarded.Instance.OnSuccess -= SkipLevel;

		if (menuGUIScript != null)
			AdsRewarded.Instance.OnSuccess -= menuGUIScript.ChangeScreenFromNextToChooseLevel;

		if (GameGUIScript.instance != null)
		{
			AdsRewarded.Instance.OnLoaded -= GameGUIScript.instance.ShowAdBtn;
			AdsRewarded.Instance.OnFail -= GameGUIScript.instance.HideAdBtn;
		}
#endif
	}
	
	public void SkipLevel()
    {
		GameGUIScript.instance.HideAdBtn();
        var skipStr = PlayerPrefs.GetString("skip", string.Empty);

        var skipStrList = skipStr.Split('_').ToList();

        var skipIntList = new List<int>();
        foreach (var item in skipStrList)
        {
            int.TryParse(item, out int level);

            if (level > 0 && !skipIntList.Contains(level))
                skipIntList.Add(level);
        }

        skipIntList.Add(MainGameScript.currentLevel);

        var newSkipStrList = string.Empty;
        foreach (var item in skipIntList)
        {
            newSkipStrList += item.ToString() + "_";
        }

        PlayerPrefs.SetString("skip", newSkipStrList);

        MainGameScript.openedLevel++;
        MainGameScript.currentLevel++;
        PlayerPrefs.SetInt("level", MainGameScript.openedLevel);		
    }
	
	public static void UndoSkipLevel(int levelNumber)
    {
        var skipStr = PlayerPrefs.GetString("skip", string.Empty);

        var skipStrList = skipStr.Split('_').ToList();

        var skipIntList = new List<int>();
        foreach (var item in skipStrList)
        {
            int.TryParse(item, out int level);

            if (level > 0 && !skipIntList.Contains(level))
                skipIntList.Add(level);
        }

		if (skipIntList.Contains(levelNumber))
        	skipIntList.Remove(levelNumber);

        var newSkipStrList = string.Empty;
        foreach (var item in skipIntList)
        {
            newSkipStrList += item.ToString() + "_";
        }

        PlayerPrefs.SetString("skip", newSkipStrList);
    }

	// загружаем данные для уровня из файла
	public void GetLevel(int level)
	{
		XmlDocument xmlDoc = new XmlDocument ();
		xmlDoc.LoadXml (LevelAsset.text);		
		XmlNodeList levelContent = xmlDoc.GetElementsByTagName ("level" + level)[0].ChildNodes;

		foreach (XmlNode entity in levelContent)
		{
			// записываем данные деталей для нужного уровня в planesValues
			Dictionary<string,string> planesValues = new Dictionary<string,string> ();

			planesValues.Add ("number", entity.Attributes["number"].Value);
			planesValues.Add ("px", entity.Attributes["px"].Value);
			planesValues.Add ("pz", entity.Attributes["pz"].Value);
			planesValues.Add ("rx", entity.Attributes["rx"].Value);
			planesValues.Add ("ry", entity.Attributes["ry"].Value);

			// добавляем данные деталей для нужного уровня в массив
			listPlanesValues.Add(planesValues);
		}
	}

	// загружаем уровень, а именно располагаем все детали так как нужно
	public void LoadLevel()
	{
		GetPlanesData ();
		RemoveUntouchablePlanesFromMainMassiv ();
		AddScriptsToOtherPlanes ();
		StartCoroutine (MoveUntoucheblesPlanesToBoard ());
	}

	private int GetPlaneNumber (int index)
	{
		string levelNumberFromFile;
		listPlanesValues[index].TryGetValue("number", out levelNumberFromFile);
		return int.Parse(levelNumberFromFile) - 1;
	}

	// на детали вещается скрипт, который переместит их на поле, где они останутся до конца игры
	private MoveUntachablePlanesScript AddMoveUntachablePlanesScriptToPlane (int planeNumber)
	{		
		GameObject plane = MainGameScript.instance.planesList [planeNumber];			
		MoveUntachablePlanesScript planeScript = plane.AddComponent<MoveUntachablePlanesScript>();
		planeScript.enabled = false;
		allPlaneScripts.Add(planeScript);
		return planeScript;
	}
	
	private void GetPosition (int index, MoveUntachablePlanesScript planeScript, float lastPosY)
	{
		planeScript.sortingsStartPosY = lastPosY;
		string pX;
		string pZ;
		listPlanesValues[index].TryGetValue("px",out pX);
		listPlanesValues[index].TryGetValue("pz",out pZ);
		planeScript.endPosX = float.Parse(pX);
		planeScript.endPosZ = float.Parse(pZ);
	}
	
	private void GetRotation (int index, MoveUntachablePlanesScript planeScript)
	{
		string rX;
		string rY;
		listPlanesValues[index].TryGetValue("rx",out rX);
		listPlanesValues[index].TryGetValue("ry",out rY);
		planeScript.endRotX = float.Parse(rX);
		planeScript.endRotY = float.Parse(rY);
	}

	private void GetPlanesData ()
	{
		float posYValue = START_SORT_Y;
		for (int i = 0; i < listPlanesValues.Count; i++)
		{
			int planeNumber = GetPlaneNumber (i);
			MoveUntachablePlanesScript planeScript = AddMoveUntachablePlanesScriptToPlane (planeNumber);
			posYValue -= DELTA_SORT;
			GetPosition (i, planeScript, posYValue);
			GetRotation (i, planeScript);
		}
	}

	private void RemoveUntouchablePlanesFromMainMassiv ()
	{		
		for (int i = listPlanesValues.Count - 1; i >= 0; i--) 
		{
			int planeNumber = GetPlaneNumber (i);
			MainGameScript.instance.planesList.RemoveAt(planeNumber);
			MainGameScript.instance.planesDataList.RemoveAt(planeNumber);
			MainGameScript.instance.commonCountOfPlanes--;
		}
	}

	// на детали, которые можно будет премещать, вешается скрипт проверки пересечений
	private void AddScriptsToOtherPlanes()
	{
		for (int i = 0; i < MainGameScript.instance.planesList.Count; i++) 
		{
			MainGameScript.instance.planesList[i].AddComponent<CheckScript> ();
			MainGameScript.instance.planesList[i].GetComponent<CheckScript>().planeData =
				MainGameScript.instance.planesList[i].GetComponent<PlanesDataScript> ();
			MainGameScript.instance.planesList[i].GetComponent<PlanesDataScript> ().checkScript = 
				MainGameScript.instance.planesList[i].GetComponent<CheckScript> ();
		}
	}

	// через небольшой интервал после загрузки уровня детали которые нелья будет двигать перемещаются на игровое поле
	IEnumerator MoveUntoucheblesPlanesToBoard() 
	{
		yield return new WaitForSeconds (WAITING_TIME);
		foreach (MoveUntachablePlanesScript planeScripts in allPlaneScripts)
		{
			planeScripts.enabled = true;
		}		
	}
}
