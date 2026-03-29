using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class HouseOptimizer : MonoBehaviour {
    [ContextMenu("Combine House to New Mesh")]
    public void CombineHouse() {
        // 1. Find all MeshFilters in the house
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        if (meshFilters.Length == 0) {
            Debug.LogError("No meshes found to combine!");
            return;
        }
        
        // 2. Map every material to the meshes that use it
        Dictionary<Material, List<CombineInstance>> materialToMesh = new Dictionary<Material, List<CombineInstance>>();

        foreach (var filter in meshFilters) {
            MeshRenderer renderer = filter.GetComponent<MeshRenderer>();
            if (renderer == null || filter.sharedMesh == null) continue;

            // Handle objects with multiple materials (Submeshes)
            for (int s = 0; s < filter.sharedMesh.subMeshCount; s++) {
                Material mat = renderer.sharedMaterials[s];
                if (!materialToMesh.ContainsKey(mat)) {
                    materialToMesh[mat] = new List<CombineInstance>();
                }

                CombineInstance combine = new CombineInstance();
                combine.mesh = filter.sharedMesh;
                combine.subMeshIndex = s;
                // Aligns local position to the house parent
                combine.transform = transform.worldToLocalMatrix * filter.transform.localToWorldMatrix;
                materialToMesh[mat].Add(combine);
            }
        }

        // 3. Create the final combined object
        GameObject combinedObj = new GameObject("Combined_House_Optimized");
        MeshFilter finalMf = combinedObj.AddComponent<MeshFilter>();
        MeshRenderer finalMr = combinedObj.AddComponent<MeshRenderer>();

        List<Mesh> submeshes = new List<Mesh>();
        Material[] materials = new Material[materialToMesh.Count];
        int idx = 0;

        foreach (var entry in materialToMesh) {
            Mesh submesh = new Mesh();
            submesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            submesh.CombineMeshes(entry.Value.ToArray(), true, true);
            submeshes.Add(submesh);
            materials[idx] = entry.Key;
            idx++;
        }

        // 4. Group submeshes into one final mesh
        CombineInstance[] finalCombine = new CombineInstance[submeshes.Count];
        for (int i = 0; i < submeshes.Count; i++) {
            finalCombine[i].mesh = submeshes[i];
            finalCombine[i].transform = Matrix4x4.identity;
        }

        Mesh finalMesh = new Mesh();
        finalMesh.name = "CombinedHouseMesh";
        finalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        
        // 'false' keeps the materials separate!
        finalMesh.CombineMeshes(finalCombine, false); 
        finalMf.sharedMesh = finalMesh;
        finalMr.sharedMaterials = materials;

        // 5. Save the mesh to the Project folder so it's permanent
        #if UNITY_EDITOR
        string path = "Assets/CombinedHouseMesh_" + System.DateTime.Now.Ticks + ".asset";
        AssetDatabase.CreateAsset(finalMesh, path);
        AssetDatabase.SaveAssets();
        Debug.Log("<b>Success!</b> Reduced House to " + materials.Length + " materials. Mesh saved at: " + path);
        #endif
    }
}