using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpMatter.SharpMath;
using System.IO;

using OrganizationalModel.Managers;
using OrganizationalModel.Export;
using OrganizationalModel.Behaviors;

namespace OrganizationalModel.Population
{
    public class PixelPopulation : MonoBehaviour
    {
        public Pixel[,] pixel2DArray; // create 2d Array to store flow vectors
        public Pixel[,,] pixel3DArray;

        public float resolution;
        public int columns, rows, height;
        public bool is3D;
        public bool is2D;

        // Use this for initialization
        void Start()
        {


            if (is2D) pixel2DArray = new Pixel[columns, rows];

            if (is3D) pixel3DArray = new Pixel[columns, rows, height];

            Init();
            Name();


            if (is3D)
            {
                bool exportArrayPointCloud = true;
                bool exportOrganizationPointcloud = false;
                bool exportMobileAgentData = false;
                bool exportClusterAgentData = false;
                string filePath = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Summer\Spyropolous Studio\Data\SimulationData\3D\PixelArray\";
                string fileName = "PixelArray.txt";

                ExportData.ExportPointCloudToTxtFile(filePath, pixel3DArray, pixel2DArray, AgentPopulation.populationList,fileName,columns,rows,height,is3D,is2D, exportArrayPointCloud, exportOrganizationPointcloud,
                    exportMobileAgentData, exportClusterAgentData);
            }

        }


      
        // Update is called once per frame
        void Update()
        {

            if (GenerationManager.generationChange)
            {
                if (is3D)
                {


                    bool exportArrayPointCloud = false;
                    bool exportOrganizationPointcloud = false;
                    bool exportMobileAgentData = true;
                    bool exportClusterAgentData = false;
                    string filePath = @"C:\Users\nicol\Documents\Architecture\1.AADRL\Summer\Spyropolous Studio\Data\SimulationData\3D\PixelData\MobileAgents\";
                    string fileName = "PixelDataModibleAgents" + GenerationManager.generationCount.ToString() + ".txt";

                    ExportData.ExportPointCloudToTxtFile(filePath, pixel3DArray, pixel2DArray, AgentPopulation.populationList, fileName, columns, rows, height, is3D, is2D, exportArrayPointCloud, exportOrganizationPointcloud,
                        exportMobileAgentData, exportClusterAgentData);






                    StreamWriter writer2 = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Summer\Spyropolous Studio\Data\SimulationData\3D\PixelData\ClusterAgents\" +
                        "ClusterAgents" + GenerationManager.generationCount.ToString() + ".txt");

                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            for (int k = 0; k < height; k++)
                            {
                                string outPut = pixel3DArray[i, j, k].CountClusterAgents.ToString();


                                writer2.WriteLine(outPut);

                            }
                        }
                    }

                    writer2.Close();



                }
            }

        }


        /// <summary>
        /// Initialize 2D vector array
        /// </summary>
        public void Init()
        {

            if (is2D)
            {

                for (int i = 0; i < columns; i++)
                {


                    for (int j = 0; j < rows; j++)
                    {


                        float x = i * resolution;
                        float z = j * resolution;
                        pixel2DArray[i, j] = new Pixel(x, 0, z);




                    }


                }
            }

            if (is3D)
            {


                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {

                        for (int k = 0; k < height; k++)
                        {
                            float x = i * resolution;
                            float z = j * resolution;
                            float y = k * resolution;

                            pixel3DArray[i, j, k] = new Pixel(x, z, y);


                        }

                    }

                }


            }

        }





        /// <summary>
        /// returns current pixel
        /// </summary>
        /// <param name="lookup"></param>
        /// <returns></returns>
        public Pixel Lookup2D(Vector3 lookup)
        {

            int column = (int)(SharpMath.Constrain(lookup.x / resolution, 0, columns - 1));
            int row = (int)(SharpMath.Constrain(lookup.y / resolution, 0, rows - 1));

            return pixel2DArray[column, row];
        }


        /// <summary>
        /// returns current pixel 
        /// </summary>
        /// <param name="lookup"></param>
        /// <returns></returns>
        public Pixel Lookup3D(Vector3 agentPosition)
        {
            int column = (int)(SharpMath.Constrain(agentPosition.x / resolution, 0, columns - 1));
            int row = (int)(SharpMath.Constrain(agentPosition.y / resolution, 0, rows - 1));
            int _height = (int)(SharpMath.Constrain(agentPosition.z / resolution, 0, height - 1));


            return pixel3DArray[column, row, _height];
        }








        private void Name()
        {
            if (is2D)
            {
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {

                        pixel2DArray[i, j].PixelName = "Pixel" + " " + i + "," + j;

                    }

                }
            }


            if (is3D)
            {
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        for (int k = 0; k < height; k++)
                        {

                            pixel3DArray[i, j, k].PixelName = "Pixel" + " " + i + "," + j + "," + k;

                        }


                    }

                }
            }


        }




    } //END CLASS

}
