using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    public static string rightGloveTag = "DefaultTag";
    public static string leftGloveTag = "DefaultTag";

    public static void ChangeRightGloveTag(string newTag)
    {
        rightGloveTag = newTag;
    }
    public static void ChangeLeftGloveTag(string newTag)
    {
        leftGloveTag = newTag;
    }
}
