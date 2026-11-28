using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001104 RID: 4356
	public class QuestPart_TrackWhenExitMentalState : QuestPart
	{
		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06006CBC RID: 27836 RVA: 0x00262144 File Offset: 0x00260344
		private List<Pawn> TrackedPawns
		{
			get
			{
				if (this.cachedPawns == null)
				{
					this.cachedPawns = (from p in this.mapParent.Map.mapPawns.AllPawnsSpawned
					where p.InMentalState && p.MentalStateDef == this.mentalStateDef && !p.questTags.NullOrEmpty<string>() && p.questTags.Contains(this.tag)
					select p).ToList<Pawn>();
				}
				return this.cachedPawns;
			}
		}

		// Token: 0x06006CBD RID: 27837 RVA: 0x00262190 File Offset: 0x00260390
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.mapParent == null || !this.mapParent.HasMap || !this.quest.IsParentSuitableForQuest(this.mapParent))
			{
				this.mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
			}
			if (!this.signalSent && this.inSignals.Contains(signal.tag))
			{
				Pawn pawn = this.TrackedPawns.Find((Pawn p) => p == signal.args.GetArg<Pawn>("SUBJECT"));
				if (pawn != null)
				{
					this.cachedPawns.Remove(pawn);
				}
				if (!this.cachedPawns.Any<Pawn>())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignal, false));
					this.signalSent = true;
				}
			}
		}

		// Token: 0x06006CBE RID: 27838 RVA: 0x00262264 File Offset: 0x00260464
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Defs.Look<MentalStateDef>(ref this.mentalStateDef, "mentalStateDef");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<bool>(ref this.signalSent, "signalSent", false, false);
		}

		// Token: 0x04004539 RID: 17721
		public string tag;

		// Token: 0x0400453A RID: 17722
		public List<string> inSignals;

		// Token: 0x0400453B RID: 17723
		public string outSignal;

		// Token: 0x0400453C RID: 17724
		public MapParent mapParent;

		// Token: 0x0400453D RID: 17725
		public MentalStateDef mentalStateDef;

		// Token: 0x0400453E RID: 17726
		private bool signalSent;

		// Token: 0x0400453F RID: 17727
		[Unsaved(false)]
		private List<Pawn> cachedPawns;
	}
}
