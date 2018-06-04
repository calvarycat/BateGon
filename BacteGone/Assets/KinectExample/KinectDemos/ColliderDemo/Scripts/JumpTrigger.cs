using UnityEngine;
using System.Collections;

public class JumpTrigger : MonoBehaviour 
{
    HandColorOverlayer handColorOverLay;
    void Start()
    {
        handColorOverLay = GameObject.FindObjectOfType<HandColorOverlayer>();
        Debug.Log(handColorOverLay.name);
    }
	void OnTriggerEnter(Collider other)
    {
		//Debug.Log ("Jump trigger activated");
        if((other.CompareTag("HandLeft") && handColorOverLay.leftHandState== KinectInterop.HandState.Closed)|| (other.CompareTag("HandRight") && handColorOverLay.RightHandState == KinectInterop.HandState.Closed))
        {           
                    CatchObject(other);          

        }
       

    }
    public void CatchObject(Collider other)
    {
        //Animation animation = gameObject.GetComponent<Animation>();
        //if (animation != null)
        //{
        //    animation.Play();
        //}

        AudioSource audioSrc = gameObject.GetComponent<AudioSource>();
        if (audioSrc != null)
        {
            audioSrc.Play();
        }
        transform.position = other.transform.position;
        transform.parent = other.transform;
        GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }
}
