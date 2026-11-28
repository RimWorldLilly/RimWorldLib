using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001464 RID: 5220
	public class CompAbilityEffect_UnnaturalHealing : CompAbilityEffect
	{
		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x0600802F RID: 32815 RVA: 0x002DB8D4 File Offset: 0x002D9AD4
		public new CompProperties_UnnaturalHealing Props
		{
			get
			{
				return (CompProperties_UnnaturalHealing)this.props;
			}
		}

		// Token: 0x06008030 RID: 32816 RVA: 0x002DB8E4 File Offset: 0x002D9AE4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return false;
			}
			bool flag;
			if (MetalhorrorUtility.IsInfected(this.parent.pawn))
			{
				Hediff hediff;
				BodyPartRecord bodyPartRecord;
				flag = HealthUtility.TryGetWorstHealthCondition(pawn, out hediff, out bodyPartRecord, new HediffDef[]
				{
					HediffDefOf.MetalhorrorImplant
				});
			}
			else
			{
				Hediff hediff;
				BodyPartRecord bodyPartRecord;
				flag = HealthUtility.TryGetWorstHealthCondition(pawn, out hediff, out bodyPartRecord, Array.Empty<HediffDef>());
			}
			if (!flag && throwMessages)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CannotUseAbility".Translate(this.parent.def.label));
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted("AbilityCannotCastNoHealableInjury".Translate(pawn.Named("PAWN")).Resolve().StripTags());
				Messages.Message(defaultInterpolatedStringHandler.ToStringAndClear(), pawn, MessageTypeDefOf.RejectInput, false);
			}
			return flag;
		}

		// Token: 0x06008031 RID: 32817 RVA: 0x002DB9C4 File Offset: 0x002D9BC4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				if (hediff.Bleeding)
				{
					hediff.Tended(1f, 1f, 1);
				}
			}
			bool flag = false;
			string text;
			if (MetalhorrorUtility.IsInfected(this.parent.pawn))
			{
				text = HealthUtility.FixWorstHealthCondition(pawn, new HediffDef[]
				{
					HediffDefOf.MetalhorrorImplant
				});
				MetalhorrorUtility.Infect(pawn, this.parent.pawn, "UnnaturalHealingImplant", null);
			}
			else
			{
				text = HealthUtility.FixWorstHealthCondition(pawn, Array.Empty<HediffDef>());
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("LetterUnnaturalHealing".Translate(this.parent.pawn.Named("CASTER"), pawn.Named("PAWN")));
			if (!string.IsNullOrEmpty(text))
			{
				stringBuilder.AppendLine("\n" + text);
			}
			if (pawn.ageTracker.Adult && Rand.Chance(0.25f) && FleshbeastUtility.TryGiveMutation(pawn, HediffDefOf.Tentacle))
			{
				stringBuilder.Append("\n" + "LetterUnnaturalHealingTentacle".Translate(pawn.Named("PAWN")));
				TaleRecorder.RecordTale(TaleDefOf.MutatedMyArm, new object[]
				{
					this.parent.pawn,
					pawn
				});
				flag = true;
			}
			else
			{
				TaleRecorder.RecordTale(TaleDefOf.HealedMe, new object[]
				{
					this.parent.pawn,
					pawn
				});
			}
			TaggedString label = "LetterUnnaturalHealingLabel".Translate();
			LetterDef textLetterDef = flag ? LetterDefOf.NegativeEvent : LetterDefOf.NeutralEvent;
			Find.LetterStack.ReceiveLetter(label, stringBuilder.ToString().TrimEndNewlines(), textLetterDef, pawn, null, null, null, null, 0, true);
		}

		// Token: 0x06008032 RID: 32818 RVA: 0x002DBBDC File Offset: 0x002D9DDC
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return false;
		}

		// Token: 0x0400500C RID: 20492
		private const float TentacleChance = 0.25f;
	}
}
