using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    private void Update()
    {
        float xDelta = Mathf.Sin(Time.time * 4) / 2;
        transform.localScale = new Vector3(1, 1, 1) + new Vector3(xDelta, xDelta);
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
}
