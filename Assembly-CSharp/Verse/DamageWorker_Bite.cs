using System;

namespace Verse
{
	// Token: 0x020003E7 RID: 999
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		// Token: 0x06001D64 RID: 7524 RVA: 0x000B3DC6 File Offset: 0x000B1FC6
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside, null);
		}
	}
}
