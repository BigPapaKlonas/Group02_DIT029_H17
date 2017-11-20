using UnityEngine;
using UnityEditor;

public class Artifact 
{
    public string name;
    public string classType;
    public Artifact(string n, string c)
    {
        name = n;
        classType = c;
    }
}