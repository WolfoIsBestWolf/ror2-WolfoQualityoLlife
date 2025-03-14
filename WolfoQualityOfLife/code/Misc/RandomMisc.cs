﻿using R2API;
using RoR2;
//using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQualityOfLife
{
    public class RandomMisc
    {
 
        public static GameObject LeptonDaisyTeleporterDecoration = null;
        public static GameObject GlowFlowerForPillar = null;

        public static void Start()
        {
            RandomMiscWithConfig.Start();

            FlowersForOtherHoldoutZones();
            if (WConfig.cfgLunarSeerName.Value)
            {
                LunarSeerStuff();
            }
           
            PriceTransformStuff();
            Unused();
            VoidAffix();

            bool otherMod = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("0p41.Sots_Items_Reworked");
            if (!otherMod)
            {
                //This like runs for every character in the game for a very minor benefit idk if that's really worth it.
                IL.RoR2.InteractionDriver.MyFixedUpdate += BiggerSaleStarRange;
            }
            
            GameObject LowerPricedChestsGlow = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/LowerPricedChestsGlow");

            LowerPricedChestsGlow.transform.GetChild(0).localPosition = new Vector3(0, 0.6f, 0f);
            LowerPricedChestsGlow.transform.GetChild(1).localPosition = new Vector3(0, 0.6f, 0f);
            LowerPricedChestsGlow.transform.GetChild(2).localPosition = new Vector3(0, 0.4f, 0f);
            LowerPricedChestsGlow.transform.GetChild(3).localPosition = new Vector3(0, 0.7f, 0f);
            LowerPricedChestsGlow.transform.GetChild(3).localScale = new Vector3(1, 1.4f, 1f);



            Color SuperGreen = new Color(0.3f, 1f, 0.1f, 1f);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineChance").GetComponent<ShrineChanceBehavior>().colorShrineRewardJackpot = SuperGreen;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineChance/ShrineChanceSandy Variant.prefab").WaitForCompletion().GetComponent<ShrineChanceBehavior>().colorShrineRewardJackpot = SuperGreen;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineChance/ShrineChanceSnowy Variant.prefab").WaitForCompletion().GetComponent<ShrineChanceBehavior>().colorShrineRewardJackpot = SuperGreen;

            //Make goolake ancient loft more obvious
            Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/bazaar/matBazaarSeerGolemplains.mat").WaitForCompletion().SetFloat("_Boost", 1.5f);
            Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/bazaar/matBazaarSeerGoolake.mat").WaitForCompletion().SetFloat("_Boost", 1.5f);
            Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC1/ancientloft/matBazaarSeerAncientloft.mat").WaitForCompletion().SetFloat("_Boost", 2f);
            Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/lemuriantemple/matBazaarSeerLemurianTemple.mat").WaitForCompletion().SetFloat("_Boost", 2.5f);
            Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitat/matBazaarSeerHabitat.mat").WaitForCompletion().SetFloat("_Boost", 2.5f);

            Material matChild = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/Child/matChild.mat").WaitForCompletion();
            matChild.SetFloat("_EliteBrightnessMax", 1.09f); //2.18
            matChild.SetFloat("_EliteBrightnessMin", -0.7f); //-1.4


            //Fix error spam on Captain Spawn
            On.RoR2.CaptainDefenseMatrixController.TryGrantItem += (orig, self) =>
            {
                orig(self);
                CaptainSupplyDropController supplyController = self.characterBody.GetComponent<CaptainSupplyDropController>();
                if (supplyController)
                {
                    supplyController.CallCmdSetSkillMask(3);
                    //Bonus stock from body 1 could work fine
                }
            };

            //Sorting the hidden stages in the menu, kinda dubmb but whatevs
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/artifactworld").shouldIncludeInLogbook = true;

            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/voidstage").stageOrder = 102;
            Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC1/voidraid/voidraid.asset").WaitForCompletion().stageOrder = 103;
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/arena").stageOrder = 104;
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/mysteryspace").stageOrder = 105;
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/limbo").stageOrder = 106;
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/bazaar").stageOrder = 107;
            //LegacyResourcesAPI.Load<SceneDef>("SceneDefs/artifactworld").stageOrder = 108;
            //LegacyResourcesAPI.Load<SceneDef>("SceneDefs/goldshores").stageOrder = 108;

#region LunarScavLunarBeadBead
            GivePickupsOnStart.ItemDefInfo Beads = new GivePickupsOnStart.ItemDefInfo { itemDef = LegacyResourcesAPI.Load<ItemDef>("ItemDefs/LunarTrinket"), count = 1, dontExceedCount = true };
            GivePickupsOnStart.ItemDefInfo[] ScavLunarBeadsGiver = new GivePickupsOnStart.ItemDefInfo[] { Beads };

            LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar1Master").AddComponent<GivePickupsOnStart>().itemDefInfos = ScavLunarBeadsGiver;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar2Master").AddComponent<GivePickupsOnStart>().itemDefInfos = ScavLunarBeadsGiver;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar3Master").AddComponent<GivePickupsOnStart>().itemDefInfos = ScavLunarBeadsGiver;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar4Master").AddComponent<GivePickupsOnStart>().itemDefInfos = ScavLunarBeadsGiver;
            #endregion

#region Captain Shock Beacon Radius
            GameObject CaptainShockBeaconRadius = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Captain/CaptainSupplyDrop, Hacking.prefab").WaitForCompletion().transform.GetChild(2).GetChild(0).GetChild(4).gameObject, "ShockIndicator", false);
            Material CaptainHackingBeaconIndicatorMaterial = Object.Instantiate(CaptainShockBeaconRadius.transform.GetChild(0).GetComponent<MeshRenderer>().material);
            CaptainHackingBeaconIndicatorMaterial.SetColor("_TintColor", new Color(0, 0.4f, 0.8f, 1f));
            CaptainShockBeaconRadius.transform.GetChild(0).GetComponent<MeshRenderer>().material = CaptainHackingBeaconIndicatorMaterial;
            CaptainShockBeaconRadius.transform.localScale = new Vector3(6.67f, 6.67f, 6.67f);


            GameObject shockBeacon = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Captain/CaptainSupplyDrop, Shocking.prefab").WaitForCompletion();
            shockBeacon.transform.GetChild(2).GetChild(0).gameObject.AddComponent<InstantiateGameObjectAtLocation>().objectToInstantiate = CaptainShockBeaconRadius;
            #endregion


            //UI spreading like how charging works on other characters
            On.EntityStates.VoidSurvivor.Weapon.ReadyMegaBlaster.OnEnter += (orig, self) =>
            {
                orig(self);
                self.characterBody.AddSpreadBloom(0.4f);
            };
            On.EntityStates.VoidSurvivor.Weapon.ReadyMegaBlaster.FixedUpdate += (orig, self) =>
            {
                orig(self);
                self.characterBody.SetSpreadBloom(0.2f, true);
            };


            //Makes it so both slots are shown on start IG, only gets called like once so should be completely fine but hella unnecessary
            //On.EntityStates.Toolbot.ToolbotStanceA.OnEnter += MULTEquipmentThing;


            //Sulfur Pools Diagram is Red instead of Yellow for ???
            GameObject SulfurpoolsDioramaDisplay = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/sulfurpools/SulfurpoolsDioramaDisplay.prefab").WaitForCompletion();
            MeshRenderer SPDiaramaRenderer = SulfurpoolsDioramaDisplay.transform.GetChild(2).GetComponent<MeshRenderer>();
            Material SPRingAltered = Object.Instantiate(SPDiaramaRenderer.material);
            SPRingAltered.SetTexture("_SnowTex", Addressables.LoadAssetAsync<Texture2D>(key: "RoR2/DLC1/sulfurpools/texSPGroundDIFVein.tga").WaitForCompletion());
            SPDiaramaRenderer.material = SPRingAltered;

            RoR2.ModelPanelParameters VoidStageDiorama = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/voidstage/VoidStageDiorama.prefab").WaitForCompletion().GetComponent<ModelPanelParameters>();
            VoidStageDiorama.minDistance = 20;
            VoidStageDiorama.minDistance = 240;

            //Rachis Radius is slightly wrong, noticible on high stacks 
            GameObject RachisObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/DamageZoneWard");
            RachisObject.transform.GetChild(1).GetChild(2).GetChild(1).localScale = new Vector3(2f, 2f, 2f);

            //Too small plant normally
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Plant/InterstellarDeskPlant.prefab").WaitForCompletion().transform.GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);

            //Unused like blue explosion so he doesn't use magma explosion ig, probably unused for a reason but it looks fine
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ElectricWorm/ElectricWormBody.prefab").WaitForCompletion().GetComponent<WormBodyPositions2>().blastAttackEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Junk/ElectricWorm/ElectricWormImpactExplosion.prefab").WaitForCompletion();


            //Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/bazaar/LunarInfectionSmallMesh.prefab").WaitForCompletion();

            /*On.RoR2.ShopTerminalBehavior.PreStartClient += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("Duplicator"))
                {
                    if (self.pickupIndex != PickupIndex.none && self.pickupIndex.pickupDef.isLunar)
                    {
                        GameObject LunarInfection = GameObject.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/bazaar/LunarInfectionSmallMesh.prefab").WaitForCompletion(), self.gameObject.transform,false);
                        LunarInfection.transform.localPosition = new Vector3(-1.2f, 1.6f, -0.45f);
                        LunarInfection.transform.localScale = new Vector3(0.35f, 0.6f, 0.6f);
                        LunarInfection.transform.localEulerAngles = new Vector3(345f, 330f, 270f);

                    }
                }
            }; */

            #region More Sounds
            //Add unused cool bubbly noise
            On.RoR2.ShopTerminalBehavior.PreStartClient += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("LunarCauldron,") || self.name.StartsWith("ShrineCleanse"))
                {
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                }
            };


            On.RoR2.ShopTerminalBehavior.DropPickup += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("LunarCauldron,") || self.name.StartsWith("ShrineCleanse"))
                {
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                };
            };
      

            //Unused Scav Spawn Sound
            On.EntityStates.ScavMonster.Sit.OnEnter += (orig, self) =>
            {
                orig(self);
                if (!self.outer.gameObject) { return; }
                RoR2.Util.PlaySound(EntityStates.ScavMonster.Sit.soundString, self.outer.gameObject);
            };

            //Pretty sure they added these sounds at some point but keeping it for good measure I guess
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += (orig, self) =>
            {
                orig(self);
                RoR2.Util.PlaySound("Play_item_lunar_specialReplace_apply", self.outer.gameObject);
            };

            //
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBoss/RoboBallMiniBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBuddy/RoboBallRedBuddyBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBuddy/RoboBallGreenBuddyBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
         
        
            //Sound is just too quiet
            On.RoR2.CharacterMaster.PlayExtraLifeSFX += (orig, self) =>
            {
                orig(self);

                GameObject bodyInstanceObject = self.GetBodyObject();
                if (bodyInstanceObject)
                {
                    Util.PlaySound("Play_item_proc_extraLife", bodyInstanceObject);
                    Util.PlaySound("Play_item_proc_extraLife", bodyInstanceObject);
                    Util.PlaySound("Play_item_proc_extraLife", bodyInstanceObject);
                }
            };
            #endregion
            //What the fuck is this for
            On.RoR2.ShopTerminalBehavior.UpdatePickupDisplayAndAnimations += (orig, self) =>
            {
                if (self.pickupIndex == PickupIndex.none && self.GetComponent<PurchaseInteraction>().available)
                {
                    self.hasStarted = false;
                    orig(self);
                    self.hasStarted = true;
                    return;
                }
                orig(self);
            };

            #region Sots Fixes
            On.RoR2.BurnEffectController.HandleDestroy += BurnEffectController_HandleDestroy;

            GameObject WarBondsObject = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/Items/BarrageOnBoss/PickupTreasuryDividends.prefab").WaitForCompletion();
            Material matGoldOnStageStartMetal = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/Items/BarrageOnBoss/matBarrageOnBossMetal.mat").WaitForCompletion();
            WarBondsObject.transform.GetChild(0).GetComponent<MeshRenderer>().materials = new Material[] { matGoldOnStageStartMetal, matGoldOnStageStartMetal };

            #endregion
        }
        private static void BurnEffectController_HandleDestroy(On.RoR2.BurnEffectController.orig_HandleDestroy orig, BurnEffectController self)
        {
            GameObject.Destroy(self);
        }

        private static void BiggerSaleStarRange(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Items", "LowerPricedChests"));

            if (c.TryGotoPrev(MoveType.After,
                x => x.MatchLdfld("RoR2.InteractionDriver", "currentInteractable")))
            {

                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<GameObject, InteractionDriver, GameObject>>((target, interactionDriver) =>
                {
                    if (interactionDriver.networkIdentity.hasAuthority) //Only players
                    {
                        //IS checkign for an item less intensive
                        //Or checking for every interactable in a decent radius
                        if (interactionDriver.characterBody.inventory.GetItemCount(DLC2Content.Items.LowerPricedChests) > 0)
                        {
                            if (target == null)
                            {
                                /*if (interactionDriver.interactableCheckCooldown > 0f)
                                {
                                    return interactionDriver.currentInteractable;
                                }
                                //What?
                                interactionDriver.interactableCheckCooldown = 0f;*/
                                float num = 0f;
                                float num2 = interactionDriver.interactor.maxInteractionDistance * 2f;

                                Vector3 vector = interactionDriver.inputBank.aimOrigin;
                                Vector3 vector2 = interactionDriver.inputBank.aimDirection;
                                if (interactionDriver.useAimOffset)
                                {
                                    Vector3 a = vector + vector2 * num2;
                                    vector = interactionDriver.inputBank.aimOrigin + interactionDriver.aimOffset;
                                    vector2 = (a - vector) / num2;
                                }
                                Ray originalAimRay = new Ray(vector, vector2);
                                Ray raycastRay = CameraRigController.ModifyAimRayIfApplicable(originalAimRay, interactionDriver.gameObject, out num);
                                target = interactionDriver.interactor.FindBestInteractableObject(raycastRay, num2 + num, originalAimRay.origin, num2);
                            };
                            if (target)
                            {
                                PurchaseInteraction component = target.GetComponent<PurchaseInteraction>();
                                if (component != null && component.saleStarCompatible && !interactionDriver.saleStarEffect)
                                {
                                    interactionDriver.saleStarEffect = UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/LowerPricedChestsGlow"), target.transform.position, Quaternion.identity, target.transform);
                                    return null;
                                }
                            }
                            else if (interactionDriver.saleStarEffect)
                            {
                                UnityEngine.Object.Destroy(interactionDriver.saleStarEffect);
                                return null;
                            }
                        }
                        else if (interactionDriver.saleStarEffect)
                        {
                            UnityEngine.Object.Destroy(interactionDriver.saleStarEffect);
                            return null;
                        }
                    }
                    return null;
                });

                c.TryGotoNext(MoveType.Before,
                x => x.MatchLdfld("RoR2.InteractionDriver", "saleStarEffect"),
                x => x.MatchCall("UnityEngine.Object", "Destroy"));
                c.TryGotoNext(MoveType.Before,
                x => x.MatchLdfld("RoR2.InteractionDriver", "saleStarEffect"),
                x => x.MatchCall("UnityEngine.Object", "Destroy"));
                c.TryGotoPrev(MoveType.After,
                x => x.MatchLdfld("RoR2.InteractionDriver", "saleStarEffect"));
                //Debug.Log(c);
                c.EmitDelegate<System.Func<GameObject, GameObject>>((target) =>
                {
                    return null;
                });



                Debug.Log("IL Found: Sale Star Range");
            }
            else
            {
                Debug.LogWarning("IL Failed: Sale Star Range");
            }
        }

      
           

   
        public static void VoidAffix()
        {
            EquipmentDef VoidAffix = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteVoid/EliteVoidEquipment.asset").WaitForCompletion();
            GameObject VoidAffixDisplay = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/EliteVoid/DisplayAffixVoid.prefab").WaitForCompletion(), "PickupAffixVoidW", false);
            VoidAffixDisplay.transform.GetChild(0).GetChild(1).SetAsFirstSibling();
            VoidAffixDisplay.transform.GetChild(1).localPosition = new Vector3(0f, 0.7f, 0f);
            VoidAffixDisplay.transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, -0.5f, -0.6f);
            VoidAffixDisplay.transform.GetChild(1).GetChild(0).localScale = new Vector3(1.5f, 1.5f, 1.5f);
            VoidAffixDisplay.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            VoidAffixDisplay.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
            VoidAffixDisplay.transform.GetChild(0).eulerAngles = new Vector3(310, 0, 0);
            VoidAffixDisplay.transform.GetChild(0).localScale = new Vector3(0.75f, 0.75f, 0.75f);

            ItemDisplay display = VoidAffixDisplay.GetComponent<ItemDisplay>();
            display.rendererInfos = display.rendererInfos.Remove(display.rendererInfos[4]);

            Texture2D UniqueAffixVoid = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/UniqueAffixVoid.png");
            UniqueAffixVoid.wrapMode = TextureWrapMode.Clamp;
            Sprite UniqueAffixVoidS = Sprite.Create(UniqueAffixVoid, v.rec128, v.half);

            VoidAffix.pickupIconSprite = UniqueAffixVoidS;
            VoidAffix.pickupModelPrefab = VoidAffixDisplay;
        }


        public static void Unused()
        {

            //Gold Titan using Gold Particles
            /*On.EntityStates.TitanMonster.RechargeRocks.OnEnter += (orig, self) =>
            {
                ParticleSystemRenderer temprenderer = EntityStates.TitanMonster.RechargeRocks.rockControllerPrefab.transform.GetChild(1).GetComponent<UnityEngine.ParticleSystemRenderer>();
                if (self.gameObject.name.StartsWith("TitanGoldBody(Clone)"))
                {
                    temprenderer.material = Object.Instantiate(temprenderer.material);
                    temprenderer.material.color = new Color(0.566f, 0.4352f, 0f, 1);
                }
                else
                {
                    temprenderer.material = Object.Instantiate(temprenderer.material);
                    temprenderer.material.color = new Color(0.1587f, 0.1567f, 0.1691f, 1);
                }
                orig(self);
            };*/
        }

        internal static void PriceTransformStuff()
        {
            //Price Tag Transform things
            GameObject PrefabShrineCleanse = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanse.prefab").WaitForCompletion();
            GameObject PrefabShrineCleanseSand = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSandy Variant.prefab").WaitForCompletion();
            GameObject PrefabShrineCleanseSnow = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSnowy Variant.prefab").WaitForCompletion();

            GameObject PrefabLockbox = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/TreasureCache/Lockbox.prefab").WaitForCompletion();
            GameObject PrefabLockboxVoid = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/TreasureCacheVoid/LockboxVoid.prefab").WaitForCompletion();
            GameObject PrefabVendingMachine = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VendingMachine/VendingMachine.prefab").WaitForCompletion();

            PrefabShrineCleanse.AddComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
            PrefabShrineCleanseSand.AddComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
            PrefabShrineCleanseSnow.AddComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;

            PrefabShrineCleanse.transform.GetChild(1).localPosition = new Vector3(0.0f, 0.75f, -1.65f);
            PrefabShrineCleanseSand.transform.GetChild(1).localPosition = new Vector3(0.0f, 0.75f, -1.65f);
            PrefabShrineCleanseSnow.transform.GetChild(1).localPosition = new Vector3(0.0f, 0.75f, -1.65f);

            PrefabShrineCleanse.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = PrefabShrineCleanse.transform.GetChild(1);
            PrefabShrineCleanseSand.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = PrefabShrineCleanseSand.transform.GetChild(1);
            PrefabShrineCleanseSnow.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = PrefabShrineCleanseSnow.transform.GetChild(1);

            PrefabLockbox.AddComponent<RoR2.Hologram.HologramProjector>();
            PrefabLockboxVoid.AddComponent<RoR2.Hologram.HologramProjector>();
            PrefabVendingMachine.AddComponent<RoR2.Hologram.HologramProjector>();

            On.RoR2.ChestBehavior.Awake += (orig, self) =>
            {
                orig(self);
                //Debug.Log(self.gameObject.name);
                if (self.gameObject.name.StartsWith("Lockbox"))
                {
                    GameObject LockboxHoloPivot = new GameObject("LockboxHoloPivot");
                    LockboxHoloPivot.transform.SetParent(self.transform, false);
                    LockboxHoloPivot.transform.localPosition = new Vector3(0.0f, 2f, 0f);
                    self.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = LockboxHoloPivot.transform;
                }
            };
            On.RoR2.OptionChestBehavior.Awake += (orig, self) =>
            {
                orig(self);
                //Debug.Log(self.gameObject.name);
                if (self.gameObject.name.StartsWith("LockboxVoid"))
                {
                    GameObject LockboxHoloVoidPivot = new GameObject("LockboxVoidHoloPivot");
                    LockboxHoloVoidPivot.transform.SetParent(self.transform, false);
                    LockboxHoloVoidPivot.transform.localPosition = new Vector3(0.0f, 2f, 0f);
                    self.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = LockboxHoloVoidPivot.transform;
                }
            };
            On.RoR2.VendingMachineBehavior.Awake += (orig, self) =>
            {
                orig(self);
                //Debug.Log(self.gameObject.name);
                if (self.gameObject.name.StartsWith("VendingMachine"))
                {
                    GameObject VendingMachineHoloPivot = new GameObject("VendingMachineHoloPivot");
                    VendingMachineHoloPivot.transform.SetParent(self.transform, false);
                    VendingMachineHoloPivot.transform.localPosition = new Vector3(0.0f, 4.3f, 0f);
                    self.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = VendingMachineHoloPivot.transform;
                }
            };

        }

        public static void LunarSeerStuff()
        {
            //Seer destination in Ping and Context string
            On.RoR2.SeerStationController.OnStartClient += (orig, self) =>
            {
                orig(self);
                if (self.NetworktargetSceneDefIndex != -1)
                {
                    string temp = Language.GetString(SceneCatalog.GetSceneDef((SceneIndex)self.NetworktargetSceneDefIndex).nameToken);
                    temp = temp.Replace("Hidden Realm: ", "");
                    self.gameObject.GetComponent<PurchaseInteraction>().contextToken = (Language.GetString("BAZAAR_SEER_CONTEXT") + " of " + temp);
                    self.gameObject.GetComponent<PurchaseInteraction>().displayNameToken = (Language.GetString("BAZAAR_SEER_NAME") + " (" + temp + ")");
                }
            };
            On.RoR2.SeerStationController.OnTargetSceneChanged += (orig, self, sceneDef) =>
            {
                orig(self, sceneDef);
                //Debug.LogWarning(sceneDef);
                if (sceneDef != null)
                {
                    string temp = Language.GetString(SceneCatalog.GetSceneDef((SceneIndex)self.NetworktargetSceneDefIndex).nameToken);
                    temp = temp.Replace("Hidden Realm: ", "");
                    self.gameObject.GetComponent<PurchaseInteraction>().contextToken = (Language.GetString("BAZAAR_SEER_CONTEXT") + " of " + temp);
                    self.gameObject.GetComponent<PurchaseInteraction>().displayNameToken = (Language.GetString("BAZAAR_SEER_NAME") + " (" + temp + ")");
                }
            };

            //Change Blue Portal for Gold/Void if that is the destination, completely over unnecessary but someone suggested it and its cool
            On.RoR2.SeerStationController.SetRunNextStageToTarget += (orig, self) =>
            {
                bool isbazaar = false;
                if (SceneInfo.instance && SceneInfo.instance.sceneDef.baseSceneName == "bazaar")
                {
                    isbazaar = true;
                    GameObject ShopPortal = GameObject.Find("/PortalShop");
                    SceneDef tempscenedef = SceneCatalog.GetSceneDef((SceneIndex)self.NetworktargetSceneDefIndex);
                    //Debug.LogWarning(tempscenedef);

                    if (tempscenedef && ShopPortal)
                    {
                        ShopPortal.transform.localScale = new Vector3(0.7f, 1.26f, 0.7f);
                        ShopPortal.transform.position = new Vector3(12.88f, -5.53f, -7.34f);
                        GameObject tempportal = null;
                        GenericObjectiveProvider objective = null;
                        GameObject temp = null;
                        switch (tempscenedef.baseSceneName)
                        {
                            case "goldshores":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalGoldshores/PortalGoldshores.prefab").WaitForCompletion();
                                break;
                            case "mysteryspace":
                            case "limbo":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalMS/PortalMS.prefab").WaitForCompletion();
                                break;
                            case "artifactworld":
                            case "artifactworld01":
                            case "artifactworld02":
                            case "artifactworld03":
                                ShopPortal.transform.localPosition = new Vector3(12.88f, 0f, -7.34f);
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab").WaitForCompletion();
                                break;
                            case "arena":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalArena/PortalArena.prefab").WaitForCompletion();
                                break;
                            case "voidstage":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion();
                                break;
                            case "voidraid":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion();
                                break;
                            case "lemuriantemple":
                            case "habitat":
                            case "habitatfall":
                            case "meridian":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion();
                                break;
                            /*case "helminthroost":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PM DestinationPortal.prefab").WaitForCompletion();
                                break;*/
                            default:
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalShop/PortalShop.prefab").WaitForCompletion();
                                break;
                        }
                        if (tempportal != null)
                        {
                            objective = tempportal.GetComponent<GenericObjectiveProvider>();
                            temp = Object.Instantiate(tempportal, ShopPortal.transform.position, ShopPortal.transform.rotation);
                            NetworkServer.Spawn(temp);
                            temp.GetComponent<SceneExitController>().destinationScene = tempscenedef;
                            temp.GetComponent<SceneExitController>().useRunNextStageScene = false;
                            temp.name = ShopPortal.name;
                            Object.Destroy(ShopPortal);
                        }

                    }
                }
                orig(self);
                //Debug.LogWarning("SeerStationController.SetRunNextStageToTarget");
                if (isbazaar)
                {
                    SeerStationController[] SeerStationList = Object.FindObjectsOfType(typeof(SeerStationController)) as SeerStationController[];
                    for (var i = 0; i < SeerStationList.Length; i++)
                    {
                        SeerStationList[i].gameObject.GetComponent<PurchaseInteraction>().SetAvailable(false);
                    }
                }
            };

        }



        public static void FlowersForOtherHoldoutZones()
        {

            RoR2.HoldoutZoneController TempLeptonDasiy1 = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/Teleporter1").GetComponent<RoR2.HoldoutZoneController>();
            LeptonDaisyTeleporterDecoration = TempLeptonDasiy1.healingNovaItemEffect;

            GlowFlowerForPillar = PrefabAPI.InstantiateClone(LeptonDaisyTeleporterDecoration, "GlowFlowerForPillar", false); //Special1 (Enter)


            GlowFlowerForPillar.transform.localScale = new Vector3(0.6f, 0.6f, 0.55f);
            GlowFlowerForPillar.transform.localPosition = new Vector3(0f, 0f, 1f);
            GlowFlowerForPillar.transform.localRotation = new Quaternion(0.5373f, 0.8434f, 0, 0);

            GlowFlowerForPillar.transform.GetChild(0).localPosition = new Vector3(-0.9f, -2.9f, 4.35f);
            GlowFlowerForPillar.transform.GetChild(0).localRotation = new Quaternion(-0.4991f, 0.0297f, 0.2375f, -0.8328f);
            GlowFlowerForPillar.transform.GetChild(0).localScale = new Vector3(0.4f, 0.4f, 0.4f);

            GlowFlowerForPillar.transform.GetChild(1).localPosition = new Vector3(0.9f, 2.9f, 4.3f);
            GlowFlowerForPillar.transform.GetChild(1).localRotation = new Quaternion(0.5f, 0f, 0, -0.866f);
            GlowFlowerForPillar.transform.GetChild(1).localScale = new Vector3(0.55f, 0.55f, 0.55f);

            GlowFlowerForPillar.transform.GetChild(2).localPosition = new Vector3(-2.7f, 1.3f, 4.4f);
            GlowFlowerForPillar.transform.GetChild(2).localRotation = new Quaternion(-0.1504f, -0.4924f, -0.0868f, 0.8529f);
            GlowFlowerForPillar.transform.GetChild(2).localScale = new Vector3(0.45f, 0.45f, 0.45f);

            GlowFlowerForPillar.transform.GetChild(3).localPosition = new Vector3(2.8f, -1f, 4.3f);
            GlowFlowerForPillar.transform.GetChild(3).localRotation = new Quaternion(0.1504f, 0.4924f, -0.0868f, 0.8529f);
            GlowFlowerForPillar.transform.GetChild(3).localScale = new Vector3(0.5f, 0.5f, 0.5f);


            //Debug.LogWarning(LeptonDaisyTeleporterDecoration);

            On.RoR2.HoldoutZoneController.Awake += AddFlowersToMiscHoldoutZone;

            On.RoR2.HoldoutZoneController.Start += (orig, self) =>
            {
                orig(self);
                if (self.applyHealingNova)
                {
                    if (Util.GetItemCountForTeam(TeamIndex.Player, RoR2Content.Items.TPHealingNova.itemIndex, false, true) > 0)
                    {
                        if (self.healingNovaItemEffect)
                        {
                            self.healingNovaItemEffect.SetActive(true);
                            self.healingNovaItemEffect = null;
                        }
                    }
                    else
                    {
                        if (self.healingNovaItemEffect)
                        {
                            self.healingNovaItemEffect.SetActive(false);
                        }
                    }
                }
                //Debug.LogWarning("HoldoutZoneController.Start");
            };

        }

        private static void AddFlowersToMiscHoldoutZone(On.RoR2.HoldoutZoneController.orig_Awake orig, HoldoutZoneController self)
        {
            orig(self);
            //Debug.LogWarning(self);
            if (self.applyHealingNova)
            {
                if (!self.healingNovaItemEffect)
                {
                    if (self.name.StartsWith("InfiniteTowerSafeWard"))
                    {
                        self.healingNovaItemEffect = Object.Instantiate(LeptonDaisyTeleporterDecoration, self.transform.GetChild(0).GetChild(0).GetChild(8).GetChild(0).GetChild(1).GetChild(0));
                        self.healingNovaItemEffect.transform.rotation = new Quaternion(0, -0.7071f, -0.7071f, 0);
                        self.healingNovaItemEffect.transform.localScale = new Vector3(0.1928f, 0.1928f, 0.1928f);
                        self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -0.4193f, 0);
                    }
                    else if (self.name.StartsWith("MoonBattery"))
                    {
                        self.healingNovaItemEffect = Object.Instantiate(GlowFlowerForPillar, self.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1));
                        //self.healingNovaItemEffect.transform.rotation = new Quaternion(-0.683f, -0.183f, -0.183f, 0.683f);
                        //self.healingNovaItemEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.55f);
                        //self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -2.4f, 0f);
                    }
                    else if (self.name.StartsWith("NullSafeZone"))
                    {
                        self.healingNovaItemEffect = Object.Instantiate(LeptonDaisyTeleporterDecoration, self.transform);
                        self.healingNovaItemEffect.transform.rotation = new Quaternion(-0.5649f, -0.4254f, -0.4254f, 0.5649f);
                        self.healingNovaItemEffect.transform.localScale = new Vector3(0.275f, 0.275f, 0.225f);
                        self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -0.3f, 0f);
                    }
                    else if (self.name.StartsWith("DeepVoidPortalBattery(Clone)"))
                    {
                        self.healingNovaItemEffect = self.healingNovaItemEffect = Object.Instantiate(GlowFlowerForPillar, self.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetChild(0));
                        self.healingNovaItemEffect.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
                        self.healingNovaItemEffect.transform.localScale = new Vector3(0.375f, 0.375f, 0.375f);
                        self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -0.4f, 0f);
                    }

                }
                if (Util.GetItemCountForTeam(TeamIndex.Player, RoR2Content.Items.TPHealingNova.itemIndex, false, true) > 0)
                {
                    if (self.healingNovaItemEffect)
                    {
                        self.healingNovaItemEffect.SetActive(true);
                        self.healingNovaItemEffect = null;
                    }
                }
            }
        }

        public static void MULTEquipmentThing(On.EntityStates.Toolbot.ToolbotStanceA.orig_OnEnter orig, global::EntityStates.Toolbot.ToolbotStanceA self)
        {

            if (NetworkServer.active)
            {
                Inventory tempinv = self.outer.gameObject.GetComponent<CharacterBody>().inventory;
                //Debug.LogWarning(tempinv.GetEquipmentSlotCount());
                if (tempinv.GetEquipmentSlotCount() < 2)
                {
                    SkillLocator tempSkill = self.outer.gameObject.GetComponent<SkillLocator>();
                    if (tempSkill && tempSkill.special.skillDef == tempSkill.special.defaultSkillDef)
                    {
                        if (tempinv)
                        {
                            if (tempinv.currentEquipmentIndex == EquipmentIndex.None)
                            {
                                tempinv.SetEquipment(new EquipmentState(EquipmentIndex.None, Run.FixedTimeStamp.negativeInfinity, 0), 0);
                            }
                            if (tempinv.alternateEquipmentIndex == EquipmentIndex.None)
                            {
                                tempinv.SetEquipment(new EquipmentState(EquipmentIndex.None, Run.FixedTimeStamp.negativeInfinity, 0), 1);
                            }
                            Debug.Log("MulT equipment thing");
                        }
                    }
                }
            }
            orig(self);
        }



        public class InstantiateGameObjectAtLocation : MonoBehaviour
        {
            public GameObject objectToInstantiate;

            public void Start()
            {
                Instantiate(objectToInstantiate, this.transform);
            }
        }
    }

}
