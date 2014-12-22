using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieController : MonoBehaviour {
	
	public AudioClip enemyContactSound;
	public AudioClip catContactSound;
	public float moveSpeed;
	public float turnSpeed;

	public float moveX;
	public float moveY;

	private Vector3 moveDirection;
	//Make a private variable still accessible from the editor.
	[SerializeField]
	private PolygonCollider2D[] colliders;
	private int currentColliderIndex = 0;
	private bool isInvincible = false;
	private float timeSpentInvincible;
	private int lives = 3;

	private List<Transform> congaLine = new List<Transform>();

	// Use this for initialization
	void Start () {
		moveDirection = Vector3.right;
	}
	
	// Update is called once per frame
//	void Update () {
//		// 1
//		Vector3 currentPosition = transform.position;
//		// 2
//		if( Input.GetButton("Fire1") ) {
//			// 3
//			Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );
//			// 4
//			moveDirection = moveToward - currentPosition;
//			moveDirection.z = 0; 
//			moveDirection.Normalize();
//
//			if (isInvincible) {
//				//2
//				timeSpentInvincible += Time.deltaTime;
//				
//				//3
//				if (timeSpentInvincible < 3f) {
//					float remainder = timeSpentInvincible % .3f;
//					renderer.enabled = remainder > .15f; 
//				}
//				//4
//				else {
//					renderer.enabled = true;
//					isInvincible = false;
//				}
//			}
//		}
//
//		Vector3 target = moveDirection * moveSpeed + currentPosition;
//		transform.position = Vector3.Lerp(currentPosition, target, Time.deltaTime);
//
//		float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
//		transform.rotation = Quaternion.Slerp(transform.rotation, 
//			                 Quaternion.Euler(0, 0, targetAngle), 
//			                 turnSpeed * Time.deltaTime);
//
//		EnforceBounds();
//	}

	void FixedUpdate() {
		
		moveX = Input.GetAxis ("Horizontal");
		moveY = Input.GetAxis ("Vertical");
		rigidbody2D.velocity = new Vector2 (moveX * moveSpeed, moveY * moveSpeed);
		if(Input.GetKey("up") || Input.GetKey("w")) {
			transform.localEulerAngles = new Vector3(0,0,90);
		}
		else if(Input.GetKey("down") || Input.GetKey("s")) {
			transform.localEulerAngles = new Vector3(0,0,270);
		}
		else if(Input.GetKey("left") || Input.GetKey("a")) {
			transform.localEulerAngles = new Vector3(0,0,180);
		}
		else if(Input.GetKey("right") || Input.GetKey("d")) {
			transform.localEulerAngles = new Vector3(0,0,0);
		}
	}

	public void SetColliderForSprite(int spriteNum) {
		colliders[currentColliderIndex].enabled = false;
		currentColliderIndex = spriteNum;
		colliders[currentColliderIndex].enabled = true;
	}

	
	
	void OnTriggerEnter2D(Collider2D other) {
		if(other.CompareTag("cat")) {
			audio.PlayOneShot(catContactSound);
			Transform followTarget = congaLine.Count == 0 ? transform : congaLine[congaLine.Count-1];
			other.transform.parent.GetComponent<CatController>().JoinConga( followTarget, moveSpeed, turnSpeed );
			congaLine.Add(other.transform);

			if (congaLine.Count >= 5) {
				Debug.Log("You won!");
				Application.LoadLevel("WinScene");
			}
		}
		else if(!isInvincible && other.CompareTag("enemy")) {
			audio.PlayOneShot(enemyContactSound);
			isInvincible = true;
			timeSpentInvincible = 0;
			for( int i = 0; i < 2 && congaLine.Count > 0; i++ ) {
				int lastIdx = congaLine.Count-1;
				Transform cat = congaLine[ lastIdx ];
				congaLine.RemoveAt(lastIdx);
				cat.parent.GetComponent<CatController>().ExitConga();
			}
			if (--lives <= 0) {
				Debug.Log("You lost!");
				Application.LoadLevel("LoseScene");
			}
		}
	}

	private void EnforceBounds()
	{
		// 1
		Vector3 newPosition = transform.position; 
		Camera mainCamera = Camera.main;
		Vector3 cameraPosition = mainCamera.transform.position;
		
		// 2
		float xDist = mainCamera.aspect * mainCamera.orthographicSize; 
		float xMax = cameraPosition.x + xDist;
		float xMin = cameraPosition.x - xDist;
		
		// 3
		if ( newPosition.x < xMin || newPosition.x > xMax ) {
			newPosition.x = Mathf.Clamp( newPosition.x, xMin, xMax );
			moveDirection.x = -moveDirection.x;
		}
		// TODO vertical bounds
		
		// 4
		transform.position = newPosition;
	}
}
