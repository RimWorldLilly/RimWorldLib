using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D6 RID: 6358
	[StaticConstructorOnStartup]
	public class CompLaunchable : ThingComp
	{
		// Token: 0x17001B63 RID: 7011
		// (get) Token: 0x06009F62 RID: 40802 RVA: 0x003922D6 File Offset: 0x003904D6
		public CompProperties_Launchable Props
		{
			get
			{
				return (CompProperties_Launchable)this.props;
			}
		}

		// Token: 0x17001B64 RID: 7012
		// (get) Token: 0x06009F63 RID: 40803 RVA: 0x003922E4 File Offset: 0x003904E4
		protected CompTransporter Transporter
		{
			get
			{
				CompTransporter result;
				if ((result = this.cachedCompTransporter) == null)
				{
					result = (this.cachedCompTransporter = this.parent.GetComp<CompTransporter>());
				}
				return result;
			}
		}

		// Token: 0x17001B65 RID: 7013
		// (get) Token: 0x06009F64 RID: 40804 RVA: 0x00392310 File Offset: 0x00390510
		public virtual CompRefuelable Refuelable
		{
			get
			{
				CompRefuelable result;
				if ((result = this.cachedCompRefuelable) == null)
				{
					result = (this.cachedCompRefuelable = this.parent.GetComp<CompRefuelable>());
				}
				return result;
			}
		}

		// Token: 0x17001B66 RID: 7014
		// (get) Token: 0x06009F65 RID: 40805 RVA: 0x0039233C File Offset: 0x0039053C
		private CompShuttle Shuttle
		{
			get
			{
				CompShuttle result;
				if ((result = this.cachedCompShuttle) == null)
				{
					result = (this.cachedCompShuttle = this.parent.GetComp<CompShuttle>());
				}
				return result;
			}
		}

		// Token: 0x17001B67 RID: 7015
		// (get) Token: 0x06009F66 RID: 40806 RVA: 0x00392367 File Offset: 0x00390567
		protected List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		// Token: 0x17001B68 RID: 7016
		// (get) Token: 0x06009F67 RID: 40807 RVA: 0x0039237F File Offset: 0x0039057F
		private bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnyInGroupHasAnythingLeftToLoad;
			}
		}

		// Token: 0x17001B69 RID: 7017
		// (get) Token: 0x06009F68 RID: 40808 RVA: 0x0039238C File Offset: 0x0039058C
		private Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoadInGroup;
			}
		}

		// Token: 0x17001B6A RID: 7018
		// (get) Token: 0x06009F69 RID: 40809 RVA: 0x00392399 File Offset: 0x00390599
		public virtual float FuelLevel
		{
			get
			{
				return this.Refuelable.Fuel;
			}
		}

		// Token: 0x17001B6B RID: 7019
		// (get) Token: 0x06009F6A RID: 40810 RVA: 0x003923A6 File Offset: 0x003905A6
		public virtual float MaxFuelLevel
		{
			get
			{
				return this.Refuelable.Props.fuelCapacity;
			}
		}

		// Token: 0x17001B6C RID: 7020
		// (get) Token: 0x06009F6B RID: 40811 RVA: 0x003923B8 File Offset: 0x003905B8
		public virtual bool RequiresFuelingPort
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B6D RID: 7021
		// (get) Token: 0x06009F6C RID: 40812 RVA: 0x003923BC File Offset: 0x003905BC
		private bool AnyInGroupIsUnderRoof
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (transportersInGroup[i].parent.Position.Roofed(this.parent.Map))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001B6E RID: 7022
		// (get) Token: 0x06009F6D RID: 40813 RVA: 0x00392408 File Offset: 0x00390608
		private bool AllLaunchablesInGroupHaveFuelForLaunch
		{
			get
			{
				foreach (CompTransporter compTransporter in this.TransportersInGroup)
				{
					if (compTransporter.Launchable.FuelLevel < compTransporter.Launchable.Props.minFuelCost)
					{
						return false;
					}
					if (compTransporter.Launchable.RequiresFuelingPort && !compTransporter.Launchable.Refuelable.HasFuel)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001B6F RID: 7023
		// (get) Token: 0x06009F6E RID: 40814 RVA: 0x0039249C File Offset: 0x0039069C
		private float MinFuelLevelInGroup
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				float num = 0f;
				bool flag = false;
				foreach (CompTransporter compTransporter in transportersInGroup)
				{
					float fuelLevel = compTransporter.Launchable.FuelLevel;
					if (!flag || fuelLevel < num)
					{
						num = fuelLevel;
						flag = true;
					}
				}
				if (flag)
				{
					return num;
				}
				return 0f;
			}
		}

		// Token: 0x06009F6F RID: 40815 RVA: 0x00392510 File Offset: 0x00390710
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastLaunchTick, "lastLaunchTick", 0, false);
		}

		// Token: 0x06009F70 RID: 40816 RVA: 0x0039252C File Offset: 0x0039072C
		public override void CompTick()
		{
			if (!this.Props.cooldownEndedMessage.NullOrEmpty() && this.lastLaunchTick > 0 && this.lastLaunchTick + this.Props.cooldownTicks == Find.TickManager.TicksGame)
			{
				Messages.Message(this.Props.cooldownEndedMessage.Formatted(this.parent.LabelCap), this.parent, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x06009F71 RID: 40817 RVA: 0x003925B0 File Offset: 0x003907B0
		public virtual AcceptanceReport CanLaunch(float? overrideFuelLevel = null)
		{
			if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
			{
				return "CommandLaunchGroupFailNotLoaded".Translate();
			}
			if (this.parent.Spawned && this.AnyInGroupIsUnderRoof)
			{
				return "CommandLaunchGroupFailUnderRoof".Translate();
			}
			if (overrideFuelLevel != null)
			{
				float? num = overrideFuelLevel;
				float minFuelCost = this.Props.minFuelCost;
				if (num.GetValueOrDefault() < minFuelCost & num != null)
				{
					return "CommandLaunchGroupFailNoFuel".Translate();
				}
			}
			else if (!this.AllLaunchablesInGroupHaveFuelForLaunch)
			{
				return "CommandLaunchGroupFailNoFuel".Translate();
			}
			if (this.Transporter.OverMassCapacity)
			{
				return "CommandLaunchGroupFailOverMassCapacity".Translate() + ": " + "MassUsageString".Translate(this.Transporter.MassUsage.ToString("F0"), this.Transporter.MassCapacity.ToString("F0"));
			}
			int num2 = this.Props.cooldownTicks - Find.TickManager.TicksGame + this.lastLaunchTick;
			if (this.Props.cooldownTicks > 0 && this.lastLaunchTick > 0 && num2 > 0)
			{
				return "CommandLaunchGroupCooldown".Translate() + " (" + num2.ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
			if (this.Shuttle != null)
			{
				return this.Shuttle.CanLaunch;
			}
			return true;
		}

		// Token: 0x06009F72 RID: 40818 RVA: 0x0039274C File Offset: 0x0039094C
		public int MaxLaunchDistanceEver(PlanetLayer layer)
		{
			if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
			{
				return 0;
			}
			int num = this.MaxLaunchDistanceAtFuelLevel(this.MaxFuelLevel, layer);
			if (this.Props.fixedLaunchDistanceMax >= 0)
			{
				num = Mathf.Min(num, Mathf.RoundToInt((float)this.Props.fixedLaunchDistanceMax / layer.Def.rangeDistanceFactor));
			}
			return num;
		}

		// Token: 0x06009F73 RID: 40819 RVA: 0x003927A9 File Offset: 0x003909A9
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompLaunchable.<CompGetGizmosExtra>d__36 <CompGetGizmosExtra>d__ = new CompLaunchable.<CompGetGizmosExtra>d__36(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009F74 RID: 40820 RVA: 0x003927BC File Offset: 0x003909BC
		public override string CompInspectStringExtra()
		{
			if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
			if (this.Refuelable != null && !this.Refuelable.HasFuel)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllLaunchablesInGroupHaveAnyFuel".Translate().CapitalizeFirst() + ".";
			}
			if (this.AnyInGroupHasAnythingLeftToLoad)
			{
				return "NotReadyForLaunch".Translate() + ": " + "LaunchableInGroupHasSomethingLeftToLoad".Translate().CapitalizeFirst() + ".";
			}
			if (!this.AllLaunchablesInGroupHaveFuelForLaunch)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllLaunchablesInGroupHaveAnyFuel".Translate().CapitalizeFirst() + ".";
			}
			int num = this.Props.cooldownTicks - Find.TickManager.TicksGame + this.lastLaunchTick;
			if (this.Props.cooldownTicks > 0 && this.lastLaunchTick > 0 && num > 0)
			{
				return "NotReadyForLaunch".Translate() + ": " + "CommandLaunchGroupCooldown".Translate() + " (" + num.ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
			return "ReadyForLaunch".Translate();
		}

		// Token: 0x06009F75 RID: 40821 RVA: 0x00392948 File Offset: 0x00390B48
		public void StartChoosingDestination(Action<PlanetTile, TransportersArrivalAction> launchAction, float? overrideFuelLevel = null)
		{
			PlanetTile tile = this.parent.Tile;
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(new GlobalTargetInfo(tile)), CameraJumper.MovementMode.Pan);
			Find.WorldSelector.ClearSelection();
			Find.WorldTargeter.BeginTargeting((GlobalTargetInfo t) => this.ChoseWorldTarget(t, launchAction, overrideFuelLevel), true, CompLaunchable.TargeterMouseAttachment, !CaravanShuttleUtility.IsCaravanShuttle(this.Transporter), delegate
			{
				PlanetTile planetTile;
				if (this.cachedLayer != Find.WorldSelector.SelectedLayer || this.cachedOrigin != tile)
				{
					this.cachedLayer = Find.WorldSelector.SelectedLayer;
					this.cachedOrigin = tile;
					planetTile = (this.cachedClosest = Find.WorldSelector.SelectedLayer.GetClosestTile_NewTemp(tile, false));
				}
				else
				{
					planetTile = this.cachedClosest;
				}
				int num = this.MaxLaunchDistanceEver(planetTile.Layer);
				GenDraw.DrawWorldRadiusRing(planetTile, num, CompPilotConsole.GetThrusterRadiusMat(planetTile));
				if (this.Refuelable != null)
				{
					int num2 = this.MaxLaunchDistanceAtFuelLevel(this.FuelLevel, PlanetLayer.Selected);
					if (num2 < num)
					{
						GenDraw.DrawWorldRadiusRing(planetTile, num2, CompPilotConsole.GetFuelRadiusMat(planetTile));
					}
				}
			}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, tile, this.MaxLaunchDistanceEver(this.cachedClosest.Layer), this.TransportersInGroup, launchAction, this, overrideFuelLevel), null, new PlanetTile?(tile), true);
		}

		// Token: 0x06009F76 RID: 40822 RVA: 0x003929F0 File Offset: 0x00390BF0
		private bool ChoseWorldTarget(GlobalTargetInfo target, Action<PlanetTile, TransportersArrivalAction> launchAction, float? overrideFuelLevel = null)
		{
			if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
			{
				return true;
			}
			this.cachedClosest = (this.cachedOrigin = PlanetTile.Invalid);
			this.cachedLayer = null;
			PlanetTile tile = this.parent.Tile;
			return CompLaunchable.ChoseWorldTarget(target, tile, this.TransportersInGroup, this.MaxLaunchDistanceEver(target.Tile.Layer), launchAction, this, overrideFuelLevel);
		}

		// Token: 0x06009F77 RID: 40823 RVA: 0x00392A58 File Offset: 0x00390C58
		public void TryLaunch(PlanetTile destinationTile, TransportersArrivalAction arrivalAction)
		{
			if (!this.parent.Spawned)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to launch ");
				defaultInterpolatedStringHandler.AppendFormatted<ThingWithComps>(this.parent);
				defaultInterpolatedStringHandler.AppendLiteral(", but it's unspawned.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			if (transportersInGroup == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Tried to launch ");
				defaultInterpolatedStringHandler2.AppendFormatted<ThingWithComps>(this.parent);
				defaultInterpolatedStringHandler2.AppendLiteral(", but it's not in any group.");
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				return;
			}
			if (!this.CanLaunch(null))
			{
				return;
			}
			Map map = this.parent.Map;
			int num = Find.WorldGrid.TraversalDistanceBetween(map.Tile, destinationTile, true, int.MaxValue, true);
			Current.Game.CurrentMap = map;
			if (num > this.MaxLaunchDistanceAtFuelLevel(this.MinFuelLevelInGroup, destinationTile.Layer))
			{
				return;
			}
			this.Transporter.TryRemoveLord(map);
			int groupID = this.Transporter.groupID;
			float amount = Mathf.Max(this.FuelNeededToLaunchAtDist((float)num, destinationTile.Layer), 1f);
			this.lastLaunchTick = Find.TickManager.TicksGame;
			for (int i = 0; i < transportersInGroup.Count; i++)
			{
				CompTransporter compTransporter = transportersInGroup[i];
				CompRefuelable refuelable = compTransporter.Launchable.Refuelable;
				if (refuelable != null)
				{
					refuelable.ConsumeFuel(amount);
				}
				ThingOwner directlyHeldThings = compTransporter.GetDirectlyHeldThings();
				ActiveTransporter activeTransporter = (ActiveTransporter)ThingMaker.MakeThing(this.Props.activeTransporterDef ?? ThingDefOf.ActiveDropPod, null);
				activeTransporter.Contents = new ActiveTransporterInfo();
				activeTransporter.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, true, true);
				activeTransporter.Contents.sentTransporterDef = this.parent.def;
				activeTransporter.Rotation = this.parent.Rotation;
				FlyShipLeaving flyShipLeaving = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(this.Props.skyfallerLeaving ?? ThingDefOf.DropPodLeaving, activeTransporter);
				flyShipLeaving.groupID = groupID;
				flyShipLeaving.destinationTile = destinationTile;
				flyShipLeaving.arrivalAction = arrivalAction;
				flyShipLeaving.worldObjectDef = (this.Props.worldObjectDef ?? WorldObjectDefOf.TravellingTransporters);
				if (compTransporter.parent.HasComp<CompShuttle>())
				{
					activeTransporter.Contents.SetShuttle(compTransporter.parent);
				}
				else
				{
					compTransporter.CleanUpLoadingVars(map);
					compTransporter.parent.Destroy(DestroyMode.Vanish);
				}
				GenSpawn.Spawn(flyShipLeaving, compTransporter.parent.Position, map, WipeMode.Vanish);
				INotifyLaunchableLaunch notifyLaunchableLaunch = ((refuelable != null) ? refuelable.parent : null) as INotifyLaunchableLaunch;
				if (notifyLaunchableLaunch != null)
				{
					notifyLaunchableLaunch.Notify_LaunchableLaunched(compTransporter.Launchable);
				}
			}
			CameraJumper.TryHideWorld();
		}

		// Token: 0x06009F78 RID: 40824 RVA: 0x00392D0F File Offset: 0x00390F0F
		public void Notify_FuelingPortSourceDeSpawned()
		{
			if (this.Transporter.CancelLoad())
			{
				Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06009F79 RID: 40825 RVA: 0x00392D44 File Offset: 0x00390F44
		public int MaxLaunchDistanceAtFuelLevel(float fuelLevel)
		{
			return this.MaxLaunchDistanceAtFuelLevel(fuelLevel, this.parent.Tile.Layer);
		}

		// Token: 0x06009F7A RID: 40826 RVA: 0x00392D6C File Offset: 0x00390F6C
		public int MaxLaunchDistanceAtFuelLevel(float fuelLevel, PlanetLayer layer)
		{
			if (fuelLevel < this.Props.minFuelCost)
			{
				return 0;
			}
			int num = Mathf.FloorToInt(fuelLevel / (this.Props.fuelPerTile * layer.Def.rangeDistanceFactor));
			if (num <= 0)
			{
				num = int.MaxValue;
			}
			if (this.Props.fixedLaunchDistanceMax >= 0)
			{
				num = Mathf.Min(num, Mathf.RoundToInt((float)this.Props.fixedLaunchDistanceMax / layer.Def.rangeDistanceFactor));
			}
			return num;
		}

		// Token: 0x06009F7B RID: 40827 RVA: 0x00392DE8 File Offset: 0x00390FE8
		public float FuelNeededToLaunchAtDist(float dist, PlanetLayer layer)
		{
			float num = dist * (this.Props.fuelPerTile * layer.Def.rangeDistanceFactor);
			if (this.Props.minFuelCost > 0f && num < this.Props.minFuelCost)
			{
				return this.Props.minFuelCost;
			}
			return num;
		}

		// Token: 0x06009F7C RID: 40828 RVA: 0x00392E3C File Offset: 0x0039103C
		private IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptionsAt(PlanetTile tile, Action<PlanetTile, TransportersArrivalAction> launchAction, bool isShuttle)
		{
			CompLaunchable.<GetTransportersFloatMenuOptionsAt>d__48 <GetTransportersFloatMenuOptionsAt>d__ = new CompLaunchable.<GetTransportersFloatMenuOptionsAt>d__48(-2);
			<GetTransportersFloatMenuOptionsAt>d__.<>4__this = this;
			<GetTransportersFloatMenuOptionsAt>d__.<>3__tile = tile;
			<GetTransportersFloatMenuOptionsAt>d__.<>3__launchAction = launchAction;
			<GetTransportersFloatMenuOptionsAt>d__.<>3__isShuttle = isShuttle;
			return <GetTransportersFloatMenuOptionsAt>d__;
		}

		// Token: 0x06009F7D RID: 40829 RVA: 0x00392E64 File Offset: 0x00391064
		public static bool ChoseWorldTarget(GlobalTargetInfo target, PlanetTile tile, IEnumerable<IThingHolder> pods, int maxLaunchDistance, Action<PlanetTile, TransportersArrivalAction> launchAction, CompLaunchable launchable, float? overrideFuelLevel = null)
		{
			if (!target.IsValid)
			{
				Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (target.HasWorldObject && !target.WorldObject.def.validLaunchTarget)
			{
				Messages.Message("MessageWorldObjectIsInvalid".Translate(target.WorldObject.Named("OBJECT")), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (ModsConfig.OdysseyActive && target.HasWorldObject && target.WorldObject.RequiresSignalJammerToReach)
			{
				Messages.Message("TransportPodDestinationRequiresSignalJammer".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			float fuelLevel = overrideFuelLevel ?? ((launchable != null) ? launchable.FuelLevel : -1f);
			int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, true, int.MaxValue, true);
			int? num2 = (launchable != null) ? new int?(launchable.MaxLaunchDistanceAtFuelLevel(fuelLevel, target.Tile.Layer)) : null;
			float num3 = (num2 != null) ? ((float)num2.GetValueOrDefault()) : -1f;
			float fuelRequired = (launchable != null) ? launchable.FuelNeededToLaunchAtDist((float)num, target.Tile.Layer) : -1f;
			if (maxLaunchDistance >= 0 && num > maxLaunchDistance)
			{
				Messages.Message("TransportPodDestinationBeyondMaximumRange".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (num3 >= 0f && (float)num > num3)
			{
				Messages.Message("TransportPodNotEnoughFuel".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			bool flag = ((launchable != null) ? launchable.Shuttle : null) != null;
			List<FloatMenuOption> list = ((launchable != null) ? launchable.GetTransportersFloatMenuOptionsAt(target.Tile, launchAction, flag) : CompLaunchable.GetOptionsForTile(target.Tile, pods, launchAction)).ToList<FloatMenuOption>();
			if (!list.Any<FloatMenuOption>())
			{
				if (Find.World.Impassable(target.Tile))
				{
					Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				if (flag && fuelLevel < fuelRequired * 2f)
				{
					WorldObject worldObject = target.WorldObject;
					bool flag2;
					if (worldObject == null)
					{
						flag2 = true;
					}
					else
					{
						Faction faction = worldObject.Faction;
						flag2 = !((faction != null) ? new bool?(faction.IsPlayer) : null).GetValueOrDefault();
					}
					if (flag2)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmShuttleNotEnoughFuelToReturn".Translate(fuelLevel, fuelRequired * 2f), delegate()
						{
							launchAction(target.Tile, null);
						}, false, null, WindowLayer.Dialog));
						return true;
					}
				}
				launchAction(target.Tile, null);
				return true;
			}
			else
			{
				if (flag && fuelLevel < fuelRequired * 2f)
				{
					WorldObject worldObject2 = target.WorldObject;
					bool flag3;
					if (worldObject2 == null)
					{
						flag3 = true;
					}
					else
					{
						Faction faction2 = worldObject2.Faction;
						flag3 = !((faction2 != null) ? new bool?(faction2.IsPlayer) : null).GetValueOrDefault();
					}
					if (flag3)
					{
						for (int i = 0; i < list.Count; i++)
						{
							Action oldAction = list[i].action;
							list[i].action = delegate()
							{
								Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmShuttleNotEnoughFuelToReturn".Translate(fuelLevel, fuelRequired * 2f), oldAction, false, null, WindowLayer.Dialog));
							};
						}
					}
				}
				if (list.Count != 1)
				{
					Find.WindowStack.Add(new FloatMenu(list));
					return false;
				}
				if (!list.First<FloatMenuOption>().Disabled)
				{
					list.First<FloatMenuOption>().action();
					return true;
				}
				return false;
			}
		}

		// Token: 0x06009F7E RID: 40830 RVA: 0x00393297 File Offset: 0x00391497
		public static IEnumerable<FloatMenuOption> GetOptionsForTile(PlanetTile tile, IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			CompLaunchable.<GetOptionsForTile>d__50 <GetOptionsForTile>d__ = new CompLaunchable.<GetOptionsForTile>d__50(-2);
			<GetOptionsForTile>d__.<>3__tile = tile;
			<GetOptionsForTile>d__.<>3__pods = pods;
			<GetOptionsForTile>d__.<>3__launchAction = launchAction;
			return <GetOptionsForTile>d__;
		}

		// Token: 0x06009F7F RID: 40831 RVA: 0x003932B8 File Offset: 0x003914B8
		public static TaggedString TargetingLabelGetter(GlobalTargetInfo target, PlanetTile tile, int maxLaunchDistance, IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction, CompLaunchable launchable, float? overrideFuelLevel = null)
		{
			if (!target.IsValid)
			{
				return null;
			}
			if (target.Tile.Layer != tile.Layer && !tile.Layer.HasConnectionPathTo(target.Tile.Layer))
			{
				GUI.color = ColorLibrary.RedReadable;
				return "TransportPodDestinationNoPath".Translate(target.Tile.Layer.Def.Named("LAYER"));
			}
			if (ModsConfig.OdysseyActive)
			{
				WorldObject worldObject = Find.World.worldObjects.WorldObjectAt<WorldObject>(target.Tile);
				if (worldObject != null && worldObject.RequiresSignalJammerToReach)
				{
					GUI.color = ColorLibrary.RedReadable;
					return "TransportPodDestinationRequiresSignalJammer".Translate();
				}
			}
			int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, true, maxLaunchDistance, true);
			if (maxLaunchDistance > 0 && num > maxLaunchDistance)
			{
				GUI.color = ColorLibrary.RedReadable;
				return "TransportPodDestinationBeyondMaximumRange".Translate();
			}
			bool flag = ((launchable != null) ? launchable.Shuttle : null) != null;
			string text = "";
			if (flag)
			{
				float num2 = overrideFuelLevel ?? launchable.FuelLevel;
				float num3 = launchable.FuelNeededToLaunchAtDist((float)num, target.Tile.Layer);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Cost".Translate().CapitalizeFirst());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("FuelAmount".Translate(num3, ThingDefOf.Chemfuel));
				text = defaultInterpolatedStringHandler.ToStringAndClear();
				if (num3 > num2)
				{
					string str = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler2.AppendLiteral(" (");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("TransportPodNotEnoughFuel".Translate());
					defaultInterpolatedStringHandler2.AppendLiteral(")");
					text = (str + defaultInterpolatedStringHandler2.ToStringAndClear()).Colorize(ColorLibrary.RedReadable);
				}
			}
			if (target.HasWorldObject && !target.WorldObject.def.validLaunchTarget)
			{
				return string.Empty;
			}
			IEnumerable<FloatMenuOption> source = (launchable != null) ? launchable.GetTransportersFloatMenuOptionsAt(target.Tile, launchAction, flag) : CompLaunchable.GetOptionsForTile(target.Tile, pods, launchAction);
			if (!source.Any<FloatMenuOption>())
			{
				return string.Empty;
			}
			if (source.Count<FloatMenuOption>() == 1)
			{
				if (source.First<FloatMenuOption>().Disabled)
				{
					GUI.color = ColorLibrary.RedReadable;
				}
				return source.First<FloatMenuOption>().Label + "\n" + text;
			}
			MapParent mapParent = target.WorldObject as MapParent;
			if (mapParent != null)
			{
				return "ClickToSeeAvailableOrders_WorldObject".Translate(mapParent.LabelCap) + "\n" + text;
			}
			return "ClickToSeeAvailableOrders_Empty".Translate() + "\n" + text;
		}

		// Token: 0x06009F80 RID: 40832 RVA: 0x003935A5 File Offset: 0x003917A5
		public void Notify_Arrived()
		{
			this.lastLaunchTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04005E4D RID: 24141
		public int lastLaunchTick = -1;

		// Token: 0x04005E4E RID: 24142
		private CompTransporter cachedCompTransporter;

		// Token: 0x04005E4F RID: 24143
		private CompRefuelable cachedCompRefuelable;

		// Token: 0x04005E50 RID: 24144
		private CompShuttle cachedCompShuttle;

		// Token: 0x04005E51 RID: 24145
		public static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		// Token: 0x04005E52 RID: 24146
		public static readonly Texture2D LaunchCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);

		// Token: 0x04005E53 RID: 24147
		private PlanetTile cachedClosest;

		// Token: 0x04005E54 RID: 24148
		private PlanetTile cachedOrigin;

		// Token: 0x04005E55 RID: 24149
		private PlanetLayer cachedLayer;
	}
}
