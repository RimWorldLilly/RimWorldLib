using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001734 RID: 5940
	public class CompRitualEffect_ConstantCenter : CompRitualEffect_Constant
	{
		// Token: 0x0600941E RID: 37918 RVA: 0x0036018C File Offset: 0x0035E38C
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.Cell.ToVector3Shifted();
		}
	}
}
