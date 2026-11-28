using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001013 RID: 4115
	public class WeaponTraitWorker_PsyfocusOnKill : WeaponTraitWorker
	{
		// Token: 0x060067B1 RID: 26545 RVA: 0x0024A53E File Offset: 0x0024873E
		public override void Notify_KilledPawn(Pawn pawn)
		{
			base.Notify_KilledPawn(pawn);
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(0.2f);
		}

		// Token: 0x040041F9 RID: 16889
		private const float PsyfocusOffset = 0.2f;
	}
}
