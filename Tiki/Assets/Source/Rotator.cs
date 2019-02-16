using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    int k = 0;
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(5, 30, 0) * Time.deltaTime);
        Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
        k++;

    }
}   
