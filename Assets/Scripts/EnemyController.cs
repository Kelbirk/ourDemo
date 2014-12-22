using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float speed = -1;
	public Transform spawnPoint;

	void OnBecameInvisible()
	{
		float yMax = Camera.main.orthographicSize - 0.5f;
		transform.position = new Vector3( spawnPoint.position.x, 
		                                 Random.Range(-yMax, yMax), 
		                                 transform.position.z );
	}

	// Updates at fixed intervals based upon the physics simulation.
	void FixedUpdate () {
		rigidbody2D.velocity = new Vector2(speed, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
