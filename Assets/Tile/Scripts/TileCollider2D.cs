using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ChunksWalker {
    public static int DIRECTION_STAY = 0;
    public static int DIRECTION_UP = 1;
    public static int DIRECTION_RIGHT = 2;
    public static int DIRECTION_DOWN = 3;
    public static int DIRECTION_LEFT = 4;

    public float tileWidth = 1f;
    public float tileHeight = 1f;

    private Vector2 mPos;
    private Vector2[] mChunks;
    private int mDirection;

    public ChunksWalker(Vector2 pos, Vector2[] chunks) {
        this.mPos = pos;
        this.mChunks = chunks;
    }

    public static bool IsSamePoint(Vector2 a, Vector2 b) {
        return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
    }

    private bool ApproximatelyIn(Vector2 aim, Vector2[] chunks) {
        foreach (Vector2 point in chunks) {
            if (IsSamePoint(aim, point)) {
                return true;
            }
        }
        return false;
    }

    public int State() {
        Vector2 pos0 = new Vector2(mPos.x - tileWidth, mPos.y);
        Vector2 pos1 = mPos;
        Vector2 pos2 = new Vector2(mPos.x - tileWidth, mPos.y - tileHeight);
        Vector2 pos3 = new Vector2(mPos.x, mPos.y - tileHeight);
        int bit0 = ApproximatelyIn(pos0, mChunks) ? 1 : 0;
        int bit1 = ApproximatelyIn(pos1, mChunks) ? 1 << 1 : 0;
        int bit2 = ApproximatelyIn(pos2, mChunks) ? 1 << 2 : 0;
        int bit3 = ApproximatelyIn(pos3, mChunks) ? 1 << 3 : 0;
        return bit0 | bit1 | bit2 | bit3;
    }

    public Vector2 Pos() {
        return mPos;
    }

    public void Jump(float deltaX, float deltaY) {
        mPos.x += deltaX;
        mPos.y += deltaY;
    }

    public void WalkUp() {
        mPos.y += tileHeight;
        mDirection = DIRECTION_UP;
    }

    public void WalkDown() {
        mPos.y -= tileHeight;
        mDirection = DIRECTION_DOWN;
    }

    public void WalkRight() {
        mPos.x += tileWidth;
        mDirection = DIRECTION_RIGHT;
    }

    public void WalkLeft() {
        mPos.x -= tileWidth;
        mDirection = DIRECTION_LEFT;
    }

    public int Direction() {
        return mDirection;
    }
}

interface IConvexStateHandler {
    void Handle(ChunksWalker walker, List<Vector2> convex);
}

class ConvexStartFinder : IConvexStateHandler {
    public void Handle(ChunksWalker walker, List<Vector2> convex) {
        int state = walker.State();
        if (state == 15) {
            walker.Jump(-walker.tileWidth, -walker.tileHeight);
        } else if (walker.Direction() == ChunksWalker.DIRECTION_STAY) {
            // start point
            convex.Add(walker.Pos());
            if (state == 1 || state == 5 || state == 13 || state == 9) {
                walker.WalkUp();
            } else if (state == 2 || state == 3 || state == 7 || state == 6) {
                walker.WalkRight();
            } else if (state == 8 || state == 10 || state == 11) {
                walker.WalkDown();
            } else if (state == 4 || state == 12 || state == 14) {
                walker.WalkLeft();
            }
            convex.Add(walker.Pos());
        }
    }
}

class WalkingUp : IConvexStateHandler {
    public void Handle(ChunksWalker walker, List<Vector2> convex) {
        int direction = walker.Direction();
        if (direction == ChunksWalker.DIRECTION_STAY) {
            return;
        }
        int state = walker.State();
        if (state == 1 || state == 5 || state == 13 || 
            (direction == ChunksWalker.DIRECTION_RIGHT && state == 9)) {
            walker.WalkUp();
            convex.Add(walker.Pos());
        }
    }
}

class WalkingRight : IConvexStateHandler {
    public void Handle(ChunksWalker walker, List<Vector2> convex) {
        int direction = walker.Direction();
        if (direction == ChunksWalker.DIRECTION_STAY) {
            return;
        }
        int state = walker.State();
        if (state == 2 || state == 3 || state == 7 ||
            (direction == ChunksWalker.DIRECTION_DOWN && state == 6)) {
            walker.WalkRight();
            convex.Add(walker.Pos());
        }
    }
}

class WalkingDown : IConvexStateHandler {
    public void Handle(ChunksWalker walker, List<Vector2> convex) {
        int direction = walker.Direction();
        if (direction == ChunksWalker.DIRECTION_STAY) {
            return;
        }
        int state = walker.State();
        if (state == 8 || state == 10 || state == 11 ||
            (direction == ChunksWalker.DIRECTION_LEFT && state == 9)) {
            walker.WalkDown();
            convex.Add(walker.Pos());
        }
    }
}

class WalkingLeft : IConvexStateHandler{
    public void Handle(ChunksWalker walker, List<Vector2> convex) {
        int direction = walker.Direction();
        if (direction == ChunksWalker.DIRECTION_STAY) {
            return;
        }
        int state = walker.State();
        if (state == 4 || state == 12 || state == 14 ||
            (direction == ChunksWalker.DIRECTION_UP && state == 6)) {
            walker.WalkLeft();
            convex.Add(walker.Pos());
        }
    }
}

[RequireComponent(typeof(PolygonCollider2D))]
[ExecuteInEditMode]
public class TileCollider2D : MonoBehaviour {
	public float tileWidth = 1.0f;
	public float tileHeight = 1.0f;
    public Vector2[] chunks;

    private IConvexStateHandler[] mHandlers;
    private PolygonCollider2D mPoly;

    public void Awake() {
        Debug.Log("[TileCollider2D.Awake]");

        List<IConvexStateHandler> handlers = new List<IConvexStateHandler>();
        handlers.Add(new ConvexStartFinder());
        handlers.Add(new WalkingUp());
        handlers.Add(new WalkingRight());
        handlers.Add(new WalkingDown());
        handlers.Add(new WalkingLeft());
        mHandlers = handlers.ToArray();

        mPoly = GetComponent<PolygonCollider2D>();
    }

    public void Start()
    {
        mPoly.SetPath(0, VerticalConvex(chunks));
    }

    private Vector2[] VerticalConvex(Vector2[] chunks) {
        List<Vector2> convex = new List<Vector2>();
        if (chunks.Length <= 0) {
            return convex.ToArray();
        }
        ChunksWalker walker = new ChunksWalker(chunks[0], chunks);
        while (true) {
            foreach (IConvexStateHandler handler in mHandlers) {
                handler.Handle(walker, convex);
            }
            if (convex.Count > 0) {
                Vector2 pos = walker.Pos();
                Vector2 start = convex[0];
                if (ChunksWalker.IsSamePoint(pos, start)) {
                    break;
                }
            }
        }
        return convex.ToArray();
    }
}
