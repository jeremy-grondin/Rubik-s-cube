using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameObject cubeSizeText = null;
    GameObject shuffleText = null;

    [SerializeField]
    GameObject mainPanel = null;
    [SerializeField]
    GameObject optionPanel = null;

    private void Start()
    {
        cubeSizeText = GameObject.FindGameObjectWithTag("ProfondeurText");
        cubeSizeText.GetComponent<Text>().text = "2";

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
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }

    public void Resume()
    {
        Debug.Log("NOT IMPLEMENTED YET");
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

}
