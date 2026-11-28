using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200109B RID: 4251
	public class QuestPart_ExitOnShuttle : QuestPart_MakeLord
	{
		// Token: 0x06006A6A RID: 27242 RVA: 0x00256CD5 File Offset: 0x00254ED5
		public QuestPart_ExitOnShuttle()
		{
			ModLister.CheckRoyaltyOrIdeology("Shuttle");
		}

		// Token: 0x06006A6B RID: 27243 RVA: 0x00256CEF File Offset: 0x00254EEF
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_ExitOnShuttle(this.shuttle, this.addFleeToil), base.Map, null);
		}

		// Token: 0x06006A6C RID: 27244 RVA: 0x00256D14 File Offset: 0x00254F14
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x0400439C RID: 17308
		public Thing shuttle;

		// Token: 0x0400439D RID: 17309
		public bool addFleeToil = true;
	}
}
