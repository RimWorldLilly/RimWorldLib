using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF8 RID: 6904
	public class LavaEmergenceImmediate : LavaEmergence
	{
		// Token: 0x17001FD3 RID: 8147
		// (get) Token: 0x0600B078 RID: 45176 RVA: 0x003E2924 File Offset: 0x003E0B24
		protected override IntRange PoolSizeRange
		{
			get
			{
				return new IntRange(100, 150);
			}
		}

		// Token: 0x17001FD4 RID: 8148
		// (get) Token: 0x0600B079 RID: 45177 RVA: 0x003E2932 File Offset: 0x003E0B32
		protected override bool FireLetter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B07A RID: 45178 RVA: 0x003E2935 File Offset: 0x003E0B35
		protected override void Tick()
		{
			while (this.lavaCells.Count < this.poolSize && this.openCells.Count > 0)
			{
				base.SpreadLava();
			}
			base.BeginCooling();
		}

		// Token: 0x0600B07B RID: 45179 RVA: 0x003E2966 File Offset: 0x003E0B66
		protected override IEnumerable<IntVec3> GetInitialCells(Map map)
		{
			LavaEmergenceImmediate.<GetInitialCells>d__5 <GetInitialCells>d__ = new LavaEmergenceImmediate.<GetInitialCells>d__5(-2);
			<GetInitialCells>d__.<>4__this = this;
			return <GetInitialCells>d__;
		}
	}
}
