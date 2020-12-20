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
    float rotationSpeedSlice = 220;
    [SerializeField]
    float slerpSliceScale = 5;
    [SerializeField]
    float slerpSliceShuffleScale = 2;
    [SerializeField]
    float slerpCubeScale = 1;
    [SerializeField]
    float minimumMouseOffset = 2;
    [SerializeField]
    int size = 5;

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

    List<GameObject> pivotsAxisX = new List<GameObject>();
    List<GameObject> pivotsAxisY = new List<GameObject>();
    List<GameObject> pivotsAxisZ = new List<GameObject>();
    List<GameObject> allCubes = new List<GameObject>();

    List<GameObject> selectedCubes = new List<GameObject>();
    GameObject selectedCube = null;
    GameObject currentPivot = null;


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
        size = PlayerPrefs.GetInt("CubeSize");
        GenerateCubes();
    }
    void GenerateCubes()
    {
        //calculate MaxOffSet
        float maxOffSet = -0.5f * (size - 1);

        //generate Pivots
        GeneratePivots(maxOffSet);

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

        if (isRotatingVerticalSlice)
            SetVerticalSlices();
        else
            SetHorizontalSlices();

        SetPivot(isRotatingVerticalSlice);
        foreach (GameObject cube in selectedCubes)
            cube.transform.parent = currentPivot.transform;
    }
    void SetVerticalSlices()
    {
        MouseAxis = Vector3.up;
        int sizeSquared = size * size;

        //fill array
        if (selectedCubeNormal == Vector3.right || selectedCubeNormal == Vector3.left)
        {
            currentPivotAxis = Vector3.back;
            int numberOfTheDepth = (allCubes.IndexOf(selectedCube) % sizeSquared) % size;


            for (int i = numberOfTheDepth; i < allCubes.Count; i += size)
                selectedCubes.Add(allCubes[i]);
        }
        else
        {
            currentPivotAxis = Vector3.right;
            //variable in int to remove decimals
            int numberOfTheColumn = allCubes.IndexOf(selectedCube) / sizeSquared;

            for (int i = numberOfTheColumn * sizeSquared; i < numberOfTheColumn * sizeSquared + sizeSquared; ++i)
                selectedCubes.Add(allCubes[i]);
        }
    }
    void SetHorizontalSlices()
    {
        MouseAxis = Vector3.right;
        int sizeSquared = size * size;


        //fill array
        if (selectedCubeNormal == Vector3.down || selectedCubeNormal == Vector3.up)
        {
            currentPivotAxis = Vector3.forward;
            int numberOfTheDepth = (allCubes.IndexOf(selectedCube) % sizeSquared) % size;

            for (int i = sizeSquared - size + numberOfTheDepth; i < allCubes.Count; i += sizeSquared)
                for (int j = 0; j < size; ++j)
                    selectedCubes.Add(allCubes[i - j * size]);
        }
        else
        {
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
    IEnumerator EndRotation()
    {
        float waitedTime = 0f;
        isRotatingASlice = false;
        isSlerpingASlice = true;

        Quaternion initailRotaion = currentPivot.transform.localRotation;
        // scale the three euler angle to the nearest 90 degree
        Quaternion finalRotation = Quaternion.Euler(RoundVectorNearestRightAngle(currentPivot.transform.localRotation.eulerAngles));

        while (waitedTime <= 1)
        {
            waitedTime += Time.deltaTime * slerpSliceScale;
            currentPivot.transform.localRotation = Quaternion.Slerp(initailRotaion, finalRotation, waitedTime);
            yield return null;
        }

        foreach (GameObject cube in selectedCubes)
            cube.transform.parent = transform;

        ReajustArrayMap();
        isSlerpingASlice = false;
        currentPivot = null;
        selectedCube = null;
        selectedCubes.Clear();
        rotationOverAll = Vector3.zero;
        CheckIfSolved();
    }

    void CheckIfSolved()
    {
        //A Rubick's cube is solved if all the cubes composing it have the same Rotation
        Quaternion rotationToHave = allCubes[0].transform.rotation;

        //we can start from i = 1 as allcubes[0] will always have the same rotation as himself
        for (int i = 1; i < allCubes.Count; ++i)
            if (allCubes[i].transform.rotation != rotationToHave)
                return;

        Debug.Log("VICTORY");
    }

    IEnumerator EndRotationShuffle(Quaternion finalRotation)
    {
        float waitedTime = 0f;
        isRotatingASlice = false;
        isSlerpingASlice = true;

        Quaternion initailRotaion = currentPivot.transform.localRotation;

        while (waitedTime <= 1)
        {
            waitedTime += Time.deltaTime * slerpSliceShuffleScale;
            currentPivot.transform.localRotation = Quaternion.Slerp(initailRotaion, finalRotation, waitedTime);
            yield return null;
        }

        foreach (GameObject cube in selectedCubes)
            cube.transform.parent = transform;

        ReajustArrayMap();
        isSlerpingASlice = false;
        currentPivot = null;
        selectedCube = null;
        selectedCubes.Clear();
        rotationOverAll = Vector3.zero;
    }

    IEnumerator SetupRotation(Quaternion finalRotation)
    {
        float waitedTime = 0f;
        isSlerpingCube = true;

        Quaternion initailRotaion = transform.rotation;

        while (waitedTime <= 1)
        {
            waitedTime += Time.deltaTime * slerpCubeScale;
            transform.rotation = Quaternion.Slerp(initailRotaion, finalRotation, waitedTime);
            yield return null;
        }

        isSlerpingCube = false;
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
    Vector3 RoundVectorNearestRightAngle(Vector3 vector)
    {
        return new Vector3(Mathf.Round(vector.x / 90) * 90,
                           Mathf.Round(vector.y / 90) * 90,
                           Mathf.Round(vector.z / 90) * 90);
    }


    IEnumerator Shuffle(int numberOfShuffle)
    {
        for (int i = 0; i < numberOfShuffle; ++i)
        {
            //get a random direction in 3d space
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector3 randomCameraPos = transform.position + direction * 100;

            RaycastHit hit;
            Ray ray = new Ray(randomCameraPos, -direction);
            Physics.Raycast(ray, out hit, 200);
            selectedCube = hit.collider.gameObject;
            selectedCubeNormal = transform.worldToLocalMatrix * hit.normal;


            //selectedCube = allCubes[Random.Range(0, allCubes.Count)];
            StartRotation(Random.Range(0, 2) == 1 ? true : false);
            rotationOverAll = currentPivotAxis * (Random.Range(1, 4) * 90);
            yield return StartCoroutine(EndRotationShuffle(currentPivot.transform.localRotation * Quaternion.Euler(rotationOverAll)));
        }
    }


    void Update()
    {

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
            StartCoroutine(EndRotation());

        //Mouse Right Button Hold
        //Quat * rotation in this order to rotate in World
        if (Input.GetMouseButton(1))
            transform.rotation = Quaternion.Euler(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeedCube)
                                 * transform.rotation;

        //Control the setup to all 6 axis
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
                StartCoroutine(Shuffle(5));
        }

    }
    #endregion

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
}
