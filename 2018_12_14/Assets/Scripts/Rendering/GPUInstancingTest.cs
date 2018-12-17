using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUInstancingTest : MonoBehaviour
{

    public GameObject prefab;

    public int instances = 5000;

    public float radius = 50f;

    List<GameObject> test = new List<GameObject>();
    void Start()
    {

        MaterialPropertyBlock properties = new MaterialPropertyBlock();
        //for (int i = 0; i < instances; i++) {
        //	Transform t = Instantiate(prefab);
        //	t.localPosition = Random.insideUnitSphere * radius;
        //	t.SetParent(transform);

        //         // properties.SetColor("_Color", new Color(Random.value, Random.value, Random.value));

        //         // properties.SetColor("_Color", new Color(.7f, .7f, .7f));


        //          MeshRenderer r = t.GetComponent<MeshRenderer>();
        //	if (r) {
        //		r.SetPropertyBlock(properties);
        //	}
        //	//else {
        //	//	for (int ci = 0; ci < t.childCount; ci++) {
        //	//		r = t.GetChild(ci).GetComponent<MeshRenderer>();
        //	//		if (r) {
        //	//			r.SetPropertyBlock(properties);
        //	//		}
        //	//	}
        //	//}
        //}

        for (int i = 0; i < System.Math.Ceiling(Mathf.Pow(instances, 1.0f / 3.0f)); i++)
        {

            for (int j = 0; j < System.Math.Ceiling(Mathf.Pow(instances, 1.0f / 3.0f)); j++)
            {

                for (int k = 0; k < System.Math.Ceiling(Mathf.Pow(instances, 1.0f / 3.0f)); k++)
                {

                    Vector3 distributionArea = new Vector3(i * 2, j * 2, k * 2);



                    test.Add((Instantiate(prefab, distributionArea, Quaternion.identity)));


                   


                }

            }




            for (int l = 0; l < test.Count; l++)
            {
                properties.SetColor("_Color", new Color(.7f, .7f, .7f));
                MeshRenderer r = test[l].GetComponent<MeshRenderer>();
                if (r)
                {
                    r.SetPropertyBlock(properties);
                }
            }
        }
    }
}