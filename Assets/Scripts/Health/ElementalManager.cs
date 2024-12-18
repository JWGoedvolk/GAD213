using SAE.Health;
using SAE.Weapons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElementalManager : MonoBehaviour
{
    public enum ElementType
    {
        Fire,
        Ice,
        Acid,
        Plasma,
        None // This is neutral to everything if it gets in
    }
    public ElementType element;
    public TMP_Dropdown changeSelector;
    public ElementalManager playerArmorElement;
    public WeaponSystem weaponSystem;

    /// <summary>
    /// This is called from the damager and so this instance's element is taken as the defense element
    /// </summary>
    /// <param name="damageType"></param>
    /// <returns></returns>
    public float CalculateDamageModifier(ElementType damageType)
    {
        ElementType defense = element;
        ElementType attack = damageType;
        if (attack == defense || defense == ElementType.None) // The same elements and none gains no bonuses
        {
            Debug.Log($"Elemental damage modifier: {1f}");
            return 1f;
        }

        // If the defending element is the element the attacking element is strong against, give a 50% damage increase (x1.5)
        if (defense == ReturnStrength(attack))
        {
            Debug.Log($"Elemental damage modifier: {1.5f}");
            return 1.5f;
        }
        else // If it isn't strong or nuetral against the defending then it must be weak to it
        {
            Debug.Log($"Elemental damage modifier: {0.5f}");
            return 0.5f;
        }
    }

    // Return what the attacking element is strong against
    public ElementType ReturnStrength(ElementType type)
    {
        switch (type)
        {
            case ElementType.Fire:   return ElementType.Ice;
            case ElementType.Ice:    return ElementType.Acid;
            case ElementType.Acid:   return ElementType.Plasma;
            case ElementType.Plasma: return ElementType.Fire;
        }
        return ElementType.None;
    }

    public void ChangeElement(int changeTo)
    {
        ElementType changeToElement = ElementType.None;

        switch (changeTo)
        {
            case 0: changeToElement = ElementType.None;   break;
            case 1: changeToElement = ElementType.Fire;   break;
            case 2: changeToElement = ElementType.Ice;    break;
            case 3: changeToElement = ElementType.Acid;   break;
            case 4: changeToElement = ElementType.Plasma; break;
            default:
                break;
        }

        if (changeSelector.value == 0)
        {
            weaponSystem.bulletElement = changeToElement;
        }
        else if (changeSelector.value == 1)
        {
            playerArmorElement.element = changeToElement;
        }
    }

    public static string ReadableElement(ElementType type)
    {
        switch (type)
        {
            case ElementType.None  : return "Neutral";
            case ElementType.Fire  : return "Fire"   ;
            case ElementType.Ice   : return "Ice"    ;
            case ElementType.Acid  : return "Acid"   ;
            case ElementType.Plasma: return "Plasma" ;
            default: return string.Empty;
        }
    }
}
