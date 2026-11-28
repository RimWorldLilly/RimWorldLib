using System;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.QuestGen
{
	// Token: 0x020022D0 RID: 8912
	public class QuestPart_MetalHell : QuestPartActivable
	{
		// Token: 0x17002637 RID: 9783
		// (get) Token: 0x0600DBC8 RID: 56264 RVA: 0x0051E359 File Offset: 0x0051C559
		private int VFXStartTick
		{
			get
			{
				return this.enableTick + 540 - 300;
			}
		}

		// Token: 0x17002638 RID: 9784
		// (get) Token: 0x0600DBC9 RID: 56265 RVA: 0x0051E36D File Offset: 0x0051C56D
		private int SkipTick
		{
			get
			{
				return this.enableTick + 540;
			}
		}

		// Token: 0x0600DBCA RID: 56266 RVA: 0x0051E37B File Offset: 0x0051C57B
		public QuestPart_MetalHell()
		{
		}

		// Token: 0x0600DBCB RID: 56267 RVA: 0x0051E383 File Offset: 0x0051C583
		public QuestPart_MetalHell(string activateMonolithSignal, string voidNodeTag)
		{
			this.inSignalEnable = activateMonolithSignal;
			this.voidNodeTag = voidNodeTag;
			this.reactivatable = true;
		}

		// Token: 0x0600DBCC RID: 56268 RVA: 0x0051E3A0 File Offset: 0x0051C5A0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalEnable)
			{
				signal.args.TryGetArg<Pawn>("ACTIVATOR", out this.pawnToSend);
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0600DBCD RID: 56269 RVA: 0x0051E3D4 File Offset: 0x0051C5D4
		public override void QuestPartTick()
		{
			if (this.pawnToSend.Downed || this.pawnToSend.Dead)
			{
				Messages.Message("GleamingMonolithInterrupted".Translate(this.pawnToSend.Named("PAWN")), MessageTypeDefOf.NeutralEvent, true);
				this.Disable();
				return;
			}
			if (Find.TickManager.TicksGame > this.VFXStartTick && Find.TickManager.TicksGame < this.SkipTick)
			{
				if (this.monolithEffecter == null)
				{
					this.monolithEffecter = EffecterDefOf.AbductWarmup_Monolith.Spawn(Find.Anomaly.monolith, Find.Anomaly.monolith.Map, 1f);
				}
				if (this.pawnEffecter == null)
				{
					this.pawnEffecter = EffecterDefOf.AbductWarmup_Pawn.Spawn(this.pawnToSend, this.pawnToSend.Map, 1f);
				}
				this.pawnEffecter.EffectTick(this.pawnToSend, this.pawnToSend);
				this.monolithEffecter.EffectTick(Find.Anomaly.monolith, Find.Anomaly.monolith);
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			if (Find.TickManager.TicksGame == this.SkipTick)
			{
				ScreenFader.StartFade(Color.black, 0f);
			}
			if (Find.TickManager.TicksGame == this.SkipTick)
			{
				this.GenerateMetalHellAndSkipPawn();
				ScreenFader.StartFade(Color.clear, 7f);
				this.Disable();
			}
		}

		// Token: 0x0600DBCE RID: 56270 RVA: 0x0051E564 File Offset: 0x0051C764
		private void GenerateMetalHellAndSkipPawn()
		{
			if (this.metalHell == null)
			{
				this.metalHell = PocketMapUtility.GeneratePocketMap(new IntVec3(75, 1, 75), MapGeneratorDefOf.MetalHell, null, Find.Anomaly.monolith.Map);
			}
			Thing voidNode = this.metalHell.listerThings.ThingsOfDef(ThingDefOf.VoidNode).FirstOrDefault<Thing>();
			QuestUtility.AddQuestTag(ref voidNode.questTags, this.voidNodeTag);
			IntVec3 cell = CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(this.metalHell) && !c.InHorDistOf(this.metalHell.Center, 10f) && this.metalHell.reachability.CanReach(c, voidNode, PathEndMode.Touch, TraverseMode.PassDoors), this.metalHell, 1000);
			SkipUtility.SkipTo(this.pawnToSend, cell, this.metalHell);
			this.pawnToSend.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			this.pawnToSend.drafter.Drafted = true;
			CameraJumper.TryJump(this.pawnToSend, CameraJumper.MovementMode.Cut);
			Find.TickManager.slower.SignalForceNormalSpeed();
			Find.LetterStack.ReceiveLetter("VoidAwakeningFinalStructuresActivatedLabel".Translate(this.pawnToSend.Named("STUDIER")), "VoidAwakeningFinalStructuresActivatedText".Translate(this.pawnToSend.Named("STUDIER")), LetterDefOf.NeutralEvent, this.pawnToSend, null, null, null, null, 0, false);
			Find.MusicManagerPlay.CheckTransitions();
		}

		// Token: 0x0600DBCF RID: 56271 RVA: 0x0051E6B8 File Offset: 0x0051C8B8
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && this.metalHell != null && !Find.World.pocketMaps.Contains(this.metalHell.Parent))
			{
				this.metalHell = null;
			}
			Scribe_Values.Look<string>(ref this.voidNodeTag, "voidNodeTag", null, false);
			Scribe_References.Look<Map>(ref this.metalHell, "metalHell", false);
			Scribe_References.Look<Pawn>(ref this.pawnToSend, "pawnToSend", false);
		}

		// Token: 0x0600DBD0 RID: 56272 RVA: 0x0051E732 File Offset: 0x0051C932
		public override void Cleanup()
		{
			base.Cleanup();
			this.metalHell = null;
			this.pawnToSend = null;
		}

		// Token: 0x04008D10 RID: 36112
		private const int MetalHellSize = 75;

		// Token: 0x04008D11 RID: 36113
		private const int MinSpawnDistFromMetalHellCenter = 10;

		// Token: 0x04008D12 RID: 36114
		private const int SkipDelayTicks = 540;

		// Token: 0x04008D13 RID: 36115
		private const int EffectsDurationTicks = 300;

		// Token: 0x04008D14 RID: 36116
		private const int FadeOutDurationTicks = 0;

		// Token: 0x04008D15 RID: 36117
		private const int FadeInDurationTicks = 420;

		// Token: 0x04008D16 RID: 36118
		private string voidNodeTag;

		// Token: 0x04008D17 RID: 36119
		private Map metalHell;

		// Token: 0x04008D18 RID: 36120
		private Pawn pawnToSend;

		// Token: 0x04008D19 RID: 36121
		[Unsaved(false)]
		private Effecter monolithEffecter;

		// Token: 0x04008D1A RID: 36122
		[Unsaved(false)]
		private Effecter pawnEffecter;
	}
}
