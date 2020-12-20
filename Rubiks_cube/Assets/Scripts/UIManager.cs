using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameObject cubeSizeText;
    GameObject shuffleText;

    [SerializeField]
    GameObject mainPanel;
    [SerializeField]
    GameObject optionPanel;

    private void Start()
    {


        cubeSizeText = GameObject.FindGameObjectWithTag("ProfondeurText");
        if (cubeSizeText != null)
            cubeSizeText.GetComponent<Text>().text = "2";
        else
            Debug.Log("HMMMMMM");

        shuffleText = GameObject.FindGameObjectWithTag("ShuffleText");
        shuffleText.GetComponent<Text>().text = "0";

        optionPanel.SetActive(false);
    }

    public void UpdateSliderCubeSize()
    {
        cubeSizeText.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("SliderProfondeur").GetComponent<Slider>().value.ToString();
    }

    public void UpdateSliderShuffle()
    {
        shuffleText.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("ShuffleValue").GetComponent<Slider>().value.ToString();
    }

    public void DisplayOptionPanel()
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void LaunchLevel()
    {
        PlayerPrefs.SetInt("CubeSize", (int)GameObject.FindGameObjectWithTag("SliderProfondeur").GetComponent<Slider>().value);
        PlayerPrefs.SetInt("Shuffle", (int)GameObject.FindGameObjectWithTag("ShuffleValue").GetComponent<Slider>().value);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void Resume()
    {
        
    }
    
    public void GoBack()
    {
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
    }


    public void RotateCube()
    {
        //TODO make a preview of the rotation of the cube 
    }
}
