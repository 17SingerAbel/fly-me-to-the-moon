using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainControl : MonoBehaviour
{
    public Transform[] terrains;
    public Transform player;
    public Transform centerTarrain;
    public float terrainWidth = 40f;
    public float terrainHeight = 20f;

    int checkPlayerPosition()
    {
        Vector3 direction = new Vector3(0, 0, 0);
        if (player.position.z - centerTarrain.position.z > terrainWidth / 2)
        {
            direction.z = 1;
        }
        else if (centerTarrain.position.z - player.position.z > terrainWidth / 2)
        {
            direction.z = -1;
        }

        if (player.position.x - centerTarrain.position.x > terrainHeight / 2)
        {
            direction.x = 1;
        }
        else if (centerTarrain.position.x - player.position.x > terrainHeight / 2)
        {
            direction.x = -1;
        }
        if (Vector3.Magnitude(direction) == 0)
        {
            return 4;
        }
        else if (Vector3.Magnitude(direction) == 1)
        {
            if (direction.x > 0)
            {
                return 7;
            }
            else if (direction.x < 0)
            {
                return 1;
            }
            else if (direction.z > 0)
            {
                return 5;
            }
            else
            {
                return 3;
            }
        }
        else if (Vector3.Magnitude(direction) > 1)
        {
            if (direction.x > 0 && direction.z > 0)
            {
                return 8;
            }
            else if (direction.x > 0 && direction.z < 0)
            {
                return 6;
            }
            else if (direction.x < 0 && direction.z < 0)
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }
        return 4;

    }

    // Start is called before the first frame update
    void Start()
    {
        centerTarrain = terrains[4];
    }

    // Update is called once per frame
    void Update()
    {
        int center = checkPlayerPosition();
        if (center != 4)
        {
            Vector3 translation = terrains[center].position - centerTarrain.position;
            // if (center != 4) {
            //     Transform[] newTerrains = new Transform[9];
            //     for (int i=center; i < 9 + center; i++) {
            //         newTerrains[(4+ i - center) % 8 ] = terrains[center % 8]; 

            //     }
            //     terrains = newTerrains;
            // }
            for (int i = 0; i < 9; i++)
            {
                terrains[i].position += translation;
            }
        }
    }

}

