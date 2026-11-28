using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D7 RID: 5591
	public class Thought_IdeoRoleEmpty : Thought_Situational
	{
		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x06008AE9 RID: 35561 RVA: 0x00326092 File Offset: 0x00324292
		public Precept_Role Role
		{
			get
			{
				return (Precept_Role)this.sourcePrecept;
			}
		}

		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x06008AEA RID: 35562 RVA: 0x0032609F File Offset: 0x0032429F
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.Role.Named("ROLE"));
			}
		}

		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x06008AEB RID: 35563 RVA: 0x003260C6 File Offset: 0x003242C6
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Role.ideo.memberName, this.Role.Named("ROLE"));
			}
		}

		// Token: 0x06008AEC RID: 35564 RVA: 0x00326104 File Offset: 0x00324304
		protected override ThoughtState CurrentStateInternal()
		{
			if (this.pawn.IsSlave)
			{
				return false;
			}
			if (GenDate.DaysPassed < 10)
			{
				return false;
			}
			if (this.Role.def.leaderRole && !Faction.OfPlayer.ideos.IsPrimary(this.Role.ideo))
			{
				return false;
			}
			return this.Role.Active && this.pawn.Ideo == this.Role.ideo && this.Role.ChosenPawnSingle() == null;
		}

		// Token: 0x06008AED RID: 35565 RVA: 0x003261A8 File Offset: 0x003243A8
		public override bool GroupsWith(Thought other)
		{
			Thought_IdeoRoleEmpty thought_IdeoRoleEmpty = other as Thought_IdeoRoleEmpty;
			return thought_IdeoRoleEmpty != null && this.Role == thought_IdeoRoleEmpty.Role;
		}
	}
}
