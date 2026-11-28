using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D87 RID: 7559
	[StaticConstructorOnStartup]
	public class MultiPawnGotoController
	{
		// Token: 0x1700245D RID: 9309
		// (get) Token: 0x0600C3EB RID: 50155 RVA: 0x0047AC87 File Offset: 0x00478E87
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x0600C3EC RID: 50156 RVA: 0x0047AC8F File Offset: 0x00478E8F
		public void Deactivate()
		{
			this.active = false;
			this.pawns.Clear();
			this.dests.Clear();
			this.lastUpdateTicks = null;
		}

		// Token: 0x0600C3ED RID: 50157 RVA: 0x0047ACBC File Offset: 0x00478EBC
		public void StartInteraction(IntVec3 mouseCell)
		{
			this.Deactivate();
			this.end = mouseCell;
			this.start = mouseCell;
			this.lastUpdateTicks = null;
		}

		// Token: 0x0600C3EE RID: 50158 RVA: 0x0047ACEB File Offset: 0x00478EEB
		public void FinalizeInteraction()
		{
			this.RecomputeDestinations();
			this.IssueGotoJobs();
			this.Deactivate();
		}

		// Token: 0x0600C3EF RID: 50159 RVA: 0x0047ACFF File Offset: 0x00478EFF
		public void AddPawn(Pawn pawn)
		{
			this.active = true;
			this.pawns.Add(pawn);
			this.dests.Add(IntVec3.Invalid);
			this.lastUpdateTicks = null;
		}

		// Token: 0x0600C3F0 RID: 50160 RVA: 0x0047AD30 File Offset: 0x00478F30
		public void ProcessInputEvents()
		{
			IntVec3 intVec = UI.MouseCell();
			if (!this.Active || !this.pawns.Any<Pawn>() || !this.pawns[0].Spawned || !intVec.InBounds(this.pawns[0].Map))
			{
				return;
			}
			int ticksGame = Find.TickManager.TicksGame;
			if (!(intVec != this.end) && this.lastUpdateTicks != null)
			{
				float num = (float)ticksGame;
				int? num2 = this.lastUpdateTicks;
				float? num3 = (num2 != null) ? new float?((float)num2.GetValueOrDefault() + 10f) : null;
				if (!(num > num3.GetValueOrDefault() & num3 != null))
				{
					return;
				}
			}
			if (intVec != this.end)
			{
				SoundDefOf.DragGoto.PlayOneShotOnCamera(null);
			}
			this.end = intVec;
			this.lastUpdateTicks = new int?(ticksGame);
			this.RecomputeDestinations();
		}

		// Token: 0x0600C3F1 RID: 50161 RVA: 0x0047AE24 File Offset: 0x00479024
		public void RecomputeDestinations()
		{
			for (int i = 0; i < this.dests.Count; i++)
			{
				this.dests[i] = IntVec3.Invalid;
			}
			float num = (float)((this.pawns.Count > 1) ? (this.pawns.Count - 1) : 1);
			for (int j = 0; j < this.pawns.Count; j++)
			{
				Pawn pawn = this.pawns[j];
				if (pawn.Spawned)
				{
					IntVec3 root;
					if (pawn.Map.exitMapGrid.IsExitCell(this.end))
					{
						root = this.end;
					}
					else
					{
						float d = (float)j / num;
						root = (this.start.ToVector3() + (this.end.ToVector3() - this.start.ToVector3()) * d).ToIntVec3();
					}
					IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(root, pawn, (IntVec3 c) => this.<RecomputeDestinations>g__CanGoTo|27_0(pawn, c), true);
					if (ModsConfig.BiotechActive && pawn.IsColonyMech && !MechanitorUtility.InMechanitorCommandRange(pawn, intVec))
					{
						intVec = IntVec3.Invalid;
					}
					this.dests[j] = intVec;
				}
			}
		}

		// Token: 0x0600C3F2 RID: 50162 RVA: 0x0047AF84 File Offset: 0x00479184
		private void IssueGotoJobs()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Pawn pawn = this.pawns[i];
				IntVec3 gotoLoc = this.dests[i];
				if (gotoLoc.IsValid)
				{
					FloatMenuOptionProvider_DraftedMove.PawnGotoAction(this.end, pawn, gotoLoc);
				}
			}
			SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
			if (this.start == this.end)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.GroupGotoHereDragging, OpportunityType.GoodToKnow);
				return;
			}
			if ((float)this.start.DistanceToSquared(this.end) > 1.9f)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GroupGotoHereDragging, KnowledgeAmount.SpecificInteraction);
			}
		}

		// Token: 0x0600C3F3 RID: 50163 RVA: 0x0047B028 File Offset: 0x00479228
		public void Draw()
		{
			if (!this.active)
			{
				return;
			}
			Vector3 s = new Vector3(1.7f, 1f, 1.7f);
			float num = AltitudeLayer.MetaOverlays.AltitudeFor();
			float addedAltitude = num + 0.03658537f;
			float addedAltitude2 = num - 0.03658537f;
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Pawn pawn = this.pawns[i];
				IntVec3 c = this.dests[i];
				if (c.IsValid && pawn.Spawned && !c.Fogged(pawn.Map))
				{
					Vector3 drawLoc = c.ToVector3ShiftedWithAltitude(num);
					pawn.Drawer.renderer.RenderPawnAt(drawLoc, new Rot4?(Rot4.South), false);
					Vector3 pos = c.ToVector3ShiftedWithAltitude(addedAltitude);
					Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(pos, Quaternion.identity, s), MultiPawnGotoController.GotoCircleMaterial, 0);
				}
			}
			Vector3 a = this.start.ToVector3ShiftedWithAltitude(addedAltitude2);
			Vector3 b = this.end.ToVector3ShiftedWithAltitude(addedAltitude2);
			GenDraw.DrawLineBetween(a, b, MultiPawnGotoController.GotoBetweenLineMaterial, 0.9f);
		}

		// Token: 0x0600C3F4 RID: 50164 RVA: 0x0047B148 File Offset: 0x00479348
		public void OnGUI()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Pawn pawn = this.pawns[i];
				IntVec3 c = this.dests[i];
				if (c.IsValid && pawn.Spawned && !c.Fogged(pawn.Map))
				{
					Rect rect = c.ToUIRect();
					Vector2 pos = new Vector2(rect.center.x, rect.yMax + 5f);
					GenMapUI.DrawPawnLabel(pawn, pos, 0.5f, 9999f, null, GameFont.Tiny, true, true);
				}
			}
		}

		// Token: 0x0600C3F7 RID: 50167 RVA: 0x0047B2B1 File Offset: 0x004794B1
		[CompilerGenerated]
		private bool <RecomputeDestinations>g__CanGoTo|27_0(Pawn pawn, IntVec3 c)
		{
			return !this.dests.Contains(c) && (!ModsConfig.BiotechActive || !pawn.IsColonyMech || MechanitorUtility.InMechanitorCommandRange(pawn, c));
		}

		// Token: 0x04006FF2 RID: 28658
		private const float PawnLabelOffsetY = 5f;

		// Token: 0x04006FF3 RID: 28659
		private const float PawnLabelAlpha = 0.5f;

		// Token: 0x04006FF4 RID: 28660
		private static readonly Color FeedbackColor = GenColor.FromBytes(153, 207, 135, 255);

		// Token: 0x04006FF5 RID: 28661
		private const float GotoCircleScale = 1.7f;

		// Token: 0x04006FF6 RID: 28662
		private const float GotoCircleAlpha = 0.4f;

		// Token: 0x04006FF7 RID: 28663
		private static readonly Color GotoCircleColor = MultiPawnGotoController.FeedbackColor * new Color(1f, 1f, 1f, 0.4f);

		// Token: 0x04006FF8 RID: 28664
		private const string GotoCircleTexPath = "UI/Overlays/Circle75Solid";

		// Token: 0x04006FF9 RID: 28665
		private static readonly Material GotoCircleMaterial = MaterialPool.MatFrom("UI/Overlays/Circle75Solid", ShaderDatabase.Transparent, MultiPawnGotoController.GotoCircleColor);

		// Token: 0x04006FFA RID: 28666
		private const float GotoBetweenLineWidth = 0.9f;

		// Token: 0x04006FFB RID: 28667
		private const float GotoBetweenLineAlpha = 0.18f;

		// Token: 0x04006FFC RID: 28668
		private const string GotoBetweenLineTexPath = "UI/Overlays/ThickLine";

		// Token: 0x04006FFD RID: 28669
		private static readonly Color GotoBetweenLineColor = MultiPawnGotoController.FeedbackColor * new Color(1f, 1f, 1f, 0.18f);

		// Token: 0x04006FFE RID: 28670
		private static readonly Material GotoBetweenLineMaterial = MaterialPool.MatFrom("UI/Overlays/ThickLine", ShaderDatabase.Transparent, MultiPawnGotoController.GotoBetweenLineColor);

		// Token: 0x04006FFF RID: 28671
		private const float RecomputeDestinationsFrequencyTicks = 10f;

		// Token: 0x04007000 RID: 28672
		private bool active;

		// Token: 0x04007001 RID: 28673
		private IntVec3 start;

		// Token: 0x04007002 RID: 28674
		private IntVec3 end;

		// Token: 0x04007003 RID: 28675
		private int? lastUpdateTicks;

		// Token: 0x04007004 RID: 28676
		private readonly List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04007005 RID: 28677
		private readonly List<IntVec3> dests = new List<IntVec3>();
	}
}
