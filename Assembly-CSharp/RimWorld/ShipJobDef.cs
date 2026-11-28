using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE2 RID: 4066
	public class ShipJobDef : Def
	{
		// Token: 0x060066D8 RID: 26328 RVA: 0x002479CB File Offset: 0x00245BCB
		public override IEnumerable<string> ConfigErrors()
		{
			ShipJobDef.<ConfigErrors>d__2 <ConfigErrors>d__ = new ShipJobDef.<ConfigErrors>d__2(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04004024 RID: 16420
		public Type jobClass;

		// Token: 0x04004025 RID: 16421
		public bool blocksDisposalIfQueuedUnspawned = true;
	}
}
