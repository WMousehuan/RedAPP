using PathologicalGames;
using System.Collections;

public class OreoCracker : Chip
{
	public override void Awake()
	{
		base.Awake();
		chipType = ChipType.OreoCracker;
		destroyable = false;
	}

	public override void Update()
	{
		base.Update();
		if (!destroing && (bool)parentSlot && GameMain.main.isPlaying && !AnimationAssistant.main.Swaping && parentSlot.slot.bringDownEndSlot)
		{
			destroing = true;
			StartCoroutine(CollectChip());
		}
	}

	private IEnumerator CollectChip()
	{
		GameMain.main.targetBringDownRemainCount--;
		ParentRemove();
		yield return null;
		GameMain.main.DecreaseCollect(CollectBlockType.OreoCracker, countPrevValue: true);
		PoolManager.PoolGameBlocks.Despawn(base.transform);
	}
}
