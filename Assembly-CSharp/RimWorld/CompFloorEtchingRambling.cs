using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001893 RID: 6291
	public class CompFloorEtchingRambling : CompInteractable
	{
		// Token: 0x06009D99 RID: 40345 RVA: 0x0038A320 File Offset: 0x00388520
		protected override void OnInteracted(Pawn caster)
		{
			this.deciphered = true;
			this.GenerateMessage();
			TaggedString label = "LetterLabelFloorEtchingRamblings".Translate();
			TaggedString taggedString = "LetterFloorEtchingsRamblings".Translate(caster.Named("PAWN"));
			TaggedString t = taggedString;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(this.message);
			taggedString = t + defaultInterpolatedStringHandler.ToStringAndClear();
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, this.parent, null, null, null, null, 0, true);
		}

		// Token: 0x06009D9A RID: 40346 RVA: 0x0038A3B0 File Offset: 0x003885B0
		protected void GenerateMessage()
		{
			this.deciphered = true;
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.LabyrinthRamblings);
			foreach (Thing thing in this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					Pawn innerPawn = corpse.InnerPawn;
					if (innerPawn.RaceProps.Humanlike && !innerPawn.IsColonist && corpse.timeOfDeath < this.parent.TickSpawned)
					{
						CompFloorEtchingRambling.PossibleCorpses.Add(innerPawn);
					}
				}
			}
			TaleData_Pawn taleData_Pawn;
			if (CompFloorEtchingRambling.PossibleCorpses.Any<Pawn>())
			{
				taleData_Pawn = TaleData_Pawn.GenerateFrom(CompFloorEtchingRambling.PossibleCorpses.RandomElement<Pawn>());
			}
			else
			{
				taleData_Pawn = TaleData_Pawn.GenerateRandom(true);
			}
			foreach (Rule item in taleData_Pawn.GetRules("PAWN", request.Constants))
			{
				request.Rules.Add(item);
			}
			this.message = GrammarResolver.Resolve("r_root", request, null, false, null, null, null, true);
			CompFloorEtchingRambling.PossibleCorpses.Clear();
		}

		// Token: 0x06009D9B RID: 40347 RVA: 0x0038A514 File Offset: 0x00388714
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.deciphered)
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append(this.message);
			}
			else
			{
				stringBuilder.Append(base.CompInspectStringExtra());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06009D9C RID: 40348 RVA: 0x0038A55D File Offset: 0x0038875D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompFloorEtchingRambling.<CompGetGizmosExtra>d__6 <CompGetGizmosExtra>d__ = new CompFloorEtchingRambling.<CompGetGizmosExtra>d__6(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009D9D RID: 40349 RVA: 0x0038A56D File Offset: 0x0038876D
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompFloorEtchingRambling.<CompFloatMenuOptions>d__7 <CompFloatMenuOptions>d__ = new CompFloorEtchingRambling.<CompFloatMenuOptions>d__7(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x06009D9E RID: 40350 RVA: 0x0038A584 File Offset: 0x00388784
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.deciphered, "deciphered", false, false);
			Scribe_Values.Look<TaggedString>(ref this.message, "message", default(TaggedString), false);
		}

		// Token: 0x04005D57 RID: 23895
		public bool deciphered;

		// Token: 0x04005D58 RID: 23896
		public TaggedString message;

		// Token: 0x04005D59 RID: 23897
		private static readonly List<Pawn> PossibleCorpses = new List<Pawn>();
	}
}
