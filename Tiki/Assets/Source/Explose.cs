using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explose : MonoBehaviour {
    float tmpTime = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        tmpTime += Time.deltaTime;
        if (tmpTime >= 0.5f)
        {
            Destroy(gameObject);
        }
	}
}
