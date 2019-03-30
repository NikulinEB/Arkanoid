using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewBlock", menuName = "Block", order = 52)]
public class Block : ScriptableObject
{
    public Sprite Sprite;
    public int HitCount;
}