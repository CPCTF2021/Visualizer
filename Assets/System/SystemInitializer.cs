using UnityEngine;
using UserScripts;
using TreeScripts;
using static TreeScripts.TreeGenerator;
using RankingScript;

namespace VisualizerSystem
{
    public class SystemInitializer : MonoBehaviour
    {
        static UserManager userManager;
        static EventManager eventManager = new EventManager();
        static RankingManager rankingManager;
        void Start()
        {
            GetComponent<TreeGenerator>().MakeTree();
            userManager = GetComponent<UserManager>();
            userManager.SetTree();

            eventManager.Init();

            rankingManager = GameObject.Find("Scroll View").GetComponent<RankingManager>();

            TimeAdjusterEvent timeAdjusterEvent = new TimeAdjusterEvent(userManager);
            eventManager.Register(timeAdjusterEvent.Handler);

            UserCreatedEvent userCreatedEvent = new UserCreatedEvent(userManager, rankingManager);
            eventManager.Register(userCreatedEvent.Handler);

            ProblemSolvedEvent problemSolvedEvent = new ProblemSolvedEvent(userManager);
            eventManager.Register(problemSolvedEvent.Handler);

            RankingUpdatedEvent rankingUpdatedEvent = new RankingUpdatedEvent(userManager, rankingManager);
            eventManager.Register(rankingUpdatedEvent.Handler);
        }
        void Update()
        {
                eventManager.Handle();
        }
        void OnDestroy()
        {
            eventManager.Shutdown();
        }
    }
}