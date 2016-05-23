using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Tabo : MonoBehaviour {

	public List<Player> players;
	[SerializeField]float radius;

	public int shotInterval = 500; // ショット間隔 (ms)

	private Vector2 prePos;

	[SerializeField]
	private GameObject shotPrefab;
	private int startTime;
	private int nextShotTime; // 次のショット時間

	private int taboTouchId;
	private int frontId;
	private int leftId;
	private int rightId;

	[SerializeField]
	private Vector2 vec;
	// 3way 10°ずつ0.5s
	// 方向　弾を打つ bullet 

	private bool shotting = false;

	[SerializeField]
	private Vector2 lastTaboPos;
	private bool checkTabo = false;
	private Vector2 lastFrontPos;
	private Vector2 lastLeftPos;
	private Vector2 lastRightPos;

	// [SerializeField]
	// private AudioSource shotSE;


	public void SetTouch(int fid, int lid, int rid)
	{
		this.frontId = fid;
		this.leftId = lid;
		this.rightId = rid;
		try{
			Vector2 fpos = GetTouch(fid);
			Vector2 lpos = GetTouch(lid);
			Vector2 rpos = GetTouch(rid);
			
			Vector2 a = fpos - lpos;
			Vector2 b = fpos - rpos;
			this.vec = (a + b).normalized;
		}catch(Exception e)
		{

		}
		//debug用
		this.vec = new Vector2(4, 3).normalized;
		this.prePos = this.GetPos2() + new Vector2 (0, -1);


	}

	public void OnStartCheckTABO()
	{
		this.checkTabo = true;
	}

	public void GameStart()
	{
		this.shotting = true;
		this.startTime = GetNow();
		this.nextShotTime = this.startTime + this.shotInterval;
	}

	public void GameEnd()
	{
		this.shotting = false;
	}

	private void Awake()
	{
		this.frontId = this.leftId = this.rightId = -1;
	}

	// Use this for initialization
	void Start ()
	{
		//this.GameStart ();
		this.SetTouch (-1, -1, -1);
	}
	
	// Update is called once per frame
	private void Update ()
	{

		Vector2 nowPos = GetPos2();
			
		if(nowPos - this.prePos != new Vector2(0,0))
			this.vec = nowPos - this.prePos;

		this.vec.Normalize ();

		if (this.shotting) {
			if (GetNow() > this.nextShotTime) { 
				// shot
				for (int i = -1; i < 2; i++) {
					GameObject go = GameObject.Instantiate (this.shotPrefab);
					go.GetComponent<Shot> ().Init (this.players, this.Rotate (this.vec, i * 20));
					go.transform.position = this.transform.position;

				}
				// this.shotSE.Play ();
				this.nextShotTime += this.shotInterval;
			}
		}

		this.prePos = this.GetPos2();
	}

	private void CheckingTABO()
	{
		
	}

	private Vector2 Rotate(Vector2 vec, int kakudo)
	{
		Vector2 v = new Vector2();
		double d = kakudo * Math.PI / 180;
		v.x = (float)(vec.x * Math.Cos (d) - vec.y * Math.Sin (d));
		v.y = (float)(vec.x * Math.Sin (d) + vec.y * Math.Cos (d));

		Debug.Log ("vec x:" + vec.x + " y:" + vec.y);
		return v;
	}

	public float getRadius()
	{
		return radius;
	}
					
	/// <summary>
	/// get utcnow (ms)
	/// </summary>
	/// <returns>The now.</returns>
	public static int GetNow()
	{
		int i = 0;
		i = (int)(DateTime.UtcNow.Ticks / 10000); // msに変換
		return i;
	}

	public static Vector2 GetTouch(int id)
	{
		try{
			Vector2 pos = Input.GetTouch (id).position;
			pos = Camera.main.ScreenToWorldPoint (pos);
			return pos;
		}catch(Exception e) {
			return Vector2.zero;
		}
	}

	public Vector2 GetPos2()
	{
		return new Vector2 (this.transform.position.x, this.transform.position.y);
	}

}
