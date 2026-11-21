using Raylib_cs;
using System.Numerics;

namespace Checklist;

internal sealed class Viewport
{
    internal float scroll = 0f;

    /// <summary>
    /// Provides viewport controls like drag-scrolling, normal scrolling, region highlighting etc.
    /// </summary>
    internal void Update() {
        if (Raylib.IsMouseButtonDown(Settings.ScrollDragButton)) {
            scroll += Raylib.GetMouseDelta().X;
            if (Settings.BlockCursorWhenReachedStart && scroll > 0) {
                Raylib.SetMousePosition((int)Raylib.GetMousePosition().X - (int)Raylib.GetMouseDelta().X, (int)Raylib.GetMousePosition().Y);
            }

            if(Settings.WrapCursorWhenScrolling) {
                if (Raylib.GetMousePosition().X < 0) {
                    Raylib.SetMousePosition((int)Raylib.GetScreenWidth()-1, (int)Raylib.GetMousePosition().Y);
                }
                if (Raylib.GetMousePosition().X > Program.width) {
                    Raylib.SetMousePosition(1, (int)Raylib.GetMousePosition().Y);
                }
            }
        }
        scroll += Raylib.GetMouseWheelMove() * Settings.ScrollSpeed / Program.uiScale;

        if (scroll > 0) {
            scroll = 0;
        }
    }
}
