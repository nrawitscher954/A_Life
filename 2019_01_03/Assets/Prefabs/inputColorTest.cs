using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputColorTest : MonoBehaviour {

	
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKey(KeyCode.Q))
        {
            if (gameObject.GetComponent<MeshRenderer>().enabled == false) gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (gameObject.GetComponent<MeshRenderer>().enabled == false) gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (gameObject.GetComponent<MeshRenderer>().enabled == false) gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (gameObject.GetComponent<MeshRenderer>().enabled == false) gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.grey;
        }

        if (Input.GetKey(KeyCode.T))
        {
            if (gameObject.GetComponent<MeshRenderer>().enabled == false) gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
        }

        if (Input.GetKey(KeyCode.Y))
        {
            if (gameObject.GetComponent<MeshRenderer>().enabled == false) gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

    }
}
