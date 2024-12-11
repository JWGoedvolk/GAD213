using System.Collections;
using System.Collections.Generic;
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
            return 1f;
        }

        // If the defending element is the element the attacking element is strong against, give a 50% damage increase (x1.5)
        if (defense == ReturnStrength(attack))
        {
            return 1.5f;
        }
        else // If it isn't strong or nuetral against the defending then it must be weak to it
        {
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
}
