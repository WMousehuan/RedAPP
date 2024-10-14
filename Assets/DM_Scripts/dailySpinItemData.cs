internal struct dailySpinItemData
{
	public ServerItemIndex rewardIndex;

	public int rewardCount;

	public int prob;

	public dailySpinItemData(ServerItemIndex rewardIndex, int rewardCount, int prob)
	{
		this.rewardIndex = rewardIndex;
		this.rewardCount = rewardCount;
		this.prob = prob;
	}
}
