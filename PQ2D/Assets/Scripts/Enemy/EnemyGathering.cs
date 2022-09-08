using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EnemyGathering : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    [SerializeField] private float timeToGather = 1.0f;
    [SerializeField] private AttentionMark attentionMark;
    // Here sprite control scaled

    EnemyController enemyController;
    CenterStock stock;

    

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public void StartGathering(CenterStock centerStock)
    {
        stock = centerStock;
        enemyController.Stop();
        StartCoroutine(Gather());
        
    }

    IEnumerator Gather()
    {
        float timer = 0.0f;
        while (timer < timeToGather)
        {
            timer += Time.deltaTime;
            
            RuntimeManager.StudioSystem.setParameterByName("Alarm", +1);

            if (attentionMark)
            {
                attentionMark.SetPercent(timer / timeToGather);
            }

            yield return null;
        }

        RuntimeManager.StudioSystem.setParameterByName("Alarm", -1);

        stock.SuccessGather();
        enemyController.Die();
    }
}
