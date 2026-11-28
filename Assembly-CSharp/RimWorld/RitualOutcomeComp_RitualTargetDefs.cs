using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D0 RID: 5840
	public class RitualOutcomeComp_RitualTargetDefs : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x06009224 RID: 37412 RVA: 0x00356204 File Offset: 0x00354404
		public override bool Applies(LordJob_Ritual ritual)
		{
			if (this.autoApplyInClassicMode && Find.IdeoManager.classicMode)
			{
				return true;
			}
			Thing thing = ritual.selectedTarget.Thing;
			if (thing == null)
			{
				return false;
			}
			if (!this.CheckIdeoCorrectness((ritual != null) ? ritual.Ritual : null, thing))
			{
				return false;
			}
			if (this.allowAltars && thing.def.isAltar)
			{
				return true;
			}
			if (!this.defs.NullOrEmpty<ThingDef>())
			{
				List<ThingDef> list = this.defs;
				Thing thing2 = ritual.selectedTarget.Thing;
				return list.Contains((thing2 != null) ? thing2.def : null);
			}
			return false;
		}

		// Token: 0x06009225 RID: 37413 RVA: 0x00356298 File Offset: 0x00354498
		protected bool CheckIdeoCorrectness(Precept_Ritual ritual, Thing thing)
		{
			Ideo ideo = (ritual != null) ? ritual.ideo : null;
			return thing.StyleSourcePrecept == null || (ideo != null && thing.StyleSourcePrecept.ideo == ideo);
		}

		// Token: 0x06009226 RID: 37414 RVA: 0x003562D0 File Offset: 0x003544D0
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			if (this.autoApplyInClassicMode && Find.IdeoManager.classicMode)
			{
				return new QualityFactor
				{
					quality = this.qualityOffset
				};
			}
			Thing thing = ritualTarget.Thing;
			if (thing == null)
			{
				return null;
			}
			bool flag = this.CheckIdeoCorrectness(ritual, thing) && ((this.allowAltars && thing.def.isAltar) || (!this.defs.NullOrEmpty<ThingDef>() && this.defs.Contains(ritualTarget.Thing.def)));
			TaggedString taggedString = "RitualOutcomeCompTip_RitualTargetDefsPart1".Translate(ritual.LabelCap, this.expectedThingLabelTip);
			if (!flag)
			{
				taggedString += "\n\n" + "RitualOutcomeCompTip_RitualTargetDefsPart2".Translate(ritual.LabelCap, thing);
			}
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				qualityChange = this.ExpectedOffsetDesc(flag, -1f),
				positive = flag,
				present = flag,
				quality = (flag ? this.qualityOffset : 0f),
				priority = 2f,
				toolTip = taggedString
			};
		}

		// Token: 0x06009227 RID: 37415 RVA: 0x00356410 File Offset: 0x00354610
		public override IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
		{
			RitualOutcomeComp_RitualTargetDefs.<BlockingIssues>d__8 <BlockingIssues>d__ = new RitualOutcomeComp_RitualTargetDefs.<BlockingIssues>d__8(-2);
			<BlockingIssues>d__.<>4__this = this;
			<BlockingIssues>d__.<>3__target = target;
			return <BlockingIssues>d__;
		}

		// Token: 0x06009228 RID: 37416 RVA: 0x00356427 File Offset: 0x00354627
		protected override string ExpectedOffsetDesc(bool positive, float quality = -1f)
		{
			quality = ((quality == -1f) ? this.qualityOffset : quality);
			if (!positive)
			{
				return "";
			}
			return quality.ToStringWithSign("0.#%");
		}

		// Token: 0x0400588F RID: 22671
		[MustTranslate]
		public string expectedThingLabelTip;

		// Token: 0x04005890 RID: 22672
		public List<ThingDef> defs;

		// Token: 0x04005891 RID: 22673
		public bool allowAltars;

		// Token: 0x04005892 RID: 22674
		public bool autoPoweredNeedsToBeOn;

		// Token: 0x04005893 RID: 22675
		public bool autoApplyInClassicMode;
	}
}
