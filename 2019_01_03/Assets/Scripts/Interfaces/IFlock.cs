using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IFlock<T,V> {


    //void Flock(List<T> neighbours, V cohesionStrength, V allignmentStrength);
    void Flock(List<T> neighbours);
    void Cohesion(List<T> neighbours,V cohesionStrength);
    void Allignment(List<T> neighbours, V allignmentStrength);
    void Separation(List<T> neighbours, V separationStrength);
}
