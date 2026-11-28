using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200167B RID: 5755
	public class RitualStage_OnTheThrone : RitualStage
	{
		// Token: 0x06009050 RID: 36944 RVA: 0x0034FBB6 File Offset: 0x0034DDB6
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.Cell.GetFirstThing(ritual.Map);
		}
	}
}
