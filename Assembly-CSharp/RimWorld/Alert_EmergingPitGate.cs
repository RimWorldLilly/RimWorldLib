using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDF RID: 7135
	public class Alert_EmergingPitGate : Alert
	{
		// Token: 0x0600B6A5 RID: 46757 RVA: 0x004057E5 File Offset: 0x004039E5
		public Alert_EmergingPitGate()
		{
			this.defaultPriority = AlertPriority.High;
			this.requireAnomaly = true;
		}

		// Token: 0x17002102 RID: 8450
		// (get) Token: 0x0600B6A6 RID: 46758 RVA: 0x004057FB File Offset: 0x004039FB
		private BuildingGroundSpawner EmergingPitGate
		{
			get
			{
				Map currentMap = Find.CurrentMap;
				this.pitGate = (((currentMap != null) ? currentMap.listerThings.ThingsOfDef(ThingDefOf.PitGateSpawner).FirstOrDefault<Thing>() : null) as BuildingGroundSpawner);
				return this.pitGate;
			}
		}

		// Token: 0x0600B6A7 RID: 46759 RVA: 0x0040582E File Offset: 0x00403A2E
		public override string GetLabel()
		{
			return "AlertEmergingPitGate".Translate() + ": " + this.pitGate.TicksUntilSpawn.ToStringTicksToPeriodVerbose(true, true);
		}

		// Token: 0x0600B6A8 RID: 46760 RVA: 0x00405860 File Offset: 0x00403A60
		public override TaggedString GetExplanation()
		{
			return string.Format("AlertEmergingPitGateDesc".Translate(), Array.Empty<object>());
		}

		// Token: 0x0600B6A9 RID: 46761 RVA: 0x00405880 File Offset: 0x00403A80
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritIs(this.EmergingPitGate);
		}

		// Token: 0x040068E0 RID: 26848
		private BuildingGroundSpawner pitGate;
	}
}
