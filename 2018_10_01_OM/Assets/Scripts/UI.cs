using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrganizationalModel.Managers;
using OrganizationalModel.Population;
using OrganizationalModel.Behaviors;
namespace OrganizationalModel.UserInterface
{
    public class UI : MonoBehaviour
    {


        // NON STATIC PUBLIC VARIABLES TO REFERENCE FROM OTHER CLASSES  

        private void Start()
        {
            ///////////////////////////////VARIABLE INITIALIZATION////////////////////////////////
            // // NON STATIC PUBLIC VARIABLE INITIALIZATION


            ///////////////////////////////METHOD INITIALIZATION////////////////////////////////
        }


        private void OnGUI()
        {

            GUI.Label(new Rect(10, 25, 500, 200), "Total Generations:" + "  " + GenerationManager.generationCount);
            GUI.Label(new Rect(10, 40, 300, 200), "Total signal emmiters:" + "  " + AgentPopulation.emmiterAgentList.Count);
            GUI.Label(new Rect(10, 110, 300, 200), "Generation change:" + "  " + GenerationManager.generationChange);
            GUI.Label(new Rect(10, 70, 300, 200), "total Freezed agents:" + "  " + AgentPopulation.freezedAgentList.Count.ToString());
            GUI.Label(new Rect(10, 55, 300, 200), "Total signal Receivers:" + "  " + AgentPopulation.signalReceiverAgentList.Count);



            GUI.Label(new Rect(10, 130, 300, 200), "Total Population:" + "  " + AgentPopulation.populationList.Count);


           GUI.Label(new Rect(10, 145, 300, 200), "migrateCluster:" + "  " + AgentPopulation.migrateCluster);

            GUI.Label(new Rect(10, 160, 300, 200), "total passive deacivated:" + "  " + AgentPopulation.deActivatedPassiveAgentList.Count);


        }

    }

}
