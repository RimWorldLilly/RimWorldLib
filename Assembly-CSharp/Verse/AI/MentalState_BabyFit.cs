using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x0200087A RID: 2170
	public abstract class MentalState_BabyFit : MentalState
	{
		// Token: 0x060043C6 RID: 17350
		protected abstract void AuraEffect(Thing source, Pawn hearer);

		// Token: 0x060043C7 RID: 17351 RVA: 0x001AA2BE File Offset: 0x001A84BE
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Normal;
		}

		// Token: 0x060043C8 RID: 17352 RVA: 0x001AA2C4 File Offset: 0x001A84C4
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if ((float)Find.TickManager.TicksGame <= this.lastScreamTick + 150f)
			{
				return;
			}
			if (this.pawn.IsWorldPawn())
			{
				return;
			}
			Caravan caravan = this.pawn.GetCaravan();
			if (caravan != null)
			{
				using (List<Pawn>.Enumerator enumerator = caravan.PawnsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn hearer = enumerator.Current;
						this.DoPawnHear(this.pawn, hearer);
					}
					goto IL_96;
				}
			}
			GenClamor.DoClamor(this.pawn, 9.9f, new GenClamor.ClamorEffect(this.DoPawnHear));
			IL_96:
			this.lastScreamTick = (float)Find.TickManager.TicksGame;
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x001AA388 File Offset: 0x001A8588
		private void DoPawnHear(Thing source, Pawn hearer)
		{
			if (hearer == source)
			{
				return;
			}
			if (this.alreadyHeard.Contains(hearer))
			{
				return;
			}
			this.alreadyHeard.Add(hearer);
			this.AuraEffect(source, hearer);
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x001AA3B2 File Offset: 0x001A85B2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.lastScreamTick, "lastScreamTick", 0f, false);
			Scribe_Collections.Look<Pawn>(ref this.alreadyHeard, "alreadyHeard", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04002C15 RID: 11285
		public const float BabyScreamRadius = 9.9f;

		// Token: 0x04002C16 RID: 11286
		public const int ScreamInterval = 150;

		// Token: 0x04002C17 RID: 11287
		private float lastScreamTick = -1f;

		// Token: 0x04002C18 RID: 11288
		private List<Pawn> alreadyHeard = new List<Pawn>(32);
	}
}
