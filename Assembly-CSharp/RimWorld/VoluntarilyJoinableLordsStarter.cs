using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A0 RID: 5024
	public class VoluntarilyJoinableLordsStarter : IExposable
	{
		// Token: 0x06007C87 RID: 31879 RVA: 0x002C7E55 File Offset: 0x002C6055
		public VoluntarilyJoinableLordsStarter(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007C88 RID: 31880 RVA: 0x002C7E6F File Offset: 0x002C606F
		public bool TryStartMarriageCeremony(Pawn firstFiance, Pawn secondFiance)
		{
			if (!GatheringDefOf.MarriageCeremony.CanExecute(firstFiance.Map, firstFiance, true))
			{
				return false;
			}
			GatheringDefOf.MarriageCeremony.Worker.TryExecute(firstFiance.Map, firstFiance);
			this.lastLordStartTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x06007C89 RID: 31881 RVA: 0x002C7EB0 File Offset: 0x002C60B0
		public bool TryStartRandomGathering(bool forceStart = false)
		{
			VoluntarilyJoinableLordsStarter.tmpGatherings.Clear();
			foreach (GatheringDef gatheringDef in DefDatabase<GatheringDef>.AllDefsListForReading)
			{
				if (gatheringDef.IsRandomSelectable && gatheringDef.CanExecute(this.map, null, forceStart))
				{
					VoluntarilyJoinableLordsStarter.tmpGatherings.Add(gatheringDef);
				}
			}
			GatheringDef gatheringDef2;
			return VoluntarilyJoinableLordsStarter.tmpGatherings.TryRandomElementByWeight((GatheringDef def) => def.randomSelectionWeight, out gatheringDef2) && this.TryStartGathering(gatheringDef2);
		}

		// Token: 0x06007C8A RID: 31882 RVA: 0x002C7F60 File Offset: 0x002C6160
		public bool TryStartGathering(GatheringDef gatheringDef)
		{
			if (!gatheringDef.Worker.TryExecute(this.map, null))
			{
				return false;
			}
			this.lastLordStartTick = Find.TickManager.TicksGame;
			this.startRandomGatheringASAP = false;
			return true;
		}

		// Token: 0x06007C8B RID: 31883 RVA: 0x002C7F90 File Offset: 0x002C6190
		public void VoluntarilyJoinableLordsStarterTick()
		{
			this.Tick_TryStartRandomGathering();
		}

		// Token: 0x06007C8C RID: 31884 RVA: 0x002C7F98 File Offset: 0x002C6198
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLordStartTick, "lastLordStartTick", 0, false);
			Scribe_Values.Look<bool>(ref this.startRandomGatheringASAP, "startPartyASAP", false, false);
		}

		// Token: 0x06007C8D RID: 31885 RVA: 0x002C7FC0 File Offset: 0x002C61C0
		private void Tick_TryStartRandomGathering()
		{
			if (!this.map.IsPlayerHome)
			{
				return;
			}
			if (Find.TickManager.TicksGame % 5000 == 0)
			{
				if (Rand.MTBEventOccurs(40f, 60000f, 5000f))
				{
					this.startRandomGatheringASAP = true;
				}
				if (this.startRandomGatheringASAP && Find.TickManager.TicksGame - this.lastLordStartTick >= 600000)
				{
					this.TryStartRandomGathering(false);
				}
			}
		}

		// Token: 0x04004E65 RID: 20069
		private Map map;

		// Token: 0x04004E66 RID: 20070
		private int lastLordStartTick = -999999;

		// Token: 0x04004E67 RID: 20071
		private bool startRandomGatheringASAP;

		// Token: 0x04004E68 RID: 20072
		private const int CheckStartGatheringIntervalTicks = 5000;

		// Token: 0x04004E69 RID: 20073
		private const float StartGatheringMTBDays = 40f;

		// Token: 0x04004E6A RID: 20074
		private static List<GatheringDef> tmpGatherings = new List<GatheringDef>();
	}
}
