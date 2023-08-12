using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ViewScript : MonoBehaviour
{
   
}

[System.Serializable]
public class Data
{
    public List<int> gridSize { get; set; }
    public List<int> time { get; set; }
    public List<List<int>> pieceIDs { get; set; }
    //public int[] pieceAngles { get; set; }
   
}



