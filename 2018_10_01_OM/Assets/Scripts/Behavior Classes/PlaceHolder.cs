using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using SharpMatter.SharpMath;
using OrganizationalModel.Behaviors;

public class PlaceHolder : MonoBehaviour {

    public Agent MysignalReceiver;
    public string PlaceHoldername;
    

  
	// Use this for initialization
	void Start () {
       // DestroyMyself();
    }
	
	// Update is called once per frame
	void Update () {
       
      DestroyMyself();

      //  if (OrganizationBehavior.time > 12 && OrganizationBehavior.getActivePlaceHolders == false) DestroyMyself();

        


    }

    
   

    /// <summary>
    /// 
    /// </summary>
    /// <param name="placeHolderToSearchFrom"></param> place holder to search from
    /// <param name="DeActivatedAgents"></param> Available deActivated agents
    /// <returns> returns closest DeActivated Agent</returns>
    public Agent SearchClosestNeighbour(PlaceHolder placeHolderToSearchFrom, List<Agent> DeActivatedAgents)
    {
        return Utility.ClosestObject(placeHolderToSearchFrom, DeActivatedAgents);
    }

    private void OnTriggerEnter(Collider other)
    {

        //if (MysignalReceiver != null && other.gameObject.name == MysignalReceiver.name && other.gameObject.tag != "Voxel")
        //{
        //    this.gameObject.tag = "CollidedPlaceHolder";
        //}

        if (other.gameObject.tag == "ActivatedPlaceHolder")
        {
            this.gameObject.tag = "CollidedPlaceHolder";
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ActivatedPlaceHolder" ||  other.gameObject.tag=="SignalEmmiter")
        {
           
            if(SharpMath.Similar(other.gameObject.transform.position,this.gameObject.transform.position,0.03f)) 
            {
                this.gameObject.tag = "CollidedPlaceHolder";
            }
            
        }



        if (other.gameObject.tag == "Freezed")

        {
            this.gameObject.tag = "CollidedPlaceHolder";
        }


    }

    private void DestroyMyself()
    {
        if(this.gameObject.tag== "CollidedPlaceHolder")
        {
            Destroy(this.gameObject);
            

        }
    }


    private void DeActivateMyself()
    {
        if (this.gameObject.tag == "CollidedPlaceHolder")
        {
            this.gameObject.SetActive(false);

        }
    }



}
