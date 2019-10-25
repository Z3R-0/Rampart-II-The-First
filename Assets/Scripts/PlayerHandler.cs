using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour {
    [System.NonSerialized]
    public PlayerClass playerClass;
    [SerializeField]
    private ClassName Class;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text primaryAmmoText;
    [SerializeField]
    private Text reserveAmmoText;
    

    [System.NonSerialized]
    public int currenthealth;
    [System.NonSerialized]
    public int currentPrimaryAmmo;
    [System.NonSerialized]
    public int currentReserveAmmo;

    void Start() {
        playerClass = PlayerClass.GetPreset(Class);
        currenthealth = playerClass.health;
        currentPrimaryAmmo = playerClass.primaryAmmo;
        currentReserveAmmo = playerClass.reserveAmmo;

        UpdateHud();
    }
    
    
    public void UpdateHud() {
        healthText.text = currenthealth.ToString();
        primaryAmmoText.text = currentPrimaryAmmo.ToString();
        reserveAmmoText.text = currentReserveAmmo.ToString();
    }

    public static IEnumerator WaitForAnimation(Animation animation) {
        do {
            yield return null;
        } while (animation.isPlaying);
    }
}
