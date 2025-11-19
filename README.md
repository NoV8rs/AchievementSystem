# AchievementSystem

**Description**  
AchievementSystem is a flexible and efficient achievement handling script written in Native C#. It provides methods for registering general-purpose achievements as well as kill-based achievements, and can be seamlessly integrated into other projects.

## Features
- **Achievement Registration**: Register achievements using `RegisterAchievement` for general-purpose goals.
- **Kill-Based Achievements**: Use `RegisterKillAchievement` to associate achievements with defeating specific enemy types.
- **Extensible Design**: Easily extend to support a wide variety of achievements, from kills to custom game objectives.
- **Lightweight and Modular**: Designed to adapt to any project with minimal setup.

## Installation
1. Clone this repository:
   ```bash
   git clone https://github.com/NoV8rs/AchievementSystem.git
   ```
2. Add the script files to your project directory.
3. Set up any necessary namespaces and imports for your project.
4. Build and integrate the system into your project.

## Usage
The `AchievementSystem` offers two primary methods for managing achievements:

### Registering Achievements
The `RegisterAchievement` method is used for creating generic achievements:
```csharp
AchievementManager.RegisterAchievement("Explorer", "Discover your first location");
```
- **Parameters**:
  - `string achievementName`: The name of the achievement.
  - `string description`: A short description of the achievement.

### Registering Kill-Based Achievements
The `registerkillachievement` method is used for creating achievements that trigger when specific enemy types are defeated:
```csharp
AchievementManager.RegisterKillAchievement(
    "Zombie Hunter", 
    "Defeat 100 zombies in the game.", 
    EnemyType.Zombie
);
```
- **Parameters**:
  - `string achievementName`: The name of the achievement.
  - `string description`: A short description of the achievement.
  - `EnemyType enemyType`: The type of enemy to associate with the achievement.
  - `Enemy Count`: The amount of kills needed.

### Defeating Enemies and Tracking Progress
When an enemy is defeated, call the `EnemyDefeated` method in the `Enemy` script to notify the `AchievementManager`:
```csharp
// In Enemy.cs
public void EnemyDefeated(EnemyType enemyType)
{
    // Notify AchievementManager about this kill
    AchievementManager.EnemyDefeated(enemyType);
}
```

Make sure your `Enemy` script defines an `EnemyType` enum for classifying enemies:
```csharp
public enum EnemyType
{
    Zombie,
    Dragon,
    Skeleton,
}
```

### Workflow Example
1. Register general achievements using `RegisterAchievement`.
2. Define kill-based achievements using `RegisterKillAchievement`.
3. Trigger `EnemyDefeated` in the `Enemy` script whenever an enemy is defeated.
4. Achievements are automatically unlocked when progress requirements are met.

## Example Code
Here's an example integrating the `RegisterAchievement` and `RegisterKillAchievement` methods:
```csharp
using System;

namespace MyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Register achievements
            AchievementManager.RegisterAchievement("First Steps", "Complete your first level.");
            AchievementManager.RegisterKillAchievement("Zombie Slayer", "Defeat 50 zombies.", EnemyType.Zombie);

            // Simulate defeating an enemy
            Enemy enemy = new Enemy(EnemyType.Zombie);
            enemy.EnemyDefeated(EnemyType.Zombie);

            Console.WriteLine("Achievements have been updated!");
        }
    }

    public class Enemy
    {
        private EnemyType enemyType;

        public Enemy(EnemyType type)
        {
            enemyType = type;
        }

        public void EnemyDeath()
        {
            AchievementManager.EnemyDefeated(enemyType);
            Console.WriteLine($"{enemyType} defeated.");
        }
    }

    public enum EnemyType
    {
        Zombie,
        Dragon,
        Skeleton
    }
}
```

## Contribution
Contributions are welcome!  
1. Fork the repository.
2. Create a feature branch:
   ```bash
   git checkout -b feature/your-feature
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add description of your feature"
   ```
4. Push to your branch:
   ```bash
   git push origin feature/your-feature
   ```
5. Open a pull request.

## License
This project is licensed under the **MIT License**. See the [LICENSE](./LICENSE) file for details.

## Contact
For any inquiries or support, feel free to open an [issue](https://github.com/NoV8rs/AchievementSystem/issues) in the repository.

---
**Language Composition**: 100% C#
