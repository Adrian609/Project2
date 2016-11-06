using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour {
	struct CubeState {
		public int x;
		public int y;
		public float zRotate;
		public float spearY;
	}
	//float attacked= 0;//this is to control the timing for the spear going out and then back to resting as well as a cooldown
	bool attacking = false;
	float movementSpeed = 8f;
	float rotationSpeed = 4f;
	public int score = 0;//kills
	public GameObject spear;
	CubeState state;
	// Use this for initialization
	void Start () {
		InitState ();
	}
	void InitState() {
		state = new CubeState {
			x = 0,
			y = 0,
			zRotate = 0,
			spearY = 0
		};
	}
	// Update is called once per frame
	void Update () {
		KeyCode[] arrowKeys = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow };
		foreach (KeyCode arrowKey in arrowKeys) {
			if (!Input.GetKeyDown (arrowKey))
				continue;
			state = Move(state, arrowKey);
		}
		KeyCode[] rotKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
		foreach (KeyCode rotkey in rotKeys) {
			if (!Input.GetKeyDown (rotkey))
				continue;
			state = Rotate (state, rotkey);
		}
		if (attacking == false && Input.GetMouseButtonDown (0)) {//this is why I drink, runs half a million times per mouse click. Changed spearY++ to spearY = .8, took way to long to think of that...
			attacking = true;
		}
		SyncState();
	}
	void SyncState () {
		if (!attacking) {
			transform.position = Vector2.Lerp (transform.position, new Vector2 (state.x, state.y), Time.deltaTime * movementSpeed);
			Quaternion target = Quaternion.Euler (0, 0, state.zRotate);
			transform.rotation = Quaternion.Slerp (transform.rotation, target, Time.deltaTime * rotationSpeed);
		} else {
			state.spearY = 0.8f;
			spear.transform.position = Vector3.Lerp(spear.transform.position, new Vector3(spear.transform.position.x, state.spearY, spear.transform.position.z), Time.deltaTime * movementSpeed);
			StartCoroutine (Wait (1f));
		}
	}
	IEnumerator Wait (float seconds){
		yield return new WaitForSeconds(seconds);
		state.spearY = 0f;
		spear.transform.position = Vector3.Lerp(spear.transform.position, new Vector3(spear.transform.position.x, state.spearY, spear.transform.position.z), Time.deltaTime * movementSpeed);
		attacking = false;
	}
	CubeState Move(CubeState previous, KeyCode arrowKey) {
		int dx = 0;
		int dy = 0;
		switch (arrowKey) {
		case KeyCode.UpArrow:
			dy = 1;
			break;
		case KeyCode.DownArrow:
			dy = -1;
			break;
		case KeyCode.RightArrow:
			dx = 1;
			break;
		case KeyCode.LeftArrow:
			dx = -1;
			break;
		}
		return new CubeState {
			x = dx + previous.x,
			y = dy + previous.y,
			zRotate = previous.zRotate
		};
	}
	CubeState Rotate(CubeState previous, KeyCode rotKey){
		float dRot = 0f;
		switch (rotKey) {
		case KeyCode.W:
			dRot = 0f;
			break;
		case KeyCode.D:
			dRot = 270f;
			break;
		case KeyCode.S:
			dRot = 180f;
			break;
		case KeyCode.A:
			dRot = 90f;
			break;
		}
		return new CubeState {
			x = previous.x,
			y = previous.y,
			zRotate = dRot
		};
	}
	void OnTriggerEnter(Collider other){//may have to put on a script on player object, not player prefab. ya know, cuz this script controlls player and spear. Thats a pain...
		if (other.gameObject.CompareTag ("SpearPoint")) {
			//die
		}
	}
}