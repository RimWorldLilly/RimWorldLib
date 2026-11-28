using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB9 RID: 4025
	public abstract class PawnGroupKindWorker
	{
		// Token: 0x06006642 RID: 26178
		public abstract float MinPointsToGenerateAnything(PawnGroupMaker groupMaker, FactionDef faction, PawnGroupMakerParms parms = null);

		// Token: 0x06006643 RID: 26179 RVA: 0x00245618 File Offset: 0x00243818
		public List<Pawn> GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, bool errorOnZeroResults = true)
		{
			List<Pawn> list = new List<Pawn>();
			PawnGroupKindWorker.pawnsBeingGeneratedNow.Add(list);
			try
			{
				this.GeneratePawns(parms, groupMaker, list, errorOnZeroResults);
			}
			catch (Exception ex)
			{
				string str = "Exception while generating pawn group: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Destroy(DestroyMode.Vanish);
				}
				list.Clear();
			}
			finally
			{
				PawnGroupKindWorker.pawnsBeingGeneratedNow.Remove(list);
			}
			return list;
		}

		// Token: 0x06006644 RID: 26180
		protected abstract void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true);

		// Token: 0x06006645 RID: 26181 RVA: 0x002456B0 File Offset: 0x002438B0
		public virtual bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return true;
		}

		// Token: 0x06006646 RID: 26182
		public abstract IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker);

		// Token: 0x04003E9E RID: 16030
		public PawnGroupKindDef def;

		// Token: 0x04003E9F RID: 16031
		public static List<List<Pawn>> pawnsBeingGeneratedNow = new List<List<Pawn>>();
	}
}
