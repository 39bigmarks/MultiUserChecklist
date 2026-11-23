using Raylib_cs;
using System.Numerics;

namespace Checklist;

/// <summary>
/// Icons are stored in Assets/Icons as 512x512 images
/// </summary>
internal class Theme
{
    // first two are specified so that the compiler doesn't go "WAHH WAHH WAHH!!!!"
    public string Name                      { get; set; } = "";
    public string FontPath                  { get; set; } = "";
    public float UiScale                    { get; set; }
    public int FontFilter                   { get; set; }
    public int IconFilter                   { get; set; }
    public Color BackgroundColor            { get; set; }
    public Color MenuBarColor               { get; set; }
    public Color ChecklistTitleColor        { get; set; }
    public Color ChecklistItemColor         { get; set; }
    public Color ChecklistBgColor           { get; set; }
    public Color ChecklistSecondaryBgColor  { get; set; }
    public Color ButtonColor                { get; set; }
    public Color ButtonHighlightColor       { get; set; }
    public Color ButtonPressedColor         { get; set; }
    public Color ButtonDisabledColor        { get; set; }
    public Color TextColor                  { get; set; }
    public Color TextDisabledColor          { get; set; }
    public Vector2 NewChecklistButtonSize   { get; set; }
    public float Padding                    { get; set; }
    public float ChecklistSpacing           { get; set; }
    public float ChecklistTitleSize         { get; set; }
    public float ChecklistItemSize          { get; set; }
    public float ChecklistItemSpacing       { get; set; }
    public float ChecklistWidth             { get; set; }
    public float ChecklistBoxSize           { get; set; }
    public float ChecklistLineThickness     { get; set; }
    public float MenuBarButtonSize          { get; set; }
    public float MenuBarButtonSpacing       { get; set; }
    public float ButtonTextSize             { get; set; }
    public bool PadChecklistEmptySpace      { get; set; }
    public bool Crossout                    { get; set; }

    public Font font16;
    public Font font28;
    public Font font32;
    public Font font64;
    public Font font128;
    
    public static Theme DarkTheme = new() {
        Name = "Dark Theme",
        FontPath = "Assets\\Fonts\\Open_Sans\\static\\OpenSans-Regular.ttf",
        UiScale = 1,
        FontFilter = (int)TextureFilter.Trilinear,
        IconFilter = (int)TextureFilter.Trilinear,
        BackgroundColor = new(12, 12, 16),
        MenuBarColor = new(48, 48, 56),
        ChecklistTitleColor = new(102, 191, 255),
        ChecklistItemColor = new(245, 245, 245),
        ButtonColor = new(55, 55, 79),
        ButtonHighlightColor = new(84, 84, 114),
        ButtonPressedColor = new(74, 74, 104),
        ButtonDisabledColor = new(56, 56, 56),
        ChecklistBgColor = new(32, 32, 40),
        ChecklistSecondaryBgColor = new(23, 23, 30),
        TextColor = new (245, 245, 245),
        TextDisabledColor = new Color(95, 95, 95),
        PadChecklistEmptySpace = false,
        Crossout = false,
        Padding = 8,
        ButtonTextSize = 18,
        ChecklistSpacing = 0,
        MenuBarButtonSize = 48,
        MenuBarButtonSpacing = 5,
        ChecklistTitleSize = 32,
        ChecklistItemSize = 28,
        ChecklistItemSpacing = 15,
        ChecklistBoxSize = 36,
        ChecklistWidth = 100,
    };

    /// <summary>
    /// Asynchronously loads the theme
    /// </summary>
    public async Task LoadTheme() {
        Console.WriteLine("STYLE INFO: Loading Theme...");
        Unload();

        if (string.IsNullOrWhiteSpace(FontPath)) {
            font16 = Raylib.GetFontDefault();
            font28 = Raylib.GetFontDefault();
            font32 = Raylib.GetFontDefault();
            font64 = Raylib.GetFontDefault();
            font128 = Raylib.GetFontDefault();
            Console.WriteLine($"STYLE INFO: Font was not specified, reverting to default. ({FontPath})");
        } else {
            if (!Path.Exists(FontPath)) {
                // TODO: add custom console
                Console.WriteLine("STYLE WARN: Theme's font path doesn't exist, reverting to default.");
                return;
            }

            // font loading and initialization
            font16 = Raylib.LoadFontEx(FontPath, 16, null, 0);
            font28 = Raylib.LoadFontEx(FontPath, 28, null, 0);
            font32 = Raylib.LoadFontEx(FontPath, 32, null, 0);
            font64 = Raylib.LoadFontEx(FontPath, 64, null, 0);
            font128 = Raylib.LoadFontEx(FontPath, 128, null, 0);
            
            // if above "point" and "bilinear" (bilinear supposedly doesn't use mipmaps)
            if(FontFilter > 1) {
                Raylib.GenTextureMipmaps(ref font16.Texture);
                Raylib.GenTextureMipmaps(ref font28.Texture);
                Raylib.GenTextureMipmaps(ref font32.Texture);
                Raylib.GenTextureMipmaps(ref font64.Texture);
                Raylib.GenTextureMipmaps(ref font128.Texture);
            }

            Raylib.SetTextureFilter(font16.Texture, (TextureFilter)FontFilter);
            Raylib.SetTextureFilter(font28.Texture, (TextureFilter)FontFilter);
            Raylib.SetTextureFilter(font32.Texture, (TextureFilter)FontFilter);
            Raylib.SetTextureFilter(font64.Texture, (TextureFilter)FontFilter);
            Raylib.SetTextureFilter(font128.Texture, (TextureFilter)FontFilter);
            
            if(IconFilter > 1) {
                Program.menuBar.GenTextureMipmaps();
            }

            Program.menuBar.SetFilter((TextureFilter)IconFilter);

            await Checklist.LoadTextures();
        }
    }

    /// <summary>
    /// Unloads the fonts and icons
    /// </summary>
    public void Unload() {
        Program.menuBar.Unload();
        Checklist.Unload();

        Raylib.UnloadFont(font16);
        Raylib.UnloadFont(font28);
        Raylib.UnloadFont(font32);
        Raylib.UnloadFont(font64);
        Raylib.UnloadFont(font128);
    }
}
