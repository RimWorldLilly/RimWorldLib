using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C14 RID: 7188
	public class Alert_ToxifierGeneratorStopped : Alert
	{
		// Token: 0x0600B781 RID: 46977 RVA: 0x0040AFAF File Offset: 0x004091AF
		public Alert_ToxifierGeneratorStopped()
		{
			this.requireBiotech = true;
		}

		// Token: 0x0600B782 RID: 46978 RVA: 0x0040AFCC File Offset: 0x004091CC
		public override string GetLabel()
		{
			string result;
			if ((result = this.defaultLabel) == null)
			{
				result = (this.defaultLabel = "AlertToxifierGeneratorStopped".Translate(ThingDefOf.ToxifierGenerator.LabelCap));
			}
			return result;
		}

		// Token: 0x0600B783 RID: 46979 RVA: 0x0040B00C File Offset: 0x0040920C
		public override TaggedString GetExplanation()
		{
			string str;
			if ((str = this.defaultExplanation) == null)
			{
				str = (this.defaultExplanation = "AlertToxifierGeneratorStoppedDesc".Translate(ThingDefOf.ToxifierGenerator.label));
			}
			return str;
		}

		// Token: 0x0600B784 RID: 46980 RVA: 0x0040B050 File Offset: 0x00409250
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.ToxifierGenerator))
				{
					if (!building.TryGetComp<CompToxifier>().CanPolluteNow)
					{
						this.targets.Add(building);
					}
				}
			}
		}

		// Token: 0x0600B785 RID: 46981 RVA: 0x0040B0EC File Offset: 0x004092EC
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400693D RID: 26941
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
