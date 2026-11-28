using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017AF RID: 6063
	public class ScenPart_PawnFilter_Age : ScenPart
	{
		// Token: 0x06009713 RID: 38675 RVA: 0x0036CCAB File Offset: 0x0036AEAB
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.IntRange(listing.GetScenPartRect(this, 31f), (int)listing.CurHeight, ref this.allowedAgeRange, 15, 120, null, 4);
		}

		// Token: 0x06009714 RID: 38676 RVA: 0x0036CCD4 File Offset: 0x0036AED4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.allowedAgeRange, "allowedAgeRange", default(IntRange), false);
		}

		// Token: 0x06009715 RID: 38677 RVA: 0x0036CD04 File Offset: 0x0036AF04
		public override string Summary(Scenario scen)
		{
			if (this.allowedAgeRange.min > 15)
			{
				if (this.allowedAgeRange.max < 10000)
				{
					return "ScenPart_StartingPawnAgeRange".Translate(this.allowedAgeRange.min, this.allowedAgeRange.max);
				}
				return "ScenPart_StartingPawnAgeMin".Translate(this.allowedAgeRange.min);
			}
			else
			{
				if (this.allowedAgeRange.max < 10000)
				{
					return "ScenPart_StartingPawnAgeMax".Translate(this.allowedAgeRange.max);
				}
				throw new Exception();
			}
		}

		// Token: 0x06009716 RID: 38678 RVA: 0x0036CDB9 File Offset: 0x0036AFB9
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return this.allowedAgeRange.Includes(pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x06009717 RID: 38679 RVA: 0x0036CDD4 File Offset: 0x0036AFD4
		public override void Randomize()
		{
			this.allowedAgeRange = new IntRange(15, 120);
			switch (Rand.RangeInclusive(0, 2))
			{
			case 0:
				this.allowedAgeRange.min = Rand.Range(20, 60);
				break;
			case 1:
				this.allowedAgeRange.max = Rand.Range(20, 60);
				break;
			case 2:
				this.allowedAgeRange.min = Rand.Range(20, 60);
				this.allowedAgeRange.max = Rand.Range(20, 60);
				break;
			}
			this.MakeAllowedAgeRangeValid();
		}

		// Token: 0x06009718 RID: 38680 RVA: 0x0036CE68 File Offset: 0x0036B068
		private void MakeAllowedAgeRangeValid()
		{
			if (this.allowedAgeRange.max < 19)
			{
				this.allowedAgeRange.max = 19;
			}
			if (this.allowedAgeRange.max - this.allowedAgeRange.min < 4)
			{
				this.allowedAgeRange.min = this.allowedAgeRange.max - 4;
			}
		}

		// Token: 0x06009719 RID: 38681 RVA: 0x0036CEC3 File Offset: 0x0036B0C3
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.allowedAgeRange.GetHashCode();
		}

		// Token: 0x04005A86 RID: 23174
		public IntRange allowedAgeRange = new IntRange(0, 999999);

		// Token: 0x04005A87 RID: 23175
		private const int RangeMin = 15;

		// Token: 0x04005A88 RID: 23176
		private const int RangeMax = 120;

		// Token: 0x04005A89 RID: 23177
		private const int RangeMinMax = 19;

		// Token: 0x04005A8A RID: 23178
		private const int RangeMinWidth = 4;
	}
}
