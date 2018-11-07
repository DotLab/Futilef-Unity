using Futilef;

#if FBP
public unsafe partial struct NarrativeUi {
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
	
	const float PlayerBubbleTextFontSize = PlayerBubbleHeight - PlayerBubbleTextMarginVertical * 2;
	const float PlayerBubbleTextMarginTop = 20;
	const float PlayerBubbleTextMarginLeft = 50;
	const float PlayerBubbleTextLineWidth = PlayerBubbleWidth - PlayerBubbleTextMarginLeft - PlayerBubbleTextMarginTop;
	const float PlayerBubbleTextLineSpacing = 1.5f;

	const float PlayerListTransitionDuration = .6f;
	const float PlayerListShiftOffset = 20;

	fixed TpSprite playerAvatars[PlayerListCount];
	fixed TpSpriteSliced playerBubbles[PlayerListCount];
	fixed BmText playerBubbleTexts[PlayerListCount];

	int playerCount;
	
	public static void InitPlayerList(NarrativeUi *self) {
		var playerAvatars = self->playerAvatars;
		var playerBubbles = self->playerBubbles;
		var playerBubbleTexts = self->playerBubbleTexts;

		float Scr2World = Scr.Scr2World;

		for (int i = 0; i < PlayerListCount; i += 1) {
			var playerAvatar = playerAvatars + i;
			var playerBubble = playerBubbles + i;
			var playerBubbleText = playerBubbleTexts + i;

			TpSprite.Init(playerAvatar);
			TpSprite.SetPivot(playerAvatar, Rel.TopLeft);
			TpSprite.SetPos(playerAvatar, Rel.TopLeft, 
				PlayerAvatarMarginLeft * Scr2World, 
				(-PlayerAvatarMarginTop - i * PlayerAvatarSpacing) * Scr2World);
			TpSprite.SetSize(playerAvatar, 
				PlayerAvatarSize * Scr2World, 
				PlayerAvatarSize * Scr2World);
			PtrLst.Push(playerAvatarLst, playerAvatar);

			TpSpriteSliced.Init(playerBubble, Res.GetTpSpriteMeta(PlayerBubbleImgId));
			TpSpriteSliced.SetPivot(playerBubble, Rel.TopLeft);
			TpSpriteSliced.SetPosRel(playerBubble, playerAvatar, Rel.TopRight, 
				PlayerBubbleMarginLeft * Scr2World, 
				-PlayerBubbleMarginTop * Scr2World);
			TpSpriteSliced.SetSize(playerBubble, 
				PlayerBubbleWidth * Scr2World, 
				PlayerBubbleHeight * Scr2World);
			TpSpriteSliced.SetVisible(playerBubble, false);
			PtrLst.Push(playerBubbleLst, playerBubble);

			BmText.Init(playerBubbleText);
			BmText.Pivot(playerBubbleText, Rel.TopLeft);
			BmText.SetPosRel(playerBubbleText, playerBubble, Rel.TopLeft, 
				PlayerBubbleTextMarginLeft * Scr2World, 
				-PlayerBubbleTextMarginTop * Scr2World);
			BmText.SetFontSize(playerBubbleText, PlayerBubbleTextFontSize * Scr2World);
			BmText.SetLineWidth(playerBubbleText, PlayerBubbleTextLineWidth * Scr2World);
			BmText.SetLineSpacing(playerBubbleText, PlayerBubbleTextLineSpacing * Scr2World);
			BmText.SetVisible(playerBubbleText, false);
			PtrLst.Push(playerBubbleTextLst, playerBubbleText);
		}
	}

	public static void SetPlayerAvatars(NarrativeUi *self, int[] playerAvatarImgIds) {
		var playerAvatars = self->playerAvatars;
		var playerBubbles = self->playerBubbles;
		var playerBubbleTexts = self->playerBubbleTexts;

		int oldPlayerCount = playerCount;
		playerCount = playerAvatarImgIds.Length;

		float Scr2World = Scr.Scr2World;

		var job = EsWorker.PrepBatch(esWorker, Es.CubicOut, PlayerListTransitionDuration);
		for (int i = 0; i < oldPlayerCount; i += 1) {
			var playerAvatar = playerAvatars + i;
			var playerBubble = playerBubbles + i;
			var playerBubbleText = playerBubbleTexts + i;

			if (i < playerCount) {  // already showing, switch
				TpSprite.SetMeta(playerAvatar, Res.GetTpSpriteMeta(playerAvatarImgIds[i]));
				if (playerAvatar->meta->name != playerAvatarImgIds[i]) {  // different image
					EsBatchJob.RestoreScaleAlt(playerAvatar, Rel.Center, 0, 0, Es.BackOut);
				}
			} else {  // not showing any more, hide
				EsBatchJob.SetScale(playerAvatar, Rel.Center, 0, 0, EsWorker.SetVisibleAtEnd);
				EsBatchJob.ShiftPos(playerAvatar, 0, Scr2World * PlayerListShiftOffset);
			}

			if (playerBubble->isVisible) {  // hide all bubbles
				EsBatchJob.ShiftPos(job, playerBubble, 0, Scr2World * -PlayerListShiftOffset);
				EsBatchJob.ShiftPos(job, playerBubbleText, 0, Scr2World * -PlayerListShiftOffset);
				EsBatchJob.SetOpacity(job, playerBubble, 0, EsWorker.SetVisibleAtEnd);
				EsBatchJob.SetOpacity(job, playerBubbleText, 0, EsWorker.SetVisibleAtEnd);
			}
		}

		for (int i = oldPlayerCount; i < playerCount; i += 1) {  // not showing, show
			var playerAvatar = playerAvatarArr[i];	

			Node.SetMeta(playerAvatar, Res.GetTpSpriteMeta(playerAvatarImgIds[i]));
			Node.SetOpacity(playerAvatar, 0);
			Node.SetVisible(playerAvatar, true);
			EsBatchJob.RestoreScaleAlt(playerAvatar, Rel.Center, 0, 0, Es.BackOut);
			EsBatchJob.SetOpacity(playerAvatar, 1);
		}
		EsWorker.ExecBatch(esWorker, job);
	}

	public static void SetPlayerBubbleText(NarrativeUi *self, int playerIdx, string message) {
		var playerAvatar = self->playerAvatars[playerIdx];
		var playerBubble = self->playerBubbles[playerIdx];
		var playerBubbleText = self->playerBubbleTexts[playerIdx];

		float Scr2World = Scr.Scr2World;

		Node.SetText(playerBubbleText, message);
		float height = Node.GetHeight(playerBubbleText);
		Node.SetScale(playerBubbleText, 1, .5f);
		Node.SetOpacity(playerBubbleText, 0);
		Node.SetVisible(playerBubbleText, true);

		height += (PlayerBubbleTextMarginTop + PlayerBubbleTextMarginTop) * Scr2World;

		var job = EsWorker.PrepBatch(esWorker, Es.CubicOut, PlayerListTransitionDuration);
		if (!playerBubble->isVisible) {  // not visible -> fade in from top
			Node.SetHeight(playerBubble, heigh);
			Node.SetOpacity(playerBubble, 0);
			Node.SetVisible(playerBubble, true);
			
			EsBatchJob.RestorePos(playerBubble, 0, Scr2World * PlayerListShiftOffset);
			EsBatchJob.RestorePos(playerBubbleText, 0, Scr2World * PlayerListShiftOffset);
			EsBatchJob.SetOpacity(playerBubble, 1);
			EsBatchJob.SetOpacity(playerBubbleText, 1);
			EsBatchJob.SetScaleAlt(playerBubbleText, 1, 1, Es.BackOut);
		} else {  // already visible -> ease height
			EsBatchJob.SetHeight(playerBubble, heigh);
			EsBatchJob.SetOpacity(playerBubbleText, 1);
			EsBatchJob.SetScaleAlt(playerBubbleText, 1, 1, Es.BackOut);
		}
		EsWorker.ExecBatch(esWorker, job);
	}

	public static void HidePlayerBubble(NarrativeUi *self, int playerIdx) {
		var playerAvatars = self->playerAvatars;
		var playerBubbles = self->playerBubbles;
		var playerBubbleTexts = self->playerBubbleTexts;

		float Scr2World = Scr.Scr2World;

		var job = EsWorker.PrepBatch(esWorker, Es.CubicOut, PlayerListTransitionDuration);
		EsBatchJob.ShiftPos(job, playerBubble, Scr2World * -PlayerListShiftOffset, 0);
		EsBatchJob.ShiftPos(job, playerBubbleText, Scr2World * -PlayerListShiftOffset, 0);
		EsBatchJob.SetOpacity(job, playerBubble, 0, EsWorker.SetVisibleAtEnd);
		EsBatchJob.SetOpacity(job, playerBubbleText, 0, EsWorker.SetVisibleAtEnd);
		EsWorker.ExecBatch(esWorker, job);
	}

	public static void ShowPlayerList(NarrativeUi *self) {
		var playerAvatars = self->playerAvatars;
		var playerBubbles = self->playerBubbles;
		var playerBubbleTexts = self->playerBubbleTexts;

		float Scr2World = Scr.Scr2World;

		var job = EsWorker.PrepBatch(esWorker, Es.CubicOut, PlayerListTransitionDuration);
		for (int i = 0, count = self->playerCount; i < count; i += 1) {
			var playerAvatar = playerAvatarArr[i];
			
			Node.SetPos(playerAvatar, Rel.TopLeft, 
				Scr2World * PlayerAvatarMarginLeft, 
				Scr2World * (-PlayerAvatarMarginTop - i * PlayerAvatarSpacing));
			Node.SetOpacity(playerAvatar, 0);
			Node.SetVisible(playerAvatar, true);

			EsBatchJob.RestorePos(job, playerAvatar, 0, Scr2World * PlayerListShiftOffset);
			EsBatchJob.SetOpacity(job, playerAvatar, 1);
		}
		EsWorker.ExecBatch(esWorker, job);
	}

	public static void HidePlayerList(NarrativeUi *self) {
		var playerAvatars = self->playerAvatars;
		var playerBubbles = self->playerBubbles;
		var playerBubbleTexts = self->playerBubbleTexts;

		float Scr2World = Scr.Scr2World;

		var job = EsWorker.PrepBatch(esWorker, Es.CubicOut, PlayerListTransitionDuration);
		for (int i = 0, count = self->playerCount; i < count; i += 1) {
			var playerAvatar = playerAvatarArr[i];
			var playerBubble = playerBubbleArr[i];
			var playerBubbleText = playerBubbleTextArr[i];

			EsBatchJob.ShiftPos(job, playerAvatar, 0, Scr2World * -PlayerListShiftOffset);
			EsBatchJob.SetOpacity(job, playerAvatar, 0, EsWorker.SetVisibleAtEnd);

			if (playerBubble->isVisible) {
				EsBatchJob.ShiftPos(job, playerBubble, 0, Scr2World * -PlayerListShiftOffset);
				EsBatchJob.ShiftPos(job, playerBubbleText, 0, Scr2World * -PlayerListShiftOffset);
				EsBatchJob.SetOpacity(job, playerBubble, 0, EsWorker.SetVisibleAtEnd);
				EsBatchJob.SetOpacity(job, playerBubbleText, 0, EsWorker.SetVisibleAtEnd);
			}
		}
		EsWorker.ExecBatch(esWorker, job);
	}

	static void TouchPlayerList(NarrativeUi *self, Tch *tches, int tchCount) {
		var playerBubbles = self->playerBubbles;
		
		// HidePlayerBubble if touched on the bubble
		for (int i = 0, count = self->playerCount; i < count; i += 1) {
			var playerBubble = playerBubbles[i];

			if (playerBubble->isVisible) {
				for (int i = 0; i < tchCount; i += 1) {
					var tch = tches + i;
					if (tch->phase == TchPhase.Enter && Node.Raycast(playerBubble, tch->pos)) {
						HidePlayerBubble(self, i);
						break;
					}
				}
			}
		}

	}
}
#endif