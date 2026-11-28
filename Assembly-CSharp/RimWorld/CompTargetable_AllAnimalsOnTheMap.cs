using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A04 RID: 6660
	public class CompTargetable_AllAnimalsOnTheMap : CompTargetable_AllPawnsOnTheMap
	{
		// Token: 0x0600A7BA RID: 42938 RVA: 0x003BB7A4 File Offset: 0x003B99A4
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = target.Thing as Pawn;
			return pawn != null && pawn.IsAnimal && base.ValidateTarget(target.Thing, showMessages);
		}
	}
}
