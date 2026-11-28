using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AE RID: 6318
	public class CompGrowsFleshmassTendrils : ThingComp
	{
		// Token: 0x17001B20 RID: 6944
		// (get) Token: 0x06009E30 RID: 40496 RVA: 0x0038C752 File Offset: 0x0038A952
		public CompProperties_GrowsFleshmassTendrils Props
		{
			get
			{
				return (CompProperties_GrowsFleshmassTendrils)this.props;
			}
		}

		// Token: 0x17001B21 RID: 6945
		// (get) Token: 0x06009E31 RID: 40497 RVA: 0x0038C75F File Offset: 0x0038A95F
		protected Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		// Token: 0x17001B22 RID: 6946
		// (get) Token: 0x06009E32 RID: 40498 RVA: 0x0038C76C File Offset: 0x0038A96C
		private IntVec3 Position
		{
			get
			{
				return this.parent.Position;
			}
		}

		// Token: 0x17001B23 RID: 6947
		// (get) Token: 0x06009E33 RID: 40499 RVA: 0x0038C779 File Offset: 0x0038A979
		public int GrowthPoints
		{
			get
			{
				return this.growthPoints;
			}
		}

		// Token: 0x17001B24 RID: 6948
		// (get) Token: 0x06009E34 RID: 40500 RVA: 0x0038C781 File Offset: 0x0038A981
		public IEnumerable<IntVec3> ContiguousFleshmass
		{
			get
			{
				return this.contiguousFleshmass;
			}
		}

		// Token: 0x06009E35 RID: 40501 RVA: 0x0038C78C File Offset: 0x0038A98C
		public override void PostExposeData()
		{
			Scribe_Collections.Look<FleshTendril>(ref this.tendrils, "tendrils", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.growthPoints, "growthPoints", 0, false);
			Scribe_Values.Look<int>(ref this.nextGrowth, "nextGrowth", 0, false);
			Scribe_Values.Look<int>(ref this.fleshmassUntilFleshbeastBirth, "fleshmassUntilFleshbeastBirth", 0, false);
		}

		// Token: 0x06009E36 RID: 40502 RVA: 0x0038C7E8 File Offset: 0x0038A9E8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.growthPoints = Mathf.RoundToInt(this.Props.startingGrowthPointsByThreat.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.Map)));
				foreach (IntVec3 pos in this.parent.OccupiedRect().Cells)
				{
					this.CreateTendril(pos);
				}
				if (this.Props.fleshbeastBirthThresholdRange != null)
				{
					this.fleshmassUntilFleshbeastBirth = this.Props.fleshbeastBirthThresholdRange.Value.RandomInRange;
					return;
				}
			}
			else
			{
				this.RecalculateContiguousFleshmass();
			}
		}

		// Token: 0x06009E37 RID: 40503 RVA: 0x0038C8B8 File Offset: 0x0038AAB8
		public override void CompTick()
		{
			base.CompTick();
			if (this.growthPoints > 0 && Find.TickManager.TicksGame >= this.nextGrowth)
			{
				this.Grow();
				this.growthPoints--;
				this.nextGrowth = Find.TickManager.TicksGame + CompGrowsFleshmassTendrils.GrowthIntervalTicksRange.RandomInRange;
			}
			if (this.drawDebug)
			{
				this.DrawDebug();
			}
		}

		// Token: 0x06009E38 RID: 40504 RVA: 0x0038C928 File Offset: 0x0038AB28
		protected virtual void Grow()
		{
			if (this.fleshMassWasDestroyed)
			{
				this.CheckForDetachedTendrils();
				this.fleshMassWasDestroyed = false;
			}
			IntVec3 intVec = IntVec3.Invalid;
			int num = 0;
			while (intVec == IntVec3.Invalid && num < 10)
			{
				intVec = this.FindValidGrowthPoint();
				num++;
			}
			if (intVec == IntVec3.Invalid)
			{
				return;
			}
			Thing thing = GenSpawn.Spawn(ThingDefOf.Fleshmass_Active, intVec, this.Map, WipeMode.FullRefund);
			thing.SetFaction(this.parent.Faction, null);
			thing.TryGetComp<CompFleshmass>().source = this.parent;
			this.contiguousFleshmass.Add(intVec);
			this.furthestGrownSquared = Mathf.Max(this.furthestGrownSquared, (float)intVec.DistanceToSquared(this.Position));
			EffecterDefOf.MeatExplosionTiny.Spawn(intVec, this.Map, 1f).Cleanup();
		}

		// Token: 0x06009E39 RID: 40505 RVA: 0x0038C9F8 File Offset: 0x0038ABF8
		private IntVec3 FindValidGrowthPoint()
		{
			float value = Rand.Value;
			if (value < CompGrowsFleshmassTendrils.GrowCoreChance)
			{
				return this.GrowCore();
			}
			if (value < CompGrowsFleshmassTendrils.GrowCoreChance + CompGrowsFleshmassTendrils.ThickenTendrilChance && this.contiguousFleshmass.Count > CompGrowsFleshmassTendrils.GrowthsBeforeThickening)
			{
				return this.ThickenTendril();
			}
			return this.GrowTendril();
		}

		// Token: 0x06009E3A RID: 40506 RVA: 0x0038CA48 File Offset: 0x0038AC48
		private IntVec3 GrowCore()
		{
			IntVec3 startCell = this.parent.OccupiedRect().Cells.RandomElement<IntVec3>();
			IntVec3 curPos = startCell;
			int num = 0;
			Func<IntVec3, float> <>9__0;
			while (curPos.InHorDistOf(startCell, (float)CompGrowsFleshmassTendrils.CoreRadius) && num < 1000)
			{
				IEnumerable<IntVec3> cardinalDirections = GenAdj.CardinalDirections;
				Func<IntVec3, float> weightSelector;
				if ((weightSelector = <>9__0) == null)
				{
					weightSelector = (<>9__0 = delegate(IntVec3 d)
					{
						int num2 = startCell.DistanceToSquared(curPos);
						return (float)Mathf.Max(startCell.DistanceToSquared(curPos + d) - num2, 0);
					});
				}
				IntVec3 b;
				cardinalDirections.TryRandomElementByWeight(weightSelector, out b);
				curPos += b;
				if (this.AdjacentFleshmass(curPos) == 0)
				{
					return IntVec3.Invalid;
				}
				if (this.CanPlaceFleshmass(curPos))
				{
					return curPos;
				}
				num++;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06009E3B RID: 40507 RVA: 0x0038CB20 File Offset: 0x0038AD20
		private IntVec3 GrowTendril()
		{
			if (this.tendrils.Count < CompGrowsFleshmassTendrils.MinTendrils)
			{
				for (int i = 0; i < CompGrowsFleshmassTendrils.MinTendrils - this.tendrils.Count; i++)
				{
					if (this.contiguousFleshmass.Count == 0)
					{
						this.CreateTendril(this.parent.OccupiedRect().RandomCell);
					}
					else
					{
						this.CreateTendril(this.contiguousFleshmass.RandomElement<IntVec3>());
					}
				}
			}
			FleshTendril tendril = this.tendrils.RandomElement<FleshTendril>();
			IntVec3 intVec;
			if (!GenAdj.CardinalDirections.TryRandomElementByWeight(delegate(IntVec3 dir)
			{
				if (!this.CanPlaceFleshmass(tendril.currentPos + dir))
				{
					return 0f;
				}
				if (tendril.sourceNode.DistanceToSquared(tendril.currentPos + dir) <= tendril.sourceNode.DistanceToSquared(tendril.currentPos))
				{
					return 0f;
				}
				return Mathf.Max(this.Position.DistanceTo(tendril.currentPos + dir) - this.Position.DistanceTo(tendril.currentPos), 0.5f);
			}, out intVec))
			{
				this.tendrils.Remove(tendril);
				return IntVec3.Invalid;
			}
			IntVec3 intVec2 = tendril.currentPos + intVec * 2;
			if (!intVec2.InBounds(this.Map) || this.CellContainsFleshmass(intVec2))
			{
				this.tendrils.Remove(tendril);
				return IntVec3.Invalid;
			}
			tendril.currentPos += intVec;
			tendril.length++;
			if (tendril.length > CompGrowsFleshmassTendrils.BranchDist.RandomInRange)
			{
				this.CreateBranch(tendril);
			}
			return tendril.currentPos;
		}

		// Token: 0x06009E3C RID: 40508 RVA: 0x0038CC90 File Offset: 0x0038AE90
		private IntVec3 ThickenTendril()
		{
			IntVec3 cellToThicken;
			(from cell in this.contiguousFleshmass
			where this.AdjacentFleshmass(cell) < 4
			select cell).TryRandomElementByWeight((IntVec3 cell) => this.furthestGrownSquared - (float)this.Position.DistanceToSquared(cell), out cellToThicken);
			IntVec3 b;
			if (GenAdj.AdjacentCells.TryRandomElementByWeight(delegate(IntVec3 dir)
			{
				IntVec3 cell = cellToThicken + dir;
				int num = this.AdjacentFleshmass(cell);
				if (!this.CanPlaceFleshmass(cell))
				{
					return 0f;
				}
				if (this.CellContainsFleshmass(cell))
				{
					return 0f;
				}
				if (num == 1)
				{
					return 1f;
				}
				if (num == 2)
				{
					return 10f;
				}
				if (num == 3)
				{
					return 100f;
				}
				if (num == 4)
				{
					return 1000f;
				}
				return 0f;
			}, out b))
			{
				return cellToThicken + b;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06009E3D RID: 40509 RVA: 0x0038CD05 File Offset: 0x0038AF05
		private void CreateBranch(FleshTendril tendril)
		{
			this.tendrils.Remove(tendril);
			this.CreateTendril(tendril.currentPos);
			this.CreateTendril(tendril.currentPos);
		}

		// Token: 0x06009E3E RID: 40510 RVA: 0x0038CD2C File Offset: 0x0038AF2C
		private void CreateTendril(IntVec3 pos)
		{
			this.tendrils.Add(new FleshTendril
			{
				sourceNode = pos,
				currentPos = pos
			});
		}

		// Token: 0x06009E3F RID: 40511 RVA: 0x0038CD4C File Offset: 0x0038AF4C
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			FleshmassMapComponent component = previousMap.GetComponent<FleshmassMapComponent>();
			if (component == null)
			{
				return;
			}
			component.DestroyFleshmass(60000, 1f, false, this.parent);
		}

		// Token: 0x06009E40 RID: 40512 RVA: 0x0038CD6F File Offset: 0x0038AF6F
		public override void PreSwapMap()
		{
			FleshmassMapComponent component = this.Map.GetComponent<FleshmassMapComponent>();
			if (component == null)
			{
				return;
			}
			component.DestroyFleshmass(60000, 1f, false, this.parent);
		}

		// Token: 0x06009E41 RID: 40513 RVA: 0x0038CD97 File Offset: 0x0038AF97
		public override void PostSwapMap()
		{
			this.CheckForDetachedTendrils();
		}

		// Token: 0x06009E42 RID: 40514 RVA: 0x0038CDA0 File Offset: 0x0038AFA0
		private void RecalculateContiguousFleshmass()
		{
			this.furthestGrownSquared = 0f;
			this.contiguousFleshmass.Clear();
			this.Map.floodFiller.FloodFill(this.Position, new Predicate<IntVec3>(this.CellContainsFleshmass), delegate(IntVec3 pos)
			{
				this.contiguousFleshmass.Add(pos);
			}, int.MaxValue, false, null);
			this.furthestGrownSquared = (float)this.contiguousFleshmass.Max((IntVec3 cell) => cell.DistanceToSquared(this.Position));
		}

		// Token: 0x06009E43 RID: 40515 RVA: 0x0038CE18 File Offset: 0x0038B018
		private bool CellContainsFleshmass(IntVec3 cell)
		{
			Building edifice = cell.GetEdifice(this.Map);
			return edifice != null && (edifice.def == ThingDefOf.Fleshmass_Active || edifice.def == ThingDefOf.FleshmassSpitter || edifice.def == ThingDefOf.NerveBundle || edifice.HasComp<CompGrowsFleshmassTendrils>());
		}

		// Token: 0x06009E44 RID: 40516 RVA: 0x0038CE68 File Offset: 0x0038B068
		private bool CanPlaceFleshmass(IntVec3 cell)
		{
			if (!cell.InBounds(this.Map))
			{
				return false;
			}
			if (!cell.SupportsStructureType(this.Map, ThingDefOf.Fleshmass_Active.terrainAffordanceNeeded))
			{
				return false;
			}
			foreach (Thing thing in cell.GetThingList(this.Map))
			{
				if (!(thing is Building_Door))
				{
					Building building = thing as Building;
					if ((building == null || !building.IsClearableFreeBuilding) && !(thing is Blueprint))
					{
						ThingDef def = thing.def;
						bool? flag;
						if (def == null)
						{
							flag = null;
						}
						else
						{
							BuildingProperties building2 = def.building;
							flag = ((building2 != null) ? new bool?(building2.isPowerConduit) : null);
						}
						bool? flag2 = flag;
						if (!flag2.GetValueOrDefault())
						{
							ThingDef def2 = thing.def;
							bool? flag3;
							if (def2 == null)
							{
								flag3 = null;
							}
							else
							{
								BuildingProperties building3 = def2.building;
								flag3 = ((building3 != null) ? new bool?(building3.isAttachment) : null);
							}
							flag2 = flag3;
							if (!flag2.GetValueOrDefault())
							{
								if (!GenConstruct.CanPlaceBlueprintOver(ThingDefOf.Fleshmass_Active, thing.def, null, null))
								{
									return false;
								}
								ThingDef def3 = thing.def;
								if (def3 != null && def3.preventSkyfallersLandingOn)
								{
									return false;
								}
								Pawn pawn = thing as Pawn;
								if (pawn != null && pawn.RaceProps.Humanlike)
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06009E45 RID: 40517 RVA: 0x0038CFF8 File Offset: 0x0038B1F8
		protected int AdjacentFleshmass(IntVec3 cell)
		{
			int num = 0;
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 intVec = cell + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(this.Map) && this.CellContainsFleshmass(intVec))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06009E46 RID: 40518 RVA: 0x0038D047 File Offset: 0x0038B247
		public void Notify_FleshmassDestroyed(Thing mass)
		{
			this.fleshMassWasDestroyed = true;
		}

		// Token: 0x06009E47 RID: 40519 RVA: 0x0038D050 File Offset: 0x0038B250
		public void Notify_FleshmassDestroyedByPlayer(Thing mass)
		{
			if (this.fleshmassUntilFleshbeastBirth > 0)
			{
				this.fleshmassUntilFleshbeastBirth--;
			}
			if (this.fleshmassUntilFleshbeastBirth == 0)
			{
				FleshbeastUtility.DoFleshbeastResponse(this, mass.Position);
				this.fleshmassUntilFleshbeastBirth = this.Props.fleshbeastBirthThresholdRange.Value.RandomInRange;
			}
		}

		// Token: 0x06009E48 RID: 40520 RVA: 0x0038D0A8 File Offset: 0x0038B2A8
		public void CheckForDetachedTendrils()
		{
			this.RecalculateContiguousFleshmass();
			this.tendrils.RemoveAll((FleshTendril tendril) => !this.contiguousFleshmass.Contains(tendril.currentPos));
			if (this.contiguousFleshmass.Count < 10)
			{
				foreach (IntVec3 pos in this.parent.OccupiedRect().Cells)
				{
					this.CreateTendril(pos);
				}
			}
		}

		// Token: 0x06009E49 RID: 40521 RVA: 0x0038D130 File Offset: 0x0038B330
		private void DrawDebug()
		{
			if (this.fleshMassWasDestroyed)
			{
				this.CheckForDetachedTendrils();
				this.fleshMassWasDestroyed = false;
			}
			foreach (FleshTendril fleshTendril in this.tendrils)
			{
				GenDraw.DrawFieldEdges(new List<IntVec3>
				{
					fleshTendril.currentPos
				}, Color.green, null, null, 2900);
			}
		}

		// Token: 0x06009E4A RID: 40522 RVA: 0x0038D1BC File Offset: 0x0038B3BC
		public override string CompInspectStringExtra()
		{
			if (DebugSettings.godMode)
			{
				return "DEV: Fleshmass until fleshbeast birth: " + this.fleshmassUntilFleshbeastBirth.ToString();
			}
			return "";
		}

		// Token: 0x06009E4B RID: 40523 RVA: 0x0038D1E0 File Offset: 0x0038B3E0
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompGrowsFleshmassTendrils.<CompGetGizmosExtra>d__47 <CompGetGizmosExtra>d__ = new CompGrowsFleshmassTendrils.<CompGetGizmosExtra>d__47(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005D9A RID: 23962
		private static readonly IntRange GrowthIntervalTicksRange = new IntRange(5, 15);

		// Token: 0x04005D9B RID: 23963
		private static readonly IntRange BranchDist = new IntRange(20, 50);

		// Token: 0x04005D9C RID: 23964
		[TweakValue("Fleshmass Heart", 0f, 1f)]
		private static float GrowCoreChance = 0.1f;

		// Token: 0x04005D9D RID: 23965
		[TweakValue("Fleshmass Heart", 0f, 1f)]
		private static float ThickenTendrilChance = 0.6f;

		// Token: 0x04005D9E RID: 23966
		[TweakValue("Fleshmass Heart", 0f, 300f)]
		private static int GrowthsBeforeThickening = 100;

		// Token: 0x04005D9F RID: 23967
		[TweakValue("Fleshmass Heart", 0f, 20f)]
		private static int CoreRadius = 15;

		// Token: 0x04005DA0 RID: 23968
		[TweakValue("Fleshmass Heart", 0f, 10f)]
		private static int MinTendrils = 3;

		// Token: 0x04005DA1 RID: 23969
		private List<FleshTendril> tendrils = new List<FleshTendril>();

		// Token: 0x04005DA2 RID: 23970
		protected int growthPoints;

		// Token: 0x04005DA3 RID: 23971
		private int nextGrowth = -99999;

		// Token: 0x04005DA4 RID: 23972
		private int fleshmassUntilFleshbeastBirth = -1;

		// Token: 0x04005DA5 RID: 23973
		protected List<IntVec3> contiguousFleshmass = new List<IntVec3>();

		// Token: 0x04005DA6 RID: 23974
		private float furthestGrownSquared;

		// Token: 0x04005DA7 RID: 23975
		private bool fleshMassWasDestroyed;

		// Token: 0x04005DA8 RID: 23976
		private bool drawDebug;
	}
}
