using Unity.Netcode.Components;
using UnityEngine;

[DisallowMultipleComponent]
public class ClienteNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
