using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200171E RID: 5918
	public class RitualStageAction_ClearTag : RitualStageAction
	{
		// Token: 0x060093B9 RID: 37817 RVA: 0x0035E4F0 File Offset: 0x0035C6F0
		public override void Apply(LordJob_Ritual ritual)
		{
			Pawn key = ritual.PawnWithRole(this.roleId);
			PawnTags pawnTags;
			if (ritual.perPawnTags.TryGetValue(key, out pawnTags))
			{
				List<string> tags = pawnTags.tags;
				if (tags == null)
				{
					return;
				}
				tags.Remove(this.tag);
			}
		}

		// Token: 0x060093BA RID: 37818 RVA: 0x0035E531 File Offset: 0x0035C731
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
		}

		// Token: 0x0400594B RID: 22859
		[NoTranslate]
		public string roleId;

		// Token: 0x0400594C RID: 22860
		[NoTranslate]
		public string tag;
	}
}
