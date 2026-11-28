using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BC0 RID: 7104
	public class Alert_ReimplantationAvailable : Alert
	{
		// Token: 0x170020F1 RID: 8433
		// (get) Token: 0x0600B628 RID: 46632 RVA: 0x00401FC8 File Offset: 0x004001C8
		private Pawn WaitingPawn
		{
			get
			{
				foreach (Map map in Find.Maps)
				{
					foreach (Lord lord in map.lordManager.lords)
					{
						LordToil_ReimplantXenogerm lordToil_ReimplantXenogerm = lord.CurLordToil as LordToil_ReimplantXenogerm;
						if (lordToil_ReimplantXenogerm != null)
						{
							this.waitingPawn = lordToil_ReimplantXenogerm.Data.target;
							return this.waitingPawn;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x0600B629 RID: 46633 RVA: 0x0040207C File Offset: 0x0040027C
		public Alert_ReimplantationAvailable()
		{
			this.defaultPriority = AlertPriority.High;
			this.defaultLabel = "AlertReimplantationAvailable".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x0600B62A RID: 46634 RVA: 0x004020A7 File Offset: 0x004002A7
		public override TaggedString GetExplanation()
		{
			return "AlertReimplantationAvailableDesc".Translate(this.waitingPawn);
		}

		// Token: 0x0600B62B RID: 46635 RVA: 0x004020BE File Offset: 0x004002BE
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritIs(this.WaitingPawn);
		}

		// Token: 0x040068A9 RID: 26793
		private Pawn waitingPawn;
	}
}
