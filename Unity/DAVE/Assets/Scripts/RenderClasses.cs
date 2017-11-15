using System.Collections;
using UnityEngine;

/*
 * Creates houses representing classes and assigns fields to them
 **/ 
public class RenderClasses : MonoBehaviour
{
    // House Prefab
    public GameObject classHousePrefab;

    /*
     * Makes houses by looping trhough JSON
     **/ 
    public void AddHouse(JSONClass json)
    {
        float offset = 0;   // Value used in Vector3 for house positioning
        foreach (var classes in json.Classes)
        {
            // Make Vector3 to specify the houses position
            Vector3 positioning = new Vector3(
                offset,
                0,
                offset
            );

            // Create House
            GameObject classHouse = (GameObject)Instantiate(
                classHousePrefab,
                positioning,
                transform.rotation
            );

            // Change the name of the house in Hierarchy
            classHouse.name = classes.Name;

            // Change the name of the house on the wall above the door
            classHouse.GetComponentInChildren<TextMesh>().text = classes.Name;

            // Calling method to rescale house walls based on field amount
            ResizeTopWalls(classHouse.transform, classes.Fields.Length);

            //Calling method to add fields to wall inside of the house, if there are any
            if(classes.Fields.Length > 0)
            {
                AddFields(classHouse.transform, classes.Fields);
            }

            // Increase offset for next loop iteration so houses don't overlap
            offset += 10;
        }
    }

    /*
     * Resizes houses' top walls based on the amount of fields each class has
     **/ 
    void ResizeTopWalls(Transform houseParent, int fieldCount)
    {
        // Getting the children Transform of the top walls
        Transform ceiling = houseParent.transform.Find("Ceiling");
        Transform topFront = houseParent.transform.Find("TopFront");
        Transform topBack = houseParent.transform.Find("TopBack");
        Transform topLeft = houseParent.transform.Find("TopLeft");
        Transform topRight = houseParent.transform.Find("TopRight");

        // Changing wall scales
        topFront.localScale = new Vector3(6, 2 * fieldCount, 0.01f);
        topBack.localScale = new Vector3(6, 2 * fieldCount, 0.01f);
        topLeft.localScale = new Vector3(6, 2 * fieldCount, 0.01f);
        topRight.localScale = new Vector3(6, 2 * fieldCount, 0.01f);

        // Getting the middle y point of the top walls ie how far it goes 1way along y-axis
        float topWallPoint = topFront.localScale.y / 2;

        // Changing wall global possitioning
        // 2.5 multiplier used to position top walls above bottom walls 
        topFront.position = new Vector3(
            topFront.position.x,
            2.5f + topWallPoint,    
            topFront.position.z);

        topBack.position = new Vector3(
            topBack.position.x,
            2.5f + topWallPoint,
            topBack.position.z);

        topLeft.position = new Vector3(
            topLeft.position.x,
            2.5f + topWallPoint,
            topLeft.position.z);

        topRight.position = new Vector3(
            topRight.position.x,
            2.5f + topWallPoint,
            topRight.position.z);

        // Changing the ceiling height to match the heighest point of the top walls
        ceiling.position = new Vector3(
            ceiling.position.x,
            (topFront.position.y + topWallPoint - 0.1f),
            ceiling.position.z);
    }

    /*
     * Adds class fields to the inside of a house 
     **/ 
    void AddFields(Transform parent, Field[] fieldList)
    {
        // Getting the Transform of the backwall with a TextMesh on it
        Transform textWall = parent.Find("TopBack");

        // Initial string
        string fields = "";

        // Looping through the list
        foreach (var field in fieldList)
        {
            // Adding each new field to a new line
            fields += field.Name + " : " + field.Type + "\n";
        }
        // Adding the field text to the TextMesh
        textWall.GetComponentInChildren<TextMesh>().text = fields;
    }
}