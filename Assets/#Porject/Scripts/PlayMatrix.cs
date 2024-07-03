using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMatrix
{
    static int sizeX = 7;
    static int sizeY = 8;
    static int sizeZ = 7;

    private static Cube[,,] matrix = new Cube[sizeX,sizeZ,sizeY];

    private static int[,] rows = new int[sizeX, sizeY];
    private static int[,] columns = new int[sizeZ,sizeY];

    private static Vector3 offset = new Vector3(sizeX / 2 - 0.5f, -0.5f, sizeZ / 2 - 0.5f);

    public static void AddCubes(List<Cube> cubes) {
        foreach (Cube cube in cubes) {
            Vector3 position = cube.transform.position;
            position.Scale(new Vector3(1/cube.transform.localScale.x, 1 / cube.transform.localScale.y, 1 / cube.transform.localScale.z));
            position -= offset;
            Debug.Log(position);
        }
    }
}
