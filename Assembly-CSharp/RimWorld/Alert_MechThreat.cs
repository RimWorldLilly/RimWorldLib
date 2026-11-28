using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB7 RID: 7095
	public class Alert_MechThreat : Alert_Scenario
	{
		// Token: 0x170020EC RID: 8428
		// (get) Token: 0x0600B600 RID: 46592 RVA: 0x00401983 File Offset: 0x003FFB83
		private bool Red
		{
			get
			{
				return Find.TickManager.TicksGame > this.raidTick - 60000;
			}
		}

		// Token: 0x170020ED RID: 8429
		// (get) Token: 0x0600B601 RID: 46593 RVA: 0x0040199D File Offset: 0x003FFB9D
		private bool Critical
		{
			get
			{
				return Find.TickManager.TicksGame > this.raidTick;
			}
		}

		// Token: 0x0600B602 RID: 46594 RVA: 0x004019B1 File Offset: 0x003FFBB1
		public override AlertReport GetReport()
		{
			return AlertReport.Active;
		}

		// Token: 0x0600B603 RID: 46595 RVA: 0x004019B8 File Offset: 0x003FFBB8
		public override string GetLabel()
		{
			if (this.Critical)
			{
				return "AlertMechanoidThreatCritical".Translate();
			}
			return "AlertMechanoidThreat".Translate() + ": " + (this.raidTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(false, false, false, true, false);
		}

		// Token: 0x0600B604 RID: 46596 RVA: 0x00401A16 File Offset: 0x003FFC16
		public override TaggedString GetExplanation()
		{
			if (this.Critical)
			{
				return "AlertMechanoidThreatCriticalDesc".Translate();
			}
			return "AlertMechanoidThreatDesc".Translate();
		}

		// Token: 0x170020EE RID: 8430
		// (get) Token: 0x0600B605 RID: 46597 RVA: 0x00401A35 File Offset: 0x003FFC35
		protected override Color BGColor
		{
			get
			{
				if (!this.Red)
				{
					return Color.clear;
				}
				return Alert_Critical.BgColor();
			}
		}

		// Token: 0x040068A1 RID: 26785
		public int raidTick;
	}
}
