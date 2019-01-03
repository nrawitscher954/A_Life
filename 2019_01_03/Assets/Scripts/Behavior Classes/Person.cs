using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    public float speed = 0.008f;
    public float rotSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }

        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }

        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
            
        }

        if (isWalking == true)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        if (PeoplePopulation.minVal !=float.NaN & PeoplePopulation.maxVal != float.NaN)
        {

           

            if (transform.position.x < PeoplePopulation.minVal)
            {
                Vector3 vec = transform.forward * -1;
                Vector3 forward = transform.forward;

                float rotAngle = Vector3.Angle( forward,vec);

                transform.Rotate(transform.up, rotAngle);


               // transform.position += new Vector3(-transform.position.x, 0, 0);
            }

            else if (transform.position.x > PeoplePopulation.maxVal-5)
            {
                Vector3 vec = transform.forward * -1;
                Vector3 forward = transform.forward;

                float rotAngle = Vector3.Angle(forward, vec);

                transform.Rotate(transform.up, rotAngle);

               // transform.position += new Vector3(PeoplePopulation.maxVal - transform.position.x, 0, 0);
            }

            if (transform.position.z < PeoplePopulation.minVal)
            {
                Vector3 vec = transform.forward * -1;
                Vector3 forward = transform.forward;

                float rotAngle = Vector3.Angle(forward, vec);

                transform.Rotate(transform.up, rotAngle);

               // transform.position += new Vector3(0, 0, -transform.position.z);
            }

            else if (transform.position.z > PeoplePopulation.maxVal-5)
            {
                Vector3 vec = transform.forward * -1;
                Vector3 forward = transform.forward;

                float rotAngle = Vector3.Angle(forward, vec);

                transform.Rotate(transform.up, rotAngle);

                //transform.position += new Vector3(0, 0, PeoplePopulation.maxVal - transform.position.z);
            }


        }


    }


    IEnumerator Wander()
    {
        int rotTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 4);
        int rotateLorR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 4);
        int walkTime = Random.Range(1, 6);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;

        yield return new WaitForSeconds(walkTime);

        isWalking = false;

        yield return new WaitForSeconds(rotateWait);

        if(rotateLorR ==1)
        {
            isRotatingRight = true;

            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }

        if (rotateLorR == 2)
        {
            isRotatingLeft = true;

            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }

        isWandering = false;


    }
}

