using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002371 RID: 9073
	public class CaravansBattlefield : MapParent
	{
		// Token: 0x1700279E RID: 10142
		// (get) Token: 0x0600E136 RID: 57654 RVA: 0x00539E3D File Offset: 0x0053803D
		public bool WonBattle
		{
			get
			{
				return this.wonBattle;
			}
		}

		// Token: 0x1700279F RID: 10143
		// (get) Token: 0x0600E137 RID: 57655 RVA: 0x00539E45 File Offset: 0x00538045
		public override bool CanReformFoggedEnemies
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E138 RID: 57656 RVA: 0x00539E48 File Offset: 0x00538048
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wonBattle, "wonBattle", false, false);
		}

		// Token: 0x0600E139 RID: 57657 RVA: 0x00539E62 File Offset: 0x00538062
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
			if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
			{
				return false;
			}
			alsoRemoveWorldObject = true;
			return true;
		}

		// Token: 0x0600E13A RID: 57658 RVA: 0x00539E8E File Offset: 0x0053808E
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (base.HasMap)
			{
				this.CheckWonBattle();
			}
		}

		// Token: 0x0600E13B RID: 57659 RVA: 0x00539EA5 File Offset: 0x005380A5
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			base.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(240000, -1);
		}

		// Token: 0x0600E13C RID: 57660 RVA: 0x00539EC0 File Offset: 0x005380C0
		private void CheckWonBattle()
		{
			if (this.wonBattle)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false, false))
			{
				return;
			}
			TimedDetectionRaids component = base.GetComponent<TimedDetectionRaids>();
			component.SetNotifiedSilently();
			string detectionCountdownTimeLeftString = component.DetectionCountdownTimeLeftString;
			Find.LetterStack.ReceiveLetter("LetterLabelCaravansBattlefieldVictory".Translate(), "LetterCaravansBattlefieldVictory".Translate(detectionCountdownTimeLeftString), LetterDefOf.PositiveEvent, this, null, null, null, null, 0, true);
			TaleRecorder.RecordTale(TaleDefOf.CaravanAmbushDefeated, new object[]
			{
				base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
			});
			this.wonBattle = true;
		}

		// Token: 0x04009038 RID: 36920
		private bool wonBattle;
	}
}
