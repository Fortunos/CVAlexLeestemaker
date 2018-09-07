using UnityEngine;

public static class RespawnHelper
{
    private static Vector3 checkpoint, gravreset;
    private static Quaternion rotationReset;
    private static string levelname = "";

    public static void Set(Transform self, Transform trigger, string name)
    {
        checkpoint = trigger.position;
        gravreset = Physics.gravity;
        rotationReset = self.rotation;
        levelname = name;
    }

    public static Vector3 GetPosition
    {
        get { return checkpoint; }
    }

    public static Vector3 GetGravity
    {
        get { return gravreset; }
    }

    public static Quaternion GetRotation
    {
        get { return rotationReset; }
    }

    public static string GetName
    {
        get { return levelname; }
    }
}
