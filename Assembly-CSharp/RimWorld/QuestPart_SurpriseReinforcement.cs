using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001102 RID: 4354
	public class QuestPart_SurpriseReinforcement : QuestPartActivable
	{
		// Token: 0x06006CAD RID: 27821 RVA: 0x00261DA0 File Offset: 0x0025FFA0
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			if (this.reinforcementChance == 0f)
			{
				return;
			}
			if (pawn.Faction == this.faction && pawn.MapHeld != null && pawn.MapHeld.Parent != null && pawn.MapHeld.Parent == this.mapParent)
			{
				if (Rand.Chance(this.reinforcementChance))
				{
					this.ticksTillRaid = (int)QuestPart_SurpriseReinforcement.RandomDelayRange.RandomInRange;
				}
				this.reinforcementChance = 0f;
			}
		}

		// Token: 0x06006CAE RID: 27822 RVA: 0x00261E20 File Offset: 0x00260020
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.ticksTillRaid > -1)
			{
				this.ticksTillRaid--;
				if (this.ticksTillRaid == 0)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = this.mapParent.Map;
					incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(incidentParms.target) * 2.5f;
					incidentParms.faction = this.faction;
					incidentParms.customLetterLabel = "LetterLabelSurpriseReinforcements".Translate();
					incidentParms.customLetterText = "LetterSurpriseReinforcements".Translate(incidentParms.faction.def.pawnsPlural, incidentParms.faction).Resolve();
					IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
				}
			}
		}

		// Token: 0x06006CAF RID: 27823 RVA: 0x00261EF0 File Offset: 0x002600F0
		public override bool QuestPartReserves(Faction f)
		{
			return f == this.faction;
		}

		// Token: 0x06006CB0 RID: 27824 RVA: 0x00261EFB File Offset: 0x002600FB
		public override void Notify_FactionRemoved(Faction removedFaction)
		{
			if (removedFaction == this.faction)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006CB1 RID: 27825 RVA: 0x00261F10 File Offset: 0x00260110
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.faction != null && !Find.FactionManager.AllFactions.Contains(this.faction))
			{
				this.faction = null;
			}
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<float>(ref this.reinforcementChance, "reinforcementChance", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksTillRaid, "ticksTillRaid", -1, false);
		}

		// Token: 0x0400452F RID: 17711
		public MapParent mapParent;

		// Token: 0x04004530 RID: 17712
		public Faction faction;

		// Token: 0x04004531 RID: 17713
		public float reinforcementChance;

		// Token: 0x04004532 RID: 17714
		private int ticksTillRaid = -1;

		// Token: 0x04004533 RID: 17715
		public const float RaidThreatPointsMultiplier = 2.5f;

		// Token: 0x04004534 RID: 17716
		private static readonly FloatRange RandomDelayRange = new FloatRange(300f, 900f);
	}
}
