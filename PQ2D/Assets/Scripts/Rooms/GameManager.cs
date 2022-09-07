using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPeriod
{
    Standby = -1,
    Morning = 0,
    Midday = 1,
    Evening = 2,
    Endday = 3,
}


public class GameManager : MonoBehaviour
{
    [SerializeField] private RoomManager[] rooms;

    [Header("Periods Durations")]
    [SerializeField] private float standbyDuration = 10.0f;

    [SerializeField] private float morningDuration = 30.0f;
    [SerializeField] private float middayDuration = 30.0f;
    [SerializeField] private float eveningDuration = 30.0f;

    [SerializeField] private float enddayDuration = 10.0f;

    [HideInInspector] public int EnemyNumber = 0;
    private void AddEnemy() => EnemyNumber++;
    private void RemoveEnemy() => EnemyNumber--;
    

    private int day = 0;

    private float timer = 0.0f;
    private float currentDuration = 0.0f;
    private EPeriod currentPeriod = EPeriod.Standby;

    private bool gameStopped = false;

    private void Start()
    {
        timer = 0.0f;
        currentPeriod = EPeriod.Standby;
        currentDuration = standbyDuration;

        foreach (RoomManager room in rooms)
        {
            room.SetupSpawners(AddEnemy, RemoveEnemy);
        }

        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameStopped)
            return;

        if (timer < currentDuration)
        {
            timer += Time.deltaTime;
            return;
        }

        switch (currentPeriod)
        {
            case EPeriod.Standby:
                currentPeriod = EPeriod.Morning;
                currentDuration = morningDuration;
                timer = 0.0f;
                LaunchWave((int)currentPeriod);
                break;
            case EPeriod.Morning:
                currentPeriod = EPeriod.Midday;
                currentDuration = middayDuration;
                timer = 0.0f;
                LaunchWave((int)currentPeriod);
                break;
            case EPeriod.Midday:
                currentPeriod = EPeriod.Evening;
                currentDuration = eveningDuration;
                timer = 0.0f;
                LaunchWave((int)currentPeriod);
                break;
            case EPeriod.Evening:
                currentPeriod = EPeriod.Endday;
                currentDuration = enddayDuration;
                timer = 0.0f;
                break;
            case EPeriod.Endday:
                if (day == 5)
                {
                    Win();
                    return;
                }
                currentPeriod = EPeriod.Morning;
                currentDuration = standbyDuration;
                day += 1;
                rooms[day].gameObject.SetActive(true);
                break;
            default:
                break;
        }

        timer += Time.deltaTime;
    }

    private void LaunchWave(int period)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].isActiveAndEnabled)
            {
                rooms[i].LaunchWave(day, period, currentDuration);
            }
        }
    }


    public void Win()
    {
        gameStopped = true;
        Debug.Log("WIN");
    }

    public void GameOver()
    {
        gameStopped = true;
        Debug.Log("GAME OVER");
    }

}
