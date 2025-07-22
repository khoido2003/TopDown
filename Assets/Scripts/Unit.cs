using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;

    // private MoveAction moveAction;
    // private SpinAction spinAction;
    // private ShootAction shootAction;
    private BaseAction[] baseActionArray;
    private HealthSystem healthSystem;

    private int actionPoints = 2;
    private const int ACTION_POINT_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField]
    private bool isEnemy;

    [SerializeField]
    private Transform cameraActionPositionTransform;

    private void Awake()
    {
        // moveAction = GetComponent<MoveAction>();
        // spinAction = GetComponent<SpinAction>();
        // shootAction = GetComponent<ShootAction>();


        baseActionArray = GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            // Unit change grid position
            LevelGrid.Instance.UnitMoveGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    // Use generic to get action instead each method get each action
    public T GetAction<T>()
        where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T t)
            {
                return t;
            }
        }
        return null;
    }

    // public MoveAction GetMoveAction()
    // {
    //     return moveAction;
    // }
    //
    // public SpinAction GetSpinAction()
    // {
    //     return spinAction;
    // }
    //
    // public ShootAction GetShootAction()
    // {
    //     return shootAction;
    // }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (
            (GetIsEnemy() && !TurnSystem.Instance.GetIsPlayerTurn())
            || (!GetIsEnemy() && TurnSystem.Instance.GetIsPlayerTurn())
        )
        {
            actionPoints = ACTION_POINT_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public bool GetIsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public Transform GetCameraActionPositionTransform()
    {
        return cameraActionPositionTransform;
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }
}
