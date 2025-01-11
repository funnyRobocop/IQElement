using UnityEngine;
using System.Collections;


//---------------------------------------------------------------------------------------------------
// Скрипт для проверки правильности решения уровня (не должно оставаться пустых ячеек)
// Скрипт запускоется когда все детали находятся на игровом поле и между ними отсутствуют пересечения
//---------------------------------------------------------------------------------------------------
public class FinalCheckerScript : MonoBehaviour {

	private const float START_SORT_Y = 5.5f; // высота от которой происходит расположение сортированных деталей
	private const float DELTA_SORT = 0.15f; // интервал между деталями при их сортировке
	private const float NEEDED_COUNT_OF_INTERSECTIONS_WHITH_FINALCHEKER = 52; // 50 ячеек + 2 , так как у двух делалей есть 
																			// ...пересекающиеся между собой физические тела 

	private int countOfIntersection; // подсчитывает пересечения с ячейками
	public GameObject finalChecker; // кэширование объекта FinalCheker
	public bool waitFrame { get; private set; } // включается на один фрэйм
	
	
	// обнаруживаем столновение триггера объекта FinalCheker с триггерами деталей
	void OnTriggerEnter (Collider other)
	{
		if ((other.gameObject.tag == "Plane") || (other.gameObject.tag == "NoTouch")) 
		{
			countOfIntersection++;
		}
	}
	
	// обнаруживаем выход триггера объекта FinalCheker из триггеров деталей
	void OnTriggerExit (Collider other)
	{
		if ((other.gameObject.tag == "Plane") || (other.gameObject.tag == "NoTouch")) 
		{
			countOfIntersection--;
		}
	}

	// вызывается каждый фрэйм после OnTriggerEnter и OnTriggerExit
	void FixedUpdate()
	{
		// отсортировываем детали с таким интервалом, чтобы они пересеклись с объектом FinalChecker
		// затем ждем фрэйм
		if (!waitFrame)
		{
			MainGameScript.instance.sortPlanesPosition (SelectorScript.instance.selectedPlaneData, START_SORT_Y, DELTA_SORT);
			waitFrame = true;
		}
		// ...и на следующий фрэйм после отработки триггеров 
		else
		{
			finalChecker.SetActive (false);

			// ...проверяется что не осталось ячеек которые не пересекаются с деталями
			if (countOfIntersection < NEEDED_COUNT_OF_INTERSECTIONS_WHITH_FINALCHEKER)
			{
				MenuGUIScript.instance.emptyHolesPanel.SetActive(true);
				Audio.instance.PlayWrong();
			}
			else
			{
				MainGameScript.instance.GoToNextLevel();
			}

			waitFrame = false;
			countOfIntersection = 0;
		}
	}
}
