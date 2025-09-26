using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_InputField inputField;
    public static string userText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void ReadInput()
    {
        
        userText = inputField.text;
        Debug.Log("User typed: " + userText);
        
    }




}
