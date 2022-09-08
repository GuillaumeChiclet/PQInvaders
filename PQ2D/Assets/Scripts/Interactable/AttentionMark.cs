using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionMark : MonoBehaviour
{
    [SerializeField] private GameObject[] ui;
    [SerializeField] private Transform mask;
    [SerializeField] private Vector2 minMax = new Vector2(0, 3.5f);

    private void Awake()
    {
        foreach(GameObject go in ui)
        {
            go.SetActive(false);
        }
    }

    public void SetPercent(float percent)
    {
        if (percent > 0)
        {
            foreach (GameObject go in ui)
            {
                go.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject go in ui)
            {
                go.SetActive(false);
            }
        }


        percent = Mathf.Clamp01(percent);
        float value = Mathf.Lerp(minMax.x, minMax.y, percent);
        Vector3 pos = mask.transform.localPosition;
        pos.y = value;
        mask.transform.localPosition = pos;
    }

}
