namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerHealthImplementor : IImplementor, IHealthComponent
    {
        public int currentHealth { get; set; }
        public int maxHealth { get; private set; }

        public PlayerHealthImplementor(int startingHealth)
        {
            // Set the initial health of the player.
            maxHealth = currentHealth = startingHealth;
        }
    }
}