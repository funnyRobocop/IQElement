using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialPanel : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        FindObjectOfType<MenuGUIScript>().ShowTutor(false);
    }
}
