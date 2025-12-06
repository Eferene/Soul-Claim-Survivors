using UnityEngine;
using System.Collections;

public class WTextAnim : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp()
    {
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(0, 1f, 0);
        float lifeTime = 1f;
        float time = 0;

        while (time < lifeTime)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, time / lifeTime);
            yield return null;
        }

        Destroy(this.gameObject);
    }

}
