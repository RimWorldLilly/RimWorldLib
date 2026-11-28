using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F1 RID: 5873
	public abstract class RitualPosition : IExposable
	{
		// Token: 0x060092D8 RID: 37592
		public abstract PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual);

		// Token: 0x060092D9 RID: 37593 RVA: 0x0035AB65 File Offset: 0x00358D65
		public virtual bool CanUse(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x060092DA RID: 37594 RVA: 0x0035AB68 File Offset: 0x00358D68
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.highlight, "highlight", false, false);
		}

		// Token: 0x060092DB RID: 37595 RVA: 0x0035AB7C File Offset: 0x00358D7C
		protected virtual IntVec3 GetFallbackSpot(CellRect rect, IntVec3 spot, Pawn p, LordJob_Ritual ritual, Func<IntVec3, bool> Validator)
		{
			foreach (IntVec3 intVec in rect.AdjacentCells)
			{
				if (Validator(intVec))
				{
					return intVec;
				}
			}
			IntVec3 result;
			CellFinder.TryFindRandomCellNear(spot, p.Map, 3, new Predicate<IntVec3>(Validator.Invoke), out result, -1);
			return result;
		}

		// Token: 0x040058FE RID: 22782
		public bool highlight;
	}
}
