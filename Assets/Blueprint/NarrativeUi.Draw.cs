using Futilef;

#if FBP
public unsafe partial class NarrativeUi {
    public void Draw() {
        var chatAvatarArr = (TpSprite **)chatAvatarLst->arr;
        for (int i = 0, count = chatAvatarLst->count; i < count; i += 1) {
            TpSprite.Draw(chatAvatarArr[i]);
        }
        var chatBubbleArr = (TpSprite **)chatBubbleLst->arr;
        for (int i = 0, count = chatBubbleLst->count; i < count; i += 1) {
            TpSprite.Draw(chatBubbleArr[i]);
        }
        var chatBubbleTextArr = (BmText **)chatBubbleTextLst->arr;
        for (int i = 0, count = chatBubbleTextLst->count; i < count; i += 1) {
            BmText.Draw(chatBubbleTextArr[i]);
        }
    }
}
#endif