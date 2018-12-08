using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ClassExtensions;

using SharpMatter.SharpMath;
using SharpMatter.SharpDataStructures;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;
using System.Linq;
using System.Text;
using System;




namespace OrganizationalModel.Behaviors { 
public partial class ReOrganizationBehavior : Agent, IReOrganization<Agent, float>

{
        IEnumerator ApplyWindForce2(float force, int interval)
        {
            //Vector3 wind = new Vector3(1, 2, 3);
            
            

            Vector3 wind = new Vector3(Mathf.PerlinNoise(0f, 0.1f), Mathf.PerlinNoise(0.2f, 0.6f), Mathf.PerlinNoise(0.2f, 0.2f));
            //wind.Normalize();

            wind *= force;//0.00001f;

            this.gameObject.transform.position += wind;


            yield return new WaitForSeconds(interval);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="force"></param>
        /// <param name="Interval_Seconds"></param> Repetition frecuency in seconds
        /// <returns></returns>
        IEnumerator ApplyWindForce(float force, int interval)
        {
            //Vector3 wind = new Vector3(1, 2, 3);


            Vector3 wind = new Vector3(Mathf.PerlinNoise(0f, 0.1f), Mathf.PerlinNoise(0.2f, 0.6f), Mathf.PerlinNoise(0.2f, 0.2f));
          
            wind.Normalize();

            //wind *= force;
            //Gizmos.color = Color.red;
     
            //Gizmos.DrawRay(Vector3.zero, wind);
            base.IApplyForces(wind);


            yield return new WaitForSeconds(interval);


        }


        #region AGENT BEHAVIOR CONDITIONS

        /// <summary>
        /// 
        /// </summary>
        /// 


        public void IAgentStatesBehaviorConditions()
        {
           ISignalReceiverPassiveBehaviorConditions();
           ISignalReceiverBehaviorConditions();
            ISignalEmmiterBehaviorConditions();
             FreezedAgentBehaviorConditions();

            DeactivatedAgentBehaviorConditions();

            WandererBehaviorConditions();
        }

        #region WANDERING STUFf...
        public void WandererBehaviorConditions()
        {

            if (this.gameObject.tag == "Wanderer")
            {

              


                countToSearchForEmptyPixel++;

                base.neighboursTemp.Clear();
                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;

                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());

                if (SimulationManager.Get().addFixedJoints)
                {
                    var Container = gameObject.GetComponent<FixedJoint>();

                    Destroy(Container);
                }
                   
                //    if (currentPixel.CountMobileAgents > 1) 

                base.Wander();

                if (currentPixel.CountMobileAgents < 2 || countToSearchForEmptyPixel > wandererTreshhold) base.ChangeStateToSignalReceiver(this.gameObject.GetComponent<Agent>(), 0);

               // if (countToSearchForEmptyPixel > internalTreshhold) base.ChangeStateToSignalReceiver(this.gameObject.GetComponent<Agent>(), 0);

                Bounds();

                KdTree<Agent> neighbours = base.FindNeighboursKd(this.gameObject.GetComponent<Agent>(), 3.0f, AgentPopulation.populationList);

                base.Separation(neighbours, 0.2f, 1.5f);



            }

        }


        private void Bounds()
        {
            float maxX = 80;
            float maxY = 80;
            float maxZ = 80;



            if (this.gameObject.transform.position.x < 0)
                base.IApplyForces(new Vector3(-this.gameObject.transform.position.x, 0, 0));
            else if (this.gameObject.transform.position.x > maxX)
                base.IApplyForces(new Vector3(maxX - this.gameObject.transform.position.x, 0, 0));

            if (this.gameObject.transform.position.y < 0)
                base.IApplyForces(new Vector3(0, -this.gameObject.transform.position.y, 0));
            else if (this.gameObject.transform.position.y > maxY)
                base.IApplyForces(new Vector3(0, maxY - this.gameObject.transform.position.y, 0));

            if (this.gameObject.transform.position.z < 0)
                base.IApplyForces(new Vector3(0, -this.gameObject.transform.position.z, 0));
            else if (this.gameObject.transform.position.z > maxZ)
                base.IApplyForces(new Vector3(0, 0, maxZ - this.gameObject.transform.position.z));


        }


        //public void Wander()
        //{
        //    ///The displacement force  has its origin at the circle's center, and is constrained by the circle radius. This is an imaginary circle placed at the tip of the agents Vel vector
        //    ///The greater the radius and the distance from character to the circle, the stronger the "push" the character will receive every game frame.
        //    /// This displacement force will be used to interfere with the character's route. It is used to calculate the wander force.
        //    /// 


        //    float WanderAngle = 10f;
        //    Vector3 circleCenter;

        //    circleCenter = velocity;
        //    circleCenter.Normalize();
        //    circleCenter *= (agentRadius * 2f);

        //    WanderAngle += UnityEngine.Random.Range(0f, 50f);

        //    Vector3 displacement = Vector3.right;
        //    displacement *= agentRadius * 1.5f;

        //    float len = displacement.magnitude;
        //    float sin = Mathf.Sin(WanderAngle) * len;
        //    float cos = Mathf.Cos(WanderAngle) * len;

        //    float tx = displacement.x;
        //    float tz = displacement.z;
        //    displacement.x = (cos * tx) + (sin * tz);
        //    displacement.z = (cos * tz) - (sin * tx);


        //    Vector3 wanderforce = circleCenter + displacement;

        //    base.IApplyForces(wanderforce);











        //}

        #endregion
        public void CalculateEnergyLevelsOnGeneration_0()
        {


            if (this.gameObject.tag == "Freezed") base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
            if (this.gameObject.tag == "SignalEmmiter") base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
            if (this.gameObject.tag == "SignalReceiver") base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
            if (this.gameObject.tag == "SignalReceiverPassive") base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
        }



        public void ShareEnergyLevelsOnGeneration_0()
        {


            if (this.gameObject.tag == "Freezed") base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());
            if (this.gameObject.tag == "SignalEmmiter") base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());
            if (this.gameObject.tag == "SignalReceiverPassive") base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());
        }



        public void DeactivatedAgentBehaviorConditions()
        {
            if (this.gameObject.tag == "De-Activated")
            {

                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;



                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "Wanderer"));
                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "SignalReceiver"));
                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "SignalReceiverPassive"));
                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "SignalEmmiter"));
                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "Freezed"));
                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "Cancelled"));

                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());




                //if (GenerationManager.generationChange)
                //{



                //    currentPixel.ClusterAgentCounter();

                //    currentPixel.AddClusterAgentNames(this.gameObject.name);
                //    base.myNameInCurrentPixel = currentPixel.ClusterHistoryrAgentNames;
                //    //int occurences = currentPixel.ClusterHistoryrAgentNames.NumberOfOccurences(this.gameObject.name);
                //    //base.timesInCurrentPixel = occurences;



                //}



            }


        }





        public void FreezedAgentBehaviorConditions()
        {

            if (this.gameObject.tag == "Freezed")
            {
                base.neighboursTemp.Distinct().ToList();
                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;


                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "Wanderer"));
                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "SignalReceiver"));
                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "SignalReceiverPassive"));
                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "Cancelled"));
                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "De-Activated"));



                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());

                ////////////////FOR SOME REASON THIS DECRESES PERFORMANCE DURING GENERATION CHANGE!!!!!///////////////////////////
                //if (GenerationManager.generationChange)
                //{
                //    currentPixel.ClusterAgentCounter();
                //    currentPixel.AddClusterAgentNames(this.gameObject.name);
                //    base.myNameInCurrentPixel = currentPixel.ClusterHistoryrAgentNames;
                //    int occurences = currentPixel.ClusterHistoryrAgentNames.NumberOfOccurences(this.gameObject.name);
                //    base.timesInCurrentPixel = occurences;
                //}
                ////////////////FOR SOME REASON THIS DECRESES PERFORMANCE DURING GENERATION CHANGE!!!!!///////////////////////////
                //if (destroyPlaceHolders == false)
                //{
                    for (int i = base.placeHolderLocalList.Count - 1; i >= 0; i--)
                    {

                        if (base.placeHolderLocalList[i] != null)
                        {
                            Destroy(placeHolderLocalList[i].gameObject);
                            base.placeHolderLocalList.RemoveAt(i);

                        }


                    }

                //    destroyPlaceHolders = true;
                //}



            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void ISignalEmmiterBehaviorConditions()
        {
            if (this.gameObject.tag == "SignalEmmiter")
            {
                time_ReOrg++;

                base.neighboursTemp.Distinct().ToList();
                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());


                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "Wanderer"));
                StartCoroutine(base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "SignalReceiver"));
                // base.RemoveNeighboursByTag(gameObject.GetComponent<Agent>(), "SignalReceiverPassive");

                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);

                base.mySignalReceivers = base.mySignalReceiversTemp.Distinct().ToList();


            


                if (GenerationManager.generationChange)
                {



                    currentPixel.ClusterAgentCounter();
                    currentPixel.AddClusterAgentNames(this.gameObject.name);
                    base.myNameInCurrentPixel = currentPixel.ClusterHistoryrAgentNames;
                    int occurences = currentPixel.ClusterHistoryrAgentNames.NumberOfOccurences(this.gameObject.name);
                    base.timesInCurrentPixel = occurences;



                }





                if (createPlaceHolders_ReOrg == false && time_ReOrg > 10)
                {

                    

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    Cell _currentScalarValue;


                    //if (SimulationManager.Get().ScalarField3d)
                    //{
                    //    ScalarField3DDataLookUp(SF, out _currentScalarValue);

                    //    base.currentScalarCell = _currentScalarValue.CellName;
                    //    base.currentScalarValue = _currentScalarValue.ScalarValue;

                    //   // placeHolderOffset = base.currentScalarValue;
                    //}

                    if (SimulationManager.Get().ScalarField2d)
                    {
                        ScalarFieldDataLookUp(SF, out _currentScalarValue);

                        base.currentScalarCell = _currentScalarValue.CellName;
                        base.currentScalarValueRules = _currentScalarValue.ScalarValueRules;
                        base.currentScalarValuePackingProximity = _currentScalarValue.ScalarValueProximity;
                        //placeHolderOffset = base.currentScalarValueRules * 0.1f;

                        //placeHolderOffset = base.currentScalarValueProximity;

                        placeHolderOffset = 0.0f;
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                






                 
                    this.gameObject.GetComponent<SphereCollider>().radius = this.gameObject.GetComponent<SphereCollider>().radius + placeHolderOffset;






                    if (SimulationManager.Get().singleRule) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule23);



                    //if (SimulationManager.Get().radialScalarField)
                    //{

                    //    if (base.currentScalarValueRules == 0) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);
                    //    else CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule06);
                    //}




                    if (SimulationManager.Get().interpolatedScalarField)
                    {
                        ///////////////////////////////// INCLUDE IN THE FUTURE AS USER INPUT IN EMPTY GAME OBJECT OF SIMULATION MANAGER///////////////////////////////////////////////////////////////////////////

                  
                        List<VectorData> domains = VectorData.ConvertStringToVectorData(SimulationManager.Get().ImportToUnityDomainsFilePath);

                        ///////////////////////////////// INCLUDE IN THE FUTURE AS USER INPUT IN EMPTY GAME OBJECT OF SIMULATION MANAGER///////////////////////////////////////////////////////////////////////////
                        ////FOR SOME REASON THE INNER AND OUTER DOMAINS SOMETIMES ARE NOT EQUAL IN SOME CASES, FOR EXAMPLE:

                        // [0] 0.65098,0.839216
                        // [1] 0.839216,1.45098 ----> 1.45098 should also appear in item 2. but instead the value is 1.454902
                        // [2] 1.454902,2.498039
                        // [3]2.501961,2.992157

                        // for this reason when some signal emmiters that have 0 neighbours dont create their place holders.
                        // to prevent this this. This could be changed from & base.currentScalarValueRules < domains[0].B   to      & base.currentScalarValueRules <= domains[0].B

                        if (base.currentScalarValueRules >= domains[0].A && base.currentScalarValueRules < domains[0].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule03);

                        if (base.currentScalarValueRules >= domains[1].A && base.currentScalarValueRules < domains[1].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule01);

                        if (base.currentScalarValueRules >= domains[2].A && base.currentScalarValueRules < domains[2].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule02);

                        if (base.currentScalarValueRules >= domains[3].A && base.currentScalarValueRules <= domains[3].B) CreatePlaceHolders(this.gameObject.GetComponent<Agent>(), placeHolderOffset, DirectionalityRules.Rule04);

                       

                    }



               

                    if (SimulationManager.Get().addFixedJoints)
                    {
                        for (int i = 0; i < placeHolderLocalList.Count; i++)
                        {
                            base.AddFixedJointPlaceHolders(placeHolderLocalList[i]);
                        }
                    }


                 

                    this.createPlaceHolders_ReOrg = true;

                }



                /// Only search closest neighbour once and search the first closest neighbour after n seconds
                /// time > AgentPopulation.timeToInitFirstAgent * 20 &&
                if (this.searchClosestNeighbour_ReOrg== false && time_ReOrg > 50 && AgentPopulation.deActivatedAgentList.Count > 0)
                {

                    float maxThreshold = 1.60f;
          

                    base.currentScalarValueDensityCheck = base.currentScalarValueRules;
                    if (base.currentScalarValueDensityCheck > maxThreshold) base.currentScalarValueDensityCheck = maxThreshold;
                    if (base.currentScalarValueDensityCheck < 1.0f) base.currentScalarValueDensityCheck = 1.0f;




                    base.PlaceHolderBehavior(DensityRules.RuleA, base.currentScalarValueDensityCheck); //0.9f   //1.2f    1.65f   
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






                    this.searchClosestNeighbour_ReOrg = true;

                }




                base.communicationType = base.mySignalReceivers.Count;


           

                if (mySignalReceivers.Count == 1 && mySignalReceivers[0].tag == "Freezed" && this.searchClosestNeighbourAgain_ReOrg == false)
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
                        this.searchClosestNeighbourAgain_ReOrg = true;
                    }
                }



                if (mySignalReceivers.Count == 2 && mySignalReceivers[0].tag == "Freezed" && mySignalReceivers[1].tag == "Freezed" && searchClosestNeighbourAgain_ReOrg == false)
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
                        searchClosestNeighbourAgain_ReOrg = true;
                    }
                }


                //  if (placeHolders.Count == 0 && createPlaceHolders_ReOrg == true && searchClosestNeighbour_ReOrg==true) base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);



            }



        }




        public void ISignalReceiverBehaviorConditions()
        {


            if (this.transform.tag == "SignalReceiver")
            {


                //DrawVelVector();
                if (!SimulationManager.Get().addRigidBodyCollider)
                {
                    this.gameObject.GetComponent<SphereCollider>().enabled = false;
                }

                base.neighboursTemp.Clear();
                base.neighbours.Clear();

                Pixel currentPixel;
                base.PixelDataLookUp3D(pixelPop, out currentPixel);

                currentPixel.MobileAgentCounter();
                currentPixel.AddSignalreceiverName(this.gameObject.name);
                base.currentPixel = currentPixel.PixelName;



                if (SimulationManager.Get().addRigidBodyCollider)
                {
                    this.gameObject.GetComponent<Collider>().isTrigger = false;  //false 
                  
                    rb.isKinematic = true;
                   
                }



                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
             



                PlaceHolder target = base.placeHolderTargetForSignalReceiver;

                if (addPlaceHolderTargetHistoryCoroutine == false)
                {
                    base.palceHolderTargetHistoryList.Add(target);
                    addPlaceHolderTargetHistoryCoroutine = true;
                }



                if (target == null)
                {


                    //this.gameObject.tag = "Cancelled";
                    //AgentPopulation.totalCancelledAgents++;
                    //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    //this.gameObject.SetActive(false);

                    //this.gameObject.tag = "Freezed";

                    base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(),0);
                    this.gameObject.GetComponent<SphereCollider>().enabled = true;

                    if (SimulationManager.Get().addRigidBodyCollider)
                    {
                        this.gameObject.GetComponent<Collider>().isTrigger = false;  //false 
                        rb.isKinematic = false;

                    }

                }








                if (target != null)
                {



                    /// RETRIEVE SIGNAL EMMITER
                    string[] separators = { ".", " " };
                    string value = this.gameObject.GetComponent<ReOrganizationBehavior>().placeHolderTargetForSignalReceiver.name;
                    string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string targetEmmiterAgentIndex = words[2]; /// THIS WILL ALWAYS OUTPUT A NUMBER ---> EXAMPLE: Place Holder 100.1 it will output 100
                    int index = Convert.ToInt32(targetEmmiterAgentIndex);
                    base.mySignalEmmiter = AgentPopulation.populationList[index];

                    /// RETRIEVE SIGNAL EMMITER
                    //if (addAgentToFamilyTreeCoroutine == false)
                    //{
                    //    //base.familyTree.Add(base.mySignalEmmiter);
                    //    base.familyTreeTemp = base.mySignalEmmiter.GetComponent<Agent>().familyTree;
                    //    base.familyTreeTemp.Add(base.mySignalEmmiter);
                    //    base.familyTree = base.familyTreeTemp;
                    //    //this.gameObject.transform.parent = base.mySignalEmmiter.transform;
                    //    addAgentToFamilyTreeCoroutine = true;

                    //}





                    target.gameObject.layer = 2; // add tagret to layer 2 to ignore in rayscast
                                                 // This would cast rays only against colliders in layer 2.
                    int layerMask = 1 << 2;

                    // But instead we want to collide against everything except layer 2. The ~ operator does this, it inverts a bitmask.
                    layerMask = ~layerMask;



                    base.positionHistory.Add(this.gameObject.transform.position);
                    base.positionHistoryTraceback = base.positionHistory.GetNLastElements(50);
                    base.currentAgentXValRange = new VectorData(base.positionHistoryTraceback[0].x, base.positionHistoryTraceback[base.positionHistoryTraceback.Count - 1].x);

                    float threshhold = 0.01f;


                    bool similar = SharpMath.Similar(base.currentAgentXValRange.A, base.currentAgentXValRange.B, threshhold);


                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    this.distanceToTarget = Vector3.Distance(this.gameObject.transform.position, target.transform.position);

                    if (this.distanceToTarget <= 0.1)
                    {
                        if (!SimulationManager.Get().addRigidBodyCollider)
                        {
                            this.gameObject.GetComponent<SphereCollider>().enabled = true;
                        }
                 

                        if (SimulationManager.Get().addRigidBodyCollider)
                        {
                            this.gameObject.GetComponent<Collider>().isTrigger = false;  //false 
                            rb.isKinematic = false;

                        }


                    }

                    if (mySignalEmmiter.tag == "Freezed" && distanceToTarget <= 0.1) base.ChangeStateToFreezed(this.gameObject.GetComponent<Agent>(), 0);

                    if (this.distanceToTarget >= 3)
                    {
                       // StartCoroutine(ApplyWindForce(2.5f,10));
                       //// StartCoroutine(ApplyWindForce2(0.01f, 10));

                       // this.velocity.Normalize();
                       // Vector3 force = Vector3.zero;
                       // force += this.velocity * 0.5f;//this.drag;
                       // base.IApplyForces(force);




                    }

                    base.Seek(this.gameObject.GetComponent<Agent>(), target);

                    //if(distanceToTarget>5)
                    //{

                    //    List<Agent> n = behavior.flock.FindNeighbours(this.gameObject.GetComponent<Agent>(), 3f, AgentPopulation.signalReceiverAgentList);

                    //    //base.neighbours = behavior.flock.FindNeighbours(this.gameObject.GetComponent<Agent>(), 3f, AgentPopulation.signalReceiverAgentList);

                    //    if (n.Count != 0)
                    //    {
                    //        base.IApplyForces(behavior.flock.Allignment(n, 0.02f));
                    //        base.IApplyForces(behavior.flock.Cohesion(n, 0.02f));

                    //    }

                    //    ////else

                    //}
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



                    //if (similar)
                    //{
                    //    List<Vector3> missedRays;
                    //    List<Vector3> hitRays;
                    //    int numDir = 90;
                    //    int rayRange = 10;
                    //    Color color = Color.white;
                    //    bool showRays = false;
                    //    base.Sensors(numDir, color, layerMask, out missedRays, out hitRays, showRays, rayRange);



                    //    if (this.distanceToTarget > 0.8)
                    //    {
                    //        base.Sensors(numDir, color, layerMask, out missedRays, out hitRays, showRays, rayRange);

                    //        if (missedRays.Count != 0)
                    //        {
                    //            base.AvoidObstaclesWithSensors(missedRays, target);
                    //        }
                    //    }

                    //    if (this.distanceToTarget < 0.8) base.Seek(this.gameObject.GetComponent<Agent>(), target);

                    //}


                    //if (!similar)
                    //{



                    //    float deltaVisionRadius = distanceToTarget / VisionRadius;

                    //    float deltaVisionRadiusClamped = SharpMath.Clamp(deltaVisionRadius, base.VisionRadius * 0.5f, base.VisionRadius * 1.02f);








                    //base.DrawRaysFromSphere(20, deltaVisionRadiusClamped, Color.grey);




                    //    base.Separation(AgentPopulation.populationList, 1.2f, deltaVisionRadiusClamped,"SignalEmmiter");
                    //   base.Repell(emmitersToAvoid, 0.01f);





                    //List<Vector3> missedRays;
                    //List<Vector3> hitRays;
                    //int numDir = 120;
                    //float rayRange = 20f;
                    //Color color = Color.white;
                    //bool showRays = false;

                    //if (this.distanceToTarget > 0.8)
                    //{
                    //    base.Sensors(numDir, color, layerMask, out missedRays, out hitRays, showRays, rayRange);

                    //    if (missedRays.Count != 0)
                    //    {
                    //        base.AvoidObstaclesWithSensors(missedRays, target);
                    //    }


                    //    KdTree<Agent> emmitersToAvoid = this.AvoidNonTargetEmmiters(this.gameObject.GetComponent<Agent>(), deltaVisionRadiusClamped, AgentPopulation.emmiterAgentList);

                    //    base.Separation(AgentPopulation.populationList, 0.6f, deltaVisionRadiusClamped, "SignalEmmiter");
                    //    base.Repell(emmitersToAvoid, 0.01f);
                    //}

                    //if (this.distanceToTarget < 0.8) base.Seek(this.gameObject.GetComponent<Agent>(), target);




                    Debug.DrawLine(this.gameObject.transform.position, target.transform.position, Color.red);



                        

                  //  }




                }///end if target is not null conditions

            }// END SIGNAL RECEIVER CONDITIONS


        }

        /// <summary>
        /// 
        /// </summary>
        public void ISignalReceiverPassiveBehaviorConditions()
        {

          

            if (this.gameObject.tag == "SignalReceiverPassive")

            {
                base.signalReceiverPassiveCounter++;
                base.CalculateEnergyLevels(SimulationManager.Get().gridDistribution_6Neighbours, SimulationManager.Get()._12Neighbours, SimulationManager.Get().is3D, SimulationManager.Get().is2D);
                base.EnergyLevelSharing(this.gameObject.GetComponent<Agent>());

                if (base.signalReceiverPassiveCounter > 100) base.ChangeStateToSignalEmmiter(this.gameObject.GetComponent<Agent>(), 0);

            } /// END CONDITIONS SIGNAL RECEIVER PASSIVE

        }


        #endregion


  







    } // END CLASS


} // END NAMESPACE
