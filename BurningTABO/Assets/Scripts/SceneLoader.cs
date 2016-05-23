using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;// ←new!

public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SceneManager.LoadScene("main", LoadSceneMode.Additive);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
