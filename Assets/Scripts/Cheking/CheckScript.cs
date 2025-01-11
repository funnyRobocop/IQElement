using UnityEngine;
using System.Collections;


//-----------------------------------------------------------------------------
// Скрипт вешается на каждую деталь, проверяет пересекается ли деталь с другими
//-----------------------------------------------------------------------------
public class CheckScript : MonoBehaviour {

	public PlanesDataScript planeData; // ссылка на данные летали
	public int countIntersect { get; private set;} // у каждой детали не менее 3 физических тел, количество пересечений с ними
	public bool isStand { get; set;} // если пересечений нет и деталь на игровом поле не выделена, то true
	    

    // обнаруживаем столновение с триггером
	void OnTriggerEnter (Collider other)
	{
		if ((other.gameObject.tag == "Plane") || (other.gameObject.tag == "NoTouch")) 
		{
			countIntersect++;
		}

		if (other.gameObject.tag == "Selector")
		{	
			SelectorScript.instance.selectedPlanesDataList.Add(planeData);
		}
	}

	// обнаруживаем выход из триггера
	void OnTriggerExit (Collider other)
	{
		if ((other.gameObject.tag == "Plane") || (other.gameObject.tag == "NoTouch")) 
		{
			// так как в изначальном состоянии могут уже быть пересечения
			if (countIntersect > 0) 
			{ 
				countIntersect--; 
			}
		}

		if (other.gameObject.tag == "Selector")
		{	
			SelectorScript.instance.selectedPlanesDataList.Remove(planeData);
		}
	}

	// разукрашиваем выделение в зелёный или красный
	public void CheckIsStand ()
	{
		isStand = false;
		if (planeData.rotatePlaneScript.rotationOutAndSizeLerp >= 1)
		{
			if (countIntersect == 0) 
			{
				isStand = true;
				planeData.SetRedGreenColors (0, 255);
				Audio.instance.PlaySelect();
			} 
			else 
			{
				isStand = false;
				planeData.SetRedGreenColors (255, 0);
				Audio.instance.PlayWrong();
			}
		}
	}
}
