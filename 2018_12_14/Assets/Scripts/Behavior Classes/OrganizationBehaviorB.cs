using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


using SharpMatter.SharpMath;
using SharpMatter.SharpDataStructures;
using SharpMatter.Geometry;

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


        //public void DeActivatedPassiveAgentConditions()
        //{
        //    if (this.gameObject.tag == "De-ActivatedPassive")
        //    {

        //        StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));
        //    }


        //}


        IEnumerator ApplyWindForceSignalReceiver(float force, int interval)
        {
            //Vector3 wind = new Vector3(1, 2, 3);


            if (SimulationManager.Get().is2D)
            {
                Vector3 wind = new Vector3(UnityEngine.Random.onUnitSphere.x, 0, UnityEngine.Random.onUnitSphere.z);
                wind.Normalize();

                wind *= force;

                base.IApplyForces(wind);


                yield return new WaitForSeconds(interval);
            }

            if (SimulationManager.Get().is3D)
            {
               
                 Vector3 wind = new Vector3(5f, 5f,5f);
           
                wind.Normalize();

                wind *= force;

                base.IApplyForces(wind);


                yield return new WaitForSeconds(interval);
            }
        }


            IEnumerator ApplyWindForce(float force, int interval)
        {
            //Vector3 wind = new Vector3(1, 2, 3);


            if (SimulationManager.Get().is2D)
            {
                Vector3 wind = new Vector3(UnityEngine.Random.onUnitSphere.x, 0, UnityEngine.Random.onUnitSphere.z); 
                wind.Normalize();

                wind *= force;

                base.IApplyForces(wind);


                yield return new WaitForSeconds(interval);
            }

            if(SimulationManager.Get().is3D)
            {
                // Vector3 wind = new Vector3(UnityEngine.Random.onUnitSphere.x, UnityEngine.Random.onUnitSphere.y, UnityEngine.Random.onUnitSphere.z);
                // Vector3 wind = new Vector3(5f, 5f,5f);
                Vector3 randomVal = UnityEngine.Random.onUnitSphere;
                Vector3 wind = new Vector3(randomVal.x, randomVal.y, randomVal.z);
                wind.Normalize();

                wind *= force;

                base.IApplyForces(wind);


                yield return new WaitForSeconds(interval);
            }


        }


        //public void DeActivatedPassiveAgentConditions()
        //{
        //    if (this.gameObject.tag == "De-ActivatedPassive")
        //    {

        //        StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));
        //    }


        //}


       public void DeActivatedAgentConditions()
        {
            if (this.gameObject.tag == "De-Activated")
            {
                //base.ChangeDisplayTypeKeyBoardInput(gameObject.GetComponent<Agent>(), 0);
                base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);
            }
        }


        public void FreezedAgentConditions()
        {
            if (this.gameObject.tag == "Freezed")
            {
            
                if(SimulationManager.Get().addWindForce == false && SimulationManager.Get().addaptToEnvironmentalForces == false )
                {
                    //base.ChangeDisplayTypeKeyBoardInput(gameObject.GetComponent<Agent>(), 0);

                    base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);
                }

                if (SimulationManager.Get().addWindForce)
                {
                   

                    StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));

                    displacementPositions.Add(this.gameObject.transform.position);

                    //Vector3 initialPos = displacementPositions[0];
                    //Vector3 currentPos = displacementPositions[displacementPositions.Count - 1];

                    //if (initialPos.magnitude > 0) totalDisplacement = (currentPos - initialPos).magnitude;
                    //this.totalDisplacement = (currentPos - initialPos).magnitude;

                    /// Get displacement of all neighbours
                    for (int i = 0; i < base.neighbours.Count; i++)
                    {
                        if (base.neighbours[i] != null)
                        {
                            neighboursDisplacement.Add(neighbours[i].GetComponent<OrganizationBehavior>().totalDisplacement);

                        }
                    }

                    this.totalDisplacement = base.GetTotalDisplacement(displacementPositions);

                    float totalNeighboursDisplacement = neighboursDisplacement.Sum();
                    this.averegeNeighbourDisplacement = totalNeighboursDisplacement / base.neighbours.Count;


                   

                  

                    //base.ChangeDisplayTypeKeyBoardInput(gameObject.GetComponent<Agent>(), totalDisplacement);
                    base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), true, totalDisplacement);

                    //gameObject.GetComponent<MeshRenderer>().material.color = DisplacementColor(SimulationManager.Get().startColorDisplacement, SimulationManager.Get().endColorDisplacement, totalDisplacement*100);
                }


                if (SimulationManager.Get().addWindForce && SimulationManager.Get().addaptToEnvironmentalForces)
                {

                    this.createPlaceHolders = false;
                    this.searchClosestNeighbour = false;
                    this.time = 0;

                    StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));

                    displacementPositions.Add(this.gameObject.transform.position);

                  

                    /// Get displacement of all neighbours
                    for (int i = 0; i < base.neighbours.Count; i++)
                    {
                        if (base.neighbours[i] != null)
                        {
                            neighboursDisplacement.Add(neighbours[i].GetComponent<OrganizationBehavior>().totalDisplacement);

                        }
                    }

                    this.totalDisplacement = base.GetTotalDisplacement(displacementPositions);

                    float totalNeighboursDisplacement = neighboursDisplacement.Sum();
                    this.averegeNeighbourDisplacement = totalNeighboursDisplacement / base.neighbours.Count;


                    FixedJoint[] hingeJoints = GetComponents<FixedJoint>();
                    if (this.averegeNeighbourDisplacement > 70f && AgentPopulation.deActivatedAgentList.Count!=0 && hingeJoints.Length<5)
                    {
                        base.ChangeStateToSignalEmmiter(this.gameObject.GetComponent<Agent>(), 0);

                    }

                    //base.ChangeDisplayTypeKeyBoardInput(gameObject.GetComponent<Agent>(), totalDisplacement);
                    base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), true, totalDisplacement);

                }


       


                Pixel currentPixel;
                base.PixelDataLookUp3D(pixelPop, out currentPixel);
                currentPixel.AddClusterAgentNames(this.gameObject.name);

                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);

                base.currentPixel = currentPixel.PixelName;

              


                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());
              
               
                


                for (int i = base.placeHolderLocalList.Count - 1; i >= 0; i--)
                {

                    if (base.placeHolderLocalList[i] != null)
                    {
                        Destroy(placeHolderLocalList[i].gameObject);
                        base.placeHolderLocalList.RemoveAt(i);

                    }


                }
                


                base.placeHolderLocalList.Clear();
                base.placeHolders.Clear();
                base.mySignalReceiversTemp.Clear();
            }


        }

        public void ISignalEmmiterConditions()
        {

            

            if (this.gameObject.tag == "SignalEmmiter")
            {
              

                if (SimulationManager.Get().addWindForce && this.gameObject.name != "Agent" + " " + AgentPopulation.indexActivatedAgentsList[0].ToString() && base.neighbours.Count > 3)
                {
                    StartCoroutine(ApplyWindForce(SimulationManager.Get().windForce, SimulationManager.Get().windForceIntervalSeconds));
                }



                time++;

                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;
                currentPixel.AddClusterAgentNames(this.gameObject.name);

                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());


                base.mySignalReceivers = base.mySignalReceiversTemp.Distinct().ToList();

           


                signalEmitterCounter++;
               


                if (createPlaceHolders == false && time > 10)
                {



                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    Cell _currentScalarValue;


                    ////if (SimulationManager.Get().ScalarField3d)
                    ////{
                    ////    ScalarField3DDataLookUp(SF, out _currentScalarValue);

                    ////    base.currentScalarCell = _currentScalarValue.CellName;
                    ////    base.currentScalarValue = _currentScalarValue.ScalarValue;

                    ////   // placeHolderOffset = base.currentScalarValue;
                    ////}

                    if (SimulationManager.Get().ScalarField2d)
                    {
                        ScalarFieldDataLookUp(SF, out _currentScalarValue);

                        base.currentScalarCell = _currentScalarValue.CellName;
                        base.currentScalarValueRules = _currentScalarValue.ScalarValueRules;
                        base.currentScalarValuePackingProximity = _currentScalarValue.ScalarValueProximity;
                       // placeHolderOffset = base.currentScalarValueRules * 0.4f;
                      //  if (placeHolderOffset < 1) placeHolderOffset = 0.0f; // 0.30 is the index[0] of domain.B when scaled to * 0.4


                        //placeHolderOffset = base.currentScalarValueProximity;

                          placeHolderOffset = 0.0f;
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



                   



                    this.gameObject.GetComponent<SphereCollider>().radius = this.gameObject.GetComponent<SphereCollider>().radius + placeHolderOffset;






                    if (SimulationManager.Get().singleRule)
                    {
                        CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule09);

                    }


                    if (SimulationManager.Get().multipleRules)
                    {

                        //if (SimulationManager.Get().radialScalarField)
                        //{

                        //    if (base.currentScalarValueRules == 0) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule02);
                        //    else CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule21);
                        //}

                       


                        if (SimulationManager.Get().interpolatedScalarField)
                        {
                            if (SimulationManager.Get().addWindForce && SimulationManager.Get().addaptToEnvironmentalForces)
                            {
                                List<VectorData> domains = VectorData.ConvertStringToVectorData(SimulationManager.Get().ImportToUnityDomainsFilePath);


                                if (this.averegeNeighbourDisplacement < 70)
                                {
                                    if (base.currentScalarValueRules >= domains[0].A && base.currentScalarValueRules < domains[0].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule04);

                                    if (base.currentScalarValueRules >= domains[1].A && base.currentScalarValueRules < domains[1].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule18);

                                    if (base.currentScalarValueRules >= domains[2].A && base.currentScalarValueRules < domains[2].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);

                                    if (base.currentScalarValueRules >= domains[3].A && base.currentScalarValueRules <= domains[3].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule03);
                                }

                                if (this.averegeNeighbourDisplacement > 70)
                                {
                                    CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule09);
                                }
                            }

                            else
                            {
                                List<VectorData> domains = VectorData.ConvertStringToVectorData(SimulationManager.Get().ImportToUnityDomainsFilePath);

                                if (base.currentScalarValueRules >= domains[0].A && base.currentScalarValueRules < domains[0].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule04);

                                if (base.currentScalarValueRules >= domains[1].A && base.currentScalarValueRules < domains[1].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule18);

                                if (base.currentScalarValueRules >= domains[2].A && base.currentScalarValueRules < domains[2].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);

                                if (base.currentScalarValueRules >= domains[3].A && base.currentScalarValueRules <= domains[3].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule03);


                            }


                          

                        }




                    }




                    if (SimulationManager.Get().addFixedJoints)
                    {
                        for (int i = 0; i < placeHolderLocalList.Count; i++)
                        {
                            base.AddFixedJointPlaceHolders(placeHolderLocalList[i]);
                        }
                    }


                    createPlaceHolders = true;

                }

                

                /// Only search closest neighbour once and search the first closest neighbour after n seconds
                /// time > AgentPopulation.timeToInitFirstAgent * 20 &&
                if (this.searchClosestNeighbour == false && time > 50 && AgentPopulation.deActivatedAgentList.Count > 0)
                {
                   

                    if (SimulationManager.Get().addWindForce == false)
                    {
                        float maxThreshold = 1.68f;

                        base.currentScalarValueDensityCheck = base.currentScalarValueRules;
                        if (base.currentScalarValueDensityCheck > maxThreshold) base.currentScalarValueDensityCheck = maxThreshold;
                        if (base.currentScalarValueDensityCheck < 1.15f) base.currentScalarValueDensityCheck = 0.90f;

                        base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck); //0.9f   //1.2f    1.65f    1.5f 
                    }



                    if (SimulationManager.Get().addWindForce == true && SimulationManager.Get().addaptToEnvironmentalForces ==false)
                    {
                        // get my total displacement
                        displacementPositions.Add(this.gameObject.transform.position);


                        this.totalDisplacement = base.GetTotalDisplacement(displacementPositions);

                        // set display
                        base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), true, totalDisplacement);

                        float maxThreshold = 1.68f;

                        base.currentScalarValueDensityCheck = base.currentScalarValueRules;
                        if (base.currentScalarValueDensityCheck > maxThreshold) base.currentScalarValueDensityCheck = maxThreshold;
                        if (base.currentScalarValueDensityCheck < 1.15f) base.currentScalarValueDensityCheck = 1.1f;

                        base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck); //0.9f   //1.2f    1.65f    1.5f 

                        //base.PlaceHolderBehavior(DensityRules.RuleA, 1.2f); //0.9f   //1.2f    1.65f    1.5f 
                    }




                    if (SimulationManager.Get().addWindForce && SimulationManager.Get().addaptToEnvironmentalForces)
                    {
                       // get my total displacement
                        displacementPositions.Add(this.gameObject.transform.position);

                        // get neighbours averege displacement
                        for (int i = 0; i < base.neighbours.Count; i++)
                        {
                            if (base.neighbours[i] != null)
                            {
                                neighboursDisplacement.Add(neighbours[i].GetComponent<OrganizationBehavior>().totalDisplacement);

                            }
                        }

                        this.totalDisplacement = base.GetTotalDisplacement(displacementPositions);

                        float totalNeighboursDisplacement = neighboursDisplacement.Sum();
                        this.averegeNeighbourDisplacement = totalNeighboursDisplacement / base.neighbours.Count;

                        // set display
                        base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), true, totalDisplacement);
                        // if my current displacement is > than the averege sum of my neighbours displacement

                        if(this.averegeNeighbourDisplacement > 0.7f) // choose my minimum density check value to be tighly packed
                        {
                            base.currentScalarValueDensityCheck = 0.90f;
                            base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck);
                        }

                        else // use heat map to read density values
                        {
                            float maxThreshold = 1.68f;

                            base.currentScalarValueDensityCheck = base.currentScalarValueRules;
                            if (base.currentScalarValueDensityCheck > maxThreshold) base.currentScalarValueDensityCheck = maxThreshold;
                            base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck);
                        }
                    }

                    

                    //if (base.currentScalarValuePackingProximity > maxThreshold) base.currentScalarValuePackingProximity = maxThreshold;
                    //if (base.currentScalarValuePackingProximity < 1.26f) base.currentScalarValuePackingProximity = 0.90f;




                    //  base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValuePackingProximity); //0.9f   //1.2f    1.65f    1.5f 


                    // base.PlaceHolderBehavior(DensityRules.RuleA,1.2f);


                    base.ActivateAgents();



                
                    this.searchClosestNeighbour= true;

                }





                if (mySignalReceivers.Count == 1 && mySignalReceivers[0].tag == "Freezed" && searchClosestNeighbourAgain == false)
                {
                    int activatedPlaceHolderCount = 0;
                    for (int i = 0; i < placeHolders.Count; i++)
                    {
                        if (placeHolders[i] != null)
                        {
                            if (placeHolders[i].tag == "ActivatedPlaceHolder")
                            {


                                activatedPlaceHolderCount++;

                            }
                        }
                    }

                    if (activatedPlaceHolderCount == 1)
                    {
                        mySignalReceivers.Clear();
                        mySignalReceiversTemp.Clear();


                        base.ActivateAgents();
                        searchClosestNeighbourAgain = true;
                    }
                }



                if (mySignalReceivers.Count == 2 && mySignalReceivers[0].tag == "Freezed" && mySignalReceivers[1].tag == "Freezed" && searchClosestNeighbourAgain == false)
                {
                    int activatedPlaceHolderCount = 0;
                    for (int i = 0; i < placeHolders.Count; i++)
                    {
                        if (placeHolders[i] != null)
                        {
                            if (placeHolders[i].tag == "ActivatedPlaceHolder")
                            {


                                activatedPlaceHolderCount++;

                            }
                        }
                    }

                    if (activatedPlaceHolderCount == 2)
                    {
                        mySignalReceivers.Clear();
                        mySignalReceiversTemp.Clear();


                        base.ActivateAgents();
                        searchClosestNeighbourAgain = true;
                    }
                }

                /////////////////// IF ALL MY PLACEHOLDERS ARE DEACTIVATED CHANGE MY STATE TO AGENT  /////////////////// 

                if (searchClosestNeighbour == true && createPlaceHolders == true)
                {
                    int DeactivatedPlaceHolderCount = 0;
                    int totalPlaceHolders = placeHolders.Count;

                    for (int i = 0; i < placeHolders.Count; i++)
                    {
                        if (placeHolders[i] != null)
                        {
                            if (placeHolders[i].tag == "DeActivatedPlaceHolder")
                            {
                                DeactivatedPlaceHolderCount++;
                            }

                        }
                    }

                    if (DeactivatedPlaceHolderCount == placeHolders.Count)
                    {
                      

                        if (SimulationManager.Get().addaptToEnvironmentalForces == false) base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                        if (SimulationManager.Get().addaptToEnvironmentalForces == true) base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);//base.ChangeStateToDeactivatedPassive(this.gameObject.GetComponent<Agent>(), 0);

                    }
                }

              
                if (searchClosestNeighbour == true && createPlaceHolders == true)
                {
                    base.communicationType = base.mySignalReceivers.Count;
                   // base.ChangeDisplayTypeKeyBoardInput(gameObject.GetComponent<Agent>(), 0);
                }

                if (SimulationManager.Get().addaptToEnvironmentalForces == true)
                {
                    if(placeHolders.Count==0 && AgentPopulation.deActivatedAgentList.Count==0)
                    {
                        base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);
                    }
                }


               
            }

           // base.communicationType = base.mySignalReceivers.Count;

        }// END METHOD






        public void ISignalReceiverConditions()
        {

            if (this.transform.tag == "SignalReceiver")
            {
             


                base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);



                if (SimulationManager.Get().exportTrailPaths)
                {
                    base.trailData.Add(this.gameObject.transform.position);
                }


               this.gameObject.GetComponent<SphereCollider>().enabled = false;
            

                base.neighboursTemp.Clear();
                base.neighbours.Clear();

                Pixel currentPixel;
                base.PixelDataLookUp3D(pixelPop, out currentPixel);

                currentPixel.MobileAgentCounter();
                currentPixel.AddSignalreceiverName(this.gameObject.name);
                base.currentPixel = currentPixel.PixelName;







                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);



                PlaceHolder target = base.placeHolderTargetForSignalReceiver;

                if (addPlaceHolderTargetHistoryCoroutine == false)
                {
                    base.palceHolderTargetHistoryList.Add(target);
                    addPlaceHolderTargetHistoryCoroutine = true;
                }




                if (target == null)
                {


                    this.gameObject.tag = "Cancelled";
                    AgentPopulation.totalCancelledAgents++;
                    this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    this.gameObject.SetActive(false);

                    //this.gameObject.tag = "De-Activated";




                }


               

          



                if (target != null)
                {


                    this.distanceToTarget = Vector3.Distance(this.gameObject.transform.position, target.transform.position);

                    if (this.distanceToTarget <= 0.1)
                    {
                        this.gameObject.GetComponent<SphereCollider>().enabled = true;
                    }

                    if (this.distanceToTarget >= 1)
                    {
                        if (SimulationManager.Get().addWindForce)
                        {
                            StartCoroutine(ApplyWindForceSignalReceiver(0.02f, SimulationManager.Get().windForceIntervalSeconds));

                            this.gameObject.GetComponent<Agent>().velocity *= 0.02f;
                        }




                    }

                    base.Seek(this.gameObject.GetComponent<Agent>(), target);

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




                   //Debug.DrawLine(this.gameObject.transform.position, target.transform.position, Color.red);





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

                // base.ChangeDisplayTypeKeyBoardInput(gameObject.GetComponent<Agent>(), 0);
                base.ChangeDisplayInspectorInput(gameObject.GetComponent<Agent>(), false);
                base.signalReceiverPassiveCounter++;
                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);

               


            }

        }


        #endregion


    



        public List<GameObject> FindPeople()
   {
     GameObject[] people=GameObject.FindGameObjectsWithTag("Person");

     return people.ToList();
   }
       
    }//END CLASS

}

