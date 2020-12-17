using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameObject cubeSizeText;
    GameObject shuffleText;

    private void Start()
    {
        cubeSizeText = GameObject.FindGameObjectWithTag("ProfondeurText");
        cubeSizeText.GetComponent<Text>().text = "1";

        shuffleText = GameObject.FindGameObjectWithTag("ShuffleText");
        shuffleText.GetComponent<Text>().text = "0";
    }

    public void UpdateSliderCubeSize()
    {
        cubeSizeText.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("SliderProfondeur").GetComponent<Slider>().value.ToString();
    }

    public void UpdateSliderShuffle()
    {
        shuffleText.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("ShuffleValue").GetComponent<Slider>().value.ToString();
    }

    public void LaunchLevel()
    {
        PlayerPrefs.SetInt("CubeSize", (int)GameObject.FindGameObjectWithTag("SliderProfondeur").GetComponent<Slider>().value);
        PlayerPrefs.SetInt("Shuffle", (int)GameObject.FindGameObjectWithTag("ShuffleValue").GetComponent<Slider>().value);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
