using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000436 RID: 1078
	public class HediffComp_GiveAbility : HediffComp
	{
		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x000BE37F File Offset: 0x000BC57F
		private HediffCompProperties_GiveAbility Props
		{
			get
			{
				return (HediffCompProperties_GiveAbility)this.props;
			}
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x000BE38C File Offset: 0x000BC58C
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.Props.abilityDef != null)
			{
				this.parent.pawn.abilities.GainAbility(this.Props.abilityDef);
			}
			if (!this.Props.abilityDefs.NullOrEmpty<AbilityDef>())
			{
				for (int i = 0; i < this.Props.abilityDefs.Count; i++)
				{
					this.parent.pawn.abilities.GainAbility(this.Props.abilityDefs[i]);
				}
			}
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x000BE41C File Offset: 0x000BC61C
		public override void CompPostPostRemoved()
		{
			if (this.Props.abilityDef != null)
			{
				this.parent.pawn.abilities.RemoveAbility(this.Props.abilityDef);
			}
			if (!this.Props.abilityDefs.NullOrEmpty<AbilityDef>())
			{
				for (int i = 0; i < this.Props.abilityDefs.Count; i++)
				{
					this.parent.pawn.abilities.RemoveAbility(this.Props.abilityDefs[i]);
				}
			}
		}
	}
}
