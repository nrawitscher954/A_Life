using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ClassExtensions;
using SharpMatter.SharpDataStructures;
using SharpMatter.SharpMath;
using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;



////////////////////////////////CLASS DESCRIPTION////////////////////////////////
/// <summary>
/// 
/// THIS CLASS CONTAINS ALL THE BASE FUNCTIONS AND PROPERTIES OF THE AGENT  BASE FUNCTIONALITY. IT WAS DEVELOPED SO BEHAVIOR CLASSES CAN INHERIT FROM 
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
/// 
/// 
/// 
/// 
/// 
/// 
/// 
/// 
/// 
/// </summary>
/// 




namespace OrganizationalModel.BaseClass
{
    public class Agent : MonoBehaviour, IPhysicsBehavior<float, Vector3>
    {

        [HideInInspector]
        public float IMaxForce { get; set; }
        [HideInInspector]
        public float IMass { get; set; }
        [HideInInspector]
        public float IMaxSpeed { get; set; }
        [HideInInspector]
        public float VisionRadius { get; set; }

        [HideInInspector]
        public int communicationType; // FOR DEBUGGING PURPOSES!!!

        [HideInInspector]
        public Vector3 velocity;
        [HideInInspector]
        public Vector3 acceleration;

        [HideInInspector]
        public string agentName;






        [HideInInspector]
        public int signalReceiverPassiveCounter;

        [HideInInspector]
        public List<Agent> neighboursTemp;
        public List<Agent> neighbours;
        [HideInInspector]
        public List<Agent> targetAgent;

        public List<Agent> targetForSignalReceiver;



        [HideInInspector]
        public List<Agent> mySignalReceiversTemp;

        public List<Agent> mySignalReceivers;


        [HideInInspector]
        public List<Agent> agentsToRepell;
        [HideInInspector]
        public List<Agent> agentsToAvoid;
        [HideInInspector]
        public List<Agent> closestN;




        #region CALCULATE STATE HISTORY COROUTINES

        [HideInInspector]
        public bool calculateEmmiterHistoryCoroutine;
        [HideInInspector]
        public bool calculateReceiverHistoryCoroutine;
        [HideInInspector]
        public bool calculateDeActivatedHistoryCoroutine;
        [HideInInspector]
        public bool calculateFreezedHistoryCoroutine;
        [HideInInspector]
         public bool calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine;

        #endregion


        #region ENERGY DATA



        [HideInInspector]
        public float energyDecreasingFactor;


        public float energyLevel;
      

        public float energyCapacity;
         
        #endregion


        #region PIXEL DATA


      

        // [HideInInspector]
        public string currentPixel;

        [HideInInspector]
        public VectorData currentAgentXValRange;

        [HideInInspector]
       public int totalGenerationsBeingStatic;

       public int timesInCurrentPixel; /// BY AGENT NAME
        public int currentPixelTotalVisits;
        [HideInInspector]
        public List<string> myNameInCurrentPixel;

        #endregion




        #region PLACE HOLDERS VARIABLES

        //[HideInInspector]
        public List<PlaceHolder> activatedPlaceHolders;


        public int selectedActivatedPlaceHolders;
        public int totalActivatedPlaceHolders; // FOR DEBUGGING PURPOSES!!!

        //[HideInInspector]
        public List<PlaceHolder> placeHolderLocalList;

        public PlaceHolder placeHolderTargetForSignalReceiver;



        #endregion




        #region SIMULATION HISTORY DATA

        //[HideInInspector]
        public int localCollisionHistory;
        [HideInInspector]
        public int emmiterHistoryCount;
        [HideInInspector]
        public int receiverHistoryCount;
        [HideInInspector]
        public int freezedHistoryCount;
        [HideInInspector]
        public int deActivatedHistoryCount;
        [HideInInspector]
        public Agent mostCommonAgentInConnectionHistory;

        //[HideInInspector]
        public List<Agent> neighbourHistory;

        [HideInInspector]
        public List<Vector3> positionHistory;

        [HideInInspector]
        public List<Vector3> DupPositionHistory;
        [HideInInspector]
        public List<Vector3> positionHistoryTraceback;

        #endregion







        //float startTime;

        //private void Awake()
        //{
        //    startTime = Time.time;
        //}

        //CONSTRUCTOR
        public Agent()
        {


            // NON STATIC PUBLIC VARIABLE INITIALIZATION

            

            this.velocity = new Vector3(0, 0, 0);

            this.acceleration = new Vector3(0, 0, 0);
            this.neighboursTemp = new List<Agent>();
            this.neighbours = new List<Agent>();
            this.targetAgent = new List<Agent>();

            this.agentsToRepell = new List<Agent>();
            this.agentsToAvoid = new List<Agent>();
            this.neighbourHistory = new List<Agent>();
            this.targetForSignalReceiver = new List<Agent>();
            this.closestN = new List<Agent>();
            this.mySignalReceiversTemp = new List<Agent>();
            this.mySignalReceivers = new List<Agent>();
            this.positionHistory = new List<Vector3>();

            this.placeHolderLocalList = new List<PlaceHolder>();
            this.placeHolderTargetForSignalReceiver = null;

            this.myNameInCurrentPixel = new List<string>();

           

            this.DupPositionHistory = new List<Vector3>();
            this.positionHistoryTraceback = new List<Vector3>();


      



            this.localCollisionHistory = 0;

            this.emmiterHistoryCount = 0;
            this.receiverHistoryCount = 0;
            this.freezedHistoryCount = 0;
            this.deActivatedHistoryCount = 0;

            this.totalGenerationsBeingStatic = 0;

            this.timesInCurrentPixel = 0;
            this.currentPixelTotalVisits = 0;

            this.calculateEmmiterHistoryCoroutine = false;
            this.calculateReceiverHistoryCoroutine = false;
            this.calculateDeActivatedHistoryCoroutine = false;
            this.calculateFreezedHistoryCoroutine = false;
            this.calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine = false;
          


            this.mostCommonAgentInConnectionHistory = null;


        }



       

        public Color EnergyColor(Color startColor, Color endColor, float heliumLevel, float startTime)
        {
            
            return  Color.Lerp(startColor, endColor, heliumLevel);
         
        }


        #region ENERGY LEVEL RULES BASED ON NEIGHBOUR CODNITIONS


        public void EnergyLevelSharing(Agent agentToSearchFrom, float energySharingThreshold)
        {
            float currentEnergyLevel = agentToSearchFrom.energyLevel;
            List<float> neighbourEnergyList = new List<float>();

            float totalNeighbourEnergyLevels = neighbourEnergyList.Sum();

            float neighbourEnergyAverege = totalNeighbourEnergyLevels / neighbours.Count;

            bool shareEnergy = false;

            for (int i = 0; i < neighbours.Count; i++)
            {
                neighbourEnergyList.Add(neighbours[i].energyLevel);
            }

            if (currentEnergyLevel > neighbourEnergyAverege) shareEnergy = true;
        }

        public void CalculateEnergyLevels( bool gridDistribution_6Neighbours, bool _12Neighbours, bool is3D, bool is2D )
        {
            if (this.energyLevel < 0) this.energyLevel = 0;


            if (gridDistribution_6Neighbours)
            {
                int maxNeighbours;
                if (is2D)
                {
                    maxNeighbours = 4;

                    if (neighbours.Count == maxNeighbours)
                    {
                        energyLevel -= 0;
                    }

                    else if (neighbours.Count == 3)
                    {
                        energyLevel -= energyDecreasingFactor / 3;
                    }

                    else if (neighbours.Count == 2)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 1)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 0)
                    {
                        energyLevel -= energyDecreasingFactor / 1;
                    }




                }

                if (is3D)
                {
                    maxNeighbours = 6;

                    if (neighbours.Count == maxNeighbours)
                    {
                        energyLevel -= 0;
                    }

                    else if (neighbours.Count == 5)
                    {
                        energyLevel -= energyDecreasingFactor / 5;
                    }

                    else if (neighbours.Count == 4)
                    {
                        energyLevel -= energyDecreasingFactor / 4;
                    }

                    else if (neighbours.Count == 3)
                    {
                        energyLevel -= energyDecreasingFactor / 3;
                    }

                    else if (neighbours.Count == 2)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 1)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 0)
                    {
                        energyLevel -= energyDecreasingFactor / 1;
                    }

                }



            }



            else if (_12Neighbours)
            {



                int maxNeighbours;

                if (is2D)
                {

                    maxNeighbours = 6;

                    if (neighbours.Count == maxNeighbours)
                    {
                        energyLevel -= 0;
                    }

                    else if (neighbours.Count == 5)
                    {
                        energyLevel -= energyDecreasingFactor / 5;
                    }

                    else if (neighbours.Count == 4)
                    {
                        energyLevel -= energyDecreasingFactor / 4;
                    }

                    else if (neighbours.Count == 3)
                    {
                        energyLevel -= energyDecreasingFactor / 3;
                    }

                    else if (neighbours.Count == 2)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 1)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 0)
                    {
                        energyLevel -= energyDecreasingFactor / 1;
                    }





                }

                if (is3D)
                {
                    maxNeighbours = 12;

                    if (neighbours.Count == maxNeighbours)
                    {
                        energyLevel -= 0;
                    }

                    else if (neighbours.Count == 11)
                    {
                        energyLevel -= energyDecreasingFactor / 11;
                    }

                    else if (neighbours.Count == 10)
                    {
                        energyLevel -= energyDecreasingFactor / 10;
                    }

                    else if (neighbours.Count == 9)
                    {
                        energyLevel -= energyDecreasingFactor / 9;
                    }

                    else if (neighbours.Count == 8)
                    {
                        energyLevel -= energyDecreasingFactor / 8;
                    }

                    else if (neighbours.Count == 7)
                    {
                        energyLevel -= energyDecreasingFactor / 7;
                    }

                    else if (neighbours.Count == 6)
                    {
                        energyLevel -= energyDecreasingFactor / 6;
                    }

                    else if (neighbours.Count == 5)
                    {
                        energyLevel -= energyDecreasingFactor / 5;
                    }

                    else if (neighbours.Count == 4)
                    {
                        energyLevel -= energyDecreasingFactor / 4;
                    }

                    else if (neighbours.Count == 3)
                    {
                        energyLevel -= energyDecreasingFactor / 3;
                    }

                    else if (neighbours.Count == 2)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 1)
                    {
                        energyLevel -= energyDecreasingFactor / 2;
                    }

                    else if (neighbours.Count == 0)
                    {
                        energyLevel -= energyDecreasingFactor / 1;
                    }



                }







            }





        }




        #endregion







        /////////////////////////////SEARCH BEHAVIORS  //////////////////////////////////////////

        #region SEARCH BEHAVIORS
        public virtual List<Agent> FindNeighbours(Agent agent, float visionRadius, List<Agent> dataToSearch)
        {
            
            List<Agent> neighbours = new List<Agent>();

            foreach (Agent neighbour in dataToSearch)
            {
                if (neighbour != agent && Vector3.Distance(neighbour.transform.position, agent.transform.position) < visionRadius)
                {

                    neighbours.Add(neighbour);
                }

            }

            return neighbours;

        }


        

        /// <summary>
        /// Seek a target
        /// </summary>
        /// <param name="source"></param> source object
        /// <param name="target"></param> target object
        public void Seek(Agent source, Agent target)
        {

            Vector3 desiredVelToClosestNeighbour = target.transform.position - source.transform.position;
            desiredVelToClosestNeighbour.Normalize();
            desiredVelToClosestNeighbour *= this.IMaxSpeed; ; //0.015f; this.IMaxSpeed

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;


            Vector3 steerLimit = steer.Limit(this.IMaxForce);



            IApplyForces(steerLimit);

        }



        public void Seek(Agent source, PlaceHolder target)
        {

            Vector3 desiredVelToClosestNeighbour = target.transform.position - source.transform.position;
            desiredVelToClosestNeighbour.Normalize();
            desiredVelToClosestNeighbour *= this.IMaxSpeed; ; //0.015f; this.IMaxSpeed

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;


            Vector3 steerLimit = steer.Limit(this.IMaxForce);



            IApplyForces(steerLimit);

        }







        /// <summary>
        /// Seek the closest neighboir in a collection from an object to search from
        /// </summary>
        /// <param name="objectToSearchFrom"></param>
        /// <param name="targets"></param>
        public void SeekClosestNeighbour(Agent objectToSearchFrom, List<Agent> targets, out List<Agent> closestNeighbour)
        {
            List<Agent> data = new List<Agent>();
            Agent closestTarget = Utility.ClosestObject(objectToSearchFrom, targets);
            Vector3 desiredVelToClosestNeighbour = closestTarget.transform.position - objectToSearchFrom.transform.position;
            desiredVelToClosestNeighbour.Normalize();

            desiredVelToClosestNeighbour *= this.IMaxSpeed;

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;

            Vector3 steerLimit = steer.Limit(this.IMaxForce);

            IApplyForces(steerLimit);

            data.Add(closestTarget);
            //closestNeighbour = closestTarget;
            closestNeighbour = data;


        }



        public void SeekClosestNeighbour(PlaceHolder objectToSearchFrom, List<Agent> targets, out List<Agent> closestNeighbour)
        {
            List<Agent> data = new List<Agent>();
            Agent closestTarget = Utility.ClosestObject(objectToSearchFrom, targets);
            Vector3 desiredVelToClosestNeighbour = closestTarget.transform.position - objectToSearchFrom.transform.position;
            desiredVelToClosestNeighbour.Normalize();

            desiredVelToClosestNeighbour *= this.IMaxSpeed;

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;

            Vector3 steerLimit = steer.Limit(this.IMaxForce);

            IApplyForces(steerLimit);

            data.Add(closestTarget);
            //closestNeighbour = closestTarget;
            closestNeighbour = data;


        }





        public void SeekClosestPixel(Agent objectToSearchFrom, Pixel [,,] pixels )
        {
            List<Pixel> pixelPopulationList = ConvertArrayToList(pixels);
         
            Pixel closestTarget = Utility.ClosestObject(objectToSearchFrom, pixelPopulationList);
            Vector3 desiredVelToClosestNeighbour = closestTarget.transform.position - objectToSearchFrom.transform.position;
            desiredVelToClosestNeighbour.Normalize();

            desiredVelToClosestNeighbour *= this.IMaxSpeed;

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;

            Vector3 steerLimit = steer.Limit(this.IMaxForce);

            IApplyForces(steerLimit);

           

        }


        private List<Pixel>ConvertArrayToList(Pixel [,,] data)
        {
            List<Pixel> returnList = new List<Pixel>();
            for (int i = 0; i < data.GetLength(1); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    for (int k = 0; k < data.GetLength(1); k++)
                    {
                        returnList.Add(data[i, j, k]);
                    }
                }
            }
            return returnList;
        }

        public void SeekClosestNeighbour(Agent objectToSearchFrom, List<Agent> targets)
        {

            Agent closestTarget = Utility.ClosestObject(objectToSearchFrom, targets);
            Vector3 desiredVelToClosestNeighbour = closestTarget.transform.position - objectToSearchFrom.transform.position;
            desiredVelToClosestNeighbour.Normalize();

            desiredVelToClosestNeighbour *= this.IMaxSpeed;

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;

            Vector3 steerLimit = steer.Limit(this.IMaxForce);

            IApplyForces(steerLimit);




        }



        public void SeekClosestNeighbour(PlaceHolder objectToSearchFrom, List<Agent> targets)
        {

            Agent closestTarget = Utility.ClosestObject(objectToSearchFrom, targets);
            Vector3 desiredVelToClosestNeighbour = closestTarget.transform.position - objectToSearchFrom.transform.position;
            desiredVelToClosestNeighbour.Normalize();

            desiredVelToClosestNeighbour *= this.IMaxSpeed;

            Vector3 steer = desiredVelToClosestNeighbour - this.velocity;

            Vector3 steerLimit = steer.Limit(this.IMaxForce);

            IApplyForces(steerLimit);




        }


        

        #endregion

        /////////////////////////////SEARCH BEHAVIORS END //////////////////////////////////////////

        /////////////////////////////STEERING BEHAVIORS //////////////////////////////////////////


        #region STEERING BEHAVIORS

       




        public void Arrive(Agent source, PlaceHolder target, float arrivalRadius, float maxSpeed, float maxForce)
        {
            Vector3 desired = target.transform.position - source.gameObject.transform.position;
            float desiredMagnitude = desired.magnitude;
            desired.Normalize();


            if (desiredMagnitude < arrivalRadius)  /// if we are closer than n pixels, set the magnitude of the desired velocity as a function of proximity
            {


                float m = SharpMath.Remap(desiredMagnitude, 0, arrivalRadius, 0, maxSpeed);

                desired *= m;
            }
            else
            {
                desired *= maxSpeed;
            }


            Vector3 steer = desired - velocity;

            steer.Limit(maxForce);

            IApplyForces(steer);

        }



        public void Arrive(Agent source, Agent target, float arrivalRadius, float maxSpeed, float maxForce)
        {
            Vector3 desired = target.transform.position - source.gameObject.transform.position;
            float desiredMagnitude = desired.magnitude;
            desired.Normalize();


            if (desiredMagnitude < arrivalRadius)  /// if we are closer than n pixels, set the magnitude of the desired velocity as a function of proximity
            {


                float m = SharpMath.Remap(desiredMagnitude, 0, arrivalRadius, 0, maxSpeed);

                desired *= m;
            }
            else
            {
                desired *= maxSpeed;
            }


            Vector3 steer = desired - velocity;

            steer.Limit(maxForce);

            IApplyForces(steer);

        }










        public void Separation(List<Agent> neighbours)
        {



            float separationStrength = 0;
            float distanceToNeighbourThreshold = 0;
            Vector3 separation = new Vector3(0, 0, 0);



            if (GenerationManager.generationCount != 0)
            {
                separationStrength = .9f; //0.02
                distanceToNeighbourThreshold = 1.3f; //1.1

            }

            if (GenerationManager.generationCount == 0)
            {
                separationStrength = 0.2f; //0.017f;
                distanceToNeighbourThreshold = 1.4f;  //1.82f;


            }



            for (int i = 0; i < neighbours.Count; i++)
            {
                float distanceToNeighbour = Vector3.Distance(this.gameObject.transform.position, neighbours[i].transform.position);

                if (distanceToNeighbour <= distanceToNeighbourThreshold) /// sensitive parameter related to separation strength //1.82f
                {
                    Vector3 getAway = this.gameObject.transform.position - neighbours[i].gameObject.transform.position;

                    /// scale get away vector by inverse of distance to its neighbour
                    /// this makes the get away vector larger as the agent gets closer to its neighbour
                    separation += getAway / (getAway.magnitude * distanceToNeighbour);
                    separation.Normalize();

                    Vector3 separationForce = separation.Multiply(separationStrength);

                    IApplyForces(separationForce);

                }

            }

        }






        public void Separation(List<Agent> neighbours, float strength, float distanceToNeighbourThreshold)
        {




            Vector3 separation = new Vector3(0, 0, 0);
            for (int i = 0; i < neighbours.Count; i++)
            {
                float distanceToNeighbour = Vector3.Distance(this.gameObject.transform.position, neighbours[i].transform.position);

                if (distanceToNeighbour <= distanceToNeighbourThreshold) /// sensitive parameter related to separation strength //1.82f
                {
                    Vector3 getAway = this.gameObject.transform.position - neighbours[i].gameObject.transform.position;

                    /// scale get away vector by inverse of distance to its neighbour
                    /// this makes the get away vector larger as the agent gets closer to its neighbour
                    separation += getAway / (getAway.magnitude * distanceToNeighbour);
                    separation.Normalize();

                    Vector3 separationForce = separation.Multiply(strength);

                    IApplyForces(separationForce);

                }

            }

        }








        public void Repell(List<Agent> avoid)
        {

            float repellMaxForce = 0;

            if (GenerationManager.generationCount != 0)
            {
                repellMaxForce = 0.02f;

            }

            if (GenerationManager.generationCount == 0)
            {

                repellMaxForce = 0.025f;


            }


            foreach (Agent agent in avoid)
            {
                float distanceToRepeller = Vector3.Distance(this.gameObject.transform.position, agent.transform.position);

                Vector3 repell = this.gameObject.transform.position - agent.transform.position;

                // Repulstion gets stronger as the agent gets closer to the repeller
                repell /= (repell.magnitude * distanceToRepeller);

                // Repulsion strength is also proportional to the radius of the repeller circle/sphere
                // This allows the user to tweak the repulsion strength by tweaking the radius
                repell *= repellMaxForce;// 0.0115f ///PARAMETER IS SENSIBLE TO MAXsPEED¬!!!! good value for maxSpeed 0.04 and 2d
                                         //repulsion *= 30.0 * repeller.Radius;
                IApplyForces(repell);


            }
        }



        public void Repell(List<Agent> avoid, float repulsionStrength)
        {




            foreach (Agent agent in avoid)
            {
                float distanceToRepeller = Vector3.Distance(this.gameObject.transform.position, agent.transform.position);

                Vector3 repell = this.gameObject.transform.position - agent.transform.position;

                // Repulstion gets stronger as the agent gets closer to the repeller
                repell /= (repell.magnitude * distanceToRepeller);

                // Repulsion strength is also proportional to the radius of the repeller circle/sphere
                // This allows the user to tweak the repulsion strength by tweaking the radius
                repell *= repulsionStrength;// 0.0115f ///PARAMETER IS SENSIBLE TO MAXsPEED¬!!!! good value for maxSpeed 0.04 and 2d
                                            //repulsion *= 30.0 * repeller.Radius;
                IApplyForces(repell);


            }
        }




        #endregion

        /////////////////////////////STEERING BEHAVIORS END //////////////////////////////////////////




        ////////////////////////////////STATE HISTORY////////////////////////////////////////////////

        #region STATE HISTORY
        /// <summary>
        /// Calculate the times an agent changes states to Emmiter during the simulation
        /// </summary>
        public void CalculateEmmiterHistory()
        {
            if (this.gameObject.tag == "SignalEmmiter" && this.calculateEmmiterHistoryCoroutine == false)
            {
                this.emmiterHistoryCount++;
                this.calculateEmmiterHistoryCoroutine = true;

            }

            this.ResetCalculateEmmiterHistory();
        }

        /// <summary>
        /// Reset calculateEmmiterHistory variable to false
        /// </summary>
        private void ResetCalculateEmmiterHistory()
        {
            if (this.gameObject.tag != "SignalEmmiter")
            {
                this.calculateEmmiterHistoryCoroutine = false;
            }
        }

        /// <summary>
        /// Calculate the times an agent changes states to Receiver during the simulation
        /// </summary>
        public void CalculateReceiverHistory()
        {
            if (this.gameObject.tag == "SignalReceiver" && this.calculateReceiverHistoryCoroutine == false)
            {
                this.receiverHistoryCount++;
                this.calculateReceiverHistoryCoroutine = true;

            }

            this.ResetCalculateReceiverHistory();
        }

        /// <summary>
        /// Reset calculateReceiverHistory variable to false
        /// </summary>

        private void ResetCalculateReceiverHistory()
        {
            if (this.gameObject.tag != "SignalReceiver")
            {
                this.calculateReceiverHistoryCoroutine = false;
            }
        }


        /// <summary>
        /// Calculate the times an agent changes states to De-Activated during the simulation
        /// </summary>
        public void CalculateDeActivatedHistory()
        {
            if (this.gameObject.tag == "De-Activated" && this.calculateDeActivatedHistoryCoroutine == false)
            {
                this.deActivatedHistoryCount++;

                this.calculateDeActivatedHistoryCoroutine = true;

            }
            if (this.gameObject.tag == "De-Activated" && this.deActivatedHistoryCount > 0 && this.freezedHistoryCount == 0 && this.emmiterHistoryCount == 0 && this.receiverHistoryCount == 0
                && GenerationManager.generationChange == true && this.calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine == false)
            {
                this.deActivatedHistoryCount++;
                this.calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine = true;
            }
            ResetCalculateDeActivatedHistory();
            ///calculates agents that have only been De-Activated
            if (this.gameObject.tag == "De-Activated" && this.deActivatedHistoryCount > 0 && this.freezedHistoryCount == 0 && this.emmiterHistoryCount == 0 && this.receiverHistoryCount == 0
              && GenerationManager.generationChange == false && this.calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine == true)
            {

                this.calculateDeActivatedHistoryIfDoesNotChangeInGenerationCoroutine = false;
            }
        }

        /// <summary>
        /// Reset calculateDeActivatedHistory variable to false
        /// </summary>
        private void ResetCalculateDeActivatedHistory()
        {
            if (this.gameObject.tag != "De-Activated")
            {
                this.calculateDeActivatedHistoryCoroutine = false;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateFreezedHistory()
        {
            if (this.gameObject.tag == "Freezed" && this.calculateFreezedHistoryCoroutine == false)
            {
                this.freezedHistoryCount++;
                this.calculateFreezedHistoryCoroutine = true;
            }

            ResetCalculateFreezedHistory();
        }
        /// <summary>
        /// 
        /// </summary>

        private void ResetCalculateFreezedHistory()
        {
            if (this.gameObject.tag != "Freezed") this.calculateFreezedHistoryCoroutine = false;
        }


        #endregion

        ////////////////////////////////STATE HISTORY END////////////////////////////////////////////////


        ////////////////////////////////ENVIRONMENT EVALUATION////////////////////////////////////////////////  

        #region ENVIRONMENT EVALUATION  



        /// <summary>
        /// Draws rays from a shpere outward
        /// </summary>
        /// <param name="numDirections"></param>
        /// 
        public void DrawRaysFromSphere(int numDirections, float rayLength, Color color)
        {


            foreach (var direction in Utility.UnifromSphericalPointDistribution(numDirections))
            {
                Debug.DrawRay(this.gameObject.transform.position, direction.normalized * rayLength, color);



            }


        }



        public void Sensors(int numDirections, float rayLength, Color color, int layerMask, out List<Vector3> missedRays, out List<Vector3> hitRays, bool displayRays)
        {


            List<Vector3> _missedRays = new List<Vector3>();
            List<Vector3> _hitRays = new List<Vector3>();

            foreach (var direction in Utility.UnifromSphericalPointDistribution(numDirections))
            {


                RaycastHit hit;

                if (Physics.Raycast(this.gameObject.transform.position, direction, out hit, Mathf.Infinity, layerMask))
                {
                    _hitRays.Add(direction);
                    if (displayRays) Debug.DrawRay(this.gameObject.transform.position, direction * hit.distance, color);


                }


                else
                {
                    _missedRays.Add(direction);

                }


            }




            missedRays = _missedRays;
            hitRays = _hitRays;

        }





        public void DisplayClosestHitObstacle(Vector3 data, Color color)
        {
            Debug.DrawLine(this.gameObject.transform.position, data, color);
        }

        public void DisplayMissedRays(List<Vector3> missedRays, Color color)
        {
            foreach (var direction in missedRays)
            {

                Debug.DrawRay(this.gameObject.transform.position, direction, color);
            }
        }



        #endregion







        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="target"></param>
        public void SeekWithRayCast(Agent agent, PlaceHolder target)

        {
            float force = 20f;
            Vector3 desiredVelToClosestNeighbour = (target.transform.position - agent.transform.position).normalized;

            RaycastHit hit;
            float maxDistance = 10f;
            if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(agent.transform.position, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = hit.normal * force;
                }
            }

            Vector3 leftRayCast = agent.transform.position;
            Vector3 rightRayCast = agent.transform.position;

            leftRayCast.x -= 2;
            rightRayCast.x += 2;

            if (Physics.Raycast(leftRayCast, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(leftRayCast, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = hit.normal * force;
                }
            }


            if (Physics.Raycast(rightRayCast, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(rightRayCast, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = hit.normal * force;
                }
            }


            Quaternion rotation = Quaternion.LookRotation(desiredVelToClosestNeighbour);
            agent.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);


            IApplyForces(desiredVelToClosestNeighbour);


        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="target"></param>
        public void SeekWithRayCast(Agent agent, Agent target)

        {
            Vector3 desiredVelToClosestNeighbour = (target.transform.position - agent.transform.position).normalized;

            RaycastHit hit;
            float maxDistance = 1f;
            if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(agent.transform.position, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = hit.normal * IMaxSpeed;
                }
            }

            Vector3 leftRayCast = agent.transform.position;
            Vector3 rightRayCast = agent.transform.position;

            leftRayCast.x -= 2;
            rightRayCast.x += 2;

            if (Physics.Raycast(leftRayCast, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(leftRayCast, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = hit.normal * IMaxSpeed;
                }
            }


            if (Physics.Raycast(rightRayCast, agent.transform.forward, out hit, maxDistance))
            {
                if (hit.transform != agent.transform)
                {
                    Debug.DrawLine(rightRayCast, hit.point, Color.blue);
                    desiredVelToClosestNeighbour = hit.normal * IMaxSpeed;
                }
            }


            Quaternion rotation = Quaternion.LookRotation(desiredVelToClosestNeighbour);
            agent.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);


            IApplyForces(desiredVelToClosestNeighbour);


        }







        ////////////////////////////////ENVIRONMENT EVALUATION END////////////////////////////////////////////

        #region PLACE HOLDERS







        /// <summary>
        /// The corresponding agent will become the parent of its place holders
        /// </summary>
        /// 
        public void SetPlaceHolderParent()
        {
            for (int i = 0; i < AgentPopulation.populationList.Count; i++)
            {
                if (AgentPopulation.populationList[i].name == "Agent" + " " + i)
                {

                    for (int j = 0; j < placeHolderLocalList.Count; j++)
                    {
                        placeHolderLocalList[j].transform.parent = this.gameObject.transform;

                    }

                }
            }

        }

        /// <summary>
        /// Set the place holders name according to each agent
        /// </summary>

        public void SetPlaceHolderName()
        {
            for (int i = 0; i < AgentPopulation.populationList.Count; i++)
            {
                for (int j = 0; j < placeHolderLocalList.Count; j++)
                {

                    if (this.gameObject.name == "Agent" + " " + i)
                    {
                        placeHolderLocalList[j].PlaceHoldername = "Place Holder" + " " + i + "." + j;
                        placeHolderLocalList[j].name = "Place Holder" + " " + i + "." + j;
                    }

                }

            }
        }




        /// <summary>
        /// Set initial state of Place holders ; i.e tag
        /// </summary>
        public void InitializeState(List<PlaceHolder> placeHolders)
        {

            for (int i = 0; i < placeHolders.Count; i++)
            {
                placeHolders[i].tag = "ActivatedPlaceHolder";

            }


        }


        #endregion


        #region PIXEL DATA



        /// <summary>
        /// Looks up current Pixel that corresponds to the agents position. This method already has internalized
        /// conditions for all agent states
        /// </summary>
        /// <param name="pixel"></param> 
        /// <param name="currentPixel"></param> Pixel object output with query data
        public void PixelDataLookUp3D(PixelPopulation pixel, out Pixel currentPixel)
        {


            Pixel _currentPixel = pixel.Lookup3D(this.gameObject.transform.position);


            //if (this.gameObject.tag == "SignalReceiver")
            //{
            //    _currentPixel.MobileAgentCounter();
            //    _currentPixel.AddSignalreceiverName(this.gameObject.name);
            //    this.currentPixel = _currentPixel.PixelName;
            //}

            ///// this.calculatePixelPermanenceClusterAgent might need condition
            //if (this.gameObject.tag == "Freezed" || this.gameObject.tag == "SignalEmmiter" || this.gameObject.tag == "Deactivated" && this.calculatePixelPermanenceClusterAgent == false &&
            //    GenerationManager.generationChange)
            //{
            //    print("test");
            //    _currentPixel.ClusterAgentCounter();
            //    _currentPixel.AddClusterAgentNames(this.gameObject.name);
            //    this.currentPixel = _currentPixel.PixelName;

            //    this.calculatePixelPermanenceClusterAgent = true;
            //}




            currentPixel = _currentPixel;

        }



        #endregion
        /////////////////////////////PHYSICS //////////////////////////////////////////

        public void IRunPhysics()
        {

            this.velocity += this.acceleration;
            this.velocity.Normalize();
            this.velocity *= this.IMaxSpeed;
            this.gameObject.transform.position += this.velocity;



            this.IResetForces();
        }

        public void IResetForces()
        {
            this.acceleration *= 0.0f;
        }

        public void IApplyForces(Vector3 force)
        {

            this.acceleration += force / this.IMass;

        }

        /////////////////////////////PHYSICS END //////////////////////////////////////////
    }

}