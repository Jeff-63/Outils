using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnchorsHelper{

    public static readonly Vector2 TopLeftPositionMinLife = new Vector2(0, .75f)
                    , TopRightPositionMinLife = new Vector2(.5f, .75f)
                    , BottomLeftPositionMinLife = new Vector2(0, 0)
                    , BottomRightPositionMinLife = new Vector2(.5f, 0);

    public static readonly Vector2 TopLeftPositionMaxLife = new Vector2(.5f, 1)
                    , TopRightPositionMaxLife = new Vector2(1, 1)
                    , BottomLeftPositionMaxLife = new Vector2(.5f, .25f)
                    , BottomRightPositionMaxLife = new Vector2(1, .25f);

    public static readonly Vector2 TopLeftPositionMinMiniMap = new Vector2(0, .75f)
                    , TopRightPositionMinMiniMap = new Vector2(.85f, .75f)
                    , BottomLeftPositionMinMiniMap = new Vector2(0, 0)
                    , BottomRightPositionMinMiniMap = new Vector2(.85f, 0);

    public static readonly Vector2 TopLeftPositionMaxMiniMap = new Vector2(.15f, 1)
                    , TopRightPositionMaxMiniMap = new Vector2(1, 1)
                    , BottomLeftPositionMaxMiniMap = new Vector2(.15f, .25f)
                    , BottomRightPositionMaxMiniMap = new Vector2(1, .25f);

    public static readonly Vector2 TopLeftPositionMinPortrait = new Vector2(0, .85f)
                    , TopRightPositionMinPortrait = new Vector2(.93f, .85f)
                    , BottomLeftPositionMinPortrait = new Vector2(0, 0)
                    , BottomRightPositionMinPortrait = new Vector2(.93f, 0);

    public static readonly Vector2 TopLeftPositionMaxPortrait = new Vector2(.075f, 1)
                    , TopRightPositionMaxPortrait = new Vector2(1, 1)
                    , BottomLeftPositionMaxPortrait = new Vector2(.075f, .15f)
                    , BottomRightPositionMaxPortrait = new Vector2(1, .15f);
}
