using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBF RID: 3519
	public class Thought_Situational_WearingDesiredApparel : Thought_Situational
	{
		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x06006041 RID: 24641 RVA: 0x0022ED54 File Offset: 0x0022CF54
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.multipleApparelItems ? "WearingDesiredApparelMulti".Translate().Named("APPAREL") : this.MetApparelPrecepts().FirstOrFallback(this.SourceApparelPrecept).apparelDef.label.Named("APPAREL")).CapitalizeFirst();
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x06006042 RID: 24642 RVA: 0x0022EDC8 File Offset: 0x0022CFC8
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.multipleApparelItems ? "WearingDesiredApparelMulti".Translate().Named("APPAREL") : Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(this.MetApparelPrecepts().FirstOrFallback(this.SourceApparelPrecept).apparelDef.label, false, false).Named("APPAREL"), this.pawn.Named("PAWN")).CapitalizeFirst() + base.CausedByBeliefInPrecept;
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x06006043 RID: 24643 RVA: 0x0022EE61 File Offset: 0x0022D061
		private Precept_Apparel SourceApparelPrecept
		{
			get
			{
				return (Precept_Apparel)this.sourcePrecept;
			}
		}

		// Token: 0x06006044 RID: 24644 RVA: 0x0022EE70 File Offset: 0x0022D070
		protected override ThoughtState CurrentStateInternal()
		{
			if (!ModsConfig.IdeologyActive || this.pawn.Ideo == null)
			{
				return ThoughtState.Inactive;
			}
			List<Precept> allPreceptsAllowingSituationalThought = this.pawn.Ideo.GetAllPreceptsAllowingSituationalThought(this.def);
			if (allPreceptsAllowingSituationalThought.Count == 0)
			{
				return ThoughtState.Inactive;
			}
			int num = 0;
			foreach (Precept precept in allPreceptsAllowingSituationalThought)
			{
				Precept_Apparel precept_Apparel = precept as Precept_Apparel;
				if (precept_Apparel != null && (precept_Apparel.TargetGender == Gender.None || precept_Apparel.TargetGender == this.pawn.gender))
				{
					if (!this.HasApparel(precept_Apparel.apparelDef))
					{
						return ThoughtState.Inactive;
					}
					num++;
				}
			}
			this.multipleApparelItems = (num > 1);
			if (num != 0)
			{
				return base.CurrentStateInternal();
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x06006045 RID: 24645 RVA: 0x0022EF54 File Offset: 0x0022D154
		private IEnumerable<Precept_Apparel> MetApparelPrecepts()
		{
			Thought_Situational_WearingDesiredApparel.<MetApparelPrecepts>d__8 <MetApparelPrecepts>d__ = new Thought_Situational_WearingDesiredApparel.<MetApparelPrecepts>d__8(-2);
			<MetApparelPrecepts>d__.<>4__this = this;
			return <MetApparelPrecepts>d__;
		}

		// Token: 0x06006046 RID: 24646 RVA: 0x0022EF64 File Offset: 0x0022D164
		private bool HasApparel(ThingDef thingDef)
		{
			using (List<Apparel>.Enumerator enumerator = this.pawn.apparel.WornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def == thingDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x0022EFC8 File Offset: 0x0022D1C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.multipleApparelItems, "multipleApparelItems", false, false);
		}

		// Token: 0x040035F2 RID: 13810
		public bool multipleApparelItems;
	}
}
