using System;
using System.Collections;
using System.IO;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    public Button btnStartGame;
    
    public SkinPlate skin;
    private Renderer _rendQuad;

    public MovePlate move;
    
    public string pathToDirectory;

    private CoroutineState _coroutineState = CoroutineState.Uninitialized;
    private Camera _mainCamera;
    private Camera _previewCamera;

    void Start()
    {
        _mainCamera = Camera.allCameras[1];
        
        _previewCamera = GameObject.Find("PreviewCamera").GetComponent<Camera>();

        _mainCamera!.usePhysicalProperties = true;
        _mainCamera!.focalLength = CrossSceneInformation.FocusLength;
        _mainCamera!.sensorSize = new Vector2(CrossSceneInformation.SensorSizeX, 
            CrossSceneInformation.SensorSizeY);
        
        _previewCamera!.usePhysicalProperties = true;
        _previewCamera!.focalLength = CrossSceneInformation.FocusLength;
        _previewCamera!.sensorSize = new Vector2(CrossSceneInformation.SensorSizeX, 
            CrossSceneInformation.SensorSizeY);

        var quad = transform.GetChild(0);
        _rendQuad = quad.GetComponent<Renderer>();
        _rendQuad.enabled = true;
        _rendQuad.sharedMaterial = Resources.Load<Material>("Materials/" + CrossSceneInformation.MarkerName);
    }

    private void Update()
    {
        if (_coroutineState == CoroutineState.Finished)
        {
            CrossSceneInformation.StateGame = StateOfGame.Finished;
            GoBack();
        }

        switch (CrossSceneInformation.StateGame)
        {
            case StateOfGame.Start:
                CrossSceneInformation.StateGame = StateOfGame.Playing;
                switch (move)
                {
                    case MovePlate.Rotate:
                        StartCoroutine(RotatePlate());
                        break;
                    case MovePlate.RotateX:
                        StartCoroutine(RotatePlateX());
                        break;
                    case MovePlate.Translate:
                        StartCoroutine(TranslatePlate());
                        break;
                    case MovePlate.RotateAndTranslate:
                        StartCoroutine(RotateAndTranslatePlate());
                        break;
                    case MovePlate.TranslateAndRotateY:
                        StartCoroutine(TranslateAndRotateY());
                        break;
                    case MovePlate.TranslateAndRotateX:
                        StartCoroutine(TranslateAndRotateX());
                        break;
                }
                break;
            case StateOfGame.SettingUp:
                Start();
                break;
        }
    }

    private void GoBack()
    {
        SceneManager.LoadScene(MyScene.MainUI.ToString());
    }

    private IEnumerator RotatePlate()
    {
        _coroutineState = CoroutineState.Running;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 91; i++)
        {
            ScreenCapture.CaptureScreenshot(pathToDirectory + $"\\{CrossSceneInformation.MarkerName}" 
                                                            + @"\Rotate\" + $"{i}.png", 1);
            //Print the time of when the function is first called.
            Debug.Log("current angle : " + i);
            
            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(0.25f);
            if (i != 90) transform.Rotate(Vector3.up);
        }
        yield return new WaitForSeconds(0.5f);
        // transform.rotation = Quaternion.Euler(0, 0, 0);
        _coroutineState = CoroutineState.Finished;
    }
    
    private IEnumerator RotatePlateX()
    {
        _coroutineState = CoroutineState.Running;
        string path = pathToDirectory + $"\\{CrossSceneInformation.MarkerName}\\RotateX";
        Directory.CreateDirectory(path);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 91; i++)
        {
            ScreenCapture.CaptureScreenshot(path + $"\\{i}.png", 1);
            //Print the time of when the function is first called.
            Debug.Log("current angle : " + i);
            
            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(0.25f);
            if (i != 90) transform.Rotate(Vector3.right);
        }
        yield return new WaitForSeconds(0.5f);
        // transform.rotation = Quaternion.Euler(0, 0, 0);
        _coroutineState = CoroutineState.Finished;
    }

    private async void RotatePlate1()
    {
        for (int i = 0; i < 91; i++)
        {
            ScreenCapture.CaptureScreenshot(pathToDirectory + $"\\{CrossSceneInformation.MarkerName}"
                                                            + @"\Rotate\" + $"{i}.png", 1);
            
            //Print the time of when the function is first called.
            Debug.Log("current angle : " + i);
            
            //yield on a new YieldInstruction that waits for 5 seconds.
            await Task.Delay(250);
    
            if (i != 90) transform.Rotate(Vector3.up);
        }
    }
    
    private IEnumerator TranslatePlate()
    {
        _coroutineState = CoroutineState.Running;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 49; i++)
        {
            ScreenCapture.CaptureScreenshot(pathToDirectory + $"\\{CrossSceneInformation.MarkerName}"
                                                            + @"\Translate\" + $"{i}.png", 1);
            //Print the time of when the function is first called.
            Debug.Log("current Z : " + (5 + i));
            
            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(0.5f);
    
            if (i != 48) transform.Translate(Vector3.back / 4);
        }
        yield return new WaitForSeconds(0.5f);
        _coroutineState = CoroutineState.Finished;
    }
    
    // private IEnumerator TranslatePlate()
    // {
    //     _coroutineState = CoroutineState.Running;
    //     transform.position = new Vector3(0, 1f, 10.005005f);
    //     string path = pathToDirectory + $"\\{CrossSceneInformation.MarkerName}\\Translate1";
    //     Directory.CreateDirectory(path);
    //     yield return new WaitForSeconds(0.5f);
    //     float[] z = new float[49];
    //     for (int i = 0; i < 49; i++)
    //     {
    //         ScreenCapture.CaptureScreenshot(path + $"\\{i}.png", 1);
    //         //Print the time of when the function is first called.
    //         // Debug.Log("current Z : " + (5 + i));
    //         
    //         //yield on a new YieldInstruction that waits for 5 seconds.
    //         yield return new WaitForSeconds(0.5f);
    //
    //         float local = Random.Range(-1f, 1f);
    //         z[i] = 10.005005f + local;
    //         if (i != 48)transform.position = new Vector3(0, 1f, z[i]);
    //     }
    //     Debug.Log(String.Join(";", z));
    //     yield return new WaitForSeconds(0.5f);
    //     _coroutineState = CoroutineState.Finished;
    // }

    private IEnumerator RotateAndTranslatePlate()
    {
        // Camera mainCam = Camera.main;
        // mainCam.transform.Rotate(0, -5, 0);
        _coroutineState = CoroutineState.Running;
        yield return new WaitForSeconds(0.5f);
        transform.Rotate(0, -5, 0);
        for (int j = 0; j < 18; j++)
        {
            // mainCam.transform.Rotate(0, 5, 0);
            transform.Rotate(0, 5, 0);
            for (int i = 0; i < 9; i++)
            {
                ScreenCapture.CaptureScreenshot(pathToDirectory + $"\\{CrossSceneInformation.MarkerName}" 
                                                                + @"\RotateTranslate\" + $"{j+1}\\" 
                                                                + $"{i}.png", superSize: 1);

                //Print the time of when the function is first called.
                // Debug.Log("current Z : " + (5 + i));
            
                //yield on a new YieldInstruction that waits for 5 seconds.
                yield return new WaitForSeconds(0.5f);

                if (i != 8) transform.position = new Vector3(0, 1.5f, 3.005005f + i + 1);
            }
            transform.position = new Vector3(0, 1.5f, 3.005005f);
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(0.5f);
        _coroutineState = CoroutineState.Finished;
    }
    
    private IEnumerator TranslateAndRotateY()
    {
        // Camera mainCam = Camera.main;
        // mainCam.transform.Rotate(0, -5, 0);
        _coroutineState = CoroutineState.Running;
        // yield return new WaitForSeconds(0.5f);
        // transform.Rotate(0, -5, 0);
        string path = pathToDirectory + $"\\{CrossSceneInformation.MarkerName}\\TranslateRotateY";
        Directory.CreateDirectory(path);
        for (int j = 0; j < 33; j++)
        {
            // mainCam.transform.Rotate(0, 5, 0);
            // transform.Rotate(0, 5, 0);
            Directory.CreateDirectory(path + $"\\{j + 1}");
            transform.position = new Vector3(0, 1.5f, 3.005005f + j * 0.25f);
            yield return new WaitForSeconds(0.25f);
            for (int i = 0; i < 91; i++)
            {
                ScreenCapture.CaptureScreenshot(path  + $"\\{j + 1}\\" + $"{i}.png", superSize: 1);

                //Print the time of when the function is first called.
                // Debug.Log("current Z : " + (5 + i));
            
                //yield on a new YieldInstruction that waits for 5 seconds.
                yield return new WaitForSeconds(0.25f);

                if (i != 90) transform.Rotate(Vector3.up);
            }
            transform.rotation = Quaternion.Euler(0, 180, 0);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(0.5f);
        _coroutineState = CoroutineState.Finished;
    }
    
    private IEnumerator TranslateAndRotateX()
    {
        // Camera mainCam = Camera.main;
        // mainCam.transform.Rotate(0, -5, 0);
        _coroutineState = CoroutineState.Running;
        // yield return new WaitForSeconds(0.5f);
        // transform.Rotate(0, -5, 0);
        string path = pathToDirectory + $"\\{CrossSceneInformation.MarkerName}\\TranslateRotateX";
        Directory.CreateDirectory(path);
        for (int j = 0; j < 33; j++)
        {
            // mainCam.transform.Rotate(0, 5, 0);
            // transform.Rotate(0, 5, 0);
            Directory.CreateDirectory(path + $"\\{j + 1}");
            transform.position = new Vector3(0, 1.5f, 3.005005f + j * 0.25f);
            yield return new WaitForSeconds(0.25f);
            for (int i = 0; i < 91; i++)
            {
                ScreenCapture.CaptureScreenshot(path  + $"\\{j + 1}\\" + $"{i}.png", superSize: 1);

                //Print the time of when the function is first called.
                // Debug.Log("current Z : " + (5 + i));
            
                //yield on a new YieldInstruction that waits for 5 seconds.
                yield return new WaitForSeconds(0.25f);

                if (i != 90) transform.Rotate(Vector3.left);
            }
            transform.rotation = Quaternion.Euler(0, 180, 0);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(0.5f);
        _coroutineState = CoroutineState.Finished;
    }
    
    private void Exit()
    {
        Application.Quit();
    }
    
}
