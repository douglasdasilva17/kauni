using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Supertactic
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public Sprite itemIcon;
        public string itemName;

        [Header("One Handed Attack Animations")]
        public string equipAnimationName;
        public string unequipAnimationName;
    }
}