using System;
using System.Collections.Generic;
using RimWorld.Utility;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019D6 RID: 6614
	public class CompVoidNode : CompInteractable
	{
		// Token: 0x0600A675 RID: 42613 RVA: 0x003B5A3C File Offset: 0x003B3C3C
		private float AgeSecs()
		{
			return (float)(Find.TickManager.TicksGame - this.parent.TickSpawned) / 60f;
		}

		// Token: 0x0600A676 RID: 42614 RVA: 0x003B5A5B File Offset: 0x003B3C5B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Void node"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.parent.Graphic.MatSingle.SetFloat(ShaderPropertyIDs.VoidNodeDestructionStartAge, float.MaxValue);
				this.ambientSustainer = SoundDefOf.VoidNode_Ambient.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
			});
		}

		// Token: 0x0600A677 RID: 42615 RVA: 0x003B5A90 File Offset: 0x003B3C90
		public override void CompTick()
		{
			base.CompTick();
			if (this.closeTick > 0)
			{
				this.ambientSustainer.End();
				if (Find.TickManager.TicksGame + 260 == this.closeTick)
				{
					Find.MusicManagerPlay.ForcePlaySong(SongDefOf.AnomalyCreditsSong, false);
				}
				if (Find.TickManager.TicksGame == this.closeTick)
				{
					Find.TickManager.Pause();
					Thing.allowDestroyNonDestroyable = true;
					this.parent.Destroy(DestroyMode.Vanish);
					Thing.allowDestroyNonDestroyable = false;
					CameraJumper.TryJump(Find.Anomaly.monolith, CameraJumper.MovementMode.Cut);
					if (this.embracing)
					{
						VoidAwakeningUtility.EmbraceTheVoid(this.interactedPawn);
						return;
					}
					VoidAwakeningUtility.DisruptTheLink(this.interactedPawn);
				}
			}
		}

		// Token: 0x0600A678 RID: 42616 RVA: 0x003B5B4C File Offset: 0x003B3D4C
		private void OpenDialog(Pawn pawn)
		{
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(new DiaNode("VoidNodeLetter".Translate(pawn.Named("PAWN")))
			{
				options = 
				{
					new DiaOption("VoidNodeDisrupt".Translate())
					{
						action = new Action(this.DisruptNode),
						resolveTree = true
					},
					new DiaOption("VoidNodeEmbrace".Translate())
					{
						action = delegate()
						{
							this.CloseNode(true, 300, 0);
						},
						resolveTree = true
					},
					new DiaOption("VoidNodePostpone".Translate())
					{
						resolveTree = true
					}
				}
			}, false, false, null);
			dialog_NodeTree.forcePause = true;
			Find.WindowStack.Add(dialog_NodeTree);
		}

		// Token: 0x0600A679 RID: 42617 RVA: 0x003B5C24 File Offset: 0x003B3E24
		private void DisruptNode()
		{
			SoundDefOf.VoidNode_Explode.PlayOneShotOnCamera(null);
			EffecterDefOf.VoidNodeDisrupted.SpawnMaintained(this.parent, this.parent.Map, 1f);
			EffecterDefOf.VoidNodeDisruptedFlashes.SpawnMaintained(this.parent, this.parent.Map, 1f);
			this.parent.Graphic.MatSingle.SetFloat(ShaderPropertyIDs.VoidNodeDestructionStartAge, this.AgeSecs());
			this.CloseNode(false, 24, 600);
		}

		// Token: 0x0600A67A RID: 42618 RVA: 0x003B5CAC File Offset: 0x003B3EAC
		private void CloseNode(bool embraceVoid, int fadeTimeTicks, int fadeDelayTicks)
		{
			this.closeTick = Find.TickManager.TicksGame + fadeTimeTicks + fadeDelayTicks;
			this.embracing = embraceVoid;
			this.activated = true;
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "NodeClosed");
			Find.TickManager.slower.SignalForceNormalSpeed();
			Find.MusicManagerPlay.ForceSilenceFor(999f);
			CameraJumper.TryJump(this.parent, CameraJumper.MovementMode.Cut);
			ScreenFader.StartFade(Color.white, (float)(fadeTimeTicks - 5) / 60f, (float)fadeDelayTicks / 60f);
		}

		// Token: 0x0600A67B RID: 42619 RVA: 0x003B5D3B File Offset: 0x003B3F3B
		protected override void OnInteracted(Pawn caster)
		{
			this.interactedPawn = caster;
			this.OpenDialog(caster);
		}

		// Token: 0x0600A67C RID: 42620 RVA: 0x003B5D4B File Offset: 0x003B3F4B
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (this.activated)
			{
				return false;
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}

		// Token: 0x0600A67D RID: 42621 RVA: 0x003B5D64 File Offset: 0x003B3F64
		public void Reset()
		{
			this.activated = false;
		}

		// Token: 0x0600A67E RID: 42622 RVA: 0x003B5D6D File Offset: 0x003B3F6D
		public override string CompInspectStringExtra()
		{
			if (this.activated)
			{
				return "";
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x0600A67F RID: 42623 RVA: 0x003B5D83 File Offset: 0x003B3F83
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompVoidNode.<CompFloatMenuOptions>d__18 <CompFloatMenuOptions>d__ = new CompVoidNode.<CompFloatMenuOptions>d__18(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0600A680 RID: 42624 RVA: 0x003B5D9A File Offset: 0x003B3F9A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompVoidNode.<CompGetGizmosExtra>d__19 <CompGetGizmosExtra>d__ = new CompVoidNode.<CompGetGizmosExtra>d__19(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A681 RID: 42625 RVA: 0x003B5DAC File Offset: 0x003B3FAC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.activated, "activated", false, false);
			Scribe_References.Look<Pawn>(ref this.interactedPawn, "interactedPawn", false);
			Scribe_Values.Look<bool>(ref this.embracing, "embracing", false, false);
			Scribe_Values.Look<int>(ref this.closeTick, "closeTick", 0, false);
		}

		// Token: 0x0400621B RID: 25115
		private const int SlowFade = 300;

		// Token: 0x0400621C RID: 25116
		private const int QuickFade = 24;

		// Token: 0x0400621D RID: 25117
		private const int MusicIntroLengthTicks = 260;

		// Token: 0x0400621E RID: 25118
		private bool activated;

		// Token: 0x0400621F RID: 25119
		private Pawn interactedPawn;

		// Token: 0x04006220 RID: 25120
		private bool embracing;

		// Token: 0x04006221 RID: 25121
		private int closeTick = -99999;

		// Token: 0x04006222 RID: 25122
		private Sustainer ambientSustainer;
	}
}
