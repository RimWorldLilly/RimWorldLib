using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200184D RID: 6221
	public class CompCerebrexCore : CompInteractable
	{
		// Token: 0x17001A92 RID: 6802
		// (get) Token: 0x06009BB1 RID: 39857 RVA: 0x00381DA0 File Offset: 0x0037FFA0
		private Graphic BrainGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.cachedBrainGraphic) == null)
				{
					result = (this.cachedBrainGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/CerebrexCore/CerebrexCore_Brain", ShaderDatabase.Cutout, CompCerebrexCore.BrainDrawSize, Color.white));
				}
				return result;
			}
		}

		// Token: 0x17001A93 RID: 6803
		// (get) Token: 0x06009BB2 RID: 39858 RVA: 0x00381DDE File Offset: 0x0037FFDE
		public override bool Active
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A94 RID: 6804
		// (get) Token: 0x06009BB3 RID: 39859 RVA: 0x00381DE1 File Offset: 0x0037FFE1
		public Lord AssemblerLord
		{
			get
			{
				return this.assemblerLord;
			}
		}

		// Token: 0x06009BB4 RID: 39860 RVA: 0x00381DEC File Offset: 0x0037FFEC
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (!this.defencesLowered)
			{
				return "CoreNotInteractableStabilizersRemaining".Translate(this.stabilizersRemaining);
			}
			if (this.deactivated)
			{
				return "CoreNotInteractableDeactivated".Translate();
			}
			return true;
		}

		// Token: 0x06009BB5 RID: 39861 RVA: 0x00381E3C File Offset: 0x0038003C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.defencesLowered, "defencesLowered", false, false);
			Scribe_Values.Look<bool>(ref this.deactivated, "deactivated", false, false);
			Scribe_References.Look<Pawn>(ref this.interactedPawn, "interactedPawn", false);
			Scribe_Values.Look<bool>(ref this.scavenging, "scavenging", false, false);
			Scribe_Values.Look<int>(ref this.closeTick, "closeTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastRaidTick, "lastRaidTick", 0, false);
			Scribe_References.Look<Lord>(ref this.assemblerLord, "assemblerLord", false);
			Scribe_Values.Look<int>(ref this.stabilizersRemaining, "stabilizersRemaining", 0, false);
		}

		// Token: 0x06009BB6 RID: 39862 RVA: 0x00381EE0 File Offset: 0x003800E0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.assemblerLord = LordMaker.MakeNewLord(this.parent.Faction, new LordJob_DefendCerebrexCore(this.parent.Position), this.parent.Map, null);
			this.lastRaidTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06009BB7 RID: 39863 RVA: 0x00381F3C File Offset: 0x0038013C
		public override void CompTick()
		{
			base.CompTick();
			if (Find.TickManager.TicksGame > this.lastRaidTick + 60000 && Rand.MTBEventOccurs(48f, 2500f, 1f))
			{
				List<Pawn> list = this.AssemblerLord.ownedPawns.ToList<Pawn>();
				if (list.Any<Pawn>())
				{
					Lord lord = LordMaker.MakeNewLord(this.parent.Faction, new LordJob_AssaultColony(this.parent.Faction, true, true, false, false, true, false, false), this.parent.Map, null);
					this.AssemblerLord.RemoveAllPawns(true);
					lord.AddPawns(list, true);
					this.lastRaidTick = Find.TickManager.TicksGame;
					Find.LetterStack.ReceiveLetter("MechhiveRaidLetterLabel".Translate(), "MechhiveRaidLetterText".Translate(), LetterDefOf.ThreatBig, list, null, null, null, null, 0, true);
				}
			}
			if (this.closeTick <= 0)
			{
				return;
			}
			if (Find.TickManager.TicksGame == this.closeTick)
			{
				this.DeactivateCore(this.scavenging);
			}
		}

		// Token: 0x06009BB8 RID: 39864 RVA: 0x0038204C File Offset: 0x0038024C
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			float z = this.ZOffset + 0.5f * (1f + Mathf.Sin(6.2831855f * (float)GenTicks.TicksGame / 300f)) * this.BobHeight;
			Matrix4x4 matrix = Matrix4x4.TRS(drawLoc + new Vector3(0f, 0.35f, z), Quaternion.AngleAxis(0f, Vector3.up), Vector3.one);
			GenDraw.DrawMeshNowOrLater(this.BrainGraphic.MeshAt(Rot4.South), matrix, this.BrainGraphic.MatSouth, false, null);
		}

		// Token: 0x06009BB9 RID: 39865 RVA: 0x003820E0 File Offset: 0x003802E0
		private void DeactivateCore(bool scavenging)
		{
			Find.TickManager.Pause();
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "CoreDefeated", this.parent.Named("SUBJECT"));
			ScenPart_PursuingMechanoids scenPart_PursuingMechanoids = Find.Scenario.AllParts.FirstOrDefault((ScenPart x) => x is ScenPart_PursuingMechanoids) as ScenPart_PursuingMechanoids;
			if (scenPart_PursuingMechanoids != null)
			{
				scenPart_PursuingMechanoids.Notify_QuestCompleted();
			}
			if (scavenging)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Apparel_CerebrexNode, null);
				GenPlace.TryPlaceThing(thing, this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, null, null, 1);
				Find.LetterStack.ReceiveLetter("CerebrexCoreScavengedLetterLabel".Translate(), "CerebrexCoreScavengedLetterText".Translate(this.interactedPawn.Named("PAWN")), LetterDefOf.PositiveEvent, thing, null, null, null, null, 0, true);
				GameVictoryUtility.ShowCredits("OdysseyScavengedCredits".Translate(this.interactedPawn.Named("PAWN")), SongDefOf.OdysseyCreditsSong, false, 0f);
				return;
			}
			Map map = this.parent.Map;
			Thing.allowDestroyNonDestroyable = true;
			this.parent.Destroy(DestroyMode.Vanish);
			Thing.allowDestroyNonDestroyable = false;
			GenSpawn.Spawn(ThingDefOf.CerebrexCore_Destroyed, this.parent.Position, map, WipeMode.Vanish);
			Find.LetterStack.ReceiveLetter("CerebrexCoreDestroyedLetterLabel".Translate(), "CerebrexCoreDestroyedLetterText".Translate(this.interactedPawn.Named("PAWN")), LetterDefOf.PositiveEvent, this.parent, null, null, null, null, 0, true);
			if (Faction.OfMechanoids != null)
			{
				Faction.OfMechanoids.deactivated = true;
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
			{
				if (pawn == this.interactedPawn)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (needs != null)
					{
						Need_Mood mood = needs.mood;
						if (mood != null)
						{
							ThoughtHandler thoughts = mood.thoughts;
							if (thoughts != null)
							{
								MemoryThoughtHandler memories = thoughts.memories;
								if (memories != null)
								{
									memories.TryGainMemory(ThoughtDefOf.DestroyedMechhiveMainPawn, null, null);
								}
							}
						}
					}
				}
				else
				{
					Pawn_NeedsTracker needs2 = pawn.needs;
					if (needs2 != null)
					{
						Need_Mood mood2 = needs2.mood;
						if (mood2 != null)
						{
							ThoughtHandler thoughts2 = mood2.thoughts;
							if (thoughts2 != null)
							{
								MemoryThoughtHandler memories2 = thoughts2.memories;
								if (memories2 != null)
								{
									memories2.TryGainMemory(ThoughtDefOf.DestroyedMechhive, null, null);
								}
							}
						}
					}
				}
			}
			GameVictoryUtility.ShowCredits("OdysseyDestroyedCredits".Translate(this.interactedPawn.Named("PAWN")), SongDefOf.OdysseyCreditsSong, false, 0f);
		}

		// Token: 0x06009BBA RID: 39866 RVA: 0x0038238C File Offset: 0x0038058C
		public void Notify_StabilizerDisabled()
		{
			this.stabilizersRemaining--;
			if (this.stabilizersRemaining == 0 && !this.defencesLowered)
			{
				this.LowerDefences();
			}
		}

		// Token: 0x06009BBB RID: 39867 RVA: 0x003823B4 File Offset: 0x003805B4
		private void LowerDefences()
		{
			this.defencesLowered = true;
			Find.LetterStack.ReceiveLetter("CerebrexCoreLoweredLetterLabel".Translate(), "CerebrexCoreLoweredLetterText".Translate(), LetterDefOf.PositiveEvent, this.parent, null, null, null, null, 0, true);
		}

		// Token: 0x06009BBC RID: 39868 RVA: 0x003823FC File Offset: 0x003805FC
		protected override void OnInteracted(Pawn caster)
		{
			this.interactedPawn = caster;
			this.OpenDialog();
		}

		// Token: 0x06009BBD RID: 39869 RVA: 0x0038240C File Offset: 0x0038060C
		private void OpenDialog()
		{
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(new DiaNode("CerebrexCoreDialog".Translate(this.interactedPawn.Named("PAWN")))
			{
				options = 
				{
					new DiaOption("CerebrexCoreOption_Destroy".Translate())
					{
						action = delegate()
						{
							this.StartCoreDeactivation(false);
						},
						resolveTree = true
					},
					new DiaOption("CerebrexCoreOption_Scavenge".Translate())
					{
						action = delegate()
						{
							this.StartCoreDeactivation(true);
						},
						resolveTree = true
					},
					new DiaOption("CerebrexCoreOption_Postpone".Translate())
					{
						resolveTree = true
					}
				}
			}, false, false, null);
			dialog_NodeTree.forcePause = true;
			Find.WindowStack.Add(dialog_NodeTree);
		}

		// Token: 0x06009BBE RID: 39870 RVA: 0x003824EC File Offset: 0x003806EC
		private void StartCoreDeactivation(bool scavenge)
		{
			this.deactivated = true;
			this.scavenging = scavenge;
			this.closeTick = Find.TickManager.TicksGame + 300;
			Find.TickManager.slower.SignalForceNormalSpeed();
			Find.MusicManagerPlay.ForceFadeoutAndSilenceFor(999f, 3f, false);
			SoundDefOf.CerebrexCore_Pain.PlayOneShotOnCamera(null);
			CameraJumper.TryJump(this.parent, CameraJumper.MovementMode.Cut);
			ScreenFader.StartFade(Color.white, 4.9166665f);
		}

		// Token: 0x06009BBF RID: 39871 RVA: 0x0038256C File Offset: 0x0038076C
		public override string CompInspectStringExtra()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("StabilizersRemaining".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.stabilizersRemaining);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(3);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04005C95 RID: 23701
		private const int FadeDurationTicks = 300;

		// Token: 0x04005C96 RID: 23702
		public const int StabilizerCount = 3;

		// Token: 0x04005C97 RID: 23703
		private static readonly Vector3 BrainDrawSize = new Vector3(7f, 7f, 7f);

		// Token: 0x04005C98 RID: 23704
		private float ZOffset = 2f;

		// Token: 0x04005C99 RID: 23705
		private float BobHeight = 0.35f;

		// Token: 0x04005C9A RID: 23706
		private const int RaidMTBHours = 48;

		// Token: 0x04005C9B RID: 23707
		private const int RaidMinTimeHours = 24;

		// Token: 0x04005C9C RID: 23708
		private bool defencesLowered;

		// Token: 0x04005C9D RID: 23709
		private bool deactivated;

		// Token: 0x04005C9E RID: 23710
		private Pawn interactedPawn;

		// Token: 0x04005C9F RID: 23711
		private bool scavenging;

		// Token: 0x04005CA0 RID: 23712
		private int closeTick = -99999;

		// Token: 0x04005CA1 RID: 23713
		public int stabilizersRemaining = 3;

		// Token: 0x04005CA2 RID: 23714
		private Lord assemblerLord;

		// Token: 0x04005CA3 RID: 23715
		private int lastRaidTick = -99999;

		// Token: 0x04005CA4 RID: 23716
		[Unsaved(false)]
		private Graphic cachedBrainGraphic;
	}
}
