using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TeleportBack : MonoBehaviour
{
    TeleportPoint tp;

	public SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("TeleportBack");

    [SerializeField]
	private Player player;
    private CharacterController cc;

    GameController gameController;

	// Start is called before the first frame update
	void Start()
    {
        tp = GetComponent<TeleportPoint>();

        TeleportPlayer();

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        cc = player.GetComponent<CharacterController>();
    }

    //Lateupdate is called to ensure that the player gets teleported back
    void LateUpdate()
    {
        if (teleportAction.stateDown)
        {
            TeleportPlayer();
        }
    }

    public void ForcedTeleport()
    {
        TeleportPlayer();
    }

	private void TeleportPlayer()
	{
        Vector3 teleportPosition = tp.transform.position;

        if (tp.ShouldMovePlayer())
        {
            Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
            player.trackingOriginTransform.position = teleportPosition + playerFeetOffset;

            if (player.leftHand.currentAttachedObjectInfo.HasValue)
                player.leftHand.ResetAttachedTransform(player.leftHand.currentAttachedObjectInfo.Value);
            if (player.rightHand.currentAttachedObjectInfo.HasValue)
                player.rightHand.ResetAttachedTransform(player.rightHand.currentAttachedObjectInfo.Value);
        }
        else
        {
            tp.TeleportPlayer(teleportPosition);
        }

        Teleport.Player.Send(tp);
	}
}
