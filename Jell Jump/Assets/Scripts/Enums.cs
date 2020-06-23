﻿
public enum CubeState
{   // Defines cube's states
    DEADLY,     // Cause character' death
    DANGEROUSLY,    // Cause damage
    NONEFFECTIVELY,     // No effect on Character
    USEFULLY,    // Earns golds, health, points etc.
    NICELY      // FEVERRRRRR
}

public enum GameState
{
    ONGAMEOVER,
    ONPLAY,
    ONFINISH
}

public enum BlockState
{
    NORMAL,
    STONED,
    FRAGILE
}

public enum CameraAnimationState
{
    ANIM_REVERSEFOCUS,
    ANIM_FOCUS,
    ANIM_CLOSEFOCUS
}