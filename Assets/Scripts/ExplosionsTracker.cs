using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Variables;

public class ExplosionsTracker : MonoBehaviour
{
    public static ExplosionsTracker instance;

    public Image explosionImage;
    public List<Sprite> explosionsCollection;

    private void Start()
    {
        instance = this;
    }

    public void SetExplosionTracker()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(nbExplosions > Mathf.Ceil(i * 0.5f));
        explosionImage.sprite = explosionsCollection[Mathf.Min(nbExplosions, 3)];
    }
}
