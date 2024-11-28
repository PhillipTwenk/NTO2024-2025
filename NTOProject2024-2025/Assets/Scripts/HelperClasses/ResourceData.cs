using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceData
{
   public List<int> resources;
   public List<int> SpriteAssetsUsingIndex; // Индексы спрайтов в шрифте TMPro, соответствующие порядку спрайтов при их появлении на панели 
}
