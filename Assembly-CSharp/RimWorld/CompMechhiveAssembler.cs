using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020018EF RID: 6383
	public class CompMechhiveAssembler : ThingComp
	{
		// Token: 0x17001B95 RID: 7061
		// (get) Token: 0x0600A009 RID: 40969 RVA: 0x003954AE File Offset: 0x003936AE
		private CompProperties_MechhiveAssembler Props
		{
			get
			{
				return (CompProperties_MechhiveAssembler)this.props;
			}
		}

		// Token: 0x17001B96 RID: 7062
		// (get) Token: 0x0600A00A RID: 40970 RVA: 0x003954BC File Offset: 0x003936BC
		private CompCanBeDormant DormancyComp
		{
			get
			{
				CompCanBeDormant result;
				if ((result = this.dormancyCompCached) == null)
				{
					result = (this.dormancyCompCached = this.parent.TryGetComp<CompCanBeDormant>());
				}
				return result;
			}
		}

		// Token: 0x17001B97 RID: 7063
		// (get) Token: 0x0600A00B RID: 40971 RVA: 0x003954E7 File Offset: 0x003936E7
		private bool Active
		{
			get
			{
				return this.core != null && this.DormancyComp.Awake;
			}
		}

		// Token: 0x0600A00C RID: 40972 RVA: 0x003954FE File Offset: 0x003936FE
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastSpawnTick, "lastSpawnTick", 0, false);
			Scribe_Values.Look<float>(ref this.mapThreatPoints, "mapThreatPoints", 0f, false);
		}

		// Token: 0x0600A00D RID: 40973 RVA: 0x00395528 File Offset: 0x00393728
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			List<Thing> list = this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.CerebrexCore);
			if (!list.NullOrEmpty<Thing>())
			{
				Thing thing = list[0];
				this.core = ((thing != null) ? thing.TryGetComp<CompCerebrexCore>() : null);
			}
			if (!respawningAfterLoad)
			{
				Site site = this.parent.Map.Parent as Site;
				this.mapThreatPoints = ((site != null) ? site.ActualThreatPoints : -1f);
				if (this.mapThreatPoints < 0f)
				{
					Log.Error("Mechhive assembler failed to get threat points from site");
					this.mapThreatPoints = StorytellerUtility.DefaultThreatPointsNow(this.parent.Map);
				}
				this.lastSpawnTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600A00E RID: 40974 RVA: 0x003955DC File Offset: 0x003937DC
		public override void CompTick()
		{
			if (!this.Active)
			{
				return;
			}
			float num = (float)this.lastSpawnTick + CompMechhiveAssembler.SpawnIntervalCurve.Evaluate(this.mapThreatPoints);
			if ((float)Find.TickManager.TicksGame > num)
			{
				this.lastSpawnTick = Find.TickManager.TicksGame;
				this.SpawnMech();
			}
		}

		// Token: 0x0600A00F RID: 40975 RVA: 0x00395630 File Offset: 0x00393830
		private void SpawnMech()
		{
			float pointsPerAssembler = CompMechhiveAssembler.MaxThreatPointsCurve.Evaluate(this.mapThreatPoints);
			float remainingPoints = pointsPerAssembler * 4f;
			foreach (Pawn pawn in this.core.AssemblerLord.ownedPawns)
			{
				remainingPoints -= pawn.kindDef.combatPower;
			}
			IEnumerable<PawnGenOption> source = from x in this.Props.options
			where x.Cost < pointsPerAssembler && x.Cost < remainingPoints
			select x;
			if (!source.Any<PawnGenOption>())
			{
				return;
			}
			PawnGenOption pawnGenOption = source.RandomElementByWeight((PawnGenOption x) => x.selectionWeight);
			if (pawnGenOption == null)
			{
				return;
			}
			Pawn pawn2 = PawnGenerator.GeneratePawn(pawnGenOption.kind, this.core.AssemblerLord.faction, null);
			IntVec3 loc = CellFinder.StandableCellNear(this.parent.Position, this.parent.Map, 5f, null);
			GenSpawn.Spawn(pawn2, loc, this.parent.Map, WipeMode.Vanish);
			this.core.AssemblerLord.AddPawn(pawn2);
			remainingPoints -= pawnGenOption.Cost;
		}

		// Token: 0x0600A010 RID: 40976 RVA: 0x003957A4 File Offset: 0x003939A4
		public override string CompInspectStringExtra()
		{
			if (this.Active)
			{
				return "ProducingMechanoids".Translate();
			}
			return null;
		}

		// Token: 0x04005EA5 RID: 24229
		private static readonly SimpleCurve MaxThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 200f),
				true
			},
			{
				new CurvePoint(1000f, 250f),
				true
			},
			{
				new CurvePoint(5000f, 750f),
				true
			},
			{
				new CurvePoint(10000f, 1250f),
				true
			}
		};

		// Token: 0x04005EA6 RID: 24230
		private static readonly SimpleCurve SpawnIntervalCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 35000f),
				true
			},
			{
				new CurvePoint(1000f, 30000f),
				true
			},
			{
				new CurvePoint(5000f, 20000f),
				true
			},
			{
				new CurvePoint(10000f, 15000f),
				true
			}
		};

		// Token: 0x04005EA7 RID: 24231
		private int lastSpawnTick;

		// Token: 0x04005EA8 RID: 24232
		private float mapThreatPoints = -1f;

		// Token: 0x04005EA9 RID: 24233
		[Unsaved(false)]
		private CompCerebrexCore core;

		// Token: 0x04005EAA RID: 24234
		[Unsaved(false)]
		private CompCanBeDormant dormancyCompCached;
	}
}
