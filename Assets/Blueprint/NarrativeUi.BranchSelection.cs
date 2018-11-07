using Futilef;

#if FBP
public unsafe partial struct NarrativeUi {
	const int BranchOptionCount = 5;
	const int BranchOptionVoteCount = 4;
	const int BranchOptionTextMaxLength = 25;
	
	const float BranchOptionHeight = 98;
	const float BranchOptionWidth = 512;
	const float BranchOptionMarginTop = 50;
	const float BranchOptionMarginRight = 33;
	const float BranchOptionSpacingY = BranchOptionHeight + 50;
	
	const float BranchVoteSize = 90;
	const float BranchVoteMarginRight = 57;
	const float BranchVoteMarginBottom = 45;
	const float BranchVoteSpacingX = BranchVoteSize + 3;

	const float BranchOptionTextSize = 40;

	fixed TpSpriteSliced branchOptions[BranchOptionCount];
	fixed BmText branchOptionTexts[BranchOptionCount];
	fixed TpSprite branchOptionVotes[BranchOptionCount * BranchOptionVoteCount];

	fixed int optionCount;
	fixed int voteCounts[BranchOptionCount];

	
}
#endif