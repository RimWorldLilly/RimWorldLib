using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D2 RID: 4306
	public class QuestPart_NewColony : QuestPart
	{
		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06006BA5 RID: 27557 RVA: 0x0025C28F File Offset: 0x0025A48F
		// (set) Token: 0x06006BA6 RID: 27558 RVA: 0x0025C296 File Offset: 0x0025A496
		public static bool IsGeneratingNewColony { get; private set; }

		// Token: 0x06006BA7 RID: 27559 RVA: 0x0025C2A0 File Offset: 0x0025A4A0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Find.MainTabsRoot.EscapeCurrentTab(false);
				Find.World.renderer.RegenerateLayersIfDirtyInLongEvent();
				Find.WindowStack.Add(new Dialog_ChooseThingsForNewColony(new Action<List<Thing>>(this.PostThingsSelected), 5, 5, this.maxRelics, 7, delegate()
				{
					if (!this.outSignalCancelled.NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalCancelled, false));
					}
				}));
			}
		}

		// Token: 0x06006BA8 RID: 27560 RVA: 0x0025C314 File Offset: 0x0025A514
		private void PostThingsSelected(List<Thing> allThings)
		{
			Action<PlanetTile> <>9__3;
			Action <>9__4;
			Find.WindowStack.Add(new Screen_ArchonexusSettlementCinematics(delegate()
			{
				CameraJumper.TryJump(CameraJumper.GetWorldTarget(allThings.First((Thing t) => t is Pawn)), CameraJumper.MovementMode.Pan);
			}, delegate()
			{
				Action<PlanetTile> targetChosen;
				if ((targetChosen = <>9__3) == null)
				{
					targetChosen = (<>9__3 = delegate(PlanetTile choseTile)
					{
						this.TileChosen(choseTile, allThings);
					});
				}
				Action noTileChosen;
				if ((noTileChosen = <>9__4) == null)
				{
					noTileChosen = (<>9__4 = delegate()
					{
						if (!this.outSignalCancelled.NullOrEmpty())
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignalCancelled, false));
						}
					});
				}
				MoveColonyUtility.PickNewColonyTile(targetChosen, noTileChosen);
				ScreenFader.StartFade(Color.clear, 2f);
			}));
		}

		// Token: 0x06006BA9 RID: 27561 RVA: 0x0025C35C File Offset: 0x0025A55C
		private void TileChosen(PlanetTile chosenTile, List<Thing> allThings)
		{
			if (ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode)
			{
				this.tmpPawns.Clear();
				for (int i = 0; i < allThings.Count; i++)
				{
					Pawn pawn = allThings[i] as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						this.tmpPawns.Add(pawn);
					}
				}
				Find.WindowStack.Add(new Dialog_ConfigureIdeo(this.tmpPawns, delegate()
				{
					this.InitMoveColony(allThings, chosenTile);
				}, true));
				this.tmpPawns.Clear();
				return;
			}
			this.InitMoveColony(allThings, chosenTile);
		}

		// Token: 0x06006BAA RID: 27562 RVA: 0x0025C424 File Offset: 0x0025A624
		private void InitMoveColony(List<Thing> things, PlanetTile choseTile)
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				Find.MusicManagerPlay.ForceFadeoutAndSilenceFor(120f, 10f, false);
				List<Thing> list = new List<Thing>();
				list.AddRange(MoveColonyUtility.GetStartingThingsForNewColony());
				list.AddRange(things);
				QuestPart_NewColony.IsGeneratingNewColony = true;
				Settlement settlement = MoveColonyUtility.MoveColonyAndReset(choseTile, list, this.otherFaction, this.worldObjectDef);
				QuestPart_NewColony.IsGeneratingNewColony = false;
				CameraJumper.TryJump(MapGenerator.PlayerStartSpot, settlement.Map, CameraJumper.MovementMode.Pan);
				Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
				if (!this.outSignalCompleted.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalCompleted, false));
				}
			}, "GeneratingMap", false, null, true, false, null);
		}

		// Token: 0x06006BAB RID: 27563 RVA: 0x0025C45A File Offset: 0x0025A65A
		public override void Notify_FactionRemoved(Faction f)
		{
			base.Notify_FactionRemoved(f);
			if (this.otherFaction == f)
			{
				this.otherFaction = null;
			}
		}

		// Token: 0x06006BAC RID: 27564 RVA: 0x0025C474 File Offset: 0x0025A674
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.otherFaction, "otherFaction", false);
			Scribe_Values.Look<string>(ref this.outSignalCompleted, "outSignalCompleted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalCancelled, "outSignalCancelled", null, false);
			Scribe_Defs.Look<WorldObjectDef>(ref this.worldObjectDef, "worldObjecctDef");
			Scribe_Values.Look<int>(ref this.maxRelics, "maxRelics", 0, false);
		}

		// Token: 0x04004472 RID: 17522
		public string inSignal;

		// Token: 0x04004473 RID: 17523
		public Faction otherFaction;

		// Token: 0x04004474 RID: 17524
		public string outSignalCompleted;

		// Token: 0x04004475 RID: 17525
		public string outSignalCancelled;

		// Token: 0x04004476 RID: 17526
		public WorldObjectDef worldObjectDef;

		// Token: 0x04004477 RID: 17527
		public int maxRelics = 1;

		// Token: 0x04004478 RID: 17528
		private List<Pawn> tmpPawns = new List<Pawn>();
	}
}
