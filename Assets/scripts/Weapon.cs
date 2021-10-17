using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public Sprite Part1;
    public Sprite Part2;
    public Vector2 Velocity = new Vector2(10f, 10f);

    public override void Interact(Interaction i)
    {
        GameObject part1 = new GameObject("Garbage");
        part1.transform.position = gameObject.transform.position;
        SpriteRenderer spriteRendrer1 = part1.AddComponent<SpriteRenderer>();
        spriteRendrer1.sprite = Part1;

        

        GameObject part2 = new GameObject("Garbage");
        part2.transform.position = gameObject.transform.position;
        SpriteRenderer spriteRendrer2 = part2.AddComponent<SpriteRenderer>();
        spriteRendrer2.sprite = Part2;
        
        Destroy(gameObject);

        part1.AddComponent<BoxCollider2D>();
        Rigidbody2D rigid1 = part1.AddComponent<Rigidbody2D>();
        rigid1.velocity = Velocity;

        part2.AddComponent<BoxCollider2D>();
        Rigidbody2D rigid2 = part2.AddComponent<Rigidbody2D>();
        Velocity.x *= -1;
        rigid2.velocity = Velocity;
    }
}
