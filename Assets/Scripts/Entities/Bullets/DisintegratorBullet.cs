using System.Collections;
using UnityEngine;

public class DisintegratorBullet : BulletClass
{
    public ParticleSystem Effect;
    public GameObject bullet;
    public float bulletSpeed;
    public int shouldSpawn = 10;

    public void Start()
    {
        HitEffect = Effect;
        StartCoroutine(SetLifetime(Random.Range(3f, 4f)));
        StartCoroutine(splitBullet(0.3f));
    }

    public void updateSpawn(int spawnAmount)
    {
        shouldSpawn = spawnAmount;
    }

    public IEnumerator splitBullet(float time)
    {
        yield return new WaitForSeconds(time);
        if (shouldSpawn >= 1)
        {
            GameObject holder = Instantiate(bullet, transform.position, transform.rotation);
            holder.GetComponent<DisintegratorBullet>().updateSpawn(shouldSpawn - 1);
            holder.transform.rotation = holder.transform.rotation;
            holder.transform.Rotate(new Vector3(0, 0, Random.Range(-10f, 10f)));

            // Register the bullet
            float speed = Random.Range(bulletSpeed - 10, bulletSpeed + 10) * Research.research_bulletspeed;

            // Dependent on the bullet, register under the correct master script
            GameObject.Find("Bullet Handler").GetComponent<BulletHandler>().RegisterBullet(holder.transform, null, speed, 50, 250);
        }
    }

    public override void collide()
    {
        Instantiate(HitEffect, transform.position, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 180f));
        Destroy(gameObject);
    }

}
