using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrganizationalModel.Population;
using OrganizationalModel.Behaviors;

namespace OrganizationalModel.Managers
{
    public class GenerationManager : MonoBehaviour
    {

        ////////////////////////////////CLASS DESCRIPTION////////////////////////////////
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
        /// </summary>
        ////////////////////////////////DECLARE GLOBAL VARIABLES////////////////////////////////

        // REFERENCE PREFABS / EMPTY GAMEOBJECTS

        //CLASS INSTANCES

        //PUBLIC LISTS

        //PUBLIC STATIC LISTS
       public List<int> generationCountHistoryList = new List<int>();
        //PRIVATE LISTS

        // PUBLIC VARIABLES

        // PUBLIC STATIC VARIABLES
        public static int deActivatedAgentAndFreezedAgentTotal = 0;
        public static int generationCount = 0;

        public int generationChangeWaitTime = 0;

        public static bool generationChange = false;
        public static bool temporaryStabilityGeneration_0 = false;
        public static bool temporaryStabilityGeneration_1ToInfinity = false;
        //PRIVATE VARIABLES
        public static bool generationCoroutine = false;
        // NON STATIC PUBLIC VARIABLES





        void Start()
        {

            ///////////////////////////////VARIABLE INITIALIZATION////////////////////////////////

            // NON STATIC PUBLIC VARIABLE INITIALIZATION

            ///////////////////////////////METHOD INITIALIZATION////////////////////////////////


        } // END START


        void Update()
        {

            GenerationKeeper();


        } //END UPDATE

        ////////////////////////////////METHODS////////////////////////////////////////////////

        /// <summary>
        ///  Keeps track of when a generation change happens and the amount of generations 
        /// </summary>
        private void GenerationKeeper()
        {


            /// Initialize Generation 0.

            //if (AgentPopulation.deActivatedAgentList.Count == AgentPopulation.populationList.Count && generationCoroutine == false && generationCount == 0)
            //{
            //    generationChange = false;
            //    generationCountHistoryList.Add(generationCount);
            //    //generationCoroutine = true;
            //}

            /// calculate genertion change from Generation_0 to Generation_1 ///+ AgentPopulation.freezedAgentList.Count  ///(AgentPopulation.deActivatedAgentList.Count
            if (AgentPopulation.freezedAgentList.Count == AgentPopulation.populationList.Count && generationCount == 0 && generationCoroutine == false)
            {
                generationChange = true;
                generationCount++;
                generationCountHistoryList.Add(generationCount);
                generationCoroutine = true;

            }

            /// calculate genertion change for all generations except from Generation_0 to Generation_1                                                  
            if (AgentPopulation.deActivatedAgentList.Count + AgentPopulation.freezedAgentList.Count + AgentPopulation.emmiterAgentList.Count
                == AgentPopulation.populationList.Count && generationCount >= 1 && generationCoroutine == false)


            {
                generationChange = true;
                generationCount++;
                generationCountHistoryList.Add(generationCount);
                generationCoroutine = true;
            }
            //+ AgentPopulation.freezedAgentList.Count 
            if (AgentPopulation.deActivatedAgentList.Count == AgentPopulation.populationList.Count && generationCount != 0)
            {
                generationChangeWaitTime++;

            }

            if (generationChangeWaitTime == 2)
            {
                generationChange = false;
                generationChangeWaitTime = 0;
                generationCoroutine = false;
                ReOrganizationBehavior.calculatePixelPermanenceClusterAgent = false;
            }




        }




    }/////// END CLASS

}
