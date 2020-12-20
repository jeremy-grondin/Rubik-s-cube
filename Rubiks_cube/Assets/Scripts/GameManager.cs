using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    RubiksCube cube;
    // Start is called before the first frame update
    void Start()
    {
        cube = GameObject.FindGameObjectWithTag("RubiksCube").GetComponent<RubiksCube>();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Reset()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
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