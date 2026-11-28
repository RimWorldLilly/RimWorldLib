using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023AF RID: 9135
	[StaticConstructorOnStartup]
	public class MapParent : WorldObject, IThingHolder
	{
		// Token: 0x170027F3 RID: 10227
		// (get) Token: 0x0600E366 RID: 58214 RVA: 0x00549441 File Offset: 0x00547641
		public bool HasMap
		{
			get
			{
				return this.Map != null;
			}
		}

		// Token: 0x170027F4 RID: 10228
		// (get) Token: 0x0600E367 RID: 58215 RVA: 0x0054944C File Offset: 0x0054764C
		public virtual AcceptanceReport CanBeSettled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170027F5 RID: 10229
		// (get) Token: 0x0600E368 RID: 58216 RVA: 0x00549454 File Offset: 0x00547654
		protected virtual bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170027F6 RID: 10230
		// (get) Token: 0x0600E369 RID: 58217 RVA: 0x00549457 File Offset: 0x00547657
		public Map Map
		{
			get
			{
				return Current.Game.FindMap(this);
			}
		}

		// Token: 0x170027F7 RID: 10231
		// (get) Token: 0x0600E36A RID: 58218 RVA: 0x00549464 File Offset: 0x00547664
		public virtual MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return this.def.mapGenerator ?? MapGeneratorDefOf.Encounter;
			}
		}

		// Token: 0x170027F8 RID: 10232
		// (get) Token: 0x0600E36B RID: 58219 RVA: 0x0054947A File Offset: 0x0054767A
		public virtual IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				return Enumerable.Empty<GenStepWithParams>();
			}
		}

		// Token: 0x170027F9 RID: 10233
		// (get) Token: 0x0600E36C RID: 58220 RVA: 0x00549481 File Offset: 0x00547681
		public override bool ExpandMore
		{
			get
			{
				return base.ExpandMore || this.HasMap;
			}
		}

		// Token: 0x170027FA RID: 10234
		// (get) Token: 0x0600E36D RID: 58221 RVA: 0x00549493 File Offset: 0x00547693
		public virtual bool HandlesConditionCausers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170027FB RID: 10235
		// (get) Token: 0x0600E36E RID: 58222 RVA: 0x00549498 File Offset: 0x00547698
		public virtual Vector3 WorldCameraPosition
		{
			get
			{
				return base.Tile.Layer.GetTileCenter(base.Tile);
			}
		}

		// Token: 0x0600E36F RID: 58223 RVA: 0x005494C0 File Offset: 0x005476C0
		public virtual void PostMapGenerate()
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMapGenerate();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapGenerated", this.Named("SUBJECT"));
		}

		// Token: 0x0600E370 RID: 58224 RVA: 0x0054950C File Offset: 0x0054770C
		public virtual void Notify_MyMapAboutToBeRemoved()
		{
		}

		// Token: 0x0600E371 RID: 58225 RVA: 0x00549510 File Offset: 0x00547710
		public virtual void Notify_MyMapRemoved(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapRemoved();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapRemoved", this.Named("SUBJECT"));
		}

		// Token: 0x0600E372 RID: 58226 RVA: 0x0054955C File Offset: 0x0054775C
		public virtual void Notify_MyMapSettled(Map map)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostMyMapSettled();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "MapSettled", this.Named("SUBJECT"));
		}

		// Token: 0x0600E373 RID: 58227 RVA: 0x005495A8 File Offset: 0x005477A8
		public virtual void Notify_CaravanFormed(Caravan caravan)
		{
			List<WorldObjectComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].PostCaravanFormed(caravan);
			}
		}

		// Token: 0x0600E374 RID: 58228 RVA: 0x005495DA File Offset: 0x005477DA
		public virtual void Notify_HibernatableChanged()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x0600E375 RID: 58229 RVA: 0x005495E2 File Offset: 0x005477E2
		public virtual void FinalizeLoading()
		{
			this.RecalculateHibernatableIncidentTargets();
		}

		// Token: 0x0600E376 RID: 58230 RVA: 0x005495EA File Offset: 0x005477EA
		public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x0600E377 RID: 58231 RVA: 0x005495F0 File Offset: 0x005477F0
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.HasMap)
			{
				Current.Game.DeinitAndRemoveMap(this.Map, true);
			}
		}

		// Token: 0x0600E378 RID: 58232 RVA: 0x00549614 File Offset: 0x00547814
		public virtual void Abandon(bool wasGravshipLaunch)
		{
			bool flag = false;
			int count = Find.Maps.Count;
			for (int i = 0; i < count; i++)
			{
				if (Find.Maps[i].IsPlayerHome && Find.Maps[i] != this.Map)
				{
					flag = true;
					break;
				}
			}
			if (this.Map.IsPlayerHome && !flag)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					Pawn_GeneTracker genes = pawn.genes;
					if (genes != null)
					{
						genes.Notify_NewColony();
					}
					if (!pawn.BeingTransportedOnGravship)
					{
						Pawn_NeedsTracker needs = pawn.needs;
						MemoryThoughtHandler memoryThoughtHandler;
						if (needs == null)
						{
							memoryThoughtHandler = null;
						}
						else
						{
							Need_Mood mood = needs.mood;
							if (mood == null)
							{
								memoryThoughtHandler = null;
							}
							else
							{
								ThoughtHandler thoughts = mood.thoughts;
								memoryThoughtHandler = ((thoughts != null) ? thoughts.memories : null);
							}
						}
						MemoryThoughtHandler memoryThoughtHandler2 = memoryThoughtHandler;
						if (memoryThoughtHandler2 != null)
						{
							memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyOptimism);
							memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyHope);
							if (pawn.IsFreeNonSlaveColonist)
							{
								memoryThoughtHandler2.TryGainMemory(ThoughtDefOf.NewColonyOptimism, null, null);
							}
						}
					}
				}
			}
			this.Destroy();
			if (wasGravshipLaunch && base.Tile.LayerDef == PlanetLayerDefOf.Surface)
			{
				WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.GravshipLaunch);
				worldObject.Tile = base.Tile;
				worldObject.SetFaction(base.Faction);
				Find.WorldObjects.Add(worldObject);
			}
		}

		// Token: 0x0600E379 RID: 58233 RVA: 0x0054978C File Offset: 0x0054798C
		public override void Destroy()
		{
			if (this.Map != null)
			{
				for (int i = this.Map.listerThings.AllThings.Count - 1; i >= 0; i--)
				{
					this.Map.listerThings.AllThings[i].Notify_LeftBehind();
				}
			}
			base.Destroy();
		}

		// Token: 0x0600E37A RID: 58234 RVA: 0x005497E4 File Offset: 0x005479E4
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.CheckRemoveMapNow();
		}

		// Token: 0x0600E37B RID: 58235 RVA: 0x005497F3 File Offset: 0x005479F3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forceRemoveWorldObjectWhenMapRemoved, "forceRemoveWorldObjectWhenMapRemoved", false, false);
			Scribe_Values.Look<bool>(ref this.doorsAlwaysOpenForPlayerPawns, "doorsAlwaysOpenForPlayerPawns", false, false);
		}

		// Token: 0x0600E37C RID: 58236 RVA: 0x0054981F File Offset: 0x00547A1F
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent.<GetGizmos>d__35 <GetGizmos>d__ = new MapParent.<GetGizmos>d__35(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600E37D RID: 58237 RVA: 0x0054982F File Offset: 0x00547A2F
		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			MapParent.<IncidentTargetTags>d__36 <IncidentTargetTags>d__ = new MapParent.<IncidentTargetTags>d__36(-2);
			<IncidentTargetTags>d__.<>4__this = this;
			return <IncidentTargetTags>d__;
		}

		// Token: 0x0600E37E RID: 58238 RVA: 0x0054983F File Offset: 0x00547A3F
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			MapParent.<GetFloatMenuOptions>d__37 <GetFloatMenuOptions>d__ = new MapParent.<GetFloatMenuOptions>d__37(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__caravan = caravan;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600E37F RID: 58239 RVA: 0x00549856 File Offset: 0x00547A56
		public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			MapParent.<GetTransportersFloatMenuOptions>d__38 <GetTransportersFloatMenuOptions>d__ = new MapParent.<GetTransportersFloatMenuOptions>d__38(-2);
			<GetTransportersFloatMenuOptions>d__.<>4__this = this;
			<GetTransportersFloatMenuOptions>d__.<>3__pods = pods;
			<GetTransportersFloatMenuOptions>d__.<>3__launchAction = launchAction;
			return <GetTransportersFloatMenuOptions>d__;
		}

		// Token: 0x0600E380 RID: 58240 RVA: 0x00549874 File Offset: 0x00547A74
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			MapParent.<GetShuttleFloatMenuOptions>d__40 <GetShuttleFloatMenuOptions>d__ = new MapParent.<GetShuttleFloatMenuOptions>d__40(-2);
			<GetShuttleFloatMenuOptions>d__.<>4__this = this;
			<GetShuttleFloatMenuOptions>d__.<>3__pods = pods;
			<GetShuttleFloatMenuOptions>d__.<>3__launchAction = launchAction;
			return <GetShuttleFloatMenuOptions>d__;
		}

		// Token: 0x0600E381 RID: 58241 RVA: 0x00549894 File Offset: 0x00547A94
		public void CheckRemoveMapNow()
		{
			bool flag;
			if (this.HasMap && this.ShouldRemoveMapNow(out flag))
			{
				Map map = this.Map;
				Current.Game.DeinitAndRemoveMap(map, true);
				if (!base.Destroyed && (flag || this.forceRemoveWorldObjectWhenMapRemoved))
				{
					this.Destroy();
				}
			}
		}

		// Token: 0x0600E382 RID: 58242 RVA: 0x005498E0 File Offset: 0x00547AE0
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			string t;
			if (this.HasMap && GravshipUtility.TryGetNameOfGravshipOnMap(this.Map, out t))
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "GravshipOnTileInspectString".Translate().CapitalizeFirst() + ": " + t;
			}
			if (this.EnterCooldownBlocksEntering())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "EnterCooldown".Translate(this.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false));
			}
			if (!this.HandlesConditionCausers && this.HasMap)
			{
				List<Thing> list = this.Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser);
				for (int i = 0; i < list.Count; i++)
				{
					text += "\n" + list[i].LabelShortCap + " (" + "ConditionCauserRadius".Translate(list[i].TryGetComp<CompCauseGameCondition>().Props.worldRange) + ")";
				}
			}
			return text;
		}

		// Token: 0x0600E383 RID: 58243 RVA: 0x00549A30 File Offset: 0x00547C30
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (!this.HandlesConditionCausers && this.HasMap)
			{
				int num = 0;
				List<Thing> list = this.Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser);
				for (int i = 0; i < list.Count; i++)
				{
					num = Mathf.Max(num, list[i].TryGetComp<CompCauseGameCondition>().Props.worldRange);
				}
				if (num > 0)
				{
					GenDraw.DrawWorldRadiusRing(base.Tile, num, null);
				}
			}
		}

		// Token: 0x0600E384 RID: 58244 RVA: 0x00549AA7 File Offset: 0x00547CA7
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x0600E385 RID: 58245 RVA: 0x00549AAA File Offset: 0x00547CAA
		public virtual void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.HasMap)
			{
				outChildren.Add(this.Map);
			}
		}

		// Token: 0x0600E386 RID: 58246 RVA: 0x00549ACC File Offset: 0x00547CCC
		private void RecalculateHibernatableIncidentTargets()
		{
			this.hibernatableIncidentTargets = null;
			foreach (ThingWithComps thing in this.Map.listerThings.ThingsOfDef(ThingDefOf.Ship_Reactor).OfType<ThingWithComps>())
			{
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Starting && compHibernatable.Props.incidentTargetWhileStarting != null)
				{
					if (this.hibernatableIncidentTargets == null)
					{
						this.hibernatableIncidentTargets = new HashSet<IncidentTargetTagDef>();
					}
					this.hibernatableIncidentTargets.Add(compHibernatable.Props.incidentTargetWhileStarting);
				}
			}
		}

		// Token: 0x04009101 RID: 37121
		public bool forceRemoveWorldObjectWhenMapRemoved;

		// Token: 0x04009102 RID: 37122
		public bool doorsAlwaysOpenForPlayerPawns;

		// Token: 0x04009103 RID: 37123
		private HashSet<IncidentTargetTagDef> hibernatableIncidentTargets;

		// Token: 0x04009104 RID: 37124
		private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap", true);

		// Token: 0x04009105 RID: 37125
		private static Rot4 shuttleRotation = Rot4.East;
	}
}
