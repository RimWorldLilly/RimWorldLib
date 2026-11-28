using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B5 RID: 6069
	public static class PlayerPawnsArriveMethodExtension
	{
		// Token: 0x06009746 RID: 38726 RVA: 0x0036E058 File Offset: 0x0036C258
		public static string ToStringHuman(this PlayerPawnsArriveMethod method)
		{
			switch (method)
			{
			case PlayerPawnsArriveMethod.Standing:
				return "PlayerPawnsArriveMethod_Standing".Translate();
			case PlayerPawnsArriveMethod.DropPods:
				return "PlayerPawnsArriveMethod_DropPods".Translate();
			case PlayerPawnsArriveMethod.Gravship:
				return "PlayerPawnsArriveMethod_Gravship".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
