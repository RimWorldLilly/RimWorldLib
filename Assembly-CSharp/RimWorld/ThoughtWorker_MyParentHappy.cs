using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E83 RID: 3715
	public class ThoughtWorker_MyParentHappy : ThoughtWorker
	{
		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x06006245 RID: 25157 RVA: 0x00235579 File Offset: 0x00233779
		protected virtual int RequiredParentCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06006246 RID: 25158 RVA: 0x0023557C File Offset: 0x0023377C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (p.Suspended)
			{
				return ThoughtState.Inactive;
			}
			if (!this.HasRequiredHappyChildren(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x06006247 RID: 25159 RVA: 0x002355AD File Offset: 0x002337AD
		protected virtual bool HasRequiredHappyChildren(Pawn p)
		{
			return this.HappyParentCount(p) == this.RequiredParentCount;
		}

		// Token: 0x06006248 RID: 25160 RVA: 0x002355C0 File Offset: 0x002337C0
		protected int HappyParentCount(Pawn pawn)
		{
			int num = 0;
			Pawn father = pawn.GetFather();
			Pawn mother = pawn.GetMother();
			if (father != null && ThoughtWorker_RelatedChildMoodBase.InSameMapOrCaravan(pawn, father) && ThoughtWorker_MyParentHappy.Counts(father))
			{
				num++;
			}
			if (mother != null && ThoughtWorker_RelatedChildMoodBase.InSameMapOrCaravan(pawn, mother) && ThoughtWorker_MyParentHappy.Counts(mother))
			{
				num++;
			}
			return num;
		}

		// Token: 0x06006249 RID: 25161 RVA: 0x00235610 File Offset: 0x00233810
		public static bool Counts(Pawn pawn)
		{
			Pawn_NeedsTracker needs = pawn.needs;
			return ((needs != null) ? needs.mood : null) != null && pawn.needs.mood.CurLevelPercentage > 0.6f && !(pawn.ParentHolder is Building_GrowthVat) && !(pawn.ParentHolder is Building_CryptosleepCasket);
		}

		// Token: 0x04003620 RID: 13856
		private const float HappyMoodThreshold = 0.6f;
	}
}
