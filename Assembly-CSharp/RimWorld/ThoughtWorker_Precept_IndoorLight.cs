using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCD RID: 3533
	public class ThoughtWorker_Precept_IndoorLight : ThoughtWorker_Precept
	{
		// Token: 0x0600606B RID: 24683 RVA: 0x0022F36C File Offset: 0x0022D56C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Awake() || PawnUtility.IsBiologicallyOrArtificiallyBlind(p))
			{
				return false;
			}
			return p.Map.glowGrid.PsychGlowAt(p.Position) != PsychGlow.Dark && p.Position.Roofed(p.Map) && !DarklightUtility.IsDarklightAt(p.Position, p.Map);
		}
	}
}
