using UnityEngine;
using System.Collections;

//-------------------------------------
// Интерфэйс состояния вращения деталей.
//-------------------------------------
public interface RotationState 
{
	void Go (RotatePlaneBase rotatePlaneScript);
}
