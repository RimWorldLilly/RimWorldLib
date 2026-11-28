using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002020 RID: 8224
	public class SurgeryOutcome_Failure : SurgeryOutcome
	{
		// Token: 0x0600D00F RID: 53263 RVA: 0x004C7B78 File Offset: 0x004C5D78
		protected virtual bool CanApply(RecipeDef recipe)
		{
			return Rand.Chance(this.chance);
		}

		// Token: 0x0600D010 RID: 53264 RVA: 0x004C7B85 File Offset: 0x004C5D85
		public override bool Apply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			if (this.CanApply(recipe))
			{
				base.ApplyDamage(patient, part);
				this.PostDamagedApplied(patient);
				if (!patient.Dead)
				{
					this.TryGainBotchedSurgeryThought(patient, surgeon);
				}
				base.SendLetter(surgeon, patient, recipe);
				return true;
			}
			return false;
		}

		// Token: 0x0600D011 RID: 53265 RVA: 0x004C7BC1 File Offset: 0x004C5DC1
		protected virtual void PostDamagedApplied(Pawn patient)
		{
		}

		// Token: 0x0600D012 RID: 53266 RVA: 0x004C7BC3 File Offset: 0x004C5DC3
		protected void TryGainBotchedSurgeryThought(Pawn patient, Pawn surgeon)
		{
			if (!patient.RaceProps.Humanlike || patient.needs.mood == null)
			{
				return;
			}
			patient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BotchedMySurgery, surgeon, null);
		}
	}
}
