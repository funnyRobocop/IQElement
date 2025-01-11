using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//-------------------------------
// Скрипт для обработчика касаний
//-------------------------------
public class SelectorScript : MonoBehaviour {

	private const float POS_OUT_Y = 1000; // позиция за пределами экрана для обработчика касаний
	private const float DISTANCE_Z = 975; // расстояние по оси Z от камеры до обработчика касаний

	private bool canRotate ; // можно вращать только одну фигуру за раз
	private string directionOfArrow; // направление для вращения выбранной фигуры
	private int selectedIndex; // индекс выбранной фигуры в массиве фигур класса MainGameScript
	public PlanesDataScript selectedPlaneData; // доступ к скриптам выбранной фигуры
	public List<PlanesDataScript> selectedPlanesDataList; // хранит все фигуры на которые попадает обработчик касаний

	public static SelectorScript instance { get; private set; } // обработчик касаний только один
	public bool moveSomePlane { get; private set; } //для контролирования возмжности выбирать/вращать другие детали
	public bool rotateSomePlane { get; private set; } //для контролирования возмжности выбирать/вращать другие детали
	public Transform thisTransform { get; private set; } // кэширование трансформа обработчика касаний	
	public bool canMoveOut { get; set; } // запрещает фигуре уходить на исходное состояние сразу после её выбора
	public bool canSelect { get; private set; } // можно выбрать только одну фигуру за раз


	// вызывается сразу при запуске
	void Awake()
	{
		instance = this;
		thisTransform = this.transform;
		canSelect = true;
		canRotate = true;
	}

	// проверка пересечения селектора с фигурами, вызывается каждый фрэм перед Update()
	void OnTriggerEnter(Collider otherTrigger) 
	{
		if (otherTrigger.gameObject.tag == "Rotate" && canRotate)
		{				
			rotateSomePlane = true;
			directionOfArrow = otherTrigger.gameObject.name;
		}
		
		if (canSelect)
		{
			if (otherTrigger.gameObject.tag == "Plane")
			{
				moveSomePlane = true;
			}
			
			if (otherTrigger.gameObject.tag == "Plate" )
			{			
				selectedPlaneData.SmoothTurnOffRotatePlaneScript();
			}
		}
	}

	// вызывается каждый фрэйм
	void Update () 
	{
		CheckEscape ();
		CheckInput ();
		SwitchAction ();
	}

	public void CheckEscape()
	{
		// переход на экран меню 
		if (Input.GetKey(KeyCode.Escape))
		{
			MenuGUIScript.instance.ChangeScreenFromGameToMenu();
			// отключение сообщения о невеном решении уровня, так как оно может быть включено
			MenuGUIScript.instance.emptyHolesPanel.SetActive(false);
		}
	}

	public void SwitchAction()
	{
		// вращение детали при этом отключается возможность выбирать/вращать другие детали
		if (rotateSomePlane)
		{
			RotatePlane();
			canRotate = false;
			canSelect = false;
		}
		// находим деталь с минимальным сортировочным индексом и двигаем её
		else if (moveSomePlane)
		{
			int minimumSortIndex = 1;
			for (int i = 0; i < selectedPlanesDataList.Count; i++)
			{
				if (selectedPlanesDataList[i].sortIndex < minimumSortIndex)
				{
					minimumSortIndex = selectedPlanesDataList[i].sortIndex;
					selectedIndex = i;
				}
			}
			
			MovePlane();
			canSelect = false;
			selectedIndex = 0;
		}
		
		// каждый фрэйм сбрасывается возможность выбора/вращения
		moveSomePlane = false;
		rotateSomePlane = false;
	}

	// в зависимости от платформы
	public void CheckInput()
	{	
//#if UNITY_EDITOR
			if (Input.GetMouseButton(0)) 
			{
				MoveSelector(); 
			}
		
			if (Input.GetMouseButtonUp(0))
			{
			StartCoroutine(TouchUp()); 
				selectedPlaneData.rotatePlaneScript.needResetRotation = true;
			}
/*#else
			if (Input.touchCount > 0) 
			{ 	
				MoveSelector();
				if (Input.GetTouch(0).phase == TouchPhase.Ended)
				{
					TouchUp();
					selectedPlaneData.rotatePlaneScript.needResetRotation = true;
				}
		}
#endif*/
	}

	// координаты для движения селектора переводятся в мировые
	public void MoveSelector()
	{
		thisTransform.position = CameraScript.instance.camObject.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, 
		                                                                    		Input.mousePosition.y, 
		                                                                    		DISTANCE_Z));
	}

	// селектор уходит за пределы видимости
	public void StopSelector()
	{
		thisTransform.position = new Vector3 (thisTransform.position.x, 
		                                      POS_OUT_Y, 
		                                      thisTransform.position.z);
	}

	private IEnumerator TouchUp()
	{
		yield return null;
		canSelect = true;
		canRotate = true;
		canMoveOut = false;
		StopSelector ();
		selectedPlaneData.SmoothTurnOffMovePlaneScript();
	}

	private void RotatePlane()
	{
		selectedPlaneData.WardOffTurnOffRotatePlaneScript ();
		selectedPlaneData.TurnOnArrow();			
		selectedPlaneData.rotatePlaneScript.RunRotate(directionOfArrow);
	}
	
	private void MovePlane()
	{
		// сначала отключается скрипт вращения у последней выбранной,
		selectedPlaneData.SmoothTurnOffRotatePlaneScript();

		// ...потом уже выбирается новая
		selectedPlaneData = selectedPlanesDataList[selectedIndex];
		selectedPlaneData.rotatePlaneScript.needResetRotation = false;
		selectedPlaneData.TurnOffArrow();
		selectedPlaneData.TurnOffColors();
		selectedPlaneData.checkScript.isStand = false;
		selectedPlaneData.TurnOnMovePlaneScript(thisTransform.position.x, thisTransform.position.z);
		selectedPlaneData.TurnOnRotatePlaneScript();

		// детали сортируются при каждом выборе, чтобы не было наслоения 3D моделей друг на друга
		// и чтобы при выборе новой детали выбиралась сначала верхняя, если они находятся друг на друге
		MainGameScript.instance.sortPlanesPosition(selectedPlaneData, 4.9f, 0.25f);

		// звук проигрывается только когда деталь выходит на игровое поле
		if (selectedPlaneData.rotatePlaneScript.rotationOutAndSizeLerp > 1)
		{
			Audio.instance.PlaySelect ();
		}
	}
}
