using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "TabletSO")]
public class TabletSO : ScriptableObject
{
    public string tablet_id;
    public string Title;
    public Sprite picture;
    public string description;
    public TransformData transformData;
}
