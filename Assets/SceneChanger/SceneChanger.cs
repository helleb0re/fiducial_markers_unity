using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class SceneChanger : MonoBehaviour
{
    public TMP_Dropdown sceneChooser;
    public TMP_Dropdown markerChooser;
    public TMP_Dropdown resolutionChooser;

    public Slider focusLength;

    public TMP_InputField focusLengthValue;

    public TMP_InputField sensorSizeX;
    public TMP_InputField sensorSizeY;
    public RawImage previewScreen;
    
    private void Start()
    {
        CrossSceneInformation.StateGame = StateOfGame.SettingUp;

        PopulateListSkinPlate();
        PopulateListScene();
        PopulateListResolution();

        sceneChooser.onValueChanged.AddListener(SetUpPreview);
        int numScene = (int)CrossSceneInformation.ActiveScene - 1;
        if (numScene == 0) SetUpPreview(numScene);
        else sceneChooser.value = numScene;

        focusLength.onValueChanged.AddListener((idx) =>
        {
            focusLengthValue.text = idx.ToString("F");
        });

        focusLengthValue.onValueChanged.AddListener((idx) =>
        {
            if (float.TryParse(idx, out float tmp))
            {
                if (tmp >= 5 && tmp <= 100)
                {
                    CrossSceneInformation.FocusLength = tmp;
                    focusLength.value = tmp;
                }
            }
        });
        focusLength.value = CrossSceneInformation.FocusLength;

        markerChooser.onValueChanged.AddListener((idx) =>
        {
            CrossSceneInformation.MarkerName = markerChooser.options[idx].text;
        });
        markerChooser.value = markerChooser.options
            .Select(x => x.text)
            .ToList()
            .IndexOf(CrossSceneInformation.MarkerName);
        resolutionChooser.onValueChanged.AddListener((idx) =>
        {
            CrossSceneInformation.ActiveResolution = "Res_" + markerChooser.options[idx].text;
        });
        resolutionChooser.value = resolutionChooser.options
            .Select(x => x.text)
            .ToList()
            .IndexOf(CrossSceneInformation.ActiveResolution.Split('_')[1]);
        
        sensorSizeX.onValueChanged.AddListener((value) =>
        {
            float.TryParse(value, out float x);
            CrossSceneInformation.SensorSizeX = x;
        });
        sensorSizeX.text = CrossSceneInformation.SensorSizeX.ToString("F");
        
        sensorSizeY.onValueChanged.AddListener((value) =>
        {
            float.TryParse(value, out float y);
            CrossSceneInformation.SensorSizeY = y;
        });
        sensorSizeY.text = CrossSceneInformation.SensorSizeY.ToString("F");
        
    }

    void SetUpPreview(int idx)
    {
        if (SceneManager.sceneCount > 1)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(CrossSceneInformation.ActiveScene.ToString()));
        }
        CrossSceneInformation.ActiveScene = (MyScene)(idx + 1);
        SceneManager.LoadScene(CrossSceneInformation.ActiveScene.ToString(), LoadSceneMode.Additive);
        previewScreen.texture = Resources.Load<Texture>("Texture/RenderTexture" + CrossSceneInformation.ActiveScene);
    }
    
    void PopulateListSkinPlate()
    {
        string[] enumName = Enum.GetNames(typeof(SkinPlate));
        List<string> names = new List<string>(enumName);
        markerChooser.AddOptions(names);
    }
    
    void PopulateListScene()
    {
        string[] enumName = Enum.GetNames(typeof(MyScene)).Skip(1).ToArray();
        List<string> names = new List<string>(enumName);
        sceneChooser.AddOptions(names);
    }
    
    void PopulateListResolution()
    {
        string[] enumName = Enum.GetNames(typeof(Resolution)).Select(res => res.Split('_')[1]).ToArray();
        List<string> names = new List<string>(enumName);
        resolutionChooser.AddOptions(names);
    }
    
    public void StartTest()
    {
        string[] strRes = CrossSceneInformation.ActiveResolution.Split('_')[1].Split('x');
        int width = int.Parse(strRes[0]);
        int height = int.Parse(strRes[1]);
        Screen.SetResolution(width, height, false);
        CrossSceneInformation.StateGame = StateOfGame.Start;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(CrossSceneInformation.ActiveScene.ToString()));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(MyScene.MainUI.ToString()));
    }
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != MyScene.MainUI.ToString())
        {
            // CrossSceneInformation.ActiveScene = MyScene.MainUI;
            SceneManager.LoadScene(MyScene.MainUI.ToString());
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
