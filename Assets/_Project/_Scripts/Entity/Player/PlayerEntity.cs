using System;
using GoodVillageGames.Core.Character;

public class PlayerEntity : Entity
{
    // To make my life easier when trying to get the player from the managers
    public static PlayerEntity Instance { get; private set; }
    public static event Action<PlayerEntity> OnPlayerSpawned;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }

    private void OnEnable()
    {
        OnPlayerSpawned?.Invoke(this);
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
