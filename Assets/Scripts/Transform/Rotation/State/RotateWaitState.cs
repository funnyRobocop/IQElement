using UnityEngine;
using System.Collections;


//----------------------------------------------------------
// Состояние ожидания для скрипта вращения. 
// Запускает проверки на пересечения и прохождения уровня
//----------------------------------------------------------
public class RotateWaitState : RotationState {

	public void Go (RotatePlaneBase rotatePlaneScript) {

		// отключение скрипта
		if (!rotatePlaneScript.isNeedNext)
		{	
			// ждем фрэйм
			if (!rotatePlaneScript.waitFrame)
			{
				// увеличиваем триггеры деталек, чтобы проверить их пересечения друг с другом
				// каждый раз проверяются все детали
				foreach (PlanesDataScript item in MainGameScript.instance.planesDataList)
				{
					item.SetTriggerSizesToChecking();
				}
				rotatePlaneScript.waitFrame = true;
			}
			else
			{
				// на следующий фрэйм когда триггеры отработают проверяем количество пересечений
				foreach (PlanesDataScript item in MainGameScript.instance.planesDataList)
				{
					item.checkScript.CheckIsStand();
				}
				// ...и возвращаем триггеры в нормальное состояние
				foreach (PlanesDataScript item in MainGameScript.instance.planesDataList)
				{
					item.SetTriggerSizesToNormal();
				}

				// после отключения каждого скрипта вращения идет проверка на завершенность уровня
				MainGameScript.instance.CheckIsAllStand ();

				rotatePlaneScript.planeData.TurnOffArrow(); 
				rotatePlaneScript.planeData.TurnOnColors();              
				rotatePlaneScript.planeData.rotatePlaneScript.enabled = false;
				rotatePlaneScript.waitFrame = false;
			}
		}
	}
}
