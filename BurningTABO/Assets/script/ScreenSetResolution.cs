using UnityEngine;
using System.Collections;

public class ScreenSetResolution : MonoBehaviour {

	void Awake ()
    {
//        Screen.SetResolution(1680, 1260, true);
		Screen.SetResolution(1440, 1080, false);
    }
}
