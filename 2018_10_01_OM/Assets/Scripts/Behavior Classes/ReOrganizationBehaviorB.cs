using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ClassExtensions;

using SharpMatter.SharpMath;
using SharpMatter.SharpDataStructures;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;
namespace OrganizationalModel.Behaviors
{
    public partial class ReOrganizationBehavior : Agent, IReOrganization<Agent, float>
    {


        #region STATE RULES

        /// <summary>
        /// 
        /// Rules are only executed once!
        /// </summary>
        /// 

        public void IRulesDiscrete()
        {


            if (!GenerationManager.generationChange && GenerationManager.generationCount != 0 && calculateRulesDiscreteCoroutine == false && AgentPopulation.migrateCluster == false)
            {

                IChangeStateToSignalEmmiter();
                IChangeStateToSignalReceiverPassive();
                IChangeStateToDeactivated();
                ChangeStateToFreezedOnNeighbourCount();


                calculateRulesDiscreteCoroutine = true;

            }


        }

        /// <summary>
        /// Rules are calculated every frame per second
        /// </summary>
        public void IRulesContinuous()
        {


            /// Runs rules on all Generations except  Generation_0
            if (!GenerationManager.generationChange && GenerationManager.generationCount != 0 && AgentPopulation.migrateCluster == false)
            {
                ChangeStateToFreezedOnCollision();
                ChangeStateToSignalReceiver();


            }

            if (!GenerationManager.generationChange && GenerationManager.generationCount != 0 && AgentPopulation.migrateCluster == true)
            {
                ChangeStateToWanderer();
                //AgentPopulation.migrateCluster = false;
            }

        }








        /// <summary>
        /// 
        /// </summary>
    



        /// <summary>
        /// 
        /// </summary>
        public void IChangeStateToSignalEmmiter()
        {


            if (GenerationManager.generationCount != 0)
            {

                

                if (base.neighbours.Count == 0 && base.energyLevel < 0.3f)
                {
                    this.gameObject.tag = "SignalEmmiter";

                    if (AP.displayColorbyEnergyLevels)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                    }

                    if (AP.displayColorByState)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }

                }




                if (base.neighbours.Count == 1 && base.energyLevel < 0.3f)
                {
                    this.gameObject.tag = "SignalEmmiter";

                    if (AP.displayColorbyEnergyLevels)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                    }

                    if (AP.displayColorByState)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }

                }

                /////////////////////////////FOR 6 N /////////////////////////////FOR 6 N /////////////////////////////FOR 6 N

                if (base.neighbours.Count == 2)
                {
                    this.gameObject.tag = "SignalEmmiter";

                    if (AP.displayColorbyEnergyLevels)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                    }

                    if (AP.displayColorByState)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }

                }




                if (base.neighbours.Count == 3)
                {
                    this.gameObject.tag = "SignalEmmiter";
                    if (AP.displayColorbyEnergyLevels)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                    }

                    if (AP.displayColorByState)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }

                }


                /////////////////////////////FOR 6 N /////////////////////////////FOR 6 N /////////////////////////////FOR 6 N


                /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N

                if (base.neighbours.Count == 4)
                {
                    this.gameObject.tag = "SignalEmmiter";
                    if (AP.displayColorbyEnergyLevels)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                    }

                    if (AP.displayColorByState)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }

                }

              

                if (base.neighbours.Count == 5)
                {
                    this.gameObject.tag = "SignalEmmiter";
                    if (AP.displayColorbyEnergyLevels)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                    }

                    if (AP.displayColorByState)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }

                }
                /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N




                //if (this.gameObject.tag == "SignalReceiver" && base.energyLevel < .30f && base.localCollisionHistory >= 1)
                //{

                //    this.gameObject.tag = "SignalEmmiter";

                //    if (AP.displayColorbyEnergyLevels)
                //    {
                //        this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                //    }

                //    if (AP.displayColorByState)
                //    {
                //        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                //    }
                //}


            }
        }






        /// <summary>
        /// 
        /// </summary>
        /// 
        private void ChangeStateToSignalReceiver()
        {
            Color color = Utility.RGBToFloat(63, 191, 191);
            if (this.gameObject.tag == "SignalReceiverPassive")
            {
                //////////////////////////////////////////////////////// 
                int threshhold = 200; // hard coded number

           
                this.gameObject.GetComponent<MeshRenderer>().enabled = true;
                this.gameObject.GetComponent<SphereCollider>().enabled = true;


                ////////////////////////////////////////////////////////

                if (this.signalReceiverPassiveCounter > threshhold && this.changeStateToSignalReceiverCoroutine == false)
                {

                    this.gameObject.tag = "SignalReceiver";
              



                    if (AP.displayColorbyEnergyLevels)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                    }

                    if (AP.displayColorByState)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = color;
                    }

                    base.signalReceiverPassiveCounter = 0;
                    this.changeStateToSignalReceiverCoroutine = true;
                }
            }
        }



        public void IChangeStateToSignalReceiverPassive()
        {
            Color color = Utility.RGBToFloat(63, 191, 191);
            //if (base.neighboursTemp.Count != 3)
           // {


                if (GenerationManager.generationCount != 0)
                {

                    if (base.neighbours.Count == 1  && base.energyLevel > 0.3f)
                    {

                        this.gameObject.tag = "SignalReceiverPassive";

                    this.neighbours.Clear();
                    base.neighboursTemp.Clear();

                    if (AP.displayColorbyEnergyLevels)
                        {
                            this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                        }

                        if (AP.displayColorByState)
                        {
                            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                        }


                    }


                    if (base.neighbours.Count == 0 && base.energyLevel > 0.3f)
                    {

                        this.gameObject.tag = "SignalReceiverPassive";

                        this.neighbours.Clear();
                        base.neighboursTemp.Clear();



                    if (AP.displayColorbyEnergyLevels)
                        {
                            this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                        }

                        if (AP.displayColorByState)
                        {
                            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                        }

                    }
                }

           // }


        }/// METHOD END















        /// <summary>
        /// 
        /// </summary>
        /// 
        private void ChangeStateToFreezedOnNeighbourCount()

        {
            if (GenerationManager.generationCount != 0)
            {
                /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N
                if (base.neighbours.Count == 6)
                {

                    this.gameObject.tag = "Freezed";


                    if (AP.displayColorbyEnergyLevels)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                    }

                    if (AP.displayColorByState)
                    {
                        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.grey;
                    }


                }

                /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N


            }



        }





        /// <summary>
        /// 
        /// </summary>
        /// 
        public void IChangeStateToDeactivated()
        {
            /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N
            if (base.neighbours.Count >= 7)
            {

                this.gameObject.tag = "De-Activated";


                if (AP.displayColorbyEnergyLevels)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                }

                if (AP.displayColorByState)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
                }
            }

            /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N


            /////////////////////////////FOR 6 N /////////////////////////////FOR 6 N /////////////////////////////FOR 6 N
            //if (base.neighbours.Count >= 5)
            //{

            //    this.gameObject.tag = "De-Activated";


            //    if (AP.displayColorbyEnergyLevels)
            //    {
            //        this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
            //    }

            //    if (AP.displayColorByState)
            //    {
            //        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
            //    }
            //}

            /////////////////////////////FOR 6 N /////////////////////////////FOR 6 N /////////////////////////////FOR 6 N
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private void ChangeStateToFreezedOnCollision()
        {

            if (this.gameObject.tag == "SignalReceiver" && base.localCollisionHistory >= 1 && base.energyLevel > 0.15f) /// base.localCollisionHistory ==1 this condition only had this
            {
                this.gameObject.tag = "Freezed";



                if (AP.displayColorbyEnergyLevels)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                }

                if (AP.displayColorByState)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = Color.grey;
                }



            }

            /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N /////////////////////////////FOR 12 N

            else if (this.gameObject.tag == "SignalEmmiter" && base.neighbours.Count >= 12) // 6 FOR 6 NEIGHBOURS
            {
                this.gameObject.tag = "Freezed";



                if (AP.displayColorbyEnergyLevels)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                }

                if (AP.displayColorByState)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = Color.grey;
                }

            }


            else if (this.gameObject.tag == "SignalReceiver" && this.signalReceiverCounter > 1100)
            {


                this.gameObject.tag = "Freezed";



                if (AP.displayColorbyEnergyLevels)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                }

                if (AP.displayColorByState)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = Color.grey;
                }

            }
        }




        /// <summary>
        /// 
        /// </summary>
        public void ChangeStateToWanderer()
        {

            {
                Color color = Utility.RGBToFloat(63, 191, 191);
                this.gameObject.tag = "Wanderer";

                if (AP.displayColorbyEnergyLevels)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = base.EnergyColor(AP.startColor, AP.endColor, base.energyLevel, 0f);
                }

                if (AP.displayColorByState)
                {
                    this.gameObject.GetComponent<MeshRenderer>().material.color = color;
                }
            }
        }

        #endregion

    } // END CLASS
    

} // END NAMESPACE
