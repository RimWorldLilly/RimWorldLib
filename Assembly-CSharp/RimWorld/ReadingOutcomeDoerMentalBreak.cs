using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB6 RID: 3766
	public class ReadingOutcomeDoerMentalBreak : BookOutcomeDoer
	{
		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x06006322 RID: 25378 RVA: 0x0023A64F File Offset: 0x0023884F
		public new BookOutcomeProperties_MentalBreak Props
		{
			get
			{
				return (BookOutcomeProperties_MentalBreak)this.props;
			}
		}

		// Token: 0x06006323 RID: 25379 RVA: 0x0023A65C File Offset: 0x0023885C
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			return false;
		}

		// Token: 0x06006324 RID: 25380 RVA: 0x0023A660 File Offset: 0x00238860
		public override void OnBookGenerated(Pawn author = null)
		{
			float num = this.Props.chancePerHourRange.RandomInRange;
			num = this.Props.chanceMinMaxRange.ClampToRange(num);
			base.Book.SetMentalBreakChance(num);
		}
	}
}
