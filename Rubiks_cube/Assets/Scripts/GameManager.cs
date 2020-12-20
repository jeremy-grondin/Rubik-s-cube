using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    RubiksCube cube;
    [SerializeField]
    GameObject cubeSizeText = null;

    [SerializeField]
    GameObject shuffleText = null;

    // Start is called before the first frame update
    void Start()
    {
        cube = GameObject.FindGameObjectWithTag("RubiksCube").GetComponent<RubiksCube>();
        cubeSizeText.GetComponent<Text>().text = PlayerPrefs.GetInt("CubeSize").ToString();
        shuffleText.GetComponent<Text>().text = PlayerPrefs.GetInt("Shuffle").ToString();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Reset()
    {
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Front()
    {
        cube.Front();
    }

    public void Back()
    {
        cube.Back();
    }

    public void Left()
    {
        cube.Left();
    }

    public void Right()
    {
        cube.Right();
    }

    public void Top()
    {
        cube.Top();
    }

    public void Bottom()
    {
        cube.Bottom();
    }
}