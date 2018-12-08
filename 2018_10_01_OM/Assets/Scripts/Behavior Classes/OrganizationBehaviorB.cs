using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


using SharpMatter.SharpMath;
using SharpMatter.SharpDataStructures;
using SharpMatter.SharpData.Rtree;

using ClassExtensions;
using OrganizationalModel.Population;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Managers;
using OrganizationalModel.UtilityFunctions;
////////////////////////////////CONTROL LOG / NOTES ////////////////////////////////
/// <summary>
/// 
/// 
/// 
/// 
/// 
/// 
/// 
/// </summary>

namespace OrganizationalModel.Behaviors
{
    public partial class OrganizationBehavior : Agent, IOrganization<Agent>
    {

      


        #region AGENT BEHAVIOR CONDITIONS



        public void IAgentBehaviorConditions()
        {

            ISignalEmmiterConditions();
            ISignalReceiverConditions();
            ISignalReceiverPassiveConditions();
            //DoubleCheckNeighbours();
            FreezedAgentCoditions();

          



        }


      
        public void FreezedAgentCoditions()
        {
            if (this.gameObject.tag == "Freezed")
            {
                Pixel currentPixel;
                base.PixelDataLookUp3D(pixelPop, out currentPixel);

                base.CalculateEnergyLevels( placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);

                base.currentPixel = currentPixel.PixelName;

                

                for (int i = base.placeHolderLocalList.Count - 1; i >= 0; i--)
                {

                    if (base.placeHolderLocalList[i] != null)
                    {
                        Destroy(placeHolderLocalList[i].gameObject);
                        base.placeHolderLocalList.RemoveAt(i);

                    }


                }
                base.activatedPlaceHolders.Clear();
                base.placeHolderLocalList.Clear();
                base.mySignalReceiversTemp.Clear();
            }


        }

        public void ISignalEmmiterConditions()
        {

            if (this.gameObject.tag == "SignalEmmiter")
            {
                

                time++;

                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;

                base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);

             

                base.mySignalReceivers = base.mySignalReceiversTemp.Distinct().ToList();

                List<Agent> availableDeActivatedAgents = NotSelectedDeActivatedAgent();


                signalEmitterCounter++;



                if (createPlaceHolders == false && time > 10)
                {
                    float placeHolderOffset = 0.0f; //UnityEngine.Random.Range(0f,0.7f);
                    this.gameObject.GetComponent<SphereCollider>().radius = this.gameObject.GetComponent<SphereCollider>().radius + placeHolderOffset;
                    CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset);




                    createPlaceHolders = true;

                }


                if (time > 20 && this.getActivePlaceHolders == false)
                {


                    RemoveUnavailablePlaceHolders();
                    base.totalActivatedPlaceHolders = base.placeHolderLocalList.Count;





                    getActivePlaceHolders = true;


                }


                /// Only search closest neighbour once and search the first closest neighbour after n seconds
           
                if (searchClosestNeighbour == false && time > 50 && AgentPopulation.deActivatedAgentList.Count > 0)
                {


                    #region INITIALIZE COMMUNICATION TYPE FOR FIRST AGENT IN SIMULATION

                    if (this.gameObject.name == "Agent" + " " + AgentPopulation.indexActivatedAgentsList[0].ToString())
                    {

                        if (placeHolderPop.gridDistribution_6Neighbours) base.communicationType = UnityEngine.Random.Range(1, 7);

                        if (placeHolderPop._12Neighbours) base.communicationType = UnityEngine.Random.Range(1, 13);


                        if (base.communicationType == 1)
                        {
                            PlaceHolderBehavior(1, availableDeActivatedAgents, base.placeHolderLocalList);
                        }

                        if (base.communicationType == 2)
                        {
                            PlaceHolderBehavior(2, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        else if (base.communicationType == 3)
                        {
                            PlaceHolderBehavior(3, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        else if (base.communicationType == 4)
                        {
                            PlaceHolderBehavior(4, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        else if (base.communicationType == 5)
                        {
                            PlaceHolderBehavior(5, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        else if (base.communicationType == 6)
                        {
                            PlaceHolderBehavior(6, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        else if (base.communicationType == 7)
                        {
                            PlaceHolderBehavior(7, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        else if (base.communicationType == 8)
                        {
                            PlaceHolderBehavior(8, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        else if (base.communicationType == 9)
                        {
                            PlaceHolderBehavior(9, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        else if (base.communicationType == 10)
                        {
                            PlaceHolderBehavior(10, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        else if (base.communicationType == 11)
                        {
                            PlaceHolderBehavior(11, availableDeActivatedAgents, base.placeHolderLocalList);

                        }


                        else if (base.communicationType == 12)
                        {
                            PlaceHolderBehavior(12, availableDeActivatedAgents, base.placeHolderLocalList);

                        }





                    }

                    #endregion

                    #region COMMUNICATION TYPE FOR ALL OTHER AGENTS IN SIMULATION

                    else
                    {
                      

                        base.communicationType = UnityEngine.Random.Range(1, base.placeHolderLocalList.Count + 1); // +1 is added because max is exclusive

                        // PlaceHolderBehavior(availableDeActivatedAgents, base.placeHolderLocalList);

                        if (base.communicationType == 1)
                        {
                            PlaceHolderBehavior(1, availableDeActivatedAgents, base.placeHolderLocalList);
                        }

                        if (base.communicationType == 2)
                        {
                            PlaceHolderBehavior(2, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        if (base.communicationType == 3)
                        {
                            PlaceHolderBehavior(3, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        if (base.communicationType == 4)
                        {
                            PlaceHolderBehavior(4, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        if (base.communicationType == 5)
                        {
                            PlaceHolderBehavior(5, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        if (base.communicationType == 6)
                        {
                            PlaceHolderBehavior(6, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        if (base.communicationType == 7)
                        {
                            PlaceHolderBehavior(7, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        if (base.communicationType == 8)
                        {
                            PlaceHolderBehavior(8, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        if (base.communicationType == 9)
                        {
                            PlaceHolderBehavior(9, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        if (base.communicationType == 10)
                        {
                            PlaceHolderBehavior(10, availableDeActivatedAgents, base.placeHolderLocalList);

                        }

                        if (base.communicationType == 11)
                        {
                            PlaceHolderBehavior(11, availableDeActivatedAgents, base.placeHolderLocalList);

                        }


                        if (base.communicationType == 12)
                        {
                            PlaceHolderBehavior(12, availableDeActivatedAgents, base.placeHolderLocalList);

                        }


                    }


                 



                    #endregion






                    searchClosestNeighbour = true;

                }


          

                if (this.signalEmitterCounter > 2000 && AgentPopulation.deActivatedAgentList.Count == 0) IChangeStateToFreezed(this.gameObject.GetComponent<Agent>());



            }

        }// END METHOD






        public void ISignalReceiverConditions()
        {

            if (this.transform.tag == "SignalReceiver")
            {
               

                this.gameObject.GetComponent<SphereCollider>().enabled = false;

         

                Pixel currentPixel;
                base.PixelDataLookUp3D(pixelPop, out currentPixel);

                currentPixel.MobileAgentCounter();
                currentPixel.AddSignalreceiverName(this.gameObject.name);
                base.currentPixel = currentPixel.PixelName;


                base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);



                PlaceHolder target = base.placeHolderTargetForSignalReceiver;






                if (target != null)
                {
                    string[] separators = { ".", " " };
                    string value = this.gameObject.GetComponent<OrganizationBehavior>().placeHolderTargetForSignalReceiver.name;
                    string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string targetEmmiterAgentIndex = words[2]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
                    int index = Convert.ToInt32(targetEmmiterAgentIndex);
                    this.mySignalEmmiter = AgentPopulation.populationList[index];

                }

                if (target == null)
                {


                    this.gameObject.tag = "Freezed";
                    this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    //this.gameObject.SetActive(false);


                }







                if (target != null)
                {

                    target.gameObject.layer = 2; // add tagret to layer 2 to ignore in rayscast
                                                 // This would cast rays only against colliders in layer 2.
                    int layerMask = 1 << 2;

                    // But instead we want to collide against everything except layer 2. The ~ operator does this, it inverts a bitmask.
                    layerMask = ~layerMask;

                    //List<Vector3> missedRays;
                    //List<Vector3> hitRays;
                    //int num = 100;
                    //float length = 5;//Mathf.Infinity;
                    //bool displayRays = false;

                    //base.Sensors(num, length, Color.white, layerMask, out missedRays, out hitRays, displayRays);

                    base.positionHistory.Add(this.gameObject.transform.position);
                    base.positionHistoryTraceback = base.positionHistory.GetNLastElements(50);
                    base.currentAgentXValRange = new VectorData(base.positionHistoryTraceback[0].x, base.positionHistoryTraceback[base.positionHistoryTraceback.Count - 1].x);

                    float threshhold = 0.01f;


                    bool similar = SharpMath.Similar(base.currentAgentXValRange.A, base.currentAgentXValRange.B, threshhold);



                    this.distanceToTarget = Vector3.Distance(this.gameObject.transform.position, target.transform.position);

                    if (this.distanceToTarget <= 0.1)
                    {
                        this.gameObject.GetComponent<SphereCollider>().enabled = true;


                    }




                    if (similar)
                    {

                    //    if (distanceToTarget <= 2)
                    //    {

                           //this.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        //        //Vector3 closestMissedRayToTarget = Utility.ClosestPoint(this.gameObject.GetComponent<ReOrganizationBehavior>().closestN[0].transform.position, missedRays);
                        //        //Vector3 deltaPos = closestMissedRayToTarget - this.transform.position;

                        //        Vector3 deltaPos = missedRays.PickRandomElement();
                        //        //Debug.DrawRay(this.gameObject.transform.position, deltaPos, Color.blue);
                        //        deltaPos.Normalize();
                        //        deltaPos *= 0.2f;

                        //        this.transform.position += deltaPos;
                        base.SeekWithRayCast(this.gameObject.GetComponent<Agent>(), target);
                        //    }

                        //    if (distanceToTarget > 2)
                        //    {
                        //        if (missedRays.Count != 0)
                        //        {
                        //            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.magenta;

                        //            Vector3 deltaPos = missedRays.PickRandomElement(); //- this.gameObject.transform.position;
                        //            Debug.DrawRay(this.gameObject.transform.position, deltaPos, Color.blue);
                        //            deltaPos.Normalize();
                        //            deltaPos *= 0.7f;

                        //            this.transform.position += deltaPos;
                        //        }
                        //    }
                    }


                    if (!similar)
                    {
                        //Color color = (Utility.RGBToFloat(54, 141, 255));
                       // this.gameObject.GetComponent<MeshRenderer>().material.color = color;





                        float deltaVisionRadius = distanceToTarget / VisionRadius;

                        float deltaVisionRadiusClamped = SharpMath.Clamp(deltaVisionRadius, base.VisionRadius * 0.5f, base.VisionRadius);



                        List<Agent> receiverNeighbours = this.FindNeighbours(this.gameObject.GetComponent<Agent>(), base.VisionRadius, AgentPopulation.signalReceiverAgentList);

                        List<Agent> emmitersToAvoid = this.AvoidNonTargetEmmiters(this.gameObject.GetComponent<Agent>(), base.VisionRadius, AgentPopulation.emmiterAgentList); //deltaVisionRadiusClamped

                        base.DrawRaysFromSphere(20, deltaVisionRadiusClamped, Color.grey);


                        ////base.Repell(receiverNeighbours, 0.02f);
                        base.Separation(receiverNeighbours, 0.2f, base.VisionRadius); //0.2f,deltaVisionRadiusClamped
                        base.Separation(AgentPopulation.freezedAgentList, 40f, deltaVisionRadiusClamped); //,base.VisionRadius,0.1f,
                        base.Separation(emmitersToAvoid, 0.1f, deltaVisionRadiusClamped); // 0.1f, deltaVisionRadiusClamped)
                                                                                                                                                        

        


                        base.Seek(this.gameObject.GetComponent<Agent>(), target);
                        // base.Arrive(this.gameObject.GetComponent<Agent>(), target, 20, this.IMaxSpeed, this.IMaxForce);


                        Debug.DrawLine(this.gameObject.transform.position, target.transform.position, Color.red);



                    }




                }///end if target is not null conditions

            }// END SIGNAL RECEIVER CONDITIONS




        }


       



        /// <summary>
        /// S
        /// </summary>
        public void ISignalReceiverPassiveConditions()
        {
            if (this.gameObject.tag == "SignalReceiverPassive")
            {
                base.signalReceiverPassiveCounter++;
                base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);



            }

        }


        #endregion


        #region PLACE HOLDERS




        /// <summary>
        /// Instantiates palace holders and adds them to the local place holder list, sets their parent, their name and their initial state
        /// </summary>
        /// <param name="agent"></param> Agent to create place holders on
        /// <param name="offset"></param> offset distance from agent / packing density
        private void CreatePlaceHolders(Agent agent, float offset)
        {
            this.InstantiatePlaceHolders(agent, offset);
            base.SetPlaceHolderParent();
            base.SetPlaceHolderName();
            base.InitializeState(base.placeHolderLocalList);
            this.SetMaterial();
        }




        /// <summary>
        /// 
        /// </summary>
        private void RemoveUnavailablePlaceHolders()
        {
            for (int i = base.placeHolderLocalList.Count - 1; i >= 0; i--)
            {
                if (base.placeHolderLocalList[i] != null)
                {
                    if (base.placeHolderLocalList[i].gameObject.tag == "CollidedPlaceHolder")
                    {
                        Destroy(base.placeHolderLocalList[i].gameObject);
                        Destroy(base.placeHolderLocalList[i].gameObject.GetComponent<PlaceHolder>());
                        base.placeHolderLocalList.RemoveAt(i);

                    }
                }
            }

        }







        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="offset"></param>
        private void InstantiatePlaceHolders(Agent agent, float offset)
        {
            List<Vector3> placeHolderPositions = placeHolderPop.CreatePlaceHolderVectorPositions(agent, offset, base.neighboursTemp);
            base.placeHolderLocalList = placeHolderPop.CreatePlaceHolders(placeHolderPositions);
        }


        private void SetMaterial()
        {
            if (base.placeHolderLocalList.Count != 0)
            {
                for (int i = 0; i < base.placeHolderLocalList.Count; i++)
                {
                    base.placeHolderLocalList[i].GetComponent<MeshRenderer>().enabled = false;
                }
            }

        }


        /// <summary>
        /// Calls methods on Place holder objects. 
        /// 
        /// </summary>
        /// <param name="_communicationType"></param>
        /// <param name="DeActivatedAgents"></param>
        public void PlaceHolderBehavior(int _communicationType, List<Agent> DeActivatedAgents, List<PlaceHolder> ActivatedPlaceHolders)
        {


            if (ActivatedPlaceHolders.Count < _communicationType)
            {
                _communicationType = ActivatedPlaceHolders.Count;
            }




            ActivatedPlaceHolders.JitterList();
            base.activatedPlaceHolders = ActivatedPlaceHolders.ListSlice(0, _communicationType); //ActivatedPlaceHolders.Count

            base.selectedActivatedPlaceHolders = base.activatedPlaceHolders.Count;

            //StartCoroutine(DelaySignalReceiverSearch(base.activatedPlaceHolders, DeActivatedAgents));





            for (int i = 0; i < activatedPlaceHolders.Count; i++)
            {
                if (activatedPlaceHolders[i] != null)
                {
                    Agent closestN = activatedPlaceHolders[i].SearchClosestNeighbour(activatedPlaceHolders[i], DeActivatedAgents);
                    // print("Im am" + "  " + listPlaceHolders[i].name + " " + "and i selected a closestN");
                    SelectDeActivatedAgent(closestN);

                    base.mySignalReceiversTemp.Add(closestN);
                    /// adds designated signal receiver to its corresponding place holder
                    activatedPlaceHolders[i].MysignalReceiver = closestN;

                    IChangeStateToSignalReceiver(closestN);
                    /// adds designated place holders  to the signal receivers target list
                    closestN.GetComponent<OrganizationBehavior>().placeHolderTargetForSignalReceiver = activatedPlaceHolders[i].GetComponent<PlaceHolder>();

                }

            }


        }





        /// <summary>
        /// Calls methods on Place holder objects. 
        /// 
        /// </summary>
        /// <param name="_communicationType"></param>
        /// <param name="DeActivatedAgents"></param>
        public void PlaceHolderBehavior(List<Agent> DeActivatedAgents, List<PlaceHolder> ActivatedPlaceHolders)
        {





            ActivatedPlaceHolders.JitterList();
            base.activatedPlaceHolders = ActivatedPlaceHolders.ListSlice(0, ActivatedPlaceHolders.Count); //ActivatedPlaceHolders.Count

            base.selectedActivatedPlaceHolders = base.activatedPlaceHolders.Count;

            //StartCoroutine(DelaySignalReceiverSearch(base.activatedPlaceHolders, DeActivatedAgents));





            for (int i = 0; i < activatedPlaceHolders.Count; i++)
            {
                if (activatedPlaceHolders[i] != null)
                {
                    Agent closestN = activatedPlaceHolders[i].SearchClosestNeighbour(activatedPlaceHolders[i], DeActivatedAgents);
                    // print("Im am" + "  " + listPlaceHolders[i].name + " " + "and i selected a closestN");
                    SelectDeActivatedAgent(closestN);

                    base.mySignalReceiversTemp.Add(closestN);
                    /// adds designated signal receiver to its corresponding place holder
                    activatedPlaceHolders[i].MysignalReceiver = closestN;

                    IChangeStateToSignalReceiver(closestN);
                    /// adds designated place holders  to the signal receivers target list
                    closestN.GetComponent<OrganizationBehavior>().placeHolderTargetForSignalReceiver = activatedPlaceHolders[i].GetComponent<PlaceHolder>();

                }

            }


        }














        public IEnumerator DelaySignalReceiverSearch(List<PlaceHolder> listPlaceHolders, List<Agent> listDeActivatedAgents)
        {

            for (int i = 0; i < listPlaceHolders.Count; i++)
            {
                if (listPlaceHolders[i] != null)
                {



                    Agent closestN = listPlaceHolders[i].SearchClosestNeighbour(listPlaceHolders[i], listDeActivatedAgents);
                    // print("Im am" + "  " + listPlaceHolders[i].name + " " + "and i selected a closestN");
                    SelectDeActivatedAgent(closestN);

                    base.mySignalReceiversTemp.Add(closestN);
                    /// adds designated signal receiver to its corresponding place holder
                    listPlaceHolders[i].MysignalReceiver = closestN;

                    IChangeStateToSignalReceiver(closestN);
                    /// adds designated place holders  to the signal receivers target list
                    closestN.GetComponent<OrganizationBehavior>().placeHolderTargetForSignalReceiver = listPlaceHolders[i].GetComponent<PlaceHolder>();






                    yield return new WaitForSeconds(0.02f);
                }


            }
        }






        #endregion

    }//END CLASS

}

