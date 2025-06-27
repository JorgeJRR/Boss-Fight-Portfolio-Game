using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeExpand : MonoBehaviour
{
    public float expandDuration = 1f;

    public Vector3 originalScale;
    public Vector3 multiplyExpandedScale;

    public bool isBig = false;
    public float fuerzaEmpuje = 10f;

    public void PerformExpandAndContract()
    {
        StartCoroutine(ExpandAndContractCoroutine());
    }

    private IEnumerator ExpandAndContractCoroutine()
    {
        originalScale = transform.localScale;
        multiplyExpandedScale = originalScale * 2;

        transform.localScale = multiplyExpandedScale;
        isBig = true;

        yield return new WaitForSeconds(expandDuration);

        transform.localScale = originalScale;
        isBig = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && isBig)
        {
            Vector2 direccionDeEmpuje = (collision.transform.position - transform.position).normalized;
            Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();
            playerRigidbody.AddForce(direccionDeEmpuje * fuerzaEmpuje, ForceMode2D.Impulse);
        }
    }
}