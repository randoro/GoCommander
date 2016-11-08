using UnityEngine;
using System.Collections;

public class AddFriendButton : MonoBehaviour
{
    public GameObject addFriendBtn;

    public void AddFriendBtnClicked()
    {
        Destroy(addFriendBtn);
    }

}
