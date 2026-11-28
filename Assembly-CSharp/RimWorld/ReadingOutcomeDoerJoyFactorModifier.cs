using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB4 RID: 3764
	public class ReadingOutcomeDoerJoyFactorModifier : BookOutcomeDoer
	{
		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x0600631B RID: 25371 RVA: 0x0023A582 File Offset: 0x00238782
		public new BookOutcomeProperties_JoyFactorModifier Props
		{
			get
			{
				return (BookOutcomeProperties_JoyFactorModifier)this.props;
			}
		}

		// Token: 0x0600631C RID: 25372 RVA: 0x0023A58F File Offset: 0x0023878F
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			return true;
		}

		// Token: 0x0600631D RID: 25373 RVA: 0x0023A592 File Offset: 0x00238792
		public override void OnBookGenerated(Pawn author = null)
		{
			base.Book.SetJoyFactor(BookUtility.GetNovelJoyFactorForQuality(base.Quality));
		}

		// Token: 0x0600631E RID: 25374 RVA: 0x0023A5AC File Offset: 0x002387AC
		public override string GetBenefitsString(Pawn reader = null)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 2);
			defaultInterpolatedStringHandler.AppendLiteral(" - ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("BookJoyFactor".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": x");
			defaultInterpolatedStringHandler.AppendFormatted(base.Book.JoyFactor.ToStringPercent());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
