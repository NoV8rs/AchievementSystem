namespace AchievementSystem
{
    // ---------------------------------------------------------
    // ACHIEVEMENT CLASSES
    // ---------------------------------------------------------
    public class Achievement
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public bool IsUnlocked { get; private set; }

        public Achievement(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            IsUnlocked = false;
        }

        public void Unlock() => IsUnlocked = true;
    }

    /// <summary>
    /// Kill Achievement - track kills of specific enemy types
    /// </summary>
    public class KillAchievement : Achievement
    {
        // Properties specific to KillAchievement
        public string TargetEnemyType { get; } 
        public int TargetCount { get; }
        public int CurrentCount { get; private set; }

        /// <summary>
        /// Achievement for killing a certain type of enemy a number of times.
        /// </summary>
        /// <param name="id">id of the achievement.</param>
        /// <param name="name">Name of the achievement.</param>
        /// <param name="description">Short text for the achievement.</param>
        /// <param name="targetEnemyType">target.</param>
        /// <param name="targetCount">How much to kill before unlocking achievement.</param>
        public KillAchievement(string id, string name, string description, string targetEnemyType, int targetCount)
            : base(id, name, description)
        {
            TargetEnemyType = targetEnemyType;
            TargetCount = targetCount;
            CurrentCount = 0;
        }

        /// <summary>
        /// Increases the progress of the achievement by one.
        /// Returns true if the achievement is now complete.
        /// </summary>
        /// <returns></returns>
        public bool IncrementProgress()
        {
            if (IsUnlocked) return false;
            CurrentCount++;
            return CurrentCount >= TargetCount;
        }
    }

    // ---------------------------------------------------------
    // THE MANAGER
    // ---------------------------------------------------------
    public class AchievementManager
    {
        // Lasy Singleton Pattern
        private static readonly Lazy<AchievementManager> _instance = 
            new Lazy<AchievementManager>(() => new AchievementManager());

        // Public accessor
        public static AchievementManager Instance => _instance.Value;

        // Internal storage for achievements
        private readonly Dictionary<string, Achievement> _allAchievements = new();
        private readonly Dictionary<string, List<KillAchievement>> _enemyKillLookup = new();

        // Private constructor for Singleton
        private AchievementManager() { }

        // --- REGISTRATION METHODS ---

        /// <summary>
        /// Used for registering simple achievements without progress tracking.
        /// Could be used for finding collectibles, completing levels, etc.
        /// </summary>
        /// <param name="id">id number to unlock.</param>
        /// <param name="name">Name of the achievement. (Displays)</param>
        /// <param name="description">Short text of the achievement. (Displays)</param>
        public void RegisterAchievement(string id, string name, string description)
        {
            if (_allAchievements.ContainsKey(id)) return;
            var achievement = new Achievement(id, name, description);
            _allAchievements[id] = achievement;
        }
        
        /// <summary>
        /// Called to register a Kill Achievement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="enemyType"></param>
        /// <param name="targetCount"></param>
        public void RegisterKillAchievement(string id, string name, string description, Enum enemyType, int targetCount)
        {
            if (_allAchievements.ContainsKey(id)) return;

            // Convert Enum to String Key internally
            string typeKey = enemyType.ToString();

            var achievement = new KillAchievement(id, name, description, typeKey, targetCount);
            _allAchievements[id] = achievement;

            if (!_enemyKillLookup.ContainsKey(typeKey))
            {
                _enemyKillLookup[typeKey] = new List<KillAchievement>();
            }
            _enemyKillLookup[typeKey].Add(achievement);
        }

        // --- GAMEPLAY METHODS ---

        /// <summary>
        /// Called when an enemy is defeated to update relevant achievements.
        /// Put this in the enemy death logic.
        /// </summary>
        /// <param name="enemyType">Enum of the enemy, must be called the same name as the enum in RegisterKillAchievement.</param>
        public void EnemyDefeated(Enum enemyType)
        {
            string typeKey = enemyType.ToString();

            if (!_enemyKillLookup.TryGetValue(typeKey, out var relatedAchievements)) return;

            foreach (var achievement in relatedAchievements)
            {
                if (!achievement.IsUnlocked)
                {
                    bool isComplete = achievement.IncrementProgress();
                    Console.WriteLine($"[Achievement Progress] {achievement.Name}: {achievement.CurrentCount}/{achievement.TargetCount}");

                    if (isComplete) UnlockAchievement(achievement.Id);
                }
            }
        }

        // --- PRIVATE HELPERS ---
        /// <summary>
        /// Displays and unlocks the achievement.
        /// Private - should only be called internally.
        /// </summary>
        /// <param name="id">id of the achievement.</param>
        private void UnlockAchievement(string id)
        {
            if (!_allAchievements.TryGetValue(id, out var achievement)) return;
            if (achievement.IsUnlocked) return;

            achievement.Unlock();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n==========================================");
            Console.WriteLine($" [ACHIEVEMENT UNLOCKED] {achievement.Name}");
            Console.WriteLine($" {achievement.Description}");
            Console.WriteLine($"==========================================\n");
            Console.ResetColor();
        }
    }
}