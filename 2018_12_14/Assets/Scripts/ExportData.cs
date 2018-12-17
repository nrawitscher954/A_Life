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
        public static void ExportPointCloudToTxtFile(string filePath, Pixel[,,] data3D, Pixel[,] data2D, KdTree<Agent> population3D, string fileName, int columns, int rows, int height, bool is3D, bool is2D,
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
                                string outPut = data3D[i, k, j].X.ToString() + "," + data3D[i, k, j].Z.ToString() + "," + data3D[i, k, j].Y.ToString();

                             
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


        public static void ExportStateHistoryData(string filePath, string fileName, KdTree<Agent> agentPopulation, bool freezed, bool deActivated, bool emmiter, bool receiver)
        {
            if (freezed)
            {
                StreamWriter writer = new StreamWriter(filePath + fileName);

                for (int i = 0; i < agentPopulation.Count; i++)
                {
                    if (agentPopulation[i].gameObject.tag != "Cancelled")
                    {
                        string outPut = agentPopulation[i].freezedHistoryCount.ToString();
                        writer.WriteLine(outPut);
                    }

                }

                writer.Close();
            }



            if (deActivated)
            {
                StreamWriter writer = new StreamWriter(filePath + fileName);

                for (int i = 0; i < agentPopulation.Count; i++)
                {
                    if (agentPopulation[i].gameObject.tag != "Cancelled")
                    {
                        string outPut = agentPopulation[i].deActivatedHistoryCount.ToString();
                        writer.WriteLine(outPut);
                    }

                }

                writer.Close();
            }



            if (emmiter)
            {
                StreamWriter writer = new StreamWriter(filePath + fileName);

                for (int i = 0; i < agentPopulation.Count; i++)
                {
                    if (agentPopulation[i].gameObject.tag != "Cancelled")
                    {
                        string outPut = agentPopulation[i].emmiterHistoryCount.ToString();
                        writer.WriteLine(outPut);
                    }

                }

                writer.Close();
            }



            if (receiver)
            {
                StreamWriter writer = new StreamWriter(filePath + fileName);

                for (int i = 0; i < agentPopulation.Count; i++)
                {
                    if (agentPopulation[i].gameObject.tag != "Cancelled")
                    {
                        string outPut = agentPopulation[i].receiverHistoryCount.ToString();
                        writer.WriteLine(outPut);
                    }

                }

                writer.Close();
            }






        }



        public static void ExportPointCloudToTxtFile(string filePath, string fileName, KdTree<Agent> agentPopulation,  bool is3D, bool is2D, bool OrganizationPointCloud)
        {


            if (is3D)
            {
               

                if (OrganizationPointCloud)

                {


                    StreamWriter writer = new StreamWriter(filePath + fileName);


                   

                    for (int i = 0; i < agentPopulation.Count; i++)
                    {
                        if (agentPopulation[i].gameObject.tag != "Cancelled")
                        {
                            string outPut = agentPopulation[i].gameObject.transform.position.x.ToString() + "," + agentPopulation[i].gameObject.transform.position.z.ToString() + "," + agentPopulation[i].gameObject.transform.position.y.ToString();
                            writer.WriteLine(outPut);
                        }
                        
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
                        if (agentPopulation[i].gameObject.tag != "Cancelled")
                        {
                            string outPut = agentPopulation[i].gameObject.transform.position.x.ToString() + "," + agentPopulation[i].gameObject.transform.position.z.ToString();
                            writer.WriteLine(outPut);
                        }
                    }

                    writer.Close();

                }











            }







        }






        public static void ExportPointCloudToMultipleTxtFiled(string filePath, KdTree<Agent> agentPopulation, bool is3D, bool is2D, bool OrganizationPointCloud)
        {


            if (is3D)
            {


                if (OrganizationPointCloud)

                {



                    for (int i = 0; i < agentPopulation.Count; i++)
                    {
                       
                        if (agentPopulation[i].gameObject.tag != "Cancelled" && agentPopulation[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().trailData.Count != 0)
                        {

                          
                            print("export");
                            StreamWriter writer = new StreamWriter(filePath + "Organization" + agentPopulation[i].name + ".txt");
                                string outPut = agentPopulation[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().trailData[0].ToString("0.00#") + System.Environment.NewLine +
                                    agentPopulation[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().trailData[agentPopulation[i].GetComponent<OrganizationalModel.Behaviors.OrganizationBehavior>().trailData.Count-1].ToString("0.00#");
                           string outPutFormated =  outPut.Replace("(", "").Replace(")", ""); // remove Parenthesis
                            writer.WriteLine(outPutFormated);
                                writer.Close();
                           
                        }

                    }

                  



                }





            }// END 3D CONDITION


            if (is2D)
            {



                if (OrganizationPointCloud)

                {
                    //StreamWriter writer = new StreamWriter(filePath + fileName);

                    //for (int i = 0; i < agentPopulation.Count; i++)
                    //{
                    //    if (agentPopulation[i].gameObject.tag != "Cancelled")
                    //    {
                    //        string outPut = agentPopulation[i].gameObject.transform.position.x.ToString() + "," + agentPopulation[i].gameObject.transform.position.z.ToString();
                    //        writer.WriteLine(outPut);
                    //    }
                    //}

                    //writer.Close();

                }











            }







        }

























    } //END CLASS

}
