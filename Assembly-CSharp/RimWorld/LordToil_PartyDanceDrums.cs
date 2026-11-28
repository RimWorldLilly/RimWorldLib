using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D0A RID: 3338
	public class LordToil_PartyDanceDrums : LordToil_Ritual
	{
		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06005DEB RID: 24043 RVA: 0x00225F9F File Offset: 0x0022419F
		public new LordToilData_PartyDanceDrums Data
		{
			get
			{
				return (LordToilData_PartyDanceDrums)this.data;
			}
		}

		// Token: 0x06005DEC RID: 24044 RVA: 0x00225FAC File Offset: 0x002241AC
		public LordToil_PartyDanceDrums(IntVec3 spot, LordJob_Ritual ritual, RitualStage stage, Pawn organizer) : base(spot, ritual, stage, organizer)
		{
			this.data = new LordToilData_PartyDanceDrums();
		}

		// Token: 0x06005DED RID: 24045 RVA: 0x00225FC4 File Offset: 0x002241C4
		public override void UpdateAllDuties()
		{
			this.reservedThings.Clear();
			for (int i = this.lord.ownedPawns.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = this.DutyForPawn(pawn);
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06005DEE RID: 24046 RVA: 0x0022602C File Offset: 0x0022422C
		private PawnDuty DutyForPawn(Pawn pawn)
		{
			DutyDef def = this.stage.defaultDuty;
			IntVec3 spot = this.spot;
			LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;
			int num = this.ritual.assignments.Participants.IndexOf(pawn);
			if (num != -1 && (num % 2 == 0 || this.ritual.assignments.Participants.Count == 0))
			{
				Building_MusicalInstrument building_MusicalInstrument = (from m in pawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_MusicalInstrument>()
				where GatheringsUtility.InGatheringArea(m.InteractionCell, this.spot, pawn.Map) && GatheringWorker_Concert.InstrumentAccessible(m, pawn)
				select m).RandomElementWithFallback(null);
				if (building_MusicalInstrument != null && building_MusicalInstrument.Spawned)
				{
					def = DutyDefOf.PlayTargetInstrument;
					focusSecond = building_MusicalInstrument;
					this.ritual.usedThings.Add(building_MusicalInstrument);
					this.reservedThings.Add(building_MusicalInstrument);
					this.Data.playedInstruments.SetOrAdd(pawn, building_MusicalInstrument);
				}
			}
			return new PawnDuty(def, spot, focusSecond, (LocalTargetInfo)this.ritual.selectedTarget, -1f);
		}

		// Token: 0x06005DEF RID: 24047 RVA: 0x00226154 File Offset: 0x00224354
		public override void Notify_BuildingDespawnedOnMap(Building b)
		{
			if (b.def == ThingDefOf.Drum && this.Data.playedInstruments.ContainsValue(b))
			{
				Pawn key = this.Data.playedInstruments.First((KeyValuePair<Pawn, Building> kv) => kv.Value == b).Key;
				key.mindState.duty = this.DutyForPawn(key);
				key.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}
	}
}
