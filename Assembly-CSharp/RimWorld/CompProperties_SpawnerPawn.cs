using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001988 RID: 6536
	public class CompProperties_SpawnerPawn : CompProperties
	{
		// Token: 0x0600A419 RID: 42009 RVA: 0x003A9F30 File Offset: 0x003A8130
		public CompProperties_SpawnerPawn()
		{
			this.compClass = typeof(CompSpawnerPawn);
		}

		// Token: 0x040060F3 RID: 24819
		public List<PawnKindDef> spawnablePawnKinds;

		// Token: 0x040060F4 RID: 24820
		public SoundDef spawnSound;

		// Token: 0x040060F5 RID: 24821
		[NoTranslate]
		public string spawnMessageKey;

		// Token: 0x040060F6 RID: 24822
		[NoTranslate]
		public string noPawnsLeftToSpawnKey;

		// Token: 0x040060F7 RID: 24823
		[NoTranslate]
		public string pawnsLeftToSpawnKey;

		// Token: 0x040060F8 RID: 24824
		public bool showNextSpawnInInspect;

		// Token: 0x040060F9 RID: 24825
		public bool shouldJoinParentLord;

		// Token: 0x040060FA RID: 24826
		public Type lordJob;

		// Token: 0x040060FB RID: 24827
		public float defendRadius = 21f;

		// Token: 0x040060FC RID: 24828
		public float lordJoinRadius = 2.1474836E+09f;

		// Token: 0x040060FD RID: 24829
		public int initialPawnsCount;

		// Token: 0x040060FE RID: 24830
		public float initialPawnsPoints;

		// Token: 0x040060FF RID: 24831
		public float maxSpawnedPawnsPoints = -1f;

		// Token: 0x04006100 RID: 24832
		public FloatRange pawnSpawnIntervalDays = new FloatRange(0.85f, 1.15f);

		// Token: 0x04006101 RID: 24833
		public int pawnSpawnRadius = 2;

		// Token: 0x04006102 RID: 24834
		public IntRange maxPawnsToSpawn = IntRange.Zero;

		// Token: 0x04006103 RID: 24835
		public bool chooseSingleTypeToSpawn;

		// Token: 0x04006104 RID: 24836
		[NoTranslate]
		public string nextSpawnInspectStringKey;

		// Token: 0x04006105 RID: 24837
		[NoTranslate]
		public string nextSpawnInspectStringKeyDormant;
	}
}
