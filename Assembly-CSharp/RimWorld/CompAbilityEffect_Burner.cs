using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001473 RID: 5235
	public class CompAbilityEffect_Burner : CompAbilityEffect
	{
		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x06008072 RID: 32882 RVA: 0x002DCD54 File Offset: 0x002DAF54
		public new CompProperties_AbilityBurner Props
		{
			get
			{
				return (CompProperties_AbilityBurner)this.props;
			}
		}

		// Token: 0x06008073 RID: 32883 RVA: 0x002DCD61 File Offset: 0x002DAF61
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			CompAbilityEffect_Burner.<GetPreCastActions>d__2 <GetPreCastActions>d__ = new CompAbilityEffect_Burner.<GetPreCastActions>d__2(-2);
			<GetPreCastActions>d__.<>4__this = this;
			return <GetPreCastActions>d__;
		}
	}
}
