using System.Collections.Generic;
using UnityEngine;

/*
 * Creates houses representing classes and assigns fields to them
 **/ 
public class RenderClasses : MonoBehaviour
{
    // House Prefab
    public GameObject classHousePrefab;

    // List used to assigne house angle
    private List<Vector3> houseRotation = new List<Vector3>();

    // List use to keep track of where the houses are located
    private List<GameObject> houseList = new List<GameObject>();

    private void Start()
    {
        // Adding 90 degree angles to rotation list
        // 90 degrees are used so the door would be aligned with path
        houseRotation.Add(new Vector3(0, 90, 0));
        houseRotation.Add(new Vector3(0, 180, 0));
        houseRotation.Add(new Vector3(0, 270, 0));
        houseRotation.Add(new Vector3(0, 360, 0));
    }

    /*
     * Makes houses by looping through JSON
     **/
    public void AddHouse(JSONClass json)
    {
        float offset = 0;   // Value used in Vector3 for house positioning
        foreach (var classes in json.Classes)
        {

            // Initial position for each new house object
            Vector3 positioning = new Vector3(
                Random.Range(-45, 45),
                0,
                Random.Range(-45, 45)    
            );

            // Create House
            GameObject classHouse = Instantiate(
                classHousePrefab,
                positioning,
                transform.rotation = Quaternion.Euler(
                    houseRotation[Random.Range(0, houseRotation.Count)])
            );

            //Find if new house object collides with an existing one
            FindNewHousePosition(houseList, classHouse);

            // Add house to existing house list
            houseList.Add(classHouse);

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
     * Reposition the house in case of collisions
     **/ 
    void FindNewHousePosition(List<GameObject> housesList, GameObject newHouse)
    {

        // Collider for the newly created house
        Collider newHouseCollider = newHouse.GetComponent<Collider>();
        
        // Loop through all houses inside list
        foreach (GameObject house in housesList)
        {
            // Colliders for house already added to the map
            Collider existingHouse = house.GetComponent<Collider>();
            
            // Checking if houses collide
            if (newHouseCollider.bounds.Intersects(
                existingHouse.bounds))
            {
                // Finding the walls (max position) of the collision house 
                Vector3 collidePosition = existingHouse.bounds.max;

                // Adding 6 (house wall length) to collision Vector3, 
                // so the house don't intersect
                newHouse.transform.position = new Vector3(
                    collidePosition.x + 6,  
                    0,
                    collidePosition.z + 6
                );
            }
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