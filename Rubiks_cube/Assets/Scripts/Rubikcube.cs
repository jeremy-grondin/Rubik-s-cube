using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rubikcube : MonoBehaviour
{
    [SerializeField]
    int size = 0;

    [SerializeField]
    GameObject cube = null;

    [SerializeField]
    GameObject parent;

    [SerializeField]
    List<Material> materials;

    [SerializeField]
    GameObject shuffleValueText;

    [SerializeField]
    GameObject cubeSizeValue;

    int shuffle;

    void Start()
    {
        GenerateCubes();
        size = PlayerPrefs.GetInt("CubeSize");
        shuffle = PlayerPrefs.GetInt("Shuffle");

        if(shuffleValueText != null)
            shuffleValueText.GetComponent<Text>().text = shuffle.ToString();
        
        if(cubeSizeValue != null)
            cubeSizeValue.GetComponent<Text>().text = size.ToString();

    }

    void GenerateCubes()
    {
        if (cube == null)
            return;

        Vector3 pos = Vector3.zero;

        float decal = (size - 1) * -0.5f; 
        
        for(int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    Transform temp = Instantiate(cube.transform, parent.transform);
                    temp.position = new Vector3(decal + i, decal + j, decal + k);
                }
            }
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();   
    }

    public void SaveScene()
    {

    }

    public void Front()
    {

    }

    public void Back()
    {

    }

    public void Left()
    {

    }

    public void Right()
    {

    }

    public void Bottom()
    {

    }

    public void Top()
    {

    }

    void Update()
    {
        
    }
}
