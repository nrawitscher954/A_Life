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

      
        float startTime;




        private bool addPlaceHolderTargetHistoryCoroutine = false;
   
        Rigidbody rb;
      

        public float totalDisplacement;
        public float averegeNeighbourDisplacement;
        private List<float> neighboursDisplacement = new List<float>();

      
        private List<Vector3> displacementPositions = new List<Vector3>();

        private Vector3 cloudCenter = Vector3.zero;

        [HideInInspector]
        public float distFromCloudCenter;

        [HideInInspector]
        public float placeHolderOffset;

        public BehaviorController behavior = new BehaviorController();
        public static bool runSimulation;

         float scale =0;

       
        public Gradient myGradient;
        Material emissiveMaterial;
        float scalarFieldValuesProximityTotal = 0;
         float averege = 0; // averege values of scalarFieldValuesProximityTotal

        LineRenderer line;
        void Start()
        {

           
            emissiveMaterial = gameObject.GetComponent<MeshRenderer>().material;
            startTime = Time.time;


        


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
  
            if(SimulationManager.Get().addRigidBodyCollider==false)
            {
                base.IMaxForce = 0.06f;
                base.IMaxSpeed = 0.06f;
            }

            if (SimulationManager.Get().addRigidBodyCollider)
            {
                base.IMaxForce = 3.5f;
                base.IMaxSpeed = 3.5f;
            }

            base.VisionRadius = (this.gameObject.GetComponent<SphereCollider>().radius * 2) * 0.9f;

            rb = GetComponent<Rigidbody>();
    

            base.energyDecreasingFactor = 0.1f;
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
           
           // if (runSimulation)
           if(SimulationManager.Get().runSimulation)
            {
                if (GenerationManager.generationCount == 0)
                {
                    //  if(SimulationManager.Get().oneGeneration==false) CheckNeighours(); // this does not decrese performance

                   // CheckNeighours();

                    if (base.energyLevel < 0) base.energyLevel = 0;

                    base.neighbours = base.neighboursTemp.Distinct().ToList();




                    RulesContinous();
                    IAgentBehaviorConditions();

                    base.CalculateEmmiterHistory();
                    base.CalculateReceiverHistory();
                    base.CalculateDeActivatedHistory();
                    base.CalculateFreezedHistory();

      



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


                    //if (gameObject.tag == "SignalReceiver" || gameObject.tag == "Freezed") base.IRunPhysics();

                }
            }
        }


        ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////  ////////////////////////////////METHODS////////////////////////////////////////////////////////////////////////

        public List<Vector3> Draw2dCircle(Vector3 origin, PlaceHolder obj)
        {
           

            int nPts = 50;
            float angCount = Mathf.PI * 2 / nPts; // radians
            float theta = 0;
            float cosTheta = 0;
            float sinTheta = 0;
            float radius = 0.4f;

           // if(obj.tag =="DeActivatedPlaceHolder") radius =0.1f;

           List<Vector3> listPt = new List<Vector3>();

            // equation of a circle : x = r*cos(theta)
            //                        y = r*sin(theta)

          

                for (int i = 0; i < nPts; i++)

                {
                    theta = i * angCount;
                    cosTheta = origin.x + radius * Mathf.Cos(theta);
                    sinTheta = origin.z + radius * Mathf.Sin(theta);
                    

                    Vector3 pts = new Vector3(cosTheta, origin.y, sinTheta);
                    listPt.Add(pts);


                }

            

            return listPt;
        }



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
            FreezedAgentConditions();
            DeActivatedAgentConditions();


           


        }

      


        #endregion



   

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

                if (SimulationManager.Get().displayTopology)
                {
                    
                    Gizmos.DrawSphere(this.gameObject.transform.position, 0.1f);
                }

                


            }


        }


   


        private void CheckNeighours()
        {
            if (base.neighboursTemp.Count != 0)
            {
                for (int i = base.neighboursTemp.Count - 1; i >= 0; i--)
                {
                    if (base.neighboursTemp[i] != null)
                    {
                        if (Vector3.Distance(this.gameObject.transform.position, base.neighboursTemp[i].transform.position) > 1.1f)
                        {
                            base.neighboursTemp.RemoveAt(i);
                        }
                    }
                }
            }


            if (base.neighbours.Count != 0)
            {
                for (int i = base.neighbours.Count - 1; i >= 0; i--)
                {
                    if (base.neighbours[i] != null)
                    {
                        if (Vector3.Distance(this.gameObject.transform.position, base.neighbours[i].transform.position) > 1.1f)
                        {
                            base.neighbours.RemoveAt(i);
                        }
                    }
                }
            }
        }



        #region RIGID BODY UPDATE TEST
        private void ApplyForcesRigidBody(Vector3 force, float maxForce)
        {
            force.Normalize();
            force *= maxForce;
            rb.AddForce(force * maxForce);
        }
        #endregion

        //private void SeekSimpleForRB(Agent agent, PlaceHolder target, float maxForce)
        //{
        //    Vector3 desiredVelToClosestNeighbour = target.transform.position - agent.transform.position;

        //    ApplyForcesRigidBody(desiredVelToClosestNeighbour, maxForce);
        //}

        private void SeekSimpleForRB(Agent agent, PlaceHolder target)
        {
            Vector3 desiredVelToClosestNeighbour = target.transform.position - agent.transform.position;
            desiredVelToClosestNeighbour.Normalize();
            desiredVelToClosestNeighbour *= base.IMaxSpeed;
            Vector3 steer = desiredVelToClosestNeighbour - rb.velocity;


            Vector3 steerLimit = steer.Limit(base.IMaxForce);

            ApplyForcesRigidBody(desiredVelToClosestNeighbour, base.IMaxForce);
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
                    base.neighboursTempVector.Add(other.gameObject.transform.position);
                }

                if (other.gameObject.tag == "SignalEmmiter")
                {


                    base.ChangeStateToSignalReceiverPassive(this.gameObject.GetComponent<Agent>(),0);
                 

                }





            }



        }

        //private void OnTriggerExit(Collider other)
        //{


        //    for (int i = base.neighbours.Count - 1; i >= 0; i--)
        //    {
        //        if (base.neighbours != null)
        //        {
        //            base.neighbours.Remove(other.GetComponent<Agent>());
        //        }
        //    }
        //}


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

            //for (int i = base.neighbours.Count - 1; i >= 0; i--)
            //{
            //    if (base.neighbours != null)
            //    {
            //        base.neighbours.Remove(collision.gameObject.GetComponent<Agent>());
            //    }
            //}

            //FixedJoint[] fixedJoints;

            //fixedJoints = GetComponents<FixedJoint>();

            //foreach (FixedJoint joint in fixedJoints)
            //{
            //    Destroy(joint);
            //}
                
        }






        bool hasJoint;
     

        private void AddFixedJoint(Collision data)
        {


            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = data.rigidbody;
            joint.breakForce = 100;
            joint.breakTorque = 100;
       

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
