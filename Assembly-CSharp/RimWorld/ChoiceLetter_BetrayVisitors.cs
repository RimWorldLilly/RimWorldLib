using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D53 RID: 7507
	[Obsolete("Betray visitors is now a subquest of refugee quest.")]
	public class ChoiceLetter_BetrayVisitors : ChoiceLetter
	{
		// Token: 0x170023DE RID: 9182
		// (get) Token: 0x0600C24F RID: 49743 RVA: 0x0046CFDC File Offset: 0x0046B1DC
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023DF RID: 9183
		// (get) Token: 0x0600C250 RID: 49744 RVA: 0x0046CFE0 File Offset: 0x0046B1E0
		public override bool CanShowInLetterStack
		{
			get
			{
				if (!base.CanShowInLetterStack)
				{
					return false;
				}
				if (this.quest == null || this.quest.State != QuestState.Ongoing)
				{
					return false;
				}
				if (this.requiresAliveAsker && (this.asker == null || this.asker.Dead))
				{
					return false;
				}
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Spawned && !this.pawns[i].Destroyed)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170023E0 RID: 9184
		// (get) Token: 0x0600C251 RID: 49745 RVA: 0x0046D06D File Offset: 0x0046B26D
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_BetrayVisitors.<get_Choices>d__8 <get_Choices>d__ = new ChoiceLetter_BetrayVisitors.<get_Choices>d__8(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x0600C252 RID: 49746 RVA: 0x0046D080 File Offset: 0x0046B280
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Pawn>(ref this.asker, "asker", false);
			Scribe_Values.Look<bool>(ref this.requiresAliveAsker, "requiresAliveAsker", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006F17 RID: 28439
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006F18 RID: 28440
		public Pawn asker;

		// Token: 0x04006F19 RID: 28441
		public bool requiresAliveAsker;
	}
}
