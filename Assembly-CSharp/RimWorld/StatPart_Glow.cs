using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F8A RID: 8074
	public class StatPart_Glow : StatPart
	{
		// Token: 0x0600CD4F RID: 52559 RVA: 0x004B8223 File Offset: 0x004B6423
		public override IEnumerable<string> ConfigErrors()
		{
			StatPart_Glow.<ConfigErrors>d__5 <ConfigErrors>d__ = new StatPart_Glow.<ConfigErrors>d__5(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600CD50 RID: 52560 RVA: 0x004B8233 File Offset: 0x004B6433
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val *= this.FactorFromGlow(req.Thing);
			}
		}

		// Token: 0x0600CD51 RID: 52561 RVA: 0x004B8260 File Offset: 0x004B6460
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_LightMultiplier".Translate(this.GlowLevel(req.Thing).ToStringPercent()) + ": x" + this.FactorFromGlow(req.Thing).ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD52 RID: 52562 RVA: 0x004B82D0 File Offset: 0x004B64D0
		private bool ActiveFor(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (this.humanlikeOnly && !pawn.RaceProps.Humanlike)
				{
					return false;
				}
				if (pawn.IsShambler)
				{
					return false;
				}
				if (this.ignoreIfIncapableOfSight && PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn))
				{
					return false;
				}
				if (this.ignoreIfPrefersDarkness)
				{
					if (pawn.Ideo != null && pawn.Ideo.IdeoPrefersDarkness())
					{
						return false;
					}
					if (pawn.genes != null && !pawn.genes.AffectedByDarkness)
					{
						return false;
					}
				}
			}
			else if (this.pawnOnly)
			{
				return false;
			}
			return t.Spawned;
		}

		// Token: 0x0600CD53 RID: 52563 RVA: 0x004B8360 File Offset: 0x004B6560
		private float GlowLevel(Thing t)
		{
			return t.Map.glowGrid.GroundGlowAt(t.Position, false, false);
		}

		// Token: 0x0600CD54 RID: 52564 RVA: 0x004B837A File Offset: 0x004B657A
		private float FactorFromGlow(Thing t)
		{
			return this.factorFromGlowCurve.Evaluate(this.GlowLevel(t));
		}

		// Token: 0x04008304 RID: 33540
		private bool pawnOnly;

		// Token: 0x04008305 RID: 33541
		private bool humanlikeOnly;

		// Token: 0x04008306 RID: 33542
		private SimpleCurve factorFromGlowCurve;

		// Token: 0x04008307 RID: 33543
		private bool ignoreIfPrefersDarkness;

		// Token: 0x04008308 RID: 33544
		private bool ignoreIfIncapableOfSight;
	}
}
