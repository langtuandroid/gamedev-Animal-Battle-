using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DestructionAB : MonoBehaviour
{
    [SerializeField] private GameObject smoke;

    [SerializeField] private bool destroying = false;

    private void Start()
    {
        if (!destroying)
            StartCoroutine(DestroyTowerR());
    }


    private IEnumerator DestroyTowerR()
    {
        destroying = true;

        if (smoke)
            Instantiate(smoke.gameObject, transform.position, Quaternion.identity);
        while (transform.position.y > -20.0f)
        {
            transform.position = new Vector3(transform.position.x, (transform.position.y - 0.5f), transform.position.z);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.5f);
        destroying = false;
        Destroy(gameObject);
    }
}