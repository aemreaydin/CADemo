using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayOfLife : MonoBehaviour {

    [SerializeField]
    private GameObject plane;
    [SerializeField]
    private int width = 100, height = 100;
    
    int[,] Map;
    GameObject[,] GOMap;

    private void Start() {        
        GenerateMap();
        InvokeRepeating("ApplyRulesToMap", 1f, .25f);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GenerateMap();
        }
    }

    private void GenerateMap() {
        Map = new int[width, height];
        for(int i = 0; i != width; i++) {
            for(int j = 0; j != height; j++) {
                Map[i, j] = 1;
            }
        }

        // Create a blinker which oscillates with period of 2
        Map[3, 30] = 0;
        Map[3, 31] = 0;
        Map[3, 32] = 0;

        // Create a beehive which stays still
        Map[10, 10] = 0;
        Map[10, 11] = 0;
        Map[11, 9] = 0;
        Map[11, 12] = 0;
        Map[12, 10] = 0;
        Map[12, 11] = 0;

        // Create a beacon oscillates with period of 2
        Map[20, 20] = 0;
        Map[20, 21] = 0;
        Map[21, 20] = 0;
        Map[21, 21] = 0;
        Map[22, 22] = 0;
        Map[22, 23] = 0;
        Map[23, 22] = 0;
        Map[23, 23] = 0;

        // Create a pentadecathlon oscillates with a period of 15
        Map[40, 30] = 0;
        Map[40, 31] = 0;
        Map[39, 32] = 0;
        Map[41, 32] = 0;
        Map[40, 33] = 0;
        Map[40, 34] = 0;
        Map[40, 35] = 0;
        Map[40, 36] = 0;
        Map[39, 37] = 0;
        Map[41, 37] = 0;
        Map[40, 38] = 0;
        Map[40, 39] = 0;

        // Create a glider which constanly moves
        Map[2, 3] = 0;
        Map[3, 4] = 0;
        Map[4, 2] = 0;
        Map[4, 3] = 0;
        Map[4, 4] = 0;

        Map[32, 3] = 0;
        Map[33, 4] = 0;
        Map[34, 2] = 0;
        Map[34, 3] = 0;
        Map[34, 4] = 0;

        GOMap = new GameObject[width, height];
        for(int i = 0; i != width; i++) {
            for(int j = 0; j != height; j++) {
                Vector3 pos = new Vector3(-width / 2f + 2 * i, 0, -height / 2f + 2 * j);
                //GOMap[i, j].GetComponent<Renderer>().material.shader = Shader.Find("_Color");
                if (Map[i,j] == 1) {                    
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
    // Adjust the map using a set of rules from Conway's Game of Life
    private void ApplyRulesToMap() {
        // Copy the Map to a new array to apply the rules
        int[,] newMap = Map.Clone() as int[,];
        for (int i = 0; i != width; i++) {
            for (int j = 0; j != height; j++) {
                int aliveCells = CountAliveCells(i, j);
                
                if (Map[i, j] == 0) {
                    
                    if (aliveCells < 2) {
                        newMap[i, j] = 1;
                    }
                    else if (aliveCells == 2 || aliveCells == 3) {
                        newMap[i, j] = 0;
                        //Debug.Log(i + " " + j);
                    }
                    else if (aliveCells > 3) {
                        newMap[i, j] = 1;
                    }
                }
                if(Map[i, j] == 1 && aliveCells == 3) {
                    newMap[i, j] = 0;
                }
            }
        }
        Map = newMap;
        for (int i = 0; i != width; i++) {
            for (int j = 0; j != height; j++) {
                if(Map[i, j] == 1) {
                    GOMap[i, j].GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                }
                else {
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
    //            if(Map[i, j] == 1) {
    //                Gizmos.color = Color.black;
    //            }
    //            else {
    //                Gizmos.color = Color.white;
    //            }
    //            Gizmos.DrawCube(cubePos, Vector3.one * 2f);
    //        }
    //    }
    //}
    // Count the alive cells
    private int CountAliveCells(int _x, int _y) {
        int aliveCount = 0;

        for (int i = _x - 1; i <= _x + 1; i++) {
            for (int j = _y - 1; j <= _y + 1; j++) {
                if (IsInMap(i, j)) {
                    if (i != _x || j != _y) {
                        if(Map[i, j] == 0) {
                            aliveCount++;
                        }
                    }
                }
            }
        }

        return aliveCount;
    }
    // Check if the current point in in range of the map
    private bool IsInMap(int _x, int _y) {
        return _x >= 0 && _x < width && _y >= 0 && _y < height;
    }
}
