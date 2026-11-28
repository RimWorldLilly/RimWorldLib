using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB5 RID: 7093
	public class Alert_IdeoBuildingDisrespected : Alert_Precept
	{
		// Token: 0x0600B5FA RID: 46586 RVA: 0x00401717 File Offset: 0x003FF917
		public Alert_IdeoBuildingDisrespected()
		{
		}

		// Token: 0x0600B5FB RID: 46587 RVA: 0x00401738 File Offset: 0x003FF938
		public Alert_IdeoBuildingDisrespected(IdeoBuildingPresenceDemand demand)
		{
			this.demand = demand;
			this.label = "IdeoBuildingDisrespected".Translate(demand.parent.LabelCap);
			this.sourcePrecept = demand.parent;
		}

		// Token: 0x170020EB RID: 8427
		// (get) Token: 0x0600B5FC RID: 46588 RVA: 0x0040179C File Offset: 0x003FF99C
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.targetNames.Clear();
				Map currentMap = Find.CurrentMap;
				if (currentMap == null)
				{
					return null;
				}
				foreach (Pawn pawn in currentMap.mapPawns.FreeColonistsSpawned)
				{
					if (pawn.Ideo == this.demand.parent.ideo)
					{
						this.targetNames.Add(pawn.LabelShort);
					}
				}
				Thing thing = this.demand.BestBuilding(currentMap, false);
				if (thing != null)
				{
					this.targets.Add(thing);
				}
				return this.targets;
			}
		}

		// Token: 0x0600B5FD RID: 46589 RVA: 0x00401860 File Offset: 0x003FFA60
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			Map currentMap = Find.CurrentMap;
			if (currentMap == null || !this.demand.BuildingPresent(currentMap) || !this.demand.AppliesTo(currentMap) || this.demand.RequirementsSatisfied(currentMap))
			{
				return false;
			}
			if (!Faction.OfPlayer.ideos.Has(this.demand.parent.ideo))
			{
				return false;
			}
			return new AlertReport
			{
				active = true,
				culpritsTargets = this.Targets
			};
		}

		// Token: 0x0600B5FE RID: 46590 RVA: 0x004018FC File Offset: 0x003FFAFC
		public override TaggedString GetExplanation()
		{
			return "IdeoBuildingDisrespectedDesc".Translate(this.demand.parent.ideo.name, this.demand.parent.Label.Colorize(ColoredText.NameColor), this.demand.RoomRequirementsInfo.ToLineList(" -  "), this.targetNames.ToLineList(" -  "));
		}

		// Token: 0x0400689E RID: 26782
		public IdeoBuildingPresenceDemand demand;

		// Token: 0x0400689F RID: 26783
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x040068A0 RID: 26784
		private List<string> targetNames = new List<string>();
	}
}
