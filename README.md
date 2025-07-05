Simply at it to the depencendy folder of LabApi

### How to use
To create a Custom Role, simply do it like this:
```csharp
public class ExampleRole : CustomRoleBase
{
    public override float MaxHealth => 120;
    public override RoleTypeId Role => RoleTypeId.ClassD;
    public override Color RoleColor => Color.Orange;
    public override InventoryRoleInfo StartingInventory => new(new []
    {
        ItemType.Coin
    }, new()
    {
        [ItemType.Ammo9x19] = 30,
    });
    public override ISpawnpointHandler Spawnpoint => new RoomRoleSpawnpoint(Vector3.zero, 0, 15, 0, 0, 1, 1, RoomName.LczClassDSpawn, FacilityZone.LightContainment);
}
```  
  
If you wish the role to have a Custom Name, simply use the `ICustomNameRole` interface like so:
*This will change the Player's Custom Info, not the actual Role Name*
```csharp
public class ExampleRole : CustomRoleBase, ICustomNameRole
{
    public override float MaxHealth => 120;
    public override RoleTypeId Role => RoleTypeId.ClassD;
    public override Color RoleColor => Color.Orange;
    public override InventoryRoleInfo StartingInventory => new(new []
    {
        ItemType.Coin
    }, new()
    {
        [ItemType.Ammo9x19] = 30,
    });
    public override ISpawnpointHandler Spawnpoint => new RoomRoleSpawnpoint(Vector3.zero, 0, 15, 0, 0, 1, 1, RoomName.LczClassDSpawn, FacilityZone.LightContainment);
    public string CustomRoleName => "Class-D Fighter";
}
```
**Fyi, this would make the Player Spawn with a Coin and 30x 9x19 Ammo in the middle of the Class-D Spawn in Light Containment Zone
and would set their Custom Info to be "Class-D Fighter"**

When you wish to set the players Role to the role you created, simply use:  
`CustomRoleMananger.SetRole<ExampleRole>(Target, RoleChangeReason.RoundStart, RoleSpawnFlags.All)`  
This will set the players role to the Custom Role

You can also override the Custom Roles Team and Faction, if you want it to count as something differently  
Example:
```csharp
public override CustomTeam Team => CustomTeam.ChaosInsurgency;
public override CustomFaction Faction => CustomFaction.FoundationStaff;
```  
Changing these Variables will impact wether or not a different Role can damage the Role or not
