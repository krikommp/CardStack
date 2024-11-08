using System.Collections;

namespace Client.GameLogic.Manager.GuideManager
{
    public class GuideManager : IManagerInitializable
    {
        #region Implementation of IManagerInitializable

        public IEnumerator ManagerInit()
        {
            yield return null;
        }

        public int Priority { get; } = (int)EManagerPriority.Low;

        #endregion

        private EGuideStep m_currentStep = EGuideStep.None;

        public void DoGuide()
        {
            switch (m_currentStep)
            {
                case EGuideStep.None:
                    m_currentStep = EGuideStep.FirstEntryGame;
                    break;
                case EGuideStep.FirstEntryGame:
                    m_currentStep = EGuideStep.Finish;
                    break;
                case EGuideStep.Finish:
                    break;
                default:
                    break;
            }
        }
    }
}