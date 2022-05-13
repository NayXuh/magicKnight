using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public void Level1()
    {
        SceneManager.LoadScene(1);
        if(GameManager.current != null)
            GameManager.current.ResetStats();
        if(UIManager.current != null)
            UIManager.EnableHUD();
        
    }
    public void Level2()
    {
        SceneManager.LoadScene(2);
        if(GameManager.current != null)
            GameManager.current.ResetStats();
        if(UIManager.current != null)
            UIManager.EnableHUD();
        
    }
}
