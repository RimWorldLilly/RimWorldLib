using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D0 RID: 5072
	public class Command_Psycast : Command_Ability
	{
		// Token: 0x06007E44 RID: 32324 RVA: 0x002D23FF File Offset: 0x002D05FF
		public Command_Psycast(Psycast ability, Pawn pawn) : base(ability, pawn)
		{
		}

		// Token: 0x06007E45 RID: 32325 RVA: 0x002D240C File Offset: 0x002D060C
		protected override void DisabledCheck()
		{
			AbilityDef def = this.ability.def;
			Pawn pawn = this.ability.pawn;
			this.disabled = false;
			if (def.EntropyGain > 1E-45f)
			{
				if (pawn.GetPsylinkLevel() < def.level)
				{
					base.DisableWithReason("CommandPsycastHigherLevelPsylinkRequired".Translate(def.level));
				}
				else if (pawn.psychicEntropy.WouldOverflowEntropy(def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(pawn)))
				{
					base.DisableWithReason("CommandPsycastWouldExceedEntropy".Translate(def.label));
				}
			}
			base.DisabledCheck();
		}

		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x06007E46 RID: 32326 RVA: 0x002D24B8 File Offset: 0x002D06B8
		public override string TopRightLabel
		{
			get
			{
				AbilityDef def = this.ability.def;
				string text = "";
				if (def.EntropyGain > 1E-45f)
				{
					text += "NeuralHeatLetter".Translate() + ": " + def.EntropyGain.ToString() + "\n";
				}
				if (def.PsyfocusCost > 1E-45f)
				{
					string t;
					if (def.AnyCompOverridesPsyfocusCost)
					{
						if (def.PsyfocusCostRange.Span > 1E-45f)
						{
							t = (def.PsyfocusCostRange.min * 100f).ToString() + "-" + def.PsyfocusCostPercentMax;
						}
						else
						{
							t = def.PsyfocusCostPercentMax;
						}
					}
					else
					{
						t = def.PsyfocusCostPercent;
					}
					text += "PsyfocusLetter".Translate() + ": " + t;
				}
				return text.TrimEndNewlines();
			}
		}
	}
}
