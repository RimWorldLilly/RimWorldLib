using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020F4 RID: 8436
	public class SymbolResolver_BasePart_Outdoors : SymbolResolver
	{
		// Token: 0x0600D411 RID: 54289 RVA: 0x004E5384 File Offset: 0x004E3584
		public override void Resolve(ResolveParams rp)
		{
			bool flag;
			if (BaseGen.globalSettings.basePart_worshippedTerminalsResolved < BaseGen.globalSettings.requiredWorshippedTerminalRooms)
			{
				flag = (rp.rect.Width >= 14 && rp.rect.Height >= 14);
			}
			else
			{
				flag = (rp.rect.Width > 23 || rp.rect.Height > 23 || ((rp.rect.Width >= 11 || rp.rect.Height >= 11) && Rand.Bool));
			}
			ResolveParams resolveParams = rp;
			resolveParams.pathwayFloorDef = (rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false));
			if (flag)
			{
				BaseGen.symbolStack.Push("basePart_outdoors_division", resolveParams, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_outdoors_leafPossiblyDecorated", resolveParams, null);
		}
	}
}
