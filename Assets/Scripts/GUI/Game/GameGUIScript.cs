using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


//------------------------------------------------------------------
// Скрипт для управления игровым GUI. Меняет цвет фона, надписи и тд
//------------------------------------------------------------------
public class GameGUIScript : MonoBehaviour {

	private const float LEVELS_FOR_ONE_SKILL = 24; // 24 уровня на каждый навык

	public static GameGUIScript instance;
	public TextMeshProUGUI number; // текст с номером уровня
	public TextMeshProUGUI level; // надпись скилла
	public Image back; // стрелка
	public TextMeshProUGUI skip; // надпись пропуска уровня
	public Color[] colorsList; // цвета меняющееся в зависимости от навыка
	public string[] skillsList; // навыки
	public GameObject adBtn;

	void Awake()
	{
		instance = this;
	}

	// при смене уровня меняются все свойства
	public void ChangeGameGUIPropertiesByNumberOfLevel ()
	{
		number.text = MainGameScript.currentLevel.ToString();
		ChangeGameGUIPropertiesBySkillsLevel (CalculateLevelSkill ());
	}
	
	// при смене уровня навыков не меняется номер уровня
	public void ChangeGameGUIPropertiesBySkillsLevel (int levelSkill)
	{
		CameraScript.instance.ChangeBackgroundColor (levelSkill);
		level.color = colorsList [levelSkill];
		number.color = colorsList [levelSkill];
		skip.color = colorsList[levelSkill];
		back.color = colorsList[levelSkill];

		if (Application.systemLanguage == SystemLanguage.Russian)
			levelSkill += 5;

		level.text = skillsList [levelSkill];		
	}

	// вычисляет на каком из пяти навыков (STARTER, JUNIOR, EXPERT, MASTER, WIZARD) игрок в данный момент
	private int CalculateLevelSkill ()
	{
		return (int) ((MainGameScript.currentLevel - 1) / LEVELS_FOR_ONE_SKILL);
	}

	public void ShowAdBtn()
	{
		if (MainGameScript.currentLevel != MainGameScript.openedLevel)
            return;
		adBtn.SetActive(true);
	}
	public void HideAdBtn()
	{
		adBtn.SetActive(false);
	}
}
