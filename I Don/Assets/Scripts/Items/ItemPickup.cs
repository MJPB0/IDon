using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    Player player;
    Item closestItem;
    Consumable closestConsumable;
    GameUI gameUI;

    [SerializeField] GameObject currentPickupTarget;

    [Space]
    [Header("Equipment")]
    [SerializeField] Transform eq;
    [SerializeField] Transform playerUsables;
    [SerializeField] Transform items;
    [SerializeField] Transform usables;

    [Space]
    [SerializeField] List<GameObject> itemsInRange;
    [SerializeField] List<GameObject> consumablesInRange;

    private void Awake()
    {
        player = GetComponent<Player>();
        gameUI = FindObjectOfType<GameUI>();
    }

    private void Start()
    {
        itemsInRange = new List<GameObject>();

        Item[] currentEq = { player.Headpiece, player.Shoulders, player.Chestpiece, player.Belt, player.Legpiece, player.Gloves, player.Boots, player.LeftHand, player.RightHand };
        Consumable currentConsumable = player.PlayerConsumable;
        gameUI.UpdateEqUI(currentEq);
        gameUI.UpdateEquippedItemsDurabilitiesUI(currentEq);
        gameUI.UpdateConsumableUI(currentConsumable);
    }

    private void Update()
    {
        SetCurrentTarget();
        if (itemsInRange.Count > 0)
        {
            ToggleItemInfo();
        }
        else if (closestItem)
        {
            closestItem.ToggleInfoUI(false);
            closestItem = null;
        }
        if (consumablesInRange.Count > 0)
        {
            ToggleConsumableInfo();
        }
        else if (closestConsumable)
        {
            closestConsumable.ToggleInfoUI(false);
            closestConsumable = null;
        }
    }

    private void ToggleItemInfo()
    {
        Item newClosestItem = ClosestItemInRange();
        if (closestItem != newClosestItem)
        {
            if (closestItem != null)
                closestItem.ToggleInfoUI(false);
            closestItem = newClosestItem;
        }

        switch (closestItem.itemInfo.getSlot)
        {
            case Slot.HEAD:
                if (player.Headpiece)
                    closestItem.UpdateInfoUI(player.Headpiece.itemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.SHOULDERS:
                if (player.Shoulders)
                    closestItem.UpdateInfoUI(player.Shoulders.itemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.CHEST:
                if (player.Chestpiece)
                    closestItem.UpdateInfoUI(player.Chestpiece.itemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.WAIST:
                if (player.Belt)
                    closestItem.UpdateInfoUI(player.Belt.itemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.LEGS:
                if (player.Legpiece)
                    closestItem.UpdateInfoUI(player.Legpiece.itemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.HANDS:
                if (player.Gloves)
                    closestItem.UpdateInfoUI(player.Gloves.itemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.FEET:
                if (player.Boots)
                    closestItem.UpdateInfoUI(player.Boots.itemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.LEFTHAND:
                if (player.LeftHand && player.RightHand && closestItem.itemInfo.getWeaponType == WeaponType.TWOHANDED)
                    closestItem.UpdateInfoUI(player);
                else if (player.LeftHand)
                    closestItem.UpdateInfoUI(player.LeftHand.itemInfo);
                else if (player.RightHand && closestItem.itemInfo.getWeaponType == WeaponType.TWOHANDED)
                    closestItem.UpdateInfoUI(player.RightHand.itemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            case Slot.RIGHTHAND:
                if (player.RightHand)
                    closestItem.UpdateInfoUI(player.RightHand.itemInfo);
                else if (player.LeftHand.itemInfo.getWeaponType == WeaponType.TWOHANDED)
                    closestItem.UpdateInfoUI(player.LeftHand.itemInfo);
                else
                    closestItem.UpdateInfoUI();
                break;
            default:
                closestItem.UpdateInfoUI();
                break;
        }
        if (currentPickupTarget)
        {
            if (currentPickupTarget.CompareTag("Item"))
                closestItem.ToggleInfoUI(true);
            else
                closestItem.ToggleInfoUI(false);
        }
    }
    private void ToggleConsumableInfo()
    {
        Consumable newClosestConsumable = ClosestConsumableInRange();
        if (closestConsumable != newClosestConsumable)
        {
            if (closestConsumable != null)
                closestConsumable.ToggleInfoUI(false);
            closestConsumable = newClosestConsumable;
        }
        if (player.PlayerConsumable)
            closestConsumable.UpdateInfoUI(player.PlayerConsumable.itemInfo);
        else
            closestConsumable.UpdateInfoUI();
        if (currentPickupTarget)
        {
            if (currentPickupTarget.CompareTag("Consumable"))
                closestConsumable.ToggleInfoUI(true);
            else
                closestConsumable.ToggleInfoUI(false);
        }
    }
    private void SetCurrentTarget()
    {
        if (closestConsumable && !closestItem)
            currentPickupTarget = closestConsumable.gameObject;
        else if (!closestConsumable && closestItem)
            currentPickupTarget = closestItem.gameObject;
        else if (closestConsumable && closestItem)
        {
            if (Vector3.Distance(transform.position, closestConsumable.transform.position) > Vector3.Distance(transform.position, closestItem.transform.position))
                currentPickupTarget = closestItem.gameObject;
            else
                currentPickupTarget = closestConsumable.gameObject;
        }
        else
            currentPickupTarget = null;
    }
    public void CheckRange()
    {
        if (!player.CanAttack)
            return;

        if (itemsInRange.Count != 0 || consumablesInRange.Count != 0)
        {
            GameObject closestIt = null;
            GameObject closestCons = null;
            if (itemsInRange.Count != 0)
                closestIt = itemsInRange[0];
            else if (consumablesInRange.Count != 0)
                closestCons = consumablesInRange[0];
            foreach (GameObject itemGO in itemsInRange)
            {
                if (Vector3.Distance(transform.position, closestItem.transform.position) > Vector3.Distance(transform.position, itemGO.transform.position))
                {
                    closestIt = itemGO;
                }
            }
            foreach (GameObject consumableGO in consumablesInRange)
            {
                if (Vector3.Distance(transform.position, closestConsumable.transform.position) > Vector3.Distance(transform.position, consumableGO.transform.position))
                {
                    closestCons = consumableGO;
                }
            }
            if (closestCons)
                closestConsumable = closestCons.GetComponent<Consumable>();
            if (closestIt)
                closestItem = closestIt.GetComponent<Item>();
            SetCurrentTarget();
            if (currentPickupTarget.CompareTag("Item"))
                PickItemUp(currentPickupTarget);
            else if (currentPickupTarget.CompareTag("Consumable"))
                ConsumablePickup(currentPickupTarget);
        }
    }
    public Item ClosestItemInRange()
    {
        GameObject closestIt = itemsInRange[0];
        foreach (GameObject itemGO in itemsInRange)
        {
            if (Vector3.Distance(transform.position, closestIt.transform.position) > Vector3.Distance(transform.position, itemGO.transform.position))
            {
                closestIt = itemGO;
            }
        }
        return closestIt.GetComponent<Item>();
    }
    public Consumable ClosestConsumableInRange()
    {
        GameObject closestCons = consumablesInRange[0];
        foreach (GameObject consGo in consumablesInRange)
        {
            if (Vector3.Distance(transform.position, closestCons.transform.position) > Vector3.Distance(transform.position, consGo.transform.position))
            {
                closestCons = consGo;
            }
        }
        return closestCons.GetComponent<Consumable>();
    }

    void PickItemUp(GameObject closestIt)
    {
        Slot itemSlot = closestItem.GetComponent<Item>().itemInfo.getSlot;
        Item item = closestItem.GetComponent<Item>();

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
                    if ((player.RightHand && item.itemInfo.getWeaponType != WeaponType.TWOHANDED) || !player.RightHand)
                    {
                        EquipItemLeftHand(closestIt);
                    }else if (player.RightHand && item.itemInfo.getWeaponType == WeaponType.TWOHANDED)
                    {
                        RemoveItemRightHand(transform.position);
                        EquipItemLeftHand(closestIt);
                    }
                }
                else {
                    if (player.RightHand && item.itemInfo.getWeaponType == WeaponType.TWOHANDED)
                    {
                        RemoveItemRightHand(transform.position);

                        RemoveItemLeftHand(new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z));

                        EquipItemLeftHand(closestIt);
                    }
                    else if (!player.RightHand || item.itemInfo.getWeaponType != WeaponType.TWOHANDED)
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
                    if (player.LeftHand.itemInfo.getWeaponType != WeaponType.TWOHANDED && !player.RightHand)
                    {
                        EquipItemRightHand(closestIt);
                    }else if (player.LeftHand.itemInfo.getWeaponType != WeaponType.TWOHANDED && player.RightHand)
                    {
                        RemoveItemRightHand(transform.position);
                        EquipItemRightHand(closestIt);
                    }else if (player.LeftHand.itemInfo.getWeaponType == WeaponType.TWOHANDED)
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

    void EquipItemLeftHand(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.LeftHand = it;
        player.CountStats();
        it.itemInfo.isPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        itemsInRange.Remove(cItem);

        if (it.itemInfo.getWeaponType == WeaponType.TWOHANDED)
            player.ToggleColliderAOE(true);
    }
    void EquipItemRightHand(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.RightHand = it;
        player.CountStats();
        it.itemInfo.isPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        itemsInRange.Remove(cItem);
    }
    void EquipConsumable(GameObject cCons)
    {
        Consumable cons = cCons.GetComponent<Consumable>();
        player.PlayerConsumable = cons;
        cons.itemInfo.IsPickable = false;
        cCons.transform.SetParent(playerUsables);
        cCons.transform.position = new Vector3(0, 0, 0);
        cCons.SetActive(false);
        consumablesInRange.Remove(cCons);
    }

    void EquipItemHead(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Headpiece = it;
        player.CountStats();
        it.itemInfo.isPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        itemsInRange.Remove(cItem);
    }
    void EquipItemShoulder(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Shoulders = it;
        player.CountStats();
        it.itemInfo.isPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        itemsInRange.Remove(cItem);
    }
    void EquipItemChest(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Chestpiece = it;
        player.CountStats();
        it.itemInfo.isPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        itemsInRange.Remove(cItem);
    }
    void EquipItemHands(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Gloves = it;
        player.CountStats();
        it.itemInfo.isPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        itemsInRange.Remove(cItem);
    }
    void EquipItemWaist(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Belt = it;
        player.CountStats();
        it.itemInfo.isPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        itemsInRange.Remove(cItem);
    }
    void EquipItemLegs(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Legpiece = it;
        player.CountStats();
        it.itemInfo.isPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        itemsInRange.Remove(cItem);
    }
    void EquipItemFeet(GameObject cItem)
    {
        Item it = cItem.GetComponent<Item>();
        player.Boots = it;
        player.CountStats();
        it.itemInfo.isPickable = false;
        cItem.transform.SetParent(eq);
        cItem.transform.position = new Vector3(0, 0, 0);
        cItem.SetActive(false);
        itemsInRange.Remove(cItem);
    }

    void RemoveItemLeftHand(Vector3 pos)
    {
        GameObject oldItem = player.LeftHand.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = pos;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().itemInfo.isPickable = true;
        player.LeftHand = null;

        if (oldItem.GetComponent<Item>().itemInfo.getWeaponType == WeaponType.TWOHANDED)
            player.ToggleColliderAOE(false);
    }
    void RemoveItemRightHand(Vector3 pos)
    {
        GameObject oldItem = player.RightHand.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = pos;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().itemInfo.isPickable = true;
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
        oldItem.GetComponent<Item>().itemInfo.isPickable = true;
        player.Headpiece = null;
    }
    void RemoveItemShoulder()
    {
        GameObject oldItem = player.Shoulders.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().itemInfo.isPickable = true;
        player.Shoulders = null;
    }
    void RemoveItemChest()
    {
        GameObject oldItem = player.Chestpiece.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().itemInfo.isPickable = true;
        player.Chestpiece = null;
    }
    void RemoveItemHands()
    {
        GameObject oldItem = player.Gloves.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().itemInfo.isPickable = true;
        player.Gloves = null;
    }
    void RemoveItemWaist()
    {
        GameObject oldItem = player.Belt.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().itemInfo.isPickable = true;
        player.Belt = null;
    }
    void RemoveItemLegs()
    {
        GameObject oldItem = player.Legpiece.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().itemInfo.isPickable = true;
        player.Legpiece = null;
    }
    void RemoveItemFeet()
    {
        GameObject oldItem = player.Boots.gameObject;
        oldItem.transform.SetParent(items);
        oldItem.transform.position = transform.position;
        oldItem.SetActive(true);
        oldItem.GetComponent<Item>().itemInfo.isPickable = true;
        player.Boots = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Item") && !itemsInRange.Contains(other.gameObject))
        {
            itemsInRange.Add(other.gameObject);
        }
        if (other.CompareTag("Consumable") && !consumablesInRange.Contains(other.gameObject))
        {
            consumablesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (itemsInRange.Contains(other.gameObject))
        {
            itemsInRange.Remove(other.gameObject);
        }
        if (consumablesInRange.Contains(other.gameObject))
        {
            consumablesInRange.Remove(other.gameObject);
        }
    }
}
