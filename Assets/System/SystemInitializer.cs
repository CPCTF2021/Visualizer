using UnityEngine;
using UserScripts;
using TreeScripts;
using static TreeScripts.ControlTree;

namespace VisualizerSystem
{
    public class SystemInitializer : MonoBehaviour
    {
        // leaveの色変換は一時的なもの
        [SerializeField]
        Color leaveBaseColor;
        [SerializeField]
        float baseColorRate = 0.5f;
        static UserManager userManager;
        static EventManager eventManager = new EventManager();
        void Start()
        {
            // 葉の色
            for (int i = 0; i < GENRE_TO_COLOR.Length; i++)
            {
                GENRE_TO_COLOR[i] = GENRE_TO_COLOR[i] * 0.95f + new Color(1f, 1f, 1f) * 0.05f;
                GENRE_TO_COLOR[i] = GENRE_TO_COLOR[i] * baseColorRate + leaveBaseColor * (1f - baseColorRate);
                // Points.GENRE_TO_COLOR[i] = Mathf.Pow(new Color(1f, 1f, 1f) - Points.GENRE_TO_COLOR[i], 2.0f);
            }
            GetComponent<TreeGenerator>().MakeTree();
            userManager = GetComponent<UserManager>();
            userManager.SetTree();

            eventManager.Init();

            TimeAdjusterEvent timeAdjusterEvent = new TimeAdjusterEvent(userManager);
            eventManager.Register(timeAdjusterEvent.Handler);

            UserCreatedEvent userCreatedEvent = new UserCreatedEvent(userManager);
            eventManager.Register(userCreatedEvent.Handler);

            ProblemSolvedEvent problemSolvedEvent = new ProblemSolvedEvent(userManager);
            eventManager.Register(problemSolvedEvent.Handler);

            RankingUpdatedEvent rankingUpdatedEvent = new RankingUpdatedEvent(userManager);
            eventManager.Register(rankingUpdatedEvent.Handler);
        }
        void Update()
        {
            if (eventManager.incoming_messages.TryDequeue(out var message))
            {
                eventManager.Handle(message);
            }
        }
        void OnDestroy()
        {
            eventManager.Shutdown();
        }
    }
}