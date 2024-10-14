using System;

[Serializable]
public class ReqPacketSaveGameData : ReqPacketBase
{
	public string ab_test = "a";

	public string collect_data;

	public int game_mode;

	public int gid;

	public int map_count;

	public string map_mode;

	public int move_count;

	public int res_level;

	public int res_star;

	public int star_point_1;

	public int star_point_2;

	public int star_point_3;

	public string type;

	public ReqPacketSaveGameData(MapData mapData)
	{
		gid = mapData.gid;
		game_mode = (int)mapData.target;
		move_count = mapData.moveCount;
		map_count = mapData.SubMapCount;
		res_level = mapData.bonusOpenConditionLevel;
		res_star = mapData.bonusOpenConditionStarCount;
		star_point_1 = mapData.scorePoint[0];
		star_point_2 = mapData.scorePoint[1];
		star_point_3 = mapData.scorePoint[2];
		collect_data = mapData.GetCollectJsonFormat();
		map_mode = mapData.GetParamJsonFormat();
		ab_test = "a";
	}

	public ReqPacketSaveGameData()
	{
	}
}
