using UnityEngine;
using System.Collections;

//--------------------------------------
// Скрипт для вращения маленьких деталей
//--------------------------------------
public class RotateTripplePlaneScript : RotatePlaneBase {


	public override void MoveWhileRotateX() 
	{
		MoveWhileRotate ();
	}

	protected override void RoundCoordinates()
	{
		newX = CELL * ((int)(planeData.thisTransform.position.x / CELL)) + 1;
		newZ = CELL * ((int)(planeData.thisTransform.position.z / CELL)) + 1;
	}
	
	protected override void RotateYLeft()
	{
		PrepareRotateYLeft();
		planeData.movePlaneScript.indicator --;
		if (up) { if (planeData.movePlaneScript.indicator < 0) { planeData.movePlaneScript.indicator = 3; } }
		else { if (planeData.movePlaneScript.indicator < 4) { planeData.movePlaneScript.indicator = 7; } }
		cellPos = getCellPos ();
	}
	
	protected override void RotateYRight()
	{		
		PrepareRotateYRight();
		planeData.movePlaneScript.indicator ++; 
		if (up) { if (planeData.movePlaneScript.indicator > 3) { planeData.movePlaneScript.indicator = 0; } }
		else { if (planeData.movePlaneScript.indicator > 7) { planeData.movePlaneScript.indicator = 4; } }
		cellPos = getCellPos ();
	}
	
	protected override void RotateXUp()
	{
		PrepareRotateXUp();
		planeData.movePlaneScript.indicator += 4; 
		if (!up) { if (planeData.movePlaneScript.indicator > 7) { planeData.movePlaneScript.indicator = planeData.movePlaneScript.indicator - 4; } }
		cellPos = getCellPos ();
	}
	
	protected override void RotateXDown()
	{
		PrepareRotateXDown();
		planeData.movePlaneScript.indicator -=4; 
		if (up) { if (planeData.movePlaneScript.indicator < 0) { planeData.movePlaneScript.indicator = 4 - planeData.movePlaneScript.indicator; } }
		cellPos = getCellPos ();
	}
}