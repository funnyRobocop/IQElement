using UnityEngine;
using System.Collections;

//-----------------------------------------------------------------------------------------------
// Скрипт для доступа к данным, скриптам, кэшированным объектам детали. Вешается на каждую деталь
//-----------------------------------------------------------------------------------------------
public class PlanesDataScript : MonoBehaviour {

	private const float COLOR_ALPHA_CHANNEL = 255; // альфа канал, всегда непрозрачные
	private const float COLOR_BLUE_CHANNEL = 0; // голубой
	private const float DELTA_FOR_TRIGGERS_SIZES = 40; // триггеры растягиваются для обнаружения столкновений

	public Type type; // тип фигуры
	public MovePlaneScript movePlaneScript;
	public RotatePlaneBase rotatePlaneScript;
	public Transform thisTransform;
	public BoxCollider[] triggers; // физический триггер для обнаружения пересечений
	public SpriteRenderer[] selectingColors; // для выделения фигуры красным или зеленым
	public GameObject[] arrows; // массив стрело для поворотов фигуры
	private float[,] triggersSizes; // начальные размеры триггеров, у каждой фигуры свои

	public CheckScript checkScript { get; set;}
	public int sortIndex { get; set;} // нужен для сортировки деталей

	// в игре два типа фигур
	public enum Type
	{
		TRIPPLE = 0, // ...маленькая
		QUADRO = 1, // ...большая
	}	

	void Start()
	{	
		// запоминаем первоначальные размеры триггеров, чтобы потом к ним возвращаться
		triggersSizes = new float[3, 3] { { triggers[0].size.x, triggers[0].size.y, triggers[0].size.z }, 
									 	  { triggers[1].size.x, triggers[1].size.y, triggers[1].size.z }, 
										  { triggers[2].size.x, triggers[2].size.y, triggers[2].size.z } };
	}

	// в зависимости от положения детали включается разный набор стрел
	public void TurnOnArrow()
	{
		if (!rotatePlaneScript.up)
		{
			arrows[0].SetActive(true);
			arrows[1].SetActive(true);
			arrows[2].SetActive(true);
		}
		else
		{
			arrows[3].SetActive(true);
			arrows[4].SetActive(true);
			arrows[5].SetActive(true);
		}
	}
	
	public void TurnOffArrow()
	{		
		arrows[0].SetActive(false);
		arrows[1].SetActive(false);
		arrows[2].SetActive(false);
		arrows[3].SetActive(false);
		arrows[4].SetActive(false);
		arrows[5].SetActive(false);
	}
	
	public void SetRedGreenColors(int red, int green)
	{
		selectingColors[0].color = new Vector4 (red, green, COLOR_BLUE_CHANNEL, COLOR_ALPHA_CHANNEL);
		selectingColors[1].color = new Vector4 (red, green, COLOR_BLUE_CHANNEL, COLOR_ALPHA_CHANNEL);
	}
	
	public void TurnOnColors()
	{
		selectingColors[0].enabled = true;
		selectingColors[1].enabled = true;
	}

	public void TurnOffColors()
	{
		selectingColors[0].enabled = false;
		selectingColors[1].enabled = false;
	}
	
	public void FastTurnOffMovePlaneScript()
	{
		SmoothTurnOffMovePlaneScript();
		movePlaneScript.enabled = false;
	}
	
	public void SmoothTurnOffMovePlaneScript()
	{
		movePlaneScript.isNeedNext = false;
	}
	
	public void TurnOnMovePlaneScript (float diffX, float diffZ)
	{
		movePlaneScript.isNeedNext = true;
		movePlaneScript.enabled = true;
		movePlaneScript.SetDiffs (diffX, diffZ);
	}
	
	public void SmoothTurnOffRotatePlaneScript()
	{
		rotatePlaneScript.isNeedNext = false;
	}
	
	public void TurnOnRotatePlaneScript()
	{
		rotatePlaneScript.isNeedNext = true;
		rotatePlaneScript.enabled = true;
		rotatePlaneScript.DropDelta();
	}

	// предотвращает отключение скрипта вращения, если его запустить в том же фрэйме
	public void WardOffTurnOffRotatePlaneScript()
	{
		rotatePlaneScript.isNeedNext = true;
	}

	// так как детали на самом деле не пересекаются друг с другом (иначе 3D модели будут накладыватся друг на друга)
	// для обнаружения столкновений просто растягивается триггер в одну или другую сторону в зависимости от положения детали
	public void SetTriggerSizesToChecking()
	{
		// выполняется только если фигура на игровом поле
		if (rotatePlaneScript.rotationOutAndSizeLerp >= 1)
		{
			if (rotatePlaneScript.up) 
			{
				triggers [0].size = new Vector3 (triggersSizes [0, 0], DELTA_FOR_TRIGGERS_SIZES, triggersSizes [0, 2]);
				triggers [1].size = new Vector3 (triggersSizes [1, 0], DELTA_FOR_TRIGGERS_SIZES, triggersSizes [1, 2]);
				triggers [2].size = new Vector3 (triggersSizes [2, 0], DELTA_FOR_TRIGGERS_SIZES, triggersSizes [2, 2]);
			}
			else
			{
				triggers[0].size = new Vector3(triggersSizes[0, 0], triggersSizes[0, 1], DELTA_FOR_TRIGGERS_SIZES);
				triggers[1].size = new Vector3(triggersSizes[1, 0], triggersSizes[1, 1], DELTA_FOR_TRIGGERS_SIZES);
				triggers[2].size = new Vector3(triggersSizes[2, 0], triggersSizes[2, 1], DELTA_FOR_TRIGGERS_SIZES);
			}
		}
	}
	
	public void SetTriggerSizesToNormal()
	{
		triggers[0].size = new Vector3(triggersSizes[0, 0], triggersSizes[0, 1], triggersSizes[0, 2]);
		triggers[1].size = new Vector3(triggersSizes[1, 0], triggersSizes[1, 1], triggersSizes[1, 2]);
		triggers[2].size = new Vector3(triggersSizes[2, 0], triggersSizes[2, 1], triggersSizes[2, 2]);
	}
}
