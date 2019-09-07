﻿using System;
using UnityEngine;

namespace MainMenu
{
    enum State
    {
        Left,
        Right,
        Changing
    }

    enum MTUIType
    {
        Tree,
        Sign,
        Overlay
    }

    [SerializeField]
    class Description{
        [SerializeField]
        private string title;
        [SerializeField]
        private string description;
    }

    [Serializable]
    class MegaphoneTreeUI
    {
        [SerializeField]
        private MTUIType type;

        [SerializeField]
        private GameObject gameObject;
    }
}