using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200160C RID: 5644
	public class GoodwillSituationWorker_MemeCompatibility : GoodwillSituationWorker
	{
		// Token: 0x06008CB0 RID: 36016 RVA: 0x0033204C File Offset: 0x0033024C
		public override string GetPostProcessedLabel(Faction other)
		{
			if (this.def.otherMeme != null)
			{
				return base.GetPostProcessedLabel(other);
			}
			if (this.Applies(Faction.OfPlayer, other))
			{
				return "MemeGoodwillImpact_Player".Translate(base.GetPostProcessedLabel(other));
			}
			return "MemeGoodwillImpact_Other".Translate(base.GetPostProcessedLabel(other));
		}

		// Token: 0x06008CB1 RID: 36017 RVA: 0x003320B3 File Offset: 0x003302B3
		public override int GetNaturalGoodwillOffset(Faction other)
		{
			if (!this.Applies(other))
			{
				return 0;
			}
			return this.def.naturalGoodwillOffset;
		}

		// Token: 0x06008CB2 RID: 36018 RVA: 0x003320CB File Offset: 0x003302CB
		private bool Applies(Faction other)
		{
			return this.Applies(Faction.OfPlayer, other) || this.Applies(other, Faction.OfPlayer);
		}

		// Token: 0x06008CB3 RID: 36019 RVA: 0x003320EC File Offset: 0x003302EC
		private bool Applies(Faction a, Faction b)
		{
			Ideo primaryIdeo = a.ideos.PrimaryIdeo;
			if (primaryIdeo == null)
			{
				return false;
			}
			if (this.def.versusAll)
			{
				return primaryIdeo.memes.Contains(this.def.meme);
			}
			Ideo primaryIdeo2 = b.ideos.PrimaryIdeo;
			return primaryIdeo2 != null && primaryIdeo.memes.Contains(this.def.meme) && primaryIdeo2.memes.Contains(this.def.otherMeme);
		}
	}
}
