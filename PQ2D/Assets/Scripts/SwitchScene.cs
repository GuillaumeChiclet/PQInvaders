using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void SwitchToScene(int buildIndex = 0)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
