using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 3f;

    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
