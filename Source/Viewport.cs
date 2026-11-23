using Raylib_cs;
using System.Numerics;

namespace Checklist;

internal sealed class Viewport
{
    public static Vector2 regionChecklists = new();
    internal float scroll = 0f;

    /// <summary>
    /// Provides viewport controls like drag-scrolling, normal scrolling, mouse wrapping, etc.
    /// </summary>
    internal void Update() {
        if (Raylib.IsMouseButtonDown(Settings.ScrollDragButton)) {
            if(Settings.WrapCursorWhenScrolling) {
                if (Raylib.GetMousePosition().X < 0) {
                    Raylib.SetMousePosition(Raylib.GetScreenWidth()+(int)Raylib.GetMouseDelta().X, (int)Raylib.GetMousePosition().Y);
                }
                if (Raylib.GetMousePosition().X > Program.width) {
                    Raylib.SetMousePosition((int)Raylib.GetMouseDelta().X, (int)Raylib.GetMousePosition().Y);
                }
            }

            scroll += Raylib.GetMouseDelta().X;

            if (Settings.BlockCursorWhenReachedStart && scroll > 0) {
                Raylib.SetMousePosition((int)Raylib.GetMousePosition().X - (int)Raylib.GetMouseDelta().X, (int)Raylib.GetMousePosition().Y);
            }
        }
        scroll += Raylib.GetMouseWheelMove() * (Settings.InvertScroll ? -1 : 1) * Settings.ScrollSpeed;

        if (scroll > 0) {
            scroll = 0;
        }
    }
}