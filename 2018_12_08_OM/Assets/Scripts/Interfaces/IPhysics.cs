using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPhysicsBehavior<T,U> {

    
    T IMaxSpeed { get; set; }
    T IMaxForce { get; set; }
    T IMass { get; set; }




    void IRunPhysics();


    void IResetForces();


    void IApplyForces(U force);
   
  
   


}
