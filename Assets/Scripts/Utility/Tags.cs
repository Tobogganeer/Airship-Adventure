using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    [SerializeField]
    private List<string> tags;
    private HashSet<string> tagsHashset;

    private void Awake()
    {
        if (tags.Count == 0) return;

        tagsHashset = new HashSet<string>(tags.Count);
        foreach (string tag in tags)
            tagsHashset.Add(tag);
    }

    public bool HasTag(string tag) => tagsHashset != null && tagsHashset.Contains(tag);
}

public static class TagsExtensions
{
    public static bool HasTag(this Component comp, string tag)
    {
        return comp.CompareTag(tag) || comp.TryGetComponent(out Tags tags) && tags.HasTag(tag);
    }
}