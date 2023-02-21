using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject ui;
    [SerializeField] Item[] items;
    int itemIndex;
    int previousItemIndex = -1;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    bool grounded;
    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    Rigidbody rb;
    PhotonView PV;
    const float maxHealth = 100f;
    float currentHealth = maxHealth;
    PlayerManager playerManager;

    private float lasttime = -50.0f;

    private Animator pistol;
    private Animator rifle;
    int triggerHash = Animator.StringToHash("Shoot");

    void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        if(PV.IsMine){
            EquipItem(0);
            rifle = items[0].itemGameObject.GetComponent<Animator>();
            pistol = items[1].itemGameObject.GetComponent<Animator>();
        }else{
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }
    }

    void Update()
    {
        if(!PV.IsMine){
            return;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Look();
        Move();
        Jump();

       for (int i = 0; i < items.Length; i++)
       {
            if(Input.GetKeyDown((i+1).ToString())){
                EquipItem(i);
                break;
            }
        
       }

       if(Input.GetAxisRaw("Mouse ScrollWheel")>0f){
            if(itemIndex>=items.Length-1){
                EquipItem(0);
            }else{
                EquipItem(itemIndex+1);
            }
       }else if(Input.GetAxisRaw("Mouse ScrollWheel")<0f){
            if(itemIndex<=0){
                EquipItem(items.Length-1);
            }else{
                EquipItem(itemIndex-1);
            }
       }
       if(Input.GetMouseButtonDown(0) &&(itemIndex==0) && (Time.time - lasttime > 1.5f)){
            // Animator someWeapon;
            // someWeapon = items[itemIndex].itemGameObject.GetComponent<Animator>();
            // items[itemIndex].Use();
            // rifle.SetTrigger(triggerHash);
            // lasttime = Time.time;
            ShootRifleAnimation(triggerHash);
        }
        else if(Input.GetMouseButtonDown(0) &&(itemIndex==1) && (Time.time - lasttime > 0.4f)){
            // Animator someWeapon;
            // someWeapon = items[itemIndex].itemGameObject.GetComponent<Animator>();
            // items[itemIndex].Use();
            // pistol.SetTrigger(triggerHash);
            // lasttime = Time.time;

            // Hashtable props = new Hashtable();
            // props["Trigger"] = triggerHash;
            // PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            ShootPistolAnimation(triggerHash);
        }
        if(transform.position.y < -10f){
            Die();
        }
    }

    void ShootPistolAnimation(int _triggerHash){
        items[itemIndex].Use();
        pistol.SetTrigger(_triggerHash);
        lasttime = Time.time;
        if(PV.IsMine){
            Hashtable props = new Hashtable();
            props["Trigger"] = _triggerHash;
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
    }

        void ShootRifleAnimation(int _triggerHash){
        items[itemIndex].Use();
        rifle.SetTrigger(_triggerHash);
        lasttime = Time.time;
        if(PV.IsMine){
            Hashtable props = new Hashtable();
            props["Trigger"] = _triggerHash;
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
    }

    void Move(){
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir*(Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    void Look(){
        transform.Rotate(Vector3.up*Input.GetAxisRaw("Mouse X")*mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y")*mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Jump(){
        if(Input.GetKeyDown(KeyCode.Space) && grounded){
            rb.AddForce(transform.up * jumpForce);
       }
    }

    public void SetGroundedState(bool _grounded){
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if(!PV.IsMine){
            return;
        }
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount)* Time.fixedDeltaTime);   
    }

    void EquipItem(int _index){
        if(_index == previousItemIndex){
            return;
        }
        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);

        if(previousItemIndex != -1){
            items[previousItemIndex].itemGameObject.SetActive(false);
        } 
        previousItemIndex = itemIndex;

        if(PV.IsMine){
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex",itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(changedProps.ContainsKey("itemIndex") && !PV.IsMine && targetPlayer == PV.Owner){
            EquipItem((int)changedProps["itemIndex"]);
        }
        if (changedProps.ContainsKey("Trigger") && !PV.IsMine && targetPlayer == PV.Owner)
        {
            ShootPistolAnimation((int)changedProps["Trigger"]);
        }
    }

    public void TakeDamage(float damage){
       PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info){

        currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / maxHealth;

        if(currentHealth <= 0){
            Die();
            PlayerManager.Find(info.Sender).GetKill();
        }
    }
    void Die(){
        playerManager.Die();
    }
}

    
