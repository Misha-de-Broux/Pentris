using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class PlayMatrix : MonoBehaviour {
    [SerializeField] int sizeX = 7, sizeY = 8, sizeZ = 7;

    [SerializeField] float timeToFall = 1, timeToKill = 0.5f;

    [SerializeField] int easyMultiplier = 3, hardMultiplier = 5;

    Cube[,,] matrix;

    int[,] rows, columns;
    int[] levels;

    Vector3 offset;

    private void Start() {
        matrix = new Cube[sizeX, sizeY, sizeZ];
        rows = new int[sizeX, sizeY];
        columns = new int[sizeZ, sizeY];
        levels = new int[sizeY];
        offset = new Vector3(sizeX / 2, -0.5f, sizeZ / 2);
    }

    public void AddCubes(IEnumerable<Cube> cubes) {
        StartCoroutine(AddCubesCoroutine(cubes));
    }

    private IEnumerator AddCubesCoroutine(IEnumerable<Cube> cubes) {
        List<Vector3Int> newPositions = new List<Vector3Int>();
        foreach (Cube cube in cubes) {
            newPositions.Add(AddCube(cube));
        }

        Dictionary<Cube, List<int>> falls;
        List<Cube> cubesToRemove;
        int score;
        if (Data.IsHard) {
            HardUpdate(newPositions, out falls, out cubesToRemove, out score);
        } else {
            EasyUpdate(newPositions, out falls, out cubesToRemove, out score);
        }
        if (score > 0) {
            Data.Score += score;
        }
        int runningRoutines = 0;
        foreach (Cube cube in cubesToRemove.Distinct()) {
            RemoveCube(cube);
            StartCoroutine(StartWaitCoroutine(cube.KillCoroutine(timeToKill)));
        }
        while (runningRoutines > 0) {
            yield return null;
        }
        foreach (Cube cube in falls.Keys) {
            if (cube != null) {
                RemoveCube(cube);
                StartCoroutine(StartWaitCoroutine(cube.FallCoroutine(falls[cube].Distinct().Count(), timeToFall)));
            }
        }
        while (runningRoutines > 0) {
            yield return null;
        }
        if (falls.Count > 0) {
            AddCubes(falls.Keys);
        }
        //DebugMatrix();

        IEnumerator StartWaitCoroutine(IEnumerator Coroutine) {
            Debug.Log("startwait");
            runningRoutines++;
            yield return Coroutine;
            runningRoutines--;
        }
    }

    private void EasyUpdate(List<Vector3Int> newPositions, out Dictionary<Cube, List<int>> falls, out List<Cube> cubesToRemove, out int score) {
        falls = new Dictionary<Cube, List<int>>();
        cubesToRemove = new List<Cube>();
        score = 1;
        foreach (Vector3Int position in newPositions) {
            if (rows[position.x, position.y] == sizeX) {
                score *= easyMultiplier;
                for (int z = 0; z < sizeZ; z++) {
                    cubesToRemove.Add(matrix[position.x, position.y, z]);
                }
                for (int y = position.y + 1; y < sizeY; y++) {
                    for (int z = 0; z < sizeZ; z++) {
                        if (matrix[position.x, y, z] != null) {
                            if (!falls.ContainsKey(matrix[position.x, y, z])) {
                                falls[matrix[position.x, y, z]] = new List<int>();
                            }
                            falls[matrix[position.x, y, z]].Add(position.y);
                        }
                    }
                }
            }
            if (columns[position.z, position.y] == sizeZ) {
                score *= easyMultiplier;
                for (int x = 0; x < sizeX; x++) {
                    cubesToRemove.Add(matrix[x, position.y, position.z]);
                }
                for (int y = position.y + 1; y < sizeY; y++) {
                    for (int x = 0; x < sizeX; x++) {
                        if (matrix[x, y, position.z] != null) {
                            if (falls.ContainsKey(matrix[x, y, position.z])) {
                                falls[matrix[x, y, position.z]] = new List<int>();
                            }
                            falls[matrix[x, y, position.z]].Add(position.y);
                        }
                    }
                }
            }
        }
        score /= easyMultiplier;
    }

    private void HardUpdate(List<Vector3Int> newPositions, out Dictionary<Cube, List<int>> falls, out List<Cube> cubesToRemove, out int score) {
        falls = new Dictionary<Cube, List<int>>();
        cubesToRemove = new List<Cube>();
        score = 1;
        foreach (Vector3Int position in newPositions) {
            if (levels[position.y] == sizeX * sizeZ) {
                score *= hardMultiplier;
                for (int x = 0; x < sizeX; x++) {
                    for (int z = 0; z < sizeZ; z++) {
                        cubesToRemove.Add(matrix[x, position.y, z]);
                        for (int y = position.y+1; y<sizeY; y++) {
                            if (matrix[x, y, z] != null) {
                                if (!falls.ContainsKey(matrix[x, y, z])) {
                                    falls[matrix[x, y, z]] = new List<int>();
                                }
                                falls[matrix[x, y, z]].Add(position.y);
                            }
                        }
                    }
                }
            }
            
        }
        foreach (Vector3Int position in newPositions) {
           
        }
        score /= hardMultiplier;
    }

    private void DebugMatrix() {
        string matrixStr = "";
        string columnsStr = "";
        string rowsStr = "";
        for (int y = 0; y < sizeY; y++) {
            matrixStr += "[";
            for (int x = 0; x < sizeX; x++) {
                rowsStr += $"{rows[x, y]}, ";
                for (int z = 0; z < sizeZ; z++) {
                    if (x == 0) {
                        columnsStr += $"{columns[z, y]}, ";
                    }
                    matrixStr += $"{(matrix[x, y, z] == null ? 0 : 1)}, ";
                }
                matrixStr += "\n";
            }
            rowsStr += "\n";
            columnsStr += "\n";
            matrixStr += "]\n\n";
        }
        Debug.Log(matrixStr);
        Debug.Log(columnsStr);
        Debug.Log(rowsStr);
    }

    private Vector3Int GetPosition(Cube cube) {
        Vector3 positionFloat = cube.transform.position;
        positionFloat.Scale(new Vector3(1 / cube.transform.localScale.x, 1 / cube.transform.localScale.y, 1 / cube.transform.localScale.z));
        positionFloat += offset;
        return new Vector3Int(Mathf.RoundToInt(positionFloat.x), Mathf.RoundToInt(positionFloat.y), Mathf.RoundToInt(positionFloat.z));
    }

    private Vector3Int AddCube(Cube cube) {
        Vector3Int position = GetPosition(cube);
        matrix[position.x, position.y, position.z] = cube;
        rows[position.x, position.y]++;
        columns[position.z, position.y]++;
        levels[position.y]++;
        return position;
    }

    private void RemoveCube(Cube cube) {
        Vector3Int position = GetPosition(cube);
        if (matrix[position.x, position.y, position.z] != null) {
            matrix[position.x, position.y, position.z] = null;
            rows[position.x, position.y]--;
            columns[position.z, position.y]--;
            levels[position.y]--;
        }
    }
}
