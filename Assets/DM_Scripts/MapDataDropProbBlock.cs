
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
public class MapDataDropProbBlock : MapDataDropBlock
{
	public int prob;

	public MapDataDropProbBlock(ChipType chipType, int chipColor, int prob)
		: base(chipType, chipColor)
	{
		this.prob = prob;
	}
}
