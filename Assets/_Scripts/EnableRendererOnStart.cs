using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRendererOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
