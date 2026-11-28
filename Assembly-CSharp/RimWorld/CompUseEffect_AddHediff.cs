using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A2F RID: 6703
	public class CompUseEffect_AddHediff : CompUseEffect
	{
		// Token: 0x17001D84 RID: 7556
		// (get) Token: 0x0600A841 RID: 43073 RVA: 0x003BCC0B File Offset: 0x003BAE0B
		public CompProperties_UseEffectAddHediff Props
		{
			get
			{
				return (CompProperties_UseEffectAddHediff)this.props;
			}
		}

		// Token: 0x0600A842 RID: 43074 RVA: 0x003BCC18 File Offset: 0x003BAE18
		public override void DoEffect(Pawn user)
		{
			user.health.AddHediff(this.Props.hediffDef, null, null, null);
		}

		// Token: 0x0600A843 RID: 43075 RVA: 0x003BCC48 File Offset: 0x003BAE48
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (!this.Props.allowRepeatedUse && p.health.hediffSet.HasHediff(this.Props.hediffDef, false))
			{
				return "AlreadyHasHediff".Translate(this.Props.hediffDef.label);
			}
			return true;
		}
	}
}
