using TMPro;
using UnityEngine;

public class ShowBarafText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI barafText;

    [SerializeField] float lifeTime = 1;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float transparencyChangeSpeed = 1;
    [SerializeField] float placementJitter = .5f;

    void Start() => Destroy(gameObject, lifeTime);

    void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        
        if(barafText.color.a > 0)
            barafText.color = new Color(r: barafText.color.r,
                                        g: barafText.color.g,
                                        b: barafText.color.b,
                                        a: Mathf.MoveTowards(barafText.color.a, 0, transparencyChangeSpeed * Time.deltaTime));
    }

    //public void SetBarafText()
    //{
    //    barafText.SetText("BARAF");
    //    //transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), 0);
    //}
}
