using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A41 RID: 6721
	public class CompUseEffect_LearnSkill : CompUseEffect
	{
		// Token: 0x17001D8D RID: 7565
		// (get) Token: 0x0600A87E RID: 43134 RVA: 0x003BDC3A File Offset: 0x003BBE3A
		public CompProperties_UseEffect_LearnSkill Props
		{
			get
			{
				return (CompProperties_UseEffect_LearnSkill)this.props;
			}
		}

		// Token: 0x0600A87F RID: 43135 RVA: 0x003BDC48 File Offset: 0x003BBE48
		public override void DoEffect(Pawn user)
		{
			base.DoEffect(user);
			int level = user.skills.GetSkill(this.Props.skill).GetLevel(true);
			user.skills.Learn(this.Props.skill, this.Props.xpGainAmount, true, false);
			int level2 = user.skills.GetSkill(this.Props.skill).GetLevel(true);
			if (PawnUtility.ShouldSendNotificationAbout(user))
			{
				Messages.Message("SkillNeurotrainerUsed".Translate(user.LabelShort, this.Props.skill.LabelCap, level, level2, user.Named("USER")), user, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x0600A880 RID: 43136 RVA: 0x003BDD18 File Offset: 0x003BBF18
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (p.skills == null)
			{
				return false;
			}
			if (p.skills.GetSkill(this.Props.skill).TotallyDisabled)
			{
				return "SkillDisabled".Translate();
			}
			return base.CanBeUsedBy(p);
		}

		// Token: 0x0600A881 RID: 43137 RVA: 0x003BDD68 File Offset: 0x003BBF68
		public override bool AllowStackWith(Thing other)
		{
			if (!base.AllowStackWith(other))
			{
				return false;
			}
			CompUseEffect_LearnSkill compUseEffect_LearnSkill = other.TryGetComp<CompUseEffect_LearnSkill>();
			return compUseEffect_LearnSkill != null && compUseEffect_LearnSkill.Props.skill == this.Props.skill;
		}
	}
}
