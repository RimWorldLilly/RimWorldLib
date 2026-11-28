using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000873 RID: 2163
	public class MentalState_CocoonDisturbed : MentalState
	{
		// Token: 0x060043AB RID: 17323 RVA: 0x001A9ECC File Offset: 0x001A80CC
		public override bool ForceHostileTo(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (pawn.RaceProps.Insect)
				{
					return false;
				}
				if (pawn.RaceProps.Animal && (pawn.Roamer || pawn.Faction == null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x001A9F12 File Offset: 0x001A8112
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x001A9F15 File Offset: 0x001A8115
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
