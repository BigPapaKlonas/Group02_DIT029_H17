using System.Collections;
using UnityEngine;

public class StartMessages : MonoBehaviour
{

    public GameObject activationBoxPrefab;
    private float contentCount;

    public void NewMessage(QuickType.JSON json)
    {

        // Check for parallelism:
        if (json.Diagram.Content.Count > 1)
        {

            float offset = 0;
            contentCount = json.Diagram.Content.Count - 1;

            // Add parallel box
            GameObject parBox = (GameObject)Instantiate(Resources.Load("ParallelBox 1"));
            float trinity = 3f;
            foreach (var content in json.Diagram.Content)
            {
                Queue destList = new Queue();
                foreach (var names in content.SubContent)
                {
                    GameObject tmpFrom = GameObject.Find(names.From);
                    GameObject tmpTo = GameObject.Find(names.To);
                    destList.Enqueue(tmpFrom);
                    destList.Enqueue(tmpTo);
                }
                StartMessageChain(destList, offset);
                offset += json.Diagram.Content.Count + 0.5f;
                PlaceParLine(json, offset, parBox);
                trinity += 3f;
            }
            // No parallelism:
            print("child 1 position " + parBox.transform.GetChild(1).localPosition);
            print("child 6 position " + parBox.transform.GetChild(6).localPosition);

        }
        else
        {
            foreach (var content in json.Diagram.Content)
            {
                Queue destList = new Queue();
                foreach (var names in content.SubContent)
                {
                    GameObject tmpFrom = GameObject.Find(names.From);
                    GameObject tmpTo = GameObject.Find(names.To);
                    destList.Enqueue(tmpFrom);
                    destList.Enqueue(tmpTo);
                }
                StartMessageChain(destList, 0f);
            }
        }
    }

    void StartMessageChain(Queue queue, float yOffset)
    {

        if (queue.Count > 0)
        {

            GameObject first = (GameObject)queue.Dequeue();

            Vector3 positioning = new Vector3(
              first.transform.position.x,
              first.transform.position.y - 1 - yOffset,
              first.transform.position.z
            );

            GameObject activationBoxGO = (GameObject)Instantiate(
              activationBoxPrefab,
              positioning,
              this.transform.rotation
            );

            ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
            p.destList = queue;

        }
    }

    void PlaceParLine(QuickType.JSON json, float positionY, 
        GameObject parBox)
    {
        if(contentCount <= 0)
        {
            return;
        }

        if (json.Diagram.Node.Equals("par"))
        {
            AddParallelLine line = new AddParallelLine
            {
                parallelBox = parBox.transform,
                cube = parBox.GetComponent<MeshFilter>().mesh,
                linePosition = positionY
            };

            line.AddLine(positionY);
            contentCount--;
        }
    }
}
