using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020004C4 RID: 1220
	public class Hediff_Psylink : Hediff_Level
	{
		// Token: 0x060021FA RID: 8698 RVA: 0x000C9C81 File Offset: 0x000C7E81
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.TryGiveAbilityOfLevel(this.level, !this.suppressPostAddLetter);
			Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.Notify_GainedPsylink();
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x000C9CB4 File Offset: 0x000C7EB4
		public void ChangeLevel(int levelOffset, bool sendLetter)
		{
			if (levelOffset > 0)
			{
				float num = Math.Min((float)levelOffset, this.def.maxSeverity - (float)this.level);
				int num2 = 0;
				while ((float)num2 < num)
				{
					int abilityLevel = this.level + 1 + num2;
					this.TryGiveAbilityOfLevel(abilityLevel, sendLetter);
					Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
					if (psychicEntropy != null)
					{
						psychicEntropy.Notify_GainedPsylink();
					}
					num2++;
				}
			}
			base.ChangeLevel(levelOffset);
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x000C9D1D File Offset: 0x000C7F1D
		public override void ChangeLevel(int levelOffset)
		{
			this.ChangeLevel(levelOffset, true);
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000C9D28 File Offset: 0x000C7F28
		public static string MakeLetterTextNewPsylinkLevel(Pawn pawn, int abilityLevel, IEnumerable<AbilityDef> newAbilities = null)
		{
			string text = ((abilityLevel == 1) ? "LetterPsylinkLevelGained_First" : "LetterPsylinkLevelGained_NotFirst").Translate(pawn.Named("USER"));
			if (!newAbilities.EnumerableNullOrEmpty<AbilityDef>())
			{
				text += "\n\n" + "LetterPsylinkLevelGained_PsycastLearned".Translate(pawn.Named("USER"), abilityLevel, (from a in newAbilities
				select a.LabelCap.Resolve()).ToLineList(null, false));
			}
			return text;
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x000C9DC8 File Offset: 0x000C7FC8
		public void TryGiveAbilityOfLevel(int abilityLevel, bool sendLetter = true)
		{
			string str = "LetterLabelPsylinkLevelGained".Translate() + ": " + this.pawn.LabelShortCap;
			string str2;
			if (!this.pawn.abilities.abilities.Any((Ability a) => a.def.IsPsycast && a.def.level == abilityLevel))
			{
				AbilityDef abilityDef = (from a in DefDatabase<AbilityDef>.AllDefs
				where a.IsPsycast && a.level == abilityLevel
				select a).RandomElement<AbilityDef>();
				this.pawn.abilities.GainAbility(abilityDef);
				str2 = Hediff_Psylink.MakeLetterTextNewPsylinkLevel(this.pawn, abilityLevel, Gen.YieldSingle<AbilityDef>(abilityDef));
			}
			else
			{
				str2 = Hediff_Psylink.MakeLetterTextNewPsylinkLevel(this.pawn, abilityLevel, null);
			}
			if (sendLetter && PawnUtility.ShouldSendNotificationAbout(this.pawn) && !this.pawn.IsDuplicate)
			{
				Find.LetterStack.ReceiveLetter(str, str2, LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x000C9ED4 File Offset: 0x000C80D4
		public override void CopyFrom(Hediff other)
		{
			base.CopyFrom(other);
			Hediff_Psylink hediff_Psylink = other as Hediff_Psylink;
			if (hediff_Psylink != null)
			{
				this.level = hediff_Psylink.level;
				this.suppressPostAddLetter = true;
			}
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x000C9F05 File Offset: 0x000C8105
		public override void PostRemoved()
		{
			base.PostRemoved();
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs == null)
			{
				return;
			}
			needs.AddOrRemoveNeedsAsAppropriate();
		}

		// Token: 0x040019A1 RID: 6561
		public bool suppressPostAddLetter;
	}
}
