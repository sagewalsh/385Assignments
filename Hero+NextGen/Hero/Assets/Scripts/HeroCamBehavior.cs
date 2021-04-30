using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCamBehavior : MonoBehaviour
{
    public GreenUpBehavior hero = null;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = hero.transform.position;
        pos.z = -10;
        transform.position = pos;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        pos = hero.transform.position;
        pos.z = -10;
        transform.position = pos;
    }

    public void CallShake()
    {
        StartCoroutine(Shake(1.0f, 0.7f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = pos;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Debug.Log("In the While " + elapsed);
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }
}
