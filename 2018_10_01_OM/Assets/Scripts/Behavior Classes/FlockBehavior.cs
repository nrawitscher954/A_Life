using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Population;
using ClassExtensions;
using SharpMatter.SharpMath;
////////////////////////////////CLASS DESCRIPTION////////////////////////////////
/// <summary>
/// 
/// 
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
/// 
/// 
/// 
/// 
/// 
/// </summary>

namespace OrganizationalModel.Behaviors
{
    public class FlockBehavior : Agent, IFlock<Agent, float>
    {

        //   GameObject agentPopulation;
        //  private AgentPopulation AP;

        Rigidbody rb;

        // Use this for initialization
        void Start()
        {


            base.IMaxForce = 1.7f;
            base.IMaxSpeed = 1.7f;
            rb = GetComponent<Rigidbody>();
            rb.velocity = base.velocity;
            rb.mass = base.IMass = 1;


            //   agentPopulation = GameObject.Find("AgentPopulation"); 
            //   AP = agentPopulation.GetComponent<AgentPopulation>();

        }

        int counter = 0;

        // Update is called once per frame
        void Update()
        {
            if (AgentPopulation.flock)
            {


                Bounds();
                ColorNeighbours();
                //DrawTolopogy();

                this.gameObject.GetComponent<Collider>().isTrigger = false;
                rb.isKinematic = false;
                //  if( AP.is2D) rb.constraints = RigidbodyConstraints.FreezePositionY;

                List<Agent> neighbours = base.FindNeighbours(this.gameObject.GetComponent<Agent>(), 2.7f, AgentPopulation.populationList);



                if (neighbours.Count == 0)
                {
                    base.velocity += Vector3.zero;
                }
                else
                {
                    Flock(neighbours);
                    CheckConnectedNeighbours();
                    if (base.neighbourHistory.Count != 0) base.mostCommonAgentInConnectionHistory = base.neighbourHistory.MostCommon();

                }

                base.IRunPhysics();

            }



        }

        ////////////////////////////////METHODS////////////////////////////////////////////////

        private void Bounds()
        {
            float maxX = 70;
            float maxY = 70;
            float maxZ = 70;



            if (this.gameObject.transform.position.x < 0)
                base.IApplyForces(new Vector3(-this.gameObject.transform.position.x, 0, 0));
            else if (this.gameObject.transform.position.x > maxX)
                base.IApplyForces(new Vector3(maxX - this.gameObject.transform.position.x, 0, 0));

            if (this.gameObject.transform.position.y < 0)
                base.IApplyForces(new Vector3(0, -this.gameObject.transform.position.y, 0));
            else if (this.gameObject.transform.position.y > maxY)
                base.IApplyForces(new Vector3(0, maxY - this.gameObject.transform.position.y, 0));

            if (this.gameObject.transform.position.z < 0)
                base.IApplyForces(new Vector3(0, -this.gameObject.transform.position.z, 0));
            else if (this.gameObject.transform.position.z > maxZ)
                base.IApplyForces(new Vector3(0, 0, maxZ - this.gameObject.transform.position.z));


        }


        public void Flock(List<Agent> neighbours)
        {
            Cohesion(neighbours, 15f);
            Allignment(neighbours, 10f);
            //Separation(neighbours, 0.1f);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="cohesionStrength"></param>
        public void Cohesion(List<Agent> neighbours, float cohesionStrength)
        {

            Vector3 centre = Vector3.zero;

            foreach (Agent neighbour in neighbours)
                centre += neighbour.transform.position;

            // We divide by the number of neighbours to actually get their centre of mass
            centre /= neighbours.Count;

            Vector3 cohesion = centre - this.gameObject.transform.position; ;


            Vector3 cohesionForce = cohesionStrength * cohesion;

            base.IApplyForces(cohesionForce);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="allignmentStrength"></param>
        public void Allignment(List<Agent> neighbours, float allignmentStrength)
        {
            Vector3 alignment = Vector3.zero;

            foreach (Agent neighbour in neighbours)
                alignment += base.velocity;

            // We divide by the number of neighbours to actually get their average velocity
            alignment /= neighbours.Count;
            Vector3 alignmentStrengtForce = allignmentStrength * alignment;
            base.IApplyForces(alignmentStrengtForce);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="separationStrength"></param>
        public void Separation(List<Agent> neighbours, float separationStrength)
        {
            Vector3 separation = new Vector3(0, 0, 0);

            for (int i = 0; i < neighbours.Count; i++)
            {
                float distanceToNeighbour = Vector3.Distance(this.gameObject.transform.position, neighbours[i].transform.position);

                if (distanceToNeighbour <= 1.2f)
                {
                    Vector3 getAway = transform.position - neighbours[i].gameObject.transform.position;

                    // scale get away vector by inverse of distance to its neighbour
                    // this makes the get away vector larger as the agent gets closer to its neighbour
                    separation += getAway / (getAway.magnitude * distanceToNeighbour);
                    separation.Normalize();
                    Vector3 separationForce = separationStrength * separation;
                    base.IApplyForces(separationForce);

                }

            }
        }






        bool hasJoint;

        void OnCollisionEnter(Collision collision)
        {

            AddFixedJoint(collision);
            //AddSpringJoint(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            var container = this.gameObject.GetComponent<FixedJoint>();
            Destroy(container);

            var container2 = this.gameObject.GetComponent<SpringJoint>();
            Destroy(container2);
        }

        private void AddFixedJoint(Collision data)
        {
            if (data.gameObject.GetComponent<Rigidbody>() != null && !hasJoint)
            {
                gameObject.AddComponent<FixedJoint>();
                gameObject.GetComponent<FixedJoint>().connectedBody = data.rigidbody;
                hasJoint = true;

            }


            base.neighbourHistory.Add(data.gameObject.GetComponent<Agent>());
        }


        private void AddSpringJoint(Collision data)
        {
            if (data.gameObject.GetComponent<Rigidbody>() != null && !hasJoint)
            {
                gameObject.AddComponent<SpringJoint>();
                gameObject.GetComponent<SpringJoint>().connectedBody = data.rigidbody;
                gameObject.GetComponent<SpringJoint>().spring = 100;
                gameObject.GetComponent<SpringJoint>().damper = 10;

                gameObject.GetComponent<SpringJoint>().enableCollision = true;
                hasJoint = true;

            }

            base.neighbourHistory.Add(data.gameObject.GetComponent<Agent>());
        }


        public void CheckConnectedNeighbours()
        {
            base.neighboursTemp = base.FindNeighbours(this.GetComponent<Agent>(), this.gameObject.GetComponent<SphereCollider>().radius * 2.05f, AgentPopulation.populationList);

            for (int i = 0; i < base.neighboursTemp.Count; i++)
            {
                float distance = Vector3.Distance(base.neighboursTemp[i].gameObject.transform.position, this.gameObject.transform.position);
                if (distance > this.gameObject.GetComponent<SphereCollider>().radius * 2.05f)
                {
                    base.neighboursTemp.RemoveAt(i);
                }
            }
        }






        private void OnDrawGizmos()
        {



            for (int i = 0; i < base.neighboursTemp.Count; i++)
            {
                Gizmos.color = Color.white;
                Vector3 dir = base.neighboursTemp[i].transform.position - this.gameObject.transform.position;
                Gizmos.DrawRay(this.gameObject.transform.position, dir);

            }


            Gizmos.color = Color.yellow;
            Vector3 dir2 = base.velocity;
            dir2.Normalize();
            dir2 *= VisionRadius;
            Gizmos.DrawRay(this.gameObject.transform.position, dir2);


        }

        bool hasLineRenderer;
        private void DrawTolopogy()
        {
            if (!hasLineRenderer)
            {
                this.gameObject.AddComponent<LineRenderer>();
                hasLineRenderer = true;
            }

            Vector3[] pos = new Vector3[base.neighboursTemp.Count];

            this.gameObject.GetComponent<LineRenderer>().startWidth = 0.1f;
            this.gameObject.GetComponent<LineRenderer>().endWidth = 0.1f;

            this.gameObject.GetComponent<LineRenderer>().material.color = Color.white;
            for (int i = 0; i < base.neighboursTemp.Count; i++)
            {
                pos[i] = (base.neighboursTemp[i].transform.position);

            }
            this.gameObject.GetComponent<LineRenderer>().SetPositions(pos);



        }




        private void ColorNeighbours()
        {
            //Color young = Utility.RGBToFloat(219, 0, 40);
            Color young = Utility.RGBToFloat(180, 0, 10);
            Color old = Utility.RGBToFloat(255, 130, 160);



            //float color = neighbours.Count;
            int neighbourcount = base.neighboursTemp.Count;
            float remap = SharpMath.Remap(neighbourcount, 0, neighbourcount, 0, 1);
            for (int i = 0; i < base.neighboursTemp.Count; i++)
            {
                base.neighboursTemp[i].GetComponent<MeshRenderer>().material.color = Color.Lerp(old, young, neighbourcount * 0.4f);
            }

        }



    }///END INTERFACE

}
