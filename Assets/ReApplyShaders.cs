using UnityEngine;
using System.Collections;


public static class ReApplyShaders
{
  public static void ReWrite(GameObject target)
  {
    Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
    Material[] materials;
    string[] shaders;

    foreach (var rend in renderers)
    {
      materials = rend.sharedMaterials;
      shaders = new string[materials.Length];

      for (int i = 0; i < materials.Length; i++)
      {
        shaders[i] = materials[i].shader.name;
      }

      for (int i = 0; i < materials.Length; i++)
      {
        materials[i].shader = Shader.Find(shaders[i]);
      }
    }
  }
}