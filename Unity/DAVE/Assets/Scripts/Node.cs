using UnityEngine;
using UnityEditor;

public class Node
{
    public string name;
    public string cat;
    public Artifact[] arts;

    public Node(string n, string c, Artifact[] a)
    {
        name = n;
        cat = c;
        arts = a;
    }

}