using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NON UTILIZZATO, LO TENGO PER RICORDARMI DELLA POSSIBILITA' DI UTILIZZARE IL GRID IN MODO DINAMICO
public class TestScript : MonoBehaviour
{
    Grid grid;
    [SerializeField] GameObject sprite;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    // Start is called before the first frame update
    void Start()
    {
        var pos = grid.GetCellCenterWorld(new Vector3Int(0, 0, 0));

        Instantiate(sprite, pos, Quaternion.identity);
    }
}
