using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x02002254 RID: 8788
	public class QuestPart_SleepingMechs : QuestPart
	{
		// Token: 0x0600D93C RID: 55612 RVA: 0x00507D1C File Offset: 0x00505F1C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				this.SpawnMechs();
				return;
			}
			if (!this.wokeUp)
			{
				if (signal.tag == this.inSignalTookDamage)
				{
					if ((float)this.defendThing.HitPoints < (float)this.defendThing.MaxHitPoints * 0.75f)
					{
						this.TryWakeMechsUp();
						return;
					}
				}
				else if (signal.tag == this.inSignalLockedOut)
				{
					this.TryWakeMechsUp();
				}
			}
		}

		// Token: 0x0600D93D RID: 55613 RVA: 0x00507DA0 File Offset: 0x00505FA0
		private void TryWakeMechsUp()
		{
			if (this.wokeUp)
			{
				return;
			}
			this.wokeUp = true;
			if (this.lord != null)
			{
				foreach (Pawn thing in this.lord.ownedPawns)
				{
					thing.TryGetComp<CompCanBeDormant>().WakeUp();
				}
			}
		}

		// Token: 0x0600D93E RID: 55614 RVA: 0x00507E14 File Offset: 0x00506014
		private void SpawnMechs()
		{
			MapParent mapParent = this.mapParent;
			Map map = (mapParent != null) ? mapParent.Map : null;
			if (map == null)
			{
				return;
			}
			float num = QuestPart_SleepingMechs.ThreatPointsToMechPointsCurve.Evaluate(this.points);
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				tile = map.Tile,
				faction = Find.FactionManager.FirstFactionOfDef(FactionDefOf.Mechanoid),
				points = num
			}, true).ToList<Pawn>();
			if (!list.Any<Pawn>())
			{
				return;
			}
			this.lord = LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_SleepThenMechanoidsDefend(new List<Thing>
			{
				this.defendThing
			}, Faction.OfMechanoids, 40f, this.defendThing.Position, false, false), map, null);
			foreach (Pawn pawn in list)
			{
				GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(this.defendThing.Position, map, 5, null), map, WipeMode.Vanish);
				this.lord.AddPawn(pawn);
				CompCanBeDormant compCanBeDormant = pawn.TryGetComp<CompCanBeDormant>();
				compCanBeDormant.ToSleep();
				compCanBeDormant.wakeUpSignalTag = this.inSignal;
			}
			Building building = this.defendThing as Building;
			if (building != null)
			{
				this.lord.AddBuilding(building);
			}
		}

		// Token: 0x0600D93F RID: 55615 RVA: 0x00507F70 File Offset: 0x00506170
		public override void Cleanup()
		{
			this.defendThing = null;
			this.mapParent = null;
			this.lord = null;
		}

		// Token: 0x0600D940 RID: 55616 RVA: 0x00507F88 File Offset: 0x00506188
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.inSignalTookDamage, "inSignalTookDamage", null, false);
			Scribe_Values.Look<string>(ref this.inSignalLockedOut, "inSignalLockedOut", null, false);
			Scribe_References.Look<Thing>(ref this.defendThing, "defendThing", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
		}

		// Token: 0x04008ADE RID: 35550
		public string inSignal;

		// Token: 0x04008ADF RID: 35551
		public string inSignalTookDamage;

		// Token: 0x04008AE0 RID: 35552
		public string inSignalLockedOut;

		// Token: 0x04008AE1 RID: 35553
		public Thing defendThing;

		// Token: 0x04008AE2 RID: 35554
		public MapParent mapParent;

		// Token: 0x04008AE3 RID: 35555
		public float points;

		// Token: 0x04008AE4 RID: 35556
		private Lord lord;

		// Token: 0x04008AE5 RID: 35557
		private bool wokeUp;

		// Token: 0x04008AE6 RID: 35558
		private const float HPThresholdWakeUp = 0.75f;

		// Token: 0x04008AE7 RID: 35559
		private static readonly SimpleCurve ThreatPointsToMechPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 150f),
				true
			},
			{
				new CurvePoint(1000f, 500f),
				true
			},
			{
				new CurvePoint(10000f, 2000f),
				true
			}
		};
	}
}
