using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Test : MonoBehaviour
{

    static int number = 1;

    private void Awake()
    {
        /*if (GameObject.FindObjectsOfType<Test>().Length >1)
        {
            Destroy(this.gameObject);
        };
        DontDestroyOnLoad(this.gameObject);*/
    }

    /*IEnumerator Start()
    {
        number = 1;
        yield return new WaitForSeconds(2);

        for (var i = 1; i < 121; i ++)
        {
            MainGameScript.currentLevel = number;
            SceneManager.LoadScene("Game_scene");
            number++;
            yield return new WaitForSeconds(2);
            ScreenCapture.CaptureScreenshot("Foto/Level_" + number + ".png");
            yield return new WaitForSeconds(1);
        }
    }*/

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            MainGameScript.currentLevel = number;
            SceneManager.LoadScene("Game_scene");
            number++;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            number = 1;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            ScreenCapture.CaptureScreenshot("Level_" + number + ".png");
        }
    }*/
}
