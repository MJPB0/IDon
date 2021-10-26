using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Player player;
    private GameUI gameUI;

    [SerializeField] private GameObject currentPickupTarget;

    [Space]
    [Header("Equipment")]
    [SerializeField] private Transform eq;
    [SerializeField] private Transform playerUsables;
    [SerializeField] private Transform items;
    [SerializeField] private Transform usables;

    [Space]
    [SerializeField] private List<GameObject> objectsInRange;

    private void Awake()
    {
        player = GetComponent<Player>();
        gameUI = FindObjectOfType<GameUI>();
    }

    private void Start()
    {
        objectsInRange = new List<GameObject>();

        Item[] currentEq = { player.Headpiece, player.Shoulders, player.Chestpiece, player.Belt, player.Legpiece, player.Gloves, player.Boots, player.LeftHand, player.RightHand };
        Consumable currentConsumable = player.PlayerConsumable;
        gameUI.UpdateEqUI(currentEq);
        gameUI.UpdateEquippedItemsDurabilitiesUI(currentEq);
        gameUI.UpdateConsumableUI(currentConsumable);
    }

    private void Update()
    {
        SetCurrentTarget();
        if (currentPickupTarget && currentPickupTarget.CompareTag("Item"))
        {
            currentPickupTarget.TryGetComponent(out Item item);
            ToggleItemInfo(item);
        }
        else if (currentPickupTarget && currentPickupTarget.CompareTag("Consumable"))
        {
            currentPickupTarget.TryGetComponent(out Consumable consumable);
            ToggleConsumableInfo(consumable);
        }
    }

    private void ToggleItemInfo(Item closestItem)
    {
        switch (closestItem.ItemInfo.Slot)
        {
            case Slot.HEAD:
                if (player.Headpiece)
                    closestItem.UpdateInfoUI(player.Headpiece.ItemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.SHOULDERS:
                if (player.Shoulders)
                    closestItem.UpdateInfoUI(player.Shoulders.ItemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.CHEST:
                if (player.Chestpiece)
                    closestItem.UpdateInfoUI(player.Chestpiece.ItemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.WAIST:
                if (player.Belt)
                    closestItem.UpdateInfoUI(player.Belt.ItemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.LEGS:
                if (player.Legpiece)
                    closestItem.UpdateInfoUI(player.Legpiece.ItemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.HANDS:
                if (player.Gloves)
                    closestItem.UpdateInfoUI(player.Gloves.ItemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.FEET:
                if (player.Boots)
                    closestItem.UpdateInfoUI(player.Boots.ItemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.LEFTHAND:
                if (player.LeftHand && player.RightHand && closestItem.ItemInfo.WeaponType == WeaponType.TWOHANDED)
                    closestItem.UpdateInfoUI(player);
                else if (player.LeftHand)
                    closestItem.UpdateInfoUI(player.LeftHand.ItemInfo);
                else if (player.RightHand && closestItem.ItemInfo.WeaponType == WeaponType.TWOHANDED)
                    closestItem.UpdateInfoUI(player.RightHand.ItemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.RIGHTHAND:
                if (player.RightHand)
                    closestItem.UpdateInfoUI(player.RightHand.ItemInfo);
                else if (player.LeftHand.ItemInfo.WeaponType == WeaponType.TWOHANDED)
                    closestItem.UpdateInfoUI(player.LeftHand.ItemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            default:
                closestItem.UpdateInfoUI();
                break;
        }
        closestItem.ToggleInfoUI(true);
    }
    private void ToggleConsumableInfo(Consumable closestConsumable)
    {
        if (player.PlayerConsumable)
            closestConsumable.UpdateInfoUI(player.PlayerConsumable.itemInfo);
        else
            closestConsumable.UpdateInfoUI();
        closestConsumable.ToggleInfoUI(true);
    }

    private void SetCurrentTarget()
    {
        if (currentPickupTarget)
            ToggleOutline(false);

        if (objectsInRange.Count != 0)
        {
            GameObject closestObj = null;
            if (objectsInRange.Count != 0)
                closestObj = objectsInRange[0];
            foreach (GameObject itemGO in objectsInRange)
            {
                if (Vector3.Distance(transform.position, closestObj.transform.position) > Vector3.Distance(transform.position, itemGO.transform.position))
                {
                    closestObj = itemGO;
                }
            }

            if (currentPickupTarget && closestObj && currentPickupTarget != closestObj)
            {
                if (currentPickupTarget.TryGetComponent(out Item item))
                    item.ToggleInfoUI(false);
                else if (currentPickupTarget.TryGetComponent(out Consumable consumable))
                    consumable.ToggleInfoUI(false);
            }

            currentPickupTarget = closestObj;

            if (currentPickupTarget)
                ToggleOutline(true);
        }
    }

    public void CheckRange()
    {
        if (!player.CanAttack)
            return;

        if (!currentPickupTarget)
            return;

        if (objectsInRange.Count <= 0)
            return;

        if (currentPickupTarget.CompareTag("Item"))
            PickItemUp(currentPickupTarget);
        else if (currentPickupTarget.CompareTag("Consumable"))
            ConsumablePickup(currentPickupTarget);
    }

    void PickItemUp(GameObject closestIt)
    {
        Slot itemSlot = closestIt.GetComponent<Item>().ItemInfo.Slot;
        Item item = closestIt.GetComponent<Item>();

        switch (itemSlot)
        {
            case Slot.HEAD:
                if (!player.Headpiece)
                {
                    EquipItemHead(closestIt);
                }
                else
                {
                    RemoveItemHead();
                    EquipItemHead(closestIt);
                }
                break;
            case Slot.SHOULDERS:
                if (!player.Shoulders)
                {
                    EquipItemShoulder(closestIt);
                }
                else
                {
                    RemoveItemShoulder();
                    EquipItemShoulder(closestIt);
                }
                break;
            case Slot.CHEST:
                if (!player.Chestpiece)
                {
                    EquipItemChest(closestIt);
                }
                else
                {
                    RemoveItemChest();
                    EquipItemChest(closestIt);
                }
                break;
            case Slot.HANDS:
                if (!player.Gloves)
                {
                    EquipItemHands(closestIt);
                }
                else
                {
                    RemoveItemHands();
                    EquipItemHands(closestIt);
                }
                break;
            case Slot.WAIST:
                if (!player.Belt)
                {
                    EquipItemWaist(closestIt);
                }
                else
                {
                    RemoveItemWaist();
                    EquipItemWaist(closestIt);
                }
                break;
            case Slot.LEGS:
                if (!player.Legpiece)
                {
                    EquipItemLegs(closestIt);
                }
                else
                {
                    RemoveItemLegs();
                    EquipItemLegs(closestIt);
                }
                break;
            case Slot.FEET:
                if (!player.Boots)
                {
                    EquipItemFeet(closestIt);
                }
                else
                {
                    RemoveItemFeet();
                    EquipItemFeet(closestIt);
                }
                break;
            case Slot.LEFTHAND:
                if (!player.LeftHand)
                {
                    if ((player.RightHand && item.ItemInfo.WeaponType != WeaponType.TWOHANDED) || !player.RightHand)
                    {
                        EquipItemLeftHand(closestIt);
                    }else if (player.RightHand && item.ItemInfo.WeaponType == WeaponType.TWOHANDED)
                    {
                        RemoveItemRightHand(transform.position);
                        EquipItemLeftHand(closestIt);
                    }
                }
                else {
                    if (player.RightHand && item.ItemInfo.WeaponType == WeaponType.TWOHANDED)
                    {
                        RemoveItemRightHand(transform.position);

                        RemoveItemLeftHand(new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z));

                        EquipItemLeftHand(closestIt);
                    }
                    else if (!player.RightHand || item.ItemInfo.WeaponType != WeaponType.TWOHANDED)
                    {
                        RemoveItemLeftHand(transform.position);

                        EquipItemLeftHand(closestIt);
                    }
                }
                break;
            case Slot.RIGHTHAND:
                if (!player.LeftHand)
                {
                    if (!player.RightHand)
                    {
                        EquipItemRightHand(closestIt);
                    }
                    else
                    {
                        RemoveItemRightHand(transform.position);
                        EquipItemRightHand(closestIt);
                    }
                }
                else
                {
                    if (player.LeftHand.ItemInfo.WeaponType != WeaponType.TWOHANDED && !player.RightHand)
                    {
                        EquipItemRightHand(closestIt);
                    }else if (player.LeftHand.ItemInfo.WeaponType != WeaponType.TWOHANDED && player.RightHand)
                    {
                        RemoveItemRightHand(transform.position);
                        EquipItemRightHand(closestIt);
                    }else if (player.LeftHand.ItemInfo.WeaponType == WeaponType.TWOHANDED)
                    {
                        RemoveItemLeftHand(transform.position);
                        EquipItemRightHand(closestIt);
                    }
                }
                break;
        }
        Item[] currentEq = { player.Headpiece, player.Shoulders, player.Chestpiece, player.Belt, player.Legpiece, player.Gloves, player.Boots, player.LeftHand, player.RightHand};
        gameUI.UpdateEqUI(currentEq);
        gameUI.UpdateEquippedItemsDurabilitiesUI(currentEq);
    }
    void ConsumablePickup(GameObject closestCons)
    {
        if (!player.PlayerConsumable)
            EquipConsumable(closestCons);
        else
        {
            RemoveConsumable();
            EquipConsumable(closestCons);
        }
        Consumable currentC = player.PlayerConsumable;
        gameUI.UpdateConsumableUI(currentC);
    }

    void ToggleOutline(bool isActive)
    {
        Outline outline = currentPickupTarget.GetComponentInChildren<Outline>();
        if (isActive)
        {
            outline.OutlineColor = Color.yellow;
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineWidth = 2f;
        }
        outline.enabled = isActive;
    }

    void EquipItemLeftHand(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.LeftHand = it;
        player.CountStats();
        it.ItemInfo.IsPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        objectsInRange.Remove(cItem);

        if (it.ItemInfo.WeaponType == WeaponType.TWOHANDED)
            player.ToggleColliderAOE(true);
    }
    void EquipItemRightHand(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.RightHand = it;
        player.CountStats();
        it.ItemInfo.IsPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        objectsInRange.Remove(cItem);
    }
    void EquipConsumable(GameObject cCons)
    {
        Consumable cons = cCons.GetComponent<Consumable>();
        player.PlayerConsumable = cons;
        cons.itemInfo.IsPickable = false;
        cCons.transform.SetParent(playerUsables);
        cCons.transform.position = new Vector3(0, 0, 0);
        cCons.SetActive(false);
        objectsInRange.Remove(cCons);
    }

    void EquipItemHead(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Headpiece = it;
        player.CountStats();
        it.ItemInfo.IsPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        objectsInRange.Remove(cItem);
    }
    void EquipItemShoulder(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Shoulders = it;
        player.CountStats();
        it.ItemInfo.IsPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        objectsInRange.Remove(cItem);
    }
    void EquipItemChest(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Chestpiece = it;
        player.CountStats();
        it.ItemInfo.IsPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        objectsInRange.Remove(cItem);
    }
    void EquipItemHands(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Gloves = it;
        player.CountStats();
        it.ItemInfo.IsPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        objectsInRange.Remove(cItem);
    }
    void EquipItemWaist(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Belt = it;
        player.CountStats();
        it.ItemInfo.IsPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        objectsInRange.Remove(cItem);
    }
    void EquipItemLegs(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Legpiece = it;
        player.CountStats();
        it.ItemInfo.IsPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        objectsInRange.Remove(cItem);
    }
    void EquipItemFeet(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Boots = it;
        player.CountStats();
        it.ItemInfo.IsPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        objectsInRange.Remove(cItem);
    }

    void RemoveItemLeftHand(Vector3 pos)
    {
        GameObject oldItem = player.LeftHand.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = pos;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().ItemInfo.IsPickable = true;
        player.LeftHand = null;

        if (oldItem.GetComponent<Item>().ItemInfo.WeaponType == WeaponType.TWOHANDED)
            player.ToggleColliderAOE(false);
    }
    void RemoveItemRightHand(Vector3 pos)
    {
        GameObject oldItem = player.RightHand.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = pos;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().ItemInfo.IsPickable = true;
        player.RightHand = null;
    }
    void RemoveConsumable()
    {
        GameObject oldCons = player.PlayerConsumable.gameObject;
        oldCons.transform.SetParent(usables);
        oldCons.transform.position = transform.position;
        oldCons.SetActive(true);
        oldCons.GetComponent<Consumable>().itemInfo.IsPickable = true;
        player.PlayerConsumable = null;
    }

    void RemoveItemHead()
    {
        GameObject oldItem = player.Headpiece.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().ItemInfo.IsPickable = true;
        player.Headpiece = null;
    }
    void RemoveItemShoulder()
    {
        GameObject oldItem = player.Shoulders.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().ItemInfo.IsPickable = true;
        player.Shoulders = null;
    }
    void RemoveItemChest()
    {
        GameObject oldItem = player.Chestpiece.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().ItemInfo.IsPickable = true;
        player.Chestpiece = null;
    }
    void RemoveItemHands()
    {
        GameObject oldItem = player.Gloves.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().ItemInfo.IsPickable = true;
        player.Gloves = null;
    }
    void RemoveItemWaist()
    {
        GameObject oldItem = player.Belt.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().ItemInfo.IsPickable = true;
        player.Belt = null;
    }
    void RemoveItemLegs()
    {
        GameObject oldItem = player.Legpiece.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().ItemInfo.IsPickable = true;
        player.Legpiece = null;
    }
    void RemoveItemFeet()
    {
        GameObject oldItem = player.Boots.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().ItemInfo.IsPickable = true;
        player.Boots = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Item") && !objectsInRange.Contains(other.gameObject))
        {
            objectsInRange.Add(other.gameObject);
        }
        else if (other.CompareTag("Consumable") && !objectsInRange.Contains(other.gameObject))
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInRange.Contains(other.gameObject))
            objectsInRange.Remove(other.gameObject);
    }
}
