using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E5 RID: 4325
	public class QuestPart_RequirementsToAcceptFactionRelation : QuestPart_RequirementsToAccept
	{
		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06006C18 RID: 27672 RVA: 0x0025F4F8 File Offset: 0x0025D6F8
		public string ReasonText
		{
			get
			{
				switch (this.relationKind)
				{
				case FactionRelationKind.Hostile:
					return "QuestHostileTo".Translate(this.otherFaction);
				case FactionRelationKind.Neutral:
					return "QuestNeutralTo".Translate(this.otherFaction);
				case FactionRelationKind.Ally:
					return "QuestAlliedTo".Translate(this.otherFaction);
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown faction relation kind: ");
					defaultInterpolatedStringHandler.AppendFormatted<FactionRelationKind>(this.relationKind);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
			}
		}

		// Token: 0x06006C19 RID: 27673 RVA: 0x0025F5A0 File Offset: 0x0025D7A0
		public override AcceptanceReport CanAccept()
		{
			if (this.otherFaction != null && ((this.acceptIfDefeated && this.otherFaction.defeated) || Faction.OfPlayer.RelationKindWith(this.otherFaction) == this.relationKind))
			{
				return true;
			}
			return new AcceptanceReport(this.ReasonText);
		}

		// Token: 0x06006C1A RID: 27674 RVA: 0x0025F5F4 File Offset: 0x0025D7F4
		public override void Notify_FactionRemoved(Faction faction)
		{
			base.Notify_FactionRemoved(faction);
			if (this.otherFaction == faction)
			{
				this.otherFaction = null;
			}
		}

		// Token: 0x06006C1B RID: 27675 RVA: 0x0025F60D File Offset: 0x0025D80D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
			Scribe_References.Look<Faction>(ref this.otherFaction, "otherFaction", false);
			Scribe_Values.Look<bool>(ref this.acceptIfDefeated, "acceptIfDefeated", false, false);
		}

		// Token: 0x040044D7 RID: 17623
		public Faction otherFaction;

		// Token: 0x040044D8 RID: 17624
		public FactionRelationKind relationKind;

		// Token: 0x040044D9 RID: 17625
		public bool acceptIfDefeated;
	}
}
