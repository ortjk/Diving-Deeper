using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void DeleteThis()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
