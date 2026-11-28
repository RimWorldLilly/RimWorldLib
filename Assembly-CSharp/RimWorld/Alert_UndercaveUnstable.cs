using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA9 RID: 7081
	public class Alert_UndercaveUnstable : Alert_Critical
	{
		// Token: 0x170020DF RID: 8415
		// (get) Token: 0x0600B5C1 RID: 46529 RVA: 0x0040042C File Offset: 0x003FE62C
		private PitGate PitGate
		{
			get
			{
				return this.targets[0].Thing as PitGate;
			}
		}

		// Token: 0x170020E0 RID: 8416
		// (get) Token: 0x0600B5C2 RID: 46530 RVA: 0x00400452 File Offset: 0x003FE652
		protected override Color BGColor
		{
			get
			{
				PitGate pitGate = this.PitGate;
				if (pitGate != null && pitGate.CollapseStage == 1)
				{
					return Color.clear;
				}
				return base.BGColor;
			}
		}

		// Token: 0x0600B5C3 RID: 46531 RVA: 0x00400478 File Offset: 0x003FE678
		public Alert_UndercaveUnstable()
		{
			this.defaultLabel = "Alert_UndercaveUnstable".Translate();
			this.defaultExplanation = "Alert_UndercaveUnstableDesc".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0600B5C4 RID: 46532 RVA: 0x004004C8 File Offset: 0x003FE6C8
		private void CalculateTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Thing thing in maps[i].listerThings.ThingsOfDef(ThingDefOf.PitGate))
				{
					PitGate pitGate = (PitGate)thing;
					if (pitGate.IsCollapsing)
					{
						this.targets.Add(pitGate);
					}
				}
			}
		}

		// Token: 0x0600B5C5 RID: 46533 RVA: 0x00400564 File Offset: 0x003FE764
		public override string GetLabel()
		{
			return this.defaultLabel + ": " + this.PitGate.TicksUntilCollapse.ToStringTicksToPeriodVerbose(true, true);
		}

		// Token: 0x0600B5C6 RID: 46534 RVA: 0x00400588 File Offset: 0x003FE788
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006888 RID: 26760
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
