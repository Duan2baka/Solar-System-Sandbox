using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button menuButton;
    public Button freezeButton;
    public Button createButton;
    public Button selectButton;
    public Button resetButton;
    public Button mainMenuButton;
    public GameObject planetList;
    public GameObject selectList;
    public GameObject[] prefabs;
    GameObject[] celestials;
    public Sprite roundRectSprite;
    CameraControl myCamera;
    bool freeze = true;

    void Start()
    {
        // 隐藏行星列表
        planetList.SetActive(false);
        selectList.SetActive(false);
        myCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        // 为菜单按钮添加点击事件
        menuButton.onClick.AddListener(OnMenuButtonClick);
        freezeButton.onClick.AddListener(OnFreezeButtonClick);
        createButton.onClick.AddListener(OnCreateButtonClick);
        selectButton.onClick.AddListener(OnSelectListClick);
        resetButton.onClick.AddListener(OnResetButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        freeze = GameObject.Find("SolarSystem").GetComponent<SolarSystem>().freeze;
        freezeButton.transform.Find("Text").GetComponent<Text>().text = freeze ? "Unfreeze the sun" : "Freeze the sun";
    }

    void OnResetButtonClick(){
        var s = GameObject.Find("SpeedController").GetComponent<GameSpeedController>().speedSlider;
        s.value = 1;
    }

    void OnMenuButtonClick()
    {
        celestials = GameObject.FindGameObjectsWithTag("Celestial");
        foreach (Transform child in planetList.transform)
            Destroy(child.gameObject);
        Debug.Log("123");

        foreach(GameObject a in celestials){
            // create the button
            GameObject buttonObject = new GameObject(a.name + "Button");
            RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
            Button button = buttonObject.AddComponent<Button>();
            Image buttonImage = buttonObject.AddComponent<Image>();

            // set the position and size of the button
            rectTransform.SetParent(planetList.transform);
            rectTransform.anchoredPosition = new Vector2(0, 0);
            //rectTransform.sizeDelta = new Vector2(planetList.GetComponent<RectTransform>().rect.width, 20);
            Vector2 size = GameObject.Find("ListButton").GetComponent<RectTransform>().sizeDelta;
            // Debug.Log(size);
            size = Vector2.Scale(size, new Vector2(Screen.width / 800f, Screen.height / 600f));
            // Debug.Log(size);
            rectTransform.sizeDelta = Vector2.Scale(size, new Vector2(0.8f, 0.8f));

            // set the background color
            buttonImage.sprite = roundRectSprite;
            buttonImage.type = Image.Type.Sliced;

            // create button text
            GameObject buttonTextObject = new GameObject(a.name + "Text");
            RectTransform buttonTextRectTransform = buttonTextObject.AddComponent<RectTransform>();
            Text buttonText = buttonTextObject.AddComponent<Text>();

            // set the text of the buton
            buttonText.text = a.name;
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonText.fontSize = 15;
            buttonText.color = Color.black;
            buttonText.alignment = TextAnchor.MiddleCenter;

            // set the position and size of the text
            buttonTextRectTransform.SetParent(rectTransform);
            buttonTextRectTransform.anchoredPosition = Vector2.zero;
            buttonTextRectTransform.sizeDelta = rectTransform.sizeDelta;

            // attach the text to button
            buttonText.transform.SetParent(buttonObject.transform, false);

            //buttonImage.color = Color.white;

            button.onClick.AddListener(delegate { OnPlanetButtonClick(a.name); });
            
            buttonObject.transform.SetParent(planetList.transform, false);
        }
        

        planetList.SetActive(!planetList.activeSelf);
    }

    void OnFreezeButtonClick(){
        GameObject.Find("SolarSystem").GetComponent<SolarSystem>().freeze = freeze = !freeze;
        freezeButton.transform.Find("Text").GetComponent<Text>().text = freeze ? "Unfreeze the sun" : "Freeze the sun";
        if(!freeze) GameObject.Find("SolarSystem").GetComponent<SolarSystem>().GetVelocity();
    }

    void OnCreateButtonClick(){
        GameObject.Find("CircleOnPlane").GetComponent<CircleOnPlane>().addPlanet = !GameObject.Find("CircleOnPlane").GetComponent<CircleOnPlane>().addPlanet;
        //GameObject.Find("MouseController").GetComponent<MouseController>().prefab = GameObject.Find("MouseController").GetComponent<MouseController>().prefab0;
    }
    
    void OnSelectListClick(){
        foreach (Transform child in selectList.transform)
            Destroy(child.gameObject);

        foreach(GameObject a in prefabs){

            // 创建按钮
            GameObject buttonObject = new GameObject(a.name + "Button");
            RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
            Button button = buttonObject.AddComponent<Button>();
            Image buttonImage = buttonObject.AddComponent<Image>();

            // 设置按钮位置和大小
            rectTransform.SetParent(selectList.transform);
            rectTransform.anchoredPosition = new Vector2(0, 0);
            //rectTransform.sizeDelta = new Vector2(planetList.GetComponent<RectTransform>().rect.width, 20);
            Vector2 size = GameObject.Find("Select").GetComponent<RectTransform>().sizeDelta;
            // Debug.Log(size);
            size = Vector2.Scale(size, new Vector2(Screen.width / 800f, Screen.height / 600f));
            // Debug.Log(size);
            rectTransform.sizeDelta = Vector2.Scale(size, new Vector2(0.8f, 0.8f));

            // 设置按钮背景颜色
            buttonImage.sprite = roundRectSprite;
            buttonImage.type = Image.Type.Sliced;

            // 创建按钮文本
            GameObject buttonTextObject = new GameObject(a.name + "Text");
            RectTransform buttonTextRectTransform = buttonTextObject.AddComponent<RectTransform>();
            Text buttonText = buttonTextObject.AddComponent<Text>();

            // 设置按钮文本
            buttonText.text = a.name;
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonText.fontSize = 15;
            buttonText.color = Color.black;
            buttonText.alignment = TextAnchor.MiddleCenter;

            // 设置文本位置和大小
            buttonTextRectTransform.SetParent(rectTransform);
            buttonTextRectTransform.anchoredPosition = Vector2.zero;
            buttonTextRectTransform.sizeDelta = rectTransform.sizeDelta;

            // 将文本添加到按钮
            buttonText.transform.SetParent(buttonObject.transform, false);

            //buttonImage.color = Color.white;

            button.onClick.AddListener(delegate { OnSelectButtonClick(a); });
            
            buttonObject.transform.SetParent(selectList.transform, false);
        }
        selectList.SetActive(!selectList.activeSelf);
    }

    void OnSelectButtonClick(GameObject planetPrefab){
        GameObject.Find("MouseController").GetComponent<MouseController>().prefab = planetPrefab;
        GameObject.Find("CreateEarth").transform.Find("Text").GetComponent<Text>().text = "Create " + planetPrefab.name;
        selectList.SetActive(false);
    }

    void OnPlanetButtonClick(string planetName)
    {
        GameObject planet = GameObject.Find(planetName);
        if (planet != null){
            myCamera.pivot = planet.transform;
            myCamera.target = planet.transform;
            myCamera.setTargetDistance(planet.transform.localScale.x * 6f);
        }
        planetList.SetActive(false);
    }
    void OnMainMenuButtonClick(){
        SceneManager.LoadScene("Menu");
    }
}