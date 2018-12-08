using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraRotate : MonoBehaviour {
    public GameObject target;
  
    // Use this for initialization
    void Start () {

        //if (AgentPopulation.time == AgentPopulation.timeToInitFirstAgent && AgentPopulation.runCoroutine == false)
        //{
        //    foreach (Agent item in AgentPopulation.deActivatedAgentList)
        //    {
        //        cen = cen + item.transform.position;
        //    }
        //    cen = cen / AgentPopulation.deActivatedAgentList.Count;

        //    targett = Instantiate(taregt2, cen, Quaternion.identity);
        //}

    }
	
	// Update is called once per frame
	void Update () {





        //if(AgentPopulation.freezedAgentList.Count==0) transform.RotateAround(target.gameObject.transform.position, Vector3.up, Time.deltaTime * 3);

        //else transform.RotateAround(cen, Vector3.up, Time.deltaTime * 3);

        //if (targett != null)
        //{
        //    transform.RotateAround(targett.transform.position, Vector3.up, Time.deltaTime * 3);
        //}

        transform.RotateAround(target.gameObject.transform.position, Vector3.up, Time.deltaTime * 2);

        //transform.localPosition += new Vector3(0f, 0.15f, 0f);
    }
}
