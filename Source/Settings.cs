using Raylib_cs;

namespace Checklist;

internal static class Settings
{
    /// <summary>
    /// Will wrap the cursor around the screen when drag-scrolling
    /// </summary>
    public static bool WrapCursorWhenScrolling { get; set; } = true;

    /// <summary>
    /// Will block the cursor from moving further when the beginning of the checklist array is reached
    /// </summary>
    public static bool BlockCursorWhenReachedStart { get; set; } = true;

    /// <summary>
    /// The mouse button you hold to drag-scroll through the checklist array
    /// </summary>
    public static MouseButton ScrollDragButton { get; set; } = MouseButton.Right;

    /// <summary>
    /// Scroll wheel speed.
    /// </summary>
    public static float ScrollSpeed { get; set; } = 30f;
}
