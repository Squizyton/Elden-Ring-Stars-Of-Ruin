using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = System.Random;

public class StarBehaviour : MonoBehaviour
{
    
    
    
  [Header("Movement Variables")]
  //Array of collisions the star will check on OnTriggerEnter
  [SerializeField]private string[] CollisionsToCheck;
  //Duration of the star's movement before it explodes
  [SerializeField]private float duration;
  //Rotation Speed
  [SerializeField]private float rotationSpeed;
  //The speed in which the star will mvoe before it rotates towards the target
  [SerializeField]private float beforeTurnSpeed;
  //The Speed in which the star will move after it rotates towards the target
  [SerializeField]private float afterTurnSpeed;
  //The distance in which the star will go before it turns
  [SerializeField] private float DistanceBeforeTurn;
  //The distance in which the star will go after it turns if THERE IS NO TARGET
  [SerializeField] private float DistanceAfterTurn;
  
  [Header("Vector Positions")]
  //The position the star will move to if no target
  private Vector3 defaultPosition;
  //Start position
  private Vector3 startPosition;
  //The direction the star will face
  private Vector3 faceDirection;
  //The position the star will move to if there is a target
  private Vector3 goingToPosition;
  //Stars rotation
  private Quaternion rotation;



  [Header("Other Variables")] [SerializeField]
  private Transform target;
  [SerializeField] private bool hasTurned;
  [SerializeField] private ParticleSystem loopFx;
  [SerializeField] private ParticleSystem endFx;
  [SerializeField]private bool atTarget;
  public void Start()
  {
    
    //Getting starting Vector3's
    startPosition = transform.position;
    //Calculate the default position this star will end up at if no target
    defaultPosition = transform.position + transform.forward * DistanceAfterTurn;
    var rot = transform.rotation.eulerAngles;
    //Rotate the star to face up
    rot.y += 90;
    rot.x = UnityEngine.Random.Range(0, 360);
    transform.rotation = Quaternion.Euler(rot);

  }
  private void Update()
  {
      if (atTarget) return;
      
      if (hasTurned)
      {
          if (target)
          {
              //Look at the target
              faceDirection = target.position - transform.position;
              //Get the position of where this object is going to
              goingToPosition = target.position;
          }
          else
          {
              //If no target, look at the default position
              faceDirection = defaultPosition - transform.position;
              //Set the position of where this object is going to the default position
              goingToPosition = defaultPosition;
          }

          
          //Rotate the star to make it look at target
          rotation = Quaternion.LookRotation(faceDirection);

          //slerp rotate the star to face the target
          transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
          //Move the star towards the target
          transform.Translate(transform.forward * Time.deltaTime * afterTurnSpeed, Space.World);
          
          //Check if the star is close enough to the target
          
          
          var distance = Vector3.Distance(goingToPosition, transform.position);

          
          Debug.Log(distance);
          //If we hit the enemy
          if (distance <= .7f)
          {
              Debug.Log("Hit Target");
              Explode();
              atTarget = true;
          }
          
          //If we haven't hit anything then count down a timer
          if(duration <= 0) Explode();
          else duration -= Time.deltaTime;
      }
      else
      {
          //Translate the start forward
          transform.Translate(transform.forward * beforeTurnSpeed * Time.deltaTime,Space.World);
          
          //Calculate the distance between the start and where it's at now
          var distance = Vector3.Distance(transform.position, startPosition);
          //If the distance is greater than the the threshold, turn
          if (distance >= DistanceBeforeTurn)
          {
              hasTurned = true;
          }
      }
  }


  private void Explode()
  {
      loopFx.Stop();
      endFx.Play();
      
      Destroy(gameObject, .5f);
  }

  public void SetTarget(Transform tar)
  {
      target = tar;
  }


  private void OnTriggerEnter(Collider other)
  {
     
  }
}