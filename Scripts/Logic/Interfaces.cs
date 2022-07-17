using UnityEngine;
using System.Collections;

public interface IUnit: I_ID
{	
    int Health { get;    set;}

    int MovePoints { get; set; }

    int Attack { get; set; }

     void Die();
}

public interface I_ID
{
    int ID { get; }
}
