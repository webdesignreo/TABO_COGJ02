using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource shot;
	public AudioSource shotHit;
	public AudioSource loveup;
	public AudioSource last10;
	public AudioSource ready;



	public void OnLastTen()
	{
		this.last10.Play ();
	}

	public void OnReady()
	{
		this.ready.Play ();
	}

	public void OnLoveUp()
	{
		this.loveup.Play ();
	}

	public void OnShot()
	{
		this.shot.Play ();
	}

	public void OnHit()
	{
		this.shotHit.Play ();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
