using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001891 RID: 6289
	public class CompFleshmassNucleus : ThingComp, IActivity, IRoofCollapseAlert
	{
		// Token: 0x17001AFB RID: 6907
		// (get) Token: 0x06009D7D RID: 40317 RVA: 0x00389BDD File Offset: 0x00387DDD
		public CompProperties_FleshmassNucleus Props
		{
			get
			{
				return (CompProperties_FleshmassNucleus)this.props;
			}
		}

		// Token: 0x17001AFC RID: 6908
		// (get) Token: 0x06009D7E RID: 40318 RVA: 0x00389BEC File Offset: 0x00387DEC
		public CompActivity Activity
		{
			get
			{
				CompActivity result;
				if ((result = this.activityComp) == null)
				{
					result = (this.activityComp = this.Pawn.TryGetComp<CompActivity>());
				}
				return result;
			}
		}

		// Token: 0x17001AFD RID: 6909
		// (get) Token: 0x06009D7F RID: 40319 RVA: 0x00389C17 File Offset: 0x00387E17
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x06009D80 RID: 40320 RVA: 0x00389C24 File Offset: 0x00387E24
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.minNextFireTick, "minNextFireTick", 0, false);
		}

		// Token: 0x06009D81 RID: 40321 RVA: 0x00389C40 File Offset: 0x00387E40
		public override void PostPostMake()
		{
			this.minNextFireTick = GenTicks.TicksGame + CompFleshmassNucleus.InitialMeatDelayRange.RandomInRange;
		}

		// Token: 0x06009D82 RID: 40322 RVA: 0x00389C68 File Offset: 0x00387E68
		public override void CompTick()
		{
			if (this.debugNextMeatTick > 0 && GenTicks.TicksGame == this.debugNextMeatTick)
			{
				this.CreateMeat();
			}
			if (this.debugActiveTick > 0 && GenTicks.TicksGame == this.debugActiveTick)
			{
				this.Activity.EnterActiveState();
			}
			if (GenTicks.TicksGame < this.minNextFireTick)
			{
				return;
			}
			if (Rand.MTBEventOccurs(1f, 60000f, 1f))
			{
				this.CreateMeat();
			}
		}

		// Token: 0x06009D83 RID: 40323 RVA: 0x00389CDC File Offset: 0x00387EDC
		public override void Notify_Downed()
		{
			this.Pawn.Kill(null, null);
		}

		// Token: 0x06009D84 RID: 40324 RVA: 0x00389D00 File Offset: 0x00387F00
		private void CreateMeat()
		{
			this.minNextFireTick = GenTicks.TicksGame + 15000;
			int num = Mathf.CeilToInt(this.Props.activityMeatPerDayCurve.Evaluate(this.Activity.ActivityLevel));
			int randomInRange = CompFleshmassNucleus.MeatClumpRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 dropCell;
				if (CellFinder.TryRandomClosewalkCellNear(this.Pawn.PositionHeld, this.Pawn.MapHeld, 3, out dropCell, null))
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Meat_Twisted, null);
					thing.stackCount = Rand.RangeInclusive(20, num - 20 * (randomInRange - i - 1));
					Thing thing2;
					GenDrop.TryDropSpawn(thing, dropCell, this.Pawn.MapHeld, ThingPlaceMode.Near, out thing2, null, null, true);
				}
			}
			EffecterDefOf.MeatExplosion.Spawn(this.Pawn.PositionHeld, this.Pawn.MapHeld, 1f).Cleanup();
		}

		// Token: 0x06009D85 RID: 40325 RVA: 0x00389DDF File Offset: 0x00387FDF
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompFleshmassNucleus.<CompGetGizmosExtra>d__21 <CompGetGizmosExtra>d__ = new CompFleshmassNucleus.<CompGetGizmosExtra>d__21(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009D86 RID: 40326 RVA: 0x00389DF0 File Offset: 0x00387FF0
		public void OnActivityActivated()
		{
			IntVec3 positionHeld = this.Pawn.PositionHeld;
			Map mapHeld = this.Pawn.MapHeld;
			FleshbeastUtility.MeatSplatter(10, positionHeld, mapHeld, FleshbeastUtility.MeatExplosionSize.Large);
			this.Pawn.Destroy(DestroyMode.Vanish);
			float threatPoints = StorytellerUtility.DefaultThreatPointsNow(mapHeld) * 0.5f;
			Building_FleshmassHeart building_FleshmassHeart = (Building_FleshmassHeart)ThingMaker.MakeThing(ThingDefOf.FleshmassHeart, null);
			building_FleshmassHeart.Comp.threatPoints = threatPoints;
			building_FleshmassHeart.SetFaction(Faction.OfEntities, null);
			GenSpawn.Spawn(building_FleshmassHeart, positionHeld, mapHeld, WipeMode.Vanish);
		}

		// Token: 0x06009D87 RID: 40327 RVA: 0x00389E69 File Offset: 0x00388069
		public void OnPassive()
		{
		}

		// Token: 0x06009D88 RID: 40328 RVA: 0x00389E6B File Offset: 0x0038806B
		public bool ShouldGoPassive()
		{
			return false;
		}

		// Token: 0x06009D89 RID: 40329 RVA: 0x00389E6E File Offset: 0x0038806E
		public bool CanBeSuppressed()
		{
			return true;
		}

		// Token: 0x06009D8A RID: 40330 RVA: 0x00389E74 File Offset: 0x00388074
		public bool CanActivate()
		{
			using (IEnumerator<IntVec3> enumerator = GenAdj.CellsOccupiedBy(this.Pawn.PositionHeld, Rot4.South, new IntVec2(2, 2)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.SupportsStructureType(this.Pawn.MapHeld, ThingDefOf.FleshmassHeart.terrainAffordanceNeeded))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06009D8B RID: 40331 RVA: 0x00389EF4 File Offset: 0x003880F4
		public string ActivityTooltipExtra()
		{
			return null;
		}

		// Token: 0x06009D8C RID: 40332 RVA: 0x00389EF8 File Offset: 0x003880F8
		public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
		{
			IntVec3 cell;
			if (RCellFinder.TryFindRandomCellNearWith(this.Pawn.Position, (IntVec3 c) => CompFleshmassNucleus.IsValidCell(c, this.Pawn.MapHeld), this.Pawn.MapHeld, out cell, 10, 2147483647))
			{
				SkipUtility.SkipTo(this.Pawn, cell, this.Pawn.MapHeld);
				this.Activity.AdjustActivity(this.Props.activityOnRoofCollapsed);
			}
			return RoofCollapseResponse.RemoveThing;
		}

		// Token: 0x06009D8D RID: 40333 RVA: 0x00389F66 File Offset: 0x00388166
		private static bool IsValidCell(IntVec3 cell, Map map)
		{
			return cell.InBounds(map) && cell.Walkable(map);
		}

		// Token: 0x04005D4C RID: 23884
		private int minNextFireTick;

		// Token: 0x04005D4D RID: 23885
		private CompActivity activityComp;

		// Token: 0x04005D4E RID: 23886
		private int debugNextMeatTick = -1;

		// Token: 0x04005D4F RID: 23887
		private int debugActiveTick = -1;

		// Token: 0x04005D50 RID: 23888
		private static readonly IntRange InitialMeatDelayRange = new IntRange(5000, 12500);

		// Token: 0x04005D51 RID: 23889
		private static readonly IntRange MeatClumpRange = new IntRange(3, 6);

		// Token: 0x04005D52 RID: 23890
		private const int MinRefireTicks = 15000;

		// Token: 0x04005D53 RID: 23891
		private const int MeatDaysMTB = 1;

		// Token: 0x04005D54 RID: 23892
		private const int MinMeatPerStack = 20;

		// Token: 0x04005D55 RID: 23893
		private const float HeartPointsFactor = 0.5f;
	}
}
