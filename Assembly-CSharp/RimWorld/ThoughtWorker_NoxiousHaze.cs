using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E71 RID: 3697
	public class ThoughtWorker_NoxiousHaze : ThoughtWorker
	{
		// Token: 0x06006204 RID: 25092 RVA: 0x00234A47 File Offset: 0x00232C47
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (NoxiousHazeUtility.IsExposedToNoxiousHaze(p) && this.AppliesTo(p))
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x06006205 RID: 25093 RVA: 0x00234A68 File Offset: 0x00232C68
		private bool AppliesTo(Pawn pawn)
		{
			return ThoughtUtility.NullifyingGene(this.def, pawn) != null || (pawn.GetStatValue(StatDefOf.ToxicResistance, true, -1) < 1f && pawn.GetStatValue(StatDefOf.ToxicEnvironmentResistance, true, -1) < 1f && !pawn.kindDef.immuneToGameConditionEffects);
		}
	}
}
