using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

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
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject looseScreen;
    [SerializeField] private RoomManager[] rooms;

    [Header("Periods Durations")]
    [SerializeField] private float standbyDuration = 10.0f;

    [SerializeField] private float morningDuration = 30.0f;
    [SerializeField] private float middayDuration = 30.0f;
    [SerializeField] private float eveningDuration = 30.0f;

    [SerializeField] private float enddayDuration = 10.0f;

    [SerializeField] private GameObject[] waveUIs;
    [SerializeField] private TMP_Text dayText;

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

        dayText.text = "Day 1";

        /*
        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i].gameObject.SetActive(false);
        }
        */
        UpdateUI(0);
    }

    private void UpdateUI(int period)
    {
        for (int i = 0; i < waveUIs.Length; i++)
        {
            if (i == period)
                waveUIs[i].SetActive(true);
            else
                waveUIs[i].SetActive(false);
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
                LaunchWave(0);
                UpdateUI(1);
                RuntimeManager.StudioSystem.setParameterByName("EnemyNumber", 0);
                break;
            case EPeriod.Morning:
                currentPeriod = EPeriod.Midday;
                currentDuration = middayDuration;
                timer = 0.0f;
                LaunchWave(1);
                UpdateUI(2);
                break;
            case EPeriod.Midday:
                currentPeriod = EPeriod.Evening;
                currentDuration = eveningDuration;
                timer = 0.0f;
                LaunchWave(2);
                UpdateUI(3);
                break;
            case EPeriod.Evening:
                currentPeriod = EPeriod.Endday;
                currentDuration = enddayDuration;
                timer = 0.0f;
                UpdateUI(4);
                break;
            case EPeriod.Endday:
                if (EnemyNumber > 0)
                    break;

                if (day == 4)
                {
                    Win();
                    break;
                }
                currentPeriod = EPeriod.Standby;
                currentDuration = standbyDuration;
                timer = 0.0f;

                day += 1;
                dayText.text = "Day " + (day + 1).ToString();

                if (day < rooms.Length)
                    rooms[day].ActivateRoom();

                UpdateUI(0);

                break;
            default:
                break;
        }

        timer += Time.deltaTime;

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("EnemyNumber", EnemyNumber);
    }

    private void LaunchWave(int period)
    {
        RuntimeManager.StudioSystem.setParameterByName("Launch", 1);

        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].activated)
            {
                rooms[i].LaunchWave(day, period, currentDuration);
            }
        }
    }


    public void Win()
    {
        RuntimeManager.StudioSystem.setParameterByName("Win-Game-Lose", 0);
        gameStopped = true;

        winScreen.SetActive(true);
        //Debug.Log("WIN");
    }

    public void GameOver()
    {
        RuntimeManager.StudioSystem.setParameterByName("Win-Game-Lose", 2);
        gameStopped = true;
        looseScreen.SetActive(true);
    }
}
