using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E9 RID: 6377
	public class CompProperties_MechCarrier : CompProperties
	{
		// Token: 0x06009FD3 RID: 40915 RVA: 0x00394450 File Offset: 0x00392650
		public CompProperties_MechCarrier()
		{
			this.compClass = typeof(CompMechCarrier);
		}

		// Token: 0x06009FD4 RID: 40916 RVA: 0x0039447A File Offset: 0x0039267A
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_MechCarrier.<ConfigErrors>d__12 <ConfigErrors>d__ = new CompProperties_MechCarrier.<ConfigErrors>d__12(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005E7D RID: 24189
		public ThingDef fixedIngredient;

		// Token: 0x04005E7E RID: 24190
		public int costPerPawn;

		// Token: 0x04005E7F RID: 24191
		public int maxIngredientCount;

		// Token: 0x04005E80 RID: 24192
		public int startingIngredientCount;

		// Token: 0x04005E81 RID: 24193
		public PawnKindDef spawnPawnKind;

		// Token: 0x04005E82 RID: 24194
		public int cooldownTicks = 900;

		// Token: 0x04005E83 RID: 24195
		public int maxPawnsToSpawn = 3;

		// Token: 0x04005E84 RID: 24196
		public EffecterDef spawnEffecter;

		// Token: 0x04005E85 RID: 24197
		public EffecterDef spawnedMechEffecter;

		// Token: 0x04005E86 RID: 24198
		public bool attachSpawnedEffecter;

		// Token: 0x04005E87 RID: 24199
		public bool attachSpawnedMechEffecter;
	}
}
