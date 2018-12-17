using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using SharpMatter.SharpMath;
using OrganizationalModel.Behaviors;
using OrganizationalModel.Population;

/// <summary>
/// CONTROL LOG:
/// Edit code to check if if new placeHolder has collided with an older placeHolder. If the Older placeHOlder has and Agent targetm destroy current new 
/// palceHolder
/// </summary>
public class PlaceHolder : MonoBehaviour {

    public Agent MysignalReceiver;
    public string PlaceHoldername;
    public KdTree<Agent> neighbours = new KdTree<Agent>();
    public List<string> neighbourNames = new List<string>();
    public string currentPixel;

 
	
	// Update is called once per frame
	void Update () {
       
      

    }



  

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ActivatedPlaceHolder" && other.gameObject.GetComponent<PlaceHolder>().MysignalReceiver != null)
        {

            this.gameObject.tag = "DeActivatedPlaceHolder";
        }

      
        //if (other.gameObject.tag == "Freezed" || other.gameObject.tag == "SignalEmmiter" || other.gameObject.tag == "De-Activated")
        //{
        //    this.gameObject.tag = "DeActivatedPlaceHolder";
        //}

        if(other.gameObject.tag == "Freezed" && this.MysignalReceiver==null)
        {
           if( SharpMath.Similar(other.gameObject.transform.position, this.gameObject.transform.position,2.24f))
            {
                this.gameObject.tag = "DeActivatedPlaceHolder";
            }
        }

        if (other.gameObject.tag == "SignalEmmiter" && this.MysignalReceiver == null)
        {
            if (SharpMath.Similar(other.gameObject.transform.position, this.gameObject.transform.position, 2.24f))
            {
                this.gameObject.tag = "DeActivatedPlaceHolder";
            }
        }

        if (other.gameObject.tag == "De-Activated" && this.MysignalReceiver == null)
        {
            if (SharpMath.Similar(other.gameObject.transform.position, this.gameObject.transform.position, 2.24f))
            {
                this.gameObject.tag = "DeActivatedPlaceHolder";
            }
        }




    }

    //private void DestroyMyself()
    //{
    //    if(this.gameObject.tag== "CollidedPlaceHolder")
    //    {
    //        Destroy(this.gameObject);

    //    }
    //}



    /// <summary>
    /// Looks up current Pixel that corresponds to the agents position. This method already has internalized
    /// conditions for all agent states
    /// </summary>
    /// <param name="pixel"></param> 
    /// <param name="currentPixel"></param> Pixel object output with query data
    public void PixelDataLookUp3D(PixelPopulation pixel, out Pixel currentPixel)
    {


        Pixel _currentPixel = pixel.Lookup3D(this.gameObject.transform.position);
        currentPixel = _currentPixel;

    }


}
