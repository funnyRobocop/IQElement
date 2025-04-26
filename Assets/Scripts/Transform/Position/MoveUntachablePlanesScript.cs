using UnityEngine;
using System.Collections;


//-------------------------------------------------------------------------------
// Скрипт для расположения деталей, которые в течении уровня нелья будет двигать.
// Запускается один раз вначале игры, после загрузки уровня.
//-------------------------------------------------------------------------------
public class MoveUntachablePlanesScript : MonoBehaviour {

	private const float FINAL_SIZE = 1; //длина, ширина и высота, до которой деталь увеличивается    
	private const float START_SIZE = 0.3F; //начальная длина, ширина и высота
	private const float START_ROT_X = 45; //угол по оси X, до него поворачиваем
	private const float START_ROT_Y = 90; //угол по оси Y, до него поворачиваем
	private const float ROT_Z = 0; //угол по оси Z, до него поворачиваем
	private const float FINAL_POSITION_Y = 3.9f; //окончательное положение по Y 
	
	public float rotationAndSizeLerp = 0; //значение интерполяции
	public float positionLerp = 0; //значение интерполяции
	private PlanesDataScript planeData; //ссылка на остальные данные
	public float sortingsStartPosY; // при движении деталей у каждого своя чтобы 3D модели не накладывались друг на друга
	public float endPosX; //берется из xml фала при загрузке уровня 
	public float endPosZ; //берется из xml фала при загрузке уровня
	public float endRotX; //берется из xml фала при загрузке уровня
	public float endRotY; //берется из xml фала при загрузке уровня


	void Start () 
	{
		planeData = this.gameObject.GetComponent<PlanesDataScript>();
		//у каждой детали будет своя высота
		planeData.thisTransform.position = new Vector3 (planeData.thisTransform.position.x,
		                                                sortingsStartPosY + planeData.movePlaneScript.typeDiffPos,
		                                                planeData.thisTransform.position.z);
	}
	
	void Update () {
		//увеличиваем значение интерполяции
		positionLerp += Time.deltaTime;
		positionLerp *= MainGameScript.speed;
		
		//передвижение детали по осям X и Z
		planeData.thisTransform.position = new Vector3 (Mathf.SmoothStep (planeData.thisTransform.position.x, endPosX, positionLerp),
		                                                planeData.thisTransform.position.y,
		                                                Mathf.SmoothStep (planeData.thisTransform.position.z, endPosZ, positionLerp));

		rotationAndSizeLerp = planeData.thisTransform.position.z + 2;
		
		//увеличиваем размер
		planeData.thisTransform.localScale = new Vector3 (Mathf.SmoothStep (START_SIZE, FINAL_SIZE, rotationAndSizeLerp),
		                                                  Mathf.SmoothStep (START_SIZE, FINAL_SIZE, rotationAndSizeLerp),
		                                                  Mathf.SmoothStep (START_SIZE, FINAL_SIZE, rotationAndSizeLerp));
		//поворачиваем
		planeData.thisTransform.rotation = Quaternion.Euler (Mathf.SmoothStep (START_ROT_X, endRotX, rotationAndSizeLerp), 
		                                                     Mathf.SmoothStep (START_ROT_Y, endRotY, rotationAndSizeLerp), 
		                                                     Mathf.SmoothStep (ROT_Z, ROT_Z, rotationAndSizeLerp));
		//при достижении конечных значений
		if (positionLerp >= 1)
		{
			//ставим деталь в конечную позицию на которой она будет до конца уровня
			planeData.thisTransform.position = new Vector3 (endPosX, FINAL_POSITION_Y + planeData.movePlaneScript.typeDiffPos, endPosZ);

			//деталь не будет реагировать на касания игрока
			this.gameObject.tag = "NoTouch";

			//для экономии ресурсов делаем объект детали и всех его детей статичными
			this.gameObject.isStatic = true;
			foreach (Transform child in planeData.thisTransform)
			{
				child.gameObject.isStatic = true;
			}

			//включаем возможнось игроку выбирать другие детали
			SelectorScript.instance.enabled = true;
			this.enabled = false;
		}

	}
}
