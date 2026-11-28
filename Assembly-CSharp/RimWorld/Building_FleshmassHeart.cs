using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AAE RID: 6830
	public class Building_FleshmassHeart : Building
	{
		// Token: 0x17001EDB RID: 7899
		// (get) Token: 0x0600ACDC RID: 44252 RVA: 0x003CEC9C File Offset: 0x003CCE9C
		private Graphic CenterPartGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.cachedCenterPartGraphic) == null)
				{
					result = (this.cachedCenterPartGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/Fleshmass/FleshmassHeart/FleshmassHeart_CentralPart", ShaderDatabase.Cutout, Building_FleshmassHeart.PartDrawSize, Color.white));
				}
				return result;
			}
		}

		// Token: 0x17001EDC RID: 7900
		// (get) Token: 0x0600ACDD RID: 44253 RVA: 0x003CECDC File Offset: 0x003CCEDC
		private Graphic LeftPartGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.cachedLeftPartGraphic) == null)
				{
					result = (this.cachedLeftPartGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/Fleshmass/FleshmassHeart/FleshmassHeart_LeftPart", ShaderDatabase.Cutout, Building_FleshmassHeart.PartDrawSize, Color.white));
				}
				return result;
			}
		}

		// Token: 0x17001EDD RID: 7901
		// (get) Token: 0x0600ACDE RID: 44254 RVA: 0x003CED1C File Offset: 0x003CCF1C
		private Graphic RightPartGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.cachedRightPartGraphic) == null)
				{
					result = (this.cachedRightPartGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/Fleshmass/FleshmassHeart/FleshmassHeart_RightPart", ShaderDatabase.Cutout, Building_FleshmassHeart.PartDrawSize, Color.white));
				}
				return result;
			}
		}

		// Token: 0x17001EDE RID: 7902
		// (get) Token: 0x0600ACDF RID: 44255 RVA: 0x003CED5C File Offset: 0x003CCF5C
		public CompBiosignatureOwner BiosignatureComp
		{
			get
			{
				CompBiosignatureOwner result;
				if ((result = this.biosignatureCompInt) == null)
				{
					result = (this.biosignatureCompInt = base.GetComp<CompBiosignatureOwner>());
				}
				return result;
			}
		}

		// Token: 0x17001EDF RID: 7903
		// (get) Token: 0x0600ACE0 RID: 44256 RVA: 0x003CED82 File Offset: 0x003CCF82
		public int Biosignature
		{
			get
			{
				return this.BiosignatureComp.biosignature;
			}
		}

		// Token: 0x17001EE0 RID: 7904
		// (get) Token: 0x0600ACE1 RID: 44257 RVA: 0x003CED8F File Offset: 0x003CCF8F
		public object BiosignatureName
		{
			get
			{
				return this.BiosignatureComp.BiosignatureName;
			}
		}

		// Token: 0x17001EE1 RID: 7905
		// (get) Token: 0x0600ACE2 RID: 44258 RVA: 0x003CED9C File Offset: 0x003CCF9C
		public CompFleshmassHeart Comp
		{
			get
			{
				return base.GetComp<CompFleshmassHeart>();
			}
		}

		// Token: 0x17001EE2 RID: 7906
		// (get) Token: 0x0600ACE3 RID: 44259 RVA: 0x003CEDA4 File Offset: 0x003CCFA4
		public Lord DefendHeartLord
		{
			get
			{
				if (this.defendHeartLord == null)
				{
					this.defendHeartLord = LordMaker.MakeNewLord(base.Faction, new LordJob_DefendFleshmassHeart(this), base.Map, null);
				}
				return this.defendHeartLord;
			}
		}

		// Token: 0x0600ACE4 RID: 44260 RVA: 0x003CEDD4 File Offset: 0x003CCFD4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.bpmAccel, "bpmAccel", 0, false);
			Scribe_Values.Look<int>(ref this.bpm, "bpm", 0, false);
			Scribe_References.Look<Lord>(ref this.defendHeartLord, "defendHeartLord", false);
			Scribe_Values.Look<int>(ref this.overloadTick, "overloadTick", 0, false);
		}

		// Token: 0x0600ACE5 RID: 44261 RVA: 0x003CEE30 File Offset: 0x003CD030
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				Find.AnalysisManager.AddAnalysisTask(this.Biosignature, 3);
				if (base.Faction != Faction.OfEntities)
				{
					this.SetFaction(Faction.OfEntities, null);
				}
				EffecterDefOf.ImpactDustCloud.Spawn(base.Position, base.Map, 1f).Cleanup();
			}
		}

		// Token: 0x0600ACE6 RID: 44262 RVA: 0x003CEE9C File Offset: 0x003CD09C
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			IntVec3 positionHeld = base.PositionHeld;
			Map mapHeld = base.MapHeld;
			base.Kill(dinfo, exactCulprit);
			Thing t;
			if (!GenDrop.TryDropSpawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.FleshmassNucleus, Faction.OfEntities, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)), positionHeld, mapHeld, ThingPlaceMode.Near, out t, null, null, true))
			{
				Log.ErrorOnce("Failed to place fleshmass nucleus", 9685694);
				return;
			}
			TaggedString label = "LetterLabelFleshmassHeartDestroyed".Translate();
			TaggedString text = "LetterFleshmassHeartDestroyed".Translate();
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, t, null, null, null, null, 0, true);
		}

		// Token: 0x0600ACE7 RID: 44263 RVA: 0x003CEFB0 File Offset: 0x003CD1B0
		protected override void Tick()
		{
			base.Tick();
			if ((float)Find.TickManager.TicksGame > (float)this.lastBeatTick + 60f / (float)this.bpm * 60f)
			{
				this.Beat();
			}
			Thing.allowDestroyNonDestroyable = true;
			if (this.overloadTick > 0 && Find.TickManager.TicksGame > this.overloadTick)
			{
				this.Kill(null, null);
			}
			Thing.allowDestroyNonDestroyable = false;
		}

		// Token: 0x0600ACE8 RID: 44264 RVA: 0x003CF028 File Offset: 0x003CD228
		private void Beat()
		{
			this.lastBeatTick = Find.TickManager.TicksGame;
			this.bpm += this.bpmAccel;
			SoundDefOf.FleshmassHeart_Throb.PlayOneShot(this);
		}

		// Token: 0x0600ACE9 RID: 44265 RVA: 0x003CF060 File Offset: 0x003CD260
		public void StartTachycardiacOverload()
		{
			this.bpm *= 2;
			this.bpmAccel = 15;
			this.overloadTick = Find.TickManager.TicksGame + EffecterDefOf.TachycardiacArrest.maintainTicks;
			EffecterDefOf.TachycardiacArrest.SpawnMaintained(base.Position, base.Map, 1f);
			Messages.Message("MessageHeartAttack".Translate(), this, MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x0600ACEA RID: 44266 RVA: 0x003CF0DC File Offset: 0x003CD2DC
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			Matrix4x4 matrix = Matrix4x4.TRS(drawLoc + new Vector3(0f, 0.35f, -0.35f), Quaternion.AngleAxis(0f, Vector3.up), new Vector3(this.GetBeatScale(1.15f, 0), 1f, this.GetBeatScale(1.2f, 0)));
			GenDraw.DrawMeshNowOrLater(this.CenterPartGraphic.MeshAt(Rot4.South), matrix, this.CenterPartGraphic.MatSouth, false, null);
			matrix = Matrix4x4.TRS(drawLoc + new Vector3(-0.875f, 0.3f, 0f), Quaternion.AngleAxis(30f, Vector3.up), new Vector3(this.GetBeatScale(1.1f, 3), 1f, this.GetBeatScale(1.1f, 3)));
			GenDraw.DrawMeshNowOrLater(this.LeftPartGraphic.MeshAt(Rot4.South), matrix, this.LeftPartGraphic.MatSouth, false, null);
			matrix = Matrix4x4.TRS(drawLoc + new Vector3(0.875f, 0.3f, 0f), Quaternion.AngleAxis(10f, Vector3.up), new Vector3(this.GetBeatScale(1.1f, 7), 1f, this.GetBeatScale(1.1f, 7)));
			GenDraw.DrawMeshNowOrLater(this.RightPartGraphic.MeshAt(Rot4.South), matrix, this.RightPartGraphic.MatSouth, false, null);
		}

		// Token: 0x0600ACEB RID: 44267 RVA: 0x003CF250 File Offset: 0x003CD450
		private float GetBeatScale(float maxScale, int delay = 0)
		{
			float num = (float)(Find.TickManager.TicksGame - (this.lastBeatTick + delay)) / 15f;
			if (num < 1f)
			{
				return Mathf.Lerp(1f, maxScale, Mathf.Sin(3.1415927f * num));
			}
			return 1f;
		}

		// Token: 0x0600ACEC RID: 44268 RVA: 0x003CF29D File Offset: 0x003CD49D
		public override void PreSwapMap()
		{
			base.PreSwapMap();
			this.cachedLordPawns = new List<Pawn>();
			if (this.defendHeartLord != null)
			{
				this.cachedLordPawns.AddRange(this.defendHeartLord.ownedPawns);
			}
		}

		// Token: 0x0600ACED RID: 44269 RVA: 0x003CF2D0 File Offset: 0x003CD4D0
		public override void PostSwapMap()
		{
			base.PostSwapMap();
			Lord lord = this.defendHeartLord;
			if (lord != null)
			{
				lord.RemovePawns(this.cachedLordPawns);
			}
			this.cachedLordPawns.RemoveAll((Pawn p) => !p.Spawned);
			this.defendHeartLord = null;
			this.DefendHeartLord.AddPawns(this.cachedLordPawns, true);
			this.cachedLordPawns = null;
		}

		// Token: 0x0600ACEE RID: 44270 RVA: 0x003CF348 File Offset: 0x003CD548
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLineIfNotEmpty().AppendLine("FleshmassHeartInvulnerable".Translate());
			stringBuilder.AppendLine(base.GetInspectString());
			stringBuilder.Append((this.Comp.GrowthPoints > 0) ? "FleshmassHeartGrowing".Translate() : "FleshmassHeartCollecting".Translate());
			return stringBuilder.ToString();
		}

		// Token: 0x04006448 RID: 25672
		private const int BeatDurationTicks = 15;

		// Token: 0x04006449 RID: 25673
		private static readonly Vector3 PartDrawSize = new Vector3(3.5f, 3.5f, 3.5f);

		// Token: 0x0400644A RID: 25674
		private const int RequiredAnalysis = 3;

		// Token: 0x0400644B RID: 25675
		private int bpm = 30;

		// Token: 0x0400644C RID: 25676
		private int bpmAccel;

		// Token: 0x0400644D RID: 25677
		private Lord defendHeartLord;

		// Token: 0x0400644E RID: 25678
		private int overloadTick = -99999;

		// Token: 0x0400644F RID: 25679
		[Unsaved(false)]
		private int lastBeatTick = -99999;

		// Token: 0x04006450 RID: 25680
		[Unsaved(false)]
		private Graphic cachedCenterPartGraphic;

		// Token: 0x04006451 RID: 25681
		[Unsaved(false)]
		private Graphic cachedLeftPartGraphic;

		// Token: 0x04006452 RID: 25682
		[Unsaved(false)]
		private Graphic cachedRightPartGraphic;

		// Token: 0x04006453 RID: 25683
		[Unsaved(false)]
		private CompBiosignatureOwner biosignatureCompInt;

		// Token: 0x04006454 RID: 25684
		[Unsaved(false)]
		private List<Pawn> cachedLordPawns;
	}
}
