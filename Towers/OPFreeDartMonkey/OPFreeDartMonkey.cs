using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using System.Collections.Generic;
using System.Linq;
using static OPTowers.Main;

namespace OPFreeDartMonkey
{
    public class OPFreeDartMonkey : ModTower
    {
        public override string TowerSet => TowerSetType.Primary;
        public override string BaseTower => "DartMonkey";
        public override int Cost => 0;
        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;
        public override string Description => "Totally just a free dart monkey";
        public override string DisplayName => "\"Free Dart Monkey\"";
        public override string Icon => "OPFreeDartMonkey-Icon";
        public override string Portrait => "OPFreeDartMonkey-Portrait";
        public override bool DontAddToShop => !OPFreeDartMonkeyEnabled == true;
        public override ParagonMode ParagonMode => ParagonMode.Base000;
        public override void ModifyBaseTowerModel(TowerModel tower)
        {
            var attackModel = tower.GetAttackModel();
            var weapon = tower.GetWeapon();
            var projectile = weapon.projectile;
            attackModel.range += 1000;
            tower.range += 1000;
            projectile.pierce = 10;
            projectile.GetDamageModel().damage = 2140000000;
            projectile.GetBehavior<TravelStraitModel>().lifespan = 5;
            weapon.emission = new ArcEmissionModel("OPFreeDartMonkeyArcEmissionModel", 230, 0, 0, null, false);
            weapon.rate *= 0.85f;
            projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            projectile.GetDamageModel().distributeToChildren = true;
            tower.GetDescendants<FilterInvisibleModel>().ForEach(invisibleModel => invisibleModel.isActive = false);
            var fireStorm = Game.instance.model.GetTower(TowerType.WizardMonkey, 1, 2, 0).behaviors.First(a => a.name.Contains("Wall")).Cast<AttackModel>().Duplicate();
            fireStorm.weapons[0].projectile.GetBehavior<AgeModel>().lifespan = 2f;
            fireStorm.weapons[0].projectile.RemoveBehaviors<CreateEffectOnExhaustedModel>();
            fireStorm.weapons[0].projectile.GetDamageModel().damage = 2000000000;
            fireStorm.weapons[0].projectile.radius += 15;
            fireStorm.weapons[0].projectile.display = new PrefabReference(){ guidRef="" };
            fireStorm.weapons[0].Rate *= .0016f;
            tower.AddBehavior(fireStorm);
            tower.AddBehavior(new SlowBloonsZoneModel("MONKESlowBloonsZoneModel", 0, "", true, null, 0.925f, 0, true, 0, "", true, null));
        }
        public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
        {
            return towerSet.First(model => model.towerId == TowerType.DartMonkey).towerIndex + 1;
        }
    }
    public class OPFreeDartMonkeyDisplay : ModTowerDisplay<OPFreeDartMonkey>
    {
        public override string BaseDisplay => GetDisplay("DartMonkey");

        public override bool UseForTower(int[] tiers)
        {
            return tiers.Sum() == 0;
        }

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            SetMeshTexture(node, Name);
        }
    }
}