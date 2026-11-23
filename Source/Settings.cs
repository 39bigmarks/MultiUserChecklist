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
    /// Scroll wheel speed
    /// </summary>
    public static float ScrollSpeed { get; set; } = 30f;

    /// <summary>
    /// Will invert the direction of normal scroll
    /// </summary>
    public static bool InvertScroll { get; set; } = false;

    /// <summary>
    /// Will auto-scroll to the most recently created list
    /// </summary>
    public static bool ScrollToNewList { get; set; } = false;
}
