public enum Gun {
    Shotgun
}

public enum ClassName {
    Shotty
}

public class PlayerClass {
    public ClassName className;
    public Gun gun;
    public int health;
    public int primaryAmmo;
    public int reserveAmmo;
    public float moveSpeed;

    // Start is called before the first frame update
    public PlayerClass(ClassName name, Gun g, int hp, int pAmmo, int rAmmo, float mSpeed) {
        className = name;
        gun = g;
        health = hp;
        primaryAmmo = pAmmo;
        reserveAmmo = rAmmo;
        moveSpeed = mSpeed;
    }

    /// <summary>
    /// Get a preset class based on the currently existing ClassName values, returns a Shotty class if the given enum value isn't found
    /// </summary>
    /// <param name="className">The desired preset to retrieve</param>
    /// <returns></returns>
    public static PlayerClass GetPreset(ClassName className) {
        switch (className) {
            case ClassName.Shotty:
                return new PlayerClass(ClassName.Shotty, Gun.Shotgun, 100, 6, 36, 6.0f);
            default:
                return new PlayerClass(ClassName.Shotty, Gun.Shotgun, 100, 6, 36, 6.0f);
        }
    }
}
