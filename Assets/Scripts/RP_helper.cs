using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
#if (UNITY_EDITOR)
[ExecuteInEditMode]
public class RP_helper : MonoBehaviour
{
    public Transform TerrainParent;

    public Material HDRP_TerrainMat;
    public Material URP_TerrainMat;

    public Material HDRP_Water;
    public Material URP_Water;

    public Material HDRP_SpikedDisk;
    public Material URP_SpikedDisk;

    public Material HDRP_Player;
    public Material URP_Player;

    public Material HDRP_Leaf;
    public Material URP_Leaf;

    public Light Sun;

    public GameObject SpikedDisk;
    public GameObject Water;
    public GameObject Player;
    public GameObject Leaf;

    public void hdrp() {
        Debug.Log("Switching to hdrp...");
        foreach (Transform terrain in TerrainParent) {
            Terrain terrainComp = terrain.gameObject.GetComponent<Terrain>();
            terrainComp.materialTemplate = HDRP_TerrainMat;
        }
        Sun.intensity = 10000;
        SM(SpikedDisk, HDRP_SpikedDisk);
        SM(Player, HDRP_Player);
        SM(Water, HDRP_Water);
        SM(Leaf, HDRP_Leaf);
    }

    public void urp() {
        Debug.Log("Switching to urp...");
        foreach (Transform terrain in TerrainParent)
        {
            Terrain terrainComp = terrain.gameObject.GetComponent<Terrain>();
            terrainComp.materialTemplate = URP_TerrainMat;
        }
        Sun.intensity = 1;
        SM(SpikedDisk, URP_SpikedDisk);
        SM(Player, URP_Player);
        SM(Water, URP_Water);
        SM(Leaf, URP_Leaf);
    }

    void SM(GameObject thing, Material mat) {
        thing.GetComponent<MeshRenderer>().material = mat;
        PrefabUtility.RecordPrefabInstancePropertyModifications(thing);
    }
}
#endif