using Raylib_cs;
using System.ComponentModel;
using System.Numerics;

namespace Checklist;

internal sealed class Button {
    public string name { get; private set; } = "Button";
    public string iconPath;
    public Action clickEvent;
    public bool disabled = false;
    public Rectangle rec = new(0, 0, Program.theme.menuBarButtonSize * Program.uiScale, Program.theme.menuBarButtonSize * Program.uiScale);

    private Texture2D icon;
    private bool hovering = false;
    private bool clicking = false;

    private float nameWidth = 0;

    /// <summary>
    /// Loads the icon (if there is one) and warns if the aspect ratio between the button and the icon is different (it won't be, probably).
    /// Will silently fail if icon file doesn't exist, and will instead render the name of the button.
    /// </summary>
    public Button(string name) {
        this.name = name;
        SetName(this.name);

        if (string.IsNullOrWhiteSpace(iconPath)) return;
        if (!Path.Exists(iconPath)) {
            // TODO: add custom console
            Console.WriteLine($"WARN: Button {this.name}'s icon doesn't exist. ({iconPath})");
            return;
        }
        
        icon = Raylib.LoadTexture(iconPath);

        // calc decimal aspect ratio
        float btnQ = rec.Size.X / rec.Size.Y;
        float iconQ = icon.Dimensions.X / icon.Dimensions.Y;

        // aspect ratio discrepancy
        if(btnQ != iconQ) {
            // TODO: add custom console
            Console.WriteLine($"WARN: Button {this.name}'s size ratio does not match icon's. It may appear stretched.\n({this.name}: {btnQ}w:1h, Icon: {iconQ}w:1h)");
        }
    }
    public void Unload() {
        if (string.IsNullOrWhiteSpace(iconPath) || !Path.Exists(iconPath)) return;

        Raylib.UnloadTexture(icon);
    }

    public void SetName(string name) {
        if(string.IsNullOrWhiteSpace(name)) {
            // TODO: add custom console
            Console.WriteLine($"WARN: Button name is empty.");
            nameWidth = 0;
        } else {
            nameWidth = Raylib.MeasureTextEx(Program.theme.font32, name, Program.theme.menuBarButtonTextSize * Program.uiScale, 1f).X;
        }
    }

    /// <summary>
    /// Handles button interaction and rendering.
    /// </summary>
    public void Update() {
        Raylib.DrawTexture(icon, 0, 0, Color.White);

        if (!disabled) {
            hovering = !disabled && Raylib.IsMouseButtonUp(0) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rec);
            clicking = !disabled && hovering && Raylib.IsMouseButtonDown(0);
            
            if (!disabled && hovering && Raylib.IsMouseButtonReleased(0)) {
                clickEvent?.Invoke();
            }

            if(!clicking) {
                Raylib.DrawRectangleRec(rec, hovering && !clicking ? Program.theme.buttonHighlightColor : Program.theme.buttonColor);
            } else {
                Raylib.DrawRectangleRec(rec, Program.theme.buttonPressedColor);
            }
        } else {
            Raylib.DrawRectangleRec(rec, Program.theme.buttonDisabledColor);
        }

        if(string.IsNullOrWhiteSpace(iconPath)) {
            Raylib.DrawTextEx(Program.theme.font32, name, new(rec.X - nameWidth / 2, rec.Y + Program.theme.menuBarButtonTextSize / 2f), Program.theme.menuBarButtonTextSize * Program.uiScale, 1, disabled ? Program.theme.textDisabledColor : Program.theme.textColor);
        } else {

        }
    }
}

internal sealed class MenuBar
{
    internal Button newFile = new("New File") {iconPath = "" };

    public MenuBar() {
        newFile.rec.Position = new(Program.theme.padding / 2);
    }

    /// <summary>
    /// Updates and renders the menu bar
    /// </summary>
    public void Update() {
        Raylib.DrawRectangleV(Vector2.Zero, new(Raylib.GetScreenWidth(), Program.theme.menuBarButtonSize * Program.uiScale + Program.theme.padding), Program.theme.menuBarColor);
        newFile.Update();
    }

    /// <summary>
    /// Unloads button icons
    /// </summary>
    public void Unload() {
        newFile.Unload();
    }
}