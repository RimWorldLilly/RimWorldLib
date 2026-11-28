using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001619 RID: 5657
	public abstract class LandingOutcomeWorker
	{
		// Token: 0x17001868 RID: 6248
		// (get) Token: 0x06008CFB RID: 36091 RVA: 0x00334736 File Offset: 0x00332936
		protected virtual LetterDef LetterDef
		{
			get
			{
				return this.def.letterDef;
			}
		}

		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x06008CFC RID: 36092 RVA: 0x00334743 File Offset: 0x00332943
		protected virtual string LetterLabel
		{
			get
			{
				return this.def.letterLabel;
			}
		}

		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x06008CFD RID: 36093 RVA: 0x00334750 File Offset: 0x00332950
		protected virtual string LetterText
		{
			get
			{
				return this.def.letterText;
			}
		}

		// Token: 0x06008CFE RID: 36094 RVA: 0x0033475D File Offset: 0x0033295D
		public LandingOutcomeWorker(LandingOutcomeDef def)
		{
			this.def = def;
		}

		// Token: 0x06008CFF RID: 36095 RVA: 0x0033476C File Offset: 0x0033296C
		public virtual void ApplyOutcome(Gravship gravship)
		{
			this.SendStandardLetter(gravship.Engine, null, gravship.Engine);
		}

		// Token: 0x06008D00 RID: 36096 RVA: 0x00334788 File Offset: 0x00332988
		protected void SendStandardLetter(Building_GravEngine Engine, string extraText, LookTargets letterTarget)
		{
			TaggedString taggedString = this.LetterText.Formatted(Engine.RenamableLabel.Named("GRAVSHIP"), Engine.launchInfo.pilot.Named("PILOT"), Engine.launchInfo.copilot.Named("COPILOT"));
			if (extraText != null)
			{
				taggedString += "\n\n" + extraText;
			}
			taggedString += "\n\n" + "GravshipOutcomeChanceLetterExtra".Translate(GravshipUtility.NegativeLandingOutcomeFromQuality(Engine.launchInfo.quality));
			Find.LetterStack.ReceiveLetter(this.LetterLabel, taggedString, this.LetterDef, letterTarget, null, null, null, null, 0, true);
		}

		// Token: 0x04005685 RID: 22149
		private LandingOutcomeDef def;
	}
}
