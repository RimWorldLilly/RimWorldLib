using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200145E RID: 5214
	public class CompAbilityEffect_TrainRandomSkill : CompAbilityEffect
	{
		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x0600801D RID: 32797 RVA: 0x002DB0F5 File Offset: 0x002D92F5
		public new CompProperties_AbilityTrainRandomSkill Props
		{
			get
			{
				return (CompProperties_AbilityTrainRandomSkill)this.props;
			}
		}

		// Token: 0x0600801E RID: 32798 RVA: 0x002DB104 File Offset: 0x002D9304
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			if (target.Pawn != null)
			{
				SkillDef skillDef = (from x in DefDatabase<SkillDef>.AllDefs
				where !target.Pawn.skills.GetSkill(x).TotallyDisabled
				select x).RandomElement<SkillDef>();
				int level = target.Pawn.skills.GetSkill(skillDef).GetLevel(false);
				target.Pawn.skills.Learn(skillDef, 50000f, true, false);
				if (base.SendLetter)
				{
					int value = target.Pawn.skills.GetSkill(skillDef).GetLevel(false) - level;
					Find.LetterStack.ReceiveLetter(this.Props.customLetterLabel.Formatted(skillDef.LabelCap), this.Props.customLetterText.Formatted(this.parent.pawn, target.Pawn, skillDef, value), LetterDefOf.PositiveEvent, new LookTargets(target.Pawn), null, null, null, null, 0, true);
				}
			}
		}

		// Token: 0x04005006 RID: 20486
		private const float XPGainAmount = 50000f;
	}
}
