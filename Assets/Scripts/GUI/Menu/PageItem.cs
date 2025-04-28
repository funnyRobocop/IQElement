using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class PageItem : MonoBehaviour
{

    public Sprite[] sprites;

    public int number;

    public GameObject item;

    public List<int> skipIntList = new List<int>();


    private void Awake()
    {        
#if UNITY_WEBGL
		var skipStr = string.Empty;
		if (YG2.saves.skip != null)
			skipStr = YG2.saves.skip;
#else
        var skipStr = PlayerPrefs.GetString("skip", string.Empty);
#endif

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
