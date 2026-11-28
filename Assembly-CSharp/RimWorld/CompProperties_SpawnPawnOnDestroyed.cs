using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200198B RID: 6539
	public class CompProperties_SpawnPawnOnDestroyed : CompProperties
	{
		// Token: 0x0600A435 RID: 42037 RVA: 0x003AAAA5 File Offset: 0x003A8CA5
		public CompProperties_SpawnPawnOnDestroyed()
		{
			this.compClass = typeof(CompSpawnPawnOnDestroyed);
		}

		// Token: 0x0600A436 RID: 42038 RVA: 0x003AAABD File Offset: 0x003A8CBD
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_SpawnPawnOnDestroyed.<ConfigErrors>d__3 <ConfigErrors>d__ = new CompProperties_SpawnPawnOnDestroyed.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400610E RID: 24846
		public PawnKindDef pawnKind;

		// Token: 0x0400610F RID: 24847
		public Type lordJob;
	}
}
