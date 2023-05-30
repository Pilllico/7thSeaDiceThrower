using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RandomRotation : MonoBehaviour
{
    [Range(0.5f, 5.0f)]
    public float speed = 1f;

    [Range(0.5f, 5.0f)]
    public float time = 1f;

    float rX, rY, rZ;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SetNewRotation), 0f, time);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(rX, rY, rZ));
    }

    void SetNewRotation()
    {
        Vector3 rotation = speed * Vector3.Normalize(new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)));
        DOTween.To(() => rX, x => rX = x, rotation.x, time);
        DOTween.To(() => rY, x => rY = x, rotation.y, time);
        DOTween.To(() => rZ, x => rZ = x, rotation.z, time);
    }
}
