using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {
    int numberOfPellets = 10;
    int damagePerPellet = 8;

    [SerializeField]
    Animator animator;

    float camOffsetX = 0.425f;
    float camOffsetY = 0.425f;
    float maxBulletspread = 0.15f;
    float fireRate = 0.7f;
    float nextFire;
    float reloadSpeed = 0.5f;

    bool reloading;

    int power = 10;
    int ejectSpeed = 2;

    public GameObject bulletHole;
    public GameObject[] bloodSplatters;
    public GameObject decalsGO;
    public GameObject casingGO;
    public GameObject casing;
    public GameObject caseEjector;

    private PlayerHandler player;

    RaycastHit hit;
    List<Ray> rays = new List<Ray>();

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        player = this.GetComponent<PlayerHandler>();
    }

    void Update() {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && !reloading) {
            if (player.currentPrimaryAmmo == 0) {
                reloading = true;
                StartCoroutine(Reload());
            } else {
                nextFire = Time.time + fireRate;
                Fire(Gun.Shotgun);
            }
        }
        if (Input.GetButtonDown("Reload")) {
            reloading = true;
            StartCoroutine(Reload());
        }
    }

    void Fire(Gun gun) {
        switch (gun) {
            case Gun.Shotgun:
                for (int i = 0; i < numberOfPellets; i++) {
                    Vector2 rand = new Vector2(Random.Range(0, maxBulletspread), Random.Range(0, maxBulletspread));
                    Ray ray = Camera.main.ViewportPointToRay(rand + new Vector2(camOffsetX, camOffsetY));
                    rays.Add(ray);
                }

                foreach (Ray ray in rays) {
                    if (Physics.Raycast(ray, out hit)) {
                        if (hit.collider.tag == "Player") {
                            hit.collider.GetComponent<PlayerHandler>().currenthealth -= damagePerPellet;
                            GameObject blood;
                            blood = Instantiate(bloodSplatters[Random.Range(0, bloodSplatters.Length)], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), decalsGO.transform);
                            blood.transform.SetParent(hit.collider.gameObject.transform);
                            if (hit.collider.GetComponent<PlayerHandler>().currenthealth <= 0)
                                Destroy(hit.collider.gameObject);
                        } else {
                            Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal), decalsGO.transform);
                        }

                        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                        if (rb != null) {
                            rb.AddForceAtPosition(transform.forward * 15, hit.point);
                        }
                    }
                }

                rays.Clear();
                animator.Play("ShottyFire");
                EjectCasing();
                player.currentPrimaryAmmo--;
                player.UpdateHud();
                break;
        }

    }

    void EjectCasing() {
        GameObject clone;
        clone = Instantiate(casing, caseEjector.transform.position, Quaternion.identity);
        clone.transform.SetParent(casingGO.transform);
        clone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.left * ejectSpeed);
        Destroy(clone, 3f);
    }

    private IEnumerator Reload() {
        animator.Play("ShottyReload");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (player.currentPrimaryAmmo == 0) {
            if (player.currentReserveAmmo < player.playerClass.primaryAmmo) {
                player.currentPrimaryAmmo = player.currentReserveAmmo;
                player.currentReserveAmmo = 0;
            } else {
                player.currentPrimaryAmmo = player.playerClass.primaryAmmo;
                player.currentReserveAmmo -= player.playerClass.primaryAmmo;
            }
        } else if (player.currentReserveAmmo > 0) {
            if (player.currentReserveAmmo >= (player.playerClass.primaryAmmo - player.currentPrimaryAmmo)) {
                player.currentReserveAmmo -= player.playerClass.primaryAmmo - player.currentPrimaryAmmo;
                player.currentPrimaryAmmo = player.playerClass.primaryAmmo;
            } else {
                player.currentPrimaryAmmo += player.currentReserveAmmo;
                player.currentReserveAmmo = 0;
            }
        }
        player.UpdateHud();
        reloading = false;
    }
}