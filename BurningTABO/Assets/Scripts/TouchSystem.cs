using UnityEngine;
using System.Collections;

public class TouchSystem : MonoBehaviour {

    Vector2 cursorPos;
    Vector2 playerPos;

    GameObject[] players;
	GameObject restartButton;

	public AudioSource ato10SE;
	public AudioSource leadySE;
	public AudioSource resultSE;

    int holdFrame;
    int waitFrame;
    float time;

	enum Mode{
		SETUP_TABO,
		PLAYING,
		SEC_30,
		SEC_10,
		END
	}
	Mode nowMode = Mode.SETUP_TABO;

    // Use this for initialization
	void Start () {

        players = GameObject.FindGameObjectsWithTag("Player");
		restartButton = GameObject.Find ("restartButton");
		restartButton.SetActive (false);
        time = 60;
    }

    void Update()
    {
		GameObject inGageAB = GameObject.Find ("HeartInnerGageAB");
		GameObject inGageCD = GameObject.Find ("HeartInnerGageCD");
		GameObject outGageAB = GameObject.Find ("HeartWrapperGageAB");
		GameObject outGageCD = GameObject.Find ("HeartWrapperGageCD");

		if (nowMode != Mode.END) {
			if (inGageAB.GetComponent<heartGage> ().isMaxLovePoint ()) {
				GameObject winAB = GameObject.Find ("win_red");
				winAB.GetComponent<SpriteRenderer> ().enabled = true;
				nowMode = Mode.END;
				this.resultSE.Play ();
			}
			if (inGageCD.GetComponent<heartGage> ().isMaxLovePoint ()) {
				GameObject winCD = GameObject.Find ("win_green");
				winCD.GetComponent<SpriteRenderer> ().enabled = true;
				nowMode = Mode.END;
				this.resultSE.Play ();
			}
			if (time <= 0) {
				GameObject winTB = GameObject.Find ("win_tabo");
				winTB.GetComponent<SpriteRenderer> ().enabled = true;
				nowMode = Mode.END;
				this.resultSE.Play ();
			}
		}

		switch(nowMode)
		{
		case Mode.SETUP_TABO:
			if (Input.touchCount >= 5/* || Input.GetMouseButton(0)*/) {
				nowMode = Mode.PLAYING;

				this.leadySE.Play ();

				foreach (GameObject player in players) {
					Debug.Log (player);
					if (player.GetComponent<Player> ().getGroup () == 3) {
						player.GetComponent<Tabo> ().GameStart ();
					}
				}
			}
			break;

		case Mode.PLAYING:
			
			time -= Time.deltaTime;
			GameObject text = GameObject.Find ("text_start");
			text.GetComponent<SpriteRenderer> ().enabled = false;

			GameObject start = GameObject.Find ("start_jp");
			start.transform.position = new Vector2(start.transform.localPosition.x - Time.deltaTime * 25, start.transform.localPosition.y);

			if (time <= 30) {
				nowMode = Mode.SEC_30;
			}
			break;
		case Mode.SEC_30:
			time -= Time.deltaTime;
			GameObject ato30 = GameObject.Find ("at30sec");
			ato30.transform.position = new Vector2(ato30.transform.localPosition.x - Time.deltaTime * 25, ato30.transform.localPosition.y);

			if (time <= 10) {
				this.ato10SE.Play ();
				nowMode = Mode.SEC_10;
			}
			break;
		case Mode.SEC_10:
			time -= Time.deltaTime;
			GameObject ato10 = GameObject.Find ("at10sec");
			ato10.transform.position = new Vector2(ato10.transform.localPosition.x - Time.deltaTime * 25, ato10.transform.localPosition.y);
			if (time <= 0) {
				GameObject winTB = GameObject.Find ("win_tabo");
				winTB.GetComponent<SpriteRenderer> ().enabled = true;
				nowMode = Mode.END;
				this.resultSE.Play ();
			}
			break;
		case Mode.END:
			time -= Time.deltaTime;

			restartButton.SetActive (true);
			
			break;
		}

        if (isTouching())
        {
            foreach (Touch t in Input.touches)
            {	
				touchCheckPlayers (t);
            }
        }



		// ------------------------------------------
		//リリース前に消す！！！！！
		// ------------------------------------------
		/*
		if (Input.GetMouseButton (0)) {
			Vector2 cursorPos = Input.mousePosition;
			Vector2 worldPos = Camera.main.ScreenToWorldPoint(cursorPos);
			mouseCheckPlayers (worldPos);
		}
		*/
		// ------------------------------------------
		//リリース前に消す！！！！！
		// ------------------------------------------

		//Debug.Log(GameObject.Find("main");// ("HeartInnerGage01"));
		bool isNearAB = false;
		bool isNearCD = false;
		for (int i = 0; i < players.Length; i++) {
			for(int j = i+1; j < players.Length; j++) {
				if (isHitCilcle (players [i].GetComponent<Transform> ().position, players [j].GetComponent<Transform> ().position, players [j].GetComponent<Player> ().getRadius () * 1.5f)) {
					if (players [i].GetComponent<Player>().getGroup() == players [j].GetComponent<Player>().getGroup ()) {
						Debug.Log ("Hit");
						Vector2 a = players [i].GetComponent<Transform> ().position;
						Vector2 b = players [j].GetComponent<Transform> ().position;
						if (players [i].GetComponent<Player> ().getGroup () == 1) {
							inGageAB.GetComponent<SpriteRenderer> ().enabled = true;
							outGageAB.GetComponent<SpriteRenderer> ().enabled = true;
							inGageAB.GetComponent<heartGage> ().addLovePoint (Time.deltaTime);
							inGageAB.transform.position =  ((a + b) / 2);
							outGageAB.transform.position =  ((a + b) / 2);
							isNearAB = true;
						} else {
							inGageCD.GetComponent<SpriteRenderer> ().enabled = true;
							outGageCD.GetComponent<SpriteRenderer> ().enabled = true;
							inGageCD.GetComponent<heartGage> ().addLovePoint (Time.deltaTime);
							inGageCD.transform.position =    ((a + b) / 2);
							outGageCD.transform.position =    ((a + b) / 2);
							isNearCD = true;
						}
					}
				}
			}
		}
		if (!isNearAB) {
			inGageAB.GetComponent<SpriteRenderer> ().enabled = false;
			outGageAB.GetComponent<SpriteRenderer> ().enabled = false;
			inGageAB.GetComponent<heartGage> ().setLovingFlag (false);
		}
		if (!isNearCD) {
			inGageCD.GetComponent<SpriteRenderer> ().enabled = false;
			outGageCD.GetComponent<SpriteRenderer> ().enabled = false;
			inGageCD.GetComponent<heartGage> ().setLovingFlag (false);
		}
    }

	void mouseCheckPlayers(Vector2 mousePos)
	{
		foreach (GameObject player in players) {
			if (isHitCilcle (mousePos, player.transform.position, player.GetComponent<Player> ().getRadius())) {
				player.GetComponent<Player> ().Move (mousePos);
				player.GetComponent<Player> ().setTouch (1);
				break;
			}
		}
	}

	void touchCheckPlayers(Touch t)
	{
		switch(t.phase)
		{
		case TouchPhase.Canceled:
		case TouchPhase.Ended:
			foreach (GameObject player in players) {
				if (t.fingerId == player.GetComponent<Player> ().getTouch ()) {
					player.GetComponent<Player> ().setTouch (-1);
				}
			}
			break;
		case TouchPhase.Stationary:
		case TouchPhase.Moved:
			foreach (GameObject player in players) {
				if (t.fingerId == player.GetComponent<Player> ().getTouch ()) {
					player.GetComponent<Player> ().Move (Camera.main.ScreenToWorldPoint(t.position));
					break;
				}
			}
			break;
		case TouchPhase.Began:
			foreach (GameObject player in players) {
				if (isHitTap (Camera.main.ScreenToWorldPoint(t.position), player.transform.position, player.GetComponent<Player> ().getRadius())) {
					player.GetComponent<Player> ().Move (Camera.main.ScreenToWorldPoint(t.position));
					player.GetComponent<Player> ().setTouch (t.fingerId);
					break;
				}
			}
			break;
		}
	}

    bool isTouching()
    {
        return Input.GetMouseButton(0) || Input.touchCount > 0;
    }

    bool isHitTap(Vector2 tapPoint, Vector2 targetPoint, float targetSize)
    {
        if (Mathf.Pow(tapPoint.x - targetPoint.x, 2) +
            Mathf.Pow(tapPoint.y - targetPoint.y, 2)
            < targetSize)
            return true;
        return false;
    }

	bool isHitCilcle(Vector2 a, Vector2 b, float radius)
	{	
		if (Mathf.Pow (a.x - b.x, 2) +
		   Mathf.Pow (a.y - b.y, 2)
		   <= Mathf.Pow (2 * radius, 2))
			return true;
		return false;
	}

}
