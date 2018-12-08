using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

using OrganizationalModel.BaseClass;

namespace OrganizationalModel.Export
{
    public class ExportData : MonoBehaviour
    {
        public ExportData()
        { }

        /// <summary>
        /// Writes a text file to the specified file path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>make sure it has the  \  at the end of the file path
        /// <param name="data"></param>
        /// <param name="fileName"></param> 
        public static void ExportPointCloudToTxtFile(string filePath, Pixel[,,] data3D, Pixel[,] data2D, List<Agent> population3D, string fileName, int columns, int rows, int height, bool is3D, bool is2D,
            bool arrayPointCloud, bool OrganizationPointCloud, bool mobileAgentData, bool clusterAgentData)
        {


            if (is3D)
            {
                if (arrayPointCloud)
                {
                    StreamWriter writer = new StreamWriter(filePath + fileName);

                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            for (int k = 0; k < height; k++)
                            {
                                string outPut = data3D[i, j, k].X.ToString() + "," + data3D[i, j, k].Z.ToString() + "," + data3D[i, j, k].Y.ToString();

                             
                                writer.WriteLine(outPut);

                            }
                        }
                    }

                    writer.Close();
                }


                if(OrganizationPointCloud)

                {


                    StreamWriter writer = new StreamWriter(filePath + fileName);

                    for (int i = 0; i < population3D.Count; i++)
                    {

                     string outPut = population3D[i].gameObject.transform.position.x.ToString() + "," + population3D[i].gameObject.transform.position.z.ToString() + "," + population3D[i].gameObject.transform.position.y.ToString();
                      writer.WriteLine(outPut);                       
                    }

                    writer.Close();



                }


                if (mobileAgentData)
                {
                    StreamWriter writer = new StreamWriter(filePath + fileName);

                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            for (int k = 0; k < height; k++)
                            {
                                string outPut = data3D[i, j, k].CountMobileAgents.ToString();


                                writer.WriteLine(outPut);

                            }
                        }
                    }

                    writer.Close();
                }



                if (clusterAgentData)
                {
                    StreamWriter writer = new StreamWriter(filePath + fileName);

                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            for (int k = 0; k < height; k++)
                            {
                                string outPut = data3D[i, j, k].CountClusterAgents.ToString();


                                writer.WriteLine(outPut);

                            }
                        }
                    }

                    writer.Close();
                }




            }// END 3D CONDITION


            if (is2D)
            {
                if (arrayPointCloud)
                {
                    StreamWriter writer = new StreamWriter("@" + filePath + fileName);

                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {

                            string outPut = data2D[i, j].X.ToString() + "," + data2D[i, j].Z.ToString();


                            writer.WriteLine(outPut);


                        }
                    }

                    writer.Close();
                }




                if (OrganizationPointCloud)

                {
                    StreamWriter writer = new StreamWriter(filePath + fileName);

                    for (int i = 0; i < population3D.Count; i++)
                    {

                        string outPut = population3D[i].gameObject.transform.position.x.ToString() + "," + population3D[i].gameObject.transform.position.z.ToString();
                        writer.WriteLine(outPut);
                    }

                    writer.Close();

                }






                if (mobileAgentData)
                {
                    StreamWriter writer = new StreamWriter("@" + filePath + fileName);

                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {

                            string outPut = data2D[i, j].CountMobileAgents.ToString();


                            writer.WriteLine(outPut);


                        }
                    }

                    writer.Close();
                }




                if (clusterAgentData)
                {
                    StreamWriter writer = new StreamWriter("@" + filePath + fileName);

                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {

                            string outPut = data2D[i, j].CountClusterAgents.ToString();
                            writer.WriteLine(outPut);
                        }
                    }

                    writer.Close();
                }






            }







        }






        public static void ExportPointCloudToTxtFile(string filePath, string fileName, List<Agent> agentPopulation,  bool is3D, bool is2D, bool OrganizationPointCloud)
        {


            if (is3D)
            {
               

                if (OrganizationPointCloud)

                {


                    StreamWriter writer = new StreamWriter(filePath + fileName);

                    for (int i = 0; i < agentPopulation.Count; i++)
                    {

                        string outPut = agentPopulation[i].gameObject.transform.position.x.ToString() + "," + agentPopulation[i].gameObject.transform.position.z.ToString() + "," + agentPopulation[i].gameObject.transform.position.y.ToString();
                        writer.WriteLine(outPut);
                    }

                    writer.Close();



                }





            }// END 3D CONDITION


            if (is2D)
            {



                if (OrganizationPointCloud)

                {
                    StreamWriter writer = new StreamWriter(filePath + fileName);

                    for (int i = 0; i < agentPopulation.Count; i++)
                    {

                        string outPut = agentPopulation[i].gameObject.transform.position.x.ToString() + "," + agentPopulation[i].gameObject.transform.position.z.ToString();
                        writer.WriteLine(outPut);
                    }

                    writer.Close();

                }











            }







        }































    } //END CLASS

}
