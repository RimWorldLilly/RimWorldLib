using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BBF RID: 7103
	public class Alert_BestowerWaiting : Alert
	{
		// Token: 0x170020F0 RID: 8432
		// (get) Token: 0x0600B625 RID: 46629 RVA: 0x00401EA4 File Offset: 0x004000A4
		private List<Pawn> BestowersWaiting
		{
			get
			{
				this.bestowersWaitingResult.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Lord lord in map.lordManager.lords)
					{
						if (lord.CurLordToil is LordToil_BestowingCeremony_Wait)
						{
							this.bestowersWaitingResult.Add(lord.ownedPawns[0]);
						}
					}
				}
				return this.bestowersWaitingResult;
			}
		}

		// Token: 0x0600B626 RID: 46630 RVA: 0x00401F64 File Offset: 0x00400164
		public Alert_BestowerWaiting()
		{
			this.defaultPriority = AlertPriority.High;
			this.defaultLabel = "BestowerWaitingAlert".Translate();
			this.defaultExplanation = "BestowerWaitingAlertDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0600B627 RID: 46631 RVA: 0x00401FBA File Offset: 0x004001BA
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BestowersWaiting);
		}

		// Token: 0x040068A8 RID: 26792
		private List<Pawn> bestowersWaitingResult = new List<Pawn>();
	}
}
