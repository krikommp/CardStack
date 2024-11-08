using System.Collections;

namespace Client.GameLogic.Manager
{
    public enum EManagerPriority
    {
        High = 0,
        Normal = 1,
        Low = 2
    }
    
    public interface IManagerInitializable
    {
        IEnumerator ManagerInit();
        
        int Priority { get; }
    }

    public interface IManagerStart
    {
        void Start();
    }

    public interface IManagerUpdate
    {
        void Update();
    }
}