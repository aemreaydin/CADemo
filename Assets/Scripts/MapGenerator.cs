using UnityEngine;

public class MapGenerator : MonoBehaviour {

    [SerializeField]
    private GameObject plane;
    [SerializeField]
    private int width = 100, height = 100;
    [SerializeField]
    [Range(0, 100)]
    private int randomCellPercent = 50;
    [SerializeField]
    private int numberOfSimulations = 10;


    int[,] Map;
    GameObject[,] GOMap;
    int currentSim;
    bool first = true;

    private void Start() {
        Map = new int[width, height];
        GOMap = new GameObject[width, height];
        GenerateMap();        
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            GenerateMap();
        }
    }
    // Create a map with the given width and height values than keep simulating the rules to smooth up the caves.
    private void GenerateMap() {

        if(!first) {
            for (int i = 0; i != width; i++) {
                for (int j = 0; j != height; j++) {
                    Destroy(GOMap[i, j].gameObject);
                }
            }
        }
        else {
            first = false;
        }

        currentSim = 0;
        // Generate a random map
        CreateRandomMap();
        //for(int i = 0; i != numberOfSimulations; i++) {
        //    ApplyRulesToMap();
        //}
        InvokeRepeating("ApplyRulesToMap", 1f, 0.5f);
    }
    // Adjust the map using a set of rules to generate a cave-like structure instead of random
    private void ApplyRulesToMap() {

            for (int i = 0; i != width; i++) {
                for (int j = 0; j != height; j++) {
                    int nWalls = CountSurroundingWalls(i, j);
                    if (nWalls > 4) {
                        Map[i, j] = 1;
                    }
                    else if (nWalls < 4) {
                        Map[i, j] = 0;
                    }
                }
            }
        //}

        for (int i = 0; i != width; i++) {
            for (int j = 0; j != height; j++) {
                if (Map[i, j] == 1) {
                    GOMap[i, j].GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                }
                else {
                    GOMap[i, j].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
            }
        }
        currentSim++;
        if (currentSim == numberOfSimulations) {
            CancelInvoke("ApplyRulesToMap");
            Debug.Log("Invoke Cancelled");
        }
    }
    // Create a random map using the randomCellPercent variable which can be adjusted in the Editor.
    // 1's are walls, 0's are cells.
    private void CreateRandomMap() {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        for(int i = 0; i != width; i++) {
            for(int j = 0; j != height; j++) {
                if(i == 0 || i == width - 1 || j == 0 || j == height - 1) {
                    Map[i, j] = 1;
                }
                else {
                    Map[i, j] = (UnityEngine.Random.Range(0, 100) < randomCellPercent) ? 0 : 1;
                }
            }
        }




        for (int i = 0; i != width; i++) {
            for (int j = 0; j != height; j++) {
                Vector3 pos = new Vector3(-width / 2f + 2 * i, 0, -height / 2f + 2 * j);
                //GOMap[i, j].GetComponent<Renderer>().material.shader = Shader.Find("_Color");
                if (Map[i, j] == 1) {

                    GOMap[i, j] = Instantiate(plane, pos, Quaternion.identity);
                    GOMap[i, j].transform.SetParent(this.gameObject.transform);
                    GOMap[i, j].GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                }
                else {
                    GOMap[i, j] = Instantiate(plane, pos, Quaternion.identity);
                    GOMap[i, j].transform.SetParent(this.gameObject.transform);
                    GOMap[i, j].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
            }
        }
    }

    // Generate a map using 2d planes to see whether the code is working or not.
    //private void OnDrawGizmos() {
    //    for (int i = 0; i != width; i++) {
    //        for (int j = 0; j != height; j++) {
    //            Vector3 cubePos = new Vector3(-width / 2 + i * 2, 0, -height / 2 + j * 2);
    //            Gizmos.color = (Map[i, j] == 1) ? Color.black : Color.white;
    //            Gizmos.DrawCube(cubePos, Vector3.one * 2f);
    //        }
    //    }
    //}

    // To implement the rule set we need some helper functions
    // GetSurroundingWallCount will return number of neighbor walls for a point.
    private int CountSurroundingWalls(int _x, int _y) {
        int wallCount = 0;

        for(int i = _x - 1; i <= _x + 1; i++) {
            for(int j = _y - 1; j <= _y + 1; j++) {
                if(IsInMap(i, j)) {
                    if(i != _x || j != _y) {
                        wallCount += Map[i, j];
                    }
                }
                else {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }
    // Check if the current point in in range of the map
    private bool IsInMap(int _x, int _y) {
        return _x >= 0 && _x < width && _y >= 0 && _y < height;
    }
}
