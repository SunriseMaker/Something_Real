using UnityEngine;

public static class LayerID
{
    const string LAYER_DEFAULT = "Default";
    const string LAYER_LIVING = "Living";
    const string LAYER_LIMBO = "Limbo";
    const string LAYER_VOID = "Void";
    const string LAYER_PHYSICAL_OBJECTS = "PhysicalObjects";
    const string LAYER_INTERACTABLE_OBJECTS = "InteractableObjects";

    public static int Default;
    public static int Living;
    public static int Limbo;
    public static int Void;
    public static int PhysicalObjects;
    public static int InteractableObjects;

    static LayerID()
    {
        Default = LayerMask.NameToLayer(LAYER_DEFAULT);
        Living = LayerMask.NameToLayer(LAYER_LIVING);
        Limbo = LayerMask.NameToLayer(LAYER_LIMBO);
        Void = LayerMask.NameToLayer(LAYER_VOID);
        PhysicalObjects = LayerMask.NameToLayer(LAYER_PHYSICAL_OBJECTS);
        InteractableObjects = LayerMask.NameToLayer(LAYER_INTERACTABLE_OBJECTS);
    }
}