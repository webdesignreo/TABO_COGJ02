using UnityEngine;
using System.Collections;

public class heartGage : MonoBehaviour {

	//0.1秒で1%
	//1.4f上げるのに10秒
	//1秒で0.14f
	float lovePoint;
	const float loveMax = 1.5f;
	bool isLoving;

	public AudioSource se;
	float seTime; 

	public ParticleSystem particle;

	// Use this for initialization
	void Start () {
		isLoving = false;
		lovePoint = 0;
		seTime = 0;
	}

	// Update is called once per frame
	void Update () {
		Debug.Log (isLoving);
	}

	public void addLovePoint(float dt)
	{
		lovePoint += dt * 0.15f;

		lovePoint = Mathf.Min (lovePoint, loveMax);
		this.gameObject.GetComponent<Transform> ().localScale = new Vector3(lovePoint, lovePoint, 1);
		isLoving = true;
		if(!particle.isPlaying)
			particle.Play ();

		seTime += dt;
		if (this.seTime > 0.5f) {
			this.seTime -= 0.5f;
			se.Play ();
		}
	}
	public void minusLovePoint()
	{
		Debug.Log (lovePoint);
		lovePoint -= (isLoving) ? loveMax / 10.0f : loveMax * 0.4f;
		lovePoint = Mathf.Max (lovePoint, 0);
		this.gameObject.GetComponent<Transform> ().localScale = new Vector3(lovePoint, lovePoint, 1);
	}

	public void setPosition(Vector2 a, Vector2 b)
	{
		this.gameObject.GetComponent<Transform> ().position = new Vector2(Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2)), Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2)));
	}

	public bool isMaxLovePoint()
	{
		return (lovePoint >= loveMax);
	}

	public void setLovingFlag(bool loving)
	{
		isLoving = loving;
		if (particle.isPlaying)
			particle.Stop ();
	}
}
