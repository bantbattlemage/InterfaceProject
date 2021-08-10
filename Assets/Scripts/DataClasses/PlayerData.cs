using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
	public int Id;
	public string Name = "";
}

[System.Serializable]
public class PlayerDataList
{
	public List<PlayerData> Players;
}