using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;
using OrganizationalModel.UtilityFunctions;
using OrganizationalModel;
public class GradientTest : MonoBehaviour {

    // Use this for initialization
    public Gradient myGradient;
    

    MeshRenderer emissiveRenderer;
    Material emissiveMaterial;

    private float scale = 0;

    private bool CoroutineConstructRange = false;
    List<float> rangeValues = new List<float>();
    private float[] distances;
    void Start () {
        emissiveRenderer = GetComponent<MeshRenderer>();
        emissiveMaterial = emissiveRenderer.material;

        distances = new float[AgentPopulation.populationList.Count];
    }
	
	// Update is called once per frame
	void Update () {

       

        //float t = Mathf.Sin(Time.time * 0.8f) ;
        //float t = Mathf.Sin(Time.time * 0.2f)*Mathf.PI;
        // float t = Mathf.Sin(Time.time* Mathf.PI)*0.6f ;

        // Color color = myGradient.Evaluate(t);

        //Color c = Lerp3(Color.white, Color.red, Color.blue, t);

       //  gameObject.GetComponent<MeshRenderer>().material.color = Lerp3(Color.white,Color.red, Color.blue,t);

        //Color c = Color.Lerp(


        //    Color.white, Color.grey,
        //    Mathf.Sin(Time.time * Mathf.PI) * 0.8f + 0.8f //0.5f + 0.5f
        //);

        //emissiveMaterial.SetColor("_Emission", c);
        //////emissiveRenderer.UpdateGIMaterials();
        //DynamicGI.SetEmissive(emissiveRenderer, c);


        if (AgentPopulation.freezedAgentList.Count == AgentPopulation.populationList.Count)
        {
            if (SimulationManager.Get().displayColorByEmotion)
            {
                if(CoroutineConstructRange==false)
                {
                    rangeValues = SharpMatter.SharpMath.SharpMath.Range(AgentPopulation.freezedAgentList.Count); 
                CoroutineConstructRange = true;
                }
               
         
                scale += 0.005f;


                // CreateDistanceField(PeoplePopulation.peopleList, AgentPopulation.freezedAgentList);

            StartCoroutine(Test());
                //for (int i = 0; i < AgentPopulation.freezedAgentList.Count; i++)
                //{
                //    //float t = Mathf.Cos(rangeValues[i] * Mathf.PI + scale);
                //    float t = Mathf.Cos(AgentPopulation.populationList[i].GetComponent<Agent>().energyLevel * Mathf.PI + scale);
                //    Color c = myGradient.Evaluate(t);

                //    AgentPopulation.freezedAgentList[i].gameObject.GetComponent<MeshRenderer>().material.color = c;// Utility.Lerp3(Color.white, Color.red, Color.blue, Mathf.Cos(rangeValues[i] * Mathf.PI + scale));

                //    //  foreach (var item in distances)
                //    //  {



                //    //      AgentPopulation.freezedAgentList[i].gameObject.GetComponent<MeshRenderer>().material.color = Utility.Lerp3(Color.white, Color.red, Color.blue, Mathf.Cos(rangeValues[i] * Mathf.PI + scale) * item);
                //    //   }

                //}



            }



        }



        //if (SimulationManager.Get().displayColorByEmotion)
        //{
        //    if (CoroutineConstructRange == false)
        //    {
        //        rangeValues = SharpMatter.SharpMath.SharpMath.Range(AgentPopulation.populationList.Count); //AgentPopulation.freezedAgentList.Count
        //        CoroutineConstructRange = true;
        //    }


        //    scale += 0.02f;



        //    for (int i = 0; i < AgentPopulation.populationList.Count; i++) // AgentPopulation.freezedAgentList.Count
        //    {


        //        AgentPopulation.populationList[i].gameObject.GetComponent<MeshRenderer>().material.color = Utility.Lerp3(Color.white, Color.red, Color.blue, Mathf.Cos(rangeValues[i] * Mathf.PI + scale));

        //    }

        //}

    }

    

    /// <summary>
    /// Create a Range of numbers. Works like the GH Component "Range"
    /// </summary>
    /// <param name="numberOfSteps"></param>
    /// <returns></returns>
    List<float> Range(float numberOfSteps)
    {
        float steps = 1.0f / numberOfSteps;
        float count = 0;
        List<float> values = new List<float>();
        for (int i = 0; i < numberOfSteps; i++)
        {
            values.Add(count += steps);
        }

        return values;

    }


    public IEnumerator Test()
    {
         CreateDistanceField(PeoplePopulation.peopleList, AgentPopulation.freezedAgentList);
        for (int i = 0; i < AgentPopulation.freezedAgentList.Count; i++)
        {
            //float t = Mathf.Cos(rangeValues[i] * Mathf.PI + scale);
           // float t = Mathf.Cos(AgentPopulation.populationList[i].GetComponent<Agent>().energyLevel * Mathf.PI + scale);
            //Color c = myGradient.Evaluate(t);

            // AgentPopulation.freezedAgentList[i].gameObject.GetComponent<MeshRenderer>().material.color = c;

            foreach (var item in distances)
            {

                float t = Mathf.Cos(AgentPopulation.populationList[i].GetComponent<Agent>().energyLevel * Mathf.PI + scale)*item;
                Color c = myGradient.Evaluate(t);
                AgentPopulation.freezedAgentList[i].gameObject.GetComponent<MeshRenderer>().material.color = c;
            }


        }

        yield return new WaitForSeconds(5);
    }


    public void CreateDistanceField(List<GameObject> humans, KdTree<Agent> agentPopulation)
    {
       
        for (int i = 0; i < agentPopulation.Count; i++)
        {



             float val = GetDistanceValues(agentPopulation[i].gameObject.transform.position, humans);


            // float valRemap = SharpMatter.SharpMath.SharpMath.Normalize(val, 0f, 1f);

            float valRemap = SharpMatter.SharpMath.SharpMath.Remap(val, 0, 12, 0, 1);



                // scalarField[i, j].ScalarValueProximity = val;

            distances[i] =  valRemap;
            

        }

    }








    /// <summary>
    /// 
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="humans"></param>
    /// <returns></returns>
    private float GetDistanceValues(Vector3 pos, List<GameObject> humans)
    {
        List<float> distanceList = new List<float>();

        for (int i = 0; i < humans.Count; i++)
        {

            float distance = Vector3.Distance(pos, humans[i].gameObject.transform.position);
            distanceList.Add(distance);
        }


        int smallestIndex = distanceList.IndexOf(distanceList.Min());

        return distanceList[smallestIndex];

    }
}
