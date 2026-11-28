using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200192B RID: 6443
	public class CompProperties_PawnSpawnOnWakeup : CompProperties
	{
		// Token: 0x0600A1A8 RID: 41384 RVA: 0x0039C738 File Offset: 0x0039A938
		public CompProperties_PawnSpawnOnWakeup()
		{
			this.compClass = typeof(CompPawnSpawnOnWakeup);
		}

		// Token: 0x04005F80 RID: 24448
		public List<PawnKindDef> spawnablePawnKinds;

		// Token: 0x04005F81 RID: 24449
		public SoundDef spawnSound;

		// Token: 0x04005F82 RID: 24450
		public EffecterDef spawnEffecter;

		// Token: 0x04005F83 RID: 24451
		public Type lordJob;

		// Token: 0x04005F84 RID: 24452
		public bool shouldJoinParentLord;

		// Token: 0x04005F85 RID: 24453
		[NoTranslate]
		public string activatedMessageKey;

		// Token: 0x04005F86 RID: 24454
		public FloatRange points;

		// Token: 0x04005F87 RID: 24455
		public IntRange pawnSpawnRadius = new IntRange(2, 2);

		// Token: 0x04005F88 RID: 24456
		public bool aggressive = true;

		// Token: 0x04005F89 RID: 24457
		public bool dropInPods;

		// Token: 0x04005F8A RID: 24458
		public float defendRadius = 21f;

		// Token: 0x04005F8B RID: 24459
		public MentalStateDef mentalState;

		// Token: 0x04005F8C RID: 24460
		public bool destroyAfterSpawn;
	}
}
