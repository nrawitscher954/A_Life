using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

using SharpMatter.SharpMath;
using OrganizationalModel.Managers;
using OrganizationalModel.Export;
using OrganizationalModel.Behaviors;
using OrganizationalModel.Population;

namespace OrganizationalModel.ScalarFields
{

    public class ScalarField2D : MonoBehaviour
    {


        private Cell[,] scalarField;
        private float[,] scalarFieldValuesRules;
        public static  float[,] scalarFieldValuesProximity;
        public static float[,] scalarFieldHumanData;

        public static Cell[,,] scalarField3D;
        private float[,,] scalarField3DValues;

        public float resolution;
        public int columns, rows,height;
        private bool createFieldValues = false;

       
        // Use this for initialization
        void Start()
        {
            if (SimulationManager.Get().ScalarField2d)
            {
                scalarField = new Cell[columns, rows];
                scalarFieldValuesProximity = new float [columns, rows];
                scalarFieldHumanData = new float[columns, rows];
                scalarFieldValuesRules = new float[columns, rows];
                Init();
                SetName();

                string filePath1 = SimulationManager.Get().ScalarFieldValuesFilePath;
                string fileName1 = "FieldPointCloud" + ".txt";
                string fullPath1 = System.IO.Path.Combine(filePath1, fileName1);

                StreamWriter writer = new StreamWriter(fullPath1);

                //StreamWriter writer = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\" +
                //            "FieldPointCloud" + ".txt");

                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {

                        string outPut = scalarField[i, j].X.ToString() + "," + scalarField[i, j].Z.ToString() + "," + scalarField[i, j].Y.ToString();
                        writer.WriteLine(outPut);


                    }
                }

                writer.Close();

                string filePath2 = SimulationManager.Get().ScalarFieldValuesFilePath;
                string fileName2 = "CellName" + ".txt";
                string fullPath2 = System.IO.Path.Combine(filePath2, fileName2);

                StreamWriter writer2 = new StreamWriter(fullPath2);

                //StreamWriter writer1 = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\" +
                //          "CellName" + ".txt");

                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {

                        string outPut = scalarField[i, j].CellName.ToString();
                        writer2.WriteLine(outPut);


                    }
                }

                writer2.Close();
            }


            if (SimulationManager.Get().ScalarField3d)
            {
                scalarField3D = new Cell[columns, rows,height];
                scalarField3DValues = new float[columns, rows, height];
                Init3D();
                SetName3D();

                string filePath3 = SimulationManager.Get().ScalarFieldValuesFilePath;
                string fileName3 = "3DFieldPointCloud" + ".txt";
                string fullPath3 = System.IO.Path.Combine(filePath3, fileName3);

                StreamWriter writer3 = new StreamWriter(fullPath3);


                //StreamWriter writer = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\" +
                //            "3DFieldPointCloud" + ".txt");

                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {

                        for (int k = 0; k < height; k++)
                        {
                            string outPut = scalarField3D[i, j,k].X.ToString() + "," + scalarField3D[i, j, k].Z.ToString() + "," + scalarField3D[i, j, k].Y.ToString();
                            writer3.WriteLine(outPut);
                        }

                    


                    }
                }

                writer3.Close();



                string filePath4 = SimulationManager.Get().ScalarFieldValuesFilePath;
                string fileName4 = "CellName3D" + ".txt";
                string fullPath4 = System.IO.Path.Combine(filePath4, fileName4);

                StreamWriter writer4 = new StreamWriter(fullPath4);


                //StreamWriter writer1 = new StreamWriter(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\" +
                //          "CellName3D" + ".txt");

                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {

                        for (int k = 0; k < height; k++)
                        {
                            string outPut = scalarField3D[i, j,k].CellName.ToString();
                            writer4.WriteLine(outPut);
                        }

                        


                    }
                }

                writer4.Close();
            }



        }

        // Update is called once per frame
        void Update()
        {
            #region SCALAR FIELD 2D CONDITIONS
            if (SimulationManager.Get().ScalarField2d)
            {
                if (SimulationManager.Get().updateFieldRealTime)
                {
                    if (PeoplePopulation.peopleList.Count != 0) //&& createFieldValues == false) // People dont update
                    {


                        if (SimulationManager.Get().radialScalarField)
                        {
                            CreateDistanceField(PeoplePopulation.peopleList);
                            CreateEmotionField(PeoplePopulation.peopleList);
                        }

                        if (SimulationManager.Get().interpolatedScalarField) CreateColorValueField2D();

                        string filePath = SimulationManager.Get().ScalarFieldValuesFilePath;
                        string fileName = "ScalarFieldValuesProximity" + ".txt";
                        string fullPath = System.IO.Path.Combine(filePath, fileName);

                        StreamWriter writer = new StreamWriter(fullPath);

                        for (int i = 0; i < columns; i++)
                        {
                            for (int j = 0; j < rows; j++)
                            {



                                scalarFieldValuesProximity[i, j] = scalarField[i, j].ScalarValueProximity;
                                string outPut = scalarFieldValuesProximity[i, j].ToString();

                                writer.WriteLine(outPut);


                            }
                        }

                        writer.Close();



                        string filePath0 = SimulationManager.Get().ScalarFieldValuesFilePath;
                        string fileName0 = "ScalarFieldValuesHumanData" + ".txt";
                        string fullPath0 = System.IO.Path.Combine(filePath0, fileName0);

                        StreamWriter writer0 = new StreamWriter(fullPath);


                        for (int i = 0; i < columns; i++)
                        {
                            for (int j = 0; j < rows; j++)
                            {



                                scalarFieldValuesProximity[i, j] = scalarField[i, j].ScalarValueHumanData;
                                string outPut = scalarFieldHumanData[i, j].ToString();

                                writer0.WriteLine(outPut);


                            }
                        }

                        writer0.Close();






                        string filePath1 = SimulationManager.Get().ScalarFieldValuesFilePath;
                        string fileName1 = "ScalarFieldValuesRules" + ".txt";
                        string fullPath1 = System.IO.Path.Combine(filePath1, fileName1);

                        StreamWriter writer1 = new StreamWriter(fullPath1);



                        for (int i = 0; i < columns; i++)
                        {
                            for (int j = 0; j < rows; j++)
                            {



                                scalarFieldValuesRules[i, j] = scalarField[i, j].ScalarValueRules;
                                string outPut = scalarFieldValuesRules[i, j].ToString();

                                writer1.WriteLine(outPut);


                            }
                        }

                        writer1.Close();



                       

                    }

                }// END CONDITIONS UPDATE FIELD IN REAL TIME




                if (SimulationManager.Get().updateFieldRealTime ==false)
                {
                    if (PeoplePopulation.peopleList.Count != 0 && createFieldValues == false) 
                    {


                        if (SimulationManager.Get().radialScalarField)
                        {
                            CreateDistanceField(PeoplePopulation.peopleList);
                            CreateEmotionField(PeoplePopulation.peopleList);
                        }

                        if (SimulationManager.Get().interpolatedScalarField) CreateColorValueField2D();

                        string filePath = SimulationManager.Get().ScalarFieldValuesFilePath;
                        string fileName = "ScalarFieldValuesProximity" + ".txt";
                        string fullPath = System.IO.Path.Combine(filePath, fileName);

                        StreamWriter writer = new StreamWriter(fullPath);

                        for (int i = 0; i < columns; i++)
                        {
                            for (int j = 0; j < rows; j++)
                            {



                                scalarFieldValuesProximity[i, j] = scalarField[i, j].ScalarValueProximity;
                                string outPut = scalarFieldValuesProximity[i, j].ToString();

                                writer.WriteLine(outPut);


                            }
                        }

                        writer.Close();



                        string filePath0 = SimulationManager.Get().ScalarFieldValuesFilePath;
                        string fileName0 = "ScalarFieldValuesHumanData" + ".txt";
                        string fullPath0 = System.IO.Path.Combine(filePath0, fileName0);

                        StreamWriter writer0 = new StreamWriter(fullPath);


                        for (int i = 0; i < columns; i++)
                        {
                            for (int j = 0; j < rows; j++)
                            {



                                scalarFieldValuesProximity[i, j] = scalarField[i, j].ScalarValueHumanData;
                                string outPut = scalarFieldHumanData[i, j].ToString();

                                writer0.WriteLine(outPut);


                            }
                        }

                        writer0.Close();






                        string filePath1 = SimulationManager.Get().ScalarFieldValuesFilePath;
                        string fileName1 = "ScalarFieldValuesRules" + ".txt";
                        string fullPath1 = System.IO.Path.Combine(filePath1, fileName1);

                        StreamWriter writer1 = new StreamWriter(fullPath1);



                        for (int i = 0; i < columns; i++)
                        {
                            for (int j = 0; j < rows; j++)
                            {



                                scalarFieldValuesRules[i, j] = scalarField[i, j].ScalarValueRules;
                                string outPut = scalarFieldValuesRules[i, j].ToString();

                                writer1.WriteLine(outPut);


                            }
                        }

                        writer1.Close();






                        createFieldValues = true;


                    }

                }// END CONDITIONS DONT UPDATE FIELD IN REAL TIME











            }// END CONDITIONS SCALAR FIELD 2D

            #endregion

            if (SimulationManager.Get().ScalarField3d)
            {

                if (PeoplePopulation.peopleList.Count != 0 && createFieldValues == false) // People dont update
                {

                    CreateDistanceField3D();

                    string filePath1 = SimulationManager.Get().ScalarFieldValuesFilePath;
                    string fileName1 = "ScalarFieldValues3D" + ".txt";
                    string fullPath1 = System.IO.Path.Combine(filePath1, fileName1);

                    StreamWriter writer1 = new StreamWriter(fullPath1);


                    for (int i = 0; i < columns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {


                            for (int k = 0; k < height; k++)
                            {
                               
                                scalarField3DValues[i, j,k] = scalarField3D[i, j,k].ScalarValueRules;
                                string outPut = scalarField3DValues[i, j,k].ToString();
                                writer1.WriteLine(outPut);
                            }
                        }
                    }

                    writer1.Close();

                    createFieldValues = true;


                }



            }




        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="humans"></param>
        public void CreateDistanceField(List<GameObject> humans)
        {
            float maxSeparationDistance = 1.8f;
            for (int i = 0; i < columns; i++)
            {

                for (int j = 0; j < rows; j++)
                {

                   
                                                                          
                    float val = GetDistanceValues(scalarField[i, j], humans)* .1f; // smaller values smooth out the field. good values are between 0.02 and 0.05
                   if (val < 0.15) val = 0; // 0.30 --> original value used// When remap is used keep this commented out

                // float valRemap = SharpMath.Remap(val, 0.0f, 1.0f, maxSeparationDistance, 0.0f); // rempa is used here to inverse the distance relationship. Closet the ppl larger the value for the pixel

               //  if (valRemap < 0) valRemap = 0;
                  
                   scalarField[i, j].ScalarValueProximity = val;

                 //  scalarField[i, j].ScalarValueProximity = valRemap;
                }

            }
          
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="humans"></param>
        public void CreateEmotionField(List<GameObject> humans)
        {
            float maxSeparationDistance = 1.8f;
            for (int i = 0; i < columns; i++)
            {

                for (int j = 0; j < rows; j++)
                {


                    //.1f
                    float val = GetDistanceValues(scalarField[i, j], humans) * 0.05f;//* 0.022f; // smaller values smooth out the field. good values are between 0.02 and 0.05
                    if (val < 0.1) val = 0; // 0.30 --> original value used// When remap is used keep this commented out

                    // float valRemap = SharpMath.Remap(val, 0.0f, 1.0f, maxSeparationDistance, 0.0f); // rempa is used here to inverse the distance relationship. Closet the ppl larger the value for the pixel

                    //  if (valRemap < 0) valRemap = 0;

                    scalarField[i, j].ScalarValueHumanData = val;

                    //  scalarField[i, j].ScalarValueProximity = valRemap;
                }

            }

        }






        public void CreateDistanceField3D()
        {
            float scaleDense_Sparse = 0.05f;
            float scaleSparse_Dense = 0.02f;
            float maxSeparationDistance = 1.8f;
            for (int i = 0; i < columns; i++)
            {

                for (int j = 0; j < rows; j++)
                {

                    for (int k = 0; k < height; k++)
                    {
                       
                        float val = GetDistanceValues(scalarField3D[i, j,k], PeoplePopulation.peopleList)* scaleSparse_Dense;

                        ///REMAP IS USED HERE TO HAVE THE DISTANCE FIELD GRADIENT FROM LARGEST TO SMALLEST
                        
                        if (val < 0.30) val = 0; // When remap is used keep this commented out

                       // float valRemap = SharpMath.Remap(val, 0.0f, 1.0f, maxSeparationDistance, 0.0f);

                       //if (valRemap < 0) valRemap = 0;
                        scalarField3D[i, j,k].ScalarValueRules = val;

                      //   scalarField3D[i, j,k].ScalarValue = valRemap;

                    }



                }

            }

        }




        private void CreateColorValueField2D()
        {
            float [,] data = GetMeshColorValuesFromRhino();

           
            for (int i = 0; i < columns; i++)
            {

                for (int j = 0; j < rows; j++)
                {

                   
                    scalarField[i, j].ScalarValueRules = data[i,j];

                  
                }

            }
        }



   




        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="humans"></param>
        /// <returns></returns>
        private float GetDistanceValues(Cell cell, List<GameObject> humans)
        {
            List<float> distanceList = new List<float>();

            for (int i = 0; i < humans.Count; i++)
            {
               
                float distance = Vector3.Distance(cell.Position, humans[i].gameObject.transform.position);
                distanceList.Add(distance);
            }


            int smallestIndex = distanceList.IndexOf(distanceList.Min());

            return distanceList[smallestIndex];

        }

        /// <summary>
        /// Read exported values from meshField generated in rhino and store in a 2D array
        /// </summary>
        /// <returns></returns>
        private float[,] GetMeshColorValuesFromRhino()
        {
            
            float [,] dataArray = new float[columns, rows];
            // string[] lines = System.IO.File.ReadAllLines(@"C:\Users\nicol\Documents\Architecture\1.AADRL\Term 4\Data\SimulationData\2D\ScalarField\ToUnity\ColorMapValues.txt");
            string[] lines = System.IO.File.ReadAllLines(SimulationManager.Get().ImportToUnityInterpolatedFieldFilePath);
            var linesArray = Make2DArray(lines, columns, rows);

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    dataArray[i, j] = float.Parse(linesArray[i, j]);

                }
            }
            return dataArray;
        }


        private string[,] Make2DArray(string[] input, int height, int width)
        {
            string[,] output = new string[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }


        public void Init()
        {

            for (int i = 0; i <columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {

                   
                    float x = i* resolution;
                    float z = j * resolution;
                    scalarField[i, j] = new Cell(x, 0, z);

                }
            }

        }


        public void Init3D()
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
                        scalarField3D[i, j,k] = new Cell(x, y, z);
                    }          

                }
            }

        }

        /// <summary>
        /// returns current Cell
        /// </summary>
        /// <param name="lookup"></param>
        /// <returns></returns>
        public Cell Lookup2D(Vector3 lookup)
        {

            int column = (int)(SharpMath.Constrain(lookup.x / resolution, 0, columns - 1));
            int row = (int)(SharpMath.Constrain(lookup.z / resolution, 0, rows - 1));

            return scalarField[column, row];
        }

        public Cell Lookup3D(Vector3 lookup)
        {

            int column = (int)(SharpMath.Constrain(lookup.x / resolution, 0, columns - 1));
            int row = (int)(SharpMath.Constrain(lookup.z / resolution, 0, rows - 1));
            int _height = (int)(SharpMath.Constrain(lookup.y / resolution, 0, height - 1));

            return scalarField3D[column, row,_height];
        }

        private void SetName()
        {

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {

                    scalarField[i, j].CellName = "Cell" + " " + i + "," + j;

                }

            }
        }


        private void SetName3D()
        {

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {

                    for (int k = 0; k < height; k++)
                    {
                        scalarField3D[i, j,k].CellName = "Cell" + " " + i + "," + j +"," + k;
                    }

                  

                }

            }
        }





    }

}
