using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using OrganizationalModel.Managers;
using OrganizationalModel.Population;
using OrganizationalModel.Behaviors;
using OrganizationalModel.BaseClass;
namespace OrganizationalModel.UserInterface
{
    public class UI : MonoBehaviour
    {
        Agent agent = new Agent();
        List<float> energyLevels = new List<float>();
        float initEnergy = 650 * AgentPopulation.populationList.Count;
        float totalEnergyConsumed = 0;
      
        float avaregeEnergyConsumed = 0;

        float gameTime = 0f;
        string timeFormat;
        // NON STATIC PUBLIC VARIABLES TO REFERENCE FROM OTHER CLASSES  

        private void Start()
        {
            ///////////////////////////////VARIABLE INITIALIZATION////////////////////////////////
            // // NON STATIC PUBLIC VARIABLE INITIALIZATION


            ///////////////////////////////METHOD INITIALIZATION////////////////////////////////
        }


        private void Update()
        {
            gameTime += Time.deltaTime;

            int seconds = (int)( gameTime%  60);
            int minutes = (int)(gameTime / 60) % 60;
            int hours = (int)(gameTime / 3600) % 24;

             timeFormat = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
        }

        //private void OnGUI()
        //{

        //    GUI.Label(new Rect(10, 25, 500, 200), "Total Generations:" + "  " + GenerationManager.generationCount);
        //    GUI.Label(new Rect(10, 40, 300, 200), "Total signal emmiters:" + "  " + AgentPopulation.emmiterAgentList.Count);
        //    GUI.Label(new Rect(10, 110, 300, 200), "Generation change:" + "  " + GenerationManager.generationChange);
        //    GUI.Label(new Rect(10, 70, 300, 200), "total Freezed agents:" + "  " + AgentPopulation.freezedAgentList.Count.ToString());
        //    GUI.Label(new Rect(10, 55, 300, 200), "Total signal Receivers:" + "  " + AgentPopulation.signalReceiverAgentList.Count);



        //    GUI.Label(new Rect(10, 130, 300, 200), "Total Population:" + "  " + AgentPopulation.populationList.Count);


        //  // GUI.Label(new Rect(10, 145, 300, 200), "migrateCluster:" + "  " + AgentPopulation.migrateCluster);

        // // if(GenerationManager.generationCount!=0) GUI.Label(new Rect(10, 160, 300, 200), "total passive deacivated:" + "  " + AgentPopulation.deActivatedPassiveAgentList.Count);


        //    GUI.Label(new Rect(10, 175, 300, 200), "total deactivated agents:" + "  " + AgentPopulation.deActivatedAgentList.Count);

        //    GUI.Label(new Rect(10, 190, 500, 200), "Start WindForce:" + "  " + GenerationManager.playWindEffect);

        //  //  GUI.Label(new Rect(10, 205, 500, 200), "calculateRulesDiscreteCoroutine:"+" " + agent.calculateRulesDiscreteCoroutine.ToString());




        //}



        private void OnGUI()
        {

          

        
            
            for (int i = 0; i < AgentPopulation.populationList.Count; i++)
            {

               
                    energyLevels.Add(650 - AgentPopulation.populationList[i].energyLevel);
                

            
            }

            if (energyLevels.Count > AgentPopulation.populationList.Count)
            {
               
                energyLevels.RemoveRange(0, energyLevels.Count - AgentPopulation.populationList.Count);
            }


            if (energyLevels.Count != 0)
            {
                totalEnergyConsumed = energyLevels.Sum();

                avaregeEnergyConsumed = totalEnergyConsumed / AgentPopulation.populationList.Count;
            }


            GUIStyle aa = new GUIStyle();
            aa.normal.background = null;
            aa.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
            aa.fontSize = 15;

            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.background = null;
            guiStyle.normal.textColor = new Color(1, 1, 1);
            guiStyle.fontSize = 22;


            GUI.Label(new Rect(10, 0, 500, 200), "Goal:" + "  " + AgentPopulation.freezedAgentList.Count / (float)AgentPopulation.populationList.Count * 100 + "%", guiStyle);
            GUI.Label(new Rect(10, 35, 500, 200), "Time:" + "  " + timeFormat, guiStyle);

            GUI.Label(new Rect(10, 65, 500, 200), "Total Generations:" + "  " + GenerationManager.generationCount.ToString(), guiStyle);
            GUI.Label(new Rect(10, 95, 500, 200), "Total Population:" + "  " + AgentPopulation.populationList.Count, guiStyle);


            GUI.Label(new Rect(10, 125, 500, 200), "Total energy consumed:" + "  " + totalEnergyConsumed.ToString("0.00#") + " " + "Kilo Joules", guiStyle);
            GUI.Label(new Rect(10, 155, 500, 200), "Averege energy consumed:" + "  " + avaregeEnergyConsumed.ToString("0.00#") + " " + "Kilo Joules", guiStyle);



        }

    }

}
