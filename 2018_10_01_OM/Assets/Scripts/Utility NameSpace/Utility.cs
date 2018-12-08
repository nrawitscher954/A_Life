using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using System.IO;

using OrganizationalModel.BaseClass;

////////////////////////////////CLASS DESCRIPTION////////////////////////////////
/// <summary>
/// 
/// Library with a collection of useful methods, work in progress
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
//
/// 
/// 
/// 
/// 
/// 
/// 
/// </summary>

namespace OrganizationalModel.UtilityFunctions
{
    public class Utility : MonoBehaviour
    {


        public static  Agent ClosestObject ( Agent gameObjectToSearchFrom, List<Agent> collectionOfGameObjects)
        {


            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {

              
                float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return collectionOfGameObjects[smallestIndex];

          
          
        }



        public static Pixel ClosestObject(Agent gameObjectToSearchFrom, List<Pixel> collectionOfGameObjects)
        {


            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {


                float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return collectionOfGameObjects[smallestIndex];



        }





        public static  Agent ClosestObject ( PlaceHolder gameObjectToSearchFrom, List<Agent> collectionOfGameObjects)
        {


            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {

              
                float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return collectionOfGameObjects[smallestIndex];

          
          
        }




        public static Vector3 ClosestPoint(Vector3 gameObjectToSearchFrom, List<Vector3> collectionOfGameObjects)
        {


            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {


                float distance = Vector3.Distance(gameObjectToSearchFrom, collectionOfGameObjects[i]);
                distanceList.Add(distance);
            }

            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return collectionOfGameObjects[smallestIndex];



        }









        /// <summary>
        /// Get closestObject from Same collection. Methods makes sure that the source object doesnt
        /// return itself
        /// </summary>
        /// <param name="gameObjectToSearchFrom"></param>
        /// <param name="listToSearch"></param>
        /// <returns></returns>

        public static  Agent ClosestObjectFromSameCollection(Agent gameObjectToSearchFrom, List<Agent> listToSearch)
        {
            // returns closest point from a point collection.
            // here it makes sure that the point does not return itself

            List<float> distanceList = new List<float>();
            List<Agent> newList = new List<Agent>(); // add all objects except itself
            for (int i = 0; i < listToSearch.Count(); i++)
            {

                if (gameObjectToSearchFrom != (listToSearch[i]))
                {

                    float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, listToSearch[i].transform.position);

                    distanceList.Add(distance);
                    newList.Add(listToSearch[i]);
                }
            }
            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return newList[smallestIndex];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void CreateTextFileIndexes(string filePath, string[] data)

        {
            System.IO.File.WriteAllLines(filePath, data);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjectToSearchFrom"></param>
        /// <param name="collectionOfGameObjects"></param>
        /// <returns></returns>
        public static Agent FarthestObject(Agent gameObjectToSearchFrom, List<Agent> collectionOfGameObjects)
        {
            // THIS METHOD HAS BEEN TESTED IN C# GRASSHOPPER AND IT WORKS FINE

            List<float> distanceList = new List<float>();
            for (int i = 0; i < collectionOfGameObjects.Count; i++)
            {

                float distance = Vector3.Distance(gameObjectToSearchFrom.transform.position, collectionOfGameObjects[i].transform.position);
                distanceList.Add(distance);
            }

            int largestIndex = distanceList.IndexOf(distanceList.Max());

            return collectionOfGameObjects[largestIndex];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointToSearchFrom"></param>
        /// <param name="pointCloudToSearch"></param>
        /// <returns></returns>
        public static Vector3 FarthestPoint(Vector3 pointToSearchFrom, List<Vector3> pointCloudToSearch)
        {
            List<float> distanceList = new List<float>();
            for (int i = 0; i < pointCloudToSearch.Count; i++)
            {

                float distance = Vector3.Distance(pointToSearchFrom , pointCloudToSearch[i]);
                distanceList.Add(distance);
            }

            int largestIndex = distanceList.IndexOf(distanceList.Max());

            return pointCloudToSearch [largestIndex];
        }



   
        
     




        public static Vector3 RandomSpherePoint(float x0, float y0, float z0, float radius)
        {
            float u = Random.Range(0.0f, 1.0f);
            float v = Random.Range(0.0f, 1.0f);
            float theta = 2 * Mathf.PI * u;
            float phi = Mathf.Acos(2 * v - 1);
            float x = x0 + (radius * Mathf.Sin(phi) * Mathf.Cos(theta));
            float y = y0 + (radius * Mathf.Sin(phi) * Mathf.Sin(theta));
            float z = z0 + (radius * Mathf.Cos(phi));
            return new Vector3(x, y, z);
        }


        public static Color RGBToFloat(float r, float g, float b)
        {

            if (r > 255)
                r = 255f;

            if (g > 255)
                g = 255f;

            if (b > 255)
                b = 255f;

            r /= 255f;
            g /= 255f;
            b /= 255f;

            Color c = new Color(r, g, b);

            return c;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <param name="threshHold"></param>
        /// <returns></returns>
        public static bool Similar(Vector3 vec1, Vector3 vec2, float threshHold)
        {
            
            return Mathf.Abs((vec1 - vec2).magnitude) <= threshHold;
        }





        /// <summary>
        /// Uniform distribution of points one a sphere
        /// </summary>
        /// <param name="numDirections"></param>
        /// <returns></returns> returns an array of Vector3
        /// 

        public static Vector3[] UnifromSphericalPointDistribution(int numDirections)
        {
            // var pts = new Point3d[numDirections];
            Vector3[] pts = new Vector3[numDirections];
            float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
            float off = 2f / numDirections;

            foreach (var k in Enumerable.Range(0, numDirections))
            {
                float y = k * off - 1 + (off / 2);
                float r = Mathf.Sqrt(1 - y * y);
                float phi = k * inc;
                float x = Mathf.Cos(phi) * r;
                float z = Mathf.Sin(phi) * r;
                pts[k] = new Vector3(x, y, z);

            }

            return pts;
        }



 



    }
        

}
