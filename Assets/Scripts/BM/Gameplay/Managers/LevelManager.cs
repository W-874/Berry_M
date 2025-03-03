using BM.Data;
using BM.Utils.Singleton;

namespace BM.Gameplay.Managers
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public static LevelData LevelData { get; set; }
    }
}