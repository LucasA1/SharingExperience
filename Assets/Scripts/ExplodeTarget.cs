using UnityEngine;
using Academy.HoloToolkit.Unity;
using Academy.HoloToolkit.Sharing;

public class ExplodeTarget : Singleton<ExplodeTarget>
{
    //[Tooltip("Object to disable after the target explodes.")]
    public Transform OldHandle;

    //[Tooltip("Object to enable after the target explodes.")]
    public Transform NewHandle;

    void Start()
    {
        //// Attach ExplodingBlob to our target, so it will explode when hit by projectiles.
       // this.transform.Find("EnergyHub/BlobOutside").gameObject.AddComponent<ExplodingBlob>();

       //// If a user joins late, we need to reset the target.
        SharingSessionTracker.Instance.SessionJoined += Instance_SessionJoined;

        //// Handles the ExplodeTarget message from the network.
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.ExplodeTarget] = this.OnExplodeTarget;
    }

    /// <summary>
    /// When a new user joins the session after the underworld is enabled,
    /// reset the target so that everyone is in the same game state.
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">args</param>
    private void Instance_SessionJoined(object sender, SharingSessionTracker.SessionJoinedEventArgs e)
    {
      //  if (instantiate.NewHandle)
        {
            HologramPlacement.Instance.ResetStage();
        }
    }

    /// <summary>
    /// Disables target and spatial mapping after an explosion occurs, enables the underworld.
    /// </summary>
    public void OnSelect()
    {
        // Hide the target and show the underworld.
        Instantiate(NewHandle, OldHandle.transform.position, OldHandle.transform.rotation);
        Destroy(this.gameObject);

        // Disable spatial mapping so drones can fly out of the underworld and players can shoot projectiles inside.
        //SpatialMappingManager.Instance.gameObject.SetActive(false);
    }

    /// <summary>
    /// When a remote system has triggered an explosion, we'll be notified here.
    /// </summary>
    void OnExplodeTarget(NetworkInMessage msg)
    {
        OnSelect();
    }
}