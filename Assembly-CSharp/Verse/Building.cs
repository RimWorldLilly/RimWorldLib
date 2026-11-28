using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200054B RID: 1355
	public class Building : ThingWithComps
	{
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06002803 RID: 10243 RVA: 0x000F71CC File Offset: 0x000F53CC
		public CompPower PowerComp
		{
			get
			{
				return base.GetComp<CompPower>();
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002804 RID: 10244 RVA: 0x000F71D4 File Offset: 0x000F53D4
		public ColorDef PaintColorDef
		{
			get
			{
				return this.paintColorDef;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002805 RID: 10245 RVA: 0x000F71DC File Offset: 0x000F53DC
		public override Color DrawColor
		{
			get
			{
				if (this.paintColorDef != null)
				{
					return this.paintColorDef.color;
				}
				return base.DrawColor;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002806 RID: 10246 RVA: 0x000F71F8 File Offset: 0x000F53F8
		public virtual bool TransmitsPowerNow
		{
			get
			{
				CompPower powerComp = this.PowerComp;
				return powerComp != null && powerComp.Props.transmitsPower;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (set) Token: 0x06002807 RID: 10247 RVA: 0x000F721C File Offset: 0x000F541C
		public override int HitPoints
		{
			set
			{
				int hitPoints = this.HitPoints;
				base.HitPoints = value;
				BuildingsDamageSectionLayerUtility.Notify_BuildingHitPointsChanged(this, hitPoints);
				if (base.Spawned)
				{
					base.Map.events.Notify_BuildingHitPointsChanged(this);
				}
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002808 RID: 10248 RVA: 0x000F7257 File Offset: 0x000F5457
		public virtual int MaxItemsInCell
		{
			get
			{
				return this.def.building.maxItemsInCell;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002809 RID: 10249 RVA: 0x000F7269 File Offset: 0x000F5469
		public bool IsClearableFreeBuilding
		{
			get
			{
				return !this.def.useHitPoints && !this.def.scatterableOnMapGen && this.def.passability == Traversability.Standable && this.GetStatValue(StatDefOf.WorkToBuild, true, -1) == 0f;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x000F72A8 File Offset: 0x000F54A8
		public virtual bool IsAirtight
		{
			get
			{
				return this.def.building.isAirtight || (this.def.building.isStuffableAirtight && base.Stuff.stuffProps.isAirtight);
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x0600280B RID: 10251 RVA: 0x000F72E5 File Offset: 0x000F54E5
		public virtual bool ExchangeVacuum
		{
			get
			{
				return !this.IsAirtight || this.def.building.alwaysExchangeVacuum;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x0600280C RID: 10252 RVA: 0x000F7301 File Offset: 0x000F5501
		protected override int MinTickIntervalRate
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000F7304 File Offset: 0x000F5504
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.canChangeTerrainOnDestroyed, "canChangeTerrainOnDestroyed", true, false);
			Scribe_Defs.Look<ColorDef>(ref this.paintColorDef, "paintColorDef");
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000F7330 File Offset: 0x000F5530
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.def.IsEdifice())
			{
				map.edificeGrid.Register(this);
				if (this.def.Fillage == FillCategory.Full)
				{
					map.terrainGrid.Drawer.SetDirty();
				}
				if (this.def.AffectsFertility)
				{
					map.fertilityGrid.Drawer.SetDirty();
				}
			}
			base.SpawnSetup(map, respawningAfterLoad);
			base.Map.listerBuildings.Add(this);
			if (this.def.coversFloor)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Terrain, true, false);
			}
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					base.Map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Buildings);
					if (this.def.blockLight)
					{
						base.Map.glowGrid.LightBlockerAdded(intVec);
					}
					if (!SnowGrid.CanCoexistWithSnow(this.def))
					{
						base.Map.snowGrid.SetDepth(intVec, 0f);
					}
					if (ModsConfig.OdysseyActive && !SandGrid.CanCoexistWithSand(this.def))
					{
						base.Map.sandGrid.SetDepth(intVec, 0f);
					}
				}
			}
			if (base.Faction == Faction.OfPlayer && this.def.building != null && this.def.building.spawnedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(this.def.building.spawnedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
			AutoHomeAreaMaker.Notify_BuildingSpawned(this);
			base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
			base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
			base.Map.listerBuldingOfDefInProximity.Notify_BuildingSpawned(this);
			base.Map.listerBuildingWithTagInProximity.Notify_BuildingSpawned(this);
			if (!this.CanBeSeenOver())
			{
				base.Map.exitMapGrid.Notify_LOSBlockerSpawned();
			}
			SmoothSurfaceDesignatorUtility.Notify_BuildingSpawned(this);
			map.events.Notify_BuildingSpawned(this);
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000F7560 File Offset: 0x000F5760
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			List<Thing> attachedBuildings = GenConstruct.GetAttachedBuildings(this);
			base.DeSpawn(mode);
			if ((!this.def.IsFrame || mode != DestroyMode.Vanish) && (!this.def.building.isNaturalRock || mode != DestroyMode.WillReplace) && mode != DestroyMode.WillReplace)
			{
				foreach (Thing thing in attachedBuildings)
				{
					if (thing.def.Minifiable)
					{
						GenSpawn.Spawn(thing.MakeMinified(DestroyMode.Vanish), thing.Position, map, WipeMode.Vanish);
					}
					else
					{
						thing.Destroy(mode);
					}
				}
			}
			if (this.def.IsEdifice())
			{
				map.edificeGrid.DeRegister(this);
				if (this.def.Fillage == FillCategory.Full)
				{
					map.terrainGrid.Drawer.SetDirty();
					if (ModsConfig.BiotechActive)
					{
						map.pollutionGrid.Drawer.SetDirty();
					}
				}
				if (this.def.AffectsFertility)
				{
					map.fertilityGrid.Drawer.SetDirty();
				}
			}
			if (mode != DestroyMode.WillReplace)
			{
				if (this.def.MakeFog)
				{
					map.fogGrid.Notify_FogBlockerRemoved(this);
				}
				if (this.def.holdsRoof)
				{
					RoofCollapseCellsFinder.Notify_RoofHolderDespawned(this, map);
				}
				if (this.def.IsSmoothable)
				{
					SmoothSurfaceDesignatorUtility.Notify_BuildingDespawned(this, map);
				}
			}
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					ulong num = MapMeshFlagDefOf.Buildings;
					if (this.def.coversFloor)
					{
						num |= MapMeshFlagDefOf.Terrain;
					}
					if (this.def.Fillage == FillCategory.Full)
					{
						num |= MapMeshFlagDefOf.Roofs;
						num |= MapMeshFlagDefOf.Snow;
						if (ModsConfig.OdysseyActive)
						{
							num |= MapMeshFlagDefOf.Sand;
						}
					}
					map.mapDrawer.MapMeshDirty(intVec, num);
					if (this.def.blockLight)
					{
						map.glowGrid.LightBlockerRemoved(intVec);
					}
				}
			}
			map.listerBuildings.Remove(this);
			map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
			map.listerArtificialBuildingsForMeditation.Notify_BuildingDeSpawned(this);
			map.listerBuldingOfDefInProximity.Notify_BuildingDeSpawned(this);
			map.listerBuildingWithTagInProximity.Notify_BuildingDeSpawned(this);
			if (this.def.building.leaveTerrain != null && Current.ProgramState == ProgramState.Playing && this.canChangeTerrainOnDestroyed)
			{
				foreach (IntVec3 c in this.OccupiedRect())
				{
					map.terrainGrid.SetTerrain(c, this.def.building.leaveTerrain);
				}
			}
			map.designationManager.Notify_BuildingDespawned(this);
			if (!this.CanBeSeenOver())
			{
				map.exitMapGrid.Notify_LOSBlockerDespawned();
			}
			if (this.def.building.hasFuelingPort)
			{
				CompLaunchable compLaunchable = FuelingPortUtility.LaunchableAt(FuelingPortUtility.GetFuelingPortCell(base.Position, base.Rotation), map);
				if (compLaunchable != null && !base.BeingTransportedOnGravship)
				{
					compLaunchable.Notify_FuelingPortSourceDeSpawned();
				}
			}
			map.events.Notify_BuildingDespawned(this);
			if (this.MaxItemsInCell >= 2)
			{
				foreach (IntVec3 intVec2 in this.OccupiedRect())
				{
					int itemCount = intVec2.GetItemCount(map);
					if (itemCount > 1 && itemCount > intVec2.GetMaxItemsAllowedInCell(map))
					{
						for (int k = 0; k < itemCount - 1; k++)
						{
							Thing firstItem = intVec2.GetFirstItem(map);
							if (firstItem == null)
							{
								break;
							}
							firstItem.DeSpawn(DestroyMode.Vanish);
							GenPlace.TryPlaceThing(firstItem, intVec2, map, ThingPlaceMode.Near, null, null, null, 1);
							if (intVec2.GetItemCount(map) <= intVec2.GetMaxItemsAllowedInCell(map))
							{
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x000F7990 File Offset: 0x000F5B90
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			SmoothableWallUtility.Notify_BuildingDestroying(this, mode);
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_BuildingLost(this, null);
			}
			base.Destroy(mode);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
			if (spawned)
			{
				if (mode == DestroyMode.Deconstruct)
				{
					SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map, false));
				}
				else if (mode == DestroyMode.KillFinalize)
				{
					this.DoDestroyEffects(map);
				}
			}
			if (spawned)
			{
				Blueprint_Storage blueprint_Storage = ThingUtility.CheckAutoRebuildOnDestroyed(this, mode, map, this.def) as Blueprint_Storage;
				if (blueprint_Storage != null)
				{
					Building_Storage building_Storage = this as Building_Storage;
					if (building_Storage != null)
					{
						blueprint_Storage.SetStorageGroup(building_Storage.storageGroup, true);
						blueprint_Storage.settings = new StorageSettings();
						blueprint_Storage.settings.CopyFrom(building_Storage.settings);
					}
				}
			}
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x000F7A58 File Offset: 0x000F5C58
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
				base.Map.listerBuildingWithTagInProximity.Notify_BuildingDeSpawned(this);
				base.Map.listerBuildings.Remove(this);
			}
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
				base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
				base.Map.listerBuildingWithTagInProximity.Notify_BuildingSpawned(this);
				base.Map.listerBuildings.Add(this);
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.PowerGrid, true, false);
				if (newFaction == Faction.OfPlayer)
				{
					AutoHomeAreaMaker.Notify_BuildingClaimed(this);
				}
			}
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x000F7B24 File Offset: 0x000F5D24
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			if (this.def.building != null && !this.def.building.canBeDamagedByAttacks)
			{
				absorbed = true;
				return;
			}
			if (base.Faction != null && base.Spawned && base.Faction != Faction.OfPlayer)
			{
				for (int i = 0; i < base.Map.lordManager.lords.Count; i++)
				{
					Lord lord = base.Map.lordManager.lords[i];
					if (lord.faction == base.Faction)
					{
						lord.Notify_BuildingDamaged(this, dinfo);
					}
				}
			}
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed && base.Faction != null)
			{
				base.Faction.Notify_BuildingTookDamage(this, dinfo);
			}
			if (!absorbed)
			{
				CompStunnable comp = base.GetComp<CompStunnable>();
				if (comp == null)
				{
					return;
				}
				comp.ApplyDamage(dinfo);
			}
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x000F7C03 File Offset: 0x000F5E03
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingTookDamage(this);
			}
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x000F7C28 File Offset: 0x000F5E28
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x000F7C56 File Offset: 0x000F5E56
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building.<GetGizmos>d__33 <GetGizmos>d__ = new Building.<GetGizmos>d__33(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000F7C66 File Offset: 0x000F5E66
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Building.<SpecialDisplayStats>d__34 <SpecialDisplayStats>d__ = new Building.<SpecialDisplayStats>d__34(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000F7C78 File Offset: 0x000F5E78
		public override AcceptanceReport ClaimableBy(Faction by)
		{
			if (!this.def.Claimable)
			{
				return false;
			}
			if (base.Faction == by)
			{
				return false;
			}
			for (int i = 0; i < base.AllComps.Count; i++)
			{
				if (base.AllComps[i].CompPreventClaimingBy(by))
				{
					return false;
				}
			}
			Faction faction;
			if ((faction = base.Faction) == null)
			{
				Map map = base.Map;
				faction = ((map != null) ? map.ParentFaction : null);
			}
			string value;
			if (base.FactionPreventsClaimingOrAdopting(faction, true, out value))
			{
				return value;
			}
			return true;
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000F7D10 File Offset: 0x000F5F10
		public virtual AcceptanceReport DeconstructibleBy(Faction faction)
		{
			for (int i = 0; i < base.AllComps.Count; i++)
			{
				if (base.AllComps[i].CompForceDeconstructable())
				{
					return true;
				}
			}
			if (!this.def.building.IsDeconstructible)
			{
				return false;
			}
			if (DebugSettings.godMode)
			{
				return true;
			}
			if (base.Faction == faction)
			{
				return true;
			}
			if (this.def.building.alwaysDeconstructible)
			{
				return true;
			}
			return this.ClaimableBy(faction);
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000F7DA4 File Offset: 0x000F5FA4
		public virtual ushort PathWalkCostFor(Pawn p)
		{
			return 0;
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000F7DA7 File Offset: 0x000F5FA7
		public virtual bool IsDangerousFor(Pawn p)
		{
			return false;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000F7DAA File Offset: 0x000F5FAA
		public virtual bool IsWorking()
		{
			return true;
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000F7DB0 File Offset: 0x000F5FB0
		private void DoDestroyEffects(Map map)
		{
			if (this.def.building.destroyEffecter != null && !base.Position.Fogged(map))
			{
				Effecter effecter = this.def.building.destroyEffecter.Spawn(base.Position, map, 1f);
				effecter.Trigger(new TargetInfo(base.Position, map, false), TargetInfo.Invalid, -1);
				effecter.Cleanup();
				return;
			}
			if (!this.def.IsEdifice())
			{
				return;
			}
			SoundDef destroySound = this.GetDestroySound();
			if (destroySound != null)
			{
				destroySound.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			foreach (IntVec3 intVec in this.OccupiedRect())
			{
				int num = this.def.building.isNaturalRock ? 1 : Rand.RangeInclusive(3, 5);
				for (int i = 0; i < num; i++)
				{
					FleckMaker.ThrowDustPuffThick(intVec.ToVector3Shifted(), map, Rand.Range(1.5f, 2f), Color.white);
				}
			}
			if (Find.CurrentMap == map)
			{
				float num2 = this.def.building.destroyShakeAmount;
				if (num2 < 0f)
				{
					num2 = Building.ShakeAmountPerAreaCurve.Evaluate((float)this.def.Size.Area);
				}
				CompLifespan compLifespan = this.TryGetComp<CompLifespan>();
				if (compLifespan == null || compLifespan.age < compLifespan.Props.lifespanTicks)
				{
					Find.CameraDriver.shaker.DoShake(num2);
				}
			}
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x000F7F58 File Offset: 0x000F6158
		private SoundDef GetDestroySound()
		{
			if (!this.def.building.destroySound.NullOrUndefined())
			{
				return this.def.building.destroySound;
			}
			StuffCategoryDef stuffCategoryDef;
			if (this.def.MadeFromStuff && base.Stuff != null && !base.Stuff.stuffProps.categories.NullOrEmpty<StuffCategoryDef>())
			{
				stuffCategoryDef = base.Stuff.stuffProps.categories[0];
			}
			else
			{
				if (this.def.CostList.NullOrEmpty<ThingDefCountClass>() || !this.def.CostList[0].thingDef.IsStuff || this.def.CostList[0].thingDef.stuffProps.categories.NullOrEmpty<StuffCategoryDef>())
				{
					return null;
				}
				stuffCategoryDef = this.def.CostList[0].thingDef.stuffProps.categories[0];
			}
			switch (this.def.building.buildingSizeCategory)
			{
			case BuildingSizeCategory.None:
			{
				int area = this.def.Size.Area;
				if (area <= 1 && !stuffCategoryDef.destroySoundSmall.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundSmall;
				}
				if (area <= 4 && !stuffCategoryDef.destroySoundMedium.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundMedium;
				}
				if (!stuffCategoryDef.destroySoundLarge.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundLarge;
				}
				break;
			}
			case BuildingSizeCategory.Small:
				if (!stuffCategoryDef.destroySoundSmall.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundSmall;
				}
				break;
			case BuildingSizeCategory.Medium:
				if (!stuffCategoryDef.destroySoundMedium.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundMedium;
				}
				break;
			case BuildingSizeCategory.Large:
				if (!stuffCategoryDef.destroySoundLarge.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundLarge;
				}
				break;
			}
			return null;
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000F811C File Offset: 0x000F631C
		public override void PostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			base.PostGeneratedForTrader(trader, forTile, forFaction);
			if (this.def.building.paintable && Rand.Value < 0.1f)
			{
				this.ChangePaint((from x in DefDatabase<ColorDef>.AllDefs
				where x.colorType == ColorType.Structure
				select x).RandomElement<ColorDef>());
				return;
			}
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x000F81AC File Offset: 0x000F63AC
		public override string GetInspectStringLowPriority()
		{
			string text = base.GetInspectStringLowPriority();
			if (!this.DeconstructibleBy(Faction.OfPlayer) && (this.def.IsNonDeconstructibleAttackableBuilding || this.def.building.quickTargetable) && this.def.building.displayAttackToDestroyOnInspectPane)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "AttackToDestroy".Translate();
			}
			return text;
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000F8230 File Offset: 0x000F6430
		public void ChangePaint(ColorDef colorDef)
		{
			this.paintColorDef = colorDef;
			this.Notify_ColorChanged();
			if (base.Spawned)
			{
				DrawerType drawerType = this.def.drawerType;
				if (drawerType == DrawerType.MapMeshOnly || drawerType == DrawerType.MapMeshAndRealTime)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Buildings);
				}
			}
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000F8286 File Offset: 0x000F6486
		public static Gizmo SelectContainedItemGizmo(Thing container, Thing item)
		{
			if (!container.Faction.IsPlayerSafe())
			{
				return null;
			}
			return ContainingSelectionUtility.SelectCarriedThingGizmo(container, item);
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000F829E File Offset: 0x000F649E
		public virtual int HaulToContainerDuration(Thing thing)
		{
			return this.def.building.haulToContainerDuration;
		}

		// Token: 0x04001C5C RID: 7260
		private ColorDef paintColorDef;

		// Token: 0x04001C5D RID: 7261
		public bool canChangeTerrainOnDestroyed = true;

		// Token: 0x04001C5E RID: 7262
		private static readonly SimpleCurve ShakeAmountPerAreaCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.07f),
				true
			},
			{
				new CurvePoint(2f, 0.07f),
				true
			},
			{
				new CurvePoint(4f, 0.1f),
				true
			},
			{
				new CurvePoint(9f, 0.2f),
				true
			},
			{
				new CurvePoint(16f, 0.5f),
				true
			}
		};

		// Token: 0x04001C5F RID: 7263
		private const float ChanceToGeneratePaintedFromTrader = 0.1f;

		// Token: 0x04001C60 RID: 7264
		private const int BuildingMinTickRate = 6;
	}
}
