using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour
{
    [SerializeField] float _delay;

    public void Restart()
    {
        StartCoroutine(RestartWithDelay());
    }

    private IEnumerator RestartWithDelay()
    {
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(0);
    }
}
