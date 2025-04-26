using UnityEngine;
using System.Collections;

//----------------------------------
// Скрипт для вращения больших деталей
//------------------------------------
public class RotateQuadroPlaneScript : RotatePlaneBase {	


	public override void MoveWhileRotateX() { /*заглушка*/ }

	protected override void RoundCoordinates()
	{
		newX = CELL * ((int)(planeData.thisTransform.position.x / CELL));
		newZ = CELL * ((int)(planeData.thisTransform.position.z / CELL));
	}
	
	protected override void RotateYLeft()
	{
		PrepareRotateYLeft();
		planeData.movePlaneScript.indicator = 1 - planeData.movePlaneScript.indicator;
		cellPos = getCellPos ();
	}
	
	protected override void RotateYRight()
	{
		PrepareRotateYRight();
		planeData.movePlaneScript.indicator = 1 - planeData.movePlaneScript.indicator;
		cellPos = getCellPos ();
	}

	protected override void RotateXUp()
	{
		PrepareRotateXUp();
	}
	
	protected override void RotateXDown()
	{
		PrepareRotateXDown();
	}
}
