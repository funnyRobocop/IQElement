using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PageItem : MonoBehaviour
{

    public Sprite[] sprites;

    public int number;

    public GameObject item;

    public List<int> skipIntList = new List<int>();


    private void Awake()
    {
        var skipStr = PlayerPrefs.GetString("skip", string.Empty);

        var skipStrList = skipStr.Split('_').ToList();

        foreach (var item in skipStrList)
        {
            int.TryParse(item, out int level);

            if (level > 0 && !skipIntList.Contains(level))
            {
                skipIntList.Add(level);
            }
        }

        for (var i = 0; i < 24; i++)
        {
            var go = Instantiate(item, transform);
        }
    }
}
