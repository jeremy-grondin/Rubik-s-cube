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
        int cubeSize = PlayerPrefs.GetInt("CubeSize", 2);
        cubeSizeText.GetComponent<Text>().text = cubeSize.ToString();
        GameObject.FindGameObjectWithTag("SliderProfondeur").GetComponent<Slider>().value = cubeSize;

        shuffleText = GameObject.FindGameObjectWithTag("ShuffleText");
        int Shuffle = PlayerPrefs.GetInt("Shuffle", 0);
        shuffleText.GetComponent<Text>().text = Shuffle.ToString();
        GameObject.FindGameObjectWithTag("ShuffleValue").GetComponent<Slider>().value = Shuffle;

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
