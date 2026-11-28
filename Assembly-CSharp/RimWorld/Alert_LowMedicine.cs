using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCB RID: 7115
	public class Alert_LowMedicine : Alert
	{
		// Token: 0x0600B653 RID: 46675 RVA: 0x004033E0 File Offset: 0x004015E0
		public Alert_LowMedicine()
		{
			this.defaultLabel = "LowMedicine".Translate();
			this.defaultExplanation = "NoMedicineDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600B654 RID: 46676 RVA: 0x00403419 File Offset: 0x00401619
		public override TaggedString GetExplanation()
		{
			if (this.mapWithNoMedicine == null)
			{
				return string.Empty;
			}
			if (this.medicineCount == 0)
			{
				return this.defaultExplanation;
			}
			return "LowMedicineDesc".Translate(this.medicineCount);
		}

		// Token: 0x0600B655 RID: 46677 RVA: 0x00403457 File Offset: 0x00401657
		public override AlertReport GetReport()
		{
			if ((float)Find.TickManager.TicksGame < 150000f)
			{
				return false;
			}
			return this.MapWithLowMedicine() != null;
		}

		// Token: 0x0600B656 RID: 46678 RVA: 0x00403480 File Offset: 0x00401680
		private Map MapWithLowMedicine()
		{
			this.mapWithNoMedicine = null;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.AnyColonistSpawned && (float)this.MedicineCount(map) < 2f * (float)map.mapPawns.FreeColonistsSpawnedCount)
				{
					this.mapWithNoMedicine = map;
					break;
				}
			}
			return this.mapWithNoMedicine;
		}

		// Token: 0x0600B657 RID: 46679 RVA: 0x004034F3 File Offset: 0x004016F3
		private int MedicineCount(Map map)
		{
			this.medicineCount = map.resourceCounter.GetCountIn(ThingRequestGroup.Medicine);
			return this.medicineCount;
		}

		// Token: 0x040068B9 RID: 26809
		private Map mapWithNoMedicine;

		// Token: 0x040068BA RID: 26810
		private int medicineCount;

		// Token: 0x040068BB RID: 26811
		private const float MedicinePerColonistThreshold = 2f;
	}
}
