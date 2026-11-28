using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FC8 RID: 8136
	public class StatWorker_MeleeDPS : StatWorker
	{
		// Token: 0x0600CE8F RID: 52879 RVA: 0x004C0959 File Offset: 0x004BEB59
		public override bool IsDisabledFor(Thing thing)
		{
			return base.IsDisabledFor(thing) || StatDefOf.MeleeHitChance.Worker.IsDisabledFor(thing);
		}

		// Token: 0x0600CE90 RID: 52880 RVA: 0x004C0978 File Offset: 0x004BEB78
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.Thing == null)
			{
				string str = "Getting MeleeDPS stat for ";
				Def def = req.Def;
				Log.Error(str + ((def != null) ? def.ToString() : null) + " without concrete pawn. This always returns 0.");
			}
			return this.GetMeleeDamage(req, applyPostProcess) * this.GetMeleeHitChance(req, applyPostProcess) / this.GetMeleeCooldown(req, applyPostProcess);
		}

		// Token: 0x0600CE91 RID: 52881 RVA: 0x004C09D0 File Offset: 0x004BEBD0
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("StatsReport_MeleeDPSExplanation".Translate());
			stringBuilder.AppendLine("StatsReport_MeleeDamage".Translate() + " (" + "AverageOfAllAttacks".Translate() + ")");
			stringBuilder.AppendLine("  " + this.GetMeleeDamage(req, true).ToString("0.##"));
			stringBuilder.AppendLine("StatsReport_Cooldown".Translate() + " (" + "AverageOfAllAttacks".Translate() + ")");
			stringBuilder.AppendLine("  " + "StatsReport_CooldownFormat".Translate(this.GetMeleeCooldown(req, true).ToString("0.##")));
			stringBuilder.AppendLine("StatsReport_MeleeHitChance".Translate());
			stringBuilder.AppendLine(StatDefOf.MeleeHitChance.Worker.GetExplanationUnfinalized(req, StatDefOf.MeleeHitChance.toStringNumberSense).TrimEndNewlines().Indented("    "));
			stringBuilder.Append(StatDefOf.MeleeHitChance.Worker.GetExplanationFinalizePart(req, StatDefOf.MeleeHitChance.toStringNumberSense, this.GetMeleeHitChance(req, true)).Indented("    "));
			return stringBuilder.ToString();
		}

		// Token: 0x0600CE92 RID: 52882 RVA: 0x004C0B48 File Offset: 0x004BED48
		public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
		{
			return string.Format("{0} ( {1} x {2} / {3} )", new object[]
			{
				value.ToStringByStyle(stat.toStringStyle, numberSense),
				this.GetMeleeDamage(optionalReq, true).ToString("0.##"),
				StatDefOf.MeleeHitChance.ValueToString(this.GetMeleeHitChance(optionalReq, true), ToStringNumberSense.Absolute, true),
				this.GetMeleeCooldown(optionalReq, true).ToString("0.##")
			});
		}

		// Token: 0x0600CE93 RID: 52883 RVA: 0x004C0BC0 File Offset: 0x004BEDC0
		private float GetMeleeDamage(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false);
			if (updatedAvailableVerbsList.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
			{
				if (updatedAvailableVerbsList[i].IsMeleeAttack)
				{
					num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
				}
			}
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = 0f;
			for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
			{
				if (updatedAvailableVerbsList[j].IsMeleeAttack)
				{
					num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * updatedAvailableVerbsList[j].verb.verbProps.AdjustedMeleeDamageAmount(updatedAvailableVerbsList[j].verb, pawn);
				}
			}
			return num2;
		}

		// Token: 0x0600CE94 RID: 52884 RVA: 0x004C0CBB File Offset: 0x004BEEBB
		private float GetMeleeHitChance(StatRequest req, bool applyPostProcess = true)
		{
			if (req.HasThing)
			{
				return req.Thing.GetStatValue(StatDefOf.MeleeHitChance, applyPostProcess, -1);
			}
			return req.BuildableDef.GetStatValueAbstract(StatDefOf.MeleeHitChance, null);
		}

		// Token: 0x0600CE95 RID: 52885 RVA: 0x004C0CEC File Offset: 0x004BEEEC
		private float GetMeleeCooldown(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return 1f;
			}
			List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false);
			if (updatedAvailableVerbsList.Count == 0)
			{
				return 1f;
			}
			float num = 0f;
			for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
			{
				if (updatedAvailableVerbsList[i].IsMeleeAttack)
				{
					num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
				}
			}
			if (num == 0f)
			{
				return 1f;
			}
			float num2 = 0f;
			for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
			{
				if (updatedAvailableVerbsList[j].IsMeleeAttack)
				{
					num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * (float)updatedAvailableVerbsList[j].verb.verbProps.AdjustedCooldownTicks(updatedAvailableVerbsList[j].verb, pawn);
				}
			}
			return num2 / 60f;
		}

		// Token: 0x0600CE96 RID: 52886 RVA: 0x004C0DEE File Offset: 0x004BEFEE
		public override bool ShouldShowFor(StatRequest req)
		{
			return base.ShouldShowFor(req) && req.Thing is Pawn;
		}

		// Token: 0x0600CE97 RID: 52887 RVA: 0x004C0E0A File Offset: 0x004BF00A
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			StatWorker_MeleeDPS.<GetInfoCardHyperlinks>d__8 <GetInfoCardHyperlinks>d__ = new StatWorker_MeleeDPS.<GetInfoCardHyperlinks>d__8(-2);
			<GetInfoCardHyperlinks>d__.<>3__statRequest = statRequest;
			return <GetInfoCardHyperlinks>d__;
		}
	}
}
