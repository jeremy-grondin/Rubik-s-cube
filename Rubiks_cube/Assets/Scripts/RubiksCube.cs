using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RubiksCube : MonoBehaviour
{
    #region Variables
    [SerializeField]
    float rotationSpeedCube = 200;
    [SerializeField]
    float rotationSpeedSlice = 200;
    [SerializeField]
    float slerpSliceScale = 5;
    [SerializeField]
    float slerpCubeScale = 1;
    [SerializeField]
    float minimumMouseOffset = 2;
    [SerializeField]
    int size = 0;

    [SerializeField]
    GameObject cube = null;
    [SerializeField]
    GameObject pivot = null;
    [SerializeField]
    GameObject front = null;
    [SerializeField]
    GameObject back = null;
    [SerializeField]
    GameObject left = null;
    [SerializeField]
    GameObject right = null;
    [SerializeField]
    GameObject bottom = null;
    [SerializeField]
    GameObject top = null;
    [SerializeField]
    GameObject mainPanel = null;
    [SerializeField]
    GameObject victoryPanel = null;

    List<GameObject> pivotsAxisX = new List<GameObject>();
    List<GameObject> pivotsAxisY = new List<GameObject>();
    List<GameObject> pivotsAxisZ = new List<GameObject>();
    List<GameObject> allCubes = new List<GameObject>();

    List<GameObject> selectedCubes = new List<GameObject>();
    GameObject selectedCube = null;
    GameObject currentPivot = null;

    bool gameFinished = false;
    bool isShuffling = false;
    bool hasSelectedACube = false;
    bool isRotatingASlice = false;
    bool isSlerpingASlice = false;
    bool isSlerpingCube = false;
    Vector3 selectedCubeNormal = Vector3.zero;
    Vector3 mouseDeltaPos = Vector3.zero;
    Vector3 currentPivotAxis = Vector3.zero;
    Vector3 MouseAxis = Vector3.zero;
    Vector3 rotationOverAll = Vector3.zero;


    #endregion

    #region Function

    #region Generation
    void Start()
    {
        //Set size based on settings set in MainMenu
        size = PlayerPrefs.GetInt("CubeSize");

        //calculate MaxOffSet
        float maxOffSet = -0.5f * (size - 1);
        GenerateCubes(maxOffSet);
        GeneratePivots(maxOffSet);

        //initial shuffle based on settings set in MainMenu
        StartCoroutine(Shuffle(PlayerPrefs.GetInt("Shuffle")));
    }
    void GenerateCubes(float maxOffSet)
    {
        //generate Cubes
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                for (int k = 0; k < size; k++)
                {
                    GameObject tempCube = Instantiate(cube, new Vector3(maxOffSet + i, maxOffSet + j, maxOffSet + k), Quaternion.identity, transform);
                    GenerateFaces(tempCube, i, j, k);

                    allCubes.Add(tempCube);
                }

    }
    void GeneratePivots(float maxOffSet)
    {
        for (int i = 0; i < size; i++)
        {
            pivotsAxisX.Add(Instantiate(pivot, new Vector3(maxOffSet + i, 0, 0), Quaternion.identity, transform));
            pivotsAxisY.Add(Instantiate(pivot, new Vector3(0, maxOffSet + i, 0), Quaternion.identity, transform));
            pivotsAxisZ.Add(Instantiate(pivot, new Vector3(0, 0, maxOffSet + i), Quaternion.identity, transform));
        }
    }
    void GenerateFaces(GameObject cubeParent, int i, int j, int k)
    {
        if (i == 0)
            Instantiate(left, cubeParent.transform);
        else if (i == size - 1)
            Instantiate(right, cubeParent.transform);
        if (j == 0)
            Instantiate(bottom, cubeParent.transform);
        else if (j == size - 1)
            Instantiate(top, cubeParent.transform);
        if (k == 0)
            Instantiate(front, cubeParent.transform);
        else if (k == size - 1)
            Instantiate(back, cubeParent.transform);
    }
    #endregion

    #region Rotation
    void StartRotation(bool isRotatingVerticalSlice)
    {
        hasSelectedACube = false;
        isRotatingASlice = true;
        mouseDeltaPos = Vector3.zero;

        //Store all needed cubes in "selectedCubes"
        if (isRotatingVerticalSlice)
            SetVerticalSlices();
        else
            SetHorizontalSlices();

        //Set "CurrentPivot"
        SetPivot(isRotatingVerticalSlice);

        //"CurrentPivot" become parent of all selected Cubes
        foreach (GameObject cube in selectedCubes)
            cube.transform.parent = currentPivot.transform;
    }
    void SetVerticalSlices()
    {
        //Axis For the Mouse
        MouseAxis = Vector3.up;
        int sizeSquared = size * size;

        //fill array
        if (selectedCubeNormal == Vector3.right || selectedCubeNormal == Vector3.left)
        {
            //Axis of rotation for the pivot
            currentPivotAxis = Vector3.back;
            int numberOfTheDepth = (allCubes.IndexOf(selectedCube) % sizeSquared) % size;


            for (int i = numberOfTheDepth; i < allCubes.Count; i += size)
                selectedCubes.Add(allCubes[i]);
        }
        else
        {
            //Axis of rotation for the pivot
            currentPivotAxis = Vector3.right;
            //variable in int to remove decimals
            int numberOfTheColumn = allCubes.IndexOf(selectedCube) / sizeSquared;

            for (int i = numberOfTheColumn * sizeSquared; i < numberOfTheColumn * sizeSquared + sizeSquared; ++i)
                selectedCubes.Add(allCubes[i]);
        }
    }
    void SetHorizontalSlices()
    {
        //Axis for the mouse
        MouseAxis = Vector3.right;
        int sizeSquared = size * size;


        //fill array
        if (selectedCubeNormal == Vector3.down || selectedCubeNormal == Vector3.up)
        {
            //Axis of rotation for the pivot
            currentPivotAxis = Vector3.forward;
            int numberOfTheDepth = (allCubes.IndexOf(selectedCube) % sizeSquared) % size;

            for (int i = sizeSquared - size + numberOfTheDepth; i < allCubes.Count; i += sizeSquared)
                for (int j = 0; j < size; ++j)
                    selectedCubes.Add(allCubes[i - j * size]);
        }
        else
        {
            //Axis of rotation for the pivot
            currentPivotAxis = Vector3.up;
            //variable in int to remove decimals
            int numberOfTheRow = (allCubes.IndexOf(selectedCube) % sizeSquared) / size;

            for (int i = numberOfTheRow * size; i < allCubes.Count; i += sizeSquared)
                for (int j = 0; j < size; ++j)
                    selectedCubes.Add(allCubes[i + j]);
        }
    }
    void SetPivot(bool isRotatingVerticalSlice)
    {
        //Set "currentPivot" depending on which slice we take, and check exceptions
        if (isRotatingVerticalSlice)
        {
            if (selectedCubeNormal == Vector3.left || selectedCubeNormal == Vector3.right)
                currentPivot = pivotsAxisZ[(allCubes.IndexOf(selectedCube) % (size * size)) % size];
            else
                currentPivot = pivotsAxisX[allCubes.IndexOf(selectedCube) / (size * size)];
        }
        else
        {
            if (selectedCubeNormal == Vector3.down || selectedCubeNormal == Vector3.up)
                currentPivot = pivotsAxisZ[(allCubes.IndexOf(selectedCube) % (size * size)) % size];
            else
                currentPivot = pivotsAxisY[(allCubes.IndexOf(selectedCube) % (size * size)) / size];
        }
    }
    IEnumerator EndRotation(Quaternion finalRotation, bool needToCheckCubeSolved = true)
    {
        float waitedTime = 0f;
        isRotatingASlice = false;
        isSlerpingASlice = true;

        Quaternion initialRotation = currentPivot.transform.localRotation;

        //Slerp
        while (waitedTime <= 1)
        {
            waitedTime += Time.deltaTime * slerpSliceScale;
            currentPivot.transform.localRotation = Quaternion.Slerp(initialRotation, finalRotation, waitedTime);
            yield return null;
        }

        //Detach all selected Cubes from "currentPivot"
        foreach (GameObject cube in selectedCubes)
            cube.transform.parent = transform;

        //Reajust the List "allCubes" used as a Map
        ReajustArrayMap();

        //Check if the player solved the Rubick's Cube 
        if(needToCheckCubeSolved)
            CheckIfSolved();

        isSlerpingASlice = false;
        currentPivot = null;
        selectedCube = null;
        selectedCubes.Clear();
        rotationOverAll = Vector3.zero;
        selectedCubeNormal = Vector3.zero;
    }
    Vector3 RoundVectorNearestRightAngle(Vector3 vector)
    {
        return new Vector3(Mathf.Round(vector.x / 90) * 90,
                           Mathf.Round(vector.y / 90) * 90,
                           Mathf.Round(vector.z / 90) * 90);
    }
    void ReajustArrayMap()
    {
        //RoundUp the Rotation OverAll to clamp all component [0 ; 360]
        Vector3 rotationOverAllRounded = RoundVectorNearestRightAngle(rotationOverAll);
        //Get number of Right Angle Rotation [-3 ; 3]
        int numberOfRotation = (int)(rotationOverAllRounded.x + rotationOverAllRounded.y + rotationOverAllRounded.z) % 360 / 90;

        if (numberOfRotation == 0)
            return;


        List<int> listOfIndexes = new List<int>();
        foreach (GameObject cube in selectedCubes)
            listOfIndexes.Add(allCubes.IndexOf(cube));

        if (numberOfRotation == 1 || numberOfRotation == -3)
            for (int j = 0; j < size; ++j)
                for (int k = 0; k < size; ++k)
                    allCubes[listOfIndexes[k + j * size]] = selectedCubes[(size - 1 - j) + k * size];

        else if (numberOfRotation == 2 || numberOfRotation == -2)
            for (int j = 0; j < size; ++j)
                for (int k = 0; k < size; ++k)
                    allCubes[listOfIndexes[k + j * size]] = selectedCubes[(size - 1 - k) + size * (size - 1 - j)];

        else if (numberOfRotation == 3 || numberOfRotation == -1)
            for (int j = 0; j < size; ++j)
                for (int k = 0; k < size; ++k)
                    allCubes[listOfIndexes[k + j * size]] = selectedCubes[j + size * (size - 1 - k)];
    }
    void CheckIfSolved()
    {
        //A Rubick's cube is solved if all the cubes composing it have the same Rotation
        Quaternion rotationToHave = allCubes[0].transform.rotation;

        //we can start from i = 1 as allcubes[0] will always have the same rotation as himself
        for (int i = 1; i < allCubes.Count; ++i)
            if (allCubes[i].transform.rotation != rotationToHave)
                return;

        gameFinished = true;
        mainPanel.SetActive(false);
        victoryPanel.SetActive(true);
    }

    IEnumerator SetupRotation(Quaternion finalRotation)
    {
        float waitedTime = 0f;
        isSlerpingCube = true;

        Quaternion initialRotation = transform.rotation;

        //Slerp
        while (waitedTime <= 1)
        {
            waitedTime += Time.deltaTime * slerpCubeScale;
            transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, waitedTime);
            yield return null;
        }

        isSlerpingCube = false;
    }
    IEnumerator Shuffle(int numberOfShuffle)
    {
        isShuffling = true;

        //For "numberOfShuffle" take a random Pivot, get a Slice
        //and rotate it by 90, 180 or 270°
        for (int i = 0; i < numberOfShuffle; ++i)
        {
            int pivotAxis = Random.Range(0, 3);
            bool isRotatingVerticalSlice = false;

            List<GameObject> pivotsAxisList = new List<GameObject>();

            if (pivotAxis == 0)
            {
                isRotatingVerticalSlice = true;
                pivotsAxisList = pivotsAxisX;
            }
            else if (pivotAxis == 1)
            {
                isRotatingVerticalSlice = false;
                pivotsAxisList = pivotsAxisY;
            }
            else if (pivotAxis == 2)
            {
                isRotatingVerticalSlice = Random.Range(0, 2) == 1 ? true : false;
                selectedCubeNormal = isRotatingVerticalSlice ? Vector3.left : Vector3.up;
                pivotsAxisList = pivotsAxisZ;
            }

            int numberInAxisList = Random.Range(0, size);
            currentPivot = pivotsAxisList[numberInAxisList];

            if (pivotAxis == 0)
                selectedCube = allCubes[(size * size) * numberInAxisList];
            else if (pivotAxis == 1)
                selectedCube = allCubes[size * numberInAxisList];
            else if (pivotAxis == 2)
                selectedCube = allCubes[numberInAxisList];

            if (isRotatingVerticalSlice)
                SetVerticalSlices();
            else
                SetHorizontalSlices();

            foreach (GameObject cube in selectedCubes)
                cube.transform.parent = currentPivot.transform;

            //For unknow reasons if we call SetHorizontalSlices, the rotationOverAll must be with an angle of 180°
            rotationOverAll = isRotatingVerticalSlice ? currentPivotAxis * (Random.Range(1, 4) * 90) : currentPivotAxis * 180;
            yield return StartCoroutine(EndRotation(currentPivot.transform.localRotation * Quaternion.Euler(rotationOverAll), false));
        }

        isShuffling = false;
    }

    void Update()
    {

        //Mouse Right Button Hold
        //Quat * rotation in this order to rotate in World
        if (Input.GetMouseButton(1))
            transform.rotation = Quaternion.Euler(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeedCube)
                                 * transform.rotation;

        //when game finished or shuffling the player can still rotate the rubick's cube overall
        if (gameFinished || isShuffling)
            return;

        //Mouse Left Button Press 
        if (Input.GetMouseButtonDown(0) && !isSlerpingASlice && !isSlerpingCube)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                selectedCube = hit.collider.gameObject;
                selectedCubeNormal = transform.worldToLocalMatrix * hit.normal;
                hasSelectedACube = true;
            }

        }


        //Mouse Left Button Hold
        if (Input.GetMouseButton(0))
        {
            if (hasSelectedACube)
            {
                mouseDeltaPos += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);

                if (Vector3.Dot(mouseDeltaPos, Vector3.up) > minimumMouseOffset ||
                    Vector3.Dot(mouseDeltaPos, -Vector3.up) > minimumMouseOffset)
                    StartRotation(true);
                if (Vector3.Dot(mouseDeltaPos, Vector3.right) > minimumMouseOffset ||
                    Vector3.Dot(mouseDeltaPos, -Vector3.right) > minimumMouseOffset)
                    StartRotation(false);
            }
            else if (isRotatingASlice)
            {
                //rotation * Quat in this order to rotate in Local
                currentPivot.transform.localRotation = currentPivot.transform.localRotation * Quaternion.Euler(currentPivotAxis * Vector3.Dot(MouseAxis, new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f)) * Time.deltaTime * rotationSpeedSlice);
                //same Vector as used for the Quaternion.Euler but the mouse input X is not reverted for easier use after
                rotationOverAll += currentPivotAxis * (Vector3.Dot(MouseAxis, new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f)) * Time.deltaTime * rotationSpeedSlice);
            }
        }

        //Mouse Right Button Release
        if (Input.GetMouseButtonUp(0) && isRotatingASlice)
            StartCoroutine(EndRotation(Quaternion.Euler(RoundVectorNearestRightAngle(currentPivot.transform.localRotation.eulerAngles))));


        //Control the setup to all 6 axis and a shuffle(1)
        if (!hasSelectedACube && !isRotatingASlice && !isSlerpingASlice && !isSlerpingCube)
        {
            //Front
            if (Input.GetKeyDown(KeyCode.A))
                StartCoroutine(SetupRotation(Quaternion.Euler(0, 0, 0)));
            //Back
            if (Input.GetKeyDown(KeyCode.Z))
                StartCoroutine(SetupRotation(Quaternion.Euler(0, 180, 0)));

            //Left
            if (Input.GetKeyDown(KeyCode.E))
                StartCoroutine(SetupRotation(Quaternion.Euler(0, -90, 0)));
            //Right
            if (Input.GetKeyDown(KeyCode.R))
                StartCoroutine(SetupRotation(Quaternion.Euler(0, 90, 0)));

            //Top
            if (Input.GetKeyDown(KeyCode.T))
                StartCoroutine(SetupRotation(Quaternion.Euler(-90, 0, 0)));
            //Bottom
            if (Input.GetKeyDown(KeyCode.Y))
                StartCoroutine(SetupRotation(Quaternion.Euler(90, 0, 0)));

            //Shuffle
            if (Input.GetKeyDown(KeyCode.S))
                StartCoroutine(Shuffle(1));
        }

    }

    #endregion

    #region Setup Rotation
    public void Front()
    {
        StartCoroutine(SetupRotation(Quaternion.Euler(0, 0, 0)));
    }

    public void Back()
    {
        StartCoroutine(SetupRotation(Quaternion.Euler(0, 180, 0)));
    }

    public void Left()
    {
        StartCoroutine(SetupRotation(Quaternion.Euler(0, -90, 0)));
    }

    public void Right()
    {
        StartCoroutine(SetupRotation(Quaternion.Euler(0, 90, 0)));
    }

    public void Top()
    {
        StartCoroutine(SetupRotation(Quaternion.Euler(-90, 0, 0)));
    }

    public void Bottom()
    {
        StartCoroutine(SetupRotation(Quaternion.Euler(90, 0, 0)));
    }
    #endregion

    #endregion
}
