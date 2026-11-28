using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001606 RID: 5638
	public class FactionRelation : IExposable
	{
		// Token: 0x06008C83 RID: 35971 RVA: 0x00330F62 File Offset: 0x0032F162
		public FactionRelation()
		{
		}

		// Token: 0x06008C84 RID: 35972 RVA: 0x00330F79 File Offset: 0x0032F179
		public FactionRelation(Faction other, FactionRelationKind kind)
		{
			this.other = other;
			this.kind = kind;
		}

		// Token: 0x06008C85 RID: 35973 RVA: 0x00330FA0 File Offset: 0x0032F1A0
		public void CheckKindThresholds(Faction faction, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter)
		{
			int num = faction.GoodwillWith(this.other);
			FactionRelationKind previousKind = this.kind;
			sentLetter = false;
			if (this.kind != FactionRelationKind.Hostile && num <= -75)
			{
				this.kind = FactionRelationKind.Hostile;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind != FactionRelationKind.Ally && num >= 75)
			{
				this.kind = FactionRelationKind.Ally;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Hostile && num >= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Ally && num <= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
		}

		// Token: 0x06008C86 RID: 35974 RVA: 0x00331061 File Offset: 0x0032F261
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.other, "other", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.kind, "kind", FactionRelationKind.Neutral, false);
			Scribe_Values.Look<int>(ref this.baseGoodwill, "goodwill", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06008C87 RID: 35975 RVA: 0x003310A0 File Offset: 0x0032F2A0
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			Faction faction = this.other;
			array[num] = ((faction != null) ? faction.ToString() : null);
			array[2] = ", kind=";
			array[3] = this.kind.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x04005636 RID: 22070
		public Faction other;

		// Token: 0x04005637 RID: 22071
		public int baseGoodwill = 100;

		// Token: 0x04005638 RID: 22072
		public FactionRelationKind kind = FactionRelationKind.Neutral;
	}
}
