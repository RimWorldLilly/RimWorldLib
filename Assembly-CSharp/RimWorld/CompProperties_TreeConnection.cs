using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C9 RID: 6601
	public class CompProperties_TreeConnection : CompProperties
	{
		// Token: 0x0600A5D2 RID: 42450 RVA: 0x003B2114 File Offset: 0x003B0314
		public CompProperties_TreeConnection()
		{
			this.compClass = typeof(CompTreeConnection);
		}

		// Token: 0x040061D9 RID: 25049
		public float spawnDays = 6f;

		// Token: 0x040061DA RID: 25050
		public PawnKindDef pawnKind;

		// Token: 0x040061DB RID: 25051
		public FloatRange initialConnectionStrengthRange;

		// Token: 0x040061DC RID: 25052
		public float connectionStrengthLossPerDryadDeath = 0.1f;

		// Token: 0x040061DD RID: 25053
		public float radiusToBuildingForConnectionStrengthLoss = 7.9f;

		// Token: 0x040061DE RID: 25054
		public int maxDryadsWild;

		// Token: 0x040061DF RID: 25055
		public SimpleCurve maxDryadsPerConnectionStrengthCurve;

		// Token: 0x040061E0 RID: 25056
		public SimpleCurve connectionLossPerLevelCurve;

		// Token: 0x040061E1 RID: 25057
		public SimpleCurve connectionLossDailyPerBuildingDistanceCurve;

		// Token: 0x040061E2 RID: 25058
		public SimpleCurve connectionStrengthGainPerPlantSkill;

		// Token: 0x040061E3 RID: 25059
		public float connectionStrengthGainPerHourPruningBase = 0.01f;

		// Token: 0x040061E4 RID: 25060
		public Vector3 spawningPodOffset;

		// Token: 0x040061E5 RID: 25061
		public FloatRange spawningPodSizeRange = FloatRange.One;
	}
}
