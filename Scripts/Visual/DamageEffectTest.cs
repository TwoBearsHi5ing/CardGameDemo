using UnityEngine;
using System.Collections;

public class DamageEffectTest : MonoBehaviour {

    public GameObject DamagePrefab;
    public static DamageEffectTest Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            DamageEffect.CreateDamageEffect(transform.position, Random.Range(1, 7) , Visual_Effect.Heal);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            DamageEffect.CreateDamageEffect(transform.position, Random.Range(1, 7), Visual_Effect.Damage);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            DamageEffect.CreateDamageEffect(transform.position, Random.Range(1, 7), Visual_Effect.AttackBonus);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            DamageEffect.CreateDamageEffect(transform.position, Random.Range(1, 7), Visual_Effect.MovePointsBonus);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DamageEffect.CreateDamageEffect(transform.position, Random.Range(1, 7), Visual_Effect.ManaBonus);
        }

    }
}
