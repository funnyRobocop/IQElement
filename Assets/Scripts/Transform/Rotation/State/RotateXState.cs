using UnityEngine;
using System.Collections;

//-------------------------------------
// Состояние вращения деталей по оси X.
//-------------------------------------
public class RotateXState : RotationState {

	public void Go (RotatePlaneBase rotatePlaneScript) 
	{		
		rotatePlaneScript.rotationInLerp += Time.deltaTime;
		rotatePlaneScript.rotationInLerp *= MainGameScript.speed;

		rotatePlaneScript.startAngles = new Vector3 (Mathf.SmoothStep (rotatePlaneScript.currentAngles.x, 
		                                                               rotatePlaneScript.goalAngle, 
		                                                               rotatePlaneScript.rotationInLerp),
		                              				 rotatePlaneScript.startAngles.y,
		                               				 rotatePlaneScript.startAngles.z);

		rotatePlaneScript.MoveWhileRotateX ();
		if (rotatePlaneScript.rotationInLerp >= 1) 
		{ 
			rotatePlaneScript.AfterRotation ();
		}
	}
}
