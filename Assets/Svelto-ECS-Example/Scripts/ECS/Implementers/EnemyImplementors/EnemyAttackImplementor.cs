namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyAttackImplementor:IEnemyAttackDataComponent, IImplementor
    {
        public EnemyAttackImplementor(float timeBetweenAttacks, int attackDamage)
        {
            attackInterval = timeBetweenAttacks;
            damage = attackDamage;
        }
        
        public int damage { get; private set; }
        public float attackInterval { get; private set; }
        public float timer { get; set; }
    }
}