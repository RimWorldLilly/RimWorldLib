using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000B4 RID: 180
	public class RitualStagePositions : IExposable
	{
		// Token: 0x06000755 RID: 1877 RVA: 0x0002A00C File Offset: 0x0002820C
		public void ExposeData()
		{
			Scribe_Collections.Look<PawnRitualReference, PawnStagePosition>(ref this.referencePositions, "referencePosition", LookMode.Deep, LookMode.Deep, ref this.pawnRefListTmp, ref this.positionListTmp, true, false, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				Scribe_Collections.Look<Pawn, PawnStagePosition>(ref this.positionsTmp, "positions", LookMode.Reference, LookMode.Deep, ref this.pawnListTmp, ref this.positionListTmp, true, false, false);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe_Collections.Look<Pawn, PawnStagePosition>(ref this.positionsTmp, "positions", LookMode.Reference, LookMode.Deep, ref this.pawnListTmp, ref this.positionListTmp, true, false, false);
				if (this.positionsTmp != null && this.positionsTmp.Count > 0)
				{
					if (this.referencePositions == null)
					{
						this.referencePositions = new Dictionary<PawnRitualReference, PawnStagePosition>();
					}
					using (Dictionary<Pawn, PawnStagePosition>.Enumerator enumerator = this.positionsTmp.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<Pawn, PawnStagePosition> pawnPos = enumerator.Current;
							if (this.referencePositions.All((KeyValuePair<PawnRitualReference, PawnStagePosition> p) => p.Key.pawn != pawnPos.Key))
							{
								this.referencePositions.Add(new PawnRitualReference(pawnPos.Key), pawnPos.Value);
							}
						}
					}
				}
			}
		}

		// Token: 0x040002C4 RID: 708
		public Dictionary<PawnRitualReference, PawnStagePosition> referencePositions = new Dictionary<PawnRitualReference, PawnStagePosition>();

		// Token: 0x040002C5 RID: 709
		private Dictionary<Pawn, PawnStagePosition> positionsTmp = new Dictionary<Pawn, PawnStagePosition>();

		// Token: 0x040002C6 RID: 710
		private List<Pawn> pawnListTmp;

		// Token: 0x040002C7 RID: 711
		private List<PawnRitualReference> pawnRefListTmp;

		// Token: 0x040002C8 RID: 712
		private List<PawnStagePosition> positionListTmp;
	}
}
