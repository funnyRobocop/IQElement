using UnityEngine;
using System.Collections;


//-------------------------------------
// Состояние вращения деталей по оси Y.
//-------------------------------------
public class RotateYState : RotationState {

	public void Go (RotatePlaneBase rotatePlaneScript) 
	{		
		rotatePlaneScript.rotationInLerp += Time.deltaTime;
		rotatePlaneScript.rotationInLerp *= MainGameScript.speed;

		rotatePlaneScript.startAngles = new Vector3 (rotatePlaneScript.startAngles.x,
		                              				 Mathf.SmoothStep (rotatePlaneScript.currentAngles.y, 
		                 												rotatePlaneScript.goalAngle, 
		                 												rotatePlaneScript.rotationInLerp),
		                              				 rotatePlaneScript.startAngles.z);

		rotatePlaneScript.MoveWhileRotate ();
		if (rotatePlaneScript.rotationInLerp >= 1) 
		{ 
			rotatePlaneScript.AfterRotation ();
		}
	}
}
