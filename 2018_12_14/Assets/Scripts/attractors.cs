using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attractors : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        this.gameObject.transform.position += Random.onUnitSphere *0.03f;
	}
}
