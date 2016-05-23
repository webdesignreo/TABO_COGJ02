using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Shot : MonoBehaviour {
	[SerializeField]
	private float radius; // 円判定の半径

	public Vector2 vec; // 飛ぶ方向
	public float speed;

	public List<Player> players = new List<Player>();

	public AudioSource hitSE;

	private int startTime;

	[SerializeField]
	private GameObject hitEffectPrefab;
	private ParticleSystem effect;

	private bool isAlive = true;

	public void Init(List<Player> p, Vector2 vec)
	{
		this.players = p;
		this.vec = vec;
		this.speed = 0.5f;
		this.startTime = Tabo.GetNow();
		this.radius = 0.3f;
		this.isAlive = true;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (isAlive) {
	 
			// this.transform.position += new Vector3((vec * speed).x, (vec * speed).y, 0);
			this.transform.position = this.transform.position + new Vector3 (vec.x * speed, vec.y * speed, 0);

			// 当たり判定

			foreach (Player p in this.players) {
				if ((this.transform.position - p.transform.position).magnitude > this.radius + p.getRadius ()) {
					// not hit
				} else {
					// hit
					p.SendMessage ("Hit", this.transform);
					Debug.Log ("hit!!!");
					var particle = GameObject.Instantiate (this.hitEffectPrefab);
					this.effect = particle.GetComponentInChildren<ParticleSystem>(); 
					particle.transform.position = this.transform.position;
					particle.transform.localScale = Vector3.one;
					// this.effect.Play();

					this.hitSE.Play ();


					// GameObject.Destroy (this.gameObject);
					this.isAlive = false;
					this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
					this.startTime = Tabo.GetNow ();
				}
			}


			// 5000ms で消える
			if (Tabo.GetNow () > startTime + 5000) {
				GameObject.Destroy (this.gameObject);
			}

		} else {
			// shot isNotAlive
			if (!this.effect.isPlaying) {
				GameObject.Destroy (effect.transform.parent.gameObject);
				GameObject.Destroy (this.gameObject);
			} else if (Tabo.GetNow() > this.startTime + 1500){
				GameObject.Destroy (effect.transform.parent.gameObject);
				GameObject.Destroy (this.gameObject);
			}
		}

	}
}
