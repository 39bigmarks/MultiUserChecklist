using Raylib_cs;
using System.Numerics;

namespace Checklist;

/// <summary>
/// Icons are stored in Assets/Icons as 512x512 images.
/// List of icons:
/// NewFile.png
/// </summary>
internal class Theme
{
    public string name { get; set; }
    public string fontPath { get; set; }
    public int fontFilter { get; set; }
    public Color backgroundColor { get; set; }
    public Color menuBarColor { get; set; }
    public Color checklistBgColor { get; set; }
    public Color checklistSecondaryBgColor { get; set; }
    public Color buttonColor { get; set; }
    public Color buttonHighlightColor { get; set; }
    public Color buttonPressedColor { get; set; }
    public Color buttonDisabledColor { get; set; }
    public Color textColor { get; set; }
    public Color textDisabledColor { get; set; }
    public float padding { get; set; }
    public float checklistSpacing { get; set; }
    public float checklistTitleSize { get; set; }
    public float checklistElementSize { get; set; }
    public float checklistCheckboxSize { get; set; }
    public float checklistLineThickness { get; set; }
    public float menuBarButtonSize { get; set; }
    public float menuBarButtonTextSize { get; set; }
    public bool crossout { get; set; }

    public Font font16;
    public Font font32;
    public Font font64;
    public Font font128;
    
    public static Theme DarkTheme = new() {
        name = "Dark Theme",
        fontPath = "Assets\\Fonts\\Open_Sans\\static\\OpenSans-Regular.ttf",
        fontFilter = (int)TextureFilter.Trilinear,
        backgroundColor = new(12, 12, 16),
        menuBarColor = new(48, 48, 56),
        buttonColor = new(64, 64, 74),
        buttonHighlightColor = new(84, 84, 94),
        buttonPressedColor = new(74, 74, 104),
        buttonDisabledColor = new(56, 56, 56),
        checklistBgColor = new(32, 32, 40),
        checklistSecondaryBgColor = new(23, 23, 30),
        textColor = Color.RayWhite,
        textDisabledColor = Color.DarkGray,
        crossout = false,
        padding = 5,
        menuBarButtonSize = 64,
        menuBarButtonTextSize = 256,
        checklistTitleSize = 32,
    };

    public void LoadTheme() {
        if (string.IsNullOrWhiteSpace(fontPath)) {
            font16 = Raylib.GetFontDefault();
            font32 = Raylib.GetFontDefault();
            font64 = Raylib.GetFontDefault();
            font128 = Raylib.GetFontDefault();
            Console.WriteLine($"STYLE INFO: Font was not specified, reverting to default. ({fontPath})");
        } else {
            if (!Path.Exists(fontPath)) {
                // TODO: add custom console
                Console.WriteLine("STYLE WARN: Theme's font path doesn't exist, reverting to default.");
                return;
            }

            font16 = Raylib.LoadFontEx(fontPath, 16, null, 0);
            font32 = Raylib.LoadFontEx(fontPath, 32, null, 0);
            font64 = Raylib.LoadFontEx(fontPath, 64, null, 0);
            font128 = Raylib.LoadFontEx(fontPath, 128, null, 0);
            
            Raylib.GenTextureMipmaps(ref font16.Texture);
            Raylib.GenTextureMipmaps(ref font32.Texture);
            Raylib.GenTextureMipmaps(ref font64.Texture);
            Raylib.GenTextureMipmaps(ref font128.Texture);

            Raylib.SetTextureFilter(font16.Texture, (TextureFilter)fontFilter);
            Raylib.SetTextureFilter(font32.Texture, (TextureFilter)fontFilter);
            Raylib.SetTextureFilter(font64.Texture, (TextureFilter)fontFilter);
            Raylib.SetTextureFilter(font128.Texture, (TextureFilter)fontFilter);

        }
    }

    /// <summary>
    /// Unloads the fonts
    /// </summary>
    public void Unload() {
        Raylib.UnloadFont(font16);
        Raylib.UnloadFont(font32);
        Raylib.UnloadFont(font64);
        Raylib.UnloadFont(font128);
    }
}
