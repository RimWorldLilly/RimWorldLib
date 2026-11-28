using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B14 RID: 6932
	public class FilthGrayFleshNoticeable : Filth
	{
		// Token: 0x0600B148 RID: 45384 RVA: 0x003E7538 File Offset: 0x003E5738
		protected override void Tick()
		{
			if (this.sentLetter)
			{
				return;
			}
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.Position.InHorDistOf(base.Position, 5f) && PawnUtility.ShouldSendNotificationAbout(pawn) && !MetalhorrorUtility.IsInfected(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) && pawn.health.capacities.CanBeAwake)
				{
					this.SendLetter(pawn);
					break;
				}
			}
		}

		// Token: 0x0600B149 RID: 45385 RVA: 0x003E75F4 File Offset: 0x003E57F4
		private void SendLetter(Pawn pawn)
		{
			this.sentLetter = true;
			string text = "";
			if (!Find.Anomaly.hasSeenGrayFlesh)
			{
				text = "LetterInterrogationUnlocked".Translate() + " ";
			}
			text += "LetterGrayFleshMethodOne".Translate();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 3);
			defaultInterpolatedStringHandler.AppendLiteral("- ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendLiteral("- ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("LetterGrayFleshMethodTwo".Translate());
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendLiteral("- ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("LetterGrayFleshMethodThree".Translate());
			string arg = defaultInterpolatedStringHandler.ToStringAndClear();
			TaggedString text2 = "LetterGrayFleshDiscovered".Translate(pawn.Named("PAWN"), arg.Named("METHODS"));
			Find.LetterStack.ReceiveLetter("LetterGrayFleshDiscoveredLabel".Translate(), text2, LetterDefOf.ThreatSmall, new LookTargets(base.Position, base.Map), null, null, null, null, 0, true);
			Thing thing = ThingMaker.MakeThing(ThingDefOf.GrayFleshSample, null);
			thing.TryGetComp<CompAnalyzableBiosignature>().biosignature = this.biosignature;
			GenSpawn.Spawn(thing, base.Position, base.Map, WipeMode.Vanish);
			Find.Anomaly.hasSeenGrayFlesh = true;
		}

		// Token: 0x0600B14A RID: 45386 RVA: 0x003E774E File Offset: 0x003E594E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sentLetter, "sentLetter", false, false);
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
		}

		// Token: 0x0400666A RID: 26218
		private bool sentLetter;

		// Token: 0x0400666B RID: 26219
		public int biosignature;

		// Token: 0x0400666C RID: 26220
		private const int DetectDist = 5;
	}
}
