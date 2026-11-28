using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020001A5 RID: 421
	public class SkillRange
	{
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0003E3FB File Offset: 0x0003C5FB
		public SkillDef Skill
		{
			get
			{
				return this.skill;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x0003E403 File Offset: 0x0003C603
		public IntRange Range
		{
			get
			{
				return this.range;
			}
		}

		// Token: 0x04000AD3 RID: 2771
		private SkillDef skill;

		// Token: 0x04000AD4 RID: 2772
		private IntRange range = IntRange.One;
	}
}
