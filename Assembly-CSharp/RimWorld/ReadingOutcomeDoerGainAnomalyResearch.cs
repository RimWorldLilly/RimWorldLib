using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EAC RID: 3756
	public class ReadingOutcomeDoerGainAnomalyResearch : ReadingOutcomeDoerGainResearch
	{
		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x060062DF RID: 25311 RVA: 0x00239413 File Offset: 0x00237613
		public new BookOutcomeProperties_GainAnomalyResearch Props
		{
			get
			{
				return (BookOutcomeProperties_GainAnomalyResearch)this.props;
			}
		}

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x060062E0 RID: 25312 RVA: 0x00239420 File Offset: 0x00237620
		public override int RoundTo { get; }

		// Token: 0x060062E1 RID: 25313 RVA: 0x00239428 File Offset: 0x00237628
		protected override float GetBaseValue()
		{
			return BookUtility.GetAnomalyExpForQuality(base.Quality);
		}

		// Token: 0x060062E2 RID: 25314 RVA: 0x00239438 File Offset: 0x00237638
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				if (researchProjectDef.CanStartNow)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060062E3 RID: 25315 RVA: 0x002394A0 File Offset: 0x002376A0
		public override void OnReadingTick(Pawn reader, float factor)
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				ResearchProjectDef researchProjectDef2 = researchProjectDef;
				float num2 = num;
				if (researchProjectDef2.CanStartNow)
				{
					Find.ResearchManager.ApplyKnowledge(researchProjectDef2, num2 * factor, out num);
				}
			}
		}
	}
}
