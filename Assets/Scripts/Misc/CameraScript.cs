using UnityEngine;
using System.Collections;

//------------------------------
// Скрипт для управления камерой
//------------------------------
public class CameraScript : MonoBehaviour {

	private const float POS_X = 10;
	private const float POS_Y = 1000; // такая большая величина нужна для корректной работы шэйдера
	private const float POS_Z = 4;

	public static CameraScript instance; // скрипт камеры только один
	public Camera camObject { get; private set; } // кэширование камеры
	public Transform camTransform { get; private set; } // кэширование трансформа камеры	

	
	void Awake()
	{
		instance = this;

		camObject = Camera.main;
		camTransform = camObject.transform;

		// при каждом перезапуске игры
		//RandomizeCameraRotation ();
	}
	
	public void RandomizeCameraRotation()
	{
		camTransform.rotation = Quaternion.Euler (90, -1 + Random.Range(0f, 2f), 0);
	}
	
	public void SetCameraPositionToNormal()
	{
		camTransform.position = new Vector3 (POS_X, POS_Y, POS_Z); 
	}

	public void ChangeBackgroundColor(int page)
	{
		camObject.backgroundColor = GameGUIScript.instance.colorsList [page];
	}
}
