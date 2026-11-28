using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C9 RID: 4297
	public class QuestPart_InvolvedFactions : QuestPart
	{
		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06006B6B RID: 27499 RVA: 0x0025AA49 File Offset: 0x00258C49
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_InvolvedFactions.<get_InvolvedFactions>d__2 <get_InvolvedFactions>d__ = new QuestPart_InvolvedFactions.<get_InvolvedFactions>d__2(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006B6C RID: 27500 RVA: 0x0025AA59 File Offset: 0x00258C59
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Faction>(ref this.factions, "factions", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04004446 RID: 17478
		public List<Faction> factions = new List<Faction>();
	}
}
