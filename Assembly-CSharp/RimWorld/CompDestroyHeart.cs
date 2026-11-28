using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185D RID: 6237
	public class CompDestroyHeart : CompInteractable
	{
		// Token: 0x17001AA8 RID: 6824
		// (get) Token: 0x06009C18 RID: 39960 RVA: 0x003836E7 File Offset: 0x003818E7
		public Building_FleshmassHeart Heart
		{
			get
			{
				return this.parent as Building_FleshmassHeart;
			}
		}

		// Token: 0x17001AA9 RID: 6825
		// (get) Token: 0x06009C19 RID: 39961 RVA: 0x003836F4 File Offset: 0x003818F4
		public override bool HideInteraction
		{
			get
			{
				return !this.CanInteract(null, true);
			}
		}

		// Token: 0x17001AAA RID: 6826
		// (get) Token: 0x06009C1A RID: 39962 RVA: 0x00383706 File Offset: 0x00381906
		public override NamedArgument? ExtraNamedArg
		{
			get
			{
				return new NamedArgument?(this.Heart.BiosignatureName.Named("BIOSIGNATURE"));
			}
		}

		// Token: 0x06009C1B RID: 39963 RVA: 0x00383722 File Offset: 0x00381922
		protected override void OnInteracted(Pawn caster)
		{
			Building_FleshmassHeart heart = this.Heart;
			if (heart == null)
			{
				return;
			}
			heart.StartTachycardiacOverload();
		}

		// Token: 0x06009C1C RID: 39964 RVA: 0x00383734 File Offset: 0x00381934
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AnalysisDetails analysisDetails;
			Find.AnalysisManager.TryGetAnalysisProgress(this.Heart.Biosignature, out analysisDetails);
			if (analysisDetails == null || !analysisDetails.Satisfied)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("DestroyHeartDisabled".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<int>((analysisDetails != null) ? analysisDetails.timesDone : 0);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>((analysisDetails != null) ? analysisDetails.required : 0);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}
	}
}
