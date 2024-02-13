using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {
        sceneController = gameObject.AddComponent<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        print("Attempting to change scene");
        sceneController.LoadScene("Game");
    }
}
