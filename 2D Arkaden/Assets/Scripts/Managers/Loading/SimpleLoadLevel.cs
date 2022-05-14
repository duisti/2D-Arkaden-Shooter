using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleLoadLevel : MonoBehaviour
{
    bool loading;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OrderSceneLoad(string sceneName)
    {
        if (!loading)
        {
            StartCoroutine(LoadLevel(sceneName));
        }
    }

    IEnumerator LoadLevel(string sceneName)
    {
        loading = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        loading = false;
    }
}
