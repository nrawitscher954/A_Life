using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

using ClassExtensions;

using SharpMatter.SharpMath;
using SharpMatter.SharpDataStructures;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;

////////////////////////////////CLASS DESCRIPTION////////////////////////////////
/// <summary>
/// 
/// This class contains all agent behaviors
/// 
/// 
/// 
/// 
/// 
/// 
/// 
/// </summary>




////////////////////////////////CONTROL LOG / NOTES ////////////////////////////////
/// <summary>
/// Check Seek function some scaling numbers where changed instead of using the scale of MaxForce
/// 
/// 
/// 
/// 
/// 
/// Bug: Agent receivers choose the same signal emmiters, causing less selected signal emmiters and thus, there will be signal receivers flying around when no
/// emmiters are left.
/// 
/// 
/// 
/// 
/// </summary>
/// 
////////////////////////////////CONTROL LOG / NOTES END ////////////////////////////////

namespace OrganizationalModel.Behaviors
{
    public partial class ReOrganizationBehavior : Agent, IReOrganization<Agent, float>
    {

        ////////////////////////////////DECLARE GLOBAL VARIABLES////////////////////////////////


        public int counter;
        //public List<Agent> closestN = new List<Agent>();
        int positionHistoryCounter = 0;

        //CLASS INSTANCES

        //PUBLIC LISTS


        //PUBLIC STATIC LISTS

        //PRIVATE LISTS




        public int countToSearchForEmptyPixel = 0;


        // PUBLIC VARIABLES
        //public int signalReceiverPassiveCounter;


        //PRIVATE VARIABLES

        private int counterAgentsToRepell;
        private int counterAgentsToAvoid;
        private bool changeStateToSignalReceiverCoroutine = false;
        private bool searchAgentsToRepellCoroutine = false;
        private bool searchAgentsToAvoidCoroutine = false;
        private bool calculateRulesDiscreteCoroutine = false;
        public bool selectedTargetSignalEmmiter = false;
        private bool calculateNeighboursCoroutine = false;

        private int signalReceiverCounter = 0;

       // [HideInInspector]
        public static bool calculatePixelPermanenceClusterAgent = false;

        // NON STATIC PUBLIC VARIABLES

        GameObject agentPopulation; // create variable to store game object that holds script top reference
        private AgentPopulation AP; // store script to reference in variable

        GameObject generationManager;
        private GenerationManager GM;


        GameObject pixelPopulation; /// create variable to store game object that holds script top reference
        private PixelPopulation pixelPop; /// store script to reference in variable

        GameObject placeHolderPopulation; /// create variable to store game object that holds script top reference
        private PlaceHolderPopulation placeHolderPop; /// store script to reference in variable

        float agentRadius;


      
        void Start()
        {


            base.IMass = 1;
            base.IMaxForce = 0.03f;
            base.IMaxSpeed = 0.03f;
            base.VisionRadius = (this.gameObject.GetComponent<SphereCollider>().radius * 2) * 1.9f;

           agentRadius = this.gameObject.GetComponent<SphereCollider>().radius;
            base.energyDecreasingFactor = 0.00002f;
            base.energyLevel = (1.333f * Mathf.PI) * Mathf.Pow(agentRadius, 3);


            base.energyCapacity = (1.333f * Mathf.PI) * Mathf.Pow(agentRadius, 3);

           // if (base.heliumLevel < 0) base.heliumLevel = 0;

          




            ///////////////////////////////VARIABLE INITIALIZATION////////////////////////////////

            // NON STATIC PUBLIC VARIABLE INITIALIZATION

            agentPopulation = GameObject.Find("AgentPopulation"); // create variable to store game object that holds script top reference
            AP = agentPopulation.GetComponent<AgentPopulation>(); // store script to reference in variable

            generationManager = GameObject.Find("GenerationManager");
            GM = generationManager.GetComponent<GenerationManager>();

            pixelPopulation = GameObject.Find("PixelPopulation");
            pixelPop = pixelPopulation.GetComponent<PixelPopulation>();

            placeHolderPopulation = GameObject.Find("PlaceHolderPopulation"); /// create variable to store game object that holds script top reference
            placeHolderPop = placeHolderPopulation.GetComponent<PlaceHolderPopulation>(); /// store script to reference in variable

            ///////////////////////////////METHOD INITIALIZATION////////////////////////////////



        } // END START


        void Update()
        {
            if(GenerationManager.generationCount == 0)
            {
                CalculateHeliumLevelsOnGeneration_0();
                base.neighbours = base.neighboursTemp.Distinct().ToList();
                base.neighbourHistory = base.neighbours;

            }

            if (GenerationManager.generationCount != 0)
            {




                if (base.positionHistory.Count > 200)
                {
                    base.positionHistory.RemoveRange(0, base.positionHistory.Count - 200);
                }



          



                //make sure agents stays on plane if its 2D, for some reason with repell action it goes to a 3d position sometimes
                if (AP.is2D)
                {
                    if (this.gameObject.transform.position.y > 0 || this.gameObject.transform.position.y < 0)
                    {
                        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z);
                    }
                }


                base.neighbours = base.neighboursTemp.Distinct().ToList();

                IRulesDiscrete();/// conditions of execution are held within the method
                IRulesContinuous();/// conditions of execution are held within the method

                IAgentStatesBehaviorConditions();

                base.CalculateEmmiterHistory();
                base.CalculateReceiverHistory();
                base.CalculateDeActivatedHistory();
                base.CalculateFreezedHistory();






                if (GenerationManager.generationChange)
                {
                    IResetProperties();
                }




                if (this.gameObject.tag == "SignalReceiverPassive" || this.gameObject.tag == "SignalReceiver" || this.gameObject.tag == "Wanderer") base.IRunPhysics();

            }


        } //END UPDATE

        ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////  ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////










            /////////////////////////////AGENT STATES BEHAVIOR CONDITIONS //////////////////////////////////////////

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

            AgentWandererBehaviorConditions();
        }

        public void AgentWandererBehaviorConditions()
        {
           // if (AgentPopulation.migrateCluster)
           // {
                if (this.gameObject.tag == "Wanderer")
                {

                int internalTreshhold = 800;

                countToSearchForEmptyPixel++;
                    base.neighboursTemp.Clear();
                    Pixel currentPixel;
                    PixelDataLookUp3D(pixelPop, out currentPixel);
                    base.currentPixel = currentPixel.PixelName;

                    if (currentPixel.CountMobileAgents > 1 ) Wander();

                    if (currentPixel.CountMobileAgents <2 && countToSearchForEmptyPixel> internalTreshhold) this.gameObject.tag = "De-ActivatedPassive";

                    Bounds();

                    List<Agent> neighbours = base.FindNeighbours(this.gameObject.GetComponent<Agent>(), 3.0f,AgentPopulation.populationList);

                    base.Separation(neighbours, 0.2f, 1.5f);


                }
           // }
        }


        private void Bounds()
        {
            float maxX = 40;
            float maxY = 40;
            float maxZ = 40;



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


        public void Wander()
        {
            ///The displacement force  has its origin at the circle's center, and is constrained by the circle radius. This is an imaginary circle placed at the tip of the agents Vel vector
            ///The greater the radius and the distance from character to the circle, the stronger the "push" the character will receive every game frame.
            /// This displacement force will be used to interfere with the character's route. It is used to calculate the wander force.
            /// 


            float WanderAngle = 10f;
            Vector3 circleCenter;

            circleCenter = velocity;
            circleCenter.Normalize();
            circleCenter *= (agentRadius*2f);

            WanderAngle += Random.Range(0f, 50f);

            Vector3 displacement = Vector3.right;
            displacement *= agentRadius*1.5f;

            float len = displacement.magnitude;
            float sin = Mathf.Sin(WanderAngle) * len;
            float cos = Mathf.Cos(WanderAngle) * len;

            float tx = displacement.x;
            float tz = displacement.z;
            displacement.x = (cos * tx) + (sin * tz);
            displacement.z = (cos * tz) - (sin * tx);

           
            Vector3 wanderforce = circleCenter + displacement;

            base.IApplyForces(wanderforce);


        }


        public void CalculateHeliumLevelsOnGeneration_0()
        {
           

            if (this.gameObject.tag == "Freezed") base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);
            if (this.gameObject.tag == "SignalEmmiter") base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);
            if (this.gameObject.tag == "SignalReceiver") base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);
            if (this.gameObject.tag == "SignalReceiverPassive") base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);
        }



        public void DeactivatedAgentBehaviorConditions()
        {
            if (this.gameObject.tag == "De-Activated")
            {
                base.neighboursTemp.Distinct().ToList();
                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;

                // DoubleCheckNeighbours();

                for (int i = 0; i < base.neighboursTemp.Count; i++)
                {
                    if (base.neighboursTemp[i].gameObject.tag == "SignalReceiverPassive")
                    {
                        base.neighboursTemp.RemoveAt(i);
                    }
                }

                if (GenerationManager.generationCount != 0) base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);


                if (AP.displayColorbyEnergyLevels)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                }

                if (AP.displayColorByState)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
                }


                if (GenerationManager.generationChange)
                {
                   

                  
                    currentPixel.ClusterAgentCounter();
                 
                    currentPixel.AddClusterAgentNames(this.gameObject.name);
                    base.myNameInCurrentPixel = currentPixel.ClusterHistoryrAgentNames;
                    int occurences = currentPixel.ClusterHistoryrAgentNames.NumberOfOccurences(this.gameObject.name);
                    base.timesInCurrentPixel = occurences;
                 
                 
                    
                }

               

            }


        }





        public void FreezedAgentBehaviorConditions()
        {

            if (this.gameObject.tag == "Freezed")
            {
               
                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;
                base.neighbours = this.gameObject.GetComponent<Agent>().neighbours;
                for (int i = 0; i < base.neighboursTemp.Count; i++)
                {
                    if (base.neighboursTemp[i].gameObject.tag == "SignalReceiverPassive")
                    {
                        base.neighboursTemp.RemoveAt(i);
                    }
                }





                if (GenerationManager.generationCount != 0) base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);

                if (GenerationManager.generationChange)
                {



                    currentPixel.ClusterAgentCounter();

                    currentPixel.AddClusterAgentNames(this.gameObject.name);
                    base.myNameInCurrentPixel = currentPixel.ClusterHistoryrAgentNames;
                    int occurences = currentPixel.ClusterHistoryrAgentNames.NumberOfOccurences(this.gameObject.name);
                    base.timesInCurrentPixel = occurences;



                }



            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void ISignalEmmiterBehaviorConditions()
        {
            if (this.gameObject.tag == "SignalEmmiter")
            {
                base.neighboursTemp.Distinct().ToList();
                Pixel currentPixel;
                PixelDataLookUp3D(pixelPop, out currentPixel);
                base.currentPixel = currentPixel.PixelName;

                base.neighbours = this.gameObject.GetComponent<Agent>().neighbours;
                for (int i = 0; i < base.neighboursTemp.Count; i++)
                {
                    if (base.neighboursTemp[i].gameObject.tag == "SignalReceiverPassive")
                    {
                        base.neighboursTemp.RemoveAt(i);
                    }
                }


                if (GenerationManager.generationCount != 0) base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);

                if (GenerationManager.generationChange)
                {



                    currentPixel.ClusterAgentCounter();

                    currentPixel.AddClusterAgentNames(this.gameObject.name);
                    base.myNameInCurrentPixel = currentPixel.ClusterHistoryrAgentNames;
                    int occurences = currentPixel.ClusterHistoryrAgentNames.NumberOfOccurences(this.gameObject.name);
                    base.timesInCurrentPixel = occurences;



                }
            }



        }




        public void ISignalReceiverBehaviorConditions()
        {
            if (this.gameObject.tag == "SignalReceiver")
            {
                this.gameObject.GetComponent<SphereCollider>().enabled = true;
                Pixel currentPixel;
                base.PixelDataLookUp3D(pixelPop, out currentPixel);

                currentPixel.MobileAgentCounter();
                currentPixel.AddSignalreceiverName(this.gameObject.name);
                base.currentPixel = currentPixel.PixelName;

                base.currentPixelTotalVisits = currentPixel.CountMobileAgents;

                base.targetAgent.Clear();

                base.agentsToAvoid.Clear();
                base.agentsToRepell.Clear();


                this.signalReceiverCounter++;


                if (GenerationManager.generationCount != 0) base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);



                this.gameObject.layer = 2;

                // This would cast rays only against colliders in layer 2.
                int layerMask = 1 << 2;

                // But instead we want to collide against everything except layer 2. The ~ operator does this, it inverts a bitmask.
                layerMask = ~layerMask;


                List<Vector3> missedRays;
                List<Vector3> hitRays;
                int num = 50;
                float length = 5;//Mathf.Infinity;
                bool displayRays = false;




                base.Sensors(num, length, Color.white, layerMask, out missedRays, out hitRays, displayRays);





                base.positionHistory.Add(this.gameObject.transform.position);

                base.positionHistoryTraceback = base.positionHistory.GetNLastElements(50);

                base.currentAgentXValRange = new VectorData(base.positionHistoryTraceback[0].x, base.positionHistoryTraceback[base.positionHistoryTraceback.Count - 1].x);

                float threshhold = 0.01f;


                bool similar = SharpMath.Similar(base.currentAgentXValRange.A, base.currentAgentXValRange.B, threshhold);




                base.SeekClosestNeighbour(this.gameObject.GetComponent<Agent>(), AgentPopulation.emmiterAgentList, out closestN);
                float distanceToTarget = Vector3.Distance(this.gameObject.transform.position, closestN[0].transform.position);

             



                if (!similar)
                {

                  

                    float deltaVisionRadius = distanceToTarget / VisionRadius;

                    float deltaVisionRadiusClamped = SharpMath.Clamp(deltaVisionRadius, this.gameObject.GetComponent<SphereCollider>().radius * 1.3f, base.VisionRadius);

                    List<Agent> neighbours = this.FindNeighbours2(this.gameObject.GetComponent<Agent>(), base.VisionRadius, AgentPopulation.signalReceiverAgentList);

                    List<Agent> neighbours2 = this.FindNeighbours2(this.gameObject.GetComponent<Agent>(), deltaVisionRadiusClamped, AgentPopulation.freezedAgentList);

                    List<Agent> neighbours3 = this.FindNeighbours2(this.gameObject.GetComponent<Agent>(), deltaVisionRadiusClamped, AgentPopulation.deActivatedAgentList);

                    // base.DrawRaysFromSphere(20, deltaVisionRadiusClamped, Color.grey);
                    base.Separation(neighbours, 1.2f, 3f);
                    //base.Separation(neighbours, 1.2f, base.VisionRadius);
                    base.Repell(neighbours2, 0.02f);
                    base.Repell(neighbours3, 0.02f);

                    //base.Repell(neighbours, 0.2f);

                }




                if (similar)
                {

                    //if (distanceToTarget <= 2)
                    //{
                    if (closestN != null)
                    {
                         base.SeekWithRayCast(this.gameObject.GetComponent<Agent>(), closestN[0]);


                        //this.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        //Vector3 closestMissedRayToTarget = Utility.ClosestPoint(this.gameObject.GetComponent<ReOrganizationBehavior>().closestN[0].transform.position, missedRays);
                        //Vector3 deltaPos = closestMissedRayToTarget - this.transform.position;

                        //Debug.DrawRay(this.gameObject.transform.position, deltaPos, Color.blue);
                        //deltaPos.Normalize();
                        //deltaPos *= 0.04f;

                        //this.transform.position += deltaPos;

                        //}

                        //if (distanceToTarget > 2)
                        //{
                        //if (missedRays.Count != 0)
                        //{
                          

                        //    Vector3 deltaPos = missedRays.PickRandomElement(); //- this.gameObject.transform.position;
                        //    Debug.DrawRay(this.gameObject.transform.position, deltaPos, Color.blue);
                        //    deltaPos.Normalize();
                        //    deltaPos *= 0.2f;

                        //    this.transform.position += deltaPos;
                        //}
                        //}
                    }



                }






            }



        }

        /// <summary>
        /// 
        /// </summary>
        public void ISignalReceiverPassiveBehaviorConditions()
        {

            List<Agent> availableEmmiterList = new List<Agent>();

            if (this.gameObject.tag == "SignalReceiverPassive")

            {
                this.counterAgentsToRepell += (int)Time.time;
                this.counterAgentsToAvoid += (int)Time.time;
                base.signalReceiverPassiveCounter++;
                this.gameObject.GetComponent<SphereCollider>().enabled = false;
                base.neighboursTemp.Clear();
                Agent farthestNeighbour = null;


                Pixel currentPixel;
                base.PixelDataLookUp3D(pixelPop, out currentPixel);

                currentPixel.MobileAgentCounter();
                currentPixel.AddSignalreceiverName(this.gameObject.name);
                base.currentPixel = currentPixel.PixelName;

                base.currentPixelTotalVisits = currentPixel.CountMobileAgents;




                if (GenerationManager.generationCount!=0) base.CalculateEnergyLevels(placeHolderPop.gridDistribution_6Neighbours, placeHolderPop._12Neighbours, AP.is3D, AP.is2D);


                if (this.searchAgentsToRepellCoroutine == false && this.counterAgentsToRepell > 30 && AgentPopulation.emmiterAgentList.Count > 0)
                {



                    farthestNeighbour = Utility.FarthestObject(this.gameObject.GetComponent<Agent>(), AgentPopulation.emmiterAgentList); //availableEmmiters


                    for (int i = 0; i < AgentPopulation.signalReceiverPassiveAgentList.Count; i++)
                    {
                        if (AgentPopulation.signalReceiverPassiveAgentList[i].transform.position != this.gameObject.transform.position)
                        {
                            base.agentsToRepell.Add(AgentPopulation.signalReceiverPassiveAgentList[i]);
                        }
                    }




                    base.targetAgent.Add(farthestNeighbour);
                    //farthestNeighbour.gameObject.GetComponent<ReOrganizationBehavior>().selectedTargetSignalEmmiter = true;
                    farthestNeighbour.GetComponent<Agent>().gameObject.layer = 2;
                    /// adds istelf so that the Agent Emmiter knows which agent will come to it.
                    //farthestNeighbour.gameObject.GetComponent<ReOrganizationBehavior>().targetAgentEmmitersPerspective.Add(this.gameObject);




                    //   }// END THIRD IF STATEMENT 



                    this.counterAgentsToRepell = 0; /// only start search after n seconds. this ensures that a search is performed, because emmiters and receivers appear at the same time.
                    this.searchAgentsToRepellCoroutine = true;



                } // END SECOND IF STATEMENT 






                if (this.searchAgentsToAvoidCoroutine == false && this.counterAgentsToAvoid > 30 && base.targetAgent.Count > 0)
                {
                    for (int i = 0; i < AgentPopulation.populationList.Count; i++)
                    {
                        /// add all objects except target agent and itself     /// base.targetAgent[0].transform.position 
                        if (AgentPopulation.populationList[i].transform.position != base.targetAgent[0].transform.position && AgentPopulation.populationList[i].transform.position != this.gameObject.transform.position)
                        {
                            base.agentsToAvoid.Add(AgentPopulation.populationList[i]);
                        }
                    }

                    //   if (base.agentsToAvoid.Count > 0 && base.agentsToRepell.Count > 0)
                    //  {
                    //for (int i = base.agentsToAvoid.Count - 1; i >= 0; i--)
                    //{
                    //    for (int j = base.agentsToRepell.Count - 1; j >= 0; j--)
                    //    {
                    //        if (base.agentsToAvoid[i].transform.position == base.agentsToRepell[j].transform.position)
                    //        {
                    //            base.agentsToAvoid.RemoveAt(i);
                    //        }

                    //    }

                    //}
                    //   }
                    this.counterAgentsToAvoid = 0;
                    this.searchAgentsToAvoidCoroutine = true;

                }// END OUTER IF STATEMENT 







                if (base.targetAgent.Count > 0)
                {

                    base.Separation(agentsToAvoid);

                    base.Repell(agentsToRepell, 0.02f);


                    Wander();



                }





            } /// END CONDITIONS SIGNAL RECEIVER PASSIVE

        }


        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> returns list of Signal Emmiters that have not been selected
        private List<Agent> NotSelectedSignalEmmiters()
        {
            List<Agent> emmiterListNotSelected = new List<Agent>();
            for (int i = 0; i < AgentPopulation.emmiterAgentList.Count; i++)
            {
                if (AgentPopulation.emmiterAgentList[i].gameObject.GetComponent<ReOrganizationBehavior>().selectedTargetSignalEmmiter == false)
                {
                    emmiterListNotSelected.Add(AgentPopulation.emmiterAgentList[i]);
                }
            }

            return emmiterListNotSelected;
        }


        private List<Agent> FindNeighbours2(Agent agent, float visionRadius, List<Agent> dataToSearch)
        {
            List<Agent> neighbours = new List<Agent>();


            foreach (Agent neighbour in dataToSearch)
            {

                if (neighbour != agent && neighbour != closestN[0] && Vector3.Distance(neighbour.transform.position, agent.transform.position) < visionRadius)
                {

                    neighbours.Add(neighbour);
                }

            }


            return neighbours;

        }



        /////////////////////////////AGENT STATES BEHAVIOR CONDITIONS END //////////////////////////////////////////

    


 

        #region RESET PROPERTIES

        /// <summary>
        /// 
        /// </summary>
        public void IResetProperties()
        {
            base.agentsToAvoid.Clear();
            base.agentsToRepell.Clear();
            closestN.Clear();
            this.signalReceiverCounter = 0;
            //base.neighbours.Clear();
           // base.neighboursTemp.Clear();
            base.targetAgent.Clear();
         
            base.positionHistory.Clear();

            base.positionHistoryTraceback.Clear();

            base.localCollisionHistory = 0;


            this.gameObject.tag = "De-Activated";
            //this.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;

            this.searchAgentsToRepellCoroutine = false;
            this.searchAgentsToAvoidCoroutine = false;

            this.calculateRulesDiscreteCoroutine = false;
            this.changeStateToSignalReceiverCoroutine = false;
            this.selectedTargetSignalEmmiter = false;
            this.calculateNeighboursCoroutine = false;


            this.counterAgentsToRepell = 0;
            this.counterAgentsToAvoid = 0;
            this.signalReceiverPassiveCounter = 0;
            base.signalReceiverPassiveCounter = 0;

            base.mySignalReceivers.Clear();
            base.activatedPlaceHolders.Clear();
            base.placeHolderLocalList.Clear();


        }

        #endregion

        ////////////////////////////////PLACE HOLDER FUNCTIONALITY////////////////////////////////////////////////

        ////////////////////////////////PLACE HOLDER FUNCTIONALITY END////////////////////////////////////////////////


        private void OnDrawGizmos()
        {
            /// draw vectors to target
            /// 
            if (GenerationManager.generationCount != 0)
            {

                //if (targetAgent.Count > 0 && this.gameObject.tag == "SignalReceiverPassive" || this.gameObject.tag == "SignalReceiver" && closestN != null)//&& targetAgent.Count > 0)
                //{
                //    Gizmos.color = Color.red;

                //    //Vector3 dir = targetAgent[0].transform.position - this.gameObject.transform.position;

                //    Vector3 dir = closestN.transform.position - this.gameObject.transform.position;

                //    //dir.Normalize();
                //    //dir *= 1.2f;
                //    Gizmos.DrawRay(this.gameObject.transform.position, dir);
                //}


                if (this.gameObject.tag == "Freezed" || this.gameObject.tag == "SignalEmmiter" || this.gameObject.tag == "De-Activated")
                {
                    for (int i = 0; i < base.neighbours.Count; i++)
                    {
                        Gizmos.color = Color.white;
                        Vector3 dir = base.neighbours[i].transform.position - this.gameObject.transform.position;
                        Gizmos.DrawRay(this.gameObject.transform.position, dir);

                    }
                }


            }


        }


        #region COLLIDERS
        private void OnTriggerEnter(Collider other)
        {
            /// keeps track of neighbours in all other generations
            if (GenerationManager.generationCount != 0)
            {
                if (other.gameObject.tag == "SignalEmmiter")
                {
                    base.neighboursTemp.Add(other.gameObject.GetComponent<Agent>());
                    base.neighbourHistory.Add(other.gameObject.GetComponent<Agent>());
                    base.localCollisionHistory++;
                }

                if (other.gameObject.tag == "SignalReceiver" && this.gameObject.tag!="De-Activated" && this.gameObject.tag!="Freezed")
                {
                    base.neighboursTemp.Add(other.gameObject.GetComponent<Agent>());
                    base.neighbourHistory.Add(other.gameObject.GetComponent<Agent>());

                    base.localCollisionHistory++;

                }


            }


            ///Keeps track of neighbours of generation 0
            if (GenerationManager.generationCount == 0)
            {
                if (other.gameObject.tag != "De-Activated" && other.gameObject.tag != "Voxel" && other.gameObject.tag != "ActivatedPlaceHolder" && other.gameObject.tag != "CollidedPlaceHolder")
                {
                    /// Increment the collision history of the Agent. during first collision, SystemCollisionHistory = 1
                    base.neighboursTemp.Add(other.gameObject.GetComponent<Agent>());
                    //base.neighbourHistory.Add(other.gameObject.GetComponent<Agent>());
                    base.localCollisionHistory++;

                }



            }


        }




   



        #endregion


    }/////////////// END CLASS

}

