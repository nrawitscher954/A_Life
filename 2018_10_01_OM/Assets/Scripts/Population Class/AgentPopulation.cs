using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Text;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Managers;
using OrganizationalModel.Export;
////////////////////////////////CLASS DESCRIPTION////////////////////////////////
/// <summary>
/// 
/// This class handles the population of agents, and their initial states, color, and name
/// each state is deffined as a tag, and their name corresponds to their index order. Agent 0, Agent 1 ... etc
/// Also a random agent of the population is chosen to be a Signal Emmiter and be able to activate another Agent
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
/// /
/// 
/// In ActivateRandomAgents function I changed the index to 14. To always activate agent 14, to edit go to method
/// 
/// 
/// 
/// 
/// 
/// </summary>

namespace OrganizationalModel.Population
{
    public class AgentPopulation : MonoBehaviour
    { //Agent

        ////////////////////////////////DECLARE GLOBAL VARIABLES////////////////////////////////

        // REFERENCE PREFABS / EMPTY GAMEOBJECTS
        public GameObject agentPrefab;
        public GameObject AgentPopulationHolder;

        //CLASS INSTANCES
     


        //PUBLIC LISTS

        // PUBLIC STATIC LISTS
        public static List<Agent> populationList = new List<Agent>();



        
        public static bool migrateCluster = false;

        public static List<Agent> deActivatedAgentList;

        public static List<Agent> deActivatedPassiveAgentList;

        public static List<Agent> signalReceiverAgentList;

        public static List<Agent> signalReceiverPassiveAgentList;

        public static List<Agent> freezedAgentList;

        public static List<Agent> emmiterAgentList;


        public static List<int> indexActivatedAgentsList = new List<int>();


        //PRIVATE LISTS

        // list that holds  the indexes of the population
        private List<int> populationListIndexes = new List<int>();

        // PUBLIC VARIABLES
        public bool runSimulation;

      //  [SerializeField]
        public  bool is2D;

       // [SerializeField]
        public  bool is3D;

        [SerializeField]
        public static bool flock;

        public float arraySpacing;
        public int populationCount;
        public static int timeToInitFirstAgent = 0;

        public bool displayColorByState;
        public bool displayColorbyEnergyLevels;

        public Color startColor;
        public Color endColor;

        //PRIVATE VARIABLES
        public static bool runCoroutine = false;
        private bool convertToAgentCoroutine = false;
        public static int time;

        // NON STATIC PUBLIC VARIABLES


        // Use this for initialization

        Agent agent = new Agent();

        float startTime;


        public int staticAgentsInCluster = 0;
        void Start()
        {
            startTime = Time.time;



            if (is2D)
            {
                InitializeAgents2DArray(populationCount);



            }

            if (is3D)
            {
                InitializeAgents3DArray(populationCount);
            }



            Parent();
            AgentName();
            InitializeState();
            InitColor();



            StreamWriter writer = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Summer\Spyropolous Studio\Data\SimulationData\3D\Cluster\Init state\OrganizationInitialState.txt");
            for (int i = 0; i < populationList.Count; i++)
            {

                string outPut = populationList[i].gameObject.transform.position.x.ToString() + "," + populationList[i].gameObject.transform.position.z.ToString() + "," + populationList[i].gameObject.transform.position.y.ToString();
                writer.WriteLine(outPut);
            }

            writer.Close();






        } // END START

        // Update is called once per frame


        void Update()
        {


            //Only run method once

            time += (int)Time.time;

            if (time == timeToInitFirstAgent && runCoroutine == false)
            {

                PickRandomAgentstoActivate(1);

                runCoroutine = true;
            }



           



            deActivatedAgentList = FindObjectsWithTag("De-Activated");
            signalReceiverAgentList = FindObjectsWithTag("SignalReceiver");
            signalReceiverPassiveAgentList = FindObjectsWithTag("SignalReceiverPassive");
            freezedAgentList = FindObjectsWithTag("Freezed");
            emmiterAgentList = FindObjectsWithTag("SignalEmmiter");
            deActivatedPassiveAgentList = FindObjectsWithTag("De-ActivatedPassive");

            if (deActivatedPassiveAgentList.Count == populationList.Count)
            {
                // migrateCluster = false; /// cant add this yet... will change all of them to signal receivers passive and then will bug out
                // PickRandomAgentstoActivate2(1);

                bool exportOrganizationPointcloud = true;

                string filePath = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Summer\Spyropolous Studio\Data\SimulationData\3D\Cluster\";
                string fileName = "ClusterMigration" + GenerationManager.generationCount.ToString() + ".txt";

                ExportData.ExportPointCloudToTxtFile(filePath, fileName, AgentPopulation.populationList, is3D, is2D, exportOrganizationPointcloud);


            }



                if (GenerationManager.generationChange)
            {

                 
                bool exportOrganizationPointcloud = true;
         
                string filePath = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Summer\Spyropolous Studio\Data\SimulationData\3D\Cluster\";
                string fileName = "ClusterOrganization" + GenerationManager.generationCount.ToString() + ".txt";

                ExportData.ExportPointCloudToTxtFile(filePath, fileName, AgentPopulation.populationList, is3D, is2D, exportOrganizationPointcloud);

                CheckForClusterStability();

              

                StringBuilder tsvData = new StringBuilder();
                // tsvData.AppendLine("Agent , Neighbour History, Inactive history State, De Activated History state, Emitter History state, Receiver History state");

                tsvData.AppendLine("Agent\tNeighbour History");

                for (int i = 0; i < populationList.Count; i++)
                {
                    string agentName = populationList[i].name;
                    

                    for (int j = 0; j < populationList[i].neighbourHistory.Count; j++)
                    {
                        string agentHistory = populationList[i].neighbourHistory[j].name.ToString();
                        string inactiveHistoryState = populationList[i].freezedHistoryCount.ToString();
                        string deActivatedHistoryState = populationList[i].deActivatedHistoryCount.ToString();
                        string emmiterHistoryState = populationList[i].emmiterHistoryCount.ToString();
                        string receiverHistoryState = populationList[i].receiverHistoryCount.ToString();

                        string data = agentName + "\t" + agentHistory;//+ "," + inactiveHistoryState + "," + deActivatedHistoryState + "," + emmiterHistoryState + "," + receiverHistoryState;
                        tsvData.AppendLine(data);
                        
                    }

                   

                }

                string Path = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Summer\Spyropolous Studio\Data\SimulationData\3D\Cluster\AgentHistory\HistoryData" + GenerationManager.generationCount.ToString() + ".tsv";

                File.AppendAllText(Path, tsvData.ToString());


                string filePath2 = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Summer\Spyropolous Studio\Data\SimulationData\3D\Cluster\HeliumLevels\";
                string fileName2 = "HeliumLevels" + GenerationManager.generationCount.ToString() + ".txt";

                StreamWriter writer = new StreamWriter(filePath2 + fileName2);

                for (int i = 0; i < populationList.Count; i++)
                {
                    string OutPut = populationList[i].energyLevel.ToString();
                    writer.WriteLine(OutPut);
                }

                writer.Close();

                //XDocument xmlData = null;
                //for (int i = 0; i < populationList.Count; i++)
                //{
                //    string agentName = populationList[i].name;



                //        xmlData = new XDocument(new XElement("Agents",
                //            new XElement("Agent", new XAttribute("Name", agentName),
                //            new XElement("NeighbourHistory", populationList[i].neighbourHistory.ToArray())
                //            )));






                //}

                //xmlData.Save(@"C:\Users\nicol\OneDrive\Desktop\a\test" + GenerationManager.generationCount.ToString() + ".xml");





                //var xml = new XElement("Agents", populationList.Select(x => new XElement("foo",
                //                               new XAttribute("Name", x.name.ToString()),
                //                               new XAttribute("NeighbourHistory", x.neighbourHistory.ToArray()))));
                                              
                                             


                //xml.Save(@"C:\Users\nicol\OneDrive\Desktop\a\test" + GenerationManager.generationCount.ToString() + ".xml");





            }









        }





        ////////////////////////////////METHODS////////////////////////////////////////////////



   /// <summary>
   /// 
   /// </summary>
       private void CheckForClusterStability()
        {
            
            int maxGenerationsForBeingStatic = 8;
            float staticClusterThreshold = 0.5f;
            for (int i = 0; i < populationList.Count; i++)
            {
                if(populationList[i].timesInCurrentPixel /2 == maxGenerationsForBeingStatic && populationList[i].neighboursTemp.Count!=1)
                /// divided by 2 because per frame timesInCurrentPixel increments by 2. This depends on the GenerationManager.generationChangeWaitTime. it is set to 3. this will increment TimesInCurrentPixel by 2!!
                {
                    staticAgentsInCluster++;
                }
            }

            if (staticAgentsInCluster >= (int)populationList.Count * staticClusterThreshold)
            {
                migrateCluster = true;
            }
        }
  
   








        private List<Agent> FindObjectsWithTag(string tag)
        {
            List<Agent> data = new List<Agent>();


            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject item in allObjects)
                if (item.GetComponent<Agent>() && item.tag == tag)
                    data.Add(item.GetComponent<Agent>());

            return data;
        }


        private void DisplayTopology()
        {

            for (int i = 0; i < populationList.Count; i++)
            {
                populationList[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            }


        }

        /// <summary>
        /// Initialize population in a 2D Array
        /// </summary>
        /// <param name="number"></param> number of agents to initialize
        private void InitializeAgents2DArray(int number)
        {

            for (int i = 0; i < Mathf.Sqrt(number); i++)
            {

                for (int j = 0; j < Mathf.Sqrt(number); j++)
                {

                    Vector3 distributionArea = new Vector3(i * arraySpacing, 0, j * arraySpacing);

                    populationList.Add((Instantiate(agentPrefab.GetComponent<Agent>(), distributionArea, Quaternion.identity)));



                }

            }


            for (int i = 0; i < populationList.Count; i++)
            {
                Vector3 delta = new Vector3(Random.onUnitSphere.x, 0, Random.onUnitSphere.z);
                populationList[i].transform.position += delta * 0.02f;
            }
            //////////////////////////////////////////////////////////


            /////////////////////////////////////////////////////////////  


        }

        /// <summary>
        /// Initialize population in a 3D Array
        /// </summary>
        /// <param name="number"></param> number of agents to initialize

        private void InitializeAgents3DArray(int number)
        {
            for (int i = 0; i < System.Math.Ceiling(Mathf.Pow(number, 1.0f / 3.0f)); i++)
            {

                for (int j = 0; j < System.Math.Ceiling(Mathf.Pow(number, 1.0f / 3.0f)); j++)
                {

                    for (int k = 0; k < System.Math.Ceiling(Mathf.Pow(number, 1.0f / 3.0f)); k++)
                    {

                        Vector3 distributionArea = new Vector3(i * arraySpacing + 7, j * arraySpacing, k * arraySpacing + 7);

                        populationList.Add((Instantiate(agentPrefab.GetComponent<Agent>(), distributionArea, Quaternion.identity)));




                    }
                }

            }

            for (int i = 0; i < populationList.Count; i++)
            {
                populationList[i].transform.position += Random.onUnitSphere * 5f;
            }

        }


        private void InitializeAgents2DRandom(int number, float dustributionRangeMin, float dustributionRangeMax)
        {
            for (int i = 0; i < number; i++)
            {


                Vector3 distributionArea = new Vector3(Random.Range(0, dustributionRangeMax), 0, Random.Range(0, dustributionRangeMax));
                populationList.Add((Instantiate(agentPrefab.GetComponent<Agent>(), distributionArea, Quaternion.identity)));

            }

        }



        private List<Agent> PickRandomAgentstoActivate(int number)
        {
            List<Agent> listToReturn = new List<Agent>();
            for (int i = 0; i < number; i++)
            {

                int index = Random.Range(0, populationCount);
                //int index = populationList.Count/ 2;
                indexActivatedAgentsList.Add(index); //index
                SystemActivatedAgentHistory.systemActivatedAgentHistory++;
                SystemActivatedAgentHistory.systemActivatedAgentHistoryList.Add(SystemActivatedAgentHistory.systemActivatedAgentHistory);
                listToReturn.Add(populationList[index]); //index



            }

            for (int i = 0; i < listToReturn.Count; i++)
            {
                listToReturn[i].tag = "SignalEmmiter";
                
                if (displayColorbyEnergyLevels)
                {
                    listToReturn[i].GetComponent<MeshRenderer>().material.color = agent.EnergyColor(startColor, endColor,agent.energyLevel, startTime);
                }

                if (displayColorByState)
                {
                    listToReturn[i].GetComponent<MeshRenderer>().material.color = Color.blue;

                }
            }

            return listToReturn;
        }







        private List<Agent> PickRandomAgentstoActivate2(int number)
        {
            List<Agent> listToReturn = new List<Agent>();
            for (int i = 0; i < number; i++)
            {

                int index = Random.Range(0, populationCount);
                //int index = populationList.Count/ 2;
                indexActivatedAgentsList.Add(index); //index
                SystemActivatedAgentHistory.systemActivatedAgentHistory++;
                SystemActivatedAgentHistory.systemActivatedAgentHistoryList.Add(SystemActivatedAgentHistory.systemActivatedAgentHistory);
                listToReturn.Add(deActivatedPassiveAgentList[index]); //index



            }

            for (int i = 0; i < listToReturn.Count; i++)
            {
                listToReturn[i].tag = "SignalEmmiter";

                if (displayColorbyEnergyLevels)
                {
                    listToReturn[i].GetComponent<MeshRenderer>().material.color = agent.EnergyColor(startColor, endColor, agent.energyLevel, startTime);
                }

                if (displayColorByState)
                {
                    listToReturn[i].GetComponent<MeshRenderer>().material.color = Color.blue;

                }
            }

            return listToReturn;
        }










        /// <summary>
        /// Set Parent GameObject in the inspector to hold all Agent objects
        /// </summary>

        private void Parent()
        {
            for (int i = 0; i < populationList.Count; i++)
            {
                populationList[i].transform.parent = AgentPopulationHolder.transform;

            }
        }



        /// <summary>
        /// Set Agent name According to Index order
        /// </summary>
        private void AgentName()
        {
            for (int i = 0; i < populationList.Count; i++)
            {
                populationList[i].name = "Agent" + " " + i;
                populationList[i].GetComponent<Agent>().agentName = "Agent" + " " + i;  //ReOrganizationBehavior
                populationListIndexes.Add(i);
            }
        }


        /// <summary>
        /// Set initial state of agents; i.e tag
        /// </summary>
        private void InitializeState()
        {


            for (int i = 0; i < populationList.Count; i++)
            {
                populationList[i].tag = "De-Activated";
            }


        }



        /// <summary>
        /// Set initial color of Agents
        /// </summary>
        private void InitColor()
        {

            for (int i = 0; i < populationList.Count; i++)
            {
                if (populationList[i].tag == "De-Activated")
                {
                    populationList[i].GetComponent<MeshRenderer>().material.color = Color.black;


                    //if (displayColorbyHeliumLevels)
                    //{
                    //    populationList[i].GetComponent<MeshRenderer>().material.color = agent.HeliumColor(startColor, endColor, agent.heliumLevel);
                    //}

                    //if (displayColorByState)
                    //{
                    //    populationList[i].GetComponent<MeshRenderer>().material.color = Color.black;

                    //}

                }


            }

        }





    }// END CLASS 

}

