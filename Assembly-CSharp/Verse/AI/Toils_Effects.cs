using System;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000840 RID: 2112
	public static class Toils_Effects
	{
		// Token: 0x0600427C RID: 17020 RVA: 0x001A3B54 File Offset: 0x001A1D54
		public static Toil MakeSound(SoundDef soundDef)
		{
			Toil toil = ToilMaker.MakeToil("MakeSound");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				soundDef.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
			};
			return toil;
		}
	}
}
