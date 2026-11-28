using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C09 RID: 7177
	public class Alert_GauranlenTreeWithoutProductionMode : Alert
	{
		// Token: 0x0600B74E RID: 46926 RVA: 0x00409CF8 File Offset: 0x00407EF8
		public Alert_GauranlenTreeWithoutProductionMode()
		{
			this.defaultLabel = "AlertGauranlenTreeWithoutDryadTypeLabel".Translate();
			this.defaultExplanation = "AlertGauranlenTreeWithoutDryadTypeDesc".Translate("ChangeMode".Translate());
			this.requireIdeology = true;
		}

		// Token: 0x0600B74F RID: 46927 RVA: 0x00409D56 File Offset: 0x00407F56
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600B750 RID: 46928 RVA: 0x00409D6C File Offset: 0x00407F6C
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.DryadSpawner);
					for (int j = 0; j < list.Count; j++)
					{
						CompTreeConnection compTreeConnection = list[j].TryGetComp<CompTreeConnection>();
						if (compTreeConnection != null && compTreeConnection.Connected && !compTreeConnection.HasProductionMode)
						{
							this.targets.Add(list[j]);
						}
					}
				}
			}
		}

		// Token: 0x04006927 RID: 26919
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
