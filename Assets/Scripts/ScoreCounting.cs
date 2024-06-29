using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounting : MonoBehaviour
{
    private float Score = 0;
    private TextMeshProUGUI scoreText;
    public GameObject winParticle;
    public AudioClip scoreSound;
    // Start is called before the first frame update
    void OnEnable()
    {
        
    }
    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        scoreText.text=Score.ToString();
    }

   void OnTriggerEnter(Collider other)
   {
    if(other.gameObject.tag == "Ball"){
        Score++;
        scoreText.text=Score.ToString();
        winParticle.SetActive(true);
        AudioSource.PlayClipAtPoint(scoreSound,transform.position);
        StartCoroutine(stopParticle());
    }
   }
   IEnumerator stopParticle(){

    yield return new WaitForSeconds(1);
    winParticle.SetActive(false);
   }
}
