using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public int ammoCount;
    [SerializeField] public Text Ammo;

    public float airCurr;
    [SerializeField] public Text Air;

    public int score;
    [SerializeField] public Text PlayerScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ammo.text = "Ammo Count: " + ammoCount;
        PlayerScore.text = "Score: " + score;
    }
}
