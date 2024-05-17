using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AmiRotationMode : IRotationMode {

    private Transform transform;
    private readonly float rotateSpeed = 60f;

    public AmiRotationMode(Transform transform) {
        this.transform = transform;
    }

    public void Rotate(Transform player) {
        if(player) {
            Vector3 dir = player.position - transform.position;
            dir.y = 0;//保持在水平面上
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,rotateSpeed * Time.deltaTime);
        }
    }

}

