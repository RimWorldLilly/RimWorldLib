using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A43 RID: 6723
	public class CompUseEffect_OffsetDeathrestCapacity : CompUseEffect
	{
		// Token: 0x17001D8E RID: 7566
		// (get) Token: 0x0600A884 RID: 43140 RVA: 0x003BDDC5 File Offset: 0x003BBFC5
		private CompProperties_UseEffectOffsetDeathrestCapacity Props
		{
			get
			{
				return (CompProperties_UseEffectOffsetDeathrestCapacity)this.props;
			}
		}

		// Token: 0x0600A885 RID: 43141 RVA: 0x003BDDD2 File Offset: 0x003BBFD2
		public override void DoEffect(Pawn user)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			base.DoEffect(user);
			Pawn_GeneTracker genes = user.genes;
			if (genes == null)
			{
				return;
			}
			Gene_Deathrest firstGeneOfType = genes.GetFirstGeneOfType<Gene_Deathrest>();
			if (firstGeneOfType == null)
			{
				return;
			}
			firstGeneOfType.OffsetCapacity(this.Props.offset, true);
		}

		// Token: 0x0600A886 RID: 43142 RVA: 0x003BDE09 File Offset: 0x003BC009
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			Pawn_GeneTracker genes = p.genes;
			if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null) == null)
			{
				return "CannotDeathrest".Translate();
			}
			return base.CanBeUsedBy(p);
		}

		// Token: 0x0600A887 RID: 43143 RVA: 0x003BDE36 File Offset: 0x003BC036
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompUseEffect_OffsetDeathrestCapacity.<SpecialDisplayStats>d__4 <SpecialDisplayStats>d__ = new CompUseEffect_OffsetDeathrestCapacity.<SpecialDisplayStats>d__4(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}
	}
}
