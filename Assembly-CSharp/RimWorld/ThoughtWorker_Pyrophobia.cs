using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E70 RID: 3696
	public class ThoughtWorker_Pyrophobia : ThoughtWorker
	{
		// Token: 0x06006201 RID: 25089 RVA: 0x00234982 File Offset: 0x00232B82
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (p.genes == null || !p.genes.HasActiveGene(GeneDefOf.FireTerror) || !ThoughtWorker_Pyrophobia.NearFire(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}

		// Token: 0x06006202 RID: 25090 RVA: 0x002349C0 File Offset: 0x00232BC0
		public static bool NearFire(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return false;
			}
			IntVec3 positionHeld = pawn.PositionHeld;
			int num = GenRadial.NumCellsInRadius(19.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(mapHeld) && !intVec.Fogged(mapHeld) && GenSight.LineOfSight(positionHeld, intVec, mapHeld, true, null, 0, 0) && intVec.ContainsStaticFire(mapHeld))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003615 RID: 13845
		public const float FireRadius = 19.9f;
	}
}
