using UnityEngine;
using System.Collections;


//----------------------------------------------------------
// Скрипт для перемещения деталей. Вешается на каждую деталь
//----------------------------------------------------------
public class MovePlaneScript : MonoBehaviour {


	private const int CELL = 2; //ширина/длина ячеек
	private const float BUTTOM_BORDER_Z = -0.5f; //нижняя граница, при пересечении детали убираются с платы
	private const float FINAL_POS_Z = -2; //позиция по оси Z, на которую перемещаем

	public PlanesDataScript planeData;//ссылка данные детали
	private int[,] borderTripple = new int[8, 4] { { 1, 17, 3, 7 }, { 3, 17, 3, 9 }, { 3, 19, 3, 7 }, { 3, 17, 1, 7 },
												   { 3, 19, 3, 7 }, { 3, 17, 1, 7 }, { 1, 17, 3, 7 }, { 3, 17, 3, 9 } };
	private int[,] borderQuadro = new int[2, 4] { {2, 18, 4, 6}, {4, 16, 2, 8} };
	public int[,] borders; //границы у разных типов деталей разные
	private Vector3 cellPos; //позиция в которую передвигается деталь
	private float diffMoveX; //смещение относителтно центра детали при её захвате
	private float diffMoveZ; //смещение относителтно центра детали при её захвате
	private int diffIn; //смещение от границы ячейки
	private int diffOut; //смещение от границы ячейки
	public int finalPosX; // позиция по оси X, у каждой детали своя, на неё перемещаем	
	private float lerpValue = 1; //значение интерполяции, изначально должно быть 1
	private float realWorldX; //касание игрока в реальных координатах
	private float realWorldZ; //касание игрока в реальных координатах

	public int indicator { get; set; } //указатель на границы в массиве borders
	public bool isNeedNext { get; set; } //переменная для плавного отключения скрипта
	public float typeDiffPos { get; set; } //откличается у маленьких и больших деталей

	//вызывается при запуске приложения
	void Awake()
	{
		//инициализируем границы в зависмости от типа фигуры
		if (planeData.type == PlanesDataScript.Type.TRIPPLE) 
		{
			borders = borderTripple;
			diffIn = 0;
			diffOut = 1;
			typeDiffPos = 1;
		}
		else
		{
			borders = borderQuadro;
			diffIn = 1;
			diffOut = 0;
			typeDiffPos = 0;
		}
	}

	// вызывается каждый фрэйм
	void Update () {

		//при достижении конечных значений отключение скрипта или берутся следующие значения для перемещения
		if (lerpValue >= 1) 
		{
			if (!isNeedNext)
			{			
				planeData.movePlaneScript.enabled = false;				
				if (planeData.thisTransform.position.z > - 2)
				{
					planeData.TurnOnArrow();
				}
				return;
			}

			GetRealWorldPos ();
			cellPos = GetCellPos ();
		}

		//увеличиваем значение интерполяции
		lerpValue += Time.deltaTime;
		lerpValue *= MainGameScript.speed;

		//передвижение детали по осям X и Z
		planeData.thisTransform.position = new Vector3 (Mathf.SmoothStep (planeData.thisTransform.position.x, cellPos.x, lerpValue),
		                                                 planeData.thisTransform.position.y,
		                                                 Mathf.SmoothStep (planeData.thisTransform.position.z, cellPos.z, lerpValue));
	}

	private void GetRealWorldPos ()
	{
		//получаем позицию касания в реальных координатах
		realWorldX = SelectorScript.instance.thisTransform.position.x; 
		realWorldZ = SelectorScript.instance.thisTransform.position.z;
	}

	//функция получения координат касаний, округленных к размерам ячеек
	private Vector3 GetCellPos ()
	{
		//если передвинуть деталь вниз она убирается с платы
		if (SelectorScript.instance.canMoveOut)
		{
			if (realWorldZ < BUTTOM_BORDER_Z)
			{
				if (cellPos.z != FINAL_POS_Z)
				{
					Audio.instance.PlaySelect();
					lerpValue = 0;
				}
				return new Vector3 (finalPosX,  planeData.thisTransform.position.y, FINAL_POS_Z);
			}
		}
		else
		{	
			if (realWorldZ > BUTTOM_BORDER_Z)
			SelectorScript.instance.canMoveOut = true;
		}

		//округляем координаты до размеров ячеек
		float newX = CELL * ((int)((realWorldX - diffMoveX + diffIn) / CELL)) + diffOut;
		float newZ = CELL * ((int)((realWorldZ - diffMoveZ + diffIn) / CELL)) + diffOut;

		//проверка выхода за пределы платы
		if (newX < borders[indicator, 0]) newX = borders[indicator, 0];
		else if (newX > borders[indicator, 1]) newX = borders[indicator, 1];
		if (newZ < borders[indicator, 2]) newZ = borders[indicator, 2];
		else if (newZ > borders[indicator, 3]) newZ = borders[indicator, 3];

		//если позиция не изменилась ничего не нужно делать
		if ((newX != cellPos.x) || (newZ != cellPos.z))
		{
			Audio.instance.PlaySelect();
			lerpValue = 0;
		}

		return new Vector3 (newX,  planeData.thisTransform.position.y, newZ);
	}

	// разница между позицией касания игроком и позицией детали
	// нужен чтобы деталь можно было перемещать за любую её часть
	public void SetDiffs(float selectorX, float selectorZ)
	{
		diffMoveX = selectorX - planeData.thisTransform.position.x;
		diffMoveZ = selectorZ - planeData.thisTransform.position.z;        
    }
}