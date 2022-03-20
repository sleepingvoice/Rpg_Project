using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene_Manager : MonoBehaviour
{
    public void Go_to_GameScene()
    {
        SceneManager.LoadScene("Game");
    }

}
