using UnityEngine;
using System.Collections;

//----------------------------------------------
// Данный класс хранит текущее сосояние вращения
//----------------------------------------------
public class RotationContext : RotationState {

	public RotationState state { get; set; }

	public void Go (RotatePlaneBase rotatePlaneScript) {
		this.state.Go (rotatePlaneScript);
	}
}
