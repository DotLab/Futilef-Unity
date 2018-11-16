using Futilef;

#if FBP
public unsafe partial struct NarrativeUi {
	const int BranchOptionImgId = 10005;
	const int BranchOptionCount = 5;
	const int BranchOptionVoteCount = 4;
	const int BranchOptionTextMaxLength = 25;
	
	const float BranchOptionHeight = 98;
	const float BranchOptionWidth = 512;
	const float BranchOptionMarginTop = 50;
	const float BranchOptionMarginRight = 33;
	const float BranchOptionSpacingY = BranchOptionHeight + 50;
	
	const float BranchOptionTextSize = 40;

	const float BranchVoteSize = 90;
	const float BranchVoteMarginRight = 57;
	const float BranchVoteMarginBottom = 45;
	const float BranchVoteSpacingX = BranchVoteSize + 3;

	fixed TpSpriteSliced branchOptions[BranchOptionCount];
	fixed BmText branchOptionTexts[BranchOptionCount];
	fixed TpSprite branchOptionVotes[BranchOptionCount * BranchOptionVoteCount];

	int optionCount;
	fixed int voteCounts[BranchOptionCount];

	public static void InitBranchSelect(NarrativeUi *self) {
		var branchOptions = self->branchOptions;
		var branchOptionTexts = self->branchOptionTexts;
		var branchOptionVotes = self->branchOptionVotes;

		float Scr2World = Scr.Scr2World;

		for (int i = 0; i < BranchOptionCount; i += 1) {
			var branchOption = branchOptions + i;
			var branchOptionText = branchOptionTexts + i;

			TpSpriteSliced.Init(branchOption, Res.GetTpSpriteMeta(PlayerBubbleImgId));
			TpSpriteSliced.SetPivot(branchOption, Rel.TopRight);
			TpSpriteSliced.SetPos(branchOption,
				-Scr2World * BranchOptionMarginRight,
				-Scr2World * (BranchOptionMarginTop + i * BranchOptionSpacingY));
			TpSpriteSliced.SetSize(branchOption,
				Scr2World * BranchOptionWidth,
				Scr2World * BranchOptionHeight);
			TpSpriteSliced.SetVisible(branchOption, false);

			BmText.Init(branchOptionText, BranchOptionTextMaxLength);
			BmText.SetPivot(branchOptionText, Rel.center);
			BmText.SetPosRel(branchOptionText, branchOption, Rel.center, 0, 0);
			BmText.SetSize(branchOptionText, BranchOptionTextSize);

			for (int j = 0; j < BranchOptionVoteCount; j += 1) {
				var branchOptionVote = branchOptionVotes + i * BranchOptionVoteCount + j;

				TpSprite.Init(branchOptionVote);
				TpSprite.SetPivot(branchOptionVote, Rel.TopLeft);
				TpSprite.SetPosRel(branchOptionVote, branchOption, Rel.TopLeft,
					-Scr2World * (BranchVoteMarginRight + j * BranchVoteSpacingX),
					Scr2World * BranchVoteMarginBottom);
				TpSprite.SetSize(branchOptionVote,
					Scr2World * BranchVoteSize,
					Scr2World * BranchVoteSize);
				TpSprite.SetVisible(branchOptionVote, false);
			}
		}
	}

	/**
	 * assume: all nodes are not visible
	 */
	public static void ShowBranchOptions(NarrativeUi *self, string[] options) {
		optionCount = options.Length;
		
		
	}

	/**
	 * assume: all nodes are visible and in right position 
	 */
	public static void HideBranchOptions(NarrativeUi *self) {

	}

	public static void UpdateBranchOptionVotes(NarrativeUi *self, int[] voteImgIds, int[] optionVoteCounts) {

	}

	public static void SelectBranchOption(NarrativeUi *self, int idx) {

	}

	static void TouchBranchSelect(NarrativeUi *self, Tch *tches, int tchCount) {
		
	}
}
#endif