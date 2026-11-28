using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200188C RID: 6284
	public class CompFleshmassHeart : CompGrowsFleshmassTendrils
	{
		// Token: 0x17001AEC RID: 6892
		// (get) Token: 0x06009D3F RID: 40255 RVA: 0x00388A25 File Offset: 0x00386C25
		public Building_FleshmassHeart Heart
		{
			get
			{
				return this.parent as Building_FleshmassHeart;
			}
		}

		// Token: 0x06009D40 RID: 40256 RVA: 0x00388A34 File Offset: 0x00386C34
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.threatPoints, "threatPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.fleshbeastPoints, "fleshbeastPoints", 0f, false);
			Scribe_Values.Look<int>(ref this.nextFleshbeast, "nextFleshbeast", 0, false);
			Scribe_Values.Look<int>(ref this.lastGrowthCycle, "lastGrowthCycle", 0, false);
			Scribe_Values.Look<int>(ref this.nerveBundlesThisGrowthCycle, "nerveBundlesThisGrowthCycle", 0, false);
			Scribe_Values.Look<int>(ref this.spittersThisGrowthCycle, "spittersThisGrowthCycle", 0, false);
			Scribe_Collections.Look<Thing>(ref this.nerveBundles, "nerveBundles", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.spitters, "spitters", LookMode.Reference, Array.Empty<object>());
			this.nerveBundles.RemoveWhere((Thing b) => b == null);
			this.spitters.RemoveWhere((Thing s) => s == null);
		}

		// Token: 0x06009D41 RID: 40257 RVA: 0x00388B3C File Offset: 0x00386D3C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				if (this.threatPoints <= 0f)
				{
					this.threatPoints = StorytellerUtility.DefaultThreatPointsNow(base.Map);
				}
				this.fleshbeastPoints = (float)Mathf.RoundToInt(this.threatPoints * 0.75f);
				this.lastGrowthCycle = Find.TickManager.TicksGame;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06009D42 RID: 40258 RVA: 0x00388B9C File Offset: 0x00386D9C
		public override void CompTick()
		{
			int num = Find.TickManager.TicksGame - this.lastGrowthCycle;
			if (base.Props.maxGrowthCycleHours > 0 && num > base.Props.maxGrowthCycleHours * 2500)
			{
				this.StartGrowthCycle();
			}
			else if (base.Props.mtbGrowthCycleHours > 0 && num > base.Props.minGrowthCycleHours * 2500 && Rand.MTBEventOccurs((float)base.Props.mtbGrowthCycleHours, 2500f, 1f))
			{
				this.StartGrowthCycle();
			}
			if (Rand.MTBEventOccurs(2f, 2500f, 1f))
			{
				this.StartMiniGrowth();
			}
			if (base.Props.spawnFleshbeasts && this.fleshbeastPoints > 0f && Find.TickManager.TicksGame >= this.nextFleshbeast)
			{
				this.fleshbeastPoints -= this.SpawnFleshBeast();
				this.nextFleshbeast = Find.TickManager.TicksGame + CompFleshmassHeart.FleshbeastSpawnIntervalRange.RandomInRange;
			}
			base.CompTick();
		}

		// Token: 0x06009D43 RID: 40259 RVA: 0x00388CAC File Offset: 0x00386EAC
		private void StartGrowthCycle()
		{
			float num = StorytellerUtility.DefaultThreatPointsNow(base.Map);
			float num2 = FleshbeastUtility.ExistingFleshBeastThreat(base.Map);
			int growthPoints = this.growthPoints;
			SimpleCurve growthCyclePointsByThreat = base.Props.growthCyclePointsByThreat;
			this.growthPoints = growthPoints + Mathf.RoundToInt((growthCyclePointsByThreat != null) ? growthCyclePointsByThreat.Evaluate(num) : 0f);
			float num3 = this.fleshbeastPoints;
			SimpleCurve fleshbeastPointsByThreat = base.Props.fleshbeastPointsByThreat;
			this.fleshbeastPoints = num3 + Mathf.Clamp((fleshbeastPointsByThreat != null) ? fleshbeastPointsByThreat.Evaluate(num) : 0f, 0f, num - num2);
			this.lastGrowthCycle = Find.TickManager.TicksGame;
			this.nerveBundlesThisGrowthCycle = 0;
			this.spittersThisGrowthCycle = 0;
		}

		// Token: 0x06009D44 RID: 40260 RVA: 0x00388D54 File Offset: 0x00386F54
		private void StartMiniGrowth()
		{
			float num = StorytellerUtility.DefaultThreatPointsNow(base.Map);
			int growthPoints = this.growthPoints;
			SimpleCurve growthCyclePointsByThreat = base.Props.growthCyclePointsByThreat;
			this.growthPoints = growthPoints + Mathf.RoundToInt((growthCyclePointsByThreat != null) ? growthCyclePointsByThreat.Evaluate(num / 5f) : 0f);
		}

		// Token: 0x06009D45 RID: 40261 RVA: 0x00388DA4 File Offset: 0x00386FA4
		protected override void Grow()
		{
			if (this.nerveBundles.Count < 3 && this.nerveBundlesThisGrowthCycle < 1)
			{
				Thing thing = this.TrySpawnSpecialMass(ThingDefOf.NerveBundle);
				if (thing != null)
				{
					Messages.Message("NerveBundleGrewMessage".Translate(), thing, MessageTypeDefOf.NeutralEvent, true);
					this.nerveBundles.Add(thing);
					this.nerveBundlesThisGrowthCycle++;
					return;
				}
			}
			if ((float)this.spitters.Count < CompFleshmassHeart.TargetSpittersCountByThreatPointsCurve.Evaluate(this.threatPoints) && this.spittersThisGrowthCycle < 1)
			{
				Thing thing2 = this.TrySpawnSpecialMass(ThingDefOf.FleshmassSpitter);
				if (thing2 != null)
				{
					this.spitters.Add(thing2);
					this.spittersThisGrowthCycle++;
					return;
				}
			}
			base.Grow();
		}

		// Token: 0x06009D46 RID: 40262 RVA: 0x00388E6C File Offset: 0x0038706C
		private Thing TrySpawnSpecialMass(ThingDef def)
		{
			if (base.ContiguousFleshmass.EnumerableNullOrEmpty<IntVec3>())
			{
				return null;
			}
			for (int i = 0; i < 10; i++)
			{
				IntVec3 intVec = base.ContiguousFleshmass.RandomElement<IntVec3>();
				if (this.CanSpawnSpecialMass(def, intVec))
				{
					Thing thing = GenSpawn.Spawn(def, intVec, base.Map, WipeMode.Vanish);
					thing.TryGetComp<CompFleshmassHeartChild>().heart = this.Heart;
					thing.SetFaction(this.Heart.Faction, null);
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06009D47 RID: 40263 RVA: 0x00388EE0 File Offset: 0x003870E0
		private bool CanSpawnSpecialMass(ThingDef def, IntVec3 pos)
		{
			if (!GenAdj.OccupiedRect(pos, Rot4.North, def.size).InBounds(this.parent.Map))
			{
				return false;
			}
			if (this.parent.Position.InHorDistOf(pos, 4f))
			{
				return false;
			}
			using (List<Thing>.Enumerator enumerator = this.nerveBundles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Position.InHorDistOf(pos, 4f))
					{
						return false;
					}
				}
			}
			using (List<Thing>.Enumerator enumerator = this.spitters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Position.InHorDistOf(pos, 4f))
					{
						return false;
					}
				}
			}
			foreach (IntVec3 c in GenAdj.CellsOccupiedBy(pos, Rot4.North, def.size))
			{
				Building edifice = c.GetEdifice(this.parent.Map);
				if (((edifice != null) ? edifice.def : null) != ThingDefOf.Fleshmass_Active)
				{
					return false;
				}
				if (def == ThingDefOf.FleshmassSpitter && c.Roofed(this.parent.Map))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009D48 RID: 40264 RVA: 0x00389074 File Offset: 0x00387274
		private float SpawnFleshBeast()
		{
			PawnKindDef kind = this.PickFleshBeastToSpawn();
			Faction ofEntities = Faction.OfEntities;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			float? fixedBiologicalAge = new float?(0f);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, ofEntities, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			Building_FleshmassHeart building_FleshmassHeart = this.parent as Building_FleshmassHeart;
			if (building_FleshmassHeart != null)
			{
				building_FleshmassHeart.DefendHeartLord.AddPawn(pawn);
			}
			IntVec3 intVec = this.FindValidSpawnPoint();
			IntVec3 intVec2 = CellFinder.StandableCellNear(intVec, base.Map, 5f, null);
			if (intVec2 == IntVec3.Invalid)
			{
				return pawn.kindDef.combatPower;
			}
			GenSpawn.Spawn(pawn, intVec2, base.Map, WipeMode.FullRefund);
			pawn.rotationTracker.FaceCell(intVec2);
			EffecterDefOf.MeatExplosionExtraLarge.Spawn(intVec, base.Map, 1f).Cleanup();
			return pawn.kindDef.combatPower;
		}

		// Token: 0x06009D49 RID: 40265 RVA: 0x003891B4 File Offset: 0x003873B4
		private PawnKindDef PickFleshBeastToSpawn()
		{
			PawnKindDef pawnKindDef = FleshbeastUtility.AllFleshbeasts.RandomElement<PawnKindDef>();
			if (this.fleshbeastPoints < pawnKindDef.combatPower)
			{
				return PawnKindDefOf.Fingerspike;
			}
			return pawnKindDef;
		}

		// Token: 0x06009D4A RID: 40266 RVA: 0x003891E4 File Offset: 0x003873E4
		private IntVec3 FindValidSpawnPoint()
		{
			if (this.contiguousFleshmass.Empty<IntVec3>())
			{
				return this.parent.OccupiedRect().Cells.RandomElement<IntVec3>();
			}
			for (int i = 0; i < 20; i++)
			{
				IntVec3 intVec = this.contiguousFleshmass.RandomElement<IntVec3>();
				if (base.AdjacentFleshmass(intVec) < 4)
				{
					return intVec;
				}
			}
			this.contiguousFleshmass.Shuffle<IntVec3>();
			foreach (IntVec3 intVec2 in this.contiguousFleshmass)
			{
				if (base.AdjacentFleshmass(intVec2) < 4)
				{
					return intVec2;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06009D4B RID: 40267 RVA: 0x003892A0 File Offset: 0x003874A0
		public void Notify_ChildDied(Thing thing)
		{
			if (thing.def == ThingDefOf.NerveBundle)
			{
				this.nerveBundles.Remove(thing);
			}
			if (thing.def == ThingDefOf.FleshmassSpitter)
			{
				this.spitters.Remove(thing);
			}
		}

		// Token: 0x06009D4C RID: 40268 RVA: 0x003892D8 File Offset: 0x003874D8
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			foreach (Thing thing in this.nerveBundles)
			{
				CompFleshmassHeartChild compFleshmassHeartChild = thing.TryGetComp<CompFleshmassHeartChild>();
				if (compFleshmassHeartChild != null)
				{
					compFleshmassHeartChild.Notify_HeartDied();
				}
			}
			foreach (Thing thing2 in this.spitters)
			{
				CompFleshmassHeartChild compFleshmassHeartChild2 = thing2.TryGetComp<CompFleshmassHeartChild>();
				if (compFleshmassHeartChild2 != null)
				{
					compFleshmassHeartChild2.Notify_HeartDied();
				}
			}
		}

		// Token: 0x06009D4D RID: 40269 RVA: 0x00389388 File Offset: 0x00387588
		public override void PostSwapMap()
		{
			base.PostSwapMap();
			foreach (Thing thing in this.nerveBundles)
			{
				if (thing.Map != this.parent.Map)
				{
					CompFleshmassHeartChild compFleshmassHeartChild = thing.TryGetComp<CompFleshmassHeartChild>();
					if (compFleshmassHeartChild != null)
					{
						compFleshmassHeartChild.Notify_HeartDied();
					}
				}
			}
			foreach (Thing thing2 in this.spitters)
			{
				if (thing2.Map != this.parent.Map)
				{
					CompFleshmassHeartChild compFleshmassHeartChild2 = thing2.TryGetComp<CompFleshmassHeartChild>();
					if (compFleshmassHeartChild2 != null)
					{
						compFleshmassHeartChild2.Notify_HeartDied();
					}
				}
			}
			this.nerveBundles.RemoveWhere((Thing nb) => !nb.Spawned || nb.Map != this.parent.Map);
			this.spitters.RemoveWhere((Thing s) => !s.Spawned || s.Map != this.parent.Map);
		}

		// Token: 0x06009D4E RID: 40270 RVA: 0x0038948C File Offset: 0x0038768C
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompFleshmassHeart.<CompGetGizmosExtra>d__32 <CompGetGizmosExtra>d__ = new CompFleshmassHeart.<CompGetGizmosExtra>d__32(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005D2E RID: 23854
		private static readonly IntRange FleshbeastSpawnIntervalRange = new IntRange(60, 240);

		// Token: 0x04005D2F RID: 23855
		private const int MinDistanceBetweenSpecialMass = 4;

		// Token: 0x04005D30 RID: 23856
		private const int TargetNerveBundleCount = 3;

		// Token: 0x04005D31 RID: 23857
		private const int MaxNerveBundlesPerGrowthCycle = 1;

		// Token: 0x04005D32 RID: 23858
		private const int MaxSpittersPerGrowthCycle = 1;

		// Token: 0x04005D33 RID: 23859
		private const float InitialFleshbeastPointMultiplier = 0.75f;

		// Token: 0x04005D34 RID: 23860
		private const int SmallGrowthMTBHours = 2;

		// Token: 0x04005D35 RID: 23861
		private static readonly SimpleCurve TargetSpittersCountByThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(200f, 1f),
				true
			},
			{
				new CurvePoint(800f, 2f),
				true
			},
			{
				new CurvePoint(3000f, 4f),
				true
			},
			{
				new CurvePoint(10000f, 7f),
				true
			}
		};

		// Token: 0x04005D36 RID: 23862
		public float threatPoints;

		// Token: 0x04005D37 RID: 23863
		private float fleshbeastPoints;

		// Token: 0x04005D38 RID: 23864
		private int nextFleshbeast = -99999;

		// Token: 0x04005D39 RID: 23865
		private int lastGrowthCycle = -99999;

		// Token: 0x04005D3A RID: 23866
		private int nerveBundlesThisGrowthCycle;

		// Token: 0x04005D3B RID: 23867
		private int spittersThisGrowthCycle;

		// Token: 0x04005D3C RID: 23868
		private List<Thing> nerveBundles = new List<Thing>();

		// Token: 0x04005D3D RID: 23869
		private List<Thing> spitters = new List<Thing>();
	}
}
