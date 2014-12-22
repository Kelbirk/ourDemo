using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KittenCreator : MonoBehaviour {

	public float minSpawnTime = 0.75f; 
	public float maxSpawnTime = 2f; 

	public int pooledAmount = 15;
	List<GameObject>cats;

	public GameObject catPrefab;


	// Use this for initialization
	void Start () {
		cats = new List<GameObject>();
		for(int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate(catPrefab);
			obj.SetActive(false);
			cats.Add(obj);
		}
		Invoke("SpawnCat",minSpawnTime);
	}
	
	void SpawnCat() {
		Camera camera = Camera.main;
		Vector3 cameraPos = camera.transform.position;
		float xMax = camera.aspect * camera.orthographicSize;
		float xRange = camera.aspect * camera.orthographicSize * 1.75f;
		float yMax = camera.orthographicSize - 0.5f;
		
		// 2
		Vector3 catPos = 
			new Vector3(cameraPos.x + Random.Range(xMax - xRange, xMax),
			            Random.Range(-yMax, yMax),
			            catPrefab.transform.position.z);
		
		// 3
		for(int i = 0; i < cats.Count; i++) {
			if(!cats[i].activeInHierarchy) {
				cats[i].transform.position = catPos;
				cats[i].SetActive(true);
				break;
			}
		}
		Invoke("SpawnCat", Random.Range(minSpawnTime, maxSpawnTime));
	}
}
