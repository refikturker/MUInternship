using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEditor;

public class ShapeStorageScript : MonoBehaviour
{
    public List<ShapeDataScript> shapeData;
    public List<ShapeScript> shapeList;

    public TextAsset jsonFile;

    public List<GameObject> shapePrefabs;


    public int level;


    public void ShapeCreator()
    {
        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(jsonFile.text);

        int countLimit = Mathf.Min(shapeList.Count, data.pieceIDs[level].Count);

        if(level < 13)
        {
            for (int i = 0; i < countLimit; i++)
            {
                ShapeScript shape = shapeList[i];
                shape.MoveShapeToStartPosition();
            }

            for (int count = 0; count < countLimit; count++)
            {
                int shapeIndex = data.pieceIDs[level][count];
                ShapeScript shape = shapeList[count];
                shape.CreateShape(shapeData[shapeIndex - 1]);
            }
        }
    }

    public void DeleteSquareImagesInShapes()
    {
        foreach (GameObject shapePrefab in shapePrefabs)
        {
            ShapeScript shapeComponent = shapePrefab.GetComponent<ShapeScript>();
            if (shapeComponent != null)
            {
                shapeComponent.DeleteSquareImages();
            }
        }
    }

}
