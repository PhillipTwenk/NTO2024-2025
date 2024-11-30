using System.Collections.Generic;

[System.Serializable]
public class ShopLog
{
    public string comment;
    public string player_name;
    public string shop_Name;
    public Dictionary<string, string> resources_changed;
}
