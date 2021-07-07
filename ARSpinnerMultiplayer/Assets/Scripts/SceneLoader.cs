using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{

    private string sceneNameToBeLoaded;

   public void LoadScene(string scene)
    {
        sceneNameToBeLoaded = scene;
        StartCoroutine(InitializeSceneLoading());
    }


    IEnumerator InitializeSceneLoading()
    {
        //First, we load the loading scene
        yield return SceneManager.LoadSceneAsync("Scene_Loading");

        //load the actual scene
        StartCoroutine(LoadCurrentScene());

    }    

    IEnumerator LoadCurrentScene()
    {
        var asynSceneLoading = SceneManager.LoadSceneAsync(sceneNameToBeLoaded);

        //this value stops the scene from displaying when it is still loading... 
        asynSceneLoading.allowSceneActivation = false;

        while(!asynSceneLoading.isDone)
        {
            Debug.Log("<color=blue>" + asynSceneLoading.progress + "</color>");
            if(asynSceneLoading.progress >= 0.9f)
            {
                //finally show the scene
                asynSceneLoading.allowSceneActivation = true;
            }


            yield return null;
        }
    }
}
