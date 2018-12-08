using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAttractors : MonoBehaviour {

    public GameObject prefab;
    public int num;
    public int minDistribution;
    public int maxDistribution;
    public int arraySpacing;


    public static List<GameObject> populationList = new List<GameObject>();

    void Start()
    {
        InitializeAgents3DArray(num);

    }

    // Update is called once per frame
    //void Update () {

    //}




    private void InitializeAgents3DArray(int number)
    {
        for (int i = 0; i < System.Math.Ceiling(Mathf.Pow(number, 1.0f / 3.0f)); i++)
        {

            for (int j = 0; j < System.Math.Ceiling(Mathf.Pow(number, 1.0f / 3.0f)); j++)
            {

                for (int k = 0; k < System.Math.Ceiling(Mathf.Pow(number, 1.0f / 3.0f)); k++)
                {

                    Vector3 distributionArea = new Vector3(i * arraySpacing, j * arraySpacing, k * arraySpacing);

                    populationList.Add((Instantiate(prefab, distributionArea, Quaternion.identity)));






                }
            }

        }
    }

}
