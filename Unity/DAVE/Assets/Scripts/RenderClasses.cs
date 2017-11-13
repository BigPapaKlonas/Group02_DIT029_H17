using System.Collections;
using UnityEngine;

public class RenderClasses : MonoBehaviour
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


        ShapeTopWalls(classHouse.transform, classFields.Length);
        RenderFields(classHouse.transform, classFields);

    }

    void ShapeTopWalls(Transform wallParent, int fieldCount)
    {
        Transform ceiling = wallParent.transform.Find("Ceiling");
        Transform topFront = wallParent.transform.Find("TopFront");
        Transform topBack = wallParent.transform.Find("TopBack");
        Transform topLeft = wallParent.transform.Find("TopLeft");
        Transform topRight = wallParent.transform.Find("TopRight");

        topFront.localScale = new Vector3(6, 2 * fieldCount, 0.01f);
        topFront.position = new Vector3(topFront.position.x,
            2.5f + topFront.localScale.y / 2, topFront.position.z);

        topBack.localScale = new Vector3(6, 2 * fieldCount, 0.01f);
        topBack.position = new Vector3(topBack.position.x,
            2.5f + topBack.localScale.y / 2, topBack.position.z);

        topLeft.localScale = new Vector3(6, 2 * fieldCount, 0.01f);
        topLeft.position = new Vector3(topLeft.position.x,
           2.5f + topLeft.localScale.y / 2, topLeft.position.z);

        topRight.localScale = new Vector3(6, 2 * fieldCount, 0.01f);
        topRight.position = new Vector3(topRight.position.x,
           2.5f + topRight.localScale.y / 2, topRight.position.z);

        ceiling.position = new Vector3(ceiling.position.x,
            (topFront.position.y + topFront.localScale.y / 2 - 0.1f), ceiling.position.z);
    }

    void RenderFields(Transform parent, Field[] fieldList)
    {
        Transform textWall = parent.Find("TopBack");
        string fields = "";
        foreach (var field in fieldList)
        {
            fields += field.Name + " : " + field.Type + "\n";
        }
        textWall.GetComponentInChildren<TextMesh>().text = fields;
    }
}