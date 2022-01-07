using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    // List of active animators
    public class SpawnAnim
    {
        public SpawnAnim(Transform obj, float size = 1.5f, float speed = 0.001f, float original = 1f)
        {
            this.obj = obj;
            this.size = size;
            this.speed = speed;
            this.original = original;
        }

        public Transform obj;
        public float size = 1.5f;
        public float speed = 0.001f;
        public float original = 1f;
    }
    public List<SpawnAnim> spawnAnims = new List<SpawnAnim>();

    // Spawn animation flag
    public bool enableSpawnAnimation;

    // Subscribe to building place event
    public void Start()
    {
        if (enableSpawnAnimation)
            Events.active.onBuildingPlaced += AddSpawnAnim;
        spawnAnims = new List<SpawnAnim>();
    }

    // Update is called once per frame
    public void Update()
    {
        for(int i = 0; i < spawnAnims.Count; i++)
        {
            SpawnAnim anim = spawnAnims[i];

            if (anim.obj == null)
            {
                spawnAnims.RemoveAt(i);
                i--;
                continue;
            }

            anim.obj.localScale = new Vector2(anim.size, anim.size);
            if (anim.size <= anim.original)
            {
                anim.obj.localScale = new Vector2(anim.original, anim.original);
                spawnAnims.RemoveAt(i);
                i--;
                continue;
            }
            else
            {
                anim.size -= anim.speed;
                anim.speed *= 1.2f;
            }
        }
    }

    /// <summary>
    /// Animates a transform falling down onto the grid.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="startSize"></param>
    /// <param name="speed"></param>
    /// <param name="endSize"></param>
    public void AddDropdownAnim(Transform obj, float startSize = 1.5f, float speed = 0.001f, float endSize = 1f)
    {
        spawnAnims.Add(new SpawnAnim(obj, startSize, speed, endSize));
    }

    public void AddSpawnAnim(BaseTile tile) 
    { 
        if (!NewSaveSystem.isLoading && tile.canAnimate) 
            spawnAnims.Add(new SpawnAnim(tile.transform)); 
    }
}
