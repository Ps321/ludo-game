using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneOne : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartScene());

    }
    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Login");

    }
}
