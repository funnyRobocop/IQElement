using UnityEngine;
using System.Collections;

//-------------------------------------------------------------------------------------
// Базовый класс для вращения деталей. Если убрать деталь с платы меняется также размер.
//-------------------------------------------------------------------------------------
public abstract class RotatePlaneBase : MonoBehaviour {


	protected const int CELL = 2; //ширина/длина ячеек
	private const float FINAL_SIZE = 1; //длина, ширина и высота, до которой деталь увеличивается    
	private const float START_SIZE = 0.3F; //начальная длина, ширина и высота
	private const float END_ROT_X = 45; //угол по оси X, до него поворачиваем
	private const float END_ROT_Y = 90; //угол по оси Y, до него поворачиваем
	private const float END_ROT_Z = 0; //угол по оси Z, до него поворачиваем	
	
	private int deltaX; //смещение при выходе за границы платы
	private int deltaZ;//смещение при выходе за границы платы
	protected int newX; //округленное до размеров ячейки при выходе за границы при повороте
	protected int newZ; //округленное до размеров ячейки при выходе за границы при повороте
	public RotationContext context = new RotationContext ();
	public RotationState stateRotateX = new RotateXState ();
	public RotationState stateRotateY = new RotateYState ();
	public RotationState stateRotateWait = new RotateWaitState ();

	public bool isNeedNext { get; set; } //переменная для плавного отключения скрипта
	public bool waitFrame { get; set; }
	public float rotationOutAndSizeLerp { get; set; } //значение интерполяции зависит от положения детали по оси Y
	public float goalAngle { get; set; } //конечное значение угла поворота
	public bool up { get; set; } //изначально деталь в вертикальном положении
	public Vector3 currentAngles { get; set; } //текущие значения углов
	public Vector3 cellPos { get; set; } //позиция в которую передвигается деталь
	public PlanesDataScript planeData; //ссылка на остальные скрипты детали
	public float rotationInLerp { get; set; } //значение интерполяции про поворотах внутри платы
	public Vector3 startAngles { get; set; }
	public Vector3 jump { get; set; }
	public bool needResetRotation { get; set; }


    void Start()
    {        
		SetAnglesValues ();
		context.state = stateRotateWait;
		up = true;
    }

	//вызывается каждый фрэйм
	void Update () 
	{
		// в зависимости от состояния детали выполняются разные действия
		context.Go (this);
		Rotate ();
		CheckOutPlates ();
	}

	//запускает вращение в какую либо сторону
	public void RunRotate(string direction)
	{		
		if (direction == "arrowLeft")
		{
			RotateYLeft();
		}
		else if (direction == "arrowRight")
		{
			RotateYRight();
		}
		else if (direction == "arrowUp")
		{
			RotateXDown();
		}
		else if (direction == "arrowDown")
		{
			RotateXUp();
		}
	}

	//деталь двигается при вращении если выходит за пределв игровой доски
	public void MoveWhileRotate()
	{
		// эффект небольшого подпрыгивания при вращении
		jump = new Vector3 (jump.x - 0.5f, 0, jump.z + 0.5f);
		
		planeData.thisTransform.position = new Vector3 (Mathf.SmoothStep (planeData.thisTransform.position.x + jump.x, cellPos.x, rotationInLerp),
		                                                planeData.thisTransform.position.y,
		                                                Mathf.SmoothStep (planeData.thisTransform.position.z + jump.z, cellPos.z, rotationInLerp));
	}
	
	public void AfterRotation()
	{
		Audio.instance.PlayRotate();		
		context.state = stateRotateWait;
		planeData.TurnOnArrow ();
		jump = new Vector3 (0, 0, 0);
  }

	private void Rotate()
	{
		//текущие размер и угол поворота зависят от положения детали от нижней границы
		rotationOutAndSizeLerp = planeData.thisTransform.position.z + 2;
		
		//увеличиваем размер
		planeData.thisTransform.localScale = new Vector3 (Mathf.SmoothStep (START_SIZE, FINAL_SIZE, rotationOutAndSizeLerp),
		                                                   Mathf.SmoothStep (START_SIZE, FINAL_SIZE, rotationOutAndSizeLerp),
		                                                   Mathf.SmoothStep (START_SIZE, FINAL_SIZE, rotationOutAndSizeLerp));
		//поворачиваем
		planeData.thisTransform.rotation = Quaternion.Euler (Mathf.SmoothStep (END_ROT_X, startAngles.x, rotationOutAndSizeLerp), 
		                                                     Mathf.SmoothStep (END_ROT_Y, startAngles.y, rotationOutAndSizeLerp), 
		                                                     Mathf.SmoothStep (END_ROT_Z, startAngles.z, rotationOutAndSizeLerp));
	}

	//функция получения координат касаний, округленных к размерам ячеек
	protected Vector3 getCellPos ()
	{
		//округляем координаты до размеров ячеек
		RoundCoordinates ();
		
		//возвращаем на прежнюю позицию, с которой было смещено последний раз
		newX = newX + deltaX;
		newZ = newZ + deltaZ;

		MovePlaneScript mps = planeData.movePlaneScript;

		//проверка выхода за границы платы
		if (newX < mps.borders[mps.indicator, 0]) newX = RememberXDelta (newX, mps.borders[mps.indicator, 0]);
		else if (newX > mps.borders[mps.indicator, 1]) newX = RememberXDelta (newX, mps.borders[mps.indicator, 1]);
		if (newZ < mps.borders[mps.indicator, 2]) newZ = RememberZDelta (newZ, mps.borders[mps.indicator, 2]);
		else if (newZ > mps.borders[mps.indicator, 3]) newZ = RememberZDelta (newZ, mps.borders[mps.indicator, 3]);

		return new Vector3 (newX, planeData.thisTransform.position.y, newZ);
	}

	//сохраняем смещение из-за выхда за грницы, чтобы вернуть деталь на место при следующем повороте
	private int RememberXDelta(int newValue, int borderValue)
	{
		deltaX = newValue - borderValue;
		newValue = borderValue;
		return newValue;
	}

	//сохраняем смещение из-за выхда за грницы, чтобы вернуть деталь на место при следующем повороте
	private int RememberZDelta(int newValue, int borderValue)
	{
		deltaZ = newValue - borderValue;
		newValue = borderValue;
        return newValue;
    }

	private void PrepareRotateY()
	{
		planeData.TurnOffArrow ();
		rotationInLerp = 0;
		currentAngles = planeData.thisTransform.eulerAngles;
		context.state = stateRotateY;
	}
	
	protected void PrepareRotateYRight()
	{
		PrepareRotateY ();
		goalAngle = currentAngles.y + 90;
	}
	
	protected void PrepareRotateYLeft()
	{
		PrepareRotateY ();
		goalAngle = currentAngles.y - 90;
	}
	
	private void PrepareRotateX()
	{
		planeData.TurnOffArrow ();
		rotationInLerp = 0;
		currentAngles = planeData.thisTransform.eulerAngles;
		context.state = stateRotateX;
		up = !(up);
	}
	
	protected void PrepareRotateXUp()
	{
		PrepareRotateX ();
		goalAngle = 90;
	}
	
	protected void PrepareRotateXDown()
	{
		PrepareRotateX ();
		goalAngle = 0;
	}

	//обнуляет смещения детали 
	public void DropDelta()
	{
		deltaX = 0;
		deltaZ = 0;
	}	
	
	public void ResetRotation()
	{	
		SetAnglesValues ();
		up = true;
		planeData.movePlaneScript.indicator = 0;
	}

	//детали немного наклонены на произвольные углы
	private void SetAnglesValues () {
		startAngles = new Vector3 (0, 89 + Random.Range(0f, 4f), 0);
	}
	
	public void ResetState()
	{		
		context.state = stateRotateWait;
		planeData.checkScript.isStand = false;
		planeData.TurnOffColors ();
	}

	//когда деталь возвращается на исходное место за пределами игровой доски
	public void CheckOutPlates ()
	{
		if (rotationOutAndSizeLerp <= 0) 
		{
			ResetState();

			if (needResetRotation)
			{
				ResetRotation ();
			}
		}
	}

	abstract public void MoveWhileRotateX();
	abstract protected void RoundCoordinates();
	abstract protected void RotateYLeft ();	
	abstract protected void RotateYRight ();	
	abstract protected void RotateXUp ();		
	abstract protected void RotateXDown ();
}