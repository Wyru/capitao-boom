using System;
using UnityEngine;


public class InputHandler {

    public static void HandleInput(Character player) {

        if (Input.GetButtonDown("Up")) {
            player.MoveUp();
        }
        else if (Input.GetButtonDown("Down") ) {
            player.MoveDown();
        }
        else if (Input.GetAxis("Horizontal") > 0.1) {
            player.MoveRight();
        }
        else if (Input.GetAxis("Horizontal") < -0.1) {
            player.MoveLeft();
        }
        else if (!player.verticalMoving) {
            player.StopWalking();
        }

        if (Input.GetButtonDown("Fire1")) {
            player.BeginChargeAttack();
        }
        else if (Input.GetButtonUp("Fire1")) {
            player.Attack();
        }

        if (Input.GetButtonDown("Fire2")) {
            player.BeginChargeUltimateAttack();
        }
        else if (Input.GetButtonUp("Fire2")) {
            player.Super();
        }
    }
}
