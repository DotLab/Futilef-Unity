using Futilef;

#if FBP
public unsafe partial class NarrativeUi {
    public void Touch() {
        var chatBubbleArr = (TpSprite **)chatBubbleLst->arr;
        for (int i = 0, count = chatBubbleLst->count; i < count; i += 1) {
            if (TpSprite.Touch(chatBubbleArr[i])) {
                
            }
        }
    }
}
#endif