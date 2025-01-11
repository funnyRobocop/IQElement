using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelItem : MonoBehaviour
{

    public int number;
    public TextMeshProUGUI text;
    public GameObject lockObj;
    public Image preview;
    public Material grayscale;
    public Image playPanelBg;
    public GameObject winObj;

    private PageItem page;


    private void Awake()
    {
        page = GetComponentInParent<PageItem>();

        number = (page.number * 24) + transform.GetSiblingIndex()-1;
        text.text = number.ToString();
        text.color = GameGUIScript.instance.colorsList[page.number];

        var spriteNumber = transform.GetSiblingIndex()-2;

        if (page.sprites.Length <= spriteNumber)
            return;

        var sprite = page.sprites[spriteNumber];

        preview.sprite = sprite;
        preview.SetNativeSize();
    }

    private void OnEnable()
    {
        if (MainGameScript.openedLevel < number)
        {
            text.text = "";
            lockObj.SetActive(true);
            preview.material = grayscale;
            GetComponent<Button>().interactable = false;
            winObj.SetActive(false);
        }
        else
        {
            lockObj.SetActive(false);

            if (page.skipIntList.Contains(number))
            {
                preview.material = grayscale;
                playPanelBg.material = grayscale;
                text.color = Color.gray;
                winObj.SetActive(false);
            }
            else
            {
                preview.material = null;
                playPanelBg.material = null;

                winObj.SetActive(MainGameScript.openedLevel != number);
            }

            GetComponent<Button>().interactable = true;
        }
    }

    public void OnClick()
    {
        if (MainGameScript.openedLevel >= number)
        {
            MainGameScript.currentLevel = number;
            SceneManager.LoadScene("Game_scene");
        }
        else
        {
            //Audio.instance.PlayWrong();
        }
    }
}
