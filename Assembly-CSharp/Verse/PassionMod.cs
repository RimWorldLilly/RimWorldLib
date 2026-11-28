using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000BC RID: 188
	public class PassionMod
	{
		// Token: 0x06000770 RID: 1904 RVA: 0x0002B9A5 File Offset: 0x00029BA5
		public PassionMod()
		{
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0002B9AD File Offset: 0x00029BAD
		public PassionMod(SkillDef skill, PassionMod.PassionModType modType)
		{
			this.skill = skill;
			this.modType = modType;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0002B9C4 File Offset: 0x00029BC4
		public Passion NewPassionFor(SkillRecord skillRecord)
		{
			PassionMod.PassionModType passionModType = this.modType;
			if (passionModType != PassionMod.PassionModType.AddOneLevel)
			{
				if (passionModType == PassionMod.PassionModType.DropAll)
				{
					return Passion.None;
				}
			}
			else
			{
				Passion passion = skillRecord.passion;
				if (passion == Passion.None)
				{
					return Passion.Minor;
				}
				if (passion == Passion.Minor)
				{
					return Passion.Major;
				}
			}
			return skillRecord.passion;
		}

		// Token: 0x0400033B RID: 827
		public SkillDef skill;

		// Token: 0x0400033C RID: 828
		public PassionMod.PassionModType modType;

		// Token: 0x020024E9 RID: 9449
		public enum PassionModType
		{
			// Token: 0x0400970D RID: 38669
			None,
			// Token: 0x0400970E RID: 38670
			AddOneLevel,
			// Token: 0x0400970F RID: 38671
			DropAll
		}
	}
}
