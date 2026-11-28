using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F11 RID: 3857
	public class SkillNeed
	{
		// Token: 0x0600641D RID: 25629 RVA: 0x0023D4EC File Offset: 0x0023B6EC
		public virtual float ValueFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600641E RID: 25630 RVA: 0x0023D4F3 File Offset: 0x0023B6F3
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x04003939 RID: 14649
		public SkillDef skill;

		// Token: 0x0400393A RID: 14650
		public bool required = true;
	}
}
