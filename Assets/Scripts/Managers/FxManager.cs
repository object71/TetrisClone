using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    public ParticleSystem GlowFx;
    public ParticleSystem SpawnFx;
    private ICollection<ParticleSystem> particles = new LinkedList<ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (particles.Count > 0)
        {
            int i = 0;
            while (i < particles.Count)
            {

                ParticleSystem fx = particles.ElementAt(i);

                if (fx && !fx.IsAlive())
                {
                    particles.Remove(fx);
                    Destroy(fx.gameObject);
                }
                else
                {
                    i++;
                }

            }
        }
    }

    public void PlaySpawnFx()
    {
        if (SpawnFx)
        {
            SpawnFx.Play();
        }
    }

    public void PlayGlowFx(int x, int y)
    {
        if (GlowFx)
        {
            ParticleSystem glowFx = Instantiate(GlowFx, new Vector3(x, y, -1), Quaternion.identity);
            glowFx.Play();

            particles.Add(glowFx);
        }
    }

}
