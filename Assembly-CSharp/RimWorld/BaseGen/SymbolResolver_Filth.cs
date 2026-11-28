using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002103 RID: 8451
	public class SymbolResolver_Filth : SymbolResolver
	{
		// Token: 0x0600D449 RID: 54345 RVA: 0x004E7844 File Offset: 0x004E5A44
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.filthDef != null && rp.filthDensity != null;
		}

		// Token: 0x0600D44A RID: 54346 RVA: 0x004E7868 File Offset: 0x004E5A68
		public override void Resolve(ResolveParams rp)
		{
			foreach (IntVec3 intVec in rp.rect)
			{
				if (this.CanPlaceFilth(intVec, rp))
				{
					float num;
					for (num = rp.filthDensity.Value.RandomInRange; num > 1f; num -= 1f)
					{
						FilthMaker.TryMakeFilth(intVec, BaseGen.globalSettings.map, rp.filthDef, 1, FilthSourceFlags.None, true);
					}
					if (Rand.Chance(num))
					{
						FilthMaker.TryMakeFilth(intVec, BaseGen.globalSettings.map, rp.filthDef, 1, FilthSourceFlags.None, true);
					}
				}
			}
		}

		// Token: 0x0600D44B RID: 54347 RVA: 0x004E7924 File Offset: 0x004E5B24
		private bool CanPlaceFilth(IntVec3 cell, ResolveParams rp)
		{
			return rp.ignoreDoorways == null || !rp.ignoreDoorways.Value || cell.GetDoor(BaseGen.globalSettings.map) == null;
		}
	}
}
