using Futilef;

#if FBP
public static unsafe partial class NarrativeUi {
    static Pool *nodePool;
    static EsWorker *esWorker;
    
    public static void Init() {
        nodePool = Pool.New();
        esWorker = EsWorker.New();

        InitPlayerList();
        InitGal();
        InitBranching();
        InitMisc();
        InitFunctionMenu();
    }
}
#endif