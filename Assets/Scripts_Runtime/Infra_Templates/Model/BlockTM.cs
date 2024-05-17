using System;
using UnityEngine;

namespace Air {

    [CreateAssetMenu(fileName = "SO_Block", menuName = "Air/BlockTM")]
    public class BlockTM : ScriptableObject {

        public int typeID;
        public Sprite mesh;

    }

}