using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleClassHouse : MonoBehaviour
{

    public GameObject classHousePrefab;

    public void AddHouse(JSONClass json)
    {
        float zOffset = 0;
        foreach (var classy in json.Classes)
        {
            Queue destList = new Queue();
            Queue destListPar = new Queue();

            InstantiateHouses(classy.Name, zOffset, classy.Fields);
            zOffset += 10;
        }
    }
    
    void InstantiateHouses(string className, float zOffset, Field[] classFields)
    {
        Vector3 positioning = new Vector3(
            1,
            0,
            zOffset
        );
        GameObject classHouse = (GameObject)Instantiate(
            classHousePrefab,
            positioning,
            this.transform.rotation
        );
        classHouse.name = className;
        classHouse.GetComponentInChildren<TextMesh>().text = className;

    }


}