using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrganizationalModel.UtilityFunctions;
using OrganizationalModel.BaseClass;
using OrganizationalModel.Population;
using OrganizationalModel.Managers;
using OrganizationalModel.Behaviors;

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
    public class FlockBehavior : MonoBehaviour
    {




        ////////////////////////////////METHODS////////////////////////////////////////////////

         Vector3 velocity;
      
        Vector3 acceleration = Vector3.zero;

        float mass = 1.0f;

        float IMaxSpeed = 0.07f;
        float IMaxForce = 0.07f;

        public float springStrength;
        public float springDamper;

        public float visionRadius;
        public float coheshionStrngth;
        public float allignmntStrength;
        public float separationStrength;

        public void Start()
        {
         velocity = Random.onUnitSphere * IMaxSpeed;
        }

        public void Update()
        {
            List<GameObject> neighbours = FindNeighbours(this.gameObject, visionRadius, BoidPopulation.populationList);

            if (neighbours.Count != 0)
            {
                IApplyForces(Cohesion(neighbours, coheshionStrngth));
                IApplyForces(Allignment(neighbours, allignmntStrength));
            }

            else
                this.gameObject.transform.position += Vector3.zero;


           // IRunPhysics();

            
        }

        private void FixedUpdate()
        {
            IRunPhysics();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="cohesionStrength"></param>
        public Vector3 Cohesion(List<GameObject> neighbours, float cohesionStrength)
        {

            Vector3 centre = Vector3.zero;

            foreach (GameObject neighbour in neighbours)
                centre += neighbour.transform.position;

            // We divide by the number of neighbours to actually get their centre of mass
            centre /= neighbours.Count;

            Vector3 cohesion = centre - this.gameObject.transform.position; ;


            Vector3 cohesionForce = cohesionStrength * cohesion;

            return cohesionForce;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="allignmentStrength"></param>
        public Vector3 Allignment(List<GameObject> neighbours, float allignmentStrength)
        {
            Vector3 alignment = Vector3.zero;

            foreach (GameObject neighbour in neighbours)
                alignment += neighbour.GetComponent<FlockBehavior>().velocity;

            // We divide by the number of neighbours to actually get their average velocity
            alignment /= neighbours.Count;
            Vector3 alignmentStrengtForce = allignmentStrength * alignment;
            return alignmentStrengtForce;
        }



        public List<GameObject> FindNeighbours(GameObject agent, double visionRadius, List<GameObject> population)
        {
            List<GameObject> neighbours = new List<GameObject>();

            foreach (GameObject neighbour in population)
            {

                if (neighbour != agent && Vector3.Distance(neighbour.transform.position, agent.transform.position) < visionRadius)
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="neighbours"></param>
        /// <param name="separationStrength"></param>
        public Vector3 Separation(List<GameObject> neighbours, float separationStrength)
        {
            Vector3 separation = Vector3.zero;
            Vector3 separationForce = Vector3.zero;

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
                    separationForce = separationStrength * separation;

                    
                   

                }

            }

            return separationForce;

        }



        public void IRunPhysics()
        {
           // acceleration *= IMaxForce;
            this.velocity += this.acceleration;
            this.velocity.Normalize();
            this.velocity *= this.IMaxSpeed;

        this.gameObject.transform.position += this.velocity;

     // this.gameObject.GetComponent<Rigidbody>().AddForce(this.velocity);


            this.IResetForces();
        }

        public void IResetForces()
        {
            this.acceleration *= 0.0f;
        }

        public void IApplyForces(Vector3 force)
        {

            this.acceleration += force / this.mass;

        }

        private void OnCollisionEnter(Collision collision)
        {
            AddSpringJoint(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            Destroy(this.gameObject.GetComponent<SpringJoint>());
        }


        private void AddSpringJoint(Collision data)
        {

            SpringJoint joint = gameObject.AddComponent<SpringJoint>();
            joint.connectedBody = data.rigidbody;
            gameObject.GetComponent<SpringJoint>().spring = springStrength;
            gameObject.GetComponent<SpringJoint>().damper = springDamper;
            


        }






    }

}
