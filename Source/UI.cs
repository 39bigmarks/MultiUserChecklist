using Raylib_cs;
using System.Numerics;

namespace Checklist;

/// <summary>
/// Drawing and interaction logic for only buttons for now.
/// </summary>
internal static class UI
{
    /// <summary>
    /// Button that does something. Can be turned into a checkbox.
    /// </summary>
    public sealed class Button {
        public string name { get; private set; } = "Button";
        public string iconPath = "";
        public ButtonClickDelegate clickEvent = null;
        public object[] clickEventData = [];
        public bool disabled = false;
        public bool clickOnDown = false;
        public bool noBackground = false;
        public bool menuBar = false;
        public bool noText = false;
        public Rectangle rec = new(Vector2.Zero, Program.theme.MenuBarButtonSize, Program.theme.MenuBarButtonSize);

        private Texture2D icon;
        private bool hovering = false;
        private bool clicking = false;

        private float nameWidth = 0;

        /// <summary>
        /// Constructs with a name, position, and icon path
        /// If no icon is specified, it will just draw the name in the middle of the button.
        /// If the icon path doesn't exist, a warning is logged.
        /// If the normalized aspect ratios of the button and the icon are different, a warning is logged.
        /// </summary>
        /// <param name="name">The name/label of the button</param>
        /// <param name="position">The location of the button</param>
        /// <param name="iconPath">The path to the icon (should be a .png)</param>
        /// <param name="clickOnDown">Registers a click when first pressing down on the mouse button</param>
        /// <param name="noBackground">Disables the background entirely</param>
        /// <param name="menuBar">Disables background when not hovering</param>
        /// <param name="noText">Disables the rendering of the label</param>
        public Button(string name = "Button", Vector2 position = default, string iconPath = "", bool clickOnDown = false, bool noBackground = false, bool menuBar = false, bool noText = false) {
            this.name = name;
            nameWidth = Raylib.MeasureTextEx(Program.theme.font16, name, Program.theme.ButtonTextSize, 1f).X;
            rec.Position = position;
            this.clickOnDown = clickOnDown;
            this.noBackground = noBackground;
            this.menuBar = menuBar;
            this.noText = noText;

            if (string.IsNullOrWhiteSpace(iconPath)) return;
            if (!Path.Exists(iconPath)) {
                // TODO: add custom console
                Console.WriteLine($"WARN: Button {this.name}'s icon doesn't exist. ({iconPath})");
                return;
            }
        
            icon = Raylib.LoadTexture(iconPath);
            Raylib.GenTextureMipmaps(ref icon);
            Raylib.SetTextureFilter(icon, TextureFilter.Bilinear);

            // calc decimal aspect ratio
            float btnQ = rec.Size.X / rec.Size.Y;
            float iconQ = icon.Dimensions.X / icon.Dimensions.Y;

            // aspect ratio discrepancy
            if(btnQ != iconQ) {
                // TODO: add custom console
                Console.WriteLine($"WARN: Button {this.name}'s size ratio does not match icon's. It may appear stretched.\n({this.name}: {btnQ}w:1h, Icon: {iconQ}w:1h)");
            }
        }

        /// <summary>
        /// (Internal) Sets texture filtering depending on the theme.
        /// </summary>
        internal void SetFilter(TextureFilter filter) {
            Raylib.SetTextureFilter(icon, filter);
        }
        /// <summary>
        /// (Internal) Generates texture mipmapping depending on the theme.
        /// </summary>
        internal void GenTextureMipmaps() {
            Raylib.GenTextureMipmaps(ref icon);
        }

        /// <summary>
        /// Sets the name of the button and determines it's width.
        /// </summary>
        public void SetName(string name) {
            this.name = name;

            if (string.IsNullOrEmpty(name)) {
                nameWidth = 0;
                // TODO: add custom console
                Console.WriteLine($"WARN: Button name length is 0.");
            } else {
                nameWidth = Raylib.MeasureTextEx(Program.theme.font32, name, Program.theme.ButtonTextSize, 1).X;
            }
        }

        /// <summary>
        /// Sets the button's icon if it's texture is already loaded.
        /// </summary>
        public void SetIcon(Texture2D icon) {
            this.icon = icon;
        }

        /// <summary>
        /// Loads a new icon and unloads the previous one.
        /// </summary>
        public void SetIcon(string iconPath) {
            this.iconPath = iconPath;

            Unload();
            icon = Raylib.LoadTexture(this.iconPath);
        }

        /// <summary>
        /// Unloads the icon if available.
        /// </summary>
        public void Unload() {
            if (string.IsNullOrWhiteSpace(iconPath) || !Path.Exists(iconPath)) return;

            Raylib.UnloadTexture(icon);
        }

        /// <summary>
        /// Handles button interaction and rendering
        /// </summary>
        // a bit of a mess though...
        public void Update() {
            // handle mouse interaction if not disabled
            if (!disabled) {
                if(clickOnDown && hovering && Raylib.IsMouseButtonPressed(0))
                    clickEvent?.Invoke(clickEventData);

                clicking = !disabled && hovering && Raylib.IsMouseButtonDown(0);
                hovering = !disabled && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rec);

                if (!clickOnDown && hovering && Raylib.IsMouseButtonReleased(0))
                    clickEvent?.Invoke(clickEventData);
            } else {
                // draw disabled background
                Raylib.DrawRectangleRec(rec, Program.theme.ButtonDisabledColor);
            }

            // draw background
            if (!noBackground && !disabled) {
                if (!hovering && !clicking) {
                    if (!menuBar) Raylib.DrawRectangleRec(rec, Program.theme.ButtonColor);
                } else if (hovering && !clicking) {
                    Raylib.DrawRectangleRec(rec, Program.theme.ButtonHighlightColor);
                }
                if (clicking) {
                    Raylib.DrawRectangleRec(rec, Program.theme.ButtonPressedColor);
                }
            }

            // draw icon, otherwise text
            if(icon.Dimensions != Vector2.Zero) {
                Raylib.DrawTexturePro(icon, new(Vector2.Zero, icon.Dimensions), rec, Vector2.Zero, 0, Color.White);
            } else if(!noText) {
                Raylib.DrawTextEx(
                    Program.theme.font32,
                    name,
                    new(rec.X + (rec.Width - nameWidth) / 2,
                        rec.Y + (rec.Height - Program.theme.ButtonTextSize) / 2),
                    Program.theme.ButtonTextSize,
                    1,
                    disabled ? Program.theme.TextDisabledColor : Program.theme.TextColor
                    );
            }
        }

        /// <summary>
        /// Delegate for the click event
        /// </summary>
        /// <param name="data"></param>
        public delegate void ButtonClickDelegate(object[] data);
    }
}