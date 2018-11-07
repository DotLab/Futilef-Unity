using Futilef;

#if FBP
public static unsafe partial class NarrativeUi {
	const int PlayerBubbleImgId = 10001;
	const int PlayerListCount = 5;

	const float PlayerAvatarSize = 115;
	const float PlayerAvatarMarginTop = 27;
	const float PlayerAvatarMarginLeft = 37;
	const float PlayerAvatarSpacing = PlayerAvatarSize + 32;

	const float PlayerBubbleHeight = 78;
	const float PlayerBubbleWidth = 500;
	const float PlayerBubbleMarginTop = (PlayerAvatarSize - PlayerBubbleHeight) / 2;
	const float PlayerBubbleMarginLeft = 13;
	
	const float PlayerBubbleTextSize = PlayerBubbleHeight - PlayerBubbleTextMarginVertical * 2;
	const float PlayerBubbleTextMarginTop = 20;
	const float PlayerBubbleTextMarginLeft = 50;
	const float PlayerBubbleTextLineWidth = PlayerBubbleWidth - PlayerBubbleTextMarginLeft - PlayerBubbleTextMarginTop;

	static PtrLst *playerAvatarLst;
	static PtrLst *playerBubbleLst;
	static PtrLst *playerBubbleTextLst;
	
	public static void InitPlayerList() {
		playerAvatarLst = PtrLst.New();
		Pool.AddDependent(nodePool, playerAvatarLst);
		playerBubbleLst = PtrLst.New();
		Pool.AddDependent(nodePool, playerBubbleLst);
		playerBubbleTextLst = PtrLst.New();
		Pool.AddDependent(nodePool, playerBubbleLst);

		for (int i = 0; i < PlayerListCount; i += 1) {
			var playerAvatar = (TpSprite *)Pool.Alloc(nodePool, sizeof(TpSprite));
			TpSprite.Init(playerAvatar);
			TpSprite.SetPivot(playerAvatar, Rel.TopLeft);
			TpSprite.SetScrPos(playerAvatar, Rel.TopLeft,
				PlayerAvatarMarginLeft, -PlayerAvatarMarginTop - i * PlayerAvatarSpacing);
			TpSprite.SetScrSize(playerAvatar, PlayerAvatarSize, PlayerAvatarSize);
			PtrLst.Push(playerAvatarLst, playerAvatar);

			var playerBubble = (TpSpriteSliced *)Pool.Alloc(nodePool, sizeof(TpSpriteSliced));
			TpSpriteSliced.Init(playerBubble, Res.GetTpSpriteMeta(PlayerBubbleImgId));
			TpSpriteSliced.SetPivot(playerBubble, Rel.TopLeft);
			TpSpriteSliced.SetScrPosRel(playerBubble, playerAvatar, Rel.TopRight,
				PlayerBubbleMarginLeft, -PlayerBubbleMarginTop);
			TpSpriteSliced.SetScrSize(playerBubble, PlayerBubbleWidth, PlayerBubbleHeight);
			TpSpriteSliced.SetVisible(playerBubble, false);
			PtrLst.Push(playerBubbleLst, playerBubble);

			var playerBubbleText = (BmText *)Pool.Alloc(nodePool, sizeof(BmText));
			BmText.Init(playerBubbleText);
			BmText.Pivot(playerBubbleText, Rel.TopLeft);
			BmText.SetScrPosRel(playerBubbleText, playerBubble, Rel.TopLeft,
				PlayerBubbleTextMarginLeft, -PlayerBubbleTextMarginTop);
			BmText.SetScrLineWidth(playerBubbleText, PlayerBubbleTextLineWidth);
			BmText.SetVisible(playerBubbleText, false);
			PtrLst.Push(playerBubbleTextLst, playerBubbleText);
		}
	}

	public static void SetPlayerAvatars(int[] playerAvatarImgIds, int playerCount) {
		var playerAvatarArr = playerAvatarLst->arr;
		var playerBubbleArr = playerBubbleLst->arr;
		var playerBubbleTextArr = playerBubbleTextLst->arr;

		int i; for (i = 0; i < playerCount; i += 1) {
			TpSprite.SetMeta(playerAvatarArr[i], Res.GetTpSpriteMeta(playerAvatarImgIds[i]));
			TpSprite.SetVisible(playerAvatarArr[i], true);
			TpSpriteSliced.SetVisible(playerBubbleArr[i], false);  // hide messages when avatar changes
			BmText.SetVisible(playerBubbleTextArr[i], false);
		}

		for (; i < PlayerListCount; i += 1) {
			TpSprite.SetVisible(playerAvatarArr[i], false);
			TpSpriteSliced.SetVisible(playerBubbleArr[i], false);  // hide messages when avatar changes
			BmText.SetVisible(playerBubbleTextArr[i], false);
		}
	}

	public static void SetPlayerBubbleText(int playerIdx, string message) {
		var playerBubbleText = playerBubbleTextLst->arr[playerIdx];
		BmText.SetText(playerBubbleText, message);
		float height = BmText.GetScrHeight(playerBubbleText);
		BmText.SetVisible(playerBubbleText, true);

		var playerBubble = playerBubbleLst->arr[playerIdx];
		TpSpriteSliced.SetScrHeight(playerBubble, height + PlayerBubbleTextMarginTop + PlayerBubbleTextMarginTop);
		TpSpriteSliced.SetVisible(playerBubble, true);
	}

	public static void HidePlayerBubbleText(int playerIdx) {
		TpSpriteSliced.SetVisible(playerBubbleLst->arr[playerIdx], false);
		BmText.SetVisible(playerBubbleTextLst->arr[playerIdx], false);
	}

	public static void ShowPlayerList() {
		var playerAvatarArr = playerAvatarLst->arr;
		var playerBubbleArr = playerBubbleLst->arr;
		var playerBubbleTextArr = playerBubbleTextLst->arr;

		for (int i = 0; i < PlayerListCount; i += 1) {
			TpSprite.SetVisible(playerAvatarArr[playerIdx], false);
			TpSpriteSliced.SetVisible(playerBubbleArr[playerIdx], false);
			BmText.SetVisible(playerBubbleTextArr[playerIdx], false);
		}
	}

	public static void HidePlayerList() {
		var playerAvatarArr = playerAvatarLst->arr;
		var playerBubbleArr = playerBubbleLst->arr;
		var playerBubbleTextArr = playerBubbleTextLst->arr;

		for (int i = 0; i < PlayerListCount; i += 1) {
			TpSprite.SetVisible(playerAvatarArr[playerIdx], true);
			TpSpriteSliced.SetVisible(playerBubbleArr[playerIdx], true);
			BmText.SetVisible(playerBubbleTextArr[playerIdx], true);
		}
	}
}
#endif