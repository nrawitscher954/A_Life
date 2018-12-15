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
using OrganizationalModel.ScalarFields;

namespace OrganizationalModel.Behaviors
{
  
    public partial class OrganizationBehavior : Agent, IOrganization<Agent>
    {
       
      


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




        ////////////////////////////////DECLARE GLOBAL VARIABLES////////////////////////////////



        //CLASS INSTANCES

        //PUBLIC LISTS


        //PUBLIC STATIC LISTS

        //PRIVATE LISTS




        // PUBLIC VARIABLES

        private float averegeDist = 0;

        public float distanceToTarget;

        //PRIVATE VARIABLES
        private int signalReceiverPassiveThreshhold = 100;
        public int signalEmitterCounter = 0;
        private bool searchClosestNeighbour = false;
        public bool searchClosestNeighbourAgain = false;
        private bool createPlaceHolders = false;

        private bool addChemicalValue = false;
        
        private bool getActivePlaceHolders = false;

        private  int time = 0;

        


        // NON STATIC PUBLIC VARIABLES

        GameObject agentPopulation; // create variable to store game object that holds script top reference
        private AgentPopulation AP; // store script to reference in variable

       
        // NON STATIC PUBLIC VARIABLES
        GameObject pixelPopulation; /// create variable to store game object that holds script top reference
        private PixelPopulation pixelPop; /// store script to reference in variable

        GameObject scalarField;
        private ScalarField2D SF;

        GameObject generationManager;
        private GenerationManager GM;

        private bool selectedDeActivatedAgent = false;
        float startTime;




        private bool addPlaceHolderTargetHistoryCoroutine = false;
        private bool addAgentToFamilyTreeCoroutine = false;
        private bool getMyPositionBeforeWindSimCoroutine = false;
        Rigidbody rb;
      

        public float totalDisplacement;
        public float averegeNeighbourDisplacement;
        private List<float> neighboursDisplacement = new List<float>();

        //public Vector3 initialPos;
        private List<Vector3> displacementPositions = new List<Vector3>();

        private Vector3 cloudCenter = Vector3.zero;

        [HideInInspector]
        public float distFromCloudCenter;

        [HideInInspector]
        public float placeHolderOffset;

        public BehaviorController behavior = new BehaviorController();
        //LineRenderer lineRenderer;
        void Start()
        {
            startTime = Time.time;

         
            //lineRenderer = this.gameObject.GetComponent<LineRenderer>();
            //lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //lineRenderer.useWorldSpace = true;
            ///////////////////////////////VARIABLE INITIALIZATION////////////////////////////////

            // NON STATIC PUBLIC VARIABLE INITIALIZATION


            agentPopulation = GameObject.Find("AgentPopulation"); // create variable to store game object that holds script top reference
            AP = agentPopulation.GetComponent<AgentPopulation>(); // store script to reference in variable


            pixelPopulation = GameObject.Find("PixelPopulation");
            pixelPop = pixelPopulation.GetComponent<PixelPopulation>();
            //  NON STATIC PUBLIC VARIABLE INITIALIZATION
      

          
             scalarField = GameObject.Find("ScalarField2D"); 
             SF = scalarField.GetComponent<ScalarField2D>();

        generationManager = GameObject.Find("GenerationManager");
            GM = generationManager.GetComponent<GenerationManager>();

            ///////////////////////////////METHOD INITIALIZATION////////////////////////////////




            base.IMass = 1f;//REAL MASS OF UNIT
  
            base.IMaxForce = 0.04f;//0.02f;
            base.IMaxSpeed = 0.04f;//0.02f;
            base.VisionRadius = (this.gameObject.GetComponent<SphereCollider>().radius * 2) * 0.9f;

            rb = GetComponent<Rigidbody>();
    

            base.energyDecreasingFactor = 0.05f;
            base.energyLevel = 650f; // 650 maH --> litjuim batter capacity that we are using
            

            base.energyCapacity = 650f; // 650 maH --> litjuim batter capacity that we are using








            if (SimulationManager.Get().addRigidBodyCollider)
            {
                this.gameObject.GetComponent<SphereCollider>().isTrigger = false; 
                rb.isKinematic = false;

            }


        } // END START


        void Update()
        {
            if (SimulationManager.Get().runSimulation)
            {
                if (GenerationManager.generationCount == 0)
                {



                    if (base.energyLevel < 0) base.energyLevel = 0;

                    base.neighbours = base.neighboursTemp.Distinct().ToList();


                    //make sure agents stays on plane if its 2D, for some reason with repell action it goes to a 3d position sometimes
                    if (SimulationManager.Get().is2D)
                    {
                        if (this.gameObject.transform.position.y > 0 || this.gameObject.transform.position.y < 0)
                        {
                            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z);
                        }
                    }


                    RulesContinous();
                    IAgentBehaviorConditions();

                    base.CalculateEmmiterHistory();
                    base.CalculateReceiverHistory();
                    base.CalculateDeActivatedHistory();
                    base.CalculateFreezedHistory();

                    //DrawTopology();



                    for (int i = 0; i < AgentPopulation.signalReceiverPassiveAgentList.Count; i++)
                    {
                        if (AgentPopulation.signalReceiverPassiveAgentList[i].GetComponent<OrganizationBehavior>().signalReceiverPassiveCounter > signalReceiverPassiveThreshhold)
                        {
                            base.ChangeStateToSignalEmmiter(AgentPopulation.signalReceiverPassiveAgentList[i], 0);
                        }
                    }


                    if (SimulationManager.Get().addFixedJoints == false && SimulationManager.Get().addRigidBodyCollider == false && gameObject.tag == "SignalReceiver")
                    {
                        base.IRunPhysics();
                    }



                }
            }




        } //END UPDATE


        void FixedUpdate()
        {
            if (SimulationManager.Get().runSimulation)
            {
                if (SimulationManager.Get().addFixedJoints && SimulationManager.Get().addRigidBodyCollider)
                {
                    if (gameObject.tag == "SignalReceiver" || gameObject.tag == "Freezed" || gameObject.tag == "SignalEmmiter") base.IRunPhysics();

                    //if (gameObject.tag == "SignalReceiver" || gameObject.tag == "Freezed" || gameObject.tag == "De-ActivatedPassive") base.IRunPhysics();

                  //  if (gameObject.tag == "SignalReceiver" || gameObject.tag == "Freezed") base.IRunPhysics();

                }
            }
        }

        void LateUpdate()
        {

            //if (AgentPopulation.freezedAgentList.Count != 0)
            //{
            //    for (int i = 0; i < AgentPopulation.freezedAgentList.Count; i++)
            //    {
            //        cloudCenter = cloudCenter + AgentPopulation.freezedAgentList[i].transform.position;
            //    }

            //    cloudCenter = cloudCenter / AgentPopulation.freezedAgentList.Count;
            //}
        }

      
        ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////  ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////




        /////////////////////////////STATES //////////////////////////////////////////

        #region STATES RULES

        /// <summary>
        /// 
        /// </summary>

        public void RulesContinous()
        {
            base.ChangeStateToSignalEmmiter_Continuous();
            base.ChangeStateToFreezed_Continuous();
        }

        public void IAgentBehaviorConditions()
        {

            ISignalEmmiterConditions();
            ISignalReceiverConditions();
            ISignalReceiverPassiveConditions();
            FreezedAgentCoditions();

          //  if(this.gameObject.tag=="De-Activated") StartCoroutine(ApplyWindForce2(0.00001f, 90));


        }

      


        #endregion



        private void DrawVelVector()
        {
            Debug.DrawLine(this.gameObject.transform.position, base.velocity, Color.yellow);
          
        }

        private void OnDrawGizmos()
        {
            if (GenerationManager.generationCount == 0)
            {

                if (this.gameObject.tag == "Freezed" || this.gameObject.tag == "SignalEmmiter" || this.gameObject.tag == "SignalReceiverPassive")
                {
                    for (int i = 0; i < base.neighbours.Count; i++)
                    {
                        if (base.neighbours[i] != null)
                        {
                            Gizmos.color = Color.white;
                            Vector3 dir = base.neighbours[i].transform.position - this.gameObject.transform.position;
                            Gizmos.DrawRay(this.gameObject.transform.position, dir);
                        }

                    }
                }


            }


        }


        //private void DrawTopology()
        //{



        //        if (this.gameObject.tag == "Freezed" || this.gameObject.tag == "SignalEmmiter" || this.gameObject.tag == "SignalReceiverPassive")
        //        {
        //            lineRenderer.positionCount = base.neighbours.Count;
        //            lineRenderer.endWidth = 0.02f;
        //            lineRenderer.startWidth = 0.02f;
        //            lineRenderer.endColor = Color.white;
        //            lineRenderer.startColor = Color.white;

        //            for (int i = 0; i < base.neighbours.Count; i++)
        //            {
        //                lineRenderer.SetPosition(i, base.neighbours[i].transform.position);




        //            }
        //        }


        //}




        #region COLLIDERS
        private void OnTriggerEnter(Collider other)
        {
            if (GenerationManager.generationCount == 0)
            {
             
                if (other.gameObject.tag != "De-Activated" && other.gameObject.tag != "ActivatedPlaceHolder"
                    && other.gameObject.tag != "CollidedPlaceHolder")
                {

                    base.neighboursTemp.Add(other.gameObject.GetComponent<Agent>());
                    base.neighboursTempVector.Add(other.gameObject.transform.position);
                }

                if (other.gameObject.tag == "SignalEmmiter")
                {


                    base.ChangeStateToSignalReceiverPassive(this.gameObject.GetComponent<Agent>(),0);
                 

                }





            }



        }

        private void OnTriggerExit(Collider other)
        {
            for (int i = base.neighbours.Count - 1; i >= 0; i--)
            {
                if (base.neighbours != null)
                {
                    base.neighbours.Remove(other.GetComponent<Agent>());
                }
            }
        }


        private void OnCollisionEnter(Collision collision)
        {


            if (GenerationManager.generationCount == 0)
            {
                

                if (SimulationManager.Get().addFixedJoints)
                {
                    
                    AddFixedJoint(collision);
                    //AddSpringJoint(collision);
                }
                if (collision.gameObject.tag != "De-Activated" && collision.gameObject.tag != "ActivatedPlaceHolder"
                    && collision.gameObject.tag != "CollidedPlaceHolder")
                {

                    base.neighboursTemp.Add(collision.gameObject.GetComponent<Agent>());
                    base.neighboursTempVector.Add(collision.gameObject.transform.position);

                }

                if (collision.gameObject.tag == "SignalEmmiter")
                {


                    base.ChangeStateToSignalReceiverPassive(this.gameObject.GetComponent<Agent>(),0);


                }


            }

        }


        private void OnCollisionExit(Collision collision)
        {
            for (int i = base.neighbours.Count - 1; i >= 0; i--)
            {
                if (base.neighbours != null)
                {
                    base.neighbours.Remove(collision.gameObject.GetComponent<Agent>());
                }
            }
        }






        bool hasJoint;
     

        private void AddFixedJoint(Collision data)
        {


            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = data.rigidbody;
            joint.breakForce = 200;
            joint.breakTorque = 200;
       

            base.neighbourHistory.Add(data.gameObject.GetComponent<Agent>());
        }


        private void AddSpringJoint(Collision data)
        {
            
                SpringJoint joint = gameObject.AddComponent<SpringJoint>();
                joint.connectedBody = data.rigidbody;
                gameObject.GetComponent<SpringJoint>().spring = 30000;
                gameObject.GetComponent<SpringJoint>().damper = 30000;

          

            base.neighbourHistory.Add(data.gameObject.GetComponent<Agent>());
        }


       




        #endregion



    }//////END CLASS


}
