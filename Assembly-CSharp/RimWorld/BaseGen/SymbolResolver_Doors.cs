using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020F9 RID: 8441
	public class SymbolResolver_Doors : SymbolResolver
	{
		// Token: 0x0600D420 RID: 54304 RVA: 0x004E5AE0 File Offset: 0x004E3CE0
		public override void Resolve(ResolveParams rp)
		{
			if (Rand.Chance(0.25f) || (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.8f)))
			{
				BaseGen.symbolStack.Push("extraDoor", rp, null);
			}
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
		}

		// Token: 0x040086C3 RID: 34499
		private const float ExtraDoorChance = 0.25f;
	}
}
