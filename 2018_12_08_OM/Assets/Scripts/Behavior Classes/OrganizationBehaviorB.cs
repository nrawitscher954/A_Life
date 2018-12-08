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



    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="force"></param>
        /// <param name="Interval_Seconds"></param> Repetition frecuency in seconds
        /// <returns></returns>
        IEnumerator ApplyWindForce(float force, int interval)
        {
            //Vector3 wind = new Vector3(1, 2, 3);


            //Vector3 wind = new Vector3(Mathf.PerlinNoise(0f, 0.1f), Mathf.PerlinNoise(0.2f, 0.6f), Mathf.PerlinNoise(0.2f, 0.2f));
            //wind.Normalize();

            //wind *= force;


            base.Wander();

            //base.IApplyForces(wind);




            yield return new WaitForSeconds(interval);

           
        }



        IEnumerator ApplyWindForce2(float force, int interval)
        {
            //Vector3 wind = new Vector3(1, 2, 3);


            if (SimulationManager.Get().is2D)
            {
                Vector3 wind = new Vector3(UnityEngine.Random.onUnitSphere.x, 0, UnityEngine.Random.onUnitSphere.z); 
                wind.Normalize();

                wind *= force;//0.00001f;

                base.IApplyForces(wind);


                yield return new WaitForSeconds(interval);
            }

            if(SimulationManager.Get().is3D)
            {
                Vector3 wind = new Vector3(UnityEngine.Random.onUnitSphere.x, UnityEngine.Random.onUnitSphere.y, UnityEngine.Random.onUnitSphere.z); 
                wind.Normalize();

                wind *= force;

                base.IApplyForces(wind);


                yield return new WaitForSeconds(interval);
            }


        }



        IEnumerator ApplyStaticVibration(float force , int interval)
        {

            this.gameObject.transform.position += UnityEngine.Random.onUnitSphere * force;
            yield return new WaitForSeconds(interval);
        }
      
        public void FreezedAgentCoditions()
        {
            if (this.gameObject.tag == "Freezed")
            {
               
              //  StartCoroutine(ApplyWindForce2(0.1f, 90)); //0.00001f

                //  StartCoroutine(ApplyStaticVibration(0.008f,60));


                ///////////////// WIND ///////////////// WIND
                //   if (GenerationManager.playWindEffect)
                //   {
                ///     StartCoroutine(ApplyWindForce(0.00001f,2));
                displacementPositions.Add(this.gameObject.transform.position);

                    Vector3 initialPos = displacementPositions[0];
                    Vector3 currentPos = displacementPositions[displacementPositions.Count - 1];

                    if (initialPos.magnitude > 0) totalDisplacement = (currentPos - initialPos).magnitude;
                    this.totalDisplacement = (currentPos - initialPos).magnitude;

                    //////////////////// COLOR FOR WIND IS ALSO HERE!!

                  //  gameObject.GetComponent<MeshRenderer>().material.color = base.DisplacementColor(AP.startColorDisplacement, AP.endColorDisplacement, totalDisplacement * 30);

                    ////////////////////

              //  }

                //if (GenerationManager.playWindEffect && getMyPositionBeforeWindSimCoroutine == false) // ONLY GET INITPOS ONCE!
                //{
                //    initialPos = this.gameObject.transform.position;

                //    getMyPositionBeforeWindSimCoroutine = true;
                //}
                /////////////////// WIND ///////////////// WIND



                if (base.communicationType <2 && SimulationManager.Get().displayColorByComunication)
                {
                    gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorLine;
                }

                if (base.communicationType >= 2 && base.communicationType < 12 && SimulationManager.Get().displayColorByComunication)
                {
                    gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorBranch;
                }

                if (base.communicationType == 12 && SimulationManager.Get().displayColorByComunication)
                {
                    gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorStar;
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

                // StartCoroutine(ApplyWindForce(2, 10));

                // StartCoroutine(ApplyWindForce2(0.01f, 10));
                //  StartCoroutine(ApplyStaticVibration(0.0008f, 60));




                //PixelPopulation pixel = null;
                
                time++;

                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;
                currentPixel.AddClusterAgentNames(this.gameObject.name);

                int chemoAttractionVal = base.neighbours.Count;
                if (addChemicalValue == false)
                {
                    currentPixel.DensityValue *= chemoAttractionVal;
                    base.currentPixelDensityVal = currentPixel.DensityValue;
                    addChemicalValue = true;
                }

                


                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);

              

                base.mySignalReceivers = base.mySignalReceiversTemp.Distinct().ToList();

           


                signalEmitterCounter++;
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());


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
                       // placeHolderOffset = base.currentScalarValueRules; //* 0.4f;
                      //  if (placeHolderOffset < 1) placeHolderOffset = 0.0f; // 0.30 is the index[0] of domain.B when scaled to * 0.4


                        //placeHolderOffset = base.currentScalarValueProximity;

                          placeHolderOffset = 0.0f;
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



                   



                    this.gameObject.GetComponent<SphereCollider>().radius = this.gameObject.GetComponent<SphereCollider>().radius + placeHolderOffset;






                    if (SimulationManager.Get().singleRule) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule04);


                    if (SimulationManager.Get().multipleRules)
                    {

                        //if (SimulationManager.Get().radialScalarField)
                        //{

                        //    if (base.currentScalarValueRules == 0) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule02);
                        //    else CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule21);
                        //}

                       


                        if (SimulationManager.Get().interpolatedScalarField)
                        {
                            ///////////////////////////////// INCLUDE IN THE FUTURE AS USER INPUT IN EMPTY GAME OBJECT OF SIMULATION MANAGER///////////////////////////////////////////////////////////////////////////

                            //List<VectorData> domains = VectorData.ConvertStringToVectorData(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\ToUnity\DomainsForRuleSets.txt");

                            List<VectorData> domains = VectorData.ConvertStringToVectorData(SimulationManager.Get().ImportToUnityDomainsFilePath);

                            ///////////////////////////////// INCLUDE IN THE FUTURE AS USER INPUT IN EMPTY GAME OBJECT OF SIMULATION MANAGER///////////////////////////////////////////////////////////////////////////

                            if (base.currentScalarValueRules >= domains[0].A && base.currentScalarValueRules < domains[0].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule04);

                            if (base.currentScalarValueRules >= domains[1].A && base.currentScalarValueRules < domains[1].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule18);

                            if (base.currentScalarValueRules >= domains[2].A && base.currentScalarValueRules < domains[2].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);

                            if (base.currentScalarValueRules >= domains[3].A && base.currentScalarValueRules <= domains[3].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule03);

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
                    float maxThreshold = 1.68f;

                    base.currentScalarValueDensityCheck = base.currentScalarValueRules;
                    if (base.currentScalarValueDensityCheck > maxThreshold) base.currentScalarValueDensityCheck = maxThreshold;
                    if (base.currentScalarValueDensityCheck < 1.15f) base.currentScalarValueDensityCheck = 0.90f;





                    base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck); //0.9f   //1.2f    1.65f    1.5f 




                    //if (base.currentScalarValuePackingProximity > maxThreshold) base.currentScalarValuePackingProximity = maxThreshold;
                    //if (base.currentScalarValuePackingProximity < 1.26f) base.currentScalarValuePackingProximity = 0.90f;




                    //  base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValuePackingProximity); //0.9f   //1.2f    1.65f    1.5f 


                 // base.PlaceHolderBehavior(DensityRules.RuleA,1.2f);


                    base.ActivateAgents();



                    if (base.communicationType < 2 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorLine;
                    }

                    if (base.communicationType >= 2 && base.communicationType < 12 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorBranch;
                    }

                    if (base.communicationType == 12 && SimulationManager.Get().displayColorByComunication)
                    {
                        gameObject.GetComponent<MeshRenderer>().material.color = SimulationManager.Get().colorStar;
                    }


                    this.searchClosestNeighbour= true;

                }




            }

            base.communicationType = base.mySignalReceivers.Count;

            

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

        }// END METHOD






        public void ISignalReceiverConditions()
        {

            if (this.transform.tag == "SignalReceiver")
            {
               // StartCoroutine(ApplyWindForce2(0.1f, 90));

             
                if(SimulationManager.Get().exportTrailPaths)
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



                    /// RETRIEVE SIGNAL EMMITER
                    string[] separators = { ".", " " };
                    string value = this.gameObject.GetComponent<OrganizationBehavior>().placeHolderTargetForSignalReceiver.name;
                    string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string targetEmmiterAgentIndex = words[2]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
                    int index = Convert.ToInt32(targetEmmiterAgentIndex);
                    base.mySignalEmmiter = AgentPopulation.populationList[index];

                    /// RETRIEVE SIGNAL EMMITER
                    if (addAgentToFamilyTreeCoroutine == false)
                    {
                        //base.familyTree.Add(base.mySignalEmmiter);
                        base.familyTreeTemp = base.mySignalEmmiter.GetComponent<Agent>().familyTree;
                        base.familyTreeTemp.Add(base.mySignalEmmiter);
                        base.familyTree = base.familyTreeTemp;
                        //this.gameObject.transform.parent = base.mySignalEmmiter.transform;
                        addAgentToFamilyTreeCoroutine = true;

                    }





                    target.gameObject.layer = 2; // add tagret to layer 2 to ignore in rayscast
                                                 // This would cast rays only against colliders in layer 2.
                    int layerMask = 1 << 2;

                    // But instead we want to collide against everything except layer 2. The ~ operator does this, it inverts a bitmask.
                    layerMask = ~layerMask;

                  

                    base.positionHistory.Add(this.gameObject.transform.position);
                    base.positionHistoryTraceback = base.positionHistory.GetNLastElements(50);
                    base.currentAgentXValRange = new VectorData(base.positionHistoryTraceback[0].x, base.positionHistoryTraceback[base.positionHistoryTraceback.Count - 1].x);

                    float threshhold = 0.01f;


                   /// bool similar = SharpMath.Similar(base.currentAgentXValRange.A, base.currentAgentXValRange.B, threshhold);


      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    this.distanceToTarget = Vector3.Distance(this.gameObject.transform.position, target.transform.position);

                    if (this.distanceToTarget <= 0.1)
                    {
                        this.gameObject.GetComponent<SphereCollider>().enabled = true;



                    }

                    if (this.distanceToTarget >= 1)
                    {
                        //StartCoroutine(ApplyWindForce(1.3f,10));
                        //StartCoroutine(ApplyWindForce2(0.01f, 10));

                        //this.velocity.Normalize();
                        //Vector3 force = Vector3.zero;
                        //force += this.velocity * 0.8f;//this.drag;
                        //base.IApplyForces(force);




                    }

                    base.Seek(this.gameObject.GetComponent<Agent>(), target);

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



                    //if (similar)
                    //{
                    //   List<Vector3> missedRays;
                    // //  KdTreeVec3<Vec3> missedRays;
                    //    List<Vector3> hitRays;
                    //    int numDir = 12;
                    //    int rayRange = 10;
                    //    Color color = Color.white;
                    //    bool showRays = false;
                    //    base.Sensors(numDir, color, layerMask, out missedRays, out hitRays, showRays, rayRange);

                    //     base.AvoidObstaclesWithSensors(missedRays, target);
                    //    //base.Seek(this.gameObject.GetComponent<Agent>(), target);

                    //}


                    //if (!similar)
                    //{



                    //float deltaVisionRadius = distanceToTarget / VisionRadius;

                    //float deltaVisionRadiusClamped = SharpMath.Clamp(deltaVisionRadius, base.VisionRadius * 0.5f, base.VisionRadius * 1.02f);
                   // base.Seek(this.gameObject.GetComponent<Agent>(), target);






                    //     List<Vector3> missedRays;
                    //    //KdTreeVec3<Vec3> missedRays;
                    //    List<Vector3> hitRays;
                    //    int numDir = 12;
                    //    int rayRange = 10;
                    //    Color color = Color.white;
                    //    bool showRays = false;


                    //    if (this.distanceToTarget > 0.8)
                    //    {
                    //        base.Sensors(numDir, color, layerMask, out missedRays, out hitRays, showRays, rayRange);

                    //        if (missedRays.Count != 0)
                    //        {
                    //           base.AvoidObstaclesWithSensors(missedRays, target);
                    //        }

                    //        //KdTree<Agent> emmitersToAvoid = this.AvoidNonTargetEmmiters(this.gameObject.GetComponent<Agent>(), deltaVisionRadiusClamped, AgentPopulation.emmiterAgentList);
                    //        //StartCoroutine(base.SeparationCoroutine(AgentPopulation.populationList, 0.6f, deltaVisionRadiusClamped, "SignalEmmiter", 2f));
                    //        //base.Repell(emmitersToAvoid, 0.01f);

                    //    }

                    //    if (this.distanceToTarget < 0.8) base.Seek(this.gameObject.GetComponent<Agent>(), target);


                    //KdTree<Agent> emmitersToAvoid = this.AvoidNonTargetEmmiters(this.gameObject.GetComponent<Agent>(), deltaVisionRadiusClamped, AgentPopulation.emmiterAgentList);
                    //StartCoroutine(base.SeparationCoroutine(AgentPopulation.populationList, 0.3f, deltaVisionRadiusClamped, "SignalEmmiter", 2f));
                    //base.Repell(emmitersToAvoid, 0.02f);

                   Debug.DrawLine(this.gameObject.transform.position, target.transform.position, Color.red);





                    //}




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

