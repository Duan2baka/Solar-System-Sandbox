using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public Button solarButton;
    public Button doubleButton;
    public Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        solarButton.onClick.AddListener(OnSolarButtonClick);
        doubleButton.onClick.AddListener(OnDoubleButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    void OnSolarButtonClick(){
        SceneManager.LoadScene("SolarSystem");
    }

    void OnDoubleButtonClick(){
        SceneManager.LoadScene("DoubleStar");
    }
    
    void OnQuitButtonClick(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
