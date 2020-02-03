using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerEvents : MonoBehaviour
{
    [SerializeField]
    GameObject wardrobe, vendettaHand;
    [SerializeField]
    Transform[] spawnPointsVendetta;
    [SerializeField]
    Animator eventAnim;
    [SerializeField]
    Lightning thunderScript;
    [SerializeField]
    LampFlicker lampFlicker;
    [SerializeField]
    float incubiInAgguatoTimer, ceQualcunoNelBuioTimer, dontScreamTimer, vendettaStriscianteTimer;
    
    [Space]
    [SerializeField][Range(0.01f,1f)]
    float vendettaStriscianteChance, aVolteSiAnimanoChance;

    [Space]
    [SerializeField] [Range(0.01f, 1f)]
    float turnAroundChance, swapItemChance, objectThrowChance, objectThrowTimer;

    [Header("DEBUG")]
    [SerializeField]
    bool IIA_status, IRDO_status, CQNB_status, DS_status;
    [SerializeField]
    float timer, _vTimer, _oTimer;
    bool closeToController;
    GameObject _vHand;
    Throwable[] allPhysicalItems;
    [SerializeField]
    Vector3 faceHuggerDir;
    [SerializeField]
    Transform faceHugger;
    [SerializeField]
    bool isFaceHugging;

    private bool inThisState(string clipName)
    {
        return eventAnim.GetCurrentAnimatorStateInfo(0).IsName(clipName);
    }

    private void aVolteSiAnimano()
    {
        allPhysicalItems = new Throwable[FindObjectsOfType<Throwable>().Length];
        allPhysicalItems = FindObjectsOfType<Throwable>();

        var i = Random.Range(0, allPhysicalItems.Length);

        if (!RendererExtensions.IsVisibleFrom(allPhysicalItems[i].GetComponent<Renderer>(), Camera.main))
        {
            var chance = Random.Range(0f, 1f);
            var chance1 = Random.Range(0f, 1f);
            var chance2 = Random.Range(0f, 1f);

            if (chance <= turnAroundChance)
            {
                allPhysicalItems[i].transform.LookAt(Player.instance.hmdTransform);

                print("A Volte Si Animano : Triggered, " + allPhysicalItems[i].gameObject.name + " is facing player");
            }
            if (chance1 <= swapItemChance)
            {
                var item2 = allPhysicalItems[Random.Range(0, allPhysicalItems.Length)].transform;
                var item = allPhysicalItems[i].transform;
                var pos = item.position;
                var rot = item.rotation;

                if(item2 == item) { print("A Volte Si Animano : NOT Triggered"); return; }

                item.GetComponent<Collider>().enabled = false;
                item.SetPositionAndRotation(item2.position, item2.rotation);
                item2.SetPositionAndRotation(pos, rot);              
                item.GetComponent<Collider>().enabled = true;

                print("A Volte Si Animano : Triggered, " + item.name + " is swapped with " + item2.name);
            }
            if (chance2 <= objectThrowChance && !isFaceHugging)
            {
                var dir = Player.instance.hmdTransform.position - allPhysicalItems[i].transform.position;
                _oTimer = objectThrowTimer;
                faceHuggerDir = dir;
                faceHugger = allPhysicalItems[i].transform;
                isFaceHugging = true;

                print("A Volte Si Animano : Triggered, " + allPhysicalItems[i].gameObject.name + " is about to jump to player's face");
            }
        }
        else
        {
            print("A Volte Si Animano : NOT Triggered");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "IncubiInAgguato_Trigger" && !IIA_status)
        {
            IIA_status = true;
            eventAnim.SetTrigger("IncubiInAgguato");
            //print("i will play a slam sound");
            //print("i will play some creepy hiss sound");

            print("Incubi In Agguato : Triggered_byProximity");
        }
        if (other.gameObject.name == "IlReDellOrrore_Trigger")
        {
            closeToController = true;
        }
        if (other.gameObject.name == "IlReDellOrrore_Area" && closeToController && !IRDO_status)
        {
            IRDO_status = true;
            thunderScript.DoLighting();
            eventAnim.SetTrigger("IlReDellOrrore");

            print("Il Re Dell'Orrore : Triggered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "IlReDellOrrore_Trigger")
        {
            closeToController = false;
        }
    }

    private void LateUpdate()
    {
        if(_vHand == null)
        {
            if(Random.Range(0f,1f) <= vendettaStriscianteChance/1000)
            {
                var randomSpawn = Random.Range(0, spawnPointsVendetta.Length);
                _vHand = Instantiate(vendettaHand, spawnPointsVendetta[randomSpawn]);
                _vTimer = vendettaStriscianteTimer;

                print("Vendetta Strisciante : START_Triggered, hand is at :" + spawnPointsVendetta[randomSpawn].position);
            }
        }
        else
        {
            if(RendererExtensions.IsVisibleFrom(_vHand.GetComponent<Renderer>(), Camera.main) && _vTimer >= 0)
            {
                _vTimer -= Time.deltaTime;
                if(_vTimer <= 0)
                {
                    _vHand = null;
                    _vTimer = vendettaStriscianteTimer;

                    print("Vendetta Strisciante : END_Triggered");
                }
            }
        }
        
        if(Random.Range(0f,1f) <= aVolteSiAnimanoChance)
        {
            aVolteSiAnimano();
        }

        if(isFaceHugging && faceHugger != null)
        {
            if (RendererExtensions.IsVisibleFrom(faceHugger.GetComponent<Renderer>(), Camera.main))
            {
                if (_oTimer >= 0)
                {
                    _oTimer -= Time.deltaTime;
                }
                else
                {
                    faceHugger.GetComponent<Rigidbody>().AddForce(faceHuggerDir * 50, ForceMode.Impulse);
                    isFaceHugging = false;

                    print("A Volte Si Animano: Triggered, " + faceHugger.name + " is now jumping to player's face");
                    faceHugger = null;
                }
            }
            else
            {
                _oTimer = objectThrowTimer;
            }
        }

        if(RendererExtensions.IsVisibleFrom(wardrobe.GetComponent<Renderer>(), Camera.main) && incubiInAgguatoTimer >= 0)
        {
            incubiInAgguatoTimer -= Time.deltaTime;

            if(incubiInAgguatoTimer <= 0)
            {
                IIA_status = true;
                eventAnim.SetTrigger("IncubiInAgguato");

                print("Incubi In Agguato : Triggered_byLook");
            }
        }

        if (!inThisState("CeQualcunoNelBuio") || inThisState("CeQualcunoNelBuioReverse"))
        {
            timer += Time.deltaTime;
        }

        if (timer >= ceQualcunoNelBuioTimer && !CQNB_status)
        {
            timer = 0;
            CQNB_status = true;
            eventAnim.SetTrigger("CeQualcunoNelBuio");
            print("I will play some footsteps approaching the door");
            print("i will then play some cries");
            print("the footsteps leaving the door");

            print("C'e' Qualcuno Nel Buio : Triggered");
        }
        if (timer >= dontScreamTimer && CQNB_status && !DS_status)
        {
            timer = 0;
            DS_status = true;
            lampFlicker.DoFlicker();
            eventAnim.SetTrigger("DontScream");
            print("I will play some footsteps rapidly approaching the door");
            print("i will play a hard hit on door");

            print("Don't Scream : Triggered");
        }
    }

}
