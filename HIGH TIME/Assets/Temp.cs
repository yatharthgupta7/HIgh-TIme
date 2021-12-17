using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool space = Input.GetKeyDown(KeyCode.Space);
        if(space)
        {
            anim.SetBool("attack", true);
        }
        if(!space)
        {
            anim.SetBool("attack", false);
        }
    }
}
