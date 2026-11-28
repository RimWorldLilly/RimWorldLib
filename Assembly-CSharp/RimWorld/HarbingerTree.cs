using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B37 RID: 6967
	public class HarbingerTree : Plant
	{
		// Token: 0x17002044 RID: 8260
		// (get) Token: 0x0600B2BF RID: 45759 RVA: 0x003EED4A File Offset: 0x003ECF4A
		private bool ConsumableNearby
		{
			get
			{
				return this.rootTargets.Count > 0;
			}
		}

		// Token: 0x17002045 RID: 8261
		// (get) Token: 0x0600B2C0 RID: 45760 RVA: 0x003EED5A File Offset: 0x003ECF5A
		private int BloomLevel
		{
			get
			{
				return Mathf.Max(Mathf.FloorToInt(this.Growth / 0.25f - 0.001f), 0);
			}
		}

		// Token: 0x17002046 RID: 8262
		// (get) Token: 0x0600B2C1 RID: 45761 RVA: 0x003EED79 File Offset: 0x003ECF79
		public float ConsumeRadius
		{
			get
			{
				return HarbingerTree.ConsumeRadiusByGrowthCurve.Evaluate(this.Growth);
			}
		}

		// Token: 0x17002047 RID: 8263
		// (get) Token: 0x0600B2C2 RID: 45762 RVA: 0x003EED8B File Offset: 0x003ECF8B
		public override float GrowthRate
		{
			get
			{
				return base.GrowthRate * this.GrowthRateFactor_Consuming;
			}
		}

		// Token: 0x17002048 RID: 8264
		// (get) Token: 0x0600B2C3 RID: 45763 RVA: 0x003EED9A File Offset: 0x003ECF9A
		private float GrowthRateFactor_Consuming
		{
			get
			{
				if (!this.ConsumableNearby)
				{
					return 1f;
				}
				return 1.5f;
			}
		}

		// Token: 0x17002049 RID: 8265
		// (get) Token: 0x0600B2C4 RID: 45764 RVA: 0x003EEDAF File Offset: 0x003ECFAF
		private IEnumerable<IntVec3> RadialCells
		{
			get
			{
				return GenRadial.RadialCellsAround(base.Position, this.ConsumeRadius, true);
			}
		}

		// Token: 0x1700204A RID: 8266
		// (get) Token: 0x0600B2C5 RID: 45765 RVA: 0x003EEDC4 File Offset: 0x003ECFC4
		public override string GrowthRateCalcDesc
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(base.GrowthRateCalcDesc);
				if (this.GrowthRateFactor_Consuming != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("FleshConsumption".Translate()) + ": " + this.GrowthRateFactor_Consuming.ToStringPercent());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600B2C6 RID: 45766 RVA: 0x003EEE30 File Offset: 0x003ED030
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ThingWithComps>(ref this.rootTargets, "rootTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.nutrition, "nutrition", 0f, false);
			Scribe_Values.Look<float>(ref this.nutritionForNextTree, "nutritionForNextTree", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.rootTargets.RemoveAll((ThingWithComps x) => x == null || x.Destroyed);
			}
		}

		// Token: 0x0600B2C7 RID: 45767 RVA: 0x003EEEB8 File Offset: 0x003ED0B8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Harbinger tree"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			LongEventHandler.ExecuteWhenFinished(new Action(this.UpdateRoots));
			this.UpdateBloomingStage();
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.nutritionForNextTree = HarbingerTree.NutritionForNewTreeRange.RandomInRange;
			}
		}

		// Token: 0x0600B2C8 RID: 45768 RVA: 0x003EEF16 File Offset: 0x003ED116
		public override void TickLong()
		{
			base.TickLong();
			this.UpdateRoots();
			this.UpdateBloomingStage();
		}

		// Token: 0x0600B2C9 RID: 45769 RVA: 0x003EEF2C File Offset: 0x003ED12C
		private void UpdateRoots()
		{
			if (this.roots == null)
			{
				this.roots = new Dictionary<ThingWithComps, Mote>();
			}
			if (this.rootTargets == null)
			{
				this.rootTargets = new List<ThingWithComps>();
			}
			this.tmpRadialCells.Clear();
			this.tmpRadialCells.AddRange(this.RadialCells.ToList<IntVec3>());
			foreach (ThingWithComps thing in this.rootTargets)
			{
				this.TryMakeRoot(thing);
			}
			foreach (IntVec3 c in this.tmpRadialCells)
			{
				if (c.InBounds(base.Map))
				{
					this.tmpThings.Clear();
					this.tmpThings.AddRange(c.GetThingList(base.Map));
					foreach (Thing thing2 in this.tmpThings)
					{
						ThingWithComps thingWithComps = thing2 as ThingWithComps;
						CompHarbingerTreeConsumable compHarbingerTreeConsumable;
						if (thingWithComps != null && thingWithComps.TryGetComp(out compHarbingerTreeConsumable) && compHarbingerTreeConsumable.CanBeConsumed)
						{
							this.TryMakeRoot(thingWithComps);
						}
					}
				}
			}
			using (List<ThingWithComps>.Enumerator enumerator = this.rootTargets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingWithComps thingWithComps2 = enumerator.Current;
					if (thingWithComps2.Destroyed || !this.tmpRadialCells.Contains(thingWithComps2.PositionHeld))
					{
						this.deferredDestroy.Enqueue(thingWithComps2);
					}
				}
				goto IL_1D8;
			}
			IL_192:
			ThingWithComps thingWithComps3 = this.deferredDestroy.Dequeue();
			Mote mote;
			if (this.roots.TryGetValue(thingWithComps3, out mote) && mote != null)
			{
				mote.Destroy(DestroyMode.Vanish);
			}
			this.roots.Remove(thingWithComps3);
			this.rootTargets.Remove(thingWithComps3);
			IL_1D8:
			if (this.deferredDestroy.Count <= 0)
			{
				return;
			}
			goto IL_192;
		}

		// Token: 0x0600B2CA RID: 45770 RVA: 0x003EF154 File Offset: 0x003ED354
		private void TryMakeRoot(ThingWithComps thing)
		{
			if (!this.roots.ContainsKey(thing))
			{
				float exactRot = 0f;
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					exactRot = corpse.InnerPawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None);
				}
				this.roots[thing] = MoteMaker.MakeStaticMote(thing.Position.ToVector3Shifted(), base.Map, ThingDefOf.Mote_HarbingerTreeRoots, 1f, false, exactRot);
			}
			if (!this.rootTargets.Contains(thing))
			{
				this.rootTargets.Add(thing);
			}
		}

		// Token: 0x0600B2CB RID: 45771 RVA: 0x003EF1E4 File Offset: 0x003ED3E4
		public void AddNutrition(float amt)
		{
			if (amt <= 0f)
			{
				return;
			}
			this.nutrition += amt;
			if (this.nutrition >= this.nutritionForNextTree)
			{
				this.nutrition = 0f;
				this.nutritionForNextTree = HarbingerTree.NutritionForNewTreeRange.RandomInRange;
				this.SpawnNewTree();
			}
		}

		// Token: 0x0600B2CC RID: 45772 RVA: 0x003EF23C File Offset: 0x003ED43C
		private void SpawnNewTree()
		{
			IntVec3 intVec;
			if (IncidentWorker_HarbingerTreeSpawn.TryGetHarbingerTreeSpawnCell(base.Map, out intVec, new IntVec3?(base.Position)))
			{
				Plant plant = intVec.GetPlant(base.Map);
				if (plant != null)
				{
					plant.Destroy(DestroyMode.Vanish);
				}
				Plant plant2 = (Plant)ThingMaker.MakeThing(ThingDefOf.Plant_TreeHarbinger, null);
				plant2.Growth = IncidentDefOf.HarbingerTreeSpawn.treeGrowth;
				GenSpawn.Spawn(plant2, intVec, base.Map, WipeMode.Vanish);
				Messages.Message("HarbingerTreeSprouted".Translate(), plant2, MessageTypeDefOf.NeutralEvent, true);
				Find.Anomaly.Notify_HarbingerTreeSpawned();
			}
		}

		// Token: 0x0600B2CD RID: 45773 RVA: 0x003EF2D4 File Offset: 0x003ED4D4
		private void UpdateBloomingStage()
		{
			this.overrideGraphicIndex = new int?(this.BloomLevel);
			base.DirtyMapMesh(base.Map);
		}

		// Token: 0x0600B2CE RID: 45774 RVA: 0x003EF2F3 File Offset: 0x003ED4F3
		public override IEnumerable<ThingDefCountClass> GetAdditionalLeavings(DestroyMode mode)
		{
			HarbingerTree.<GetAdditionalLeavings>d__34 <GetAdditionalLeavings>d__ = new HarbingerTree.<GetAdditionalLeavings>d__34(-2);
			<GetAdditionalLeavings>d__.<>4__this = this;
			<GetAdditionalLeavings>d__.<>3__mode = mode;
			return <GetAdditionalLeavings>d__;
		}

		// Token: 0x0600B2CF RID: 45775 RVA: 0x003EF30A File Offset: 0x003ED50A
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			Find.Anomaly.Notify_HarbingerTreeDied();
		}

		// Token: 0x0600B2D0 RID: 45776 RVA: 0x003EF320 File Offset: 0x003ED520
		private void CreateCorpseStockpile()
		{
			List<HarbingerTree> selectedTrees = Find.Selector.SelectedObjects.OfType<HarbingerTree>().ToList<HarbingerTree>();
			if (base.Map.zoneManager.ZoneAt(base.Position) != null)
			{
				Zone zone = base.Map.zoneManager.ZoneAt(base.Position);
				Zone_Stockpile existing = zone as Zone_Stockpile;
				if (existing != null)
				{
					base.Map.floodFiller.FloodFill(base.Position, (IntVec3 c) => selectedTrees.Any((HarbingerTree tree) => tree.RadialCells.Contains(c)) && (this.Map.zoneManager.ZoneAt(c) == null || this.Map.zoneManager.ZoneAt(c) == existing) && Designator_ZoneAdd.IsZoneableCell(c, this.Map), delegate(IntVec3 c)
					{
						if (!existing.ContainsCell(c))
						{
							existing.AddCell(c);
						}
					}, int.MaxValue, false, null);
				}
				return;
			}
			Zone_Stockpile stockpile = new Zone_Stockpile(StorageSettingsPreset.CorpseStockpile, base.Map.zoneManager);
			stockpile.settings.filter.SetAllow(ThingCategoryDefOf.CorpsesMechanoid, false, null, null);
			stockpile.settings.filter.SetAllow(SpecialThingFilterDefOf.AllowCorpsesUnnatural, false);
			base.Map.zoneManager.RegisterZone(stockpile);
			Zone_Stockpile existingStockpile = null;
			base.Map.floodFiller.FloodFill(base.Position, delegate(IntVec3 c)
			{
				Zone_Stockpile zone_Stockpile = this.Map.zoneManager.ZoneAt(c) as Zone_Stockpile;
				if (zone_Stockpile != null)
				{
					existingStockpile = zone_Stockpile;
				}
				return selectedTrees.Any((HarbingerTree tree) => tree.RadialCells.Contains(c)) && this.Map.zoneManager.ZoneAt(c) == null && Designator_ZoneAdd.IsZoneableCell(c, this.Map);
			}, delegate(IntVec3 c)
			{
				stockpile.AddCell(c);
			}, int.MaxValue, false, null);
			if (existingStockpile != null)
			{
				List<IntVec3> list = stockpile.Cells.ToList<IntVec3>();
				stockpile.Delete();
				foreach (IntVec3 c2 in list)
				{
					existingStockpile.AddCell(c2);
				}
			}
		}

		// Token: 0x0600B2D1 RID: 45777 RVA: 0x003EF4E8 File Offset: 0x003ED6E8
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			GenDraw.DrawRadiusRing(base.Position, this.ConsumeRadius);
		}

		// Token: 0x0600B2D2 RID: 45778 RVA: 0x003EF504 File Offset: 0x003ED704
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			if (DebugSettings.godMode)
			{
				stringBuilder.AppendLine("DEV: Nutrition: " + this.nutrition.ToString("F2"));
			}
			if (this.ConsumableNearby)
			{
				stringBuilder.AppendLine("HarbingerTreeConsuming".Translate());
			}
			else
			{
				stringBuilder.AppendLine("HarbingerTreeNotConsuming".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600B2D3 RID: 45779 RVA: 0x003EF58D File Offset: 0x003ED78D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			HarbingerTree.<GetGizmos>d__39 <GetGizmos>d__ = new HarbingerTree.<GetGizmos>d__39(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B2D8 RID: 45784 RVA: 0x003EF644 File Offset: 0x003ED844
		[CompilerGenerated]
		private void <GetGizmos>g__DelayedSplatter|39_0(int ticks)
		{
			EffecterDef effecterDef = new EffecterDef();
			effecterDef.maintainTicks = ticks + 5;
			SubEffecterDef subEffecterDef = new SubEffecterDef();
			subEffecterDef.subEffecterClass = typeof(SubEffecter_GroupedChance);
			subEffecterDef.chancePerTick = 1f;
			subEffecterDef.children = new List<SubEffecterDef>(EffecterDefOf.HarbingerTreeConsume.children);
			subEffecterDef.initialDelayTicks = ticks;
			subEffecterDef.subTriggerOnSpawn = false;
			effecterDef.children = new List<SubEffecterDef>
			{
				subEffecterDef
			};
			foreach (ThingWithComps t in this.rootTargets)
			{
				effecterDef.SpawnMaintained(t, t, 1f);
			}
		}

		// Token: 0x04006720 RID: 26400
		private const float ConsumingGrowthRateFactor = 1.5f;

		// Token: 0x04006721 RID: 26401
		private const int TwistedFleshCountAtMaxGrowth = 30;

		// Token: 0x04006722 RID: 26402
		private static readonly FloatRange NutritionForNewTreeRange = new FloatRange(65f, 95f);

		// Token: 0x04006723 RID: 26403
		private static readonly CachedTexture CreateCorpseStockpileIcon = new CachedTexture("UI/Icons/CorpseStockpileZone");

		// Token: 0x04006724 RID: 26404
		private static readonly SimpleCurve ConsumeRadiusByGrowthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.15f, 3.9f),
				true
			},
			{
				new CurvePoint(0.85f, 5.9f),
				true
			}
		};

		// Token: 0x04006725 RID: 26405
		private float nutrition;

		// Token: 0x04006726 RID: 26406
		private float nutritionForNextTree;

		// Token: 0x04006727 RID: 26407
		private Dictionary<ThingWithComps, Mote> roots;

		// Token: 0x04006728 RID: 26408
		private List<ThingWithComps> rootTargets;

		// Token: 0x04006729 RID: 26409
		private Queue<ThingWithComps> deferredDestroy = new Queue<ThingWithComps>();

		// Token: 0x0400672A RID: 26410
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x0400672B RID: 26411
		private List<IntVec3> tmpRadialCells = new List<IntVec3>();
	}
}
