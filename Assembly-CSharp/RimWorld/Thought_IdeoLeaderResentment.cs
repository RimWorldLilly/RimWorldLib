using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D8 RID: 5592
	public class Thought_IdeoLeaderResentment : Thought_Situational
	{
		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x06008AEF RID: 35567 RVA: 0x003261D7 File Offset: 0x003243D7
		public Pawn Leader
		{
			get
			{
				return this.pawn.Faction.leader;
			}
		}

		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x06008AF0 RID: 35568 RVA: 0x003261E9 File Offset: 0x003243E9
		public override string LabelCap
		{
			get
			{
				return "IdeoLeaderDifferentIdeoThoughtLabel".Translate(this.Leader.Ideo.memberName);
			}
		}

		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x06008AF1 RID: 35569 RVA: 0x0032620F File Offset: 0x0032440F
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Leader.Ideo.memberName);
			}
		}
	}
}
