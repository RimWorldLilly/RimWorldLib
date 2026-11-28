using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000125 RID: 293
	public interface ICreepJoinerDef
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060009BC RID: 2492
		float Weight { get; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060009BD RID: 2493
		float MinCombatPoints { get; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060009BE RID: 2494
		bool CanOccurRandomly { get; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060009BF RID: 2495
		List<CreepJoinerBaseDef> Excludes { get; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060009C0 RID: 2496
		List<CreepJoinerBaseDef> Requires { get; }
	}
}
