using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001746 RID: 5958
	public class CompProperties_RitualEffectSpawnOnPawn : CompProperties_RitualEffectIntervalSpawn
	{
		// Token: 0x0400598D RID: 22925
		public Vector3 westRotationOffset;

		// Token: 0x0400598E RID: 22926
		public Vector3 eastRotationOffset;

		// Token: 0x0400598F RID: 22927
		public Vector3 northRotationOffset;

		// Token: 0x04005990 RID: 22928
		public Vector3 southRotationOffset;

		// Token: 0x04005991 RID: 22929
		[NoTranslate]
		public string requiredTag;
	}
}
