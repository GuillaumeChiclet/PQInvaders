using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Week Scriptable", menuName = "Scriptables/Week Scriptable")]
public class WeekScriptable : ScriptableObject
{
    public Day[] days;
}

[System.Serializable]
public class Day
{
    public Period[] periods;
}

[System.Serializable]
public class Period
{
    public int[] enemiesNumber = new int[2];
}
