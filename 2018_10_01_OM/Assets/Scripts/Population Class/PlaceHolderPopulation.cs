using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SharpMatter.SharpMath;
using OrganizationalModel.Population;
using OrganizationalModel.BaseClass;

namespace OrganizationalModel.Population
{
    public class PlaceHolderPopulation : MonoBehaviour
    {

        // REFERENCE PREFABS / EMPTY GAMEOBJECTS
        public GameObject placeHolderPrefab;
        //CLASS INSTANCES

        //PUBLIC LISTS

        public static List<GameObject> ActivatedplaceHolderPopulationList;
        //PRIVATE LISTS

        // PUBLIC VARIABLES
        // public bool is5Neighbours;

        // public bool is8Neighbours;
        public bool gridDistribution_6Neighbours;
        public bool _12Neighbours;


        //PRIVATE VARIABLES

        // NON STATIC PUBLIC VARIABLES
        GameObject agentPopulation; /// create variable to store game object that holds script top reference
        private AgentPopulation AP; /// store script to reference in variable

        GameObject placeHolderPopulation;
        private PlaceHolderPopulation placeHolderPop;





        // Use this for initialization
        void Start()
        {

            // // NON STATIC PUBLIC VARIABLE INITIALIZATION

            agentPopulation = GameObject.Find("AgentPopulation"); /// create variable to store game object that holds script top reference
            AP = agentPopulation.GetComponent<AgentPopulation>(); /// store script to reference in variable

            placeHolderPopulation = GameObject.Find("PlaceHolderPopulation");
            placeHolderPop = placeHolderPopulation.GetComponent<PlaceHolderPopulation>();

            ///////////////////////////////METHOD INITIALIZATION////////////////////////////////

            //SetPlaceHolderName();

        }


        void Update()
        {
            //if(PlaceHolderCollisionManager.collision == true)
            //CheckState();

            //ActivatedplaceHolderPopulationList = GameObject.FindGameObjectsWithTag("ActivatedPlaceHolder").ToList();
        }

        ////////////////////////////////METHODS////////////////////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param> Agent to create place holders on
        /// <param name="offset"></param> place holder offset from agent / PACKING DENSITY
        /// <returns> Returns a list of Vector positions </returns>
        public List<Vector3> CreatePlaceHolderVectorPositions(Agent agent, float offset, List<Agent> neighbours)
        {

            // Create list to hold place holder positions
            List<Vector3> placeHolderPositions = new List<Vector3>();

            // get radius of agent prefab
            float agentPrefabRadius = AP.agentPrefab.GetComponent<SphereCollider>().radius;
            // get radius of place holder prefab
            float placeHolderPrefabRadius = placeHolderPrefab.GetComponent<SphereCollider>().radius;

            if (AP.is2D)
            {
                //if(is5Neighbours) // will be 3 neighbours in 2d
                //{ }

                if (gridDistribution_6Neighbours) // will be 4 neighbours in 2d
                {


                    Vector3 cntrPoint = agent.transform.position;

                    Vector3 posX = cntrPoint + new Vector3((placeHolderPrefabRadius + agentPrefabRadius) + offset, 0, 0);
                    Vector3 neGativePosX = cntrPoint + new Vector3((placeHolderPrefabRadius + agentPrefabRadius) + offset, 0, 0) * -1;

                    Vector3 posZ = cntrPoint + new Vector3(0, 0, (placeHolderPrefabRadius + agentPrefabRadius) + offset);
                    Vector3 negativePosZ = cntrPoint + new Vector3(0, 0, (placeHolderPrefabRadius + agentPrefabRadius) + offset) * -1;

                    if (neighbours.Count > 0)
                    {
                        for (int i = 0; i < neighbours.Count; i++)
                        {
                            if (neighbours[i].transform.position != posX) placeHolderPositions.Add(posX);
                            if (neighbours[i].transform.position != neGativePosX) placeHolderPositions.Add(neGativePosX);
                            if (neighbours[i].transform.position != posZ) placeHolderPositions.Add(posZ);
                            if (neighbours[i].transform.position != negativePosZ) placeHolderPositions.Add(negativePosZ);
                        }
                    }

                    if (neighbours.Count == 0)
                    {
                        placeHolderPositions.Add(posX);
                        placeHolderPositions.Add(neGativePosX);
                        placeHolderPositions.Add(posZ);
                        placeHolderPositions.Add(negativePosZ);
                    }


                }


            }


            if (AP.is3D)
            {
                float similarThreshhold = 0.2f;
                if (gridDistribution_6Neighbours)
                {
                    Vector3 cntrPoint = agent.transform.position;


                    Vector3 posX = cntrPoint + new Vector3((placeHolderPrefabRadius + agentPrefabRadius) + offset, 0, 0);
                    Vector3 negativePosX = cntrPoint + new Vector3((placeHolderPrefabRadius + agentPrefabRadius) + offset, 0, 0) * -1;

                    Vector3 posY = cntrPoint + new Vector3(0, (placeHolderPrefabRadius + agentPrefabRadius) + offset, 0);
                    Vector3 negativePosY = cntrPoint + new Vector3(0, (placeHolderPrefabRadius + agentPrefabRadius) + offset, 0) * -1;

                    Vector3 posZ = cntrPoint + new Vector3(0, 0, (placeHolderPrefabRadius + agentPrefabRadius) + offset);
                    Vector3 negativePosZ = cntrPoint + new Vector3(0, 0, (placeHolderPrefabRadius + agentPrefabRadius) + offset) * -1;

                    if (neighbours.Count > 0)
                    {
                        for (int i = 0; i < neighbours.Count; i++)
                        {

                            if (!SharpMath.Similar(neighbours[i].transform.position, posX, similarThreshhold)) placeHolderPositions.Add(posX);
                            if (!SharpMath.Similar(neighbours[i].transform.position, negativePosX, similarThreshhold)) placeHolderPositions.Add(negativePosX);

                            if (!SharpMath.Similar(neighbours[i].transform.position, posY, similarThreshhold)) placeHolderPositions.Add(posY);
                            if (!SharpMath.Similar(neighbours[i].transform.position, negativePosY, similarThreshhold)) placeHolderPositions.Add(negativePosY);

                            if (!SharpMath.Similar(neighbours[i].transform.position, posZ, similarThreshhold)) placeHolderPositions.Add(posZ);
                            if (!SharpMath.Similar(neighbours[i].transform.position, negativePosZ, similarThreshhold)) placeHolderPositions.Add(negativePosZ);
                        }
                    }

                    if (neighbours.Count == 0)
                    {

                        placeHolderPositions.Add(posX);
                        placeHolderPositions.Add(negativePosX);
                        placeHolderPositions.Add(posY);
                        placeHolderPositions.Add(negativePosY);
                        placeHolderPositions.Add(posZ);
                        placeHolderPositions.Add(negativePosZ);
                    }

                }




                if (_12Neighbours)
                {



                    Vector3 cntrPoint = agent.transform.position;

                    float offsetToAvoidCollision = 1.0f; //(agentPrefabRadius *2) * 1.02f;
                    offset += offsetToAvoidCollision;
                    //6 neighbours on the middle layer =============================================================
                    Vector3 P1 = new Vector3(agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);
                    // P1 = (P1 + cntrPoint) * offset;
                    P1 = (P1 * offset) + cntrPoint;



                    Vector3 P2 = new Vector3(2 * agentPrefabRadius, 0, 0);
                    //P2 = P2 + cntrPoint * offset;
                    P2 = (P2 * offset) + cntrPoint;


                    Vector3 P3 = new Vector3(agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);
                    //P3 = P3 + cntrPoint * offset;
                    P3 = (P3 * offset) + cntrPoint;


                    Vector3 P4 = new Vector3(-agentPrefabRadius, 0, -Mathf.Pow(3, 0.5f) * agentPrefabRadius);
                    //P4 = P4 + cntrPoint * offset;
                    P4 = (P4 * offset) + cntrPoint;


                    Vector3 P5 = new Vector3(-2 * agentPrefabRadius, 0, 0);
                    //P5 = P5 + cntrPoint * offset;
                    P5 = (P5 * offset) + cntrPoint;


                    Vector3 P6 = new Vector3(-agentPrefabRadius, 0, Mathf.Pow(3, 0.5f) * agentPrefabRadius);
                    //P6 = P6 + cntrPoint * offset;
                    P6 = (P6 * offset) + cntrPoint;



                    //3 neighbours on the top layer =============================================================
                    Vector3 P7 = new Vector3(0, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);
                    // P7 = (P7 + cntrPoint) * offset;
                    P7 = (P7 * offset) + cntrPoint;


                    Vector3 P8 = new Vector3(agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);
                    //P8 = (P8 + cntrPoint) * offset;
                    P8 = (P8 * offset) + cntrPoint;

                    Vector3 P9 = new Vector3(-agentPrefabRadius, Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);
                    //P9 = (P9 + cntrPoint) * offset;
                    P9 = (P9 * offset) + cntrPoint;

                    //3 neighbours on the bottom layer =============================================================
                    Vector3 P10 = new Vector3(0, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, -Mathf.Pow(3, 0.5f) * 2 / 3 * agentPrefabRadius);
                    //P10 = P10 + cntrPoint * offset;
                    P10 = (P10 * offset) + cntrPoint;


                    Vector3 P11 = new Vector3(-agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);
                    // P11 = P11 + cntrPoint * offset;
                    P11 = (P11 * offset) + cntrPoint;


                    Vector3 P12 = new Vector3(agentPrefabRadius, -Mathf.Pow(6, 0.5f) * 2 / 3 * agentPrefabRadius, Mathf.Pow(3, 0.5f) / 3 * agentPrefabRadius);
                    // P12 = P12 + cntrPoint * offset;
                    P12 = (P12 * offset) + cntrPoint;




                    if (neighbours.Count > 0)
                    {
                        for (int i = 0; i < neighbours.Count; i++)
                        {

                            if (!SharpMath.Similar(neighbours[i].transform.position, P1, similarThreshhold)) placeHolderPositions.Add(P1);
                            if (!SharpMath.Similar(neighbours[i].transform.position, P2, similarThreshhold)) placeHolderPositions.Add(P2);

                            if (!SharpMath.Similar(neighbours[i].transform.position, P3, similarThreshhold)) placeHolderPositions.Add(P3);
                            if (!SharpMath.Similar(neighbours[i].transform.position, P4, similarThreshhold)) placeHolderPositions.Add(P4);

                            if (!SharpMath.Similar(neighbours[i].transform.position, P5, similarThreshhold)) placeHolderPositions.Add(P5);
                            if (!SharpMath.Similar(neighbours[i].transform.position, P6, similarThreshhold)) placeHolderPositions.Add(P6);

                            if (!SharpMath.Similar(neighbours[i].transform.position, P7, similarThreshhold)) placeHolderPositions.Add(P7);
                            if (!SharpMath.Similar(neighbours[i].transform.position, P8, similarThreshhold)) placeHolderPositions.Add(P8);

                            if (!SharpMath.Similar(neighbours[i].transform.position, P9, similarThreshhold)) placeHolderPositions.Add(P9);
                            if (!SharpMath.Similar(neighbours[i].transform.position, P10, similarThreshhold)) placeHolderPositions.Add(P10);

                            if (!SharpMath.Similar(neighbours[i].transform.position, P11, similarThreshhold)) placeHolderPositions.Add(P11);
                            if (!SharpMath.Similar(neighbours[i].transform.position, P12, similarThreshhold)) placeHolderPositions.Add(P12);
                        }
                    }

                    if (neighbours.Count == 0)
                    {

                        placeHolderPositions.Add(P1);
                        placeHolderPositions.Add(P2);
                        placeHolderPositions.Add(P3);
                        placeHolderPositions.Add(P4);
                        placeHolderPositions.Add(P5);
                        placeHolderPositions.Add(P6);
                        placeHolderPositions.Add(P7);
                        placeHolderPositions.Add(P8);
                        placeHolderPositions.Add(P9);
                        placeHolderPositions.Add(P10);
                        placeHolderPositions.Add(P11);
                        placeHolderPositions.Add(P12);
                    }







                }












            }

            return placeHolderPositions;
        }


        /// <summary>
        /// Create invisible place holders on agent. 
        /// </summary>
        /// <param name="placeHolderPositions"></param> Vector positions on instantiate place holders
        ///  <returns></returns> Returns a list of place holder game objects 
        ///  
        public List<PlaceHolder> CreatePlaceHolders(List<Vector3> placeHolderPositions)
        {
            List<PlaceHolder> placeHolderList = new List<PlaceHolder>();

            if (AP.is2D)
            {
                if (gridDistribution_6Neighbours)
                {


                    for (int i = 0; i < placeHolderPositions.Count; i++)
                    {

                        placeHolderList.Add((Instantiate(placeHolderPrefab.GetComponent<PlaceHolder>(), placeHolderPositions[i], Quaternion.identity)));

                    }
                }

            }



            if (AP.is3D)
            {
                if (gridDistribution_6Neighbours)
                {


                    for (int i = 0; i < placeHolderPositions.Count; i++)
                    {

                        placeHolderList.Add((Instantiate(placeHolderPrefab.GetComponent<PlaceHolder>(), placeHolderPositions[i], Quaternion.identity)));

                    }
                }

                if (_12Neighbours)
                {

                    for (int i = 0; i < placeHolderPositions.Count; i++)
                    {

                        placeHolderList.Add((Instantiate(placeHolderPrefab.GetComponent<PlaceHolder>(), placeHolderPositions[i], Quaternion.identity)));

                    }

                }

            }




            return placeHolderList;
        }










    } // END CLASS

}
