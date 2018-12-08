using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SharpMatter.SharpMath;
using SharpMatter.SharpDataStructures;
using ClassExtensions;
using System.Linq;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;

namespace OrganizationalModel.Behaviors
{
    public partial class OrganizationBehavior : Agent, IOrganization<Agent>
    {


        ////////////////////////////////CONTROL LOG / NOTES ////////////////////////////////
        /// <summary>

        /// 
        /// 
        /// BUG: When 12 neighbour connection is used sometimes some singal emmiters dont activate any signal receivers
        /// 
        /// 
        /// 
        /// 
        /// </summary>




        ////////////////////////////////DECLARE GLOBAL VARIABLES////////////////////////////////



        //CLASS INSTANCES

        //PUBLIC LISTS


        //PUBLIC STATIC LISTS

        //PRIVATE LISTS




        // PUBLIC VARIABLES


        
        private float distanceToTarget;

        //PRIVATE VARIABLES
        private int signalReceiverPassiveThreshhold = 100;
        public int signalEmitterCounter = 0;
        private bool searchClosestNeighbour = false;
        private bool createPlaceHolders = false;

        
        public bool getActivePlaceHolders = false;

        public  int time = 0;

        public Agent mySignalEmmiter = null;


        // NON STATIC PUBLIC VARIABLES

        GameObject agentPopulation; // create variable to store game object that holds script top reference
        private AgentPopulation AP; // store script to reference in variable

        GameObject placeHolderPopulation; /// create variable to store game object that holds script top reference
        private PlaceHolderPopulation placeHolderPop; /// store script to reference in variable

        // NON STATIC PUBLIC VARIABLES
        GameObject pixelPopulation; /// create variable to store game object that holds script top reference
        private PixelPopulation pixelPop; /// store script to reference in variable


        GameObject generationManager;
        private GenerationManager GM;

        public bool selectedDeActivatedAgent = false;
        float startTime;
        void Start()
        {
            startTime = Time.time;
        
            ///////////////////////////////VARIABLE INITIALIZATION////////////////////////////////

            // NON STATIC PUBLIC VARIABLE INITIALIZATION


            agentPopulation = GameObject.Find("AgentPopulation"); // create variable to store game object that holds script top reference
            AP = agentPopulation.GetComponent<AgentPopulation>(); // store script to reference in variable

            //  NON STATIC PUBLIC VARIABLE INITIALIZATION
            placeHolderPopulation = GameObject.Find("PlaceHolderPopulation"); /// create variable to store game object that holds script top reference
            placeHolderPop = placeHolderPopulation.GetComponent<PlaceHolderPopulation>(); /// store script to reference in variable

            pixelPopulation = GameObject.Find("PixelPopulation");
            pixelPop = pixelPopulation.GetComponent<PixelPopulation>();

            generationManager = GameObject.Find("GenerationManager");
            GM = generationManager.GetComponent<GenerationManager>();

            ///////////////////////////////METHOD INITIALIZATION////////////////////////////////




            base.IMass = 2;
            base.IMaxForce = 0.03f;
            base.IMaxSpeed = 0.03f;
            base.VisionRadius = (this.gameObject.GetComponent<SphereCollider>().radius * 2) * 0.9f;

            float agentRadius = this.gameObject.GetComponent<SphereCollider>().radius;
            base.energyDecreasingFactor = 0.00002f;
            base.energyLevel = (1.333f * Mathf.PI) * Mathf.Pow(agentRadius, 3);
            

            base.energyCapacity = (1.333f * Mathf.PI) * Mathf.Pow(agentRadius, 3);

           // if (base.heliumLevel < 0) base.heliumLevel = 0;









        } // END START


        void Update()
        {
            if (GenerationManager.generationCount == 0)
            {

                


                //make sure agents stays on plane if its 2D, for some reason with repell action it goes to a 3d position sometimes
                if (AP.is2D)
                {
                    if (this.gameObject.transform.position.y > 0 || this.gameObject.transform.position.y < 0)
                    {
                        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z);
                    }
                }


                RulesContinous();
                IAgentBehaviorConditions();

                base.neighbours = base.neighboursTemp.Distinct().ToList();
           

                //DrawTopology();

                for (int i = 0; i < AgentPopulation.signalReceiverPassiveAgentList.Count; i++)
                {
                    if (AgentPopulation.signalReceiverPassiveAgentList[i].GetComponent<OrganizationBehavior>().signalReceiverPassiveCounter > signalReceiverPassiveThreshhold)
                    {
                        IChangeStateToSignalEmmiter(AgentPopulation.signalReceiverPassiveAgentList[i]);
                    }
                }


                if (gameObject.tag == "SignalReceiver") base.IRunPhysics();

            }




        } //END UPDATE

        ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////  ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////


        /////////////////////////////AGENT STATES BEHAVIOR CONDITIONS //////////////////////////////////////////





        #region SEARCH    






        public List<Agent> AvoidNonTargetEmmiters(Agent agent, float visionRadius, List<Agent> dataToSearch)
        {
            string targetAgentName = null;
            if (agent.GetComponent<OrganizationBehavior>().placeHolderLocalList.Count != 0)
            {
                string[] separators = { ".", " " };
                string value = agent.GetComponent<OrganizationBehavior>().placeHolderLocalList[0].PlaceHoldername;
                string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                targetAgentName = "Agent" + " " + words[2];
            }

            List<Agent> neighbours = new List<Agent>();
            if (targetAgentName != null)
            {


                foreach (Agent neighbour in dataToSearch)
                {
                    if (neighbour.name != targetAgentName && Vector3.Distance(neighbour.transform.position, agent.transform.position) < visionRadius)
                    {

                        neighbours.Add(neighbour);
                    }

                }
            }

            return neighbours;

        }





        private void SelectDeActivatedAgent(Agent agent)
        {

            agent.gameObject.GetComponent<OrganizationBehavior>().selectedDeActivatedAgent = true;

        }






        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> returns list of De Activated agents that have not been selected. that have not been selected
        private List<Agent> NotSelectedDeActivatedAgent()
        {
            List<Agent> notSelectedDeActivatedAgent = new List<Agent>();
            for (int i = 0; i < AgentPopulation.deActivatedAgentList.Count; i++)
            {
                if (AgentPopulation.deActivatedAgentList[i].gameObject.GetComponent<OrganizationBehavior>().selectedDeActivatedAgent == false)
                {
                    notSelectedDeActivatedAgent.Add(AgentPopulation.deActivatedAgentList[i]);
                }
            }

            return notSelectedDeActivatedAgent;
        }




        #endregion





        /////////////////////////////STATES //////////////////////////////////////////

        #region STATES

        /// <summary>
        /// 
        /// </summary>

        public void RulesContinous()
        {
            ChangeStateToSignalEmmiter();
            ChangeStateToFreezedBasedOnNeighbourCount();
        }

        public void ChangeStateToSelectedDeactivatedAgent(List<Agent> agents)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].gameObject.tag = "SelectedDeactivatedAgent";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void IChangeStateToFreezed(Agent data)
        {
            data.gameObject.tag = "Freezed";

            if (AP.displayColorbyEnergyLevels)
            {
                data.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel,startTime);
            }
            
            if(AP.displayColorByState)
            {
                data.GetComponent<MeshRenderer>().material.color = Color.grey;
            }
          
           
            signalReceiverPassiveCounter = 0;






        }


        /// <summary>
        /// This method changes the current agents state to "Freezed" based on its neighbours
        /// </summary>
        public void ChangeStateToFreezedBasedOnNeighbourCount()
        {
            if (this.gameObject.tag == "SignalEmmiter")
            {

                /// if all  its neighbours are signal emmiters, change own state to freezed
                int count = 0;
                if (base.mySignalReceivers.Count != 0)
                {

                  


                    for (int i = 0; i < base.neighbours.Count; i++)
                    {
                        if (neighbours[i].gameObject.tag == "SignalEmmiter")
                        {
                            count++;
                        }
                    }



                    if (count == base.mySignalReceivers.Count)
                    {

                        IChangeStateToFreezed(this.gameObject.GetComponent<Agent>());
                    }




                }
                
           


            }



        } // END METHOD



        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void IChangeStateToSignalEmmiter(Agent data)
        {
            data.gameObject.tag = "SignalEmmiter";
         

            if (AP.displayColorbyEnergyLevels)
            {
                data.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, startTime);
            }

            if (AP.displayColorByState)
            {
                data.GetComponent<MeshRenderer>().material.color = Color.blue;
            }






        }

        /// <summary>
        /// This method changes the states of the singal emmiters neigbours from passive to emmiters.
        /// </summary>
        public void ChangeStateToSignalEmmiter()
        {


            if (this.gameObject.tag == "SignalEmmiter")
            {



                int SignalReceiverCount = 0;
                if (base.mySignalReceivers.Count != 0)
                {
                    for (int i = 0; i < base.neighboursTemp.Count; i++)
                    {

                        if (neighboursTemp[i].gameObject.tag == "SignalReceiverPassive")
                        {
                            SignalReceiverCount++;
                        }
                    }
                }

                /// Only change states of the neighbours that are Passive Signal Receivers to Signal Emmiters
                if (SignalReceiverCount == base.mySignalReceivers.Count)
                {
                    for (int i = 0; i < base.neighboursTemp.Count; i++)
                    {

                        if (base.neighboursTemp[i].tag == "SignalReceiverPassive")
                        {
                            IChangeStateToSignalEmmiter(base.neighboursTemp[i].GetComponent<Agent>());
                        }
                    }

                }

            }


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>

        public void IChangeStateToSignalReceiver(Agent data)
        {
           

            data.gameObject.tag = "SignalReceiver";
      

            if (AP.displayColorbyEnergyLevels)
            {
                data.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, startTime);
            }

            if (AP.displayColorByState)
            {
                Color color = (Utility.RGBToFloat(54, 141, 255));
                data.GetComponent<MeshRenderer>().material.color = color;
            }



        }



        /// <summary>
        /// 
        /// this method is implemented inside OntriggerEnter()
        /// </summary>
        /// <param name="data"></param>
        public void IChangeStateToSignalReceiverPassive(Agent data)
        {

            data.gameObject.tag = "SignalReceiverPassive";
           

            if (AP.displayColorbyEnergyLevels)
            {
                data.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, startTime);
            }

            if (AP.displayColorByState)
            {
                data.GetComponent<MeshRenderer>().material.color = Color.red;
            }




        }


        #endregion




        private void OnDrawGizmos()
        {
            //if (GenerationManager.generationCount == 0)
            //{

            //    if (this.gameObject.tag == "Freezed" || this.gameObject.tag == "SignalEmmiter" || this.gameObject.tag == "SignalReceiverPassive")
            //    {
            //        for (int i = 0; i < base.neighbours.Count; i++)
            //        {
            //            Gizmos.color = Color.white;
            //            Vector3 dir = base.neighbours[i].transform.position - this.gameObject.transform.position;
            //            Gizmos.DrawRay(this.gameObject.transform.position, dir);

            //        }
            //    }
            //}


        }

        #region COLLIDERS
        private void OnTriggerEnter(Collider other)
        {
            if (GenerationManager.generationCount == 0)
            {
                if (other.gameObject.tag != "De-Activated" && other.gameObject.tag != "ActivatedPlaceHolder"
                    && other.gameObject.tag != "CollidedPlaceHolder")
                {

                    base.neighboursTemp.Add(other.gameObject.GetComponent<Agent>());

                }

                if (other.gameObject.tag == "SignalEmmiter")
                {


                    IChangeStateToSignalReceiverPassive(this.gameObject.GetComponent<Agent>());


                }





            }



        }



        #endregion



    }//////END CLASS


}
