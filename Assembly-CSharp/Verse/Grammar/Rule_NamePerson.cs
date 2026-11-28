using System;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x020007B1 RID: 1969
	public class Rule_NamePerson : Rule
	{
		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06003F7D RID: 16253 RVA: 0x00196A6B File Offset: 0x00194C6B
		public override float BaseSelectionWeight
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x00196A72 File Offset: 0x00194C72
		public override Rule DeepCopy()
		{
			Rule_NamePerson rule_NamePerson = (Rule_NamePerson)base.DeepCopy();
			rule_NamePerson.gender = this.gender;
			return rule_NamePerson;
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x00196A8C File Offset: 0x00194C8C
		public override string Generate()
		{
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard);
			Gender gender = this.gender;
			if (gender == Gender.None)
			{
				gender = ((Rand.Value < 0.5f) ? Gender.Male : Gender.Female);
			}
			return nameBank.GetName(PawnNameSlot.First, gender, false);
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x00196AC2 File Offset: 0x00194CC2
		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}

		// Token: 0x040029FF RID: 10751
		public Gender gender;
	}
}
