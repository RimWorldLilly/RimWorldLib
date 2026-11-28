using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000739 RID: 1849
	public class Verb_AbilityShoot : Verb_Shoot, IAbilityVerb
	{
		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06003AAB RID: 15019 RVA: 0x00176059 File Offset: 0x00174259
		// (set) Token: 0x06003AAC RID: 15020 RVA: 0x00176061 File Offset: 0x00174261
		public Ability Ability
		{
			get
			{
				return this.ability;
			}
			set
			{
				this.ability = value;
			}
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x0017606A File Offset: 0x0017426A
		protected override bool TryCastShot()
		{
			bool flag = base.TryCastShot();
			if (flag)
			{
				this.ability.StartCooldown(this.ability.def.cooldownTicksRange.RandomInRange);
			}
			return flag;
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x00176095 File Offset: 0x00174295
		public override void ExposeData()
		{
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
			base.ExposeData();
		}

		// Token: 0x0400269E RID: 9886
		private Ability ability;
	}
}
