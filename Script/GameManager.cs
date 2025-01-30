using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Characters;

    void Start()
    {
        Characters = new List<GameObject>();
    }

    void Update()
    {
        foreach (GameObject character in Characters)
        {
            // calcular apenas os que estão perto do player e visiveis ao player
            Debug.Log(character.name);
        }
    }
}