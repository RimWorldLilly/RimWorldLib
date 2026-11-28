using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCC RID: 3532
	public class ThoughtWorker_Precept_OutdoorBlindingLight : ThoughtWorker_Precept
	{
		// Token: 0x06006069 RID: 24681 RVA: 0x0022F308 File Offset: 0x0022D508
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Awake() || PawnUtility.IsBiologicallyOrArtificiallyBlind(p))
			{
				return false;
			}
			return p.Map.glowGrid.PsychGlowAt(p.Position) == PsychGlow.Overlit && !p.Position.Roofed(p.Map);
		}
	}
}
