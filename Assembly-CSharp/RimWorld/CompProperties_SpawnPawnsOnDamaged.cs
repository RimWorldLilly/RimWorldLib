using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200198D RID: 6541
	public class CompProperties_SpawnPawnsOnDamaged : CompProperties
	{
		// Token: 0x0600A43C RID: 42044 RVA: 0x003AACE1 File Offset: 0x003A8EE1
		public CompProperties_SpawnPawnsOnDamaged()
		{
			this.compClass = typeof(CompSpawnPawnsOnDamaged);
		}

		// Token: 0x04006111 RID: 24849
		public float chance = 1f;

		// Token: 0x04006112 RID: 24850
		public int cooldownTicks;

		// Token: 0x04006113 RID: 24851
		public IntRange spawnPawnCountRange = new IntRange(1, 1);

		// Token: 0x04006114 RID: 24852
		public List<PawnKindDef> spawnPawnKindOptions = new List<PawnKindDef>();
	}
}
