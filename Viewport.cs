using Raylib_cs;
using System.Numerics;

namespace Checklist;

internal class Viewport
{
    internal float scroll = 0f;
    internal float scrollSpeed = 20f;

    internal void Update() {
        if (Raylib.IsMouseButtonDown(MouseButton.Middle)) {
            scroll += Raylib.GetMouseDelta().X;
            if (scroll > 0) {
                Raylib.SetMousePosition((int)Raylib.GetMousePosition().X - (int)Raylib.GetMouseDelta().X, (int)Raylib.GetMousePosition().Y);
            }
        }
        scroll += Raylib.GetMouseWheelMove() * scrollSpeed;

        if (scroll > 0) {
            scroll = 0;
        }

    }
}
