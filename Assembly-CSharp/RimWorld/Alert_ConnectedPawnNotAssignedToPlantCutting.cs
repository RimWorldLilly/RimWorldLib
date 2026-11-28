using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C0A RID: 7178
	public class Alert_ConnectedPawnNotAssignedToPlantCutting : Alert
	{
		// Token: 0x0600B751 RID: 46929 RVA: 0x00409E09 File Offset: 0x00408009
		public Alert_ConnectedPawnNotAssignedToPlantCutting()
		{
			this.defaultLabel = "AlertConnectedPawnNotAssignedToPlantCuttingLabel".Translate();
			this.requireIdeology = true;
		}

		// Token: 0x0600B752 RID: 46930 RVA: 0x00409E38 File Offset: 0x00408038
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600B753 RID: 46931 RVA: 0x00409E4C File Offset: 0x0040804C
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
						if (compTreeConnection != null && compTreeConnection.Connected && compTreeConnection.DesiredConnectionStrength > 0f && compTreeConnection.ConnectedPawn.workSettings.GetPriority(WorkTypeDefOf.PlantCutting) == 0)
						{
							this.targets.Add(compTreeConnection.ConnectedPawn);
						}
					}
				}
			}
		}

		// Token: 0x0600B754 RID: 46932 RVA: 0x00409F0C File Offset: 0x0040810C
		public override TaggedString GetExplanation()
		{
			return "AlertConnectedPawnNotAssignedToPlantCuttingDesc".Translate() + ":\n" + (from x in this.targets
			select ((Pawn)((Thing)x)).NameFullColored.Resolve()).ToLineList("  - ", true);
		}

		// Token: 0x04006928 RID: 26920
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
