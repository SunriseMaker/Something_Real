using UnityEngine;

public static class LayerMaskID
{
    const string LAYER_DEFAULT = "Default";
    const string LAYER_LIVING = "Living";
    const string LAYER_LIMBO = "Limbo";
    const string LAYER_VOID = "Void";
    const string LAYER_PHYSICAL_OBJECTS = "PhysicalObjects";
    const string LAYER_INTERACTABLE_OBJECTS = "InteractableObjects";

    public static int Default;
    public static int Obstacles;
    public static int Targets;
    public static int IntaractableObjects;

    static LayerMaskID()
    {
        Default = LayerMask.GetMask(LAYER_DEFAULT);
        Obstacles = LayerMask.GetMask(LAYER_DEFAULT, LAYER_PHYSICAL_OBJECTS);
        Targets = LayerMask.GetMask(LAYER_DEFAULT, LAYER_PHYSICAL_OBJECTS, LAYER_LIVING);
        IntaractableObjects = LayerMask.GetMask(LAYER_INTERACTABLE_OBJECTS);
    }
}